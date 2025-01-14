using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ABPCourse.Demo1.Products
{
    public class GetProductListDto: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
