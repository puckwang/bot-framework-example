using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AdaptiveCards;
using BotTemplate.Cards;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using RestSharp;

namespace BotTemplate.Dialogs
{
    public class Post
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
    }

    [Serializable]
    public class PostDialog : IDialog<object>
    {
        private long _page = 1;

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var text = context.Activity.AsMessageActivity().Text.ToLower();
            switch (text)
            {
                case "next":
                    _page++;
                    break;
                case "previous":
                    _page--;
                    break;
                case "exit":
                    context.Done<object>(null);
                    return;
            }

            var client = new RestClient("https://jsonplaceholder.typicode.com");
            var request = new RestRequest($"/posts?_limit=5&_page={_page}", Method.GET);
            var response = await client.ExecuteTaskAsync<List<Post>>(request);

            var prompt = context.MakeMessage();
            prompt.AttachmentLayout = AttachmentLayoutTypes.List;
            foreach (var item in response.Data)
            {
                prompt.Attachments.Add(PostCard.Build(item.title, item.body, DateTime.Now));
            }

            prompt.Attachments.Add(PostCard.BuildAction());

            await context.PostAsync(prompt);
        }
    }
}