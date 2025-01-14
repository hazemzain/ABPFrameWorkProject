using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPCourse.Demo1.Products;
using AutoMapper;

namespace ABPCourse.Demo1.Mapping
{
    public class ProductMappingProfile: Profile
    {
        public ProductMappingProfile()
        {
            CreateMap< CreateAndUpdateProductDto, Product>();
            CreateMap<Product, ProductDto>();
        }
    }
}
