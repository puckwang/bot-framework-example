using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotTemplate.Dialogs
{
    [Serializable]
    public class CardsDemoDialog : IDialog<object>
    {
        private enum LayoutTypeEnum
        {
            single = 1,
            list,
            carousel,
        }

        private enum CardTypeEnum
        {
            HeroCard = 1,
            ReceiptCard,
            ThumbnailCard,
            SigninCard,
            AnimationCard,
            AudioCard,
            VideoCard
        }

        private CardTypeEnum _cardType;

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(AskCardType);

            return Task.CompletedTask;
        }

        private async Task AskCardType(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            PromptDialog.Choice(context, AskLayoutType,
                (IEnumerable<CardTypeEnum>) Enum.GetValues(typeof(CardTypeEnum)), "請選擇卡片類型？");
        }

        private async Task AskLayoutType(IDialogContext context, IAwaitable<CardTypeEnum> result)
        {
            _cardType = await result;
            PromptDialog.Choice(context, AfterMessageReceivedAsync,
                (IEnumerable<LayoutTypeEnum>) Enum.GetValues(typeof(LayoutTypeEnum)), "請選擇排版類型？");
        }

        private async Task AfterMessageReceivedAsync(IDialogContext context, IAwaitable<LayoutTypeEnum> result)
        {
            var layoutTypesType = await result;
            var returnMessage = context.MakeMessage();

            switch (_cardType)
            {
                case CardTypeEnum.HeroCard:
                    returnMessage.Attachments.Add(GetHeroCard());
                    break;
                case CardTypeEnum.ReceiptCard:
                    returnMessage.Attachments.Add(GetReceiptCard());
                    break;
                case CardTypeEnum.ThumbnailCard:
                    returnMessage.Attachments.Add(GetThumbnailCard());
                    break;
                case CardTypeEnum.SigninCard:
                    returnMessage.Attachments.Add(GetSigninCard());
                    break;
                case CardTypeEnum.AnimationCard:
                    returnMessage.Attachments.Add(GetAnimationCard());
                    break;
                case CardTypeEnum.AudioCard:
                    returnMessage.Attachments.Add(GetAudioCard());
                    break;
                case CardTypeEnum.VideoCard:
                    returnMessage.Attachments.Add(GetVideoCard());
                    break;
            }

            returnMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            if (layoutTypesType != LayoutTypeEnum.single)
            {
                returnMessage.AttachmentLayout = layoutTypesType.ToString();
                returnMessage.Attachments.Add(returnMessage.Attachments[0]);
                returnMessage.Attachments.Add(returnMessage.Attachments[0]);
            }

            await context.PostAsync(returnMessage);

            context.Done<object>(null);
        }

        private Attachment GetVideoCard()
        {
            var videoCard = new VideoCard()
            {
                Title = "Splatoon 2 Soundtrack",
                Media = new List<MediaUrl>()
                {
                    new MediaUrl("https://youtu.be/rZaT-PpDAl8")
                },
                Subtitle = "Videmo Demo, by Puck",
                Autoloop = true,
                Autostart = true,
            };

            return videoCard.ToAttachment();
        }

        private Attachment GetAudioCard()
        {
            var audioCard = new AudioCard()
            {
                Title = "卡農 - 帕赫貝爾",
                Subtitle = "Demo, by Puck",
                Media = new List<MediaUrl>()
                {
                    new MediaUrl("https://upload.wikimedia.org/wikipedia/commons/6/62/Pachelbel%27s_Canon.ogg")
                },
                Autoloop = true,
                Autostart = true,
            };

            return audioCard.ToAttachment();
        }

        private Attachment GetAnimationCard()
        {
            var animationCard = new AnimationCard()
            {
                Title = "Thank You!",
                Subtitle = "Demo, by Puck",
                Autoloop = true,
                Autostart = true,
                Media = new List<MediaUrl>()
                {
                    new MediaUrl("https://media.giphy.com/media/osjgQPWRx3cac/giphy.gif")
                }
            };


            return animationCard.ToAttachment();
        }

        private Attachment GetReceiptCard()
        {
            var receiptCard = new ReceiptCard()
            {
                Title = "購物清單",
                Total = "256.0",
                Tax = "1.0",
                Vat = "5.0",
                Items = new List<ReceiptItem>()
                {
                    new ReceiptItem()
                    {
                        Title = "美式咖啡",
                        Subtitle = "1杯",
                        Price = "120.0",
                        Quantity = "1",
                        Image = new CardImage("https://via.placeholder.com/150/e65100/FFFFFF/?text=C")
                    },
                    new ReceiptItem()
                    {
                        Title = "全麥土司",
                        Subtitle = "2條",
                        Price = "65.0",
                        Quantity = "2",
                        Image = new CardImage("https://via.placeholder.com/150/fbc02d/FFFFFF/?text=T")
                    }
                },
                Facts = new List<Fact>()
                {
                    new Fact()
                    {
                        Key = "日期",
                        Value = DateTime.Now.ToShortDateString()
                    },
                    new Fact()
                    {
                        Key = "編號",
                        Value = "AB01234567"
                    },
                    new Fact()
                    {
                        Key = "購買人",
                        Value = "Puck"
                    }
                },
                Buttons = new List<CardAction>()
                {
                    new CardAction()
                    {
                        Type = ActionTypes.ImBack,
                        Title = "確認",
                        Value = "OK"
                    }
                }
            };

            return receiptCard.ToAttachment();
        }

        private Attachment GetSigninCard()
        {
            var signinCard = new SigninCard()
            {
                Text = "請選擇操作: ",
                Buttons = new List<CardAction>()
                {
                    new CardAction()
                    {
                        Type = ActionTypes.ImBack,
                        Title = "新增",
                        Value = "Add"
                    },
                    new CardAction()
                    {
                        Type = ActionTypes.ImBack,
                        Title = "修改",
                        Value = "Edit"
                    },
                    new CardAction()
                    {
                        Type = ActionTypes.ImBack,
                        Title = "刪除",
                        Value = "Delete"
                    }
                }
            };

            return signinCard.ToAttachment();
        }

        private Attachment GetThumbnailCard()
        {
            var thumbnailCard = new ThumbnailCard()
            {
                Title = "Splatoon 2",
                Text = "Happy New Year",
                Subtitle = "by Puck",
                Tap = new CardAction()
                {
                    Type = ActionTypes.OpenUrl,
                    Title = "Tip",
                    Value = "https://imgur.com/hwylGzp.jpg"
                },
                Images = new List<CardImage>()
                {
                    new CardImage("https://imgur.com/hwylGzp.jpg")
                },
                Buttons = new List<CardAction>()
                {
                    new CardAction()
                    {
                        Type = ActionTypes.ImBack,
                        Title = "Like",
                        Value = "Like"
                    }
                }
            };

            return thumbnailCard.ToAttachment();
        }

        private Attachment GetHeroCard()
        {
            var heroCard = new HeroCard()
            {
                Title = "Splatoon 2",
                Text = "Happy New Year.  by Puck",
                Images = new List<CardImage>()
                {
                    new CardImage("https://imgur.com/hwylGzp.jpg")
                },
                Buttons = new List<CardAction>()
                {
                    new CardAction()
                    {
                        Type = ActionTypes.OpenUrl,
                        Title = "Open",
                        Value = "https://imgur.com/hwylGzp.jpg"
                    }
                }
            };

            return heroCard.ToAttachment();
        }
    }
}