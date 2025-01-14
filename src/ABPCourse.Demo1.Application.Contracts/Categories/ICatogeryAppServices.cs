using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ABPCourse.Demo1.Categories
{
    public interface ICategoryAppServices: ICrudAppService<CategoryDto, int,PagedAndSortedResultRequestDto, CreateAndUpdateCategory>
    {
        
    }
}
