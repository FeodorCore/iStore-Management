using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Microsoft.Data.Sqlite;

namespace Apple
{
    /// <summary>
    /// Класс для работы с базой данных SQLite.
    /// </summary>
    public static class DatabaseHelper
    {
        private static readonly string DbFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Apple");
        private static readonly string DbPath = Path.Combine(DbFolder, "apple.db");

        public static string ConnectionString => $"Data Source={DbPath}";

        /// <summary>
        /// Helper: выполняет запрос и возвращает DataTable.
        /// Заменяет SqliteDataAdapter, которого нет в Microsoft.Data.Sqlite.
        /// </summary>
        private static DataTable ExecuteDataTable(SqliteCommand cmd)
        {
            var dt = new DataTable();
            using var reader = cmd.ExecuteReader();
            dt.Load(reader);
            return dt;
        }

        /// <summary>
        /// Инициализация БД: создание папки, таблиц и тестовых данных.
        /// </summary>
        public static void Initialize()
        {
            if (!Directory.Exists(DbFolder))
                Directory.CreateDirectory(DbFolder);

            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            // Включаем внешние ключи
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "PRAGMA foreign_keys = ON;";
                cmd.ExecuteNonQuery();
            }

            // Создаем таблицы
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Categories (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL UNIQUE
                    );

                    CREATE TABLE IF NOT EXISTS Suppliers (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        ContactPerson TEXT,
                        Phone TEXT,
                        Email TEXT,
                        Address TEXT
                    );

                    CREATE TABLE IF NOT EXISTS Customers (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Type TEXT NOT NULL DEFAULT 'Розничный',
                        Phone TEXT,
                        Email TEXT,
                        Address TEXT
                    );

                    CREATE TABLE IF NOT EXISTS Products (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        CategoryId INTEGER,
                        PurchasePrice REAL NOT NULL DEFAULT 0,
                        SalePrice REAL NOT NULL DEFAULT 0,
                        StockQuantity INTEGER NOT NULL DEFAULT 0,
                        FOREIGN KEY (CategoryId) REFERENCES Categories(Id) ON DELETE SET NULL
                    );

                    CREATE TABLE IF NOT EXISTS Purchases (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        ProductId INTEGER NOT NULL,
                        SupplierId INTEGER NOT NULL,
                        Quantity INTEGER NOT NULL,
                        PurchasePrice REAL NOT NULL,
                        PurchaseDate TEXT NOT NULL,
                        FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE,
                        FOREIGN KEY (SupplierId) REFERENCES Suppliers(Id) ON DELETE CASCADE
                    );

                    CREATE TABLE IF NOT EXISTS Sales (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        CustomerId INTEGER,
                        SaleDate TEXT NOT NULL,
                        Status TEXT NOT NULL DEFAULT 'Завершена',
                        TotalAmount REAL NOT NULL DEFAULT 0,
                        FOREIGN KEY (CustomerId) REFERENCES Customers(Id) ON DELETE SET NULL
                    );

                    CREATE TABLE IF NOT EXISTS SaleItems (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        SaleId INTEGER NOT NULL,
                        ProductId INTEGER NOT NULL,
                        Quantity INTEGER NOT NULL,
                        Price REAL NOT NULL,
                        FOREIGN KEY (SaleId) REFERENCES Sales(Id) ON DELETE CASCADE,
                        FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE
                    );
                ";
                cmd.ExecuteNonQuery();
            }

