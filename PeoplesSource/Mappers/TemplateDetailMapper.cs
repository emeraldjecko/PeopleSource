using PeoplesSource.Common;
using PeoplesSource.Domain;
using PeoplesSource.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PeoplesSource.Mappers
{
    public class TemplateDetailMapper : BaseMapper, IMapper<Template, TemplateDetail>
    {
        public TemplateDetail Map(Template template)
        {
            return new TemplateDetail
            {
                Id = template.Id,
                SellerId = template.seller.Sellerid,          
                TemplateContent = template.TemplateContent,         
                TemplateName = template.TemplateName,
                CreatedDate = template.CreatedDate
            };
        }
    }
}