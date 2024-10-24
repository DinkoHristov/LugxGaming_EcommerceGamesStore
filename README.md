
# **LugxGaming-Ecommerce Games Store**

Welcome to **LugxGaming**, a web-based application developed using ASP.NET Core MVC. This project serves as a gaming platform where users can explore, purchase, and interact with various games. It includes features such as user authentication, shopping cart management, and a dynamic homepage that displays top games and categories.

## **Table of Contents**

- [Project Overview](#project-overview)
- [Features](#features)
- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## **Project Overview**

LugxGaming is an ASP.NET Core MVC application designed for gaming enthusiasts. It allows users to browse games by categories, add games to their shopping cart, and proceed with purchases. The application is built with a modular architecture, utilizing services and dependency injection for efficient management of business logic and data operations.

### **Technologies Used**

- **ASP.NET Core MVC**: For building the web application.
- **Entity Framework Core**: For database operations.
- **Microsoft Identity**: For user authentication and authorization.
- **Bootstrap & jQuery**: For responsive UI components and dynamic interactions.
- **SQL Server**: As the database management system.

## **Features**

- **User Authentication**: Register, login, and manage user accounts securely.
- **Dynamic Homepage**: Displays top games and categories using data fetched from the database.
- **Shopping Cart**: Add games to the cart, view cart contents, and proceed to checkout.
- **Admin Dashboard**: Manage games, categories, and user roles (if applicable).
- **Responsive Design**: A mobile-friendly layout using Bootstrap.

## **Installation**

### **Prerequisites**

- .NET SDK 6.0 or later
- SQL Server or SQL Express
- IDE such as Visual Studio 2022

### **Steps**

1. **Clone the Repository**:
    ```bash
    git clone https://github.com/your-username/LugxGaming.git
    cd LugxGaming
    ```

2. **Install Dependencies**:
    Open the project in Visual Studio and restore NuGet packages.

3. **Set Up the Database**:
    Update the `appsettings.json` with your SQL Server connection string.
    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Your-SQL-Server-Connection-String"
    }
    ```
    Then, apply migrations:
    ```bash
    Update-Database
    ```

4. **Run the Application**:
    Start the application via Visual Studio or using the .NET CLI:
    ```bash
    dotnet run
    ```

## **Configuration**

The application settings are managed via the `appsettings.json` and `appsettings.Development.json` files. Here, you can configure the database connection, logging levels, and other environment-specific settings.

## **Usage**

Once the application is up and running:

- **Homepage**: Browse featured games and categories.
- **User Account**: Register or log in to access your account.
- **Shopping Cart**: Add games to your cart and proceed to checkout.
- **Admin Panel** (if you have admin rights): Manage games, users, and more.

## **Contributing**

Contributions are welcome! Please fork the repository and submit a pull request with your changes. Ensure your code adheres to the existing style and includes appropriate tests.

## **License**

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

