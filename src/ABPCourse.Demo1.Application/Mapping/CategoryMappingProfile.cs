using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPCourse.Demo1.Catagories;
using ABPCourse.Demo1.Categories;
using AutoMapper;

namespace ABPCourse.Demo1.Mapping
{
    public class CategoryMappingProfile:Profile 
    {
        public CategoryMappingProfile()
        {
            CreateMap<Catogry, CategoryDto>();
            CreateMap<CreateAndUpdateCategory, Catogry>();
        }
    }
}
