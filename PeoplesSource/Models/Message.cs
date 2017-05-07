namespace PeoplesSource.Models
{
    /// <summary>
    /// Message
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Enum MessageType
        /// </summary>
        public enum MessageType { ERROR, NOTICE, INFO, SUCCESS }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public MessageType Type { get; set; }

        /// <summary>
        /// Gets the type text.
        /// </summary>
        /// <value>The type text.</value>
        public string TypeText
        {
            get { return Type.ToString().ToLower(); }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }
    }
}