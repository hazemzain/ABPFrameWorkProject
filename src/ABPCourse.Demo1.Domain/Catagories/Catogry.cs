using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace ABPCourse.Demo1.Catagories
{
    public class Catogry:FullAuditedEntity<int>
    {
        public Catogry(int id, string NameAr, string NameEn, string DescriptionAr, string DescriptionEn) : base(id)
        {
            
            this.NameAr = NameAr;
            this.NameEn = NameEn;
            this.DescriptionAr = DescriptionAr;
            this.DescriptionEn = DescriptionEn;
            
        }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
    }
}
