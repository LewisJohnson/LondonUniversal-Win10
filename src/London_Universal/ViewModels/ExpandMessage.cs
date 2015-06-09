using GalaSoft.MvvmLight.Messaging;

namespace London_Universal.ViewModels
{
    class ExpandMessage : GenericMessage<bool>
    {
        public ExpandMessage(object sender, bool expand) : base(sender, expand)
        {
        }
    }
}
