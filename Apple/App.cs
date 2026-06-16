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

        // Название текущего отчёта — будет в заголовке Excel
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

            // Формируем имя файла из названия отчёта
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

        /// <summary>
        /// Экспорт в настоящий Excel (.xlsx) через ClosedXML.
        /// </summary>
        private static void ExportToExcel(DataGridView dgv, string filePath, string reportTitle)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add(reportTitle.Length > 31 ? reportTitle.Substring(0, 31) : reportTitle);

            // --- Строка 1: Заголовок отчёта (крупный, жирный) ---
            ws.Cell(1, 1).Value = reportTitle;
            ws.Cell(1, 1).Style.Font.Bold = true;
            ws.Cell(1, 1).Style.Font.FontSize = 16;
            ws.Cell(1, 1).Style.Font.FontColor = XLColor.DarkBlue;

            // --- Строка 2: Дата формирования ---
            ws.Cell(2, 1).Value = $"Дата: {DateTime.Now:dd.MM.yyyy HH:mm}";
            ws.Cell(2, 1).Style.Font.FontColor = XLColor.Gray;
            ws.Cell(2, 1).Style.Font.Italic = true;

            // --- Строка 4: Заголовки таблицы (синий фон, белый текст) ---
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

            // --- Строки данных (с 5-й) ---
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

                    // Границы
                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    cell.Style.Border.OutsideBorderColor = XLColor.LightGray;

                    // Зебра — чередование строк
                    if (r % 2 == 1)
                        cell.Style.Fill.BackgroundColor = XLColor.AliceBlue;
                }
                dataRow++;
            }

            // --- Объединяем заголовок на всю ширину таблицы ---
            if (colCount > 1)
                ws.Range(1, 1, 1, colCount).Merge();

            // --- Автоширина колонок ---
            ws.Columns().AdjustToContents();
            foreach (var col in ws.Columns(1, colCount))
            {
                if (col.Width < 10) col.Width = 10;
                if (col.Width > 50) col.Width = 50;
            }

            // --- Автофильтр на заголовках ---
            int lastDataRow = dataRow - 1;
            if (lastDataRow >= headerRow)
            {
                ws.Range(headerRow, 1, lastDataRow, colCount).SetAutoFilter();
            }

            // --- Закрепляем строку заголовков ---
            ws.SheetView.FreezeRows(headerRow);

            // --- Сохраняем ---
            workbook.SaveAs(filePath);
        }

        /// <summary>
        /// Убирает недопустимые символы из имени файла.
        /// </summary>
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
            Width = 400;
            Height = 290;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;

            int top = 15;
            Controls.Add(new Label { Text = "Название:", Left = 20, Top = top, AutoSize = true });
            _txtName = new TextBox { Left = 140, Top = top - 3, Width = 220 };
            Controls.Add(_txtName);
            top += 35;

            Controls.Add(new Label { Text = "Категория:", Left = 20, Top = top, AutoSize = true });
            _cmbCategory = new ComboBox { Left = 140, Top = top - 3, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
            var safeDt = new DataTable();
            safeDt.Columns.Add("Id", typeof(int));
            safeDt.Columns.Add("Name", typeof(string));
            try
            {
                var source = DatabaseHelper.GetCategoriesForCombo();
                foreach (DataRow row in source.Rows)
                {
                    safeDt.Rows.Add(Convert.ToInt32(row["Id"]), row["Name"].ToString());
                }
            }
            catch { }
            _cmbCategory.DataSource = safeDt;
            _cmbCategory.DisplayMember = "Name";
            _cmbCategory.ValueMember = "Id";
            Controls.Add(_cmbCategory);
            top += 35;

            Controls.Add(new Label { Text = "Закуп. цена:", Left = 20, Top = top, AutoSize = true });
            _nudPurchasePrice = new NumericUpDown { Left = 140, Top = top - 3, Width = 220, Maximum = 9999999, DecimalPlaces = 2 };
            Controls.Add(_nudPurchasePrice);
            top += 35;

            Controls.Add(new Label { Text = "Цена продажи:", Left = 20, Top = top, AutoSize = true });
            _nudSalePrice = new NumericUpDown { Left = 140, Top = top - 3, Width = 220, Maximum = 9999999, DecimalPlaces = 2 };
            Controls.Add(_nudSalePrice);
            top += 40;

            var btnOk = new Button { Text = "OK", Left = 190, Top = top, Width = 80, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Отмена", Left = 280, Top = top, Width = 80, DialogResult = DialogResult.Cancel };
            Controls.AddRange(new Control[] { btnOk, btnCancel });
            AcceptButton = btnOk;
            CancelButton = btnCancel;
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
            Width = 400;
            Height = 310;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;

            int top = 15;
            var labels = new[] { "Название:", "Контактное лицо:", "Телефон:", "Email:", "Адрес:" };
            var boxes = new TextBox[5];
            for (int i = 0; i < labels.Length; i++)
            {
                Controls.Add(new Label { Text = labels[i], Left = 20, Top = top, AutoSize = true });
                boxes[i] = new TextBox { Left = 140, Top = top - 3, Width = 220 };
                Controls.Add(boxes[i]);
                top += 35;
            }
            _txtName = boxes[0];
            _txtContact = boxes[1];
            _txtPhone = boxes[2];
            _txtEmail = boxes[3];
            _txtAddress = boxes[4];
            top += 5;

            var btnOk = new Button { Text = "OK", Left = 190, Top = top, Width = 80, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Отмена", Left = 280, Top = top, Width = 80, DialogResult = DialogResult.Cancel };
            Controls.AddRange(new Control[] { btnOk, btnCancel });
            AcceptButton = btnOk;
            CancelButton = btnCancel;
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
            Width = 400;
            Height = 310;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;

            int top = 15;
            Controls.Add(new Label { Text = "Имя/Название:", Left = 20, Top = top, AutoSize = true });
            _txtName = new TextBox { Left = 140, Top = top - 3, Width = 220 };
            Controls.Add(_txtName);
            top += 35;

            Controls.Add(new Label { Text = "Тип:", Left = 20, Top = top, AutoSize = true });
            _cmbType = new ComboBox { Left = 140, Top = top - 3, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
            _cmbType.Items.AddRange(new object[] { "Розничный", "Оптовый" });
            if (_cmbType.Items.Count > 0) _cmbType.SelectedIndex = 0;
            Controls.Add(_cmbType);
            top += 35;

            Controls.Add(new Label { Text = "Телефон:", Left = 20, Top = top, AutoSize = true });
            _txtPhone = new TextBox { Left = 140, Top = top - 3, Width = 220 };
            Controls.Add(_txtPhone);
            top += 35;

            Controls.Add(new Label { Text = "Email:", Left = 20, Top = top, AutoSize = true });
            _txtEmail = new TextBox { Left = 140, Top = top - 3, Width = 220 };
            Controls.Add(_txtEmail);
            top += 35;

            Controls.Add(new Label { Text = "Адрес:", Left = 20, Top = top, AutoSize = true });
            _txtAddress = new TextBox { Left = 140, Top = top - 3, Width = 220 };
            Controls.Add(_txtAddress);
            top += 40;

            var btnOk = new Button { Text = "OK", Left = 190, Top = top, Width = 80, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Отмена", Left = 280, Top = top, Width = 80, DialogResult = DialogResult.Cancel };
            Controls.AddRange(new Control[] { btnOk, btnCancel });
            AcceptButton = btnOk;
            CancelButton = btnCancel;
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
            Width = 420;
            Height = 320;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;

            int top = 15;
            Controls.Add(new Label { Text = "Товар:", Left = 20, Top = top, AutoSize = true });
            _cmbProduct = new ComboBox { Left = 140, Top = top - 3, Width = 240, DropDownStyle = ComboBoxStyle.DropDownList };
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
            _cmbProduct.DataSource = _products;
            _cmbProduct.DisplayMember = "DisplayName";
            _cmbProduct.ValueMember = "Id";
            _cmbProduct.SelectedIndexChanged += CmbProduct_SelectedIndexChanged;
            Controls.Add(_cmbProduct);
            top += 35;

            Controls.Add(new Label { Text = "Поставщик:", Left = 20, Top = top, AutoSize = true });
            _cmbSupplier = new ComboBox { Left = 140, Top = top - 3, Width = 240, DropDownStyle = ComboBoxStyle.DropDownList };
            _suppliers = new DataTable();
            _suppliers.Columns.Add("Id", typeof(int));
            _suppliers.Columns.Add("Name", typeof(string));
            try
            {
                var sourceSuppliers = DatabaseHelper.GetSuppliersForCombo();
                foreach (DataRow row in sourceSuppliers.Rows)
                {
                    _suppliers.Rows.Add(Convert.ToInt32(row["Id"]), row["Name"].ToString());
                }
            }
            catch { }
            _cmbSupplier.DataSource = _suppliers;
            _cmbSupplier.DisplayMember = "Name";
            _cmbSupplier.ValueMember = "Id";
            Controls.Add(_cmbSupplier);
            top += 35;

            Controls.Add(new Label { Text = "Количество:", Left = 20, Top = top, AutoSize = true });
            _nudQuantity = new NumericUpDown { Left = 140, Top = top - 3, Width = 240, Maximum = 999999, Minimum = 1, Value = 1 };
            Controls.Add(_nudQuantity);
            top += 35;

            Controls.Add(new Label { Text = "Цена закупки:", Left = 20, Top = top, AutoSize = true });
            _nudPrice = new NumericUpDown { Left = 140, Top = top - 3, Width = 240, Maximum = 9999999, DecimalPlaces = 2 };
            Controls.Add(_nudPrice);
            top += 35;

            Controls.Add(new Label { Text = "Дата:", Left = 20, Top = top, AutoSize = true });
            _dtpDate = new DateTimePicker { Left = 140, Top = top - 3, Width = 240, Format = DateTimePickerFormat.Short };
            Controls.Add(_dtpDate);
            top += 40;

            var btnOk = new Button { Text = "OK", Left = 210, Top = top, Width = 80, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Отмена", Left = 300, Top = top, Width = 80, DialogResult = DialogResult.Cancel };
            Controls.AddRange(new Control[] { btnOk, btnCancel });
            AcceptButton = btnOk;
            CancelButton = btnCancel;

            if (_cmbProduct.Items.Count > 0) _cmbProduct.SelectedIndex = 0;
            if (_cmbSupplier.Items.Count > 0) _cmbSupplier.SelectedIndex = 0;
        }

        private void CmbProduct_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_cmbProduct.SelectedItem is DataRowView drv)
            {
                try
                {
                    _nudPrice.Value = Convert.ToDecimal(drv["PurchasePrice"]);
                }
                catch { }
            }
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
        private readonly Button _btnAddItem;
        private readonly Button _btnRemoveItem;
        private readonly DataGridView _dgvItems;
        private readonly Label _lblTotal;
        private readonly Button _btnOk;
        private readonly Button _btnCancel;
        private readonly DataTable _products;
        private readonly BindingList<SaleItem> _items = new();
        private readonly DataTable _customers;

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
            Width = 700;
            Height = 550;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;

            int top = 15;
            Controls.Add(new Label { Text = "Покупатель:", Left = 20, Top = top, AutoSize = true });
            _cmbCustomer = new ComboBox { Left = 120, Top = top - 3, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            _customers = new DataTable();
            _customers.Columns.Add("Id", typeof(int));
            _customers.Columns.Add("Name", typeof(string));
            _customers.Rows.Add(0, "Без покупателя");
            try
            {
                var sourceCustomers = DatabaseHelper.GetCustomersForCombo();
                foreach (DataRow row in sourceCustomers.Rows)
                {
                    _customers.Rows.Add(Convert.ToInt32(row["Id"]), row["Name"].ToString());
                }
            }
            catch { }
            _cmbCustomer.DataSource = _customers;
            _cmbCustomer.DisplayMember = "Name";
            _cmbCustomer.ValueMember = "Id";
            if (_cmbCustomer.Items.Count > 0) _cmbCustomer.SelectedIndex = 0;
            Controls.Add(_cmbCustomer);

            Controls.Add(new Label { Text = "Дата:", Left = 340, Top = top, AutoSize = true });
            _dtpDate = new DateTimePicker { Left = 380, Top = top - 3, Width = 130, Format = DateTimePickerFormat.Short };
            Controls.Add(_dtpDate);

            Controls.Add(new Label { Text = "Статус:", Left = 530, Top = top, AutoSize = true });
            _cmbStatus = new ComboBox { Left = 580, Top = top - 3, Width = 90, DropDownStyle = ComboBoxStyle.DropDownList };
            _cmbStatus.Items.AddRange(new object[] { "Завершена", "Отменена" });
            if (_cmbStatus.Items.Count > 0) _cmbStatus.SelectedIndex = 0;
            _cmbStatus.SelectedIndexChanged += CmbStatus_SelectedIndexChanged;
            Controls.Add(_cmbStatus);
            top += 35;

            Controls.Add(new Label { Text = "Товар:", Left = 20, Top = top, AutoSize = true });
            _cmbProduct = new ComboBox { Left = 70, Top = top - 3, Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            _products = new DataTable();
            _products.Columns.Add("Id", typeof(int));
            _products.Columns.Add("DisplayName", typeof(string));
            _products.Columns.Add("SalePrice", typeof(decimal));
            _products.Columns.Add("StockQuantity", typeof(int));
            try
            {
                var sourceProducts = DatabaseHelper.GetProductsForCombo();
                foreach (DataRow row in sourceProducts.Rows)
                {
                    _products.Rows.Add(
                        Convert.ToInt32(row["Id"]),
                        row["DisplayName"].ToString(),
                        Convert.ToDecimal(row["SalePrice"]),
                        Convert.ToInt32(row["StockQuantity"])
                    );
                }
            }
            catch { }
            _cmbProduct.DataSource = _products;
            _cmbProduct.DisplayMember = "DisplayName";
            _cmbProduct.ValueMember = "Id";
            _cmbProduct.SelectedIndexChanged += CmbProduct_SelectedIndexChanged;
            Controls.Add(_cmbProduct);

            Controls.Add(new Label { Text = "Кол-во:", Left = 335, Top = top, AutoSize = true });
            _nudQuantity = new NumericUpDown { Left = 395, Top = top - 3, Width = 60, Maximum = 9999, Minimum = 1, Value = 1 };
            Controls.Add(_nudQuantity);

            Controls.Add(new Label { Text = "Цена:", Left = 465, Top = top, AutoSize = true });
            _nudPrice = new NumericUpDown { Left = 505, Top = top - 3, Width = 80, Maximum = 9999999, DecimalPlaces = 2 };
            Controls.Add(_nudPrice);

            _btnAddItem = new Button { Text = "Добавить", Left = 595, Top = top - 5, Width = 75, Height = 28 };
            _btnAddItem.Click += BtnAddItem_Click;
            Controls.Add(_btnAddItem);
            top += 40;

            _dgvItems = new DataGridView
            {
                Left = 20,
                Top = top,
                Width = 640,
                Height = 250,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            _dgvItems.Columns.Add("ProductId", "ID");
            _dgvItems.Columns["ProductId"]!.Visible = false;
            _dgvItems.Columns.Add("ProductName", "Товар");
            _dgvItems.Columns.Add("Quantity", "Количество");
            _dgvItems.Columns.Add("Price", "Цена");
            _dgvItems.Columns.Add("Total", "Сумма");
            Controls.Add(_dgvItems);
            top += 260;

            _btnRemoveItem = new Button { Text = "Удалить позицию", Left = 20, Top = top, Width = 130 };
            _btnRemoveItem.Click += BtnRemoveItem_Click;
            Controls.Add(_btnRemoveItem);

            _lblTotal = new Label
            {
                Text = "ИТОГО: 0.00 руб.",
                Left = 450,
                Top = top + 5,
                AutoSize = true,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
            Controls.Add(_lblTotal);
            top += 45;

            _btnOk = new Button { Text = "Продать", Left = 490, Top = top, Width = 80 };
            _btnOk.Click += BtnOk_Click;
            Controls.Add(_btnOk);

            _btnCancel = new Button { Text = "Отмена", Left = 580, Top = top, Width = 80, DialogResult = DialogResult.Cancel };
            Controls.Add(_btnCancel);
            CancelButton = _btnCancel;

            if (_cmbProduct.Items.Count > 0) _cmbProduct.SelectedIndex = 0;
        }

        private void CmbStatus_SelectedIndexChanged(object? sender, EventArgs e)
        {
            bool isCancelled = _cmbStatus.SelectedItem?.ToString() == "Отменена";
            _cmbProduct.Enabled = !isCancelled;
            _nudQuantity.Enabled = !isCancelled;
            _nudPrice.Enabled = !isCancelled;
            _btnAddItem.Enabled = !isCancelled;
            _btnRemoveItem.Enabled = !isCancelled;
            if (isCancelled)
            {
                _items.Clear();
                RefreshGrid();
            }
        }

        private void BtnOk_Click(object? sender, EventArgs e)
        {
            string status = _cmbStatus.SelectedItem?.ToString() ?? "Завершена";
            if (status == "Завершена" && _items.Count == 0)
            {
                MessageBox.Show("Добавьте хотя бы один товар в продажу!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CmbProduct_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_cmbProduct.SelectedItem is DataRowView drv)
            {
                try
                {
                    _nudPrice.Value = Convert.ToDecimal(drv["SalePrice"]);
                }
                catch { }
            }
        }

        private void BtnAddItem_Click(object? sender, EventArgs e)
        {
            if (_cmbProduct.SelectedItem is not DataRowView drv)
            {
                MessageBox.Show("Выберите товар из списка.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int productId;
            try { productId = Convert.ToInt32(drv["Id"]); }
            catch { return; }

            if (productId <= 0) return;

            int stock = Convert.ToInt32(drv["StockQuantity"]);
            string productName = drv["DisplayName"].ToString() ?? "";
            int qty = (int)_nudQuantity.Value;
            decimal price = _nudPrice.Value;

            if (qty <= 0) return;
            if (stock <= 0)
            {
                MessageBox.Show("Этот товар отсутствует на складе!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var existing = _items.FirstOrDefault(i => i.ProductId == productId);
            if (existing != null)
            {
                int newQty = existing.Quantity + qty;
                if (newQty > stock)
                {
                    MessageBox.Show($"Недостаточно товара на складе! В наличии: {stock}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                existing.Quantity = newQty;
            }
            else
            {
                if (qty > stock)
                {
                    MessageBox.Show($"Недостаточно товара на складе! В наличии: {stock}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                _items.Add(new SaleItem
                {
                    ProductId = productId,
                    ProductName = productName,
                    Quantity = qty,
                    Price = price
                });
            }
            RefreshGrid();
        }

        private void BtnRemoveItem_Click(object? sender, EventArgs e)
        {
            if (_dgvItems.CurrentRow == null) return;
            int idx = _dgvItems.CurrentRow.Index;
            if (idx >= 0 && idx < _items.Count)
            {
                _items.RemoveAt(idx);
                RefreshGrid();
            }
        }

        private void RefreshGrid()
        {
            _dgvItems.Rows.Clear();
            decimal total = 0;
            foreach (var item in _items)
            {
                _dgvItems.Rows.Add(
                    item.ProductId,
                    item.ProductName,
                    item.Quantity,
                    item.Price.ToString("F2"),
                    item.Total.ToString("F2")
                );
                total += item.Total;
            }
            _lblTotal.Text = $"ИТОГО: {total:F2} руб.";
        }
    }
    #endregion
}