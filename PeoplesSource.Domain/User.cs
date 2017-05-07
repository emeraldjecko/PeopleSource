using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeoplesSource.Domain
{
    /// <summary>
    /// 
    /// </summary>
   public class User
    {
        /// <summary>
        /// Gets or sets the userId.
        /// </summary>
        /// <value>The userId.</value>
        public virtual Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>The username.</value>
        public virtual string UserName { get; set; }
    }
}
