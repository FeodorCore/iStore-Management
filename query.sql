-- =============================================
-- iStore Database - Магазин Apple смартфонов
-- =============================================

-- Удаляем БД если она существует (для повторного запуска)
IF DB_ID('iStoreDB') IS NOT NULL
BEGIN
    ALTER DATABASE iStoreDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE iStoreDB;
END
GO

CREATE DATABASE iStoreDB;
GO

USE iStoreDB;
GO

-- =============================================
-- СОЗДАНИЕ ТАБЛИЦ
-- =============================================

-- Поставщики
CREATE TABLE Suppliers (
    SupplierID INT IDENTITY(1,1) PRIMARY KEY,
    SupplierName NVARCHAR(100) NOT NULL,
    ContactName NVARCHAR(100),
    Phone NVARCHAR(20),
    Email NVARCHAR(100),
    Address NVARCHAR(255)
);
GO

-- Категории товаров
CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(255)
);
GO

-- Товары (смартфоны Apple)
CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    ModelName NVARCHAR(100) NOT NULL UNIQUE,
    CategoryID INT REFERENCES Categories(CategoryID),
    Description NVARCHAR(255),
    BasePrice DECIMAL(10,2) NOT NULL CHECK (BasePrice > 0),
    StockQuantity INT NOT NULL DEFAULT 0 CHECK (StockQuantity >= 0)
);
GO

-- Закупки у поставщиков
CREATE TABLE Purchases (
    PurchaseID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT NOT NULL REFERENCES Products(ProductID),
    SupplierID INT NOT NULL REFERENCES Suppliers(SupplierID),
    PurchaseDate DATETIME NOT NULL DEFAULT GETDATE(),
    Quantity INT NOT NULL CHECK (Quantity > 0),
    UnitCost DECIMAL(10,2) NOT NULL CHECK (UnitCost > 0),
    TotalCost AS (Quantity * UnitCost) PERSISTED
);
GO

-- Продажи
CREATE TABLE Sales (
    SaleID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT NOT NULL REFERENCES Products(ProductID),
    SaleDate DATETIME NOT NULL DEFAULT GETDATE(),
    Quantity INT NOT NULL CHECK (Quantity > 0),
    UnitPrice DECIMAL(10,2) NOT NULL CHECK (UnitPrice > 0),
    TotalPrice AS (Quantity * UnitPrice) PERSISTED,
    CustomerName NVARCHAR(100),
    CustomerPhone NVARCHAR(20)
);
GO

-- =============================================
-- СОЗДАНИЕ ПРЕДСТАВЛЕНИЙ (VIEW)
-- =============================================
GO

CREATE VIEW vw_PurchaseReport AS
SELECT 
    p.PurchaseID,
    pr.ModelName,
    s.SupplierName,
    p.PurchaseDate,
    p.Quantity,
    p.UnitCost,
    p.TotalCost
FROM Purchases p
JOIN Products pr ON p.ProductID = pr.ProductID
JOIN Suppliers s ON p.SupplierID = s.SupplierID;
GO

CREATE VIEW vw_SalesReport AS
SELECT 
    sa.SaleID,
    pr.ModelName,
    sa.SaleDate,
    sa.Quantity,
    sa.UnitPrice,
    sa.TotalPrice,
    sa.CustomerName,
    sa.CustomerPhone
FROM Sales sa
JOIN Products pr ON sa.ProductID = pr.ProductID;
GO

-- =============================================
-- СОЗДАНИЕ ТРИГГЕРОВ
-- =============================================
GO

-- Триггер для автоматического обновления остатков при закупке
CREATE TRIGGER trg_UpdateStockOnPurchase
ON Purchases
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Products
    SET StockQuantity = StockQuantity + i.Quantity
    FROM Products p
    JOIN inserted i ON p.ProductID = i.ProductID;
END;
GO

-- Триггер для автоматического обновления остатков при продаже
CREATE TRIGGER trg_UpdateStockOnSale
ON Sales
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Products
    SET StockQuantity = StockQuantity - i.Quantity
    FROM Products p
    JOIN inserted i ON p.ProductID = i.ProductID;
END;
GO

-- =============================================
-- ВСТАВКА ТЕСТОВЫХ ДАННЫХ
-- =============================================

INSERT INTO Categories (CategoryName, Description) VALUES
('iPhone 15', 'Смартфоны iPhone 15 серии'),
('iPhone 14', 'Смартфоны iPhone 14 серии'),
('iPhone 13', 'Смартфоны iPhone 13 серии');
GO

INSERT INTO Suppliers (SupplierName, ContactName, Phone, Email, Address) VALUES
('Apple Russia', 'Иванов И.И.', '+7-495-123-4567', 'info@apple.ru', 'Москва, ул. Тверская 1'),
('iStore Distributor', 'Петров П.П.', '+7-812-765-4321', 'sales@istore.ru', 'СПб, Невский пр. 100');
GO

INSERT INTO Products (ModelName, CategoryID, Description, BasePrice, StockQuantity) VALUES
('iPhone 15 Pro 256GB', 1, 'Титановый корпус, A17 Pro', 120000.00, 50),
('iPhone 15 Pro Max 512GB', 1, 'Титановый корпус, A17 Pro', 150000.00, 30),
('iPhone 14 128GB', 2, 'Классический iPhone 14', 80000.00, 100),
('iPhone 13 128GB', 3, 'Бюджетный вариант', 60000.00, 75);
GO

INSERT INTO Purchases (ProductID, SupplierID, PurchaseDate, Quantity, UnitCost) VALUES
(1, 1, '2024-01-15', 20, 100000.00),
(2, 1, '2024-01-15', 15, 125000.00),
(3, 2, '2024-01-20', 50, 65000.00);
GO

INSERT INTO Sales (ProductID, SaleDate, Quantity, UnitPrice, CustomerName, CustomerPhone) VALUES
(1, '2024-02-01', 2, 120000.00, 'Александр Смирнов', '+7-999-111-2233'),
(3, '2024-02-05', 1, 80000.00, 'Мария Иванова', '+7-999-444-5566');
GO

-- =============================================
-- ПРОВЕРКА РЕЗУЛЬТАТОВ
-- =============================================

SELECT '=== Категории ===' AS Info;
SELECT * FROM Categories;

SELECT '=== Поставщики ===' AS Info;
SELECT * FROM Suppliers;

SELECT '=== Товары ===' AS Info;
SELECT * FROM Products;

SELECT '=== Отчет по закупкам ===' AS Info;
SELECT * FROM vw_PurchaseReport;

SELECT '=== Отчет по продажам ===' AS Info;
SELECT * FROM vw_SalesReport;