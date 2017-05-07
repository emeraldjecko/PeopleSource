using PeoplesSource.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeoplesSource.Domain.Services
{
    public class TemplateService : ITemplateService
    {
        public readonly IRepository<Template> _templateRepository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateServiceRepository"></param>
        public TemplateService(IRepository<Template> templateRepository)
        {
            _templateRepository = templateRepository.ThrowIfNull("templateServiceRepository");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        public void SaveTemplates(Template template)
        {
            _templateRepository.Save(template);
        }

        /// <summary>
        /// 
        /// </summary>

        /// <param name="sellerId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<Template> GetTemplates()
        {
            return _templateRepository.GetAll().ToList();

        }

        public List<Template> GetTemplatesBySellerId(int sellerId)
        {
            return _templateRepository.GetAllBy(x => x.seller.Sellerid == sellerId).ToList();

        }

        public void Delete(int Id)
        {
            _templateRepository.Delete(Id);
        }

        public Template GetTemplate(int Id)
        {
            return _templateRepository.GetById(Id);

        }
    }
}
