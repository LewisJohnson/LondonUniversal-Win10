using System;
using GalaSoft.MvvmLight.Messaging;

namespace London_Universal.ViewModels
{
    class IsItemSelectedMessage : MessageBase
    {
        private Action<bool> _callBackAction;

        public IsItemSelectedMessage(object sender, Action<bool> callBackAction ) : base(sender)
        {
            _callBackAction = callBackAction;
        }

        public void Execute(bool result)
        {
            _callBackAction?.Invoke(result);
        }
    }
}
