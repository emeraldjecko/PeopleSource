using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeoplesSource.Domain.Services
{
    public interface ITemplateService
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sellerDetail"></param>
        void SaveTemplates(Template template);


        Template GetTemplate(int Id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        List<Template> GetTemplates();

        void Delete(int Id);

        List<Template> GetTemplatesBySellerId(int sellerId);
    }
}
