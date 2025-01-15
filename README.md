
### Project Description

This project is built using the **ABP Framework**, a powerful and modular application development framework. It is designed to manage various business operations, including product and payment management, while adhering to best practices such as Domain-Driven Design (DDD) and modular architecture.

---

### Key Modules and Features

#### **1. Product Module**
The Product Module is responsible for managing product-related operations, including creating, updating, deleting, and retrieving products.

**Key Features:**
- **Product Management**: Handles CRUD operations for products.
- **Domain-Driven Design**: Represents business concepts with entities like `Product` and `Category`.
- **Validation**: Enforces business rules using ABP's validation system.
- **Localization**: Supports multi-language fields for Arabic and English compatibility.
- **DTOs and Mapping**: Uses Data Transfer Objects (DTOs) for communication and ABP's object mapping for seamless data transformation.

**Technologies Used:**
- ABP Framework
- Entity Framework Core
- FluentValidation
- NUnit and Moq for testing

**Structure:**
- **Application Layer**: Contains `ProductAppService` for business logic and API exposure.
- **Domain Layer**: Defines core entities (`Product`, `Category`) and business rules.
- **Contracts Layer**: Includes DTOs (`ProductDto`, `CreateAndUpdateProductDto`) and interfaces.
- **Test Layer**: Provides unit tests for the `ProductAppService`.

---

#### **2. Payment Module**
The Payment Module is designed to handle all payment-related operations, ensuring secure and reliable payment processing.

**Key Features:**
- **Payment Management**: Implements operations like creating and validating payments.
- **Domain-Driven Design**: Represents business concepts with the `payment` entity.
- **Validation**: Uses FluentValidation to enforce business rules and ensure data integrity.
- **DTOs and Mapping**: Uses `PaymentDto` for communication and ABP's object mapping for data transformation.
- **Error Handling**: Provides meaningful error messages and handles validation errors gracefully.

**Technologies Used:**
- ABP Framework
- Entity Framework Core
- FluentValidation
- NUnit and Moq for testing

**Structure:**
- **Application Layer**: Contains `PaymentAppServices` for business logic and API exposure.
- **Domain Layer**: Defines the `payment` entity and business rules.
- **Contracts Layer**: Includes DTOs (`PaymentDto`) and interfaces.
- **Test Layer**: Provides unit tests for the `PaymentAppServices`.

---

### Testing and Validation

The project includes comprehensive unit tests for both the Product and Payment modules to ensure correctness and reliability.

**Key Testing Features:**
- **Unit Testing Framework**: Utilizes NUnit for organizing and running test cases.
- **Mocking**: Uses Moq to mock dependencies like repositories and object mappers.
- **Validation Testing**: Verifies input validation and business rules.
- **CRUD Operations**: Tests Create, Read, Update, and Delete operations for products and payments.

**Technologies Used:**
- NUnit
- Moq
- FluentAssertions
-  Test Reporting: Allure
-  Coverelet

---

### Technologies and Frameworks Used

- **ABP Framework**: For modular application development.
- **Entity Framework Core**: For database interactions and persistence.
- **ASP.NET Core**: For building the application layer.
- **FluentValidation**: For defining and enforcing validation rules.
- **Dependency Injection**: For managing dependencies and improving testability.

---

### Project Structure

- **Application Layer**: Contains application services to handle business logic and expose APIs.
- **Domain Layer**: Defines core entities and business rules.
- **Contracts Layer**: Includes DTOs and interfaces for communication between layers.
- **Test Layer**: Provides unit tests to ensure the correctness of the application.




![abpallure50](https://github.com/user-attachments/assets/191116cc-d85f-4e4f-b682-c3201ede63ee)
![AllureAbp50](https://github.com/user-attachments/assets/c6e7775b-216d-4c2b-bb03-7cc63cdbdba8)
