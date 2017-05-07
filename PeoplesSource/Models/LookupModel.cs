namespace PeoplesSource.Models
{
    public class LookupModel : LookupItem
    {
        /// <summary>
        /// Gets or sets the group id.
        /// </summary>
        /// <value>The group id.</value>
        public int Groupid { get; set; }

        public bool Selected { get; set; }
    }
}