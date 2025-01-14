using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ABPCourse.Demo1.Catagories;
using ABPCourse.Demo1.Products;
using Allure.NUnit;
using AutoMapper;
using Moq;
using NSubstitute;
using NUnit.Framework;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace ABPCourse.Demo1.Tests
{
    [TestFixture]
    [AllureNUnit]
    public class ProductAppServiceTests
    {
        private const IQueryable<Product>? ProductQuery = (IQueryable<Product>)null;
        private Mock<IRepository<Product, int>> _productRepositoryMock;
        private Mock<IObjectMapper> _MockObjectMapper;
        private ProductAppService _ProductAppService;

        [SetUp]
        public void SetUp()
        {
            _productRepositoryMock = new Mock<IRepository<Product, int>>();
            _MockObjectMapper = new Mock<IObjectMapper>();
            _ProductAppService = new ProductAppService(_productRepositoryMock.Object, _MockObjectMapper.Object);
        }

        [Test]
        public async Task CreateProductAsync_ShouldCreateProduct_WhenInputIsValid()
        {
            var input = new CreateAndUpdateProductDto { Id = 1, NameAr = "تفاح", NameEn = "APPLE", DescriptionAr = "تتتتتتتتتتتتتتتتتفاح", DescriptionEn = "Apppppppppppppppple", CategoryId = 2 };
            var product = new Product { NameAr = "تفاح", NameEn = "APPLE", DescriptionAr = "تتتتتتتتتتتتتتتتتفاح", DescriptionEn = "Apppppppppppppppple", CategoryId = 2 };
            var productdto = new ProductDto { Id = 1, NameAr = "تفاح", NameEn = "APPLE", DescriptionAr = "تتتتتتتتتتتتتتتتتفاح", DescriptionEn = "Apppppppppppppppple", CategoryId = 2 };
            
            _MockObjectMapper
                .Setup(m => m.Map<CreateAndUpdateProductDto, Product>(input))
                .Returns(product);
            // Setup InsertAsync method to return the product
            _productRepositoryMock
                .Setup(repo => repo.InsertAsync(It.IsAny<Product>(), It.Is<bool>(b => b == true),It.Is<CancellationToken>(b=>b== default(CancellationToken))))
                .ReturnsAsync(product);
            


            _MockObjectMapper
                .Setup(m => m.Map<Product, ProductDto>(product))
                .Returns(productdto);

            // Act
            var result = await _ProductAppService.CreateProductAsync(input);

            
            Assert.That(result,Is.Not.Null);
            Assert.That(result.Id,Is.EqualTo(1));
            Assert.That(result.NameEn, Is.EqualTo("APPLE"));
            _MockObjectMapper.Verify(m => m.Map<CreateAndUpdateProductDto, Product>(input), Times.Once);
            _productRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<Product>(), true, default(CancellationToken)), Times.Once);
            _MockObjectMapper.Verify(m => m.Map<Product, ProductDto>(product), Times.Once);

        }

        #region Test_for_Invalid_Input_Validation

        [Test]
        public async Task CreateProductAsync_ShouldThrowException_WhenInputArabicNameIsEmptyAsync()
        {
            var input = new CreateAndUpdateProductDto { Id = 1,  NameEn = "APPLE", DescriptionAr = "تتتتتتتتتتتتتتتتتفاح", DescriptionEn = "Apppppppppppppppple", CategoryId = 2 };
            var exception = Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _ProductAppService.CreateProductAsync(input);
            });
            Assert.That(exception.Code, Is.EqualTo("Arabic name is required."));
            Assert.That(exception.Message, Is.EqualTo("ValidationErrors"));

        }
        [Test]
        public async Task CreateProductAsync_ShouldThrowException_WhenInputEnglishNameIsEmptyAsync()
        {
            var input = new CreateAndUpdateProductDto
            {
                Id = 1, NameAr = "تتتتتتتتتتتتتتتتتفاح", DescriptionEn = "Apppppppppppppppple", CategoryId = 2
            };
            var exception = Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _ProductAppService.CreateProductAsync(input);
            });
            Assert.That(exception.Code, Is.EqualTo("English name is required."));
            Assert.That(exception.Message, Is.EqualTo("ValidationErrors"));
        }
        [Test]
        public async Task CreateProductAsync_ShouldThrowException_WhenInputDescriptionEnIsEmptyAsync()
        {
            var input = new CreateAndUpdateProductDto { Id = 1, NameAr = "تفاح", NameEn = "APPLE", DescriptionAr = "تتتتتتتتتتتتتتتتتفاح", CategoryId = 2 };

            var exception = Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _ProductAppService.CreateProductAsync(input);
            });
            Assert.That(exception.Code, Is.EqualTo("Description is required."));
            Assert.That(exception.Message, Is.EqualTo("ValidationErrors"));
        }
        [Test]
        public async Task CreateProductAsync_ShouldThrowException_WhenInputDescriptionArIsEmptyAsync()
        {
            var input = new CreateAndUpdateProductDto
            {
                Id = 1,
                NameAr = "تفاح",
                NameEn = "APPLE",
                DescriptionEn = "APPLE",
                CategoryId = 2
            };

            var exception = Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _ProductAppService.CreateProductAsync(input);
            });
            Assert.That(exception.Code, Is.EqualTo("Description is required."));
            Assert.That(exception.Message, Is.EqualTo("ValidationErrors"));
        }

        [Test]
        public void CreateProductAsync_ShouldThrowException_WhenCategoryIdIsInvalid()
        {
            var input = new CreateAndUpdateProductDto
            {
                Id = 1,
                NameEn = "APPLE",
                NameAr = "تفاح",
                DescriptionAr = "تتتتتتتتتتتتتتتتتفاح",
                DescriptionEn = "Apppppppppppppppple",
                CategoryId = -2
            };
            var exception = Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _ProductAppService.CreateProductAsync(input);
            });
            Assert.That(exception.Code, Is.EqualTo("Category ID must be greater than 0."));
            Assert.That(exception.Message, Is.EqualTo("ValidationErrors"));
        }

        [Test]
        public void CreateProductAsync_ShouldThrowException_WhenArabicNameExceedsMaxLength()
        {
            var input = new CreateAndUpdateProductDto
            {
                Id = 1, 
                NameAr = new string('أ', 201),
                NameEn = "APPLE", 
                DescriptionAr = "تتتتتتتتتتتتتتتتتفاح", 
                DescriptionEn = "Apppppppppppppppple", 
                CategoryId = 2
            };
            var exception = Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _ProductAppService.CreateProductAsync(input);
            });
            Assert.That(exception.Code, Is.EqualTo("Arabic name must not exceed 200 characters."));
            Assert.That(exception.Message, Is.EqualTo("ValidationErrors"));


        }
        [Test]
        public async Task CreateProductAsync_ShouldThrowException_WhenEnglishNameExceedsMaxLength()
        {
            var input = new CreateAndUpdateProductDto
            {
                Id = 1,
                NameAr = "أبل",
                NameEn = new string('A', 201),
                DescriptionAr = "تتتتتتتتتتتتتتتتتفاح",
                DescriptionEn = "Apppppppppppppppple",
                CategoryId = 2
            };
            var exception = Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _ProductAppService.CreateProductAsync(input);
            });
            Assert.That(exception.Code, Is.EqualTo("English name must not exceed 200 characters."));
            Assert.That(exception.Message, Is.EqualTo("ValidationErrors"));
        }
        [Test]
        public async Task CreateProductAsync_ShouldThrowException_WhenDescriptionEnExceedsMaxLength()
        {
            var input = new CreateAndUpdateProductDto
            {
                Id = 1,
                NameAr = "أبل",
                NameEn = "Apple",
                DescriptionAr = "تتتتتتتتتتتتتتتتتفاح",
                DescriptionEn = new string('A', 1001),
                CategoryId = 2
            };
            var exception = Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _ProductAppService.CreateProductAsync(input);
            });
            Assert.That(exception.Code, Is.EqualTo("English description must not exceed 1000 characters."));
            Assert.That(exception.Message, Is.EqualTo("ValidationErrors"));
        }
        [Test]
        public async Task CreateProductAsync_ShouldThrowException_WhenDescriptionArExceedsMaxLength()
        {
            var input = new CreateAndUpdateProductDto
            {
                Id = 1,
                NameAr = "أبل",
                NameEn = "Apple",
                DescriptionAr = new string('ت', 1001),
                DescriptionEn = "Apple",
                CategoryId = 2
            };
            var exception = Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _ProductAppService.CreateProductAsync(input);
            });
            Assert.That(exception.Code, Is.EqualTo("Arabic description must not exceed 1000 characters."));
            Assert.That(exception.Message, Is.EqualTo("ValidationErrors"));
        }






        #endregion

        #region TestCases_For_DeleteProductAsync

        [Test]
        public async Task DeleteProductAsync_ShouldReturnTrue_WhenProductExists()
        {
            var productId = 1;
            var existingProduct = new Product { NameAr = "تفاح", NameEn = "APPLE", DescriptionAr = "تتتتتتتتتتتتتتتتتفاح", DescriptionEn = "Apppppppppppppppple", CategoryId = 2 };

            _productRepositoryMock
                .Setup(repo => repo.GetAsync(productId,true, default(CancellationToken)))
                .ReturnsAsync(existingProduct);
            _productRepositoryMock
                .Setup(repo => repo.DeleteAsync(existingProduct, true, default(CancellationToken)))
                .Returns(Task.CompletedTask);
            var result = await _ProductAppService.DeleteProductAsync(productId);
            Assert.That(result,Is.True);

        }
        [Test]
        public async Task DeleteProductAsync_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            var productId = 1;
            _productRepositoryMock.Setup(repo => repo.GetAsync(productId, true, default(CancellationToken)))
                .ReturnsAsync((Product)null);
            var result = await _ProductAppService.DeleteProductAsync(productId);
            Assert.That(result, Is.False);
        }
        [Test]
        public void DeleteProductAsync_ShouldThrowException_WhenDeleteFails()
        {
            // Arrange
            var productId = 1;
            var existingProduct = new Product { NameAr = "تفاح", NameEn = "APPLE", DescriptionAr = "تتتتتتتتتتتتتتتتتفاح", DescriptionEn = "Apppppppppppppppple", CategoryId = 2 };

            _productRepositoryMock
                .Setup(repo => repo.GetAsync(productId,true,default(CancellationToken)))
                .ReturnsAsync(existingProduct);

            _productRepositoryMock
                .Setup(repo => repo.DeleteAsync(existingProduct, true, default(CancellationToken)))
                .ThrowsAsync(new Exception("Database deletion failed."));

            // Act & Assert
            var exception = Assert.ThrowsAsync<Exception>(async () =>
            {
                await _ProductAppService.DeleteProductAsync(productId);
            });

            Assert.That(exception.Message,Is.EqualTo("Database deletion failed."));
            Assert.That(exception, Is.False);
        }




        #endregion

        #region TestCases_For_GetProductAsync

        [Test]
        public async Task GetProductAsync_ShouldReturnProductDto_WhenProductExists()
        {
            var productId = 1;
            var product = new Product { NameAr = "تفاح", NameEn = "APPLE", DescriptionAr = "تتتتتتتتتتتتتتتتتفاح", DescriptionEn = "Apppppppppppppppple", CategoryId = 2 };
           
            var Catagory = new Catogry(id: 2, NameAr: "فاكهه", NameEn: "Fruit", DescriptionAr: "اااااا",
                DescriptionEn: "ggggggggggggg");
            var productdto = new ProductDto { Id = 1, NameAr = "تفاح", NameEn = "APPLE", DescriptionAr = "تتتتتتتتتتتتتتتتتفاح", DescriptionEn = "Apppppppppppppppple", CategoryId = 2 };
            // Mock the repository to return a list of products, and simulate FirstOrDefaultAsync behavior
            _productRepositoryMock
                .Setup(repo => repo.WithDetailsAsync(It.IsAny<Expression<Func<Product, object>>>()))
                .ReturnsAsync(new List<Product> { product }.AsQueryable()); // Return IQueryable list of products

            // Mock FirstOrDefaultAsync to return the product based on the ID
            _productRepositoryMock
                .Setup(repo => repo.WithDetailsAsync(It.IsAny<Expression<Func<Product, object>>>()))
                .ReturnsAsync(new List<Product> { product }.AsQueryable());

            _MockObjectMapper
                .Setup(mapper => mapper.Map<Product, ProductDto>(product))
                .Returns(productdto);
            
            // Act
            var result = await _ProductAppService.GetProductAsync(productId);

            // Assert
            Assert.That(result,Is.Not.Null);
            Assert.That( result.Id,Is.EqualTo(productId));
            Assert.That(result.NameEn,Is.EqualTo("APPLE"));
           
        }
        [Test]
        public void GetProductAsync_ShouldThrowEntityNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = 1;
            _productRepositoryMock
                .Setup(repo => repo.WithDetailsAsync(It.IsAny<Expression<Func<Product, object>>>()))
                .ReturnsAsync(ProductQuery);
            // Act & Assert
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            {
                await _ProductAppService.GetProductAsync(productId);
            });
            Assert.That(exception.Message, Is.EqualTo($"Entity of type 'Product' with id '{productId}' not found."));
        }



        #endregion

        #region TestCases_For_UpdateProductAsync

        [Test]
        public void UpdateProductAsync_ShouldThrowUserFriendlyException_WhenInputArabicNameIsEmptyAsync()
        {
            // Arrange
            var input = new CreateAndUpdateProductDto { Id = 1, NameEn = "APPLE", DescriptionAr = "تتتتتتتتتتتتتتتتتفاح", DescriptionEn = "Apppppppppppppppple", CategoryId = 2 };
            var exception = Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _ProductAppService.UpdateProductAsync(input);
            });
            Assert.That(exception.Code, Is.EqualTo("Arabic name is required."));
            Assert.That(exception.Message, Is.EqualTo("ValidationErrors"));

           
        }
        [Test]
        public void UpdateProductAsync_ShouldThrowUserFriendlyException_WhenInputEnglishNameIsEmptyAsync()
        {
            // Arrange
            var input = new CreateAndUpdateProductDto
            {
                Id = 1,
                NameAr = "تفاح",
                DescriptionAr = "تتتتتتتتتتتتتتتتتفاح",
                DescriptionEn = "Apppppppppppppppple",
                CategoryId = 2
            };
            var exception = Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _ProductAppService.UpdateProductAsync(input);
            });
            Assert.That(exception.Code, Is.EqualTo("English name is required."));
            Assert.That(exception.Message, Is.EqualTo("ValidationErrors"));
        }
        [Test]
        public async Task UpdateProductAsync_ShouldThrowUserFriendlyException_WhenInputDescriptionEnIsEmptyAsync()
        {
            // Arrange
            var input = new CreateAndUpdateProductDto
            {
                Id = 1,
                NameAr = "تفاح",
                DescriptionAr = "تتتتتتتتتتتتتتتتتفاح",
                NameEn = "Apple",
                CategoryId = 2
            };
            var exception = Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _ProductAppService.UpdateProductAsync(input);
            });
            Assert.That(exception.Code, Is.EqualTo("Description is required."));
            Assert.That(exception.Message, Is.EqualTo("ValidationErrors"));
        }
        [Test]
        public async Task UpdateProductAsync_ShouldThrowUserFriendlyException_WhenInputDescriptionArIsEmptyAsync()
        {
            // Arrange
            var input = new CreateAndUpdateProductDto
            {
                Id = 1,
                NameAr = "تفاح",
                DescriptionEn = "sjshsj",
                DescriptionAr = "",
                NameEn = "Apple",
                CategoryId = 2
            };
            var exception = Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _ProductAppService.UpdateProductAsync(input);
            });
            Assert.That(exception.Code, Is.EqualTo("Description is required."));
            Assert.That(exception.Message, Is.EqualTo("ValidationErrors"));
        }
        [Test]
        public void UpdateProductAsync_ShouldThrowException_WhenCategoryIdIsInvalid()
        {
            var input = new CreateAndUpdateProductDto
            {
                Id = 1,
                NameEn = "APPLE",
                NameAr = "تفاح",
                DescriptionAr = "تتتتتتتتتتتتتتتتتفاح",
                DescriptionEn = "Apppppppppppppppple",
                CategoryId = -2
            };
            var exception = Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _ProductAppService.UpdateProductAsync(input);
            });
            Assert.That(exception.Code, Is.EqualTo("Category ID must be greater than 0."));
            Assert.That(exception.Message, Is.EqualTo("ValidationErrors"));
        }

        [Test]
        public void UpdateProductAsync_ShouldThrowException_WhenArabicNameExceedsMaxLength()
        {
            var input = new CreateAndUpdateProductDto
            {
                Id = 1,
                NameAr = new string('أ', 201),
                NameEn = "APPLE",
                DescriptionAr = "تتتتتتتتتتتتتتتتتفاح",
                DescriptionEn = "Apppppppppppppppple",
                CategoryId = 2
            };
            var exception = Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _ProductAppService.UpdateProductAsync(input);
            });
            Assert.That(exception.Code, Is.EqualTo("Arabic name must not exceed 200 characters."));
            Assert.That(exception.Message, Is.EqualTo("ValidationErrors"));


        }
        [Test]
        public async Task UpdateProductAsync_ShouldThrowException_WhenEnglishNameExceedsMaxLength()
        {
            var input = new CreateAndUpdateProductDto
            {
                Id = 1,
                NameAr = "أبل",
                NameEn = new string('A', 201),
                DescriptionAr = "تتتتتتتتتتتتتتتتتفاح",
                DescriptionEn = "Apppppppppppppppple",
                CategoryId = 2
            };
            var exception = Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _ProductAppService.UpdateProductAsync(input);
            });
            Assert.That(exception.Code, Is.EqualTo("English name must not exceed 200 characters."));
            Assert.That(exception.Message, Is.EqualTo("ValidationErrors"));
        }
        [Test]
        public async Task UpdateProductAsync_ShouldThrowException_WhenDescriptionEnExceedsMaxLength()
        {
            var input = new CreateAndUpdateProductDto
            {
                Id = 1,
                NameAr = "أبل",
                NameEn = "Apple",
                DescriptionAr = "تتتتتتتتتتتتتتتتتفاح",
                DescriptionEn = new string('A', 1001),
                CategoryId = 2
            };
            var exception = Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _ProductAppService.UpdateProductAsync(input);
            });
            Assert.That(exception.Code, Is.EqualTo("English description must not exceed 1000 characters."));
            Assert.That(exception.Message, Is.EqualTo("ValidationErrors"));
        }
        [Test]
        public async Task UpdateProductAsync_ShouldThrowException_WhenDescriptionArExceedsMaxLength()
        {
            var input = new CreateAndUpdateProductDto
            {
                Id = 1,
                NameAr = "أبل",
                NameEn = "Apple",
                DescriptionAr = new string('ت', 1001),
                DescriptionEn = "Apple",
                CategoryId = 2
            };
            var exception = Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _ProductAppService.UpdateProductAsync(input);
            });
            Assert.That(exception.Code, Is.EqualTo("Arabic description must not exceed 1000 characters."));
            Assert.That(exception.Message, Is.EqualTo("ValidationErrors"));
        }
        [Test]
        public async Task UpdateProductAsync_ShouldThrowUserFriendlyException_WhenProductNotFound()
        {
            // Arrange
            var input = new CreateAndUpdateProductDto { Id = 1, NameAr = "تفاح", NameEn = "APPLE", DescriptionAr = "تتتتتتتتتتتتتتتتتفاح", DescriptionEn = "Apppppppppppppppple", CategoryId = 2 };
            var product = new Product { NameAr = "تفاح", NameEn = "APPLE", DescriptionAr = "تتتتتتتتتتتتتتتتتفاح", DescriptionEn = "Apppppppppppppppple", CategoryId = 2 };
            var productdto = new ProductDto { Id = 1, NameAr = "تفاح", NameEn = "APPLE", DescriptionAr = "تتتتتتتتتتتتتتتتتفاح", DescriptionEn = "Apppppppppppppppple", CategoryId = 2 };

            _productRepositoryMock
                .Setup(repo => repo.GetAsync(input.Id,true,default))
                .ReturnsAsync((Product)null); 

            // Act & Assert
            var exception = Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _ProductAppService.UpdateProductAsync(input);
            });

            Assert.That(exception.Message, Is.EqualTo("Product not found!"));
        }


        [Test]
        public async Task UpdateProductAsync_ShouldReturnUpdatedProductDto_WhenProductExists()
        {
            // Arrange
            var input = new CreateAndUpdateProductDto
            {
                Id = 1,
                NameAr = "تفاح",
                NameEn = "APPLE",
                DescriptionAr = "تتتتتتتتتتتتتتتتتفاح",
                DescriptionEn = "Apppppppppppppppple",
                CategoryId = 2
            };

            var existingProduct = new Product
            {
                
                NameAr = "تفاح",
                NameEn = "APPLE",
                DescriptionAr = "تتتتتتتتتتتتتتتتتفاح",
                DescriptionEn = "Apppppppppppppppple",
                CategoryId = 2
            };

            var updatedProduct = new Product
            {
                
                NameAr = "تفاح",
                NameEn = "APPLE",
                DescriptionAr = "تتتتتتتتتتتتتتتتتفاح",
                DescriptionEn = "Updated description", 
                CategoryId = 2
            };

            var productDto = new ProductDto
            {
                Id = 1,
                NameAr = "تفاح",
                NameEn = "APPLE",
                DescriptionAr = "تتتتتتتتتتتتتتتتتفاح",
                DescriptionEn = "Updated description", 
                CategoryId = 2
            };

            _productRepositoryMock
                .Setup(repo => repo.GetAsync(input.Id, true, default)).ReturnsAsync(existingProduct); 

            _MockObjectMapper.Setup(mapper => mapper.Map(input, existingProduct)); 
            _productRepositoryMock
                .Setup(repo => repo.UpdateAsync(existingProduct, true,default))
                .ReturnsAsync(updatedProduct); 

            _MockObjectMapper.Setup(mapper => mapper.Map<Product, ProductDto>(updatedProduct))
                .Returns(productDto); 

            // Act
            var result = await _ProductAppService.UpdateProductAsync(input);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.DescriptionEn, Is.EqualTo("Updated description"));
            Assert.That(result.Id, Is.EqualTo(input.Id));
        }




        #endregion

    }
}
