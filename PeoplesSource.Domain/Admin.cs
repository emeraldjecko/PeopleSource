using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeoplesSource.Domain
{
    public class Admin
    {
        public virtual int AdminId { get; set; }

        public virtual Guid? UserId { get; set; }

        public virtual string UserName { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string Email { get; set; }

        public virtual string Password { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual User CreatedBy { get; set; }

        public virtual DateTime? CreatedDate { get; set; }

        public virtual User UpdatedBy { get; set; }

        public virtual DateTime? UpdatedDate { get; set; }        
    }
}
