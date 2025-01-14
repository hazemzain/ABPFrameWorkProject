using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPCourse.Demo1.Catagories;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ABPCourse.Demo1.Categories
{
    public class CategoriesAppServices :
            CrudAppService<Catogry, CategoryDto, int, PagedAndSortedResultRequestDto, CreateAndUpdateCategory>, ICategoryAppServices
    {
        public CategoriesAppServices(IRepository<Catogry, int> repository) : base(repository)
        {
        }
    }
}
