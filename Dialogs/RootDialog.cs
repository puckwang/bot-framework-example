using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotTemplate.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private enum MenuEnum
        {
            Hello = 1,
            Operation,
            OperationV2,
            Cards,
            Post
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result;
            var text = activity.Text.ToLower();

            switch (text)
            {
                case "hello":
                    await context.Forward(new AskNameDialog(), AfterMessageReceivedAsync, text, CancellationToken.None);
                    break;
                case "operation": // 計算機範例
                case "計算":
                    await context.Forward(new CalculateDialog(), AfterMessageReceivedAsync, text, CancellationToken.None);
                    break;
                case "operationv2": // 計算機範例- Form Flow 寫法
                case "計算v2":
                    await context.Forward(new CalculateV2Dialog(), AfterMessageReceivedAsync, text, CancellationToken.None);
                    break;
                case "cards": // Card 案例
                    await context.Forward(new CardsDemoDialog(), AfterMessageReceivedAsync, text, CancellationToken.None);
                    break;
                case "post": // Call 外部 API
                    await context.Forward(new PostDialog(), AfterMessageReceivedAsync, text, CancellationToken.None);
                    break;
                case "menu": // Menu
                case "選單":
                case "help":
                case "?":
                    await SendMenu(context);
                    context.Wait(MessageReceivedAsync);
                    break;
                default:
                    int length = (activity.Text ?? string.Empty).Length;
                    string name = context.UserData.GetValueOrDefault("Name", "");
                    await context.PostAsync($"{name + " "}你說了 {activity.Text}，總共有 {length} 個字元");
                    break;
            }
        }

        private async Task AfterMessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceivedAsync);
        }

        private async Task SendMenu(IDialogContext context)
        {
            var returnMessage = context.MakeMessage();
            var heroCard = new HeroCard()
            {
                Title = "功能清單"
            };

            foreach (var item in Enum.GetValues(typeof(MenuEnum)))
            {
                heroCard.Buttons.Add(new CardAction()
                {
                    Title = item.ToString(),
                    Value = item.ToString()
                });
            }

            returnMessage.Attachments.Add(heroCard.ToAttachment());

            await context.PostAsync(returnMessage);
        }
    }
}