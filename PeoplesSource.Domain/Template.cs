using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeoplesSource.Domain
{
    public class Template
    {
        public virtual int Id { get; set; }

        public virtual Seller seller { get; set; }

        public virtual string TemplateName { get; set; }

        public virtual string TemplateContent { get; set; }      

        public virtual DateTime CreatedDate { get; set; }
    }
}
