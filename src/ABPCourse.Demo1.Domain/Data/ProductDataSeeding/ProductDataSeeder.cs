using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPCourse.Demo1.Products;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace ABPCourse.Demo1.Data.ProductDataSeeding
{
    public class ProductDataSeeder : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Product> _productRepository;

        public ProductDataSeeder(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _productRepository.GetCountAsync() > 0)
            {
                return;
            }

            await _productRepository.InsertAsync(new Product
            {
                NameAr = "منتج 1",
                NameEn = "Product 1",
                DescriptionAr = "وصف المنتج 1",
                DescriptionEn = "Description of Product 1",
                CategoryId = 1
            });

            await _productRepository.InsertAsync(new Product
            {
                NameAr = "منتج 2",
                NameEn = "Product 2",
                DescriptionAr = "وصف المنتج 2",
                DescriptionEn = "Description of Product 2",
                CategoryId = 2
            });
        }
    }
}
