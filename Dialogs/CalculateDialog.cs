using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace BotTemplate.Dialogs
{
    [Serializable]
    public class CalculateDialog : IDialog<object>
    {
        private long _a;
        private long _b;
        private OperatorType _operatorType;

        private enum OperatorType
        {
            Add,
            Sub,
            Mul,
            Div
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(EnterA);

            return Task.CompletedTask;
        }

        private async Task EnterA(IDialogContext context, IAwaitable<object> result)
        {
            PromptDialog.Number(context, AfterEnterA, "請輸入第一個數字: ", "輸入的必須是數字");
        }

        private async Task AfterEnterA(IDialogContext context, IAwaitable<long> result)
        {
            _a = await result;

            await EnterOperator(context);
        }

        private async Task EnterOperator(IDialogContext context)
        {
            PromptDialog.Choice(context, AfterEnterOperator, (IEnumerable<OperatorType>) Enum.GetValues(typeof(OperatorType)), "請輸入運算子: ");
        }

        private async Task AfterEnterOperator(IDialogContext context, IAwaitable<OperatorType> result)
        {
            _operatorType = await result;

            await EnterB(context);
        }

        private async Task EnterB(IDialogContext context)
        {
            PromptDialog.Number(context, AfterEnterB, "請輸入第二個數字: ", "輸入的必須是數字");
        }

        private async Task AfterEnterB(IDialogContext context, IAwaitable<long> result)
        {
            _b = await result;

            await Calculate(context);
        }

        private async Task Calculate(IDialogContext context)
        {
            long ans = 0;
            string o = "";
            switch (_operatorType)
            {
                case OperatorType.Add:
                    ans = _a + _b;
                    o = "+";
                    break;
                case OperatorType.Sub:
                    ans = _a - _b;
                    o = "-";
                    break;
                case OperatorType.Mul:
                    ans = _a * _b;
                    o = "*";
                    break;
                case OperatorType.Div:
                    ans = _a / _b;
                    o = "/";
                    break;
            }

            await context.PostAsync($"{_a} {o} {_b} = {ans.ToString()}");
            context.Done<object>(null);
        }
    }
}