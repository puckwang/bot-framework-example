using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using RestSharp;

namespace BotTemplate.Dialogs
{
    [Serializable]
    public class AskNameDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(AskName);

            return Task.CompletedTask;
        }

        private async Task AskName(IDialogContext context, IAwaitable<object> result)
        {
            PromptDialog.Text(context, AfterAskName, "請輸入你的名字");
        }

        private async Task AfterAskName(IDialogContext context, IAwaitable<String> result)
        {
            string name = await result;
            context.UserData.SetValue("Name", name);
            await context.PostAsync($"設定名稱為 {name}");
            context.Done<object>(null);
        }
    }
}