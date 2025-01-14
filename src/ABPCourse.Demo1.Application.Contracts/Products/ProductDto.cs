using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ABPCourse.Demo1.Products
{
    public class ProductDto: FullAuditedEntityDto<int>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public int CategoryId { get; set; }
        //public virtual Catogry Category { get; set; }

        public string CategoryNameAr { get; set; }
        public string CategoryNameEn { get; set; }
        public string CategoryDescriptionAr { get; set; }
        public string CategoryDescriptionEn { get; set; }
    }
}
