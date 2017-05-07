using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using PeoplesSource.Models;

namespace PeoplesSource.Helpers
{
    /// <summary>
    /// Class MessageHtmlHelper.
    /// </summary>
    public static class MessageHtmlHelper
    {
        /// <summary>
        /// Gets the messages.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <returns>MvcHtmlString.</returns>
        public static MvcHtmlString GetMessages(this HtmlHelper htmlHelper)
        {
            var messages = htmlHelper.ViewContext.TempData["messages"] as IList<Message>;

            var messagesHtml = new StringBuilder();

            if (messages != null && messages.Any())
            {
                foreach (var message in messages)
                {
                    if(!string.IsNullOrEmpty(message.Text))
                    {
                        var messageDiv = new TagBuilder("div");
                        switch (message.Type)
                        {
                            case Message.MessageType.ERROR:
                                messageDiv.AddCssClass("error");
                                break;
                            case Message.MessageType.NOTICE:
                                messageDiv.AddCssClass("notice");
                                break;
                            case Message.MessageType.INFO:
                                messageDiv.AddCssClass("info");
                                break;
                            case Message.MessageType.SUCCESS:
                                messageDiv.AddCssClass("success");
                                break;
                        }
                        messageDiv.SetInnerText(message.Text);
                        messagesHtml.AppendLine(messageDiv.ToString());
                    }
                }
            }
            return MvcHtmlString.Create(messagesHtml.ToString());
        }
    }
}