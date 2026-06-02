using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace Apple
{
    public static class iStoreDB
    {
        // Путь к файлу БД — рядом с exe-файлом
        public static readonly string DbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "iStoreDB.sqlite");
        public static readonly string ConnectionString = $"Data Source={DbPath};";

        /// <summary>
        /// Создаёт БД и заполняет тестовыми данными, если её ещё нет.
        /// </summary>
        public static void Initialize()
        {
            if (File.Exists(DbPath))
                return;

            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                -- =============================================
                -- iStore Database (SQLite) - Магазин Apple
                -- =============================================

                -- Поставщики
                CREATE TABLE IF NOT EXISTS Suppliers (
                    SupplierID   INTEGER PRIMARY KEY AUTOINCREMENT,
                    SupplierName TEXT    NOT NULL,
                    ContactName  TEXT,
                    Phone        TEXT,
                    Email        TEXT,
                    Address      TEXT
                );

                -- Категории товаров
                CREATE TABLE IF NOT EXISTS Categories (
                    CategoryID   INTEGER PRIMARY KEY AUTOINCREMENT,
                    CategoryName TEXT    NOT NULL UNIQUE,
                    Description  TEXT
                );

                -- Товары
                CREATE TABLE IF NOT EXISTS Products (
                    ProductID     INTEGER PRIMARY KEY AUTOINCREMENT,
                    ModelName     TEXT    NOT NULL UNIQUE,
                    CategoryID    INTEGER REFERENCES Categories(CategoryID),
                    Description   TEXT,
                    BasePrice     REAL    NOT NULL CHECK (BasePrice > 0),
                    StockQuantity INTEGER NOT NULL DEFAULT 0 CHECK (StockQuantity >= 0)
                );

                -- Закупки
                CREATE TABLE IF NOT EXISTS Purchases (
                    PurchaseID   INTEGER PRIMARY KEY AUTOINCREMENT,
                    ProductID    INTEGER NOT NULL REFERENCES Products(ProductID),
                    SupplierID   INTEGER NOT NULL REFERENCES Suppliers(SupplierID),
                    PurchaseDate TEXT    NOT NULL DEFAULT (datetime('now','localtime')),
                    Quantity     INTEGER NOT NULL CHECK (Quantity > 0),
                    UnitCost     REAL    NOT NULL CHECK (UnitCost > 0),
                    TotalCost    REAL    NOT NULL DEFAULT 0
                );

                -- Продажи
                CREATE TABLE IF NOT EXISTS Sales (
                    SaleID        INTEGER PRIMARY KEY AUTOINCREMENT,
                    ProductID     INTEGER NOT NULL REFERENCES Products(ProductID),
                    SaleDate      TEXT    NOT NULL DEFAULT (datetime('now','localtime')),
                    Quantity      INTEGER NOT NULL CHECK (Quantity > 0),
                    UnitPrice     REAL    NOT NULL CHECK (UnitPrice > 0),
                    TotalPrice    REAL    NOT NULL DEFAULT 0,
                    CustomerName  TEXT,
                    CustomerPhone TEXT
                );

                -- =============================================
                -- ПРЕДСТАВЛЕНИЯ (VIEW)
                -- =============================================
                DROP VIEW IF EXISTS vw_PurchaseReport;
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
                JOIN Products  pr ON p.ProductID  = pr.ProductID
                JOIN Suppliers s  ON p.SupplierID = s.SupplierID;

                DROP VIEW IF EXISTS vw_SalesReport;
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

                -- =============================================
                -- ТРИГГЕРЫ
                -- =============================================

                -- Триггер: пересчёт TotalCost при вставке закупки
                DROP TRIGGER IF EXISTS trg_PurchaseInsert;
                CREATE TRIGGER trg_PurchaseInsert
                AFTER INSERT ON Purchases
                BEGIN
                    UPDATE Purchases
                    SET TotalCost = NEW.Quantity * NEW.UnitCost
                    WHERE PurchaseID = NEW.PurchaseID;

                    UPDATE Products
                    SET StockQuantity = StockQuantity + NEW.Quantity
                    WHERE ProductID = NEW.ProductID;
                END;

                -- Триггер: пересчёт TotalPrice при вставке продажи
                DROP TRIGGER IF EXISTS trg_SaleInsert;
                CREATE TRIGGER trg_SaleInsert
                AFTER INSERT ON Sales
                BEGIN
                    UPDATE Sales
                    SET TotalPrice = NEW.Quantity * NEW.UnitPrice
                    WHERE SaleID = NEW.SaleID;

                    UPDATE Products
                    SET StockQuantity = StockQuantity - NEW.Quantity
                    WHERE ProductID = NEW.ProductID;
                END;

                -- =============================================
                -- ТЕСТОВЫЕ ДАННЫЕ
                -- =============================================

                INSERT INTO Categories (CategoryName, Description) VALUES
                    ('iPhone 15', 'Смартфоны iPhone 15 серии'),
                    ('iPhone 14', 'Смартфоны iPhone 14 серии'),
                    ('iPhone 13', 'Смартфоны iPhone 13 серии');

                INSERT INTO Suppliers (SupplierName, ContactName, Phone, Email, Address) VALUES
                    ('Apple Russia',     'Иванов И.И.', '+7-495-123-4567', 'info@apple.ru',   'Москва, ул. Тверская 1'),
                    ('iStore Distributor','Петров П.П.', '+7-812-765-4321', 'sales@istore.ru', 'СПб, Невский пр. 100');

                INSERT INTO Products (ModelName, CategoryID, Description, BasePrice, StockQuantity) VALUES
                    ('iPhone 15 Pro 256GB',     1, 'Титановый корпус, A17 Pro', 120000.00, 50),
                    ('iPhone 15 Pro Max 512GB', 1, 'Титановый корпус, A17 Pro', 150000.00, 30),
                    ('iPhone 14 128GB',         2, 'Классический iPhone 14',     80000.00, 100),
                    ('iPhone 13 128GB',         3, 'Бюджетный вариант',          60000.00, 75);

                INSERT INTO Purchases (ProductID, SupplierID, PurchaseDate, Quantity, UnitCost) VALUES
                    (1, 1, '2024-01-15', 20, 100000.00),
                    (2, 1, '2024-01-15', 15, 125000.00),
                    (3, 2, '2024-01-20', 50, 65000.00);

                INSERT INTO Sales (ProductID, SaleDate, Quantity, UnitPrice, CustomerName, CustomerPhone) VALUES
                    (1, '2024-02-01', 2, 120000.00, 'Александр Смирнов', '+7-999-111-2233'),
                    (3, '2024-02-05', 1,  80000.00, 'Мария Иванова',     '+7-999-444-5566');
            ";

            cmd.ExecuteNonQuery();
        }
    }
}