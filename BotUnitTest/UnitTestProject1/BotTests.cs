using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Base;
using System.Threading;
using System.Collections.Generic;
using BotTemplate.Dialogs;

namespace UnitTestProject1
{
    [TestClass]
    public class BotTests : DialogTestBase
    {
        [TestMethod]
        public async Task ShouldReturnEcho()
        {
            // Instantiate dialog to test
            IDialog<object> rootDialog = new RootDialog();

            // Create in-memory bot environment
            Func<IDialog<object>> MakeRoot = () => rootDialog;
            using (new FiberTestBase.ResolveMoqAssembly(rootDialog))
            using (var container = Build(Options.MockConnectorFactory | Options.ScopedQueue, rootDialog))
            {
                // Create a message to send to bot
                var toBot = DialogTestBase.MakeTestMessage();
                toBot.From.Id = Guid.NewGuid().ToString();
                toBot.Text = "hi!";

                // Send message and check the answer.
                IMessageActivity toUser = await GetResponse(container, MakeRoot, toBot);

                // Verify the result
                Assert.AreEqual(" 你說了 hi!，總共有 3 個字元", toUser.Text);
            }
        }

        [TestMethod]
        public async Task TestHello()
        {
            // Instantiate dialog to test
            IDialog<object> rootDialog = new RootDialog();

            // Create in-memory bot environment
            Func<IDialog<object>> MakeRoot = () => rootDialog;
            using (new FiberTestBase.ResolveMoqAssembly(rootDialog))
            using (var container = Build(Options.MockConnectorFactory | Options.ScopedQueue, rootDialog))
            {
                // Create a message to send to bot
                var toBot = DialogTestBase.MakeTestMessage();
                toBot.From.Id = Guid.NewGuid().ToString();

                // Act
                toBot.Text = "hello";
                IMessageActivity toUser = await GetResponse(container, MakeRoot, toBot);

                // Assert
                Assert.AreEqual("請輸入你的名字", toUser.Text);

                // Act
                toBot.Text = "Puck";
                toUser = await GetResponse(container, MakeRoot, toBot);

                // Assert
                Assert.AreEqual("設定名稱為 Puck", toUser.Text);
            }
        }

        [TestMethod]
        public async Task TestOperation()
        {
            // Instantiate dialog to test
            IDialog<object> rootDialog = new RootDialog();

            // Create in-memory bot environment
            Func<IDialog<object>> MakeRoot = () => rootDialog;
            using (new FiberTestBase.ResolveMoqAssembly(rootDialog))
            using (var container = Build(Options.MockConnectorFactory | Options.ScopedQueue, rootDialog))
            {
                // Create a message to send to bot
                var toBot = DialogTestBase.MakeTestMessage();
                toBot.From.Id = Guid.NewGuid().ToString();

                // Act
                toBot.Text = "operation";
                IMessageActivity toUser = await GetResponse(container, MakeRoot, toBot);

                // Assert
                Assert.AreEqual("請輸入第一個數字: ", toUser.Text);

                // Act
                toBot.Text = "123";
                toUser = await GetResponse(container, MakeRoot, toBot);

                // Assert
                HeroCard heroCard = (HeroCard) toUser.Attachments[0].Content;
                Assert.AreEqual("請輸入運算子: ", heroCard.Text);
                Assert.AreEqual("Add", heroCard.Buttons[0].Value);
                Assert.AreEqual("Sub", heroCard.Buttons[1].Value);
                Assert.AreEqual("Mul", heroCard.Buttons[2].Value);
                Assert.AreEqual("Div", heroCard.Buttons[3].Value);

                // Act
                toBot.Text = "Add";
                toUser = await GetResponse(container, MakeRoot, toBot);

                // Assert
                Assert.AreEqual("請輸入第二個數字: ", toUser.Text);

                // Act
                toBot.Text = "456";
                toUser = await GetResponse(container, MakeRoot, toBot);

                // Assert
                Assert.AreEqual("123 + 456 = 579", toUser.Text);
            }
        }

