using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ABPCourse.Demo1.Products
{
    public interface IProductAppService
    {
        public Task<ProductDto>CreateProductAsync(CreateAndUpdateProductDto input);
        public Task<ProductDto> UpdateProductAsync(CreateAndUpdateProductDto input);
        public Task<ProductDto> GetProductAsync(int id);
        public Task <bool> DeleteProductAsync(int id);
        public Task<PagedResultDto<ProductDto>> GetProductListAsync(GetProductListDto input);
        
    }
}