            SeedDataIfEmpty(connection);
        }

        private static void SeedDataIfEmpty(SqliteConnection connection)
        {
            using var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = "SELECT COUNT(*) FROM Categories;";
            long count = (long)checkCmd.ExecuteScalar();
            if (count > 0) return;

            Execute(connection, "INSERT INTO Categories (Name) VALUES ('Смартфоны'), ('Ноутбуки'), ('Аксессуары');");

            Execute(connection, @"INSERT INTO Suppliers (Name, ContactPerson, Phone, Email, Address) VALUES 
                ('ТехноОпт', 'Иванов И.И.', '+7-900-111-22-33', 'info@technoopt.ru', 'г. Москва, ул. Складская 1'),
                ('МегаСнаб', 'Петров П.П.', '+7-900-222-33-44', 'mega@snab.ru', 'г. Санкт-Петербург, пр. Невский 50');");

            Execute(connection, @"INSERT INTO Customers (Name, Type, Phone, Email, Address) VALUES 
                ('ООО Ромашка', 'Оптовый', '+7-900-333-44-55', 'info@romashka.ru', 'г. Казань'),
                ('Сидоров А.В.', 'Розничный', '+7-900-444-55-66', 'sidorov@mail.ru', 'г. Москва');");

            Execute(connection, @"INSERT INTO Products (Name, CategoryId, PurchasePrice, SalePrice, StockQuantity) VALUES 
                ('iPhone 15 Pro', 1, 75000, 95000, 10),
                ('Samsung Galaxy S24', 1, 55000, 75000, 15),
                ('MacBook Air M2', 2, 85000, 110000, 5),
                ('ASUS VivoBook', 2, 40000, 55000, 8),
                ('Чехол iPhone', 3, 150, 500, 100),
                ('Наушники AirPods', 3, 8000, 15000, 30);");
        }

        private static void Execute(SqliteConnection connection, string sql)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        // ===================== CATEGORIES =====================

        public static DataTable GetCategories(string search = "")
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();

            if (!string.IsNullOrWhiteSpace(search))
            {
                cmd.CommandText = "SELECT Id AS 'ID', Name AS 'Название' FROM Categories WHERE Name LIKE @search ORDER BY Id;";
                cmd.Parameters.AddWithValue("@search", $"%{search}%");
            }
            else
            {
                cmd.CommandText = "SELECT Id AS 'ID', Name AS 'Название' FROM Categories ORDER BY Id;";
            }

            return ExecuteDataTable(cmd);
        }

        public static DataTable GetCategoriesForCombo()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Id, Name FROM Categories ORDER BY Name;";
            return ExecuteDataTable(cmd);
        }

        public static void AddCategory(string name)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Categories (Name) VALUES (@name);";
            cmd.Parameters.AddWithValue("@name", name);
            cmd.ExecuteNonQuery();
        }

        public static void UpdateCategory(int id, string name)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE Categories SET Name = @name WHERE Id = @id;";
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public static void DeleteCategory(int id)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Categories WHERE Id = @id;";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        // ===================== SUPPLIERS =====================

        public static DataTable GetSuppliers(string search = "")
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();

            if (!string.IsNullOrWhiteSpace(search))
            {
                cmd.CommandText = @"SELECT Id AS 'ID', Name AS 'Название', ContactPerson AS 'Контактное лицо', 
                    Phone AS 'Телефон', Email AS 'Email', Address AS 'Адрес' 
                    FROM Suppliers WHERE Name LIKE @search OR ContactPerson LIKE @search ORDER BY Id;";
                cmd.Parameters.AddWithValue("@search", $"%{search}%");
            }
            else
            {
                cmd.CommandText = @"SELECT Id AS 'ID', Name AS 'Название', ContactPerson AS 'Контактное лицо', 
                    Phone AS 'Телефон', Email AS 'Email', Address AS 'Адрес' FROM Suppliers ORDER BY Id;";
            }

            return ExecuteDataTable(cmd);
        }

        public static DataTable GetSuppliersForCombo()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Id, Name FROM Suppliers ORDER BY Name;";
            return ExecuteDataTable(cmd);
        }

        public static void AddSupplier(string name, string contactPerson, string phone, string email, string address)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Suppliers (Name, ContactPerson, Phone, Email, Address) 
                VALUES (@name, @contact, @phone, @email, @address);";
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@contact", (object?)contactPerson ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@phone", (object?)phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@email", (object?)email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@address", (object?)address ?? DBNull.Value);
            cmd.ExecuteNonQuery();
        }

        public static void UpdateSupplier(int id, string name, string contactPerson, string phone, string email, string address)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"UPDATE Suppliers SET Name=@name, ContactPerson=@contact, Phone=@phone, 
                Email=@email, Address=@address WHERE Id=@id;";
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@contact", (object?)contactPerson ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@phone", (object?)phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@email", (object?)email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@address", (object?)address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public static void DeleteSupplier(int id)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Suppliers WHERE Id = @id;";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        // ===================== CUSTOMERS =====================

        public static DataTable GetCustomers(string search = "", string type = "")
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();

            string sql = @"SELECT Id AS 'ID', Name AS 'Имя', Type AS 'Тип', Phone AS 'Телефон', 
                Email AS 'Email', Address AS 'Адрес' FROM Customers WHERE 1=1";

            if (!string.IsNullOrWhiteSpace(search))
                sql += " AND (Name LIKE @search OR Phone LIKE @search)";
            if (!string.IsNullOrWhiteSpace(type) && type != "Все")
                sql += " AND Type = @type";

            sql += " ORDER BY Id;";
            cmd.CommandText = sql;

            if (!string.IsNullOrWhiteSpace(search))
                cmd.Parameters.AddWithValue("@search", $"%{search}%");
            if (!string.IsNullOrWhiteSpace(type) && type != "Все")
                cmd.Parameters.AddWithValue("@type", type);

            return ExecuteDataTable(cmd);
        }

        public static DataTable GetCustomersForCombo()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Id, Name FROM Customers ORDER BY Name;";
            return ExecuteDataTable(cmd);
        }

        public static void AddCustomer(string name, string type, string phone, string email, string address)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Customers (Name, Type, Phone, Email, Address) 
                VALUES (@name, @type, @phone, @email, @address);";
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@type", type);
            cmd.Parameters.AddWithValue("@phone", (object?)phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@email", (object?)email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@address", (object?)address ?? DBNull.Value);
            cmd.ExecuteNonQuery();
        }

        public static void UpdateCustomer(int id, string name, string type, string phone, string email, string address)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"UPDATE Customers SET Name=@name, Type=@type, Phone=@phone, 
                Email=@email, Address=@address WHERE Id=@id;";
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@type", type);
            cmd.Parameters.AddWithValue("@phone", (object?)phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@email", (object?)email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@address", (object?)address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public static void DeleteCustomer(int id)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Customers WHERE Id = @id;";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        // ===================== PRODUCTS =====================

        public static DataTable GetProducts(string search = "", decimal? minPrice = null, decimal? maxPrice = null, int? categoryId = null)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();

            string sql = @"SELECT p.Id AS 'ID', p.Name AS 'Название', c.Name AS 'Категория', 
                p.PurchasePrice AS 'Закуп. цена', p.SalePrice AS 'Цена продажи', p.StockQuantity AS 'Остаток' 
                FROM Products p LEFT JOIN Categories c ON p.CategoryId = c.Id WHERE 1=1";

            if (!string.IsNullOrWhiteSpace(search))
                sql += " AND p.Name LIKE @search";
            if (minPrice.HasValue)
                sql += " AND p.SalePrice >= @minPrice";
            if (maxPrice.HasValue)
                sql += " AND p.SalePrice <= @maxPrice";
            if (categoryId.HasValue && categoryId.Value > 0)
                sql += " AND p.CategoryId = @categoryId";

            sql += " ORDER BY p.Id;";
            cmd.CommandText = sql;

            if (!string.IsNullOrWhiteSpace(search))
                cmd.Parameters.AddWithValue("@search", $"%{search}%");
            if (minPrice.HasValue)
                cmd.Parameters.AddWithValue("@minPrice", (double)minPrice.Value);
            if (maxPrice.HasValue)
                cmd.Parameters.AddWithValue("@maxPrice", (double)maxPrice.Value);
            if (categoryId.HasValue && categoryId.Value > 0)
                cmd.Parameters.AddWithValue("@categoryId", categoryId.Value);

            return ExecuteDataTable(cmd);
        }

        public static DataTable GetProductsForCombo()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Id, Name || ' (' || StockQuantity || ' шт.)' AS DisplayName, SalePrice, StockQuantity FROM Products WHERE StockQuantity > 0 ORDER BY Name;";
            return ExecuteDataTable(cmd);
        }

        public static DataTable GetAllProductsForCombo()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Id, Name || ' (остаток: ' || StockQuantity || ' шт.)' AS DisplayName, SalePrice, StockQuantity FROM Products ORDER BY Name;";
            return ExecuteDataTable(cmd);
        }

        public static void AddProduct(string name, int? categoryId, decimal purchasePrice, decimal salePrice, int stockQuantity)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Products (Name, CategoryId, PurchasePrice, SalePrice, StockQuantity) 
                VALUES (@name, @categoryId, @purchasePrice, @salePrice, @stock);";
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@categoryId", categoryId.HasValue ? (object)categoryId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@purchasePrice", (double)purchasePrice);
            cmd.Parameters.AddWithValue("@salePrice", (double)salePrice);
            cmd.Parameters.AddWithValue("@stock", stockQuantity);
            cmd.ExecuteNonQuery();
        }

        public static void UpdateProduct(int id, string name, int? categoryId, decimal purchasePrice, decimal salePrice, int stockQuantity)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"UPDATE Products SET Name=@name, CategoryId=@categoryId, PurchasePrice=@purchasePrice, 
                SalePrice=@salePrice, StockQuantity=@stock WHERE Id=@id;";
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@categoryId", categoryId.HasValue ? (object)categoryId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@purchasePrice", (double)purchasePrice);
            cmd.Parameters.AddWithValue("@salePrice", (double)salePrice);
            cmd.Parameters.AddWithValue("@stock", stockQuantity);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public static void DeleteProduct(int id)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Products WHERE Id = @id;";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        // ===================== PURCHASES =====================

        public static DataTable GetPurchases(string search = "", int? supplierId = null, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();

            string sql = @"SELECT pu.Id AS 'ID', p.Name AS 'Товар', s.Name AS 'Поставщик', 
                pu.Quantity AS 'Количество', pu.PurchasePrice AS 'Цена', 
                (pu.Quantity * pu.PurchasePrice) AS 'Сумма',
                pu.PurchaseDate AS 'Дата'
                FROM Purchases pu 
                JOIN Products p ON pu.ProductId = p.Id 
                JOIN Suppliers s ON pu.SupplierId = s.Id WHERE 1=1";

            if (!string.IsNullOrWhiteSpace(search))
                sql += " AND (p.Name LIKE @search OR s.Name LIKE @search)";
            if (supplierId.HasValue && supplierId.Value > 0)
                sql += " AND pu.SupplierId = @supplierId";
            if (dateFrom.HasValue)
                sql += " AND date(pu.PurchaseDate) >= date(@dateFrom)";
            if (dateTo.HasValue)
                sql += " AND date(pu.PurchaseDate) <= date(@dateTo)";

            sql += " ORDER BY pu.Id DESC;";
            cmd.CommandText = sql;

            if (!string.IsNullOrWhiteSpace(search))
                cmd.Parameters.AddWithValue("@search", $"%{search}%");
            if (supplierId.HasValue && supplierId.Value > 0)
                cmd.Parameters.AddWithValue("@supplierId", supplierId.Value);
            if (dateFrom.HasValue)
                cmd.Parameters.AddWithValue("@dateFrom", dateFrom.Value.ToString("yyyy-MM-dd"));
            if (dateTo.HasValue)
                cmd.Parameters.AddWithValue("@dateTo", dateTo.Value.ToString("yyyy-MM-dd"));

            return ExecuteDataTable(cmd);
        }

        public static void AddPurchase(int productId, int supplierId, int quantity, decimal purchasePrice, DateTime purchaseDate)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                using var cmd = connection.CreateCommand();
                cmd.Transaction = transaction;
                cmd.CommandText = @"INSERT INTO Purchases (ProductId, SupplierId, Quantity, PurchasePrice, PurchaseDate) 
                    VALUES (@productId, @supplierId, @quantity, @price, @date);";
                cmd.Parameters.AddWithValue("@productId", productId);
                cmd.Parameters.AddWithValue("@supplierId", supplierId);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@price", (double)purchasePrice);
                cmd.Parameters.AddWithValue("@date", purchaseDate.ToString("yyyy-MM-dd"));
                cmd.ExecuteNonQuery();

                using var cmd2 = connection.CreateCommand();
                cmd2.Transaction = transaction;
                cmd2.CommandText = "UPDATE Products SET StockQuantity = StockQuantity + @quantity WHERE Id = @id;";
                cmd2.Parameters.AddWithValue("@quantity", quantity);
                cmd2.Parameters.AddWithValue("@id", productId);
                cmd2.ExecuteNonQuery();

                using var cmd3 = connection.CreateCommand();
                cmd3.Transaction = transaction;
                cmd3.CommandText = "UPDATE Products SET PurchasePrice = @price WHERE Id = @id;";
                cmd3.Parameters.AddWithValue("@price", (double)purchasePrice);
                cmd3.Parameters.AddWithValue("@id", productId);
                cmd3.ExecuteNonQuery();

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public static void DeletePurchase(int id)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = "SELECT ProductId, Quantity FROM Purchases WHERE Id = @id;";
            selectCmd.Parameters.AddWithValue("@id", id);

            int productId = 0;
            int quantity = 0;
            using (var reader = selectCmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    productId = reader.GetInt32(0);
                    quantity = reader.GetInt32(1);
                }
                else
                {
                    return;
                }
            }

            using var transaction = connection.BeginTransaction();

            using var cmd1 = connection.CreateCommand();
            cmd1.Transaction = transaction;
            cmd1.CommandText = "UPDATE Products SET StockQuantity = MAX(0, StockQuantity - @quantity) WHERE Id = @id;";
            cmd1.Parameters.AddWithValue("@quantity", quantity);
            cmd1.Parameters.AddWithValue("@id", productId);
            cmd1.ExecuteNonQuery();

            using var cmd2 = connection.CreateCommand();
            cmd2.Transaction = transaction;
            cmd2.CommandText = "DELETE FROM Purchases WHERE Id = @id;";
            cmd2.Parameters.AddWithValue("@id", id);
            cmd2.ExecuteNonQuery();

            transaction.Commit();
        }

        // ===================== SALES =====================

        public static DataTable GetSales(string search = "", string status = "", DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();

            string sql = @"SELECT s.Id AS 'ID', 
                COALESCE(c.Name, 'Без покупателя') AS 'Покупатель',
                s.SaleDate AS 'Дата', s.Status AS 'Статус', s.TotalAmount AS 'Сумма'
                FROM Sales s LEFT JOIN Customers c ON s.CustomerId = c.Id WHERE 1=1";

            if (!string.IsNullOrWhiteSpace(search))
                sql += " AND (c.Name LIKE @search OR CAST(s.Id AS TEXT) LIKE @search)";
            if (!string.IsNullOrWhiteSpace(status) && status != "Все")
                sql += " AND s.Status = @status";
            if (dateFrom.HasValue)
                sql += " AND date(s.SaleDate) >= date(@dateFrom)";
            if (dateTo.HasValue)
                sql += " AND date(s.SaleDate) <= date(@dateTo)";

            sql += " ORDER BY s.Id DESC;";
            cmd.CommandText = sql;

            if (!string.IsNullOrWhiteSpace(search))
                cmd.Parameters.AddWithValue("@search", $"%{search}%");
            if (!string.IsNullOrWhiteSpace(status) && status != "Все")
                cmd.Parameters.AddWithValue("@status", status);
            if (dateFrom.HasValue)
                cmd.Parameters.AddWithValue("@dateFrom", dateFrom.Value.ToString("yyyy-MM-dd"));
            if (dateTo.HasValue)
                cmd.Parameters.AddWithValue("@dateTo", dateTo.Value.ToString("yyyy-MM-dd"));

            return ExecuteDataTable(cmd);
        }

        public static int AddSale(int? customerId, DateTime saleDate, string status, List<SaleItem> items)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                foreach (var item in items)
                {
                    using var checkCmd = connection.CreateCommand();
                    checkCmd.Transaction = transaction;
                    checkCmd.CommandText = "SELECT StockQuantity FROM Products WHERE Id = @id;";
                    checkCmd.Parameters.AddWithValue("@id", item.ProductId);
                    var stock = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (stock < item.Quantity)
                        throw new InvalidOperationException($"Недостаточно товара (ID: {item.ProductId}). В наличии: {stock}, требуется: {item.Quantity}");
                }

                decimal total = 0;
                foreach (var item in items)
                    total += item.Price * item.Quantity;

                using var cmd = connection.CreateCommand();
                cmd.Transaction = transaction;
                cmd.CommandText = @"INSERT INTO Sales (CustomerId, SaleDate, Status, TotalAmount) 
                    VALUES (@customerId, @date, @status, @total); SELECT last_insert_rowid();";
                cmd.Parameters.AddWithValue("@customerId", customerId.HasValue ? (object)customerId.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@date", saleDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@total", (double)total);
                int saleId = Convert.ToInt32(cmd.ExecuteScalar());

                foreach (var item in items)
                {
                    using var itemCmd = connection.CreateCommand();
                    itemCmd.Transaction = transaction;
                    itemCmd.CommandText = @"INSERT INTO SaleItems (SaleId, ProductId, Quantity, Price) 
                        VALUES (@saleId, @productId, @quantity, @price);";
                    itemCmd.Parameters.AddWithValue("@saleId", saleId);
                    itemCmd.Parameters.AddWithValue("@productId", item.ProductId);
                    itemCmd.Parameters.AddWithValue("@quantity", item.Quantity);
                    itemCmd.Parameters.AddWithValue("@price", (double)item.Price);
                    itemCmd.ExecuteNonQuery();

                    using var stockCmd = connection.CreateCommand();
                    stockCmd.Transaction = transaction;
                    stockCmd.CommandText = "UPDATE Products SET StockQuantity = StockQuantity - @quantity WHERE Id = @id;";
                    stockCmd.Parameters.AddWithValue("@quantity", item.Quantity);
                    stockCmd.Parameters.AddWithValue("@id", item.ProductId);
                    stockCmd.ExecuteNonQuery();
                }

                transaction.Commit();
                return saleId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public static void DeleteSale(int id)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Получаем позиции продажи для возврата товара
                using var cmdSelect = connection.CreateCommand();
                cmdSelect.Transaction = transaction;
                cmdSelect.CommandText = "SELECT ProductId, Quantity FROM SaleItems WHERE SaleId = @saleId;";
                cmdSelect.Parameters.AddWithValue("@saleId", id);

                var items = new List<(int productId, int quantity)>();
                using (var reader = cmdSelect.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add((reader.GetInt32(0), reader.GetInt32(1)));
                    }
                }

                // Возвращаем остатки
                foreach (var (productId, quantity) in items)
                {
                    using var cmdUpdate = connection.CreateCommand();
                    cmdUpdate.Transaction = transaction;
                    cmdUpdate.CommandText = "UPDATE Products SET StockQuantity = StockQuantity + @quantity WHERE Id = @id;";
                    cmdUpdate.Parameters.AddWithValue("@quantity", quantity);
                    cmdUpdate.Parameters.AddWithValue("@id", productId);
                    cmdUpdate.ExecuteNonQuery();
                }

                // Удаляем продажу (SaleItems удалятся каскадно)
                using var cmdDelete = connection.CreateCommand();
                cmdDelete.Transaction = transaction;
                cmdDelete.CommandText = "DELETE FROM Sales WHERE Id = @id;";
                cmdDelete.Parameters.AddWithValue("@id", id);
                cmdDelete.ExecuteNonQuery();

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public static DataTable GetSaleItems(int saleId)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT p.Name AS 'Товар', si.Quantity AS 'Количество', 
                si.Price AS 'Цена', (si.Quantity * si.Price) AS 'Сумма'
                FROM SaleItems si JOIN Products p ON si.ProductId = p.Id WHERE si.SaleId = @saleId;";
            cmd.Parameters.AddWithValue("@saleId", saleId);
            return ExecuteDataTable(cmd);
        }

        // ===================== REPORTS =====================

        public static DataTable GetStockReport(int? categoryId = null)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();

            string sql = @"SELECT p.Name AS 'Товар', c.Name AS 'Категория', 
                p.StockQuantity AS 'Остаток', p.PurchasePrice AS 'Закуп. цена',
                p.SalePrice AS 'Цена продажи', (p.StockQuantity * p.SalePrice) AS 'Стоимость остатков'
                FROM Products p LEFT JOIN Categories c ON p.CategoryId = c.Id WHERE 1=1";

            if (categoryId.HasValue && categoryId.Value > 0)
                sql += " AND p.CategoryId = @categoryId";

            sql += " ORDER BY p.Name;";
            cmd.CommandText = sql;

            if (categoryId.HasValue && categoryId.Value > 0)
                cmd.Parameters.AddWithValue("@categoryId", categoryId.Value);

            return ExecuteDataTable(cmd);
        }

        public static DataTable GetSalesReport(DateTime dateFrom, DateTime dateTo, int? categoryId = null)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();

            string sql = @"SELECT s.SaleDate AS 'Дата', p.Name AS 'Товар', si.Quantity AS 'Кол-во', 
                si.Price AS 'Цена', (si.Quantity * si.Price) AS 'Сумма', s.Status AS 'Статус'
                FROM Sales s 
                JOIN SaleItems si ON s.Id = si.SaleId 
                JOIN Products p ON si.ProductId = p.Id 
                WHERE date(s.SaleDate) BETWEEN date(@from) AND date(@to)";

            if (categoryId.HasValue && categoryId.Value > 0)
                sql += " AND p.CategoryId = @categoryId";

            sql += " ORDER BY s.SaleDate DESC;";
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@from", dateFrom.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@to", dateTo.ToString("yyyy-MM-dd"));

            if (categoryId.HasValue && categoryId.Value > 0)
                cmd.Parameters.AddWithValue("@categoryId", categoryId.Value);

            return ExecuteDataTable(cmd);
        }

        public static DataTable GetPurchasesReport(DateTime dateFrom, DateTime dateTo, int? categoryId = null)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();

            string sql = @"SELECT pu.PurchaseDate AS 'Дата', p.Name AS 'Товар', s.Name AS 'Поставщик',
                pu.Quantity AS 'Кол-во', pu.PurchasePrice AS 'Цена', 
                (pu.Quantity * pu.PurchasePrice) AS 'Сумма'
                FROM Purchases pu 
                JOIN Products p ON pu.ProductId = p.Id 
                JOIN Suppliers s ON pu.SupplierId = s.Id 
                WHERE date(pu.PurchaseDate) BETWEEN date(@from) AND date(@to)";

            if (categoryId.HasValue && categoryId.Value > 0)
                sql += " AND p.CategoryId = @categoryId";

            sql += " ORDER BY pu.PurchaseDate DESC;";
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@from", dateFrom.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@to", dateTo.ToString("yyyy-MM-dd"));

            if (categoryId.HasValue && categoryId.Value > 0)
                cmd.Parameters.AddWithValue("@categoryId", categoryId.Value);

            return ExecuteDataTable(cmd);
        }

        public static DataTable GetProfitReport(DateTime dateFrom, DateTime dateTo, int? categoryId = null)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();

            string sql = @"SELECT p.Name AS 'Товар', 
                SUM(si.Quantity) AS 'Продано шт.',
                SUM(si.Quantity * si.Price) AS 'Выручка',
                SUM(si.Quantity * p.PurchasePrice) AS 'Себестоимость',
                SUM(si.Quantity * (si.Price - p.PurchasePrice)) AS 'Прибыль'
                FROM Sales s 
                JOIN SaleItems si ON s.Id = si.SaleId 
                JOIN Products p ON si.ProductId = p.Id 
                WHERE date(s.SaleDate) BETWEEN date(@from) AND date(@to) AND s.Status = 'Завершена'";

            if (categoryId.HasValue && categoryId.Value > 0)
                sql += " AND p.CategoryId = @categoryId";

            sql += " GROUP BY p.Id, p.Name ORDER BY SUM(si.Quantity * (si.Price - p.PurchasePrice)) DESC;";
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@from", dateFrom.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@to", dateTo.ToString("yyyy-MM-dd"));

            if (categoryId.HasValue && categoryId.Value > 0)
                cmd.Parameters.AddWithValue("@categoryId", categoryId.Value);

            return ExecuteDataTable(cmd);
        }
    }

    public class SaleItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total => Price * Quantity;
    }
}