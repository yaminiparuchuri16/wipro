
-- Run this script in SSMS (adjust credentials as needed)
CREATE DATABASE EcommerceDB;
GO
USE EcommerceDB;
GO

CREATE TABLE Users (
    Id INT IDENTITY PRIMARY KEY,
    Username NVARCHAR(100) UNIQUE NOT NULL,
    Email NVARCHAR(255) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) DEFAULT 'Customer',
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Products (
    Id INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(500),
    Price DECIMAL(18,2) NOT NULL,
    Stock INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Orders (
    Id INT IDENTITY PRIMARY KEY,
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(Id),
    OrderDate DATETIME DEFAULT GETDATE(),
    Total DECIMAL(18,2) NOT NULL
);

CREATE TABLE OrderItems (
    Id INT IDENTITY PRIMARY KEY,
    OrderId INT NOT NULL FOREIGN KEY REFERENCES Orders(Id),
    ProductId INT NOT NULL FOREIGN KEY REFERENCES Products(Id),
    Quantity INT NOT NULL,
    Price DECIMAL(18,2) NOT NULL
);

-- Sample data
INSERT INTO Users (Username, Email, PasswordHash, Role)
VALUES ('admin', 'admin@demo.com', 'REPLACE_WITH_HASHED_PASSWORD', 'Admin');

INSERT INTO Products (Name, Description, Price, Stock)
VALUES ('Wireless Mouse','Ergonomic mouse',799,50),
       ('Keyboard','Mechanical keyboard',2499,25),
       ('Water Bottle','Steel bottle 1L',499,100);
