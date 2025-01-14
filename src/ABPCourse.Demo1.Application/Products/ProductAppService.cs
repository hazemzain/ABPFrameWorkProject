using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ABPCourse.Demo1.Bases;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace ABPCourse.Demo1.Products
{
    public class ProductAppService : BasesAppService, IProductAppService
    {
        #region Field

        public IRepository<Product, int> _ProductRepository;
        private readonly IObjectMapper _objectMapper;


        #endregion
        #region ctor

        public ProductAppService(IRepository<Product, int> productRepository, IObjectMapper objectMapper)
        {
            _ProductRepository = productRepository ;
            _objectMapper = objectMapper;
        }




        #endregion

        #region Services
        public async Task<ProductDto> CreateProductAsync(CreateAndUpdateProductDto input)
        {
            var inputValidation = new CreateAndUpdateProductValidator().Validate(input);
            if (!inputValidation.IsValid)
            {
                throw new UserFriendlyException(("ValidationErrors"),
                    inputValidation.Errors.Select(e => e.ErrorMessage).JoinAsString(", "));
            }
            var product = _objectMapper.Map<CreateAndUpdateProductDto, Product>(input);
           
            var insert=await _ProductRepository.InsertAsync(product,true);
            return _objectMapper.Map<Product, ProductDto>(insert);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var ExixtProduct=await  _ProductRepository.GetAsync(id);
            if (ExixtProduct == null)
            {
                return false;
            }
            else
            {
                await _ProductRepository.DeleteAsync(ExixtProduct, autoSave: true);
            }
            return true;
        }

        public async Task<ProductDto> GetProductAsync(int id)
        {
            var product =await  _ProductRepository
                .WithDetailsAsync(Product=>Product.Category)
                .Result
                .FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                throw new EntityNotFoundException(typeof(Product), id);
            }
            return _objectMapper.Map<Product, ProductDto>(product);
        }

        public async Task<PagedResultDto<ProductDto>> GetProductListAsync(GetProductListDto input)
        {
            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(Product.Id);
            }
            var products = await _ProductRepository.
                WithDetailsAsync(Product => Product.Category)
                .Result
     
                .AsQueryable()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), Product => Product.NameAr.Contains(input.Filter) || Product.NameEn.Contains(input.Filter))
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderBy(input.Sorting)
                .ToListAsync();

            var totalCount = await _ProductRepository.CountAsync();

            return new PagedResultDto<ProductDto>(
                totalCount,
                _objectMapper.Map<List<Product>, List<ProductDto>>(products)
            );
        }

        public async Task<ProductDto> UpdateProductAsync(CreateAndUpdateProductDto input)
        {
            var inputValidation = new CreateAndUpdateProductValidator().Validate(input);
            if (!inputValidation.IsValid)
            {
                throw new UserFriendlyException(("ValidationErrors"),
                    inputValidation.Errors.Select(e => e.ErrorMessage).JoinAsString(", "));
            }
            var product= await _ProductRepository.GetAsync(input.Id);
            if (product == null)
            {
                throw new UserFriendlyException("Product not found!");
            }
            _objectMapper.Map(input, product);
           var UpdateProduct= await _ProductRepository.UpdateAsync(product, autoSave: true);
           return _objectMapper.Map<Product, ProductDto>(UpdateProduct);
        }


        #endregion

    }
}
