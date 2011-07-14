using Volta.Core.Infrastructure.Application;

namespace Volta.Web.Handlers
{
    public class MessageModel : ComparableModelBase
    {
        public static MessageModel Information(string message)
        {
            return new MessageModel {MessageText = message};
        }

        public static MessageModel Error(string message)
        {
            return new MessageModel { MessageText = message, IsError = true };
        }

        public bool HasMessageText { get { return !string.IsNullOrEmpty(MessageText); } }
        public string MessageText { get; set; }
        public bool IsError { get; set; }
    }

    public static class MessageModelExtensions
    {
        public static bool HasMessage(this MessageModel message)
        {
            return message != null && message.HasMessageText && !message.IsError;
        }

        public static bool HasError(this MessageModel message)
        {
            return message != null && message.HasMessageText && message.IsError;
        }
    }
}