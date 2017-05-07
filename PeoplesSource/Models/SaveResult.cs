
namespace PeoplesSource.Models
{
    /// <summary>
    /// Class SaveResult.
    /// </summary>
    public class SaveResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SaveResult"/> is success.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        public string MessageText { get; set; }
    }
}