using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace Apple
{
    public partial class Form1 : Form
    {
        private TabControl tabControl;
        private DataGridView dgvProducts, dgvSuppliers, dgvPurchases, dgvSales, dgvCategories, dgvReports;

        // Цветовая палитра
        private readonly Color PrimaryColor = Color.FromArgb(0, 120, 215);
        private readonly Color DangerColor = Color.FromArgb(220, 53, 69);
        private readonly Color SecondaryColor = Color.FromArgb(108, 117, 125);
        private readonly Color SuccessColor = Color.FromArgb(40, 167, 69);
        private readonly Color BgColor = Color.White;
        private readonly Color HeaderBgColor = Color.FromArgb(245, 247, 250);

        public Form1()
        {
            // Инициализируем БД при первом запуске
            iStoreDB.Initialize();

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

            var tabProducts = new TabPage("📱 Товары"); InitializeProductsTab(tabProducts); tabControl.TabPages.Add(tabProducts);
            var tabCategories = new TabPage("📂 Категории"); InitializeCategoriesTab(tabCategories); tabControl.TabPages.Add(tabCategories);
            var tabSuppliers = new TabPage("🚚 Поставщики"); InitializeSuppliersTab(tabSuppliers); tabControl.TabPages.Add(tabSuppliers);
            var tabPurchases = new TabPage("📥 Закупки"); InitializePurchasesTab(tabPurchases); tabControl.TabPages.Add(tabPurchases);
            var tabSales = new TabPage("📤 Продажи"); InitializeSalesTab(tabSales); tabControl.TabPages.Add(tabSales);
            var tabReports = new TabPage("📊 Отчеты"); InitializeReportsTab(tabReports); tabControl.TabPages.Add(tabReports);

            this.Controls.Add(tabControl);
        }

        #region Вспомогательные методы UI

        private DataTable ExecuteQuery(string sql, params (string name, object value)[] parameters)
        {
            using var conn = new SqliteConnection(iStoreDB.ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            foreach (var (name, value) in parameters)
                cmd.Parameters.AddWithValue(name, value ?? DBNull.Value);

            var dt = new DataTable();
            using var reader = cmd.ExecuteReader();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                Type colType = reader.GetFieldType(i);
                if (colType == null) colType = typeof(object);
                dt.Columns.Add(reader.GetName(i), colType);
            }

            while (reader.Read())
            {
                var row = dt.NewRow();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[i] = reader.IsDBNull(i) ? DBNull.Value : reader.GetValue(i);
                }
                dt.Rows.Add(row);
            }

            return dt;
        }

        private int ExecuteNonQuery(string sql, params (string name, object value)[] parameters)
        {
            using var conn = new SqliteConnection(iStoreDB.ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            foreach (var (name, value) in parameters)
                cmd.Parameters.AddWithValue(name, value ?? DBNull.Value);
            return cmd.ExecuteNonQuery();
        }

        private object ExecuteScalar(string sql, params (string name, object value)[] parameters)
        {
            using var conn = new SqliteConnection(iStoreDB.ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            foreach (var (name, value) in parameters)
                cmd.Parameters.AddWithValue(name, value ?? DBNull.Value);
            return cmd.ExecuteScalar();
        }

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
                string query = @"SELECT c.CategoryID, c.CategoryName, c.Description,
                                        COUNT(p.ProductID) AS ProductsCount
                                 FROM Categories c
                                 LEFT JOIN Products p ON c.CategoryID = p.CategoryID
                                 GROUP BY c.CategoryID, c.CategoryName, c.Description
                                 ORDER BY c.CategoryName";
                var dt = ExecuteQuery(query);
                dgvCategories.DataSource = dt;

                if (dgvCategories.Columns["CategoryID"] != null) dgvCategories.Columns["CategoryID"].HeaderText = "ID";
                if (dgvCategories.Columns["CategoryName"] != null) dgvCategories.Columns["CategoryName"].HeaderText = "Название категории";
                if (dgvCategories.Columns["Description"] != null) dgvCategories.Columns["Description"].HeaderText = "Описание";
                if (dgvCategories.Columns["ProductsCount"] != null) dgvCategories.Columns["ProductsCount"].HeaderText = "Кол-во товаров";
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
                    string sql = "INSERT INTO Categories (CategoryName, Description) VALUES (@Name, @Desc)";
                    ExecuteNonQuery(sql,
                        ("@Name", txtName.Text.Trim()),
                        ("@Desc", string.IsNullOrWhiteSpace(txtDesc.Text) ? DBNull.Value : (object)txtDesc.Text.Trim()));

                    MessageBox.Show("Категория успешно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCategoriesData();
                    form.Close();
                }
                catch (SqliteException sqlEx) when (sqlEx.SqliteErrorCode == 19 || sqlEx.SqliteErrorCode == 2067)
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
                    string sql = "UPDATE Categories SET CategoryName = @Name, Description = @Desc WHERE CategoryID = @ID";
                    ExecuteNonQuery(sql,
                        ("@ID", categoryId),
                        ("@Name", txtName.Text.Trim()),
                        ("@Desc", string.IsNullOrWhiteSpace(txtDesc.Text) ? DBNull.Value : (object)txtDesc.Text.Trim()));

                    MessageBox.Show("Категория успешно обновлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCategoriesData();
                    form.Close();
                }
                catch (SqliteException sqlEx) when (sqlEx.SqliteErrorCode == 19 || sqlEx.SqliteErrorCode == 2067)
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
                int productsCount = Convert.ToInt32(ExecuteScalar("SELECT COUNT(*) FROM Products WHERE CategoryID = @ID", ("@ID", categoryId)));

                if (productsCount > 0)
                {
                    MessageBox.Show($"Нельзя удалить категорию! В ней находится {productsCount} товаров. Сначала переместите или удалите товары.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ExecuteNonQuery("DELETE FROM Categories WHERE CategoryID = @ID", ("@ID", categoryId));

                MessageBox.Show("Категория успешно удалена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCategoriesData();
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
            var btnEdit = CreateButton("✏️ Изменить", SecondaryColor, 140);
            var btnDelete = CreateButton("🗑️ Удалить", DangerColor, 120);
            var btnRefresh = CreateButton("🔄 Обновить", SecondaryColor, 120);

            btnAdd.Click += BtnAddProduct_Click;
            btnEdit.Click += BtnEditProduct_Click;
            btnDelete.Click += BtnDeleteProduct_Click;
            btnRefresh.Click += (s, e) => LoadProducts();

            panel.Controls.Add(btnAdd);
            panel.Controls.Add(btnEdit);
            panel.Controls.Add(btnDelete);
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
                string query = @"SELECT p.ProductID, p.ModelName, c.CategoryName, p.Description, 
                                        p.BasePrice, p.StockQuantity 
                                 FROM Products p 
                                 LEFT JOIN Categories c ON p.CategoryID = c.CategoryID";
                dgvProducts.DataSource = ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAddProduct_Click(object sender, EventArgs e)
        {
            var form = CreateDialogForm("Добавить товар", 450, 450, out var tlp, out var bottomPanel);

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
                    string sql = @"INSERT INTO Products (ModelName, CategoryID, Description, BasePrice, StockQuantity) 
                                   VALUES (@Model, @Category, @Desc, @Price, @Stock)";
                    ExecuteNonQuery(sql,
                        ("@Model", txtModel.Text.Trim()),
                        ("@Category", cmbCategory.SelectedValue == null || cmbCategory.SelectedValue is DBNull ? (object)DBNull.Value : Convert.ToInt32(cmbCategory.SelectedValue)),
                        ("@Desc", txtDesc.Text.Trim()),
                        ("@Price", decimal.Parse(txtPrice.Text)),
                        ("@Stock", int.Parse(txtStock.Text)));

                    MessageBox.Show("Товар успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProducts();
                    form.Close();
                }
                catch (SqliteException sqlEx) when (sqlEx.SqliteErrorCode == 19 || sqlEx.SqliteErrorCode == 2067)
                {
                    MessageBox.Show("Товар с такой моделью уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void BtnEditProduct_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар для редактирования!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int productId = Convert.ToInt32(dgvProducts.SelectedRows[0].Cells["ProductID"].Value);
            string currentModel = dgvProducts.SelectedRows[0].Cells["ModelName"].Value.ToString();
            string currentDesc = dgvProducts.SelectedRows[0].Cells["Description"].Value?.ToString() ?? "";
            decimal currentPrice = Convert.ToDecimal(dgvProducts.SelectedRows[0].Cells["BasePrice"].Value);
            int currentStock = Convert.ToInt32(dgvProducts.SelectedRows[0].Cells["StockQuantity"].Value);

            // Получаем текущий CategoryID напрямую из БД (в гриде его нет)
            object currentCategoryIdObj = ExecuteScalar(
                "SELECT CategoryID FROM Products WHERE ProductID = @ID",
                ("@ID", productId));
            int currentCategoryId = currentCategoryIdObj == null || currentCategoryIdObj is DBNull
                ? 0
                : Convert.ToInt32(currentCategoryIdObj);

            var form = CreateDialogForm("Редактировать товар", 450, 450, out var tlp, out var bottomPanel);

            var txtModel = new TextBox { Text = currentModel };
            var cmbCategory = new ComboBox();
            LoadCategories(cmbCategory);
            if (currentCategoryId > 0)
            {
                cmbCategory.SelectedValue = currentCategoryId;
            }
            var txtDesc = new TextBox { Multiline = true, ScrollBars = ScrollBars.Vertical, Text = currentDesc };
            var txtPrice = new TextBox { Text = currentPrice.ToString("0.##") };
            var txtStock = new TextBox { Text = currentStock.ToString() };

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
                    string sql = @"UPDATE Products 
                                   SET ModelName = @Model, 
                                       CategoryID = @Category, 
                                       Description = @Desc, 
                                       BasePrice = @Price, 
                                       StockQuantity = @Stock 
                                   WHERE ProductID = @ID";
                    ExecuteNonQuery(sql,
                        ("@ID", productId),
                        ("@Model", txtModel.Text.Trim()),
                        ("@Category", cmbCategory.SelectedValue == null || cmbCategory.SelectedValue is DBNull ? (object)DBNull.Value : Convert.ToInt32(cmbCategory.SelectedValue)),
                        ("@Desc", txtDesc.Text.Trim()),
                        ("@Price", decimal.Parse(txtPrice.Text)),
                        ("@Stock", int.Parse(txtStock.Text)));

                    MessageBox.Show("Товар успешно обновлён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProducts();
                    form.Close();
                }
                catch (SqliteException sqlEx) when (sqlEx.SqliteErrorCode == 19 || sqlEx.SqliteErrorCode == 2067)
                {
                    MessageBox.Show("Товар с такой моделью уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void BtnDeleteProduct_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар для удаления!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int productId = Convert.ToInt32(dgvProducts.SelectedRows[0].Cells["ProductID"].Value);
            string modelName = dgvProducts.SelectedRows[0].Cells["ModelName"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"Вы уверены, что хотите удалить товар '{modelName}'?\n\n" +
                "Внимание: если с товаром связаны закупки или продажи, удаление может быть заблокировано!",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            try
            {
                int purchasesCount = Convert.ToInt32(ExecuteScalar("SELECT COUNT(*) FROM Purchases WHERE ProductID = @ID", ("@ID", productId)));
                int salesCount = Convert.ToInt32(ExecuteScalar("SELECT COUNT(*) FROM Sales WHERE ProductID = @ID", ("@ID", productId)));

                if (purchasesCount > 0 || salesCount > 0)
                {
                    MessageBox.Show(
                        $"Нельзя удалить товар!\nС ним связано: {purchasesCount} закупок и {salesCount} продаж.\n" +
                        "Сначала удалите связанные записи.",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ExecuteNonQuery("DELETE FROM Products WHERE ProductID = @ID", ("@ID", productId));
                MessageBox.Show("Товар успешно удалён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCategories(ComboBox cmb)
        {
            try
            {
                var dt = ExecuteQuery("SELECT CategoryID, CategoryName FROM Categories");

                var emptyRow = dt.NewRow();
                emptyRow["CategoryID"] = DBNull.Value;
                emptyRow["CategoryName"] = "-- Не выбрано --";
                dt.Rows.InsertAt(emptyRow, 0);

                cmb.DataSource = dt;
                cmb.DisplayMember = "CategoryName";
                cmb.ValueMember = "CategoryID";
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
            var btnEdit = CreateButton("✏️ Изменить", SecondaryColor, 140);
            var btnDelete = CreateButton("🗑️ Удалить", DangerColor, 120);
            var btnRefresh = CreateButton("🔄 Обновить", SecondaryColor, 120);

            btnAdd.Click += BtnAddSupplier_Click;
            btnEdit.Click += BtnEditSupplier_Click;
            btnDelete.Click += BtnDeleteSupplier_Click;
            btnRefresh.Click += (s, e) => LoadSuppliers();

            panel.Controls.Add(btnAdd);
            panel.Controls.Add(btnEdit);
            panel.Controls.Add(btnDelete);
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
                string query = "SELECT SupplierID, SupplierName, ContactName, Phone, Email, Address FROM Suppliers";
                dgvSuppliers.DataSource = ExecuteQuery(query);
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
                    string sql = @"INSERT INTO Suppliers (SupplierName, ContactName, Phone, Email, Address) 
                                   VALUES (@Name, @Contact, @Phone, @Email, @Address)";
                    ExecuteNonQuery(sql,
                        ("@Name", txtName.Text.Trim()),
                        ("@Contact", txtContact.Text.Trim()),
                        ("@Phone", txtPhone.Text.Trim()),
                        ("@Email", txtEmail.Text.Trim()),
                        ("@Address", txtAddress.Text.Trim()));

                    MessageBox.Show("Поставщик успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadSuppliers();
                    form.Close();
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

        private void BtnEditSupplier_Click(object sender, EventArgs e)
        {
            if (dgvSuppliers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите поставщика для редактирования!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int supplierId = Convert.ToInt32(dgvSuppliers.SelectedRows[0].Cells["SupplierID"].Value);
            string currentName = dgvSuppliers.SelectedRows[0].Cells["SupplierName"].Value?.ToString() ?? "";
            string currentContact = dgvSuppliers.SelectedRows[0].Cells["ContactName"].Value?.ToString() ?? "";
            string currentPhone = dgvSuppliers.SelectedRows[0].Cells["Phone"].Value?.ToString() ?? "";
            string currentEmail = dgvSuppliers.SelectedRows[0].Cells["Email"].Value?.ToString() ?? "";
            string currentAddress = dgvSuppliers.SelectedRows[0].Cells["Address"].Value?.ToString() ?? "";

            var form = CreateDialogForm("Редактировать поставщика", 500, 450, out var tlp, out var bottomPanel);

            var txtName = new TextBox { Text = currentName };
            var txtContact = new TextBox { Text = currentContact };
            var txtPhone = new TextBox { Text = currentPhone };
            var txtEmail = new TextBox { Text = currentEmail };
            var txtAddress = new TextBox { Multiline = true, ScrollBars = ScrollBars.Vertical, Text = currentAddress };

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
                    string sql = @"UPDATE Suppliers 
                                   SET SupplierName = @Name, 
                                       ContactName = @Contact, 
                                       Phone = @Phone, 
                                       Email = @Email, 
                                       Address = @Address 
                                   WHERE SupplierID = @ID";
                    ExecuteNonQuery(sql,
                        ("@ID", supplierId),
                        ("@Name", txtName.Text.Trim()),
                        ("@Contact", txtContact.Text.Trim()),
                        ("@Phone", txtPhone.Text.Trim()),
                        ("@Email", txtEmail.Text.Trim()),
                        ("@Address", txtAddress.Text.Trim()));

                    MessageBox.Show("Поставщик успешно обновлён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadSuppliers();
                    form.Close();
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

        private void BtnDeleteSupplier_Click(object sender, EventArgs e)
        {
            if (dgvSuppliers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите поставщика для удаления!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int supplierId = Convert.ToInt32(dgvSuppliers.SelectedRows[0].Cells["SupplierID"].Value);
            string supplierName = dgvSuppliers.SelectedRows[0].Cells["SupplierName"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"Вы уверены, что хотите удалить поставщика '{supplierName}'?\n\n" +
                "Внимание: если с поставщиком связаны закупки, удаление будет заблокировано!",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            try
            {
                int purchasesCount = Convert.ToInt32(ExecuteScalar("SELECT COUNT(*) FROM Purchases WHERE SupplierID = @ID", ("@ID", supplierId)));

                if (purchasesCount > 0)
                {
                    MessageBox.Show(
                        $"Нельзя удалить поставщика!\nС ним связано {purchasesCount} закупок.\n" +
                        "Сначала удалите связанные закупки.",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ExecuteNonQuery("DELETE FROM Suppliers WHERE SupplierID = @ID", ("@ID", supplierId));
                MessageBox.Show("Поставщик успешно удалён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadSuppliers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                string query = "SELECT PurchaseID, ModelName, SupplierName, PurchaseDate, Quantity, UnitCost, TotalCost FROM vw_PurchaseReport ORDER BY PurchaseDate DESC";
                dgvPurchases.DataSource = ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки закупок: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAddPurchase_Click(object sender, EventArgs e)
        {
            var form = CreateDialogForm("Новая закупка", 450, 400, out var tlp, out var bottomPanel);

            var cmbProduct = new ComboBox(); LoadProductsCombo(cmbProduct);
            var cmbSupplier = new ComboBox(); LoadSuppliersCombo(cmbSupplier);
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
                    string sql = @"INSERT INTO Purchases (ProductID, SupplierID, PurchaseDate, Quantity, UnitCost) 
                                   VALUES (@Product, @Supplier, @Date, @Qty, @Cost)";
                    ExecuteNonQuery(sql,
                        ("@Product", Convert.ToInt32(cmbProduct.SelectedValue)),
                        ("@Supplier", Convert.ToInt32(cmbSupplier.SelectedValue)),
                        ("@Date", dtpDate.Value.ToString("yyyy-MM-dd HH:mm:ss")),
                        ("@Qty", int.Parse(txtQty.Text)),
                        ("@Cost", decimal.Parse(txtCost.Text)));

                    MessageBox.Show("Закупка успешно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPurchases();
                    LoadProducts();
                    form.Close();
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
                var dt = ExecuteQuery("SELECT ProductID, ModelName FROM Products");
                cmb.DataSource = dt;
                cmb.DisplayMember = "ModelName";
                cmb.ValueMember = "ProductID";
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
                var dt = ExecuteQuery("SELECT SupplierID, SupplierName FROM Suppliers");
                cmb.DataSource = dt;
                cmb.DisplayMember = "SupplierName";
                cmb.ValueMember = "SupplierID";
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
                string query = "SELECT SaleID, ModelName, SaleDate, Quantity, UnitPrice, TotalPrice, CustomerName, CustomerPhone FROM vw_SalesReport ORDER BY SaleDate DESC";
                dgvSales.DataSource = ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки продаж: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAddSale_Click(object sender, EventArgs e)
        {
            var form = CreateDialogForm("Новая продажа", 450, 450, out var tlp, out var bottomPanel);

            var cmbProduct = new ComboBox(); LoadProductsCombo(cmbProduct);
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
                    int stock = Convert.ToInt32(ExecuteScalar("SELECT StockQuantity FROM Products WHERE ProductID = @ProductID",
                                                              ("@ProductID", Convert.ToInt32(cmbProduct.SelectedValue))));
                    int requestedQty = int.Parse(txtQty.Text);

                    if (stock < requestedQty)
                    {
                        MessageBox.Show($"Недостаточно товара на складе! Текущий остаток: {stock} шт.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string sql = @"INSERT INTO Sales (ProductID, SaleDate, Quantity, UnitPrice, CustomerName, CustomerPhone) 
                                   VALUES (@Product, @Date, @Qty, @Price, @Customer, @Phone)";
                    ExecuteNonQuery(sql,
                        ("@Product", Convert.ToInt32(cmbProduct.SelectedValue)),
                        ("@Date", dtpDate.Value.ToString("yyyy-MM-dd HH:mm:ss")),
                        ("@Qty", requestedQty),
                        ("@Price", decimal.Parse(txtPrice.Text)),
                        ("@Customer", txtCustomer.Text.Trim()),
                        ("@Phone", txtPhone.Text.Trim()));

                    MessageBox.Show("Продажа успешно оформлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadSales();
                    LoadProducts();
                    form.Close();
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
                string query = @"SELECT ModelName, SaleDate, Quantity, UnitPrice, TotalPrice, CustomerName, CustomerPhone 
                                 FROM vw_SalesReport WHERE SaleID = @SaleID";

                using var conn = new SqliteConnection(iStoreDB.ConnectionString);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@SaleID", saleId);

                using var reader = cmd.ExecuteReader();
                if (!reader.Read())
                {
                    MessageBox.Show("Данные о продаже не найдены.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string modelName = reader["ModelName"].ToString();
                string saleDateStr = reader["SaleDate"].ToString();
                DateTime saleDate = DateTime.TryParse(saleDateStr, out var parsedDate) ? parsedDate : DateTime.Now;
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
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при формировании чека: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Отчеты
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
                string query = @"SELECT ModelName AS 'Модель', StockQuantity AS 'Остаток (шт)', BasePrice AS 'Цена (₽)', 
                                        (StockQuantity * BasePrice) AS 'Общая стоимость (₽)'
                                 FROM Products ORDER BY StockQuantity DESC";
                dgvReports.DataSource = ExecuteQuery(query);
                dgvReports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void BtnSalesReport_Click(object sender, EventArgs e)
        {
            try
            {
                string query = @"SELECT strftime('%d.%m.%Y', SaleDate) AS 'Дата', ModelName AS 'Товар', 
                                        Quantity AS 'Кол-во', UnitPrice AS 'Цена (₽)', TotalPrice AS 'Сумма (₽)', CustomerName AS 'Клиент'
                                 FROM vw_SalesReport ORDER BY SaleDate DESC";
                dgvReports.DataSource = ExecuteQuery(query);
                dgvReports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void BtnPurchaseReport_Click(object sender, EventArgs e)
        {
            try
            {
                string query = @"SELECT strftime('%d.%m.%Y', PurchaseDate) AS 'Дата', ModelName AS 'Товар', SupplierName AS 'Поставщик', 
                                        Quantity AS 'Кол-во', UnitCost AS 'Цена (₽)', TotalCost AS 'Сумма (₽)'
                                 FROM vw_PurchaseReport ORDER BY PurchaseDate DESC";
                dgvReports.DataSource = ExecuteQuery(query);
                dgvReports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void BtnProfitReport_Click(object sender, EventArgs e)
        {
            try
            {
                decimal totalSales = Convert.ToDecimal(ExecuteScalar("SELECT COALESCE(SUM(TotalPrice), 0) FROM Sales"));
                decimal totalPurchases = Convert.ToDecimal(ExecuteScalar("SELECT COALESCE(SUM(TotalCost),  0) FROM Purchases"));
                decimal profit = totalSales - totalPurchases;

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

                dgvReports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dgvReports.Columns[0].Width = 300;
                dgvReports.Columns[1].Width = 200;
                dgvReports.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void BtnExportToExcel_Click(object sender, EventArgs e)
        {
            if (dgvReports.DataSource == null)
            {
                MessageBox.Show("Нет данных для экспорта. Сначала сформируйте отчет.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable dt = GetDataTableFromSource(dgvReports.DataSource);
            if (dt == null || dt.Rows.Count == 0)
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
                        const string separator = ";";
                        var sb = new StringBuilder();

                        sb.AppendLine($"sep={separator}");

                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            sb.Append(EscapeCsvValue(dt.Columns[i].ColumnName, separator));
                            if (i < dt.Columns.Count - 1) sb.Append(separator);
                        }
                        sb.AppendLine();

                        foreach (DataRow row in dt.Rows)
                        {
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                string val = row[i] == null || row[i] is DBNull
                                    ? ""
                                    : row[i].ToString();

                                sb.Append(EscapeCsvValue(val, separator));
                                if (i < dt.Columns.Count - 1) sb.Append(separator);
                            }
                            sb.AppendLine();
                        }

                        File.WriteAllText(sfd.FileName, sb.ToString(), new UTF8Encoding(true));

                        MessageBox.Show(
                            "Отчет успешно экспортирован!\n\n" +
                            $"Файл сохранён:\n{sfd.FileName}\n\n" +
                            "Откройте его двойным кликом — Excel автоматически разобьёт данные по колонкам.",
                            "Успех",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private DataTable GetDataTableFromSource(object source)
        {
            if (source is DataTable dt)
                return dt;
            if (source is DataView dv)
                return dv.Table;
            if (source is BindingSource bs && bs.DataSource is DataTable bsDt)
                return bsDt;

            return null;
        }

        private string EscapeCsvValue(string value, string separator)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            bool needsQuotes = value.Contains(separator) ||
                               value.Contains("\"") ||
                               value.Contains("\n") ||
                               value.Contains("\r") ||
                               value.StartsWith(" ") ||
                               value.EndsWith(" ");

            if (needsQuotes)
            {
                string escaped = value.Replace("\"", "\"\"");
                return $"\"{escaped}\"";
            }

            return value;
        }
        #endregion
    }
}