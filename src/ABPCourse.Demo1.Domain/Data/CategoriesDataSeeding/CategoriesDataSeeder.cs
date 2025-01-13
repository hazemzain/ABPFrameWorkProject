using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPCourse.Demo1.Catagories;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace ABPCourse.Demo1.Data.CategoriesDataSeeding
{
    public class CategoriesDataSeeder : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Catogry, int> _repository;

        public CategoriesDataSeeder(IRepository<Catogry, int> repository)
        {
            _repository = repository;
        }
        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _repository.GetCountAsync() > 0)
            {
                return;
            }

            await _repository.InsertAsync(new Catogry(1, "التصنيف الأول", "Category One", "الوصف للتصنيف الأول", "Description for Category One"));

            await _repository.InsertAsync(new Catogry(2, "التصنيف الثاني", "Category Two", "الوصف للتصنيف الثاني", "Description for Category Two"));
        }
    }
}