        [TestMethod]
        public async Task TestOperationV2()
        {
            // Instantiate dialog to test
            IDialog<object> rootDialog = new RootDialog();

            // Create in-memory bot environment
            Func<IDialog<object>> MakeRoot = () => rootDialog;
            using (new FiberTestBase.ResolveMoqAssembly(rootDialog))
            using (var container = Build(Options.MockConnectorFactory | Options.ScopedQueue, rootDialog))
            {
                // Create a message to send to bot
                var toBot = DialogTestBase.MakeTestMessage();
                toBot.From.Id = Guid.NewGuid().ToString();

                // Act
                toBot.Text = "operation-v2";
                IMessageActivity toUser = await GetResponse(container, MakeRoot, toBot);

                // Assert
                Assert.AreEqual("請輸入第一個數字", toUser.Text);

                // Act
                toBot.Text = "123";
                toUser = await GetResponse(container, MakeRoot, toBot);

                // Assert
                HeroCard heroCard = (HeroCard) toUser.Attachments[0].Content;
                Assert.AreEqual("請輸入運算子 ", heroCard.Text);
                Assert.AreEqual("Add", heroCard.Buttons[0].Value);
                Assert.AreEqual("Sub", heroCard.Buttons[1].Value);
                Assert.AreEqual("Mul", heroCard.Buttons[2].Value);
                Assert.AreEqual("Div", heroCard.Buttons[3].Value);

                // Act
                toBot.Text = "Add";
                toUser = await GetResponse(container, MakeRoot, toBot);

                // Assert
                Assert.AreEqual("請輸入第二個數字", toUser.Text);

                // Act
                toBot.Text = "456";
                toUser = await GetResponse(container, MakeRoot, toBot);

                // Assert
                Assert.AreEqual("123 + 456 = 579", toUser.Text);
            }
        }

        [TestMethod]
        public async Task TestCards()
        {
            // Instantiate dialog to test
            IDialog<object> rootDialog = new RootDialog();

            // Create in-memory bot environment
            Func<IDialog<object>> MakeRoot = () => rootDialog;
            using (new FiberTestBase.ResolveMoqAssembly(rootDialog))
            using (var container = Build(Options.MockConnectorFactory | Options.ScopedQueue, rootDialog))
            {
                // Create a message to send to bot
                var toBot = DialogTestBase.MakeTestMessage();
                toBot.From.Id = Guid.NewGuid().ToString();

                // Act
                toBot.Text = "cards";
                IMessageActivity toUser = await GetResponse(container, MakeRoot, toBot);

                // Assert
                HeroCard heroCard = (HeroCard) toUser.Attachments[0].Content;
                Assert.AreEqual("請選擇卡片類型？", heroCard.Text);
                Assert.AreEqual("HeroCard", heroCard.Buttons[0].Value);
                Assert.AreEqual("ReceiptCard", heroCard.Buttons[1].Value);
                Assert.AreEqual("ThumbnailCard", heroCard.Buttons[2].Value);
                Assert.AreEqual("SigninCard", heroCard.Buttons[3].Value);
                Assert.AreEqual("AnimationCard", heroCard.Buttons[4].Value);
                Assert.AreEqual("AudioCard", heroCard.Buttons[5].Value);
                Assert.AreEqual("VideoCard", heroCard.Buttons[6].Value);

                // Act
                toBot.Text = "HeroCard";
                toUser = await GetResponse(container, MakeRoot, toBot);

                // Assert
                HeroCard heroCard2 = (HeroCard) toUser.Attachments[0].Content;
                Assert.AreEqual("請選擇排版類型？", heroCard2.Text);
                Assert.AreEqual("single", heroCard2.Buttons[0].Value);
                Assert.AreEqual("list", heroCard2.Buttons[1].Value);
                Assert.AreEqual("carousel", heroCard2.Buttons[2].Value);

                // Act
                toBot.Text = "carousel";
                toUser = await GetResponse(container, MakeRoot, toBot);

                // Assert
                HeroCard heroCard3 = (HeroCard) toUser.Attachments[0].Content;
                Assert.AreEqual("carousel", toUser.AttachmentLayout);
                Assert.AreEqual("Splatoon 2", heroCard3.Title);
                Assert.AreEqual("Happy New Year.  by Puck", heroCard3.Text);
                Assert.AreEqual(1, heroCard3.Images.Count);
                Assert.AreEqual("https://imgur.com/hwylGzp.jpg", heroCard3.Images[0].Url);
                Assert.AreEqual("https://imgur.com/hwylGzp.jpg", heroCard3.Buttons[0].Value);
                Assert.AreEqual(1, heroCard3.Buttons.Count);
                Assert.AreEqual("Open", heroCard3.Buttons[0].Title);
                Assert.AreEqual(ActionTypes.OpenUrl, heroCard3.Buttons[0].Type);
            }
        }

        /// <summary>
        /// Send a message to the bot and get repsponse.
        /// </summary>
        public async Task<IMessageActivity> GetResponse(IContainer container, Func<IDialog<object>> makeRoot, IMessageActivity toBot)
        {
            using (var scope = DialogModule.BeginLifetimeScope(container, toBot))
            {
                DialogModule_MakeRoot.Register(scope, makeRoot);

                // act: sending the message
                using (new LocalizedScope(toBot.Locale))
                {
                    var task = scope.Resolve<IPostToBot>();
                    await task.PostAsync(toBot, CancellationToken.None);
                }
                //await Conversation.SendAsync(toBot, makeRoot, CancellationToken.None);
                return scope.Resolve<Queue<IMessageActivity>>().Dequeue();
            }
        }


    }
}
