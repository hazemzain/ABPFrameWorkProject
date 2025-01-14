using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ABPCourse.Demo1.Categories
{
    public class CreateAndUpdateCategory:EntityDto<int>
    {
        [Required]
        [MaxLength(300)]
        public string NameAr { get; set; }
        [Required]
        [MaxLength(300)]
        public string NameEn { get; set; }
        [Required]
        [MaxLength(1000)]
        public string DescriptionAr { get; set; }
        [Required]
        [MaxLength(1000)]
        public string DescriptionEn { get; set; }
    }
}
