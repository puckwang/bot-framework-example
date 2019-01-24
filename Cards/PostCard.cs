using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Compilation;
using AdaptiveCards;
using Microsoft.Bot.Connector;

namespace BotTemplate.Cards
{
    public class PostCard
    {
        public static Attachment Build(string title, string body, DateTime dateTime)
        {
            AdaptiveCard adaptiveCard = new AdaptiveCard()
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveTextBlock()
                    {
                        Text = title,
                        Color = AdaptiveTextColor.Accent,
                        Size = AdaptiveTextSize.Medium,
                        Weight = AdaptiveTextWeight.Bolder
                    },
                    new AdaptiveTextBlock()
                    {
                        Text = dateTime.ToLocalTime().ToString(CultureInfo.InvariantCulture),
                        Size = AdaptiveTextSize.Small,
                        Spacing = AdaptiveSpacing.Small,
                        Color = AdaptiveTextColor.Dark,
                        HorizontalAlignment = AdaptiveHorizontalAlignment.Left
                    },
                    new AdaptiveTextBlock()
                    {
                        Text = body,
                        Wrap = true
                    }
                },
                Version = "1.0"
            };

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = adaptiveCard
            };

            return attachment;
        }

        public static Attachment BuildAction()
        {
            var herocard = new HeroCard()
            {
                Buttons = new List<CardAction>()
                {
                    new CardAction() {Title = "Previous",Value = "Previous", Type = ActionTypes.ImBack},
                    new CardAction() {Title = "Next", Value = "Next", Type = ActionTypes.ImBack},
                    new CardAction() {Title = "Exit", Value = "Exit", Type = ActionTypes.ImBack}
                }
            };

            return herocard.ToAttachment();
        }
    }
}