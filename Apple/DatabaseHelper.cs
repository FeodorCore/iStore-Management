using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Microsoft.Data.Sqlite;

namespace Apple
{
    public static class DatabaseHelper
    {
        private static readonly string DbFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Apple");
        private static readonly string DbPath = Path.Combine(DbFolder, "apple.db");
        public static string ConnectionString => $"Data Source={DbPath}";

        private static DataTable ExecuteDataTable(SqliteCommand cmd)
        {
            var dt = new DataTable();
            using var reader = cmd.ExecuteReader();
            dt.Load(reader);
            return dt;
        }

        public static void Initialize()
        {
            if (!Directory.Exists(DbFolder))
                Directory.CreateDirectory(DbFolder);

            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "PRAGMA foreign_keys = ON;";
                cmd.ExecuteNonQuery();
            }

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
                        IsActive INTEGER NOT NULL DEFAULT 1,
                        FOREIGN KEY (CategoryId) REFERENCES Categories(Id) ON DELETE SET NULL
                    );
                    CREATE TABLE IF NOT EXISTS Purchases (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        ProductId INTEGER NOT NULL,
                        SupplierId INTEGER NOT NULL,
                        Quantity INTEGER NOT NULL,
                        PurchasePrice REAL NOT NULL,
                        PurchaseDate TEXT NOT NULL,
                        Status TEXT NOT NULL DEFAULT 'Оформлена',
                        FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE RESTRICT,
                        FOREIGN KEY (SupplierId) REFERENCES Suppliers(Id) ON DELETE RESTRICT
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
                        CostPrice REAL NOT NULL DEFAULT 0,
                        FOREIGN KEY (SaleId) REFERENCES Sales(Id) ON DELETE CASCADE,
                        FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE RESTRICT
                    );
                ";
                cmd.ExecuteNonQuery();
            }

            // 🚀 БЕЗОПАСНЫЕ МИГРАЦИИ (Добавляем новые поля в старые БД)
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(*) FROM pragma_table_info('Products') WHERE name='IsActive';";
                if ((long)cmd.ExecuteScalar() == 0)
                    Execute(connection, "ALTER TABLE Products ADD COLUMN IsActive INTEGER NOT NULL DEFAULT 1;");
            }
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(*) FROM pragma_table_info('Purchases') WHERE name='Status';";
                if ((long)cmd.ExecuteScalar() == 0)
                    Execute(connection, "ALTER TABLE Purchases ADD COLUMN Status TEXT NOT NULL DEFAULT 'Оформлена';");
            }
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(*) FROM pragma_table_info('SaleItems') WHERE name='CostPrice';";
                if ((long)cmd.ExecuteScalar() == 0)
                {
                    Execute(connection, "ALTER TABLE SaleItems ADD COLUMN CostPrice REAL NOT NULL DEFAULT 0;");
                    Execute(connection, "UPDATE SaleItems SET CostPrice = (SELECT PurchasePrice FROM Products WHERE Products.Id = SaleItems.ProductId);");
                }
            }

            SeedDataIfEmpty(connection);
        }

        private static void SeedDataIfEmpty(SqliteConnection connection)
        {
            using var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = "SELECT COUNT(*) FROM Categories;";
            long count = (long)checkCmd.ExecuteScalar();
            if (count > 0) return;

            Execute(connection, "INSERT INTO Categories (Name) VALUES ('iPhone 16'), ('iPhone 17'), ('Аксессуары');");
            Execute(connection, @"INSERT INTO Suppliers (Name, ContactPerson, Phone, Email, Address) VALUES
                ('Apple Distribution International', 'Джонсон М.', '+1-408-996-1010', 'europe_supply@apple.com', 'Ирландия, г. Корк, Hollyhill Industrial Estate'),
                ('ASBISc Enterprises', 'Костас А.', '+357-25-857-000', 'info@asbis.com', 'Кипр, г. Лимасол, ул. Архиепископа Макариоса III, 195'),
                ('Ingram Micro Inc.', 'Смит Д.', '+1-714-566-1000', 'europe@ingrammicro.com', 'США, г. Ирвайн, ул. Алтон Парквей, 3351');");
            Execute(connection, @"INSERT INTO Customers (Name, Type, Phone, Email, Address) VALUES
                ('5 Элемент', 'Сеть магазинов', '+375-29-555-66-77', 'info@5element.by', 'г. Минск, ул. Притыцкого, 100'),
                ('iStore', 'Сеть магазинов', '+375-29-666-77-88', 'info@istore.by', 'г. Минск, пр. Победителей, 10'),
                ('ИП Иванов В.С.', 'Розничный', '+375-44-111-22-33', 'ivanov@mail.ru', 'г. Могилёв, ул. Первомайская, 3');");
            Execute(connection, @"INSERT INTO Products (Name, CategoryId, PurchasePrice, SalePrice, StockQuantity, IsActive) VALUES
                ('iPhone 16 128GB', 1, 2500.00, 3199.00, 0, 1),
                ('iPhone 16 Pro 256GB', 1, 3500.00, 4399.00, 0, 1),
                ('AirPods Pro 2', 3, 500.00, 749.00, 0, 1);");
        }

        private static void Execute(SqliteConnection connection, string sql)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        public static bool HasProductHistory(int productId)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT 
                (SELECT COUNT(*) FROM Purchases WHERE ProductId = @id) + 
                (SELECT COUNT(*) FROM SaleItems WHERE ProductId = @id);";
            cmd.Parameters.AddWithValue("@id", productId);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
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
            try { cmd.ExecuteNonQuery(); }
            catch (SqliteException ex)
            {
                if (ex.SqliteErrorCode == 19 || ex.Message.Contains("FOREIGN KEY constraint failed"))
                    throw new Exception("Нельзя удалить поставщика, так как есть история закупок.");
                throw;
            }
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
            try { cmd.ExecuteNonQuery(); }
            catch (SqliteException ex)
            {
                if (ex.SqliteErrorCode == 19 || ex.Message.Contains("FOREIGN KEY constraint failed"))
                    throw new Exception("Нельзя удалить покупателя, так как есть история продаж.");
                throw;
            }
        }

        // ===================== PRODUCTS =====================
        public static DataTable GetProducts(string search = "", decimal? minPrice = null, decimal? maxPrice = null, int? categoryId = null)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            string sql = @"SELECT p.Id AS 'ID', p.Name AS 'Название', c.Name AS 'Категория',
                p.PurchasePrice AS 'Закуп. цена', p.SalePrice AS 'Цена продажи', p.StockQuantity AS 'Остаток'
                FROM Products p LEFT JOIN Categories c ON p.CategoryId = c.Id WHERE p.IsActive = 1";
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
            cmd.CommandText = "SELECT Id, Name || ' (' || StockQuantity || ' шт.)' AS DisplayName, SalePrice, PurchasePrice, StockQuantity FROM Products WHERE StockQuantity > 0 AND IsActive = 1 ORDER BY Name;";
            return ExecuteDataTable(cmd);
        }

        public static DataTable GetAllProductsForCombo()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Id, Name || ' (остаток: ' || StockQuantity || ' шт.)' AS DisplayName, SalePrice, PurchasePrice, StockQuantity FROM Products WHERE IsActive = 1 ORDER BY Name;";
            return ExecuteDataTable(cmd);
        }

        public static int GetProductStock(int productId)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT StockQuantity FROM Products WHERE Id = @id;";
            cmd.Parameters.AddWithValue("@id", productId);
            var result = cmd.ExecuteScalar();
            return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
        }

        public static void AddProduct(string name, int? categoryId, decimal purchasePrice, decimal salePrice, int stockQuantity)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Products (Name, CategoryId, PurchasePrice, SalePrice, StockQuantity, IsActive)
                VALUES (@name, @categoryId, @purchasePrice, @salePrice, @stock, 1);";
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
            if (HasProductHistory(id))
                throw new Exception("Нельзя удалить товар, по которому были закупки или продажи!");

            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE Products SET IsActive = 0 WHERE Id = @id;"; // Soft Delete
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
                pu.PurchaseDate AS 'Дата', pu.Status AS 'Статус'
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
            if (productId <= 0 || supplierId <= 0 || quantity <= 0)
                throw new ArgumentException("Проверьте корректность введенных данных.");

            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                // 🚀 Получаем текущий остаток и среднюю цену для расчета WAC
                using var cmdGet = connection.CreateCommand();
                cmdGet.Transaction = transaction;
                cmdGet.CommandText = "SELECT StockQuantity, PurchasePrice FROM Products WHERE Id = @id;";
                cmdGet.Parameters.AddWithValue("@id", productId);
                using var reader = cmdGet.ExecuteReader();
                int currentStock = 0;
                decimal currentAvgCost = 0;
                if (reader.Read())
                {
                    currentStock = reader.GetInt32(0);
                    currentAvgCost = Convert.ToDecimal(reader.GetDouble(1));
                }
                reader.Close();

                decimal totalOldCost = currentStock * currentAvgCost;
                decimal totalNewCost = quantity * purchasePrice;
                int newStock = currentStock + quantity;
                decimal newAvgCost = newStock > 0 ? (totalOldCost + totalNewCost) / newStock : purchasePrice;

                using var cmdInsert = connection.CreateCommand();
                cmdInsert.Transaction = transaction;
                cmdInsert.CommandText = @"INSERT INTO Purchases (ProductId, SupplierId, Quantity, PurchasePrice, PurchaseDate, Status)
                    VALUES (@productId, @supplierId, @quantity, @price, @date, 'Оформлена');";
                cmdInsert.Parameters.AddWithValue("@productId", productId);
                cmdInsert.Parameters.AddWithValue("@supplierId", supplierId);
                cmdInsert.Parameters.AddWithValue("@quantity", quantity);
                cmdInsert.Parameters.AddWithValue("@price", (double)purchasePrice);
                cmdInsert.Parameters.AddWithValue("@date", purchaseDate.ToString("yyyy-MM-dd"));
                cmdInsert.ExecuteNonQuery();

                using var cmdUpdate = connection.CreateCommand();
                cmdUpdate.Transaction = transaction;
                cmdUpdate.CommandText = "UPDATE Products SET StockQuantity = @stock, PurchasePrice = @avgCost WHERE Id = @id;";
                cmdUpdate.Parameters.AddWithValue("@stock", newStock);
                cmdUpdate.Parameters.AddWithValue("@avgCost", (double)newAvgCost);
                cmdUpdate.Parameters.AddWithValue("@id", productId);
                cmdUpdate.ExecuteNonQuery();

                transaction.Commit();
            }
            catch { transaction.Rollback(); throw; }
        }

        public static void ReturnPurchase(int id)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                using var cmdSelect = connection.CreateCommand();
                cmdSelect.Transaction = transaction;
                cmdSelect.CommandText = "SELECT ProductId, Quantity, Status FROM Purchases WHERE Id = @id;";
                cmdSelect.Parameters.AddWithValue("@id", id);

                int productId = 0, quantity = 0;
                string status = "";
                using (var reader = cmdSelect.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        productId = reader.GetInt32(0);
                        quantity = reader.GetInt32(1);
                        status = reader.GetString(2);
                    }
                    else return;
                }

                if (status != "Оформлена")
                    throw new InvalidOperationException("Эта закупка уже отменена или возвращена.");

                using var cmdStock = connection.CreateCommand();
                cmdStock.Transaction = transaction;
                cmdStock.CommandText = "SELECT StockQuantity FROM Products WHERE Id = @id;";
                cmdStock.Parameters.AddWithValue("@id", productId);
                int currentStock = Convert.ToInt32(cmdStock.ExecuteScalar());

                if (currentStock < quantity)
                    throw new InvalidOperationException($"Невозможно вернуть закупку! Товар уже продан. На складе осталось: {currentStock} шт.");

                using var cmdUpdateProd = connection.CreateCommand();
                cmdUpdateProd.Transaction = transaction;
                cmdUpdateProd.CommandText = "UPDATE Products SET StockQuantity = StockQuantity - @quantity WHERE Id = @id;";
                cmdUpdateProd.Parameters.AddWithValue("@quantity", quantity);
                cmdUpdateProd.Parameters.AddWithValue("@id", productId);
                cmdUpdateProd.ExecuteNonQuery();

                using var cmdUpdatePur = connection.CreateCommand();
                cmdUpdatePur.Transaction = transaction;
                cmdUpdatePur.CommandText = "UPDATE Purchases SET Status = 'Возврат' WHERE Id = @id;";
                cmdUpdatePur.Parameters.AddWithValue("@id", id);
                cmdUpdatePur.ExecuteNonQuery();

                transaction.Commit();
            }
            catch { transaction.Rollback(); throw; }
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
            foreach (var item in items)
            {
                if (item.ProductId <= 0 || item.Quantity <= 0)
                    throw new ArgumentException("Ошибка в позициях продажи.");
            }

            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                bool isCompleted = (status == "Завершена");
                var costs = new Dictionary<int, decimal>();

                if (isCompleted)
                {
                    foreach (var item in items)
                    {
                        using var checkCmd = connection.CreateCommand();
                        checkCmd.Transaction = transaction;
                        checkCmd.CommandText = "SELECT StockQuantity, PurchasePrice FROM Products WHERE Id = @id;";
                        checkCmd.Parameters.AddWithValue("@id", item.ProductId);
                        using var reader = checkCmd.ExecuteReader();
                        if (!reader.Read()) throw new Exception("Товар не найден");
                        int stock = reader.GetInt32(0);
                        decimal cost = Convert.ToDecimal(reader.GetDouble(1));
                        reader.Close();

                        if (stock < item.Quantity) throw new Exception($"Недостаточно {item.ProductName}");
                        costs[item.ProductId] = cost;
                    }
                }

                decimal total = 0;
                if (isCompleted) foreach (var item in items) total += item.Price * item.Quantity;

                using var cmd = connection.CreateCommand();
                cmd.Transaction = transaction;
                cmd.CommandText = @"INSERT INTO Sales (CustomerId, SaleDate, Status, TotalAmount)
                    VALUES (@customerId, @date, @status, @total); SELECT last_insert_rowid();";
                cmd.Parameters.AddWithValue("@customerId", customerId.HasValue ? (object)customerId.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@date", saleDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@total", (double)total);
                int saleId = Convert.ToInt32(cmd.ExecuteScalar());

                if (isCompleted)
                {
                    foreach (var item in items)
                    {
                        decimal costPrice = costs[item.ProductId];
                        using var itemCmd = connection.CreateCommand();
                        itemCmd.Transaction = transaction;
                        itemCmd.CommandText = @"INSERT INTO SaleItems (SaleId, ProductId, Quantity, Price, CostPrice)
                            VALUES (@saleId, @productId, @quantity, @price, @costPrice);";
                        itemCmd.Parameters.AddWithValue("@saleId", saleId);
                        itemCmd.Parameters.AddWithValue("@productId", item.ProductId);
                        itemCmd.Parameters.AddWithValue("@quantity", item.Quantity);
                        itemCmd.Parameters.AddWithValue("@price", (double)item.Price);
                        itemCmd.Parameters.AddWithValue("@costPrice", (double)costPrice);
                        itemCmd.ExecuteNonQuery();

                        using var stockCmd = connection.CreateCommand();
                        stockCmd.Transaction = transaction;
                        stockCmd.CommandText = "UPDATE Products SET StockQuantity = StockQuantity - @quantity WHERE Id = @id;";
                        stockCmd.Parameters.AddWithValue("@quantity", item.Quantity);
                        stockCmd.Parameters.AddWithValue("@id", item.ProductId);
                        stockCmd.ExecuteNonQuery();
                    }
                }
                transaction.Commit();
                return saleId;
            }
            catch { transaction.Rollback(); throw; }
        }

        public static void DeleteSale(int id)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var cmdStatus = connection.CreateCommand();
            cmdStatus.CommandText = "SELECT Status FROM Sales WHERE Id = @id;";
            cmdStatus.Parameters.AddWithValue("@id", id);
            string status = cmdStatus.ExecuteScalar()?.ToString() ?? "";

            if (status == "Завершена")
                throw new Exception("Удаление завершенной продажи запрещено учетной политикой.");

            using var transaction = connection.BeginTransaction();
            try
            {
                using var cmdDelete = connection.CreateCommand();
                cmdDelete.Transaction = transaction;
                cmdDelete.CommandText = "DELETE FROM Sales WHERE Id = @id;";
                cmdDelete.Parameters.AddWithValue("@id", id);
                cmdDelete.ExecuteNonQuery();
                transaction.Commit();
            }
            catch { transaction.Rollback(); throw; }
        }

        public static void ReturnSale(int id)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                using var cmdStatus = connection.CreateCommand();
                cmdStatus.Transaction = transaction;
                cmdStatus.CommandText = "SELECT Status FROM Sales WHERE Id = @id;";
                cmdStatus.Parameters.AddWithValue("@id", id);
                string status = cmdStatus.ExecuteScalar()?.ToString() ?? "";
                if (status != "Завершена")
                    throw new InvalidOperationException("Оформить возврат можно только для завершенной продажи.");

                using var cmdSelect = connection.CreateCommand();
                cmdSelect.Transaction = transaction;
                cmdSelect.CommandText = "SELECT ProductId, Quantity FROM SaleItems WHERE SaleId = @saleId;";
                cmdSelect.Parameters.AddWithValue("@saleId", id);
                using (var reader = cmdSelect.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int productId = reader.GetInt32(0);
                        int quantity = reader.GetInt32(1);
                        using var cmdUpdate = connection.CreateCommand();
                        cmdUpdate.Transaction = transaction;
                        cmdUpdate.CommandText = "UPDATE Products SET StockQuantity = StockQuantity + @quantity WHERE Id = @id;";
                        cmdUpdate.Parameters.AddWithValue("@quantity", quantity);
                        cmdUpdate.Parameters.AddWithValue("@id", productId);
                        cmdUpdate.ExecuteNonQuery();
                    }
                }
                using var cmdUpdateStatus = connection.CreateCommand();
                cmdUpdateStatus.Transaction = transaction;
                cmdUpdateStatus.CommandText = "UPDATE Sales SET Status = 'Возврат' WHERE Id = @id;";
                cmdUpdateStatus.Parameters.AddWithValue("@id", id);
                cmdUpdateStatus.ExecuteNonQuery();
                transaction.Commit();
            }
            catch { transaction.Rollback(); throw; }
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
                FROM Products p LEFT JOIN Categories c ON p.CategoryId = c.Id WHERE p.IsActive = 1";
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
                (pu.Quantity * pu.PurchasePrice) AS 'Сумма', pu.Status AS 'Статус'
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
            // 🚀 ИСПРАВЛЕНО: Прибыль теперь считается по ИСТОРИЧЕСКОЙ себестоимости (CostPrice)
            string sql = @"SELECT p.Name AS 'Товар',
                SUM(si.Quantity) AS 'Продано шт.',
                SUM(si.Quantity * si.Price) AS 'Выручка',
                SUM(si.Quantity * si.CostPrice) AS 'Себестоимость',
                SUM(si.Quantity * (si.Price - si.CostPrice)) AS 'Прибыль'
                FROM Sales s
                JOIN SaleItems si ON s.Id = si.SaleId
                JOIN Products p ON si.ProductId = p.Id
                WHERE date(s.SaleDate) BETWEEN date(@from) AND date(@to) AND s.Status = 'Завершена'";
            if (categoryId.HasValue && categoryId.Value > 0)
                sql += " AND p.CategoryId = @categoryId";
            sql += " GROUP BY p.Id, p.Name ORDER BY SUM(si.Quantity * (si.Price - si.CostPrice)) DESC;";
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
        public decimal CostPrice { get; set; }
        public decimal Total => Price * Quantity;
    }
}