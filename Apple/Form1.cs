using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Apple
{
    public partial class Form1 : Form
    {
        private string connectionString = "Server=localhost;Database=iStoreDB;Trusted_Connection=True;TrustServerCertificate=True;";
        private TabControl tabControl;
        private DataGridView dgvProducts, dgvSuppliers, dgvPurchases, dgvSales, dgvCategories, dgvReports;

        // Цветовая палитра
        private readonly Color PrimaryColor = Color.FromArgb(0, 120, 215);
        private readonly Color DangerColor = Color.FromArgb(220, 53, 69);
        private readonly Color SecondaryColor = Color.FromArgb(108, 117, 125);
        private readonly Color SuccessColor = Color.FromArgb(40, 167, 69); // Зеленый для экспорта/печати
        private readonly Color BgColor = Color.White;
        private readonly Color HeaderBgColor = Color.FromArgb(245, 247, 250);

        public Form1()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Text = "iStore - Управление магазином Apple";
            this.Size = new Size(1250, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = BgColor;
            this.Font = new Font("Segoe UI", 10F);

            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Padding = new Point(10, 6)
            };

            TabPage tabProducts = new TabPage("📱 Товары");
            InitializeProductsTab(tabProducts);
            tabControl.TabPages.Add(tabProducts);

            TabPage tabCategories = new TabPage("📂 Категории");
            InitializeCategoriesTab(tabCategories);
            tabControl.TabPages.Add(tabCategories);

            TabPage tabSuppliers = new TabPage("🚚 Поставщики");
            InitializeSuppliersTab(tabSuppliers);
            tabControl.TabPages.Add(tabSuppliers);

            TabPage tabPurchases = new TabPage("📥 Закупки");
            InitializePurchasesTab(tabPurchases);
            tabControl.TabPages.Add(tabPurchases);

            TabPage tabSales = new TabPage("📤 Продажи");
            InitializeSalesTab(tabSales);
            tabControl.TabPages.Add(tabSales);

            TabPage tabReports = new TabPage("📊 Отчеты");
            InitializeReportsTab(tabReports);
            tabControl.TabPages.Add(tabReports);

            this.Controls.Add(tabControl);
        }

        #region Вспомогательные методы UI
        private Button CreateButton(string text, Color backColor, int width)
        {
            var btn = new Button
            {
                Text = text,
                Width = width,
                Height = 38,
                FlatStyle = FlatStyle.Flat,
                BackColor = backColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 12, 0)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Light(backColor);
            btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(backColor);
            return btn;
        }

        private void StyleDataGridView(DataGridView dgv)
        {
            dgv.Dock = DockStyle.Fill;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.RowTemplate.Height = 34;
            dgv.RowHeadersVisible = false;
            dgv.BackgroundColor = BgColor;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(230, 230, 230);

            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgv.DefaultCellStyle.SelectionBackColor = PrimaryColor;
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);

            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = HeaderBgColor;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(50, 50, 50);
            dgv.ColumnHeadersHeight = 42;
            dgv.EnableHeadersVisualStyles = false;

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private FlowLayoutPanel CreateToolbar()
        {
            return new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 65,
                Padding = new Padding(15, 15, 15, 10),
                BackColor = BgColor,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight
            };
        }

        private Form CreateDialogForm(string title, int width, int height, out TableLayoutPanel tlp, out FlowLayoutPanel bottomPanel)
        {
            var form = new Form
            {
                Text = title,
                Size = new Size(width, height),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = BgColor,
                Font = new Font("Segoe UI", 10F),
                Padding = new Padding(30)
            };

            tlp = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                ColumnStyles = {
                    new ColumnStyle(SizeType.Absolute, 130F),
                    new ColumnStyle(SizeType.Percent, 100F)
                },
                BackColor = BgColor,
                Padding = new Padding(0, 0, 0, 20)
            };

            bottomPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                FlowDirection = FlowDirection.RightToLeft,
                BackColor = BgColor,
                Padding = new Padding(0, 15, 0, 0)
            };

            return form;
        }

        private void AddFormRow(TableLayoutPanel tlp, string labelText, Control inputControl, int row)
        {
            if (tlp.RowCount <= row)
            {
                tlp.RowCount++;
                tlp.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }

            var lbl = new Label
            {
                Text = labelText,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.FromArgb(80, 80, 80),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                Margin = new Padding(0, 10, 15, 10),
                AutoSize = true
            };

            inputControl.Font = new Font("Segoe UI", 10F);
            if (inputControl is TextBox tb)
            {
                tb.BorderStyle = BorderStyle.FixedSingle;
                tb.Height = tb.Multiline ? 80 : 36;
            }
            else if (inputControl is ComboBox cb)
            {
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.Height = 36;
            }
            else if (inputControl is DateTimePicker dtp)
            {
                dtp.Height = 36;
            }

            inputControl.Margin = new Padding(0, 10, 0, 10);

            tlp.Controls.Add(lbl, 0, row);
            tlp.Controls.Add(inputControl, 1, row);
        }
        #endregion

        #region Категории
        private void InitializeCategoriesTab(TabPage tab)
        {
            var panel = CreateToolbar();

            var btnAdd = CreateButton("➕ Добавить", PrimaryColor, 160);
            var btnEdit = CreateButton("✏️ Изменить", SecondaryColor, 140);
            var btnDelete = CreateButton("🗑️ Удалить", DangerColor, 120);
            var btnRefresh = CreateButton("🔄 Обновить", SecondaryColor, 120);

            btnAdd.Click += BtnAddCategory_Click;
            btnEdit.Click += BtnEditCategory_Click;
            btnDelete.Click += BtnDeleteCategory_Click;
            btnRefresh.Click += (s, e) => LoadCategoriesData();

            panel.Controls.Add(btnAdd);
            panel.Controls.Add(btnEdit);
            panel.Controls.Add(btnDelete);
            panel.Controls.Add(btnRefresh);

            dgvCategories = new DataGridView();
            StyleDataGridView(dgvCategories);

            tab.Controls.Add(dgvCategories);
            tab.Controls.Add(panel);

            LoadCategoriesData();
        }

        private void LoadCategoriesData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT c.CategoryID, c.CategoryName, c.Description,
                                    COUNT(p.ProductID) as ProductsCount
                                    FROM Categories c
                                    LEFT JOIN Products p ON c.CategoryID = p.CategoryID
                                    GROUP BY c.CategoryID, c.CategoryName, c.Description
                                    ORDER BY c.CategoryName";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvCategories.DataSource = dt;

                    if (dgvCategories.Columns["CategoryID"] != null) dgvCategories.Columns["CategoryID"].HeaderText = "ID";
                    if (dgvCategories.Columns["CategoryName"] != null) dgvCategories.Columns["CategoryName"].HeaderText = "Название категории";
                    if (dgvCategories.Columns["Description"] != null) dgvCategories.Columns["Description"].HeaderText = "Описание";
                    if (dgvCategories.Columns["ProductsCount"] != null) dgvCategories.Columns["ProductsCount"].HeaderText = "Кол-во товаров";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAddCategory_Click(object sender, EventArgs e)
        {
            var form = CreateDialogForm("Добавить категорию", 450, 300, out var tlp, out var bottomPanel);

            var txtName = new TextBox();
            var txtDesc = new TextBox { Multiline = true, ScrollBars = ScrollBars.Vertical };

            AddFormRow(tlp, "Название:", txtName, 0);
            AddFormRow(tlp, "Описание:", txtDesc, 1);

            var btnSave = CreateButton("💾 Сохранить", PrimaryColor, 110);
            var btnCancel = CreateButton("❌ Отмена", SecondaryColor, 110);

            btnCancel.Click += (s, ev) => form.Close();
            btnSave.Click += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Введите название категории!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = @"INSERT INTO Categories (CategoryName, Description) VALUES (@Name, @Desc)";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Desc", string.IsNullOrWhiteSpace(txtDesc.Text) ? DBNull.Value : (object)txtDesc.Text.Trim());

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Категория успешно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCategoriesData();
                        form.Close();
                    }
                }
                catch (SqlException sqlEx) when (sqlEx.Number == 2627 || sqlEx.Number == 2601)
                {
                    MessageBox.Show("Категория с таким названием уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            bottomPanel.Controls.Add(btnCancel);
            bottomPanel.Controls.Add(btnSave);
            form.Controls.Add(tlp);
            form.Controls.Add(bottomPanel);
            form.ShowDialog();
        }

        private void BtnEditCategory_Click(object sender, EventArgs e)
        {
            if (dgvCategories.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите категорию для редактирования!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int categoryId = Convert.ToInt32(dgvCategories.SelectedRows[0].Cells["CategoryID"].Value);
            string currentName = dgvCategories.SelectedRows[0].Cells["CategoryName"].Value.ToString();
            string currentDesc = dgvCategories.SelectedRows[0].Cells["Description"].Value?.ToString() ?? "";

            var form = CreateDialogForm("Редактировать категорию", 450, 300, out var tlp, out var bottomPanel);

            var txtName = new TextBox { Text = currentName };
            var txtDesc = new TextBox { Multiline = true, ScrollBars = ScrollBars.Vertical, Text = currentDesc };

            AddFormRow(tlp, "Название:", txtName, 0);
            AddFormRow(tlp, "Описание:", txtDesc, 1);

            var btnSave = CreateButton("💾 Сохранить", PrimaryColor, 110);
            var btnCancel = CreateButton("❌ Отмена", SecondaryColor, 110);

            btnCancel.Click += (s, ev) => form.Close();
            btnSave.Click += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Введите название категории!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = @"UPDATE Categories SET CategoryName = @Name, Description = @Desc WHERE CategoryID = @ID";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ID", categoryId);
                        cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Desc", string.IsNullOrWhiteSpace(txtDesc.Text) ? DBNull.Value : (object)txtDesc.Text.Trim());

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Категория успешно обновлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCategoriesData();
                        form.Close();
                    }
                }
                catch (SqlException sqlEx) when (sqlEx.Number == 2627 || sqlEx.Number == 2601)
                {
                    MessageBox.Show("Категория с таким названием уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            bottomPanel.Controls.Add(btnCancel);
            bottomPanel.Controls.Add(btnSave);
            form.Controls.Add(tlp);
            form.Controls.Add(bottomPanel);
            form.ShowDialog();
        }

        private void BtnDeleteCategory_Click(object sender, EventArgs e)
        {
            if (dgvCategories.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите категорию для удаления!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int categoryId = Convert.ToInt32(dgvCategories.SelectedRows[0].Cells["CategoryID"].Value);
            string categoryName = dgvCategories.SelectedRows[0].Cells["CategoryName"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"Вы уверены, что хотите удалить категорию '{categoryName}'?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string checkQuery = "SELECT COUNT(*) FROM Products WHERE CategoryID = @ID";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@ID", categoryId);
                    conn.Open();
                    int productsCount = (int)checkCmd.ExecuteScalar();

                    if (productsCount > 0)
                    {
                        MessageBox.Show($"Нельзя удалить категорию! В ней находится {productsCount} товаров. Сначала переместите или удалите товары.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string query = "DELETE FROM Categories WHERE CategoryID = @ID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", categoryId);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Категория успешно удалена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCategoriesData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Товары
        private void InitializeProductsTab(TabPage tab)
        {
            var panel = CreateToolbar();

            var btnAdd = CreateButton("➕ Добавить товар", PrimaryColor, 160);
            var btnRefresh = CreateButton("🔄 Обновить", SecondaryColor, 120);

            btnAdd.Click += BtnAddProduct_Click;
            btnRefresh.Click += (s, e) => LoadProducts();

            panel.Controls.Add(btnAdd);
            panel.Controls.Add(btnRefresh);

            dgvProducts = new DataGridView();
            StyleDataGridView(dgvProducts);

            tab.Controls.Add(dgvProducts);
            tab.Controls.Add(panel);

            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT p.ProductID, p.ModelName, c.CategoryName, p.Description, 
                                   p.BasePrice, p.StockQuantity 
                                   FROM Products p 
                                   LEFT JOIN Categories c ON p.CategoryID = c.CategoryID";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvProducts.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAddProduct_Click(object sender, EventArgs e)
        {
            var form = CreateDialogForm("Добавить товар", 450, 420, out var tlp, out var bottomPanel);

            var txtModel = new TextBox();
            var cmbCategory = new ComboBox();
            LoadCategories(cmbCategory);
            var txtDesc = new TextBox { Multiline = true, ScrollBars = ScrollBars.Vertical };
            var txtPrice = new TextBox();
            var txtStock = new TextBox { Text = "0" };

            AddFormRow(tlp, "Модель:", txtModel, 0);
            AddFormRow(tlp, "Категория:", cmbCategory, 1);
            AddFormRow(tlp, "Описание:", txtDesc, 2);
            AddFormRow(tlp, "Цена:", txtPrice, 3);
            AddFormRow(tlp, "Остаток:", txtStock, 4);

            var btnSave = CreateButton("💾 Сохранить", PrimaryColor, 110);
            var btnCancel = CreateButton("❌ Отмена", SecondaryColor, 110);

            btnCancel.Click += (s, ev) => form.Close();
            btnSave.Click += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(txtModel.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
                {
                    MessageBox.Show("Заполните обязательные поля (Модель и Цена)!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = @"INSERT INTO Products (ModelName, CategoryID, Description, BasePrice, StockQuantity) 
                                       VALUES (@Model, @Category, @Desc, @Price, @Stock)";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Model", txtModel.Text.Trim());
                        cmd.Parameters.AddWithValue("@Category", cmbCategory.SelectedValue ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Desc", txtDesc.Text.Trim());
                        cmd.Parameters.AddWithValue("@Price", decimal.Parse(txtPrice.Text));
                        cmd.Parameters.AddWithValue("@Stock", int.Parse(txtStock.Text));

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Товар успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadProducts();
                        form.Close();
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Цена и Остаток должны быть числами!", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            bottomPanel.Controls.Add(btnCancel);
            bottomPanel.Controls.Add(btnSave);
            form.Controls.Add(tlp);
            form.Controls.Add(bottomPanel);
            form.ShowDialog();
        }

        private void LoadCategories(ComboBox cmb)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT CategoryID, CategoryName FROM Categories";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    var emptyRow = dt.NewRow();
                    emptyRow["CategoryID"] = DBNull.Value;
                    emptyRow["CategoryName"] = "-- Не выбрано --";
                    dt.Rows.InsertAt(emptyRow, 0);

                    cmb.DataSource = dt;
                    cmb.DisplayMember = "CategoryName";
                    cmb.ValueMember = "CategoryID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Поставщики
        private void InitializeSuppliersTab(TabPage tab)
        {
            var panel = CreateToolbar();

            var btnAdd = CreateButton("➕ Добавить поставщика", PrimaryColor, 180);
            var btnRefresh = CreateButton("🔄 Обновить", SecondaryColor, 120);

            btnAdd.Click += BtnAddSupplier_Click;
            btnRefresh.Click += (s, e) => LoadSuppliers();

            panel.Controls.Add(btnAdd);
            panel.Controls.Add(btnRefresh);

            dgvSuppliers = new DataGridView();
            StyleDataGridView(dgvSuppliers);

            tab.Controls.Add(dgvSuppliers);
            tab.Controls.Add(panel);

            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT SupplierID, SupplierName, ContactName, Phone, Email, Address FROM Suppliers";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvSuppliers.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки поставщиков: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAddSupplier_Click(object sender, EventArgs e)
        {
            var form = CreateDialogForm("Добавить поставщика", 500, 450, out var tlp, out var bottomPanel);

            var txtName = new TextBox();
            var txtContact = new TextBox();
            var txtPhone = new TextBox();
            var txtEmail = new TextBox();
            var txtAddress = new TextBox { Multiline = true, ScrollBars = ScrollBars.Vertical };

            AddFormRow(tlp, "Название:", txtName, 0);
            AddFormRow(tlp, "Контакт:", txtContact, 1);
            AddFormRow(tlp, "Телефон:", txtPhone, 2);
            AddFormRow(tlp, "Email:", txtEmail, 3);
            AddFormRow(tlp, "Адрес:", txtAddress, 4);

            var btnSave = CreateButton("💾 Сохранить", PrimaryColor, 110);
            var btnCancel = CreateButton("❌ Отмена", SecondaryColor, 110);

            btnCancel.Click += (s, ev) => form.Close();
            btnSave.Click += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Заполните название поставщика!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = @"INSERT INTO Suppliers (SupplierName, ContactName, Phone, Email, Address) 
                                       VALUES (@Name, @Contact, @Phone, @Email, @Address)";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Contact", txtContact.Text.Trim());
                        cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Поставщик успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadSuppliers();
                        form.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            bottomPanel.Controls.Add(btnCancel);
            bottomPanel.Controls.Add(btnSave);
            form.Controls.Add(tlp);
            form.Controls.Add(bottomPanel);
            form.ShowDialog();
        }
        #endregion

        #region Закупки
        private void InitializePurchasesTab(TabPage tab)
        {
            var panel = CreateToolbar();

            var btnAdd = CreateButton("➕ Новая закупка", PrimaryColor, 160);
            var btnRefresh = CreateButton("🔄 Обновить", SecondaryColor, 120);

            btnAdd.Click += BtnAddPurchase_Click;
            btnRefresh.Click += (s, e) => LoadPurchases();

            panel.Controls.Add(btnAdd);
            panel.Controls.Add(btnRefresh);

            dgvPurchases = new DataGridView();
            StyleDataGridView(dgvPurchases);

            tab.Controls.Add(dgvPurchases);
            tab.Controls.Add(panel);

            LoadPurchases();
        }

        private void LoadPurchases()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT PurchaseID, ModelName, SupplierName, PurchaseDate, Quantity, UnitCost, TotalCost FROM vw_PurchaseReport ORDER BY PurchaseDate DESC";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvPurchases.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки закупок: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAddPurchase_Click(object sender, EventArgs e)
        {
            var form = CreateDialogForm("Новая закупка", 450, 400, out var tlp, out var bottomPanel);

            var cmbProduct = new ComboBox();
            LoadProductsCombo(cmbProduct);
            var cmbSupplier = new ComboBox();
            LoadSuppliersCombo(cmbSupplier);
            var dtpDate = new DateTimePicker { Format = DateTimePickerFormat.Short };
            var txtQty = new TextBox();
            var txtCost = new TextBox();

            AddFormRow(tlp, "Товар:", cmbProduct, 0);
            AddFormRow(tlp, "Поставщик:", cmbSupplier, 1);
            AddFormRow(tlp, "Дата:", dtpDate, 2);
            AddFormRow(tlp, "Количество:", txtQty, 3);
            AddFormRow(tlp, "Цена за ед.:", txtCost, 4);

            var btnSave = CreateButton("💾 Сохранить", PrimaryColor, 110);
            var btnCancel = CreateButton("❌ Отмена", SecondaryColor, 110);

            btnCancel.Click += (s, ev) => form.Close();
            btnSave.Click += (s, ev) =>
            {
                if (cmbProduct.SelectedValue == null || cmbSupplier.SelectedValue == null ||
                    string.IsNullOrWhiteSpace(txtQty.Text) || string.IsNullOrWhiteSpace(txtCost.Text))
                {
                    MessageBox.Show("Заполните все поля!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = @"INSERT INTO Purchases (ProductID, SupplierID, PurchaseDate, Quantity, UnitCost) 
                                       VALUES (@Product, @Supplier, @Date, @Qty, @Cost)";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Product", cmbProduct.SelectedValue);
                        cmd.Parameters.AddWithValue("@Supplier", cmbSupplier.SelectedValue);
                        cmd.Parameters.AddWithValue("@Date", dtpDate.Value);
                        cmd.Parameters.AddWithValue("@Qty", int.Parse(txtQty.Text));
                        cmd.Parameters.AddWithValue("@Cost", decimal.Parse(txtCost.Text));

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Закупка успешно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadPurchases();
                        LoadProducts(); // Обновляем остатки
                        form.Close();
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Количество и Цена должны быть числами!", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            bottomPanel.Controls.Add(btnCancel);
            bottomPanel.Controls.Add(btnSave);
            form.Controls.Add(tlp);
            form.Controls.Add(bottomPanel);
            form.ShowDialog();
        }

        private void LoadProductsCombo(ComboBox cmb)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT ProductID, ModelName FROM Products";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    cmb.DataSource = dt;
                    cmb.DisplayMember = "ModelName";
                    cmb.ValueMember = "ProductID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSuppliersCombo(ComboBox cmb)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT SupplierID, SupplierName FROM Suppliers";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    cmb.DataSource = dt;
                    cmb.DisplayMember = "SupplierName";
                    cmb.ValueMember = "SupplierID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Продажи
        private void InitializeSalesTab(TabPage tab)
        {
            var panel = CreateToolbar();

            var btnAdd = CreateButton("➕ Новая продажа", PrimaryColor, 160);
            var btnRefresh = CreateButton("🔄 Обновить", SecondaryColor, 120);
            var btnPrintCheck = CreateButton("🖨️ Печать чека (.txt)", SuccessColor, 180);

            btnAdd.Click += BtnAddSale_Click;
            btnRefresh.Click += (s, e) => LoadSales();
            btnPrintCheck.Click += BtnPrintReceipt_Click;

            panel.Controls.Add(btnAdd);
            panel.Controls.Add(btnRefresh);
            panel.Controls.Add(btnPrintCheck);

            dgvSales = new DataGridView();
            StyleDataGridView(dgvSales);

            tab.Controls.Add(dgvSales);
            tab.Controls.Add(panel);

            LoadSales();
        }

        private void LoadSales()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT SaleID, ModelName, SaleDate, Quantity, UnitPrice, TotalPrice, CustomerName, CustomerPhone FROM vw_SalesReport ORDER BY SaleDate DESC";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvSales.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки продаж: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAddSale_Click(object sender, EventArgs e)
        {
            var form = CreateDialogForm("Новая продажа", 450, 450, out var tlp, out var bottomPanel);

            var cmbProduct = new ComboBox();
            LoadProductsCombo(cmbProduct);
            var dtpDate = new DateTimePicker { Format = DateTimePickerFormat.Short };
            var txtQty = new TextBox();
            var txtPrice = new TextBox();
            var txtCustomer = new TextBox();
            var txtPhone = new TextBox();

            AddFormRow(tlp, "Товар:", cmbProduct, 0);
            AddFormRow(tlp, "Дата:", dtpDate, 1);
            AddFormRow(tlp, "Количество:", txtQty, 2);
            AddFormRow(tlp, "Цена за ед.:", txtPrice, 3);
            AddFormRow(tlp, "Клиент:", txtCustomer, 4);
            AddFormRow(tlp, "Телефон:", txtPhone, 5);

            var btnSave = CreateButton("💾 Сохранить", PrimaryColor, 110);
            var btnCancel = CreateButton("❌ Отмена", SecondaryColor, 110);

            btnCancel.Click += (s, ev) => form.Close();
            btnSave.Click += (s, ev) =>
            {
                if (cmbProduct.SelectedValue == null || string.IsNullOrWhiteSpace(txtQty.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
                {
                    MessageBox.Show("Заполните обязательные поля!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string checkQuery = "SELECT StockQuantity FROM Products WHERE ProductID = @ProductID";
                        SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                        checkCmd.Parameters.AddWithValue("@ProductID", cmbProduct.SelectedValue);
                        conn.Open();
                        int stock = Convert.ToInt32(checkCmd.ExecuteScalar());
                        int requestedQty = int.Parse(txtQty.Text);

                        if (stock < requestedQty)
                        {
                            MessageBox.Show($"Недостаточно товара на складе! Текущий остаток: {stock} шт.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        string query = @"INSERT INTO Sales (ProductID, SaleDate, Quantity, UnitPrice, CustomerName, CustomerPhone) 
                                       VALUES (@Product, @Date, @Qty, @Price, @Customer, @Phone)";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Product", cmbProduct.SelectedValue);
                        cmd.Parameters.AddWithValue("@Date", dtpDate.Value);
                        cmd.Parameters.AddWithValue("@Qty", requestedQty);
                        cmd.Parameters.AddWithValue("@Price", decimal.Parse(txtPrice.Text));
                        cmd.Parameters.AddWithValue("@Customer", txtCustomer.Text.Trim());
                        cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Продажа успешно оформлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadSales();
                        LoadProducts(); // Обновляем остатки
                        form.Close();
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Количество и Цена должны быть числами!", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            bottomPanel.Controls.Add(btnCancel);
            bottomPanel.Controls.Add(btnSave);
            form.Controls.Add(tlp);
            form.Controls.Add(bottomPanel);
            form.ShowDialog();
        }

        // НОВЫЙ МЕТОД: Печать чека в .txt
        private void BtnPrintReceipt_Click(object sender, EventArgs e)
        {
            if (dgvSales.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите продажу в таблице для печати чека!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int saleId = Convert.ToInt32(dgvSales.SelectedRows[0].Cells["SaleID"].Value);

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT ModelName, SaleDate, Quantity, UnitPrice, TotalPrice, CustomerName, CustomerPhone 
                                     FROM vw_SalesReport WHERE SaleID = @SaleID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SaleID", saleId);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            MessageBox.Show("Данные о продаже не найдены.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        string modelName = reader["ModelName"].ToString();
                        DateTime saleDate = Convert.ToDateTime(reader["SaleDate"]);
                        int qty = Convert.ToInt32(reader["Quantity"]);
                        decimal price = Convert.ToDecimal(reader["UnitPrice"]);
                        decimal total = Convert.ToDecimal(reader["TotalPrice"]);
                        string customer = string.IsNullOrWhiteSpace(reader["CustomerName"].ToString()) ? "Розничный покупатель" : reader["CustomerName"].ToString();
                        string phone = reader["CustomerPhone"].ToString();

                        StringBuilder receipt = new StringBuilder();
                        receipt.AppendLine("========================================");
                        receipt.AppendLine("           iStore - Apple Shop          ");
                        receipt.AppendLine("========================================");
                        receipt.AppendLine($"Дата:  {saleDate:dd.MM.yyyy HH:mm}");
                        receipt.AppendLine($"Клиент: {customer}");
                        if (!string.IsNullOrWhiteSpace(phone)) receipt.AppendLine($"Тел.:  {phone}");
                        receipt.AppendLine("----------------------------------------");
                        receipt.AppendLine("Наименование       Кол.   Цена      Сумма");
                        receipt.AppendLine("----------------------------------------");

                        // Форматирование под ширину чека
                        string itemLine = $"{modelName,-18} {qty,-4} {price,-8:N2} {total,-8:N2}";
                        if (itemLine.Length > 40) itemLine = itemLine.Substring(0, 40);

                        receipt.AppendLine(itemLine);
                        receipt.AppendLine("----------------------------------------");
                        receipt.AppendLine($"ИТОГО К ОПЛАТЕ:       {total,12:N2} ₽");
                        receipt.AppendLine("========================================");
                        receipt.AppendLine("   Спасибо за покупку! Ждем вас снова.  ");
                        receipt.AppendLine("========================================");

                        using (SaveFileDialog sfd = new SaveFileDialog())
                        {
                            sfd.Filter = "Текстовые файлы (*.txt)|*.txt";
                            sfd.FileName = $"Check_{saleId}_{saleDate:yyyyMMdd_HHmm}.txt";
                            sfd.Title = "Сохранить чек";

                            if (sfd.ShowDialog() == DialogResult.OK)
                            {
                                File.WriteAllText(sfd.FileName, receipt.ToString(), Encoding.UTF8);
                                MessageBox.Show($"Чек успешно сохранен в:\n{sfd.FileName}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                if (MessageBox.Show("Открыть чек сейчас?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    System.Diagnostics.Process.Start("notepad.exe", sfd.FileName);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при формировании чека: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Отчеты (Табличный вид + Экспорт)
        private void InitializeReportsTab(TabPage tab)
        {
            var panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 70,
                Padding = new Padding(15, 15, 15, 10),
                BackColor = BgColor,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight
            };

            var btnStock = CreateButton("📦 Остатки", PrimaryColor, 150);
            var btnSales = CreateButton("💰 Продажи", PrimaryColor, 150);
            var btnPurchase = CreateButton("📥 Закупки", PrimaryColor, 150);
            var btnProfit = CreateButton("📈 Прибыль", PrimaryColor, 150);

            // Кнопка экспорта в Excel
            var btnExport = CreateButton("💾 Экспорт в Excel (.csv)", SuccessColor, 220);

            btnStock.Click += BtnStockReport_Click;
            btnSales.Click += BtnSalesReport_Click;
            btnPurchase.Click += BtnPurchaseReport_Click;
            btnProfit.Click += BtnProfitReport_Click;
            btnExport.Click += BtnExportToExcel_Click;

            panel.Controls.Add(btnStock);
            panel.Controls.Add(btnSales);
            panel.Controls.Add(btnPurchase);
            panel.Controls.Add(btnProfit);
            panel.Controls.Add(btnExport);

            dgvReports = new DataGridView();
            StyleDataGridView(dgvReports);

            tab.Controls.Add(dgvReports);
            tab.Controls.Add(panel);
        }

        private void BtnStockReport_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT ModelName AS 'Модель', StockQuantity AS 'Остаток (шт)', BasePrice AS 'Цена (₽)', 
                                   (StockQuantity * BasePrice) AS 'Общая стоимость (₽)'
                                   FROM Products ORDER BY StockQuantity DESC";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvReports.DataSource = dt;
                }
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void BtnSalesReport_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT CONVERT(varchar, SaleDate, 104) AS 'Дата', ModelName AS 'Товар', 
                                   Quantity AS 'Кол-во', UnitPrice AS 'Цена (₽)', TotalPrice AS 'Сумма (₽)', CustomerName AS 'Клиент'
                                   FROM vw_SalesReport ORDER BY SaleDate DESC";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvReports.DataSource = dt;
                }
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void BtnPurchaseReport_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT CONVERT(varchar, PurchaseDate, 104) AS 'Дата', ModelName AS 'Товар', SupplierName AS 'Поставщик', 
                                   Quantity AS 'Кол-во', UnitCost AS 'Цена (₽)', TotalCost AS 'Сумма (₽)'
                                   FROM vw_PurchaseReport ORDER BY PurchaseDate DESC";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvReports.DataSource = dt;
                }
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void BtnProfitReport_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    decimal totalSales = Convert.ToDecimal(new SqlCommand("SELECT ISNULL(SUM(TotalPrice), 0) FROM Sales", conn).ExecuteScalar());
                    decimal totalPurchases = Convert.ToDecimal(new SqlCommand("SELECT ISNULL(SUM(TotalCost), 0) FROM Purchases", conn).ExecuteScalar());
                    decimal profit = totalSales - totalPurchases;

                    // Создаем таблицу вручную для красивого вывода сводных данных
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Показатель", typeof(string));
                    dt.Columns.Add("Значение (₽)", typeof(string));

                    dt.Rows.Add("💰 Общая сумма продаж", totalSales.ToString("N2"));
                    dt.Rows.Add("📥 Общая сумма закупок", totalPurchases.ToString("N2"));
                    dt.Rows.Add("═══════════════════════", "═══════════════");
                    dt.Rows.Add("📊 ЧИСТАЯ ПРИБЫЛЬ", profit.ToString("N2"));

                    string status = profit > 0 ? "✅ Работаем в плюс!" : (profit < 0 ? "⚠️ Работаем в убыток!" : "⚖️ В ноль");
                    dt.Rows.Add("📈 Статус", status);

                    dgvReports.DataSource = dt;

                    // Для отчета о прибыли отключаем автоподгонку, чтобы таблица выглядела аккуратно
                    dgvReports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                    dgvReports.Columns[0].Width = 300;
                    dgvReports.Columns[1].Width = 200;
                    dgvReports.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }   

        // НОВЫЙ МЕТОД: Экспорт отчета в CSV (для Excel)
        private void BtnExportToExcel_Click(object sender, EventArgs e)
        {
            if (dgvReports.DataSource == null || ((DataTable)dgvReports.DataSource).Rows.Count == 0)
            {
                MessageBox.Show("Нет данных для экспорта. Сначала сформируйте отчет.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Файлы Excel (CSV) (*.csv)|*.csv";
                sfd.FileName = $"Otchet_{DateTime.Now:yyyyMMdd_HHmm}.csv";
                sfd.Title = "Экспорт отчета в Excel";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var sb = new StringBuilder();
                        DataTable dt = (DataTable)dgvReports.DataSource;

                        // Заголовки столбцов
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            sb.Append(dt.Columns[i].ColumnName);
                            if (i < dt.Columns.Count - 1) sb.Append(";"); // Разделитель для русской локали Excel
                        }
                        sb.AppendLine();

                        // Данные
                        foreach (DataRow row in dt.Rows)
                        {
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                // Заменяем переносы строк и точки с запятой, чтобы не сломать структуру CSV
                                string val = row[i].ToString().Replace(";", ",").Replace("\n", " ").Replace("\r", " ");
                                sb.Append(val);
                                if (i < dt.Columns.Count - 1) sb.Append(";");
                            }
                            sb.AppendLine();
                        }

                        // КРИТИЧНО: UTF8 с BOM (Byte Order Mark), чтобы Excel корректно открыл кириллицу
                        File.WriteAllText(sfd.FileName, sb.ToString(), new UTF8Encoding(true));
                        MessageBox.Show("Отчет успешно экспортирован!\nВы можете открыть этот файл в Microsoft Excel.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        #endregion
    }
}