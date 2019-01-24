using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Internals.Fibers;
using Newtonsoft.Json;

namespace BotTemplate.Dialogs
{
    [Serializable]
    public class CalculateBDialog : IDialog<object>
    {
        public enum OperatorType
        {
            Add = 1,
            Sub,
            Ｍul,
            Div
        }

        private readonly Dictionary<OperatorType, string> OperatorTypeStringDictionary =
            new Dictionary<OperatorType, string>()
            {
                {OperatorType.Add, "+"},
                {OperatorType.Div, "/"},
                {OperatorType.Sub, "-"},
                {OperatorType.Ｍul, "*"}
            };

        [Serializable]
        public class Operator
        {
            [Describe("第一個數字")] [Prompt("請輸入{&}")] public int FirstNumber;

            [Describe("第二個數字")] [Prompt("請輸入{&}")] public int SecondNumber;

            [Describe("運算子")] [Prompt("請輸入{&} {||}")]
            public OperatorType OperatorType;

            public static IForm<Operator> BuildForm()
            {
                FormBuilder<Operator> fromBuilder = new FormBuilder<Operator>();

                return fromBuilder
                    .AddRemainingFields()
                    .Build();
            }
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(FormFlow);
            return Task.CompletedTask;
        }

        private async Task FormFlow(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(FormDialog.FromForm(Operator.BuildForm, FormOptions.PromptInStart), AfterFormFlow);
        }

        private async Task AfterFormFlow(IDialogContext context, IAwaitable<Operator> result)
        {
            Operator @operator = await result;
            int sol = 0;

            switch (@operator.OperatorType)
            {
                case OperatorType.Add:
                    sol = @operator.FirstNumber + @operator.SecondNumber;
                    break;
                case OperatorType.Sub:
                    sol = @operator.FirstNumber - @operator.SecondNumber;
                    break;
                case OperatorType.Ｍul:
                    sol = @operator.FirstNumber * @operator.SecondNumber;
                    break;
                case OperatorType.Div:
                    sol = @operator.FirstNumber / @operator.SecondNumber;
                    break;
            }

            await context.PostAsync(
                $"{@operator.FirstNumber} {OperatorTypeStringDictionary[@operator.OperatorType]} {@operator.SecondNumber} = {sol} ");
            context.Done<object>(null);
        }
    }
}