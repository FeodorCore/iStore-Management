using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Apple
{
    public partial class App : Form
    {
        private DataTable? _currentReceiptData;
        private string _currentReceiptCustomer = "";
        private int _currentReceiptSaleId = 0;
        private string _currentReportTitle = "Отчёт";

        public App()
        {
            InitializeComponent();
            InitializeApp();
        }

        private static DataTable CreateSafeComboTable(DataTable source, int defaultId, string defaultName)
        {
            var result = new DataTable();
            result.Columns.Add("Id", typeof(int));
            result.Columns.Add("Name", typeof(string));
            result.Rows.Add(defaultId, defaultName);
            if (source != null)
            {
                foreach (DataRow row in source.Rows)
                {
                    result.Rows.Add(Convert.ToInt32(row["Id"]), row["Name"]?.ToString() ?? "");
                }
            }
            return result;
        }

        private void InitializeApp()
        {
            try
            {
                DatabaseHelper.Initialize();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации БД: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
            button1.Click += BtnAddProduct_Click;
            button2.Click += BtnEditProduct_Click;
            button3.Click += BtnDeleteProduct_Click;
            textBox1.TextChanged += ProductsSearch_Changed;
            textBox2.TextChanged += ProductsSearch_Changed;
            textBox3.TextChanged += ProductsSearch_Changed;
            comboBox1.SelectedIndexChanged += ProductsSearch_Changed;
            dataGridView1.CellDoubleClick += DataGridView_Products_DoubleClick;

            btnAddCategory.Click += BtnAddCategory_Click;
            btnEditCategory.Click += BtnEditCategory_Click;
            btnDeleteCategory.Click += BtnDeleteCategory_Click;
            textBoxCatSearch.TextChanged += CategoriesSearch_Changed;
            dataGridView2.CellDoubleClick += DataGridView_Categories_DoubleClick;

            btnAddSupplier.Click += BtnAddSupplier_Click;
            btnEditSupplier.Click += BtnEditSupplier_Click;
            btnDeleteSupplier.Click += BtnDeleteSupplier_Click;
            textBoxSupSearch.TextChanged += SuppliersSearch_Changed;
            dataGridView3.CellDoubleClick += DataGridView_Suppliers_DoubleClick;

            btnAddCustomer.Click += BtnAddCustomer_Click;
            btnEditCustomer.Click += BtnEditCustomer_Click;
            btnDeleteCustomer.Click += BtnDeleteCustomer_Click;
            textBoxCustSearch.TextChanged += CustomersSearch_Changed;
            comboBoxCustType.SelectedIndexChanged += CustomersSearch_Changed;
            dataGridView4.CellDoubleClick += DataGridView_Customers_DoubleClick;

            btnAddPurchase.Click += BtnAddPurchase_Click;
            btnEditPurchase.Click += BtnEditPurchase_Click;
            btnDeletePurchase.Click += BtnDeletePurchase_Click;
            textBoxPurSearch.TextChanged += PurchasesSearch_Changed;
            comboBoxPurSupplier.SelectedIndexChanged += PurchasesSearch_Changed;
            dateTimePickerPurFrom.ValueChanged += PurchasesSearch_Changed;
            dateTimePickerPurTo.ValueChanged += PurchasesSearch_Changed;

            btnAddSale.Click += BtnAddSale_Click;
            btnEditSale.Click += BtnEditSale_Click;
            btnDeleteSale.Click += BtnDeleteSale_Click;
            btnPrintCheck.Click += BtnPrintCheck_Click;
            textBoxSaleSearch.TextChanged += SalesSearch_Changed;
            comboBoxSaleStatus.SelectedIndexChanged += SalesSearch_Changed;
            dateTimePickerSaleFrom.ValueChanged += SalesSearch_Changed;
            dateTimePickerSaleTo.ValueChanged += SalesSearch_Changed;

            dataGridView6.CellDoubleClick += DataGridView_Sales_DoubleClick;

            btnReportStock.Click += BtnReportStock_Click;
            btnReportSales.Click += BtnReportSales_Click;
            btnReportPurchases.Click += BtnReportPurchases_Click;
            btnReportProfit.Click += BtnReportProfit_Click;
            btnExportExcel.Click += BtnExportExcel_Click;

            dataGridView7.DataError += DataGridView7_DataError;

            LoadProducts();
            LoadCategoryCombo();
            LoadCustomerTypeCombo();
            LoadSupplierCombo();
            LoadSaleStatusCombo();

            var now = DateTime.Now;
            dateTimePickerPurFrom.Value = now.AddMonths(-1);
            dateTimePickerPurTo.Value = now;
            dateTimePickerSaleFrom.Value = now.AddMonths(-1);
            dateTimePickerSaleTo.Value = now;
            dateTimePickerRepFrom.Value = now.AddMonths(-1);
            dateTimePickerRepTo.Value = now;
        }

        #region Tab Switching
        private void TabControl1_SelectedIndexChanged(object? sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0: LoadProducts(); break;
                case 1: LoadCategories(); break;
                case 2: LoadSuppliers(); break;
                case 3: LoadCustomers(); break;
                case 4: LoadPurchases(); break;
                case 5: LoadSales(); break;
                case 6: LoadReports(); break;
            }
        }
        #endregion

        #region Products (Tab 1)
        private void LoadProducts()
        {
            try
            {
                string search = textBox1.Text.Trim();
                decimal? minPrice = ParseDecimal(textBox2.Text);
                decimal? maxPrice = ParseDecimal(textBox3.Text);
                int? categoryId = null;
                if (comboBox1.SelectedItem is DataRowView drv && drv["Id"] != DBNull.Value)
                    categoryId = Convert.ToInt32(drv["Id"]);

                dataGridView1.DataSource = DatabaseHelper.GetProducts(search, minPrice, maxPrice, categoryId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCategoryCombo()
        {
            try
            {
                var sourceCategories = DatabaseHelper.GetCategoriesForCombo();
                var dt1 = CreateSafeComboTable(sourceCategories, 0, "Все");
                comboBox1.DataSource = dt1;
                comboBox1.DisplayMember = "Name";
                comboBox1.ValueMember = "Id";
                if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;

                var dt2 = CreateSafeComboTable(sourceCategories, 0, "Все");
                comboBoxRepCategory.DataSource = dt2;
                comboBoxRepCategory.DisplayMember = "Name";
                comboBoxRepCategory.ValueMember = "Id";
                if (comboBoxRepCategory.Items.Count > 0) comboBoxRepCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProductsSearch_Changed(object? sender, EventArgs e) => LoadProducts();

        private void DataGridView_Products_DoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) BtnEditProduct_Click(sender, EventArgs.Empty);
        }

        private void BtnAddProduct_Click(object? sender, EventArgs e)
        {
            using var form = new ProductForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DatabaseHelper.AddProduct(form.ProductName, form.CategoryId,
                        form.PurchasePrice, form.SalePrice, 0);
                    LoadProducts();
                    LoadCategoryCombo();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка добавления товара: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnEditProduct_Click(object? sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;
            var row = dataGridView1.CurrentRow;
            if (row.Cells["colProdId"].Value == null) return;

            int id = Convert.ToInt32(row.Cells["colProdId"].Value);
            int currentStock = DatabaseHelper.GetProductStock(id);

            using var form = new ProductForm();
            form.LoadProduct(id,
                row.Cells["colProdName"].Value?.ToString() ?? "",
                GetCategoryIdFromName(row.Cells["colProdCategory"].Value?.ToString()),
                Convert.ToDecimal(row.Cells["colProdPurchasePrice"].Value ?? 0),
                Convert.ToDecimal(row.Cells["colProdSalePrice"].Value ?? 0));

            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DatabaseHelper.UpdateProduct(id, form.ProductName, form.CategoryId,
                        form.PurchasePrice, form.SalePrice, currentStock);
                    LoadProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка обновления товара: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnDeleteProduct_Click(object? sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;
            if (dataGridView1.CurrentRow.Cells["colProdId"].Value == null) return;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["colProdId"].Value);

            if (DatabaseHelper.HasProductHistory(id))
            {
                MessageBox.Show("Нельзя удалить товар, по которому были закупки или продажи!\nЭто нарушит финансовую историю.", "Запрещено", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Удалить выбранный товар?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DatabaseHelper.DeleteProduct(id);
                    LoadProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления товара: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private int? GetCategoryIdFromName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            try
            {
                var dt = DatabaseHelper.GetCategoriesForCombo();
                foreach (DataRow row in dt.Rows)
                {
                    if (row["Name"].ToString() == name)
                        return Convert.ToInt32(row["Id"]);
                }
            }
            catch { }
            return null;
        }
        #endregion

        #region Categories (Tab 2)
        private void LoadCategories()
        {
            try
            {
                dataGridView2.DataSource = DatabaseHelper.GetCategories(textBoxCatSearch.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CategoriesSearch_Changed(object? sender, EventArgs e) => LoadCategories();

        private void DataGridView_Categories_DoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) BtnEditCategory_Click(sender, EventArgs.Empty);
        }

        private void BtnAddCategory_Click(object? sender, EventArgs e)
        {
            string? name = ShowInputDialog("Добавить категорию", "Название категории:");
            if (!string.IsNullOrWhiteSpace(name))
            {
                try
                {
                    DatabaseHelper.AddCategory(name.Trim());
                    LoadCategories();
                    LoadCategoryCombo();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnEditCategory_Click(object? sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow == null) return;
            if (dataGridView2.CurrentRow.Cells["colCatId"].Value == null) return;

            int id = Convert.ToInt32(dataGridView2.CurrentRow.Cells["colCatId"].Value);
            string currentName = dataGridView2.CurrentRow.Cells["colCatName"].Value?.ToString() ?? "";
            string? name = ShowInputDialog("Редактировать категорию", "Название категории:", currentName);

            if (!string.IsNullOrWhiteSpace(name))
            {
                try
                {
                    DatabaseHelper.UpdateCategory(id, name.Trim());
                    LoadCategories();
                    LoadCategoryCombo();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnDeleteCategory_Click(object? sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow == null) return;
            if (dataGridView2.CurrentRow.Cells["colCatId"].Value == null) return;

            int id = Convert.ToInt32(dataGridView2.CurrentRow.Cells["colCatId"].Value);
            if (MessageBox.Show("Удалить выбранную категорию?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DatabaseHelper.DeleteCategory(id);
                    LoadCategories();
                    LoadCategoryCombo();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region Suppliers (Tab 3)
        private void LoadSuppliers()
        {
            try
            {
                dataGridView3.DataSource = DatabaseHelper.GetSuppliers(textBoxSupSearch.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки поставщиков: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSupplierCombo()
        {
            try
            {
                var sourceSuppliers = DatabaseHelper.GetSuppliersForCombo();
                var dt = CreateSafeComboTable(sourceSuppliers, 0, "Все");
                comboBoxPurSupplier.DataSource = dt;
                comboBoxPurSupplier.DisplayMember = "Name";
                comboBoxPurSupplier.ValueMember = "Id";
                if (comboBoxPurSupplier.Items.Count > 0) comboBoxPurSupplier.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки поставщиков: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SuppliersSearch_Changed(object? sender, EventArgs e) => LoadSuppliers();

        private void DataGridView_Suppliers_DoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) BtnEditSupplier_Click(sender, EventArgs.Empty);
        }

        private void BtnAddSupplier_Click(object? sender, EventArgs e)
        {
            using var form = new SupplierForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DatabaseHelper.AddSupplier(form.SupplierName, form.ContactPerson,
                        form.Phone, form.Email, form.Address);
                    LoadSuppliers();
                    LoadSupplierCombo();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnEditSupplier_Click(object? sender, EventArgs e)
        {
            if (dataGridView3.CurrentRow == null) return;
            if (dataGridView3.CurrentRow.Cells["colSupId"].Value == null) return;

            int id = Convert.ToInt32(dataGridView3.CurrentRow.Cells["colSupId"].Value);
            using var form = new SupplierForm();
            form.LoadData(
                dataGridView3.CurrentRow.Cells["colSupName"].Value?.ToString() ?? "",
                dataGridView3.CurrentRow.Cells["colSupContact"].Value?.ToString() ?? "",
                dataGridView3.CurrentRow.Cells["colSupPhone"].Value?.ToString() ?? "",
                dataGridView3.CurrentRow.Cells["colSupEmail"].Value?.ToString() ?? "",
                dataGridView3.CurrentRow.Cells["colSupAddress"].Value?.ToString() ?? "");

            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DatabaseHelper.UpdateSupplier(id, form.SupplierName, form.ContactPerson,
                        form.Phone, form.Email, form.Address);
                    LoadSuppliers();
                    LoadSupplierCombo();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnDeleteSupplier_Click(object? sender, EventArgs e)
        {
            if (dataGridView3.CurrentRow == null) return;
            if (dataGridView3.CurrentRow.Cells["colSupId"].Value == null) return;

            int id = Convert.ToInt32(dataGridView3.CurrentRow.Cells["colSupId"].Value);
            if (MessageBox.Show("Удалить выбранного поставщика?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DatabaseHelper.DeleteSupplier(id);
                    LoadSuppliers();
                    LoadSupplierCombo();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region Customers (Tab 4)
        private void LoadCustomers()
        {
            try
            {
                string search = textBoxCustSearch.Text.Trim();
                string type = comboBoxCustType.SelectedItem?.ToString() ?? "Все";
                dataGridView4.DataSource = DatabaseHelper.GetCustomers(search, type);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки покупателей: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCustomerTypeCombo()
        {
            comboBoxCustType.Items.Clear();
            comboBoxCustType.Items.Add("Все");
            comboBoxCustType.Items.Add("Розничный");
            comboBoxCustType.Items.Add("Оптовый");
            if (comboBoxCustType.Items.Count > 0) comboBoxCustType.SelectedIndex = 0;
        }

        private void CustomersSearch_Changed(object? sender, EventArgs e) => LoadCustomers();

        private void DataGridView_Customers_DoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) BtnEditCustomer_Click(sender, EventArgs.Empty);
        }

        private void BtnAddCustomer_Click(object? sender, EventArgs e)
        {
            using var form = new CustomerForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DatabaseHelper.AddCustomer(form.CustomerName, form.CustomerType,
                        form.Phone, form.Email, form.Address);
                    LoadCustomers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnEditCustomer_Click(object? sender, EventArgs e)
        {
            if (dataGridView4.CurrentRow == null) return;
            if (dataGridView4.CurrentRow.Cells["colCustId"].Value == null) return;

            int id = Convert.ToInt32(dataGridView4.CurrentRow.Cells["colCustId"].Value);
            using var form = new CustomerForm();
            form.LoadData(
                dataGridView4.CurrentRow.Cells["colCustName"].Value?.ToString() ?? "",
                dataGridView4.CurrentRow.Cells["colCustType"].Value?.ToString() ?? "Розничный",
                dataGridView4.CurrentRow.Cells["colCustPhone"].Value?.ToString() ?? "",
                dataGridView4.CurrentRow.Cells["colCustEmail"].Value?.ToString() ?? "",
                dataGridView4.CurrentRow.Cells["colCustAddress"].Value?.ToString() ?? "");

            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DatabaseHelper.UpdateCustomer(id, form.CustomerName, form.CustomerType,
                        form.Phone, form.Email, form.Address);
                    LoadCustomers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnDeleteCustomer_Click(object? sender, EventArgs e)
        {
            if (dataGridView4.CurrentRow == null) return;
            if (dataGridView4.CurrentRow.Cells["colCustId"].Value == null) return;

            int id = Convert.ToInt32(dataGridView4.CurrentRow.Cells["colCustId"].Value);
            if (MessageBox.Show("Удалить выбранного покупателя?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DatabaseHelper.DeleteCustomer(id);
                    LoadCustomers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region Purchases (Tab 5)
        private void LoadPurchases()
        {
            try
            {
                string search = textBoxPurSearch.Text.Trim();
                int? supplierId = null;
                if (comboBoxPurSupplier.SelectedItem is DataRowView drv && drv["Id"] != DBNull.Value)
                {
                    int val = Convert.ToInt32(drv["Id"]);
                    if (val > 0) supplierId = val;
                }
                dataGridView5.DataSource = DatabaseHelper.GetPurchases(search, supplierId,
                    dateTimePickerPurFrom.Value, dateTimePickerPurTo.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки закупок: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PurchasesSearch_Changed(object? sender, EventArgs e) => LoadPurchases();

        private void BtnAddPurchase_Click(object? sender, EventArgs e)
        {
            using var form = new PurchaseForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.ProductId <= 0 || form.SupplierId <= 0 || form.Quantity <= 0)
                {
                    MessageBox.Show("Заполните корректно товар, поставщика и количество!", "Внимание",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                try
                {
                    DatabaseHelper.AddPurchase(form.ProductId, form.SupplierId,
                        form.Quantity, form.PurchasePrice, form.PurchaseDate);
                    LoadPurchases();
                    LoadProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка добавления закупки: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnEditPurchase_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Редактирование закупки запрещено учетной политикой.\nОформите возврат поставщику и создайте новую закупку.", "Информация",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnDeletePurchase_Click(object? sender, EventArgs e)
        {
            if (dataGridView5.CurrentRow == null) return;
            if (dataGridView5.CurrentRow.Cells["colPurId"].Value == null) return;

            int id = Convert.ToInt32(dataGridView5.CurrentRow.Cells["colPurId"].Value);
            if (MessageBox.Show("Оформить ВОЗВРАТ поставщику?\n(Товар должен быть в наличии на складе)", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DatabaseHelper.ReturnPurchase(id);
                    LoadPurchases();
                    LoadProducts();
                    MessageBox.Show("Возврат успешно оформлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region Sales (Tab 6)
        private void LoadSales()
        {
            try
            {
                string search = textBoxSaleSearch.Text.Trim();
                string status = comboBoxSaleStatus.SelectedItem?.ToString() ?? "Все";
                dataGridView6.DataSource = DatabaseHelper.GetSales(search, status,
                    dateTimePickerSaleFrom.Value, dateTimePickerSaleTo.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки продаж: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridView_Sales_DoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dataGridView6.CurrentRow == null) return;
            if (dataGridView6.CurrentRow.Cells["colSaleId"].Value == null) return;

            int saleId = Convert.ToInt32(dataGridView6.CurrentRow.Cells["colSaleId"].Value);
            string customer = dataGridView6.CurrentRow.Cells["colSaleCustomer"]?.Value?.ToString() ?? "Без покупателя";
            string status = dataGridView6.CurrentRow.Cells["colSaleStatus"]?.Value?.ToString() ?? "";

            try
            {
                var items = DatabaseHelper.GetSaleItems(saleId);
                if (items == null || items.Rows.Count == 0)
                {
                    MessageBox.Show("В этой продаже нет позиций.", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using var form = new SaleItemsViewForm(saleId, customer, status, items);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки позиций: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSaleStatusCombo()
        {
            comboBoxSaleStatus.Items.Clear();
            comboBoxSaleStatus.Items.Add("Все");
            comboBoxSaleStatus.Items.Add("Завершена");
            comboBoxSaleStatus.Items.Add("Отменена");
            comboBoxSaleStatus.Items.Add("Возврат");
            if (comboBoxSaleStatus.Items.Count > 0) comboBoxSaleStatus.SelectedIndex = 0;
        }

        private void SalesSearch_Changed(object? sender, EventArgs e) => LoadSales();

        private void BtnAddSale_Click(object? sender, EventArgs e)
        {
            using var form = new SaleForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (form.Status == "Завершена" && form.Items.Count == 0)
                    {
                        MessageBox.Show("Невозможно создать пустую завершенную продажу.", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    int saleId = DatabaseHelper.AddSale(form.CustomerId, form.SaleDate, form.Status, form.Items);
                    LoadSales();
                    LoadProducts();

                    if (form.Status == "Завершена" && form.Items.Count > 0)
                    {
                        _currentReceiptSaleId = saleId;
                        _currentReceiptCustomer = form.CustomerName;
                        _currentReceiptData = DatabaseHelper.GetSaleItems(saleId);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка продажи: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnEditSale_Click(object? sender, EventArgs e)
        {
            if (dataGridView6.CurrentRow == null) return;
            if (dataGridView6.CurrentRow.Cells["colSaleId"].Value == null) return;

            int id = Convert.ToInt32(dataGridView6.CurrentRow.Cells["colSaleId"].Value);
            string status = dataGridView6.CurrentRow.Cells["colSaleStatus"].Value?.ToString() ?? "";

            if (status == "Завершена")
            {
                var result = MessageBox.Show(
                    "Оформить ВОЗВРАТ?\n(Товар вернется на склад, статус сменится на 'Возврат')",
                    "Управление продажей",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        DatabaseHelper.ReturnSale(id);
                        LoadSales();
                        LoadProducts();
                        MessageBox.Show("Возврат успешно оформлен! Товар возвращен на склад.", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка возврата: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (status == "Возврат" || status == "Отменена")
            {
                MessageBox.Show("Эта продажа уже обработана. Редактирование невозможно.", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnDeleteSale_Click(object? sender, EventArgs e)
        {
            if (dataGridView6.CurrentRow == null) return;
            if (dataGridView6.CurrentRow.Cells["colSaleId"].Value == null) return;

            int id = Convert.ToInt32(dataGridView6.CurrentRow.Cells["colSaleId"].Value);
            string status = dataGridView6.CurrentRow.Cells["colSaleStatus"].Value?.ToString() ?? "";

            if (status == "Завершена")
            {
                MessageBox.Show("Удаление завершенной продажи запрещено!\nИспользуйте 'Редактировать' -> 'Оформить возврат'.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string msg = "Удалить продажу?";
            if (status == "Отменена") msg += "\nСклад не изменится (товар не списывался).";
            else if (status == "Возврат") msg += "\nСклад не изменится (товар уже был возвращен ранее).";

            if (MessageBox.Show(msg, "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DatabaseHelper.DeleteSale(id);
                    LoadSales();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnPrintCheck_Click(object? sender, EventArgs e)
        {
            if (dataGridView6.CurrentRow == null)
            {
                MessageBox.Show("Выберите продажу для печати чека.", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dataGridView6.CurrentRow.Cells["colSaleId"].Value == null) return;

            int saleId = Convert.ToInt32(dataGridView6.CurrentRow.Cells["colSaleId"].Value);
            _currentReceiptSaleId = saleId;
            _currentReceiptCustomer = dataGridView6.CurrentRow.Cells["colSaleCustomer"].Value?.ToString() ?? "Без покупателя";
            _currentReceiptData = DatabaseHelper.GetSaleItems(saleId);

            var printDoc = new PrintDocument();
            printDoc.PrintPage += PrintDoc_PrintPage;
            var preview = new PrintPreviewDialog
            {
                Document = printDoc,
                Width = 500,
                Height = 600,
                Text = "Предпросмотр чека"
            };
            try { preview.ShowDialog(); }
            catch (Exception ex) { MessageBox.Show($"Ошибка печати: {ex.Message}"); }
        }

        private void PrintDoc_PrintPage(object? sender, PrintPageEventArgs e)
        {
            if (_currentReceiptData == null || e.Graphics == null) return;
            var g = e.Graphics;
            float y = 20;
            float leftMargin = 20;
            var titleFont = new Font("Arial", 14, FontStyle.Bold);
            var headerFont = new Font("Arial", 10, FontStyle.Bold);
            var normalFont = new Font("Arial", 9);

            g.DrawString("=== ЧЕК ===", titleFont, Brushes.Black, leftMargin, y); y += 30;
            g.DrawString($"Чек №: {_currentReceiptSaleId}", normalFont, Brushes.Black, leftMargin, y); y += 18;
            g.DrawString($"Дата: {DateTime.Now:dd.MM.yyyy HH:mm}", normalFont, Brushes.Black, leftMargin, y); y += 18;
            g.DrawString($"Покупатель: {_currentReceiptCustomer}", normalFont, Brushes.Black, leftMargin, y); y += 25;
            g.DrawString(new string('-', 50), normalFont, Brushes.Black, leftMargin, y); y += 18;

            g.DrawString("Товар", headerFont, Brushes.Black, leftMargin, y);
            g.DrawString("Кол-во", headerFont, Brushes.Black, leftMargin + 200, y);
            g.DrawString("Цена", headerFont, Brushes.Black, leftMargin + 270, y);
            g.DrawString("Сумма", headerFont, Brushes.Black, leftMargin + 350, y); y += 20;
            g.DrawString(new string('-', 50), normalFont, Brushes.Black, leftMargin, y); y += 15;

            decimal total = 0;
            foreach (DataRow row in _currentReceiptData.Rows)
            {
                string product = row["Товар"].ToString() ?? "";
                string qty = row["Количество"].ToString() ?? "0";
                string price = Convert.ToDecimal(row["Цена"]).ToString("F2");
                string sum = Convert.ToDecimal(row["Сумма"]).ToString("F2");
                total += Convert.ToDecimal(row["Сумма"]);

                g.DrawString(product, normalFont, Brushes.Black, leftMargin, y);
                g.DrawString(qty, normalFont, Brushes.Black, leftMargin + 210, y);
                g.DrawString(price, normalFont, Brushes.Black, leftMargin + 270, y);
                g.DrawString(sum, normalFont, Brushes.Black, leftMargin + 350, y);
                y += 16;
            }
            y += 5;
            g.DrawString(new string('-', 50), normalFont, Brushes.Black, leftMargin, y); y += 18;
            g.DrawString($"ИТОГО: {total:F2} руб.", headerFont, Brushes.Black, leftMargin + 280, y); y += 25;
            g.DrawString("Спасибо за покупку!", normalFont, Brushes.Black, leftMargin + 130, y); y += 18;
            g.DrawString("=== Apple Store ===", normalFont, Brushes.Black, leftMargin + 120, y);
        }
        #endregion

        #region Reports (Tab 7)
        private void SetReportData(DataTable data)
        {
            dataGridView7.DataSource = null;
            dataGridView7.Columns.Clear();
            dataGridView7.DataSource = data;
            foreach (DataGridViewColumn col in dataGridView7.Columns)
            {
                string header = col.HeaderText.ToLower();
                if (header.Contains("цена") || header.Contains("сумма") ||
                    header.Contains("выручка") || header.Contains("себестоимость") ||
                    header.Contains("прибыль") || header.Contains("стоимость"))
                {
                    col.DefaultCellStyle.Format = "N2";
                }
            }
        }

        private void DataGridView7_DataError(object? sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void LoadReports()
        {
            _currentReportTitle = "Остатки на складе";
            try { SetReportData(DatabaseHelper.GetStockReport()); }
            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}"); }
        }

        private void BtnReportStock_Click(object? sender, EventArgs e)
        {
            _currentReportTitle = "Остатки на складе";
            try
            {
                int? categoryId = null;
                if (comboBoxRepCategory.SelectedItem is DataRowView drv && drv["Id"] != DBNull.Value)
                {
                    int val = Convert.ToInt32(drv["Id"]);
                    if (val > 0) categoryId = val;
                }
                SetReportData(DatabaseHelper.GetStockReport(categoryId));
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}"); }
        }

        private void BtnReportSales_Click(object? sender, EventArgs e)
        {
            _currentReportTitle = "Продажи";
            try
            {
                int? categoryId = null;
                if (comboBoxRepCategory.SelectedItem is DataRowView drv && drv["Id"] != DBNull.Value)
                {
                    int val = Convert.ToInt32(drv["Id"]);
                    if (val > 0) categoryId = val;
                }
                SetReportData(DatabaseHelper.GetSalesReport(
                    dateTimePickerRepFrom.Value, dateTimePickerRepTo.Value, categoryId));
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}"); }
        }

        private void BtnReportPurchases_Click(object? sender, EventArgs e)
        {
            _currentReportTitle = "Закупки";
            try
            {
                int? categoryId = null;
                if (comboBoxRepCategory.SelectedItem is DataRowView drv && drv["Id"] != DBNull.Value)
                {
                    int val = Convert.ToInt32(drv["Id"]);
                    if (val > 0) categoryId = val;
                }
                SetReportData(DatabaseHelper.GetPurchasesReport(
                    dateTimePickerRepFrom.Value, dateTimePickerRepTo.Value, categoryId));
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}"); }
        }

        private void BtnReportProfit_Click(object? sender, EventArgs e)
        {
            _currentReportTitle = "Прибыль";
            try
            {
                int? categoryId = null;
                if (comboBoxRepCategory.SelectedItem is DataRowView drv && drv["Id"] != DBNull.Value)
                {
                    int val = Convert.ToInt32(drv["Id"]);
                    if (val > 0) categoryId = val;
                }
                SetReportData(DatabaseHelper.GetProfitReport(
                    dateTimePickerRepFrom.Value, dateTimePickerRepTo.Value, categoryId));
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}"); }
        }

        private void BtnExportExcel_Click(object? sender, EventArgs e)
        {
            if (dataGridView7.Rows.Count == 0)
            {
                MessageBox.Show("Нет данных для экспорта.", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string safeFileName = SanitizeFileName(_currentReportTitle);

            using var dialog = new SaveFileDialog
            {
                Filter = "Excel файл (*.xlsx)|*.xlsx",
                FileName = $"{safeFileName}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                DefaultExt = "xlsx"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ExportToExcel(dataGridView7, dialog.FileName, _currentReportTitle);
                    MessageBox.Show("Отчёт успешно сохранён в Excel!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка экспорта: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static void ExportToExcel(DataGridView dgv, string filePath, string reportTitle)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add(reportTitle.Length > 31 ? reportTitle.Substring(0, 31) : reportTitle);

            ws.Cell(1, 1).Value = reportTitle;
            ws.Cell(1, 1).Style.Font.Bold = true;
            ws.Cell(1, 1).Style.Font.FontSize = 16;
            ws.Cell(1, 1).Style.Font.FontColor = XLColor.DarkBlue;

            ws.Cell(2, 1).Value = $"Дата: {DateTime.Now:dd.MM.yyyy HH:mm}";
            ws.Cell(2, 1).Style.Font.FontColor = XLColor.Gray;
            ws.Cell(2, 1).Style.Font.Italic = true;

            int headerRow = 4;
            int colCount = dgv.Columns.Count;

            for (int c = 0; c < colCount; c++)
            {
                var cell = ws.Cell(headerRow, c + 1);
                cell.Value = dgv.Columns[c].HeaderText;
                cell.Style.Font.Bold = true;
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Fill.BackgroundColor = XLColor.DarkBlue;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            }

            int dataRow = headerRow + 1;
            for (int r = 0; r < dgv.Rows.Count; r++)
            {
                var dgvRow = dgv.Rows[r];
                if (dgvRow.IsNewRow) continue;

                for (int c = 0; c < colCount; c++)
                {
                    var cell = ws.Cell(dataRow, c + 1);
                    var val = dgvRow.Cells[c].Value;

                    if (val == null || val == DBNull.Value)
                    {
                        cell.Value = "";
                    }
                    else if (val is decimal dec)
                    {
                        cell.Value = (double)dec;
                        cell.Style.NumberFormat.Format = "#,##0.00";
                    }
                    else if (val is double d)
                    {
                        cell.Value = d;
                        cell.Style.NumberFormat.Format = "#,##0.00";
                    }
                    else if (val is int i)
                    {
                        cell.Value = i;
                        cell.Style.NumberFormat.Format = "#,##0";
                    }
                    else if (val is long l)
                    {
                        cell.Value = l;
                    }
                    else if (val is DateTime dt)
                    {
                        cell.Value = dt;
                        cell.Style.NumberFormat.Format = "dd.MM.yyyy HH:mm";
                    }
                    else
                    {
                        cell.Value = val.ToString();
                    }

                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    cell.Style.Border.OutsideBorderColor = XLColor.LightGray;

                    if (r % 2 == 1)
                        cell.Style.Fill.BackgroundColor = XLColor.AliceBlue;
                }
                dataRow++;
            }

            if (colCount > 1)
                ws.Range(1, 1, 1, colCount).Merge();

            ws.Columns().AdjustToContents();
            foreach (var col in ws.Columns(1, colCount))
            {
                if (col.Width < 10) col.Width = 10;
                if (col.Width > 50) col.Width = 50;
            }

            int lastDataRow = dataRow - 1;
            if (lastDataRow >= headerRow)
            {
                ws.Range(headerRow, 1, lastDataRow, colCount).SetAutoFilter();
            }

            ws.SheetView.FreezeRows(headerRow);

            workbook.SaveAs(filePath);
        }

        private static string SanitizeFileName(string fileName)
        {
            var invalid = Path.GetInvalidFileNameChars();
            var sb = new StringBuilder(fileName.Length);
            foreach (char c in fileName)
                sb.Append(invalid.Contains(c) ? '_' : c);
            return sb.ToString();
        }
        #endregion

        #region Helper Methods
        private static decimal? ParseDecimal(string? text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            if (decimal.TryParse(text, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out decimal result))
                return result;
            if (decimal.TryParse(text, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.CurrentCulture, out result))
                return result;
            return null;
        }

        private static string? ShowInputDialog(string title, string prompt, string defaultValue = "")
        {
            var form = new Form
            {
                Width = 350,
                Height = 160,
                Text = title,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };
            var label = new Label { Left = 20, Top = 20, Text = prompt, AutoSize = true };
            var textBox = new TextBox { Left = 20, Top = 45, Width = 290, Text = defaultValue };
            var btnOk = new Button { Text = "OK", Left = 150, Width = 75, Top = 80, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Отмена", Left = 235, Width = 75, Top = 80, DialogResult = DialogResult.Cancel };
            form.Controls.AddRange(new Control[] { label, textBox, btnOk, btnCancel });
            form.AcceptButton = btnOk;
            form.CancelButton = btnCancel;
            return form.ShowDialog() == DialogResult.OK ? textBox.Text : null;
        }
        #endregion
    }

    #region Helper Forms
    public class ProductForm : Form
    {
        private readonly TextBox _txtName;
        private readonly ComboBox _cmbCategory;
        private readonly NumericUpDown _nudPurchasePrice;
        private readonly NumericUpDown _nudSalePrice;

        public string ProductName => _txtName.Text.Trim();
        public int? CategoryId
        {
            get
            {
                if (_cmbCategory.SelectedItem is DataRowView drv && drv["Id"] != DBNull.Value)
                {
                    int val = Convert.ToInt32(drv["Id"]);
                    return val > 0 ? val : null;
                }
                return null;
            }
        }
        public decimal PurchasePrice => _nudPurchasePrice.Value;
        public decimal SalePrice => _nudSalePrice.Value;

        public ProductForm()
        {
            Text = "Товар";
            Width = 420;
            Height = 310;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;

            var mainTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 6,
                Padding = new Padding(15),
                AutoScroll = true
            };

            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            for (int i = 0; i < 4; i++)
                mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 15));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));

            mainTable.Controls.Add(CreateLabel("Название:"), 0, 0);
            _txtName = new TextBox { Dock = DockStyle.Fill, Margin = new Padding(0, 3, 0, 0) };
            mainTable.Controls.Add(_txtName, 1, 0);

            mainTable.Controls.Add(CreateLabel("Категория:"), 0, 1);
            _cmbCategory = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Margin = new Padding(0, 3, 0, 0) };
            var safeDt = new DataTable();
            safeDt.Columns.Add("Id", typeof(int));
            safeDt.Columns.Add("Name", typeof(string));
            try
            {
                var source = DatabaseHelper.GetCategoriesForCombo();
                foreach (DataRow row in source.Rows)
                    safeDt.Rows.Add(Convert.ToInt32(row["Id"]), row["Name"].ToString());
            }
            catch { }
            _cmbCategory.DataSource = safeDt;
            _cmbCategory.DisplayMember = "Name";
            _cmbCategory.ValueMember = "Id";
            mainTable.Controls.Add(_cmbCategory, 1, 1);

            mainTable.Controls.Add(CreateLabel("Закуп. цена:"), 0, 2);
            _nudPurchasePrice = new NumericUpDown { Dock = DockStyle.Fill, Maximum = 9999999, DecimalPlaces = 2, Margin = new Padding(0, 3, 0, 0) };
            mainTable.Controls.Add(_nudPurchasePrice, 1, 2);

            mainTable.Controls.Add(CreateLabel("Цена продажи:"), 0, 3);
            _nudSalePrice = new NumericUpDown { Dock = DockStyle.Fill, Maximum = 9999999, DecimalPlaces = 2, Margin = new Padding(0, 3, 0, 0) };
            mainTable.Controls.Add(_nudSalePrice, 1, 3);

            var btnOk = new Button { Text = "OK", Width = 90, Height = 35, DialogResult = DialogResult.OK, Margin = new Padding(3) };
            var btnCancel = new Button { Text = "Отмена", Width = 90, Height = 35, DialogResult = DialogResult.Cancel, Margin = new Padding(3) };

            var btnPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Anchor = AnchorStyles.None
            };
            btnPanel.Controls.Add(btnCancel);
            btnPanel.Controls.Add(btnOk);
            mainTable.SetColumnSpan(btnPanel, 2);
            mainTable.Controls.Add(btnPanel, 0, 5);

            Controls.Add(mainTable);
            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }

        private Label CreateLabel(string text)
        {
            return new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false,
                Margin = new Padding(0, 5, 0, 0)
            };
        }

        public void LoadProduct(int id, string name, int? categoryId, decimal purchasePrice, decimal salePrice)
        {
            Text = "Редактировать товар";
            _txtName.Text = name;
            _nudPurchasePrice.Value = purchasePrice;
            _nudSalePrice.Value = salePrice;

            if (categoryId.HasValue)
            {
                for (int i = 0; i < _cmbCategory.Items.Count; i++)
                {
                    if (_cmbCategory.Items[i] is DataRowView drv && Convert.ToInt32(drv["Id"]) == categoryId.Value)
                    {
                        _cmbCategory.SelectedIndex = i;
                        break;
                    }
                }
            }
        }
    }

    public class SupplierForm : Form
    {
        private readonly TextBox _txtName;
        private readonly TextBox _txtContact;
        private readonly TextBox _txtPhone;
        private readonly TextBox _txtEmail;
        private readonly TextBox _txtAddress;

        public string SupplierName => _txtName.Text.Trim();
        public string ContactPerson => _txtContact.Text.Trim();
        public string Phone => _txtPhone.Text.Trim();
        public string Email => _txtEmail.Text.Trim();
        public string Address => _txtAddress.Text.Trim();

        public SupplierForm()
        {
            Text = "Поставщик";
            Width = 440;
            Height = 340;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;

            var mainTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 7,
                Padding = new Padding(15),
                AutoScroll = true
            };

            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            for (int i = 0; i < 5; i++)
                mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 15));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));

            string[] labels = { "Название:", "Контактное лицо:", "Телефон:", "Email:", "Адрес:" };
            TextBox[] boxes = new TextBox[5];

            for (int i = 0; i < 5; i++)
            {
                mainTable.Controls.Add(CreateLabel(labels[i]), 0, i);
                boxes[i] = new TextBox { Dock = DockStyle.Fill, Margin = new Padding(0, 3, 0, 0) };
                mainTable.Controls.Add(boxes[i], 1, i);
            }

            _txtName = boxes[0];
            _txtContact = boxes[1];
            _txtPhone = boxes[2];
            _txtEmail = boxes[3];
            _txtAddress = boxes[4];

            var btnOk = new Button { Text = "OK", Width = 90, Height = 35, DialogResult = DialogResult.OK, Margin = new Padding(3) };
            var btnCancel = new Button { Text = "Отмена", Width = 90, Height = 35, DialogResult = DialogResult.Cancel, Margin = new Padding(3) };

            var btnPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft
            };
            btnPanel.Controls.Add(btnCancel);
            btnPanel.Controls.Add(btnOk);
            mainTable.SetColumnSpan(btnPanel, 2);
            mainTable.Controls.Add(btnPanel, 0, 6);

            Controls.Add(mainTable);
            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }

        private Label CreateLabel(string text)
        {
            return new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false,
                Margin = new Padding(0, 5, 0, 0)
            };
        }

        public void LoadData(string name, string contact, string phone, string email, string address)
        {
            Text = "Редактировать поставщика";
            _txtName.Text = name;
            _txtContact.Text = contact;
            _txtPhone.Text = phone;
            _txtEmail.Text = email;
            _txtAddress.Text = address;
        }
    }

    public class CustomerForm : Form
    {
        private readonly TextBox _txtName;
        private readonly ComboBox _cmbType;
        private readonly TextBox _txtPhone;
        private readonly TextBox _txtEmail;
        private readonly TextBox _txtAddress;

        public string CustomerName => _txtName.Text.Trim();
        public string CustomerType => _cmbType.SelectedItem?.ToString() ?? "Розничный";
        public string Phone => _txtPhone.Text.Trim();
        public string Email => _txtEmail.Text.Trim();
        public string Address => _txtAddress.Text.Trim();

        public CustomerForm()
        {
            Text = "Покупатель";
            Width = 440;
            Height = 340;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;

            var mainTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 7,
                Padding = new Padding(15),
                AutoScroll = true
            };

            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            for (int i = 0; i < 5; i++)
                mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 15));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));

            mainTable.Controls.Add(CreateLabel("Имя/Название:"), 0, 0);
            _txtName = new TextBox { Dock = DockStyle.Fill, Margin = new Padding(0, 3, 0, 0) };
            mainTable.Controls.Add(_txtName, 1, 0);

            mainTable.Controls.Add(CreateLabel("Тип:"), 0, 1);
            _cmbType = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Margin = new Padding(0, 3, 0, 0) };
            _cmbType.Items.AddRange(new object[] { "Розничный", "Оптовый" });
            if (_cmbType.Items.Count > 0) _cmbType.SelectedIndex = 0;
            mainTable.Controls.Add(_cmbType, 1, 1);

            mainTable.Controls.Add(CreateLabel("Телефон:"), 0, 2);
            _txtPhone = new TextBox { Dock = DockStyle.Fill, Margin = new Padding(0, 3, 0, 0) };
            mainTable.Controls.Add(_txtPhone, 1, 2);

            mainTable.Controls.Add(CreateLabel("Email:"), 0, 3);
            _txtEmail = new TextBox { Dock = DockStyle.Fill, Margin = new Padding(0, 3, 0, 0) };
            mainTable.Controls.Add(_txtEmail, 1, 3);

            mainTable.Controls.Add(CreateLabel("Адрес:"), 0, 4);
            _txtAddress = new TextBox { Dock = DockStyle.Fill, Margin = new Padding(0, 3, 0, 0) };
            mainTable.Controls.Add(_txtAddress, 1, 4);

            var btnOk = new Button { Text = "OK", Width = 90, Height = 35, DialogResult = DialogResult.OK, Margin = new Padding(3) };
            var btnCancel = new Button { Text = "Отмена", Width = 90, Height = 35, DialogResult = DialogResult.Cancel, Margin = new Padding(3) };

            var btnPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft
            };
            btnPanel.Controls.Add(btnCancel);
            btnPanel.Controls.Add(btnOk);
            mainTable.SetColumnSpan(btnPanel, 2);
            mainTable.Controls.Add(btnPanel, 0, 6);

            Controls.Add(mainTable);
            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }

        private Label CreateLabel(string text)
        {
            return new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false,
                Margin = new Padding(0, 5, 0, 0)
            };
        }

        public void LoadData(string name, string type, string phone, string email, string address)
        {
            Text = "Редактировать покупателя";
            _txtName.Text = name;
            _cmbType.SelectedItem = type;
            _txtPhone.Text = phone;
            _txtEmail.Text = email;
            _txtAddress.Text = address;
        }
    }

    public class PurchaseForm : Form
    {
        private readonly ComboBox _cmbProduct;
        private readonly ComboBox _cmbSupplier;
        private readonly NumericUpDown _nudQuantity;
        private readonly NumericUpDown _nudPrice;
        private readonly DateTimePicker _dtpDate;
        private readonly DataTable _products;
        private readonly DataTable _suppliers;

        public int ProductId
        {
            get
            {
                if (_cmbProduct.SelectedItem is DataRowView drv && drv["Id"] != DBNull.Value)
                    return Convert.ToInt32(drv["Id"]);
                return 0;
            }
        }
        public int SupplierId
        {
            get
            {
                if (_cmbSupplier.SelectedItem is DataRowView drv && drv["Id"] != DBNull.Value)
                    return Convert.ToInt32(drv["Id"]);
                return 0;
            }
        }
        public int Quantity => (int)_nudQuantity.Value;
        public decimal PurchasePrice => _nudPrice.Value;
        public DateTime PurchaseDate => _dtpDate.Value;

        public PurchaseForm()
        {
            Text = "Новая закупка";
            Width = 450;
            Height = 340;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;

            var mainTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 7,
                Padding = new Padding(15),
                AutoScroll = true
            };

            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            for (int i = 0; i < 5; i++)
                mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 15));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));

            _products = new DataTable();
            _products.Columns.Add("Id", typeof(int));
            _products.Columns.Add("DisplayName", typeof(string));
            _products.Columns.Add("SalePrice", typeof(decimal));
            _products.Columns.Add("PurchasePrice", typeof(decimal));
            _products.Columns.Add("StockQuantity", typeof(int));
            try
            {
                var sourceProducts = DatabaseHelper.GetAllProductsForCombo();
                foreach (DataRow row in sourceProducts.Rows)
                {
                    _products.Rows.Add(
                        Convert.ToInt32(row["Id"]),
                        row["DisplayName"].ToString(),
                        Convert.ToDecimal(row["SalePrice"]),
                        Convert.ToDecimal(row["PurchasePrice"]),
                        Convert.ToInt32(row["StockQuantity"])
                    );
                }
            }
            catch { }

            _suppliers = new DataTable();
            _suppliers.Columns.Add("Id", typeof(int));
            _suppliers.Columns.Add("Name", typeof(string));
            try
            {
                var sourceSuppliers = DatabaseHelper.GetSuppliersForCombo();
                foreach (DataRow row in sourceSuppliers.Rows)
                    _suppliers.Rows.Add(Convert.ToInt32(row["Id"]), row["Name"].ToString());
            }
            catch { }

            mainTable.Controls.Add(CreateLabel("Товар:"), 0, 0);
            _cmbProduct = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Margin = new Padding(0, 3, 0, 0) };
            _cmbProduct.DataSource = _products;
            _cmbProduct.DisplayMember = "DisplayName";
            _cmbProduct.ValueMember = "Id";
            _cmbProduct.SelectedIndexChanged += CmbProduct_SelectedIndexChanged;
            mainTable.Controls.Add(_cmbProduct, 1, 0);

            mainTable.Controls.Add(CreateLabel("Поставщик:"), 0, 1);
            _cmbSupplier = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Margin = new Padding(0, 3, 0, 0) };
            _cmbSupplier.DataSource = _suppliers;
            _cmbSupplier.DisplayMember = "Name";
            _cmbSupplier.ValueMember = "Id";
            mainTable.Controls.Add(_cmbSupplier, 1, 1);

            mainTable.Controls.Add(CreateLabel("Количество:"), 0, 2);
            _nudQuantity = new NumericUpDown { Dock = DockStyle.Fill, Maximum = 999999, Minimum = 1, Value = 1, Margin = new Padding(0, 3, 0, 0) };
            mainTable.Controls.Add(_nudQuantity, 1, 2);

            mainTable.Controls.Add(CreateLabel("Цена закупки:"), 0, 3);
            _nudPrice = new NumericUpDown { Dock = DockStyle.Fill, Maximum = 9999999, DecimalPlaces = 2, Margin = new Padding(0, 3, 0, 0) };
            mainTable.Controls.Add(_nudPrice, 1, 3);

            mainTable.Controls.Add(CreateLabel("Дата:"), 0, 4);
            _dtpDate = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short, Margin = new Padding(0, 3, 0, 0) };
            mainTable.Controls.Add(_dtpDate, 1, 4);

            var btnOk = new Button { Text = "OK", Width = 90, Height = 35, DialogResult = DialogResult.OK, Margin = new Padding(3) };
            var btnCancel = new Button { Text = "Отмена", Width = 90, Height = 35, DialogResult = DialogResult.Cancel, Margin = new Padding(3) };

            var btnPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft
            };
            btnPanel.Controls.Add(btnCancel);
            btnPanel.Controls.Add(btnOk);
            mainTable.SetColumnSpan(btnPanel, 2);
            mainTable.Controls.Add(btnPanel, 0, 6);

            Controls.Add(mainTable);
            AcceptButton = btnOk;
            CancelButton = btnCancel;

            if (_cmbProduct.Items.Count > 0) _cmbProduct.SelectedIndex = 0;
            if (_cmbSupplier.Items.Count > 0) _cmbSupplier.SelectedIndex = 0;
        }

        private Label CreateLabel(string text)
        {
            return new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false,
                Margin = new Padding(0, 5, 0, 0)
            };
        }

        private void CmbProduct_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_cmbProduct.SelectedItem is DataRowView drv)
            {
                try { _nudPrice.Value = Convert.ToDecimal(drv["PurchasePrice"]); }
                catch { }
            }
        }
    }

    public class SaleItem : INotifyPropertyChanged
    {
        private int _productId;
        private string _productName = "";
        private int _quantity;
        private decimal _price;

        public int ProductId
        {
            get => _productId;
            set { _productId = value; OnPropertyChanged(nameof(ProductId)); }
        }

        public string ProductName
        {
            get => _productName;
            set { _productName = value; OnPropertyChanged(nameof(ProductName)); }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(Total));
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(Total));
            }
        }

        public decimal Total => _quantity * _price;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class SaleForm : Form
    {
        private readonly ComboBox _cmbCustomer;
        private readonly ComboBox _cmbStatus;
        private readonly DateTimePicker _dtpDate;
        private readonly ComboBox _cmbProduct;
        private readonly NumericUpDown _nudQuantity;
        private readonly NumericUpDown _nudPrice;
        private readonly Label _lblStock;
        private readonly Button _btnAddItem;
        private readonly Button _btnRemoveItem;
        private readonly DataGridView _dgvItems;
        private readonly Label _lblTotal;
        private readonly Button _btnOk;
        private readonly Button _btnCancel;
        private readonly BindingList<SaleItem> _items = new();
        private readonly Dictionary<int, int> _productStocks = new();
        private readonly Dictionary<int, string> _productNames = new();

        public int? CustomerId
        {
            get
            {
                if (_cmbCustomer.SelectedItem is DataRowView drv && drv["Id"] != DBNull.Value)
                {
                    int val = Convert.ToInt32(drv["Id"]);
                    return val > 0 ? val : null;
                }
                return null;
            }
        }

        public string CustomerName => _cmbCustomer.SelectedItem is DataRowView drv
            ? drv["Name"]?.ToString() ?? "Без покупателя"
            : "Без покупателя";

        public DateTime SaleDate => _dtpDate.Value;
        public string Status => _cmbStatus.SelectedItem?.ToString() ?? "Завершена";
        public List<SaleItem> Items => _items.ToList();

        public SaleForm()
        {
            Text = "Новая продажа";
            Width = 950;
            Height = 650;
            MinimumSize = new Size(850, 550);
            FormBorderStyle = FormBorderStyle.Sizable;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;

            // === DataGridView - ДОБАВЛЯЕМ ПЕРВЫМ (Fill) ===
            _dgvItems = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = SystemColors.Window,
                RowHeadersVisible = false,
                GridColor = Color.LightGray,
                BorderStyle = BorderStyle.Fixed3D,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(248, 248, 255) },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(70, 130, 180),
                    ForeColor = Color.White,
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                EnableHeadersVisualStyles = false,
                RowTemplate = { Height = 35 }
            };
            _dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ProductId", HeaderText = "ID", Visible = false });
            _dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ProductName", HeaderText = "Товар", FillWeight = 50 });
            _dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "Quantity", HeaderText = "Кол-во", FillWeight = 15, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            _dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "Price", HeaderText = "Цена", FillWeight = 20, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" } });
            _dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "Total", HeaderText = "Сумма", FillWeight = 20, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2", Font = new Font("Arial", 10, FontStyle.Bold), ForeColor = Color.DarkGreen } });
            _dgvItems.DataSource = _items;
            _items.ListChanged += (s, e) => UpdateTotalLabel();
            Controls.Add(_dgvItems);

            // === Нижняя панель (Bottom) ===
            var bottomPanel = new Panel { Dock = DockStyle.Bottom, Height = 60, BackColor = Color.FromArgb(245, 245, 250), Padding = new Padding(10) };
            _btnRemoveItem = new Button { Text = "🗑 Удалить", Width = 130, Height = 35, Location = new Point(10, 12), BackColor = Color.FromArgb(200, 80, 80), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            _btnRemoveItem.Click += BtnRemoveItem_Click;
            bottomPanel.Controls.Add(_btnRemoveItem);
            _lblTotal = new Label { Text = "Добавьте товары", Font = new Font("Arial", 14, FontStyle.Bold), Location = new Point(160, 15), Width = 450, Height = 30, ForeColor = Color.Gray, TextAlign = ContentAlignment.MiddleLeft };
            bottomPanel.Controls.Add(_lblTotal);
            _btnCancel = new Button { Text = "Отмена", Width = 90, Height = 35, Dock = DockStyle.Right, DialogResult = DialogResult.Cancel, FlatStyle = FlatStyle.Flat, Margin = new Padding(3) };
            bottomPanel.Controls.Add(_btnCancel);
            _btnOk = new Button { Text = "✓ Продать", Width = 110, Height = 35, Dock = DockStyle.Right, DialogResult = DialogResult.OK, BackColor = Color.FromArgb(50, 150, 50), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Arial", 10, FontStyle.Bold) };
            _btnOk.Click += BtnOk_Click;
            bottomPanel.Controls.Add(_btnOk);
            Controls.Add(bottomPanel);

            // === Панель добавления (Top) ===
            var addPanel = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.FromArgb(250, 250, 245), Padding = new Padding(10, 8, 10, 8) };
            addPanel.Controls.Add(new Label { Text = "Товар:", Location = new Point(10, 15), AutoSize = true });
            _cmbProduct = new ComboBox { Width = 280, Location = new Point(60, 12), DropDownStyle = ComboBoxStyle.DropDownList };
            addPanel.Controls.Add(_cmbProduct);
            addPanel.Controls.Add(new Label { Text = "Кол-во:", Location = new Point(360, 15), AutoSize = true });
            _nudQuantity = new NumericUpDown { Width = 70, Location = new Point(420, 12), Maximum = 9999, Minimum = 1, Value = 1 };
            addPanel.Controls.Add(_nudQuantity);
            addPanel.Controls.Add(new Label { Text = "Цена:", Location = new Point(510, 15), AutoSize = true });
            _nudPrice = new NumericUpDown { Width = 90, Location = new Point(560, 12), Maximum = 9999999, DecimalPlaces = 2 };
            addPanel.Controls.Add(_nudPrice);
            _btnAddItem = new Button { Text = "➕ Добавить", Width = 100, Height = 30, Location = new Point(670, 10), BackColor = Color.FromArgb(70, 130, 180), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            _btnAddItem.Click += BtnAddItem_Click;
            addPanel.Controls.Add(_btnAddItem);
            _lblStock = new Label { Text = "Остаток: —", Location = new Point(790, 15), AutoSize = true, ForeColor = Color.Gray, Font = new Font("Arial", 9, FontStyle.Bold) };
            addPanel.Controls.Add(_lblStock);
            Controls.Add(addPanel);

            // === Верхняя панель (Top) ===
            var topPanel = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.FromArgb(245, 245, 250), Padding = new Padding(10, 8, 10, 8) };
            topPanel.Controls.Add(new Label { Text = "Покупатель:", Location = new Point(10, 15), AutoSize = true });
            _cmbCustomer = new ComboBox { Width = 200, Location = new Point(90, 12), DropDownStyle = ComboBoxStyle.DropDownList };
            topPanel.Controls.Add(_cmbCustomer);
            topPanel.Controls.Add(new Label { Text = "Дата:", Location = new Point(310, 15), AutoSize = true });
            _dtpDate = new DateTimePicker { Width = 130, Location = new Point(360, 12), Format = DateTimePickerFormat.Short };
            topPanel.Controls.Add(_dtpDate);
            topPanel.Controls.Add(new Label { Text = "Статус:", Location = new Point(510, 15), AutoSize = true });
            _cmbStatus = new ComboBox { Width = 120, Location = new Point(570, 12), DropDownStyle = ComboBoxStyle.DropDownList };
            _cmbStatus.Items.AddRange(new object[] { "Завершена", "Отменена" });
            if (_cmbStatus.Items.Count > 0) _cmbStatus.SelectedIndex = 0;
            _cmbStatus.SelectedIndexChanged += CmbStatus_SelectedIndexChanged;
            topPanel.Controls.Add(_cmbStatus);
            Controls.Add(topPanel);

            CancelButton = _btnCancel;
            AcceptButton = _btnOk;
            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            var dtCust = new DataTable();
            dtCust.Columns.Add("Id", typeof(int));
            dtCust.Columns.Add("Name", typeof(string));
            dtCust.Rows.Add(0, "Без покупателя");
            try { foreach (DataRow row in DatabaseHelper.GetCustomersForCombo().Rows) dtCust.Rows.Add(Convert.ToInt32(row["Id"]), row["Name"].ToString()); } catch { }
            _cmbCustomer.DataSource = dtCust;
            _cmbCustomer.DisplayMember = "Name";
            _cmbCustomer.ValueMember = "Id";
            if (_cmbCustomer.Items.Count > 0) _cmbCustomer.SelectedIndex = 0;

            var dtProd = new DataTable();
            dtProd.Columns.Add("Id", typeof(int));
            dtProd.Columns.Add("DisplayName", typeof(string));
            dtProd.Columns.Add("CleanName", typeof(string));
            dtProd.Columns.Add("SalePrice", typeof(decimal));
            dtProd.Columns.Add("StockQuantity", typeof(int));
            try
            {
                foreach (DataRow row in DatabaseHelper.GetProductsForCombo().Rows)
                {
                    int id = Convert.ToInt32(row["Id"]);
                    int stock = Convert.ToInt32(row["StockQuantity"]);
                    string name = row["DisplayName"].ToString() ?? "";
                    _productStocks[id] = stock;
                    _productNames[id] = name;
                    dtProd.Rows.Add(id, stock > 0 ? $"{name} [в наличии: {stock} шт.]" : $"{name} [НЕТ В НАЛИЧИИ]", name, Convert.ToDecimal(row["SalePrice"]), stock);
                }
            }
            catch { }
            _cmbProduct.DataSource = dtProd;
            _cmbProduct.DisplayMember = "DisplayName";
            _cmbProduct.ValueMember = "Id";
            if (_cmbProduct.Items.Count > 0) _cmbProduct.SelectedIndex = 0;
            _cmbProduct.SelectedIndexChanged += CmbProduct_SelectedIndexChanged;
        }

        private void CmbStatus_SelectedIndexChanged(object? sender, EventArgs e)
        {
            bool isCancelled = _cmbStatus.SelectedItem?.ToString() == "Отменена";
            _cmbProduct.Enabled = _nudQuantity.Enabled = _nudPrice.Enabled = _btnAddItem.Enabled = _btnRemoveItem.Enabled = !isCancelled;
            if (isCancelled) _items.Clear();
        }

        private void CmbProduct_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_cmbProduct.SelectedItem is DataRowView drv)
            {
                try
                {
                    int pid = Convert.ToInt32(drv["Id"]);
                    decimal price = Convert.ToDecimal(drv["SalePrice"]);
                    int stock = Convert.ToInt32(drv["StockQuantity"]);
                    if (price >= _nudPrice.Minimum && price <= _nudPrice.Maximum) _nudPrice.Value = price;
                    if (stock > 0) { _lblStock.Text = $"✓ Остаток: {stock} шт."; _lblStock.ForeColor = Color.FromArgb(50, 120, 50); }
                    else { _lblStock.Text = "✗ НЕТ В НАЛИЧИИ!"; _lblStock.ForeColor = Color.Red; }
                    if (stock > 0) { var ex = _items.FirstOrDefault(i => i.ProductId == pid); int added = ex?.Quantity ?? 0; _nudQuantity.Maximum = Math.Max(1, stock - added + (int)_nudQuantity.Value); _nudQuantity.Value = Math.Min(_nudQuantity.Value, _nudQuantity.Maximum); }
                    else { _nudQuantity.Maximum = 0; _nudQuantity.Value = 0; }
                }
                catch (Exception ex) { _lblStock.Text = $"Ошибка: {ex.Message}"; _lblStock.ForeColor = Color.Red; }
            }
            else { _lblStock.Text = "Остаток: —"; _lblStock.ForeColor = Color.Gray; }
        }

        private void BtnAddItem_Click(object? sender, EventArgs e)
        {
            if (_cmbProduct.SelectedItem is not DataRowView drv) { MessageBox.Show("Выберите товар.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            int pid; try { pid = Convert.ToInt32(drv["Id"]); } catch { return; }
            if (pid <= 0) return;
            _productStocks.TryGetValue(pid, out int stock);
            _productNames.TryGetValue(pid, out string pname);
            if (string.IsNullOrEmpty(pname)) pname = drv["CleanName"]?.ToString() ?? drv["DisplayName"]?.ToString() ?? "Товар";
            int qty = (int)_nudQuantity.Value; decimal price = _nudPrice.Value;
            if (qty <= 0 || stock <= 0) { MessageBox.Show(stock <= 0 ? "Товар отсутствует на складе!" : "Некорректное количество.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            var existing = _items.FirstOrDefault(i => i.ProductId == pid);
            if (existing != null) { if (existing.Quantity + qty > stock) { MessageBox.Show($"Недостаточно товара! В наличии: {stock}, уже добавлено: {existing.Quantity}.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; } existing.Quantity += qty; existing.Price = price; }
            else { if (qty > stock) { MessageBox.Show($"Недостаточно товара! В наличии: {stock}.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; } _items.Add(new SaleItem { ProductId = pid, ProductName = pname, Quantity = qty, Price = price }); }
        }

        private void BtnRemoveItem_Click(object? sender, EventArgs e)
        {
            if (_dgvItems.CurrentRow == null) return;
            int idx = _dgvItems.CurrentRow.Index;
            if (idx >= 0 && idx < _items.Count)
            {
                var rem = _items[idx];
                if (MessageBox.Show($"Удалить \"{rem.ProductName}\" ({rem.Quantity} шт.)?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    _items.RemoveAt(idx);
            }
        }

        private void BtnOk_Click(object? sender, EventArgs e)
        {
            if (_cmbStatus.SelectedItem?.ToString() == "Завершена" && _items.Count == 0) { MessageBox.Show("Добавьте хотя бы один товар!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            DialogResult = DialogResult.OK; Close();
        }

        private void UpdateTotalLabel()
        {
            decimal total = _items.Sum(i => i.Total);
            if (_items.Count == 0) { _lblTotal.Text = "Добавьте товары в продажу"; _lblTotal.ForeColor = Color.Gray; }
            else { _lblTotal.Text = $"ИТОГО: {total:N2} руб. ({_items.Count} поз.)"; _lblTotal.ForeColor = Color.FromArgb(30, 100, 30); }
        }
    }

    public class SaleItemsViewForm : Form
    {
        public SaleItemsViewForm(int saleId, string customer, string status, DataTable items)
        {
            Text = $"Позиции продажи №{saleId}"; Width = 800; Height = 550; MinimumSize = new Size(700, 400);
            StartPosition = FormStartPosition.CenterParent; FormBorderStyle = FormBorderStyle.Sizable; MaximizeBox = false; MinimizeBox = false;

            Panel header = new Panel { Dock = DockStyle.Top, Height = 80, BackColor = Color.FromArgb(70, 130, 180), Padding = new Padding(15) };
            header.Controls.Add(new Label { Text = $"Продажа №{saleId}", Font = new Font("Arial", 16, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 30, TextAlign = ContentAlignment.MiddleLeft });
            header.Controls.Add(new Label { Text = $"Покупатель: {customer} | Статус: {status}", Font = new Font("Arial", 10), ForeColor = Color.FromArgb(220, 220, 255), Dock = DockStyle.Top, Height = 25, TextAlign = ContentAlignment.MiddleLeft });
            Controls.Add(header);

            DataGridView dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = SystemColors.Window,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                GridColor = Color.LightGray,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(248, 248, 255) },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(50, 100, 150), ForeColor = Color.White, Font = new Font("Arial", 10, FontStyle.Bold), Alignment = DataGridViewContentAlignment.MiddleCenter },
                EnableHeadersVisualStyles = false,
                RowTemplate = { Height = 32 }
            };
            if (items.Columns.Contains("Товар")) dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Product", HeaderText = "Товар", DataPropertyName = "Товар", FillWeight = 50 });
            if (items.Columns.Contains("Количество")) dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Quantity", HeaderText = "Количество", DataPropertyName = "Количество", FillWeight = 15, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, Font = new Font("Arial", 10, FontStyle.Bold) } });
            if (items.Columns.Contains("Цена")) dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Price", HeaderText = "Цена", DataPropertyName = "Цена", FillWeight = 20, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" } });
            if (items.Columns.Contains("Сумма")) dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Total", HeaderText = "Сумма", DataPropertyName = "Сумма", FillWeight = 20, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2", Font = new Font("Arial", 10, FontStyle.Bold), ForeColor = Color.DarkGreen } });
            dgv.DataSource = items; Controls.Add(dgv);

            Panel bottom = new Panel { Dock = DockStyle.Bottom, Height = 60, BackColor = Color.FromArgb(245, 245, 250), Padding = new Padding(15) };
            decimal total = 0; foreach (DataRow row in items.Rows) if (items.Columns.Contains("Сумма") && row["Сумма"] != DBNull.Value) try { total += Convert.ToDecimal(row["Сумма"]); } catch { }
            bottom.Controls.Add(new Label { Text = $"ОБЩИЙ ИТОГ: {total:N2} руб. | Позиций: {items.Rows.Count}", Font = new Font("Arial", 13, FontStyle.Bold), ForeColor = Color.FromArgb(30, 100, 30), Dock = DockStyle.Left, Width = 500, TextAlign = ContentAlignment.MiddleLeft });
            var btnClose = new Button { Text = "Закрыть", Width = 120, Height = 35, Dock = DockStyle.Right, DialogResult = DialogResult.Cancel, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(100, 100, 100), ForeColor = Color.White, Font = new Font("Arial", 10) };
            bottom.Controls.Add(btnClose); Controls.Add(bottom);
            CancelButton = AcceptButton = btnClose;
        }
    }
    #endregion
}