using PeoplesSource.Common;
using PeoplesSource.Domain;
using PeoplesSource.Models;

namespace PeoplesSource.Mappers
{
    public class TemplateListMapper : BaseMapper, IMapper<Template, TemplateLine>
    {
        public TemplateLine Map(Template template)
        {
            return new TemplateLine
            {
                Id = template.Id,
                Seller = template.seller == null ? "" : template.seller.SellarName,
                SellerId = template.seller == null ? 0 : template.seller.Sellerid,
                TemplateContent = template.TemplateContent,
                TemplateName = template.TemplateName,
                CreatedDate = template.CreatedDate
            };
        }

    }
}