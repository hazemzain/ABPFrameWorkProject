This project is built using the ABP Framework, a powerful and modular application development framework. 
### Key Features:
- **ABP Framework Integration**: Leverages the ABP Framework's modular architecture, dependency injection, and pre-built modules to streamline development.
- **Product Management**: Implements core product-related operations, including creating, updating, deleting, and retrieving products.
- **Domain-Driven Design**: Adopts a domain-driven approach with entities like `Product` and `Category` to represent business concepts.
- **Validation**: Uses ABP's validation system to enforce business rules and ensure data integrity.
- **DTOs and Mapping**: Utilizes Data Transfer Objects (DTOs) for communication between layers and ABP's object mapping for seamless data transformation.
- **Localization**: Supports multi-language fields such as `NameAr`, `NameEn`, `DescriptionAr`, and `DescriptionEn` for Arabic and English language compatibility.

### Technologies Used:
- **ABP Framework**: For modular application development.
- **Entity Framework Core**: For database interactions and persistence.
- **NUnit and Moq**: For unit testing and mocking dependencies.
- **ASP.NET Core**: For building the web application layer.
- **Dependency Injection**: For managing dependencies and improving testability.

### Structure:
- **Application Layer**: Contains application services like `ProductAppService` to handle business logic and expose APIs.
- **Domain Layer**: Defines core entities (`Product`, `Category`) and business rules.
- **Contracts Layer**: Includes DTOs (`CreateAndUpdateProductDto`, `ProductDto`) and interfaces for communication between layers.
- **Test Layer**: Provides unit tests to ensure the correctness of the application.




This project contains unit tests for the `ProductAppService` class, which is part of the `ABPCourse.Demo1` application. The `ProductAppService` is responsible for managing product-related operations, such as creating, updating, and deleting products. The tests ensure the correctness and reliability of the service's functionality.

### Key Features:
- **Unit Testing Framework**: Utilizes NUnit for writing and organizing test cases.
- **Mocking**: Employs Moq to mock dependencies like repositories and object mappers, ensuring isolated and focused tests.
- **Validation Testing**: Includes tests to verify input validation, such as checking for empty or excessively long fields.
- **CRUD Operations**: Covers Create, Read, Update, and Delete operations for products.
- **Domain-Specific Testing**: Validates business rules, such as ensuring valid category IDs and proper field lengths.

### Structure:
- **Test Class**: `ProductAppServiceTests` contains all test cases for the `ProductAppService`.
- **Test Methods**: Each test method targets a specific functionality or edge case, such as:
  - Validating input fields.
  - Ensuring proper exception handling for invalid inputs.
  - Verifying successful updates and deletions of products.

### Technologies Used:
- **NUnit**: For writing and running tests.
- **Moq**: For mocking dependencies.
- **FluentAssertions**: For expressive and readable assertions.

### Example Test:
The `UpdateProductAsync_ShouldReturnUpdatedProductDto_WhenProductExists` test ensures that the `UpdateProductAsync` method correctly updates a product and returns the expected `ProductDto`.

![ABPAluree2](https://github.com/user-attachments/assets/a1f8f310-9b40-4be2-ac4b-507f2ea9482e)
![ABPAluree1](https://github.com/user-attachments/assets/2de118b7-a6f1-4034-b964-79e88bec6b9c)
![CodeCoverage](https://github.com/user-attachments/assets/990d3492-924b-4cf7-b073-95e9074f90ab)
