using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PeoplesSource.Common;
using PeoplesSource.Domain;
using PeoplesSource.Models;

namespace PeoplesSource.Mappers
{
    public class TemplateMapper : BaseMapper, IDomainMapper<TemplateDetail, Template>
    {
        public Template Map(TemplateDetail templateDetail)
        {

            return new Template
            {
                Id = templateDetail.Id,                
                TemplateName = templateDetail.TemplateName,
                TemplateContent = templateDetail.TemplateContent,              
                CreatedDate = templateDetail.CreatedDate,
                seller = templateDetail.SellerId != null ? Session.Load<Seller>(templateDetail.SellerId) : null,

            };
        }


        public void MapToExisting(TemplateDetail templateDetail, Template template)
        {            
            template.TemplateName = templateDetail.TemplateName;
            template.TemplateContent = templateDetail.TemplateContent;
            template.CreatedDate = templateDetail.CreatedDate;
            template.seller = templateDetail.SellerId != null ? Session.Load<Seller>(templateDetail.SellerId) : null;
        }
    }
}