using System;
using System.Data;
using System.IO;
using Microsoft.Data.Sqlite;

namespace Apple
{
    public static class DatabaseManager
    {
        public static readonly string DbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "iStoreDB.sqlite");
        public static readonly string ConnectionString = $"Data Source={DbPath}";

        public static void Initialize()
        {
            bool isNew = !File.Exists(DbPath);

            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            var cmd = conn.CreateCommand();

            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Suppliers (
                    SupplierID   INTEGER PRIMARY KEY AUTOINCREMENT,
                    SupplierName TEXT    NOT NULL,
                    ContactName  TEXT,
                    Phone        TEXT,
                    Email        TEXT,
                    Address      TEXT
                );

                CREATE TABLE IF NOT EXISTS Categories (
                    CategoryID   INTEGER PRIMARY KEY AUTOINCREMENT,
                    CategoryName TEXT    NOT NULL UNIQUE,
                    Description  TEXT
                );

                CREATE TABLE IF NOT EXISTS Products (
                    ProductID     INTEGER PRIMARY KEY AUTOINCREMENT,
                    ModelName     TEXT    NOT NULL UNIQUE,
                    CategoryID    INTEGER REFERENCES Categories(CategoryID),
                    Description   TEXT,
                    BasePrice     REAL    NOT NULL CHECK (BasePrice > 0),
                    StockQuantity INTEGER NOT NULL DEFAULT 0 CHECK (StockQuantity >= 0)
                );

                CREATE TABLE IF NOT EXISTS Purchases (
                    PurchaseID   INTEGER PRIMARY KEY AUTOINCREMENT,
                    ProductID    INTEGER NOT NULL REFERENCES Products(ProductID),
                    SupplierID   INTEGER NOT NULL REFERENCES Suppliers(SupplierID),
                    PurchaseDate TEXT    NOT NULL DEFAULT (datetime('now','localtime')),
                    Quantity     INTEGER NOT NULL CHECK (Quantity > 0),
                    UnitCost     REAL    NOT NULL CHECK (UnitCost > 0),
                    TotalCost    REAL    NOT NULL DEFAULT 0
                );

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

                DROP VIEW IF EXISTS vw_PurchaseReport;
                CREATE VIEW vw_PurchaseReport AS
                SELECT p.PurchaseID, pr.ModelName, s.SupplierName, p.PurchaseDate, p.Quantity, p.UnitCost, p.TotalCost
                FROM Purchases p JOIN Products pr ON p.ProductID = pr.ProductID JOIN Suppliers s ON p.SupplierID = s.SupplierID;

                DROP VIEW IF EXISTS vw_SalesReport;
                CREATE VIEW vw_SalesReport AS
                SELECT sa.SaleID, pr.ModelName, sa.SaleDate, sa.Quantity, sa.UnitPrice, sa.TotalPrice, sa.CustomerName, sa.CustomerPhone
                FROM Sales sa JOIN Products pr ON sa.ProductID = pr.ProductID;

                DROP TRIGGER IF EXISTS trg_PurchaseInsert;
                CREATE TRIGGER trg_PurchaseInsert AFTER INSERT ON Purchases
                BEGIN
                    UPDATE Purchases SET TotalCost = NEW.Quantity * NEW.UnitCost WHERE PurchaseID = NEW.PurchaseID;
                    UPDATE Products SET StockQuantity = StockQuantity + NEW.Quantity WHERE ProductID = NEW.ProductID;
                END;

                DROP TRIGGER IF EXISTS trg_SaleInsert;
                CREATE TRIGGER trg_SaleInsert AFTER INSERT ON Sales
                BEGIN
                    UPDATE Sales SET TotalPrice = NEW.Quantity * NEW.UnitPrice WHERE SaleID = NEW.SaleID;
                    UPDATE Products SET StockQuantity = StockQuantity - NEW.Quantity WHERE ProductID = NEW.ProductID;
                END;
            ";
            cmd.ExecuteNonQuery();

            if (isNew) SeedData();
        }

        private static void SeedData()
        {
            ExecuteNonQuery("INSERT INTO Categories (CategoryName, Description) VALUES ('Смартфоны', 'iPhone и другие')");
            ExecuteNonQuery("INSERT INTO Categories (CategoryName, Description) VALUES ('Ноутбуки', 'MacBook')");
            ExecuteNonQuery("INSERT INTO Categories (CategoryName, Description) VALUES ('Аксессуары', 'Чехлы, AirPods')");

            ExecuteNonQuery("INSERT INTO Suppliers (SupplierName, ContactName, Phone, Email, Address) VALUES ('Apple Russia', 'Иванов И.И.', '+7-495-123-4567', 'info@apple.ru', 'Москва')");
            ExecuteNonQuery("INSERT INTO Suppliers (SupplierName, ContactName, Phone, Email, Address) VALUES ('iStore Dist', 'Петров П.П.', '+7-812-765-4321', 'sales@istore.ru', 'СПб')");

            ExecuteNonQuery("INSERT INTO Products (ModelName, CategoryID, Description, BasePrice, StockQuantity) VALUES ('iPhone 15 Pro 256GB', 1, 'Титан', 120000, 50)");
            ExecuteNonQuery("INSERT INTO Products (ModelName, CategoryID, Description, BasePrice, StockQuantity) VALUES ('MacBook Air M2', 2, '8GB RAM', 110000, 20)");
            ExecuteNonQuery("INSERT INTO Products (ModelName, CategoryID, Description, BasePrice, StockQuantity) VALUES ('AirPods Pro 2', 3, 'USB-C', 25000, 100)");
        }

        public static DataTable ExecuteQuery(string sql, params (string name, object value)[] parameters)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            foreach (var (name, value) in parameters) cmd.Parameters.AddWithValue(name, value ?? DBNull.Value);
            var dt = new DataTable();
            using var reader = cmd.ExecuteReader();
            dt.Load(reader);
            return dt;
        }

        public static int ExecuteNonQuery(string sql, params (string name, object value)[] parameters)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            foreach (var (name, value) in parameters) cmd.Parameters.AddWithValue(name, value ?? DBNull.Value);
            return cmd.ExecuteNonQuery();
        }

        public static object ExecuteScalar(string sql, params (string name, object value)[] parameters)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            foreach (var (name, value) in parameters) cmd.Parameters.AddWithValue(name, value ?? DBNull.Value);
            return cmd.ExecuteScalar();
        }
    }
}