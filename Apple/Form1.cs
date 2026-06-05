using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace Apple
{
    // ═══════════════════════════════════════════════════════════════
    //  Строгая плоская кнопка с левой акцентной полоской и hover
    // ═══════════════════════════════════════════════════════════════
    public class FlatButton : Button
    {
        private Color _accentColor;
        private Color _hoverColor;
        private Color _hoverAccentColor;
        private int _accentWidth;

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")]
        [Description("Цвет левой акцентной полоски")]
        public Color AccentColor
        {
            get { return _accentColor; }
            set { _accentColor = value; Invalidate(); }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")]
        [Description("Цвет фона при наведении")]
        public Color HoverColor
        {
            get { return _hoverColor; }
            set { _hoverColor = value; Invalidate(); }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")]
        [Description("Цвет акцентной полоски при наведении")]
        public Color HoverAccentColor
        {
            get { return _hoverAccentColor; }
            set { _hoverAccentColor = value; Invalidate(); }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")]
        [Description("Ширина левой акцентной полоски в пикселях")]
        public int AccentWidth
        {
            get { return _accentWidth; }
            set { _accentWidth = value; Invalidate(); }
        }

        private bool _isHovered;
        private bool _isPressed;

        public FlatButton()
        {
            _accentColor = Color.FromArgb(99, 102, 241);
            _hoverColor = Color.FromArgb(241, 245, 249);
            _hoverAccentColor = Color.Empty;
            _accentWidth = 3;

            SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            FlatAppearance.MouseOverBackColor = Color.Transparent;
            FlatAppearance.MouseDownBackColor = Color.Transparent;
            Cursor = Cursors.Hand;
            Font = new Font("Segoe UI Semibold", 9.5F);
            ForeColor = Color.FromArgb(30, 41, 59);
            TextAlign = ContentAlignment.MiddleCenter;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var rect = new Rectangle(0, 0, Width, Height);

            Color bg = BackColor;
            if (_isPressed)
                bg = ControlPaint.Dark(_hoverColor, 0.05f);
            else if (_isHovered)
                bg = _hoverColor;

            using (var bgBrush = new SolidBrush(bg))
                g.FillRectangle(bgBrush, rect);

            Color accent = _accentColor;
            if (_isHovered && _hoverAccentColor != Color.Empty)
                accent = _hoverAccentColor;

            using (var accentBrush = new SolidBrush(accent))
                g.FillRectangle(accentBrush, 0, 0, _accentWidth, Height);

            if (_isHovered)
            {
                using (var borderPen = new Pen(Color.FromArgb(203, 213, 225), 1))
                {
                    g.DrawLine(borderPen, _accentWidth, Height - 1, Width - 1, Height - 1);
                }
            }

            using (var textBrush = new SolidBrush(ForeColor))
            {
                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    var textRect = new RectangleF(_accentWidth, 0, Width - _accentWidth, Height);
                    g.DrawString(Text, Font, textBrush, textRect, sf);
                }
            }
        }

        protected override void OnMouseEnter(EventArgs e) { _isHovered = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _isHovered = false; _isPressed = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs e) { _isPressed = true; Invalidate(); base.OnMouseDown(e); }
        protected override void OnMouseUp(MouseEventArgs e) { _isPressed = false; Invalidate(); base.OnMouseUp(e); }
    }

    // ═══════════════════════════════════════════════════════════════
    //  Современный TextBox
    // ═══════════════════════════════════════════════════════════════
    public class ModernTextBox : TextBox
    {
        public ModernTextBox()
        {
            BorderStyle = BorderStyle.FixedSingle;
            BackColor = Color.FromArgb(248, 250, 252);
            Font = new Font("Segoe UI", 10.5F);
            Height = 38;
            Padding = new Padding(8, 0, 8, 0);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            BackColor = Color.White;
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            BackColor = Color.FromArgb(248, 250, 252);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  Класс для хранения данных категории в ComboBox
    // ═══════════════════════════════════════════════════════════════
    public class CategoryItem
    {
        public int? CategoryID { get; set; }
        public string CategoryName { get; set; }

        public override string ToString() => CategoryName;
    }

    // ═══════════════════════════════════════════════════════════════
    //  Основная форма
    // ═══════════════════════════════════════════════════════════════
    public partial class Form1 : Form
    {
        private readonly Color DarkGraphite = Color.FromArgb(30, 41, 59);
        private readonly Color Slate700 = Color.FromArgb(51, 65, 85);
        private readonly Color Slate500 = Color.FromArgb(100, 116, 139);
        private readonly Color Slate200 = Color.FromArgb(226, 232, 240);
        private readonly Color Slate50 = Color.FromArgb(248, 250, 252);
        private readonly Color Indigo600 = Color.FromArgb(79, 70, 229);
        private readonly Color Red600 = Color.FromArgb(220, 38, 38);
        private readonly Color Red50 = Color.FromArgb(254, 242, 242);
        private readonly Color Green600 = Color.FromArgb(5, 150, 105);
        private readonly Color Green50 = Color.FromArgb(240, 253, 250);
        private readonly Color TextPrimary = Color.FromArgb(15, 23, 42);

        public Form1()
        {
            InitializeComponent();
            ApplyGlobalTheme();
        }

        private void ApplyGlobalTheme()
        {
            BackColor = Color.White;
            tabControl.Font = new Font("Segoe UI Semibold", 10F);
            tabControl.Padding = new Point(16, 10);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;
            DatabaseManager.Initialize();

            var allGrids = new[] { dgvProducts, dgvCategories, dgvSuppliers, dgvPurchases, dgvSales, dgvReports };
            foreach (var dgv in allGrids)
            {
                StyleDataGridView(dgv);
                dgv.DataError += Dgv_DataError;
            }

            LoadCategoriesData();
            LoadProducts();
            LoadSuppliers();
            LoadPurchases();
            LoadSales();
        }

        private void Dgv_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
            e.Cancel = true;
        }

        private void StyleDataGridView(DataGridView dgv)
        {
            dgv.Dock = DockStyle.Fill;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.RowTemplate.Height = 44;
            dgv.RowHeadersVisible = false;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Slate200;

            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgv.DefaultCellStyle.ForeColor = TextPrimary;
            dgv.DefaultCellStyle.Padding = new Padding(14, 0, 14, 0);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(238, 242, 255);
            dgv.DefaultCellStyle.SelectionForeColor = Indigo600;

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Slate50;
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(238, 242, 255);
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = Indigo600;

            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Slate50;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Slate500;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(14, 8, 14, 8);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Slate50;
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Slate500;
            dgv.ColumnHeadersHeight = 46;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgv.EnableHeadersVisualStyles = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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
                BackColor = Color.White,
                Font = new Font("Segoe UI", 10F),
                Padding = new Padding(28, 20, 28, 20)
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
                Padding = new Padding(0, 0, 0, 16)
            };
            bottomPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 64,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 12, 0, 0)
            };
            return form;
        }

        private void AddFormRow(TableLayoutPanel tlp, string labelText, Control inputControl, int row)
        {
            if (tlp.RowCount <= row) { tlp.RowCount++; tlp.RowStyles.Add(new RowStyle(SizeType.AutoSize)); }

            var lbl = new Label
            {
                Text = labelText,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Slate700,
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                Margin = new Padding(0, 12, 14, 12),
                AutoSize = true
            };

            if (inputControl is ModernTextBox mtb)
            {
                mtb.Font = new Font("Segoe UI", 10.5F);
                mtb.Height = 38;
            }
            else if (inputControl is TextBox tb)
            {
                tb.Font = new Font("Segoe UI", 10.5F);
                tb.BorderStyle = BorderStyle.FixedSingle;
                tb.BackColor = Slate50;
                tb.Height = tb.Multiline ? 80 : 38;
            }
            else if (inputControl is ComboBox cb)
            {
                cb.Font = new Font("Segoe UI", 10.5F);
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.FlatStyle = FlatStyle.Flat;
                cb.BackColor = Slate50;
                cb.Height = 38;
            }
            else if (inputControl is DateTimePicker dtp)
            {
                dtp.Font = new Font("Segoe UI", 10.5F);
                dtp.CalendarForeColor = TextPrimary;
                dtp.Height = 38;
            }

            inputControl.Margin = new Padding(0, 6, 0, 6);
            tlp.Controls.Add(lbl, 0, row);
            tlp.Controls.Add(inputControl, 1, row);
        }

        private FlatButton CreateDialogButton(string text, Color accent, Color hoverBg, Color hoverAccent, int width)
        {
            return new FlatButton
            {
                Text = text,
                Width = width,
                Height = 40,
                BackColor = Color.White,
                AccentColor = accent,
                HoverColor = hoverBg,
                HoverAccentColor = hoverAccent,
                ForeColor = TextPrimary,
                Margin = new Padding(0, 0, 10, 0)
            };
        }

        // 🔧 НОВЫЙ МЕТОД: заполнение ComboBox вручную без DataSource
        private void LoadCategoriesCombo(ComboBox cmb, int? selectedCategoryId = null)
        {
            cmb.Items.Clear();

            // Добавляем "Не выбрано"
            cmb.Items.Add(new CategoryItem { CategoryID = null, CategoryName = "— Не выбрано —" });

            // Загружаем категории из базы
            var dt = DatabaseManager.ExecuteQuery("SELECT CategoryID, CategoryName FROM Categories ORDER BY CategoryName");

            int selectedIndex = 0;
            int currentIndex = 1; // 0 - это "Не выбрано"

            foreach (DataRow row in dt.Rows)
            {
                var item = new CategoryItem
                {
                    CategoryID = Convert.ToInt32(row["CategoryID"]),
                    CategoryName = row["CategoryName"].ToString()
                };
                cmb.Items.Add(item);

                // Проверяем, совпадает ли с нужным ID
                if (selectedCategoryId.HasValue && item.CategoryID == selectedCategoryId.Value)
                {
                    selectedIndex = currentIndex;
                }
                currentIndex++;
            }

            // Устанавливаем выбранный элемент
            if (cmb.Items.Count > 0)
            {
                cmb.SelectedIndex = selectedIndex;
            }
        }

        private void LoadCombo(ComboBox cmb, string query, string display, string value)
        {
            cmb.DataSource = DatabaseManager.ExecuteQuery(query);
            cmb.DisplayMember = display;
            cmb.ValueMember = value;
        }

        // ──────────────────── Категории ────────────────────

        private void LoadCategoriesData()
        {
            dgvCategories.DataSource = DatabaseManager.ExecuteQuery(@"
                SELECT c.CategoryID AS 'ID', c.CategoryName AS 'Название', c.Description AS 'Описание',
                       COUNT(p.ProductID) AS 'Товаров'
                FROM Categories c LEFT JOIN Products p ON c.CategoryID = p.CategoryID
                GROUP BY c.CategoryID, c.CategoryName, c.Description ORDER BY c.CategoryName");
        }

        private void BtnAddCategory_Click(object sender, EventArgs e)
        {
            var form = CreateDialogForm("Добавить категорию", 480, 300, out var tlp, out var bottomPanel);
            var txtName = new ModernTextBox();
            var txtDesc = new ModernTextBox { Multiline = true, Height = 80 };

            AddFormRow(tlp, "Название", txtName, 0);
            AddFormRow(tlp, "Описание", txtDesc, 1);

            var btnSave = CreateDialogButton("💾  Сохранить", Indigo600, Color.FromArgb(238, 242, 255), Indigo600, 140);
            var btnCancel = CreateDialogButton("Отмена", Slate500, Slate50, Slate700, 120);

            btnCancel.Click += (s, ev) => form.Close();
            btnSave.Click += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(txtName.Text)) { MessageBox.Show("Введите название!"); return; }
                try
                {
                    DatabaseManager.ExecuteNonQuery(
                        "INSERT INTO Categories (CategoryName, Description) VALUES (@n, @d)",
                        ("@n", txtName.Text.Trim()),
                        ("@d", string.IsNullOrWhiteSpace(txtDesc.Text) ? DBNull.Value : (object)txtDesc.Text.Trim()));
                    LoadCategoriesData();
                    form.Close();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            };

            bottomPanel.Controls.Add(btnCancel);
            bottomPanel.Controls.Add(btnSave);
            form.Controls.Add(tlp);
            form.Controls.Add(bottomPanel);
            form.ShowDialog();
        }

        private void BtnEditCategory_Click(object sender, EventArgs e)
        {
            if (dgvCategories.SelectedRows.Count == 0) return;
            int id = Convert.ToInt32(dgvCategories.SelectedRows[0].Cells["ID"].Value);

            var form = CreateDialogForm("Изменить категорию", 480, 300, out var tlp, out var bottomPanel);
            var txtName = new ModernTextBox { Text = dgvCategories.SelectedRows[0].Cells["Название"].Value.ToString() };
            var txtDesc = new ModernTextBox { Multiline = true, Height = 80, Text = dgvCategories.SelectedRows[0].Cells["Описание"].Value?.ToString() };

            AddFormRow(tlp, "Название", txtName, 0);
            AddFormRow(tlp, "Описание", txtDesc, 1);

            var btnSave = CreateDialogButton("💾  Сохранить", Indigo600, Color.FromArgb(238, 242, 255), Indigo600, 140);
            var btnCancel = CreateDialogButton("Отмена", Slate500, Slate50, Slate700, 120);

            btnCancel.Click += (s, ev) => form.Close();
            btnSave.Click += (s, ev) =>
            {
                try
                {
                    DatabaseManager.ExecuteNonQuery(
                        "UPDATE Categories SET CategoryName=@n, Description=@d WHERE CategoryID=@id",
                        ("@id", id), ("@n", txtName.Text.Trim()),
                        ("@d", (object)txtDesc.Text.Trim() ?? DBNull.Value));
                    LoadCategoriesData();
                    form.Close();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            };

            bottomPanel.Controls.Add(btnCancel);
            bottomPanel.Controls.Add(btnSave);
            form.Controls.Add(tlp);
            form.Controls.Add(bottomPanel);
            form.ShowDialog();
        }

        private void BtnDeleteCategory_Click(object sender, EventArgs e)
        {
            if (dgvCategories.SelectedRows.Count == 0) return;
            int id = Convert.ToInt32(dgvCategories.SelectedRows[0].Cells["ID"].Value);
            if (MessageBox.Show("Удалить категорию?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try { DatabaseManager.ExecuteNonQuery("DELETE FROM Categories WHERE CategoryID=@id", ("@id", id)); LoadCategoriesData(); }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void BtnRefreshCategories_Click(object sender, EventArgs e) => LoadCategoriesData();

        // ──────────────────── Товары ────────────────────

        private void LoadProducts()
        {
            dgvProducts.DataSource = DatabaseManager.ExecuteQuery(@"
                SELECT p.ProductID AS 'ID', p.ModelName AS 'Модель', c.CategoryName AS 'Категория',
                       p.Description AS 'Описание', p.BasePrice AS 'Цена', p.StockQuantity AS 'Остаток'
                FROM Products p LEFT JOIN Categories c ON p.CategoryID = c.CategoryID");
        }

        // 🔧 ИСПРАВЛЕННЫЙ МЕТОД: добавление товара
        private void BtnAddProduct_Click(object sender, EventArgs e)
        {
            var form = CreateDialogForm("Добавить товар", 480, 400, out var tlp, out var bottomPanel);
            var txtModel = new ModernTextBox();
            var cmbCat = new ComboBox();
            var txtDesc = new ModernTextBox { Multiline = true, Height = 70 };
            var txtPrice = new ModernTextBox { Text = "0" };

            // 🔧 ВАЖНО: сначала добавляем контролы в форму, потом загружаем данные!
            AddFormRow(tlp, "Модель", txtModel, 0);
            AddFormRow(tlp, "Категория", cmbCat, 1);
            AddFormRow(tlp, "Описание", txtDesc, 2);
            AddFormRow(tlp, "Цена (₽)", txtPrice, 3);

            // 🔧 Теперь ComboBox уже в форме, можно безопасно загружать
            LoadCategoriesCombo(cmbCat);

            var btnSave = CreateDialogButton("💾  Сохранить", Indigo600, Color.FromArgb(238, 242, 255), Indigo600, 140);
            var btnCancel = CreateDialogButton("Отмена", Slate500, Slate50, Slate700, 120);

            btnCancel.Click += (s, ev) => form.Close();
            btnSave.Click += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(txtModel.Text)) { MessageBox.Show("Введите модель!"); return; }
                try
                {
                    var selectedItem = cmbCat.SelectedItem as CategoryItem;
                    object catValue = selectedItem?.CategoryID ?? (object)DBNull.Value;

                    DatabaseManager.ExecuteNonQuery(
                        "INSERT INTO Products (ModelName, CategoryID, Description, BasePrice, StockQuantity) VALUES (@m, @c, @d, @p, 0)",
                        ("@m", txtModel.Text.Trim()),
                        ("@c", catValue),
                        ("@d", string.IsNullOrWhiteSpace(txtDesc.Text) ? DBNull.Value : (object)txtDesc.Text.Trim()),
                        ("@p", decimal.Parse(txtPrice.Text)));
                    LoadProducts();
                    form.Close();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            };

            bottomPanel.Controls.Add(btnCancel);
            bottomPanel.Controls.Add(btnSave);
            form.Controls.Add(tlp);
            form.Controls.Add(bottomPanel);
            form.ShowDialog();
        }

        // 🔧 ИСПРАВЛЕННЫЙ МЕТОД: редактирование товара
        private void BtnEditProduct_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0) return;
            int id = Convert.ToInt32(dgvProducts.SelectedRows[0].Cells["ID"].Value);
            var dt = DatabaseManager.ExecuteQuery("SELECT * FROM Products WHERE ProductID = @id", ("@id", id));
            if (dt.Rows.Count == 0) return;
            var row = dt.Rows[0];

            var form = CreateDialogForm("Изменить товар", 480, 460, out var tlp, out var bottomPanel);
            var txtModel = new ModernTextBox { Text = row["ModelName"].ToString() };
            var cmbCat = new ComboBox();
            var txtDesc = new ModernTextBox { Multiline = true, Height = 70, Text = row["Description"]?.ToString() };
            var txtPrice = new ModernTextBox { Text = row["BasePrice"].ToString() };
            var txtStock = new ModernTextBox { Text = row["StockQuantity"].ToString() };

            // 🔧 ВАЖНО: сначала добавляем контролы в форму
            AddFormRow(tlp, "Модель", txtModel, 0);
            AddFormRow(tlp, "Категория", cmbCat, 1);
            AddFormRow(tlp, "Описание", txtDesc, 2);
            AddFormRow(tlp, "Цена (₽)", txtPrice, 3);
            AddFormRow(tlp, "Остаток", txtStock, 4);

            // 🔧 Теперь загружаем категории с выбранным значением
            int? selectedCategoryId = null;
            if (row["CategoryID"] != DBNull.Value)
            {
                selectedCategoryId = Convert.ToInt32(row["CategoryID"]);
            }
            LoadCategoriesCombo(cmbCat, selectedCategoryId);

            var btnSave = CreateDialogButton("💾  Сохранить", Indigo600, Color.FromArgb(238, 242, 255), Indigo600, 140);
            var btnCancel = CreateDialogButton("Отмена", Slate500, Slate50, Slate700, 120);

            btnCancel.Click += (s, ev) => form.Close();
            btnSave.Click += (s, ev) =>
            {
                try
                {
                    var selectedItem = cmbCat.SelectedItem as CategoryItem;
                    object catValue = selectedItem?.CategoryID ?? (object)DBNull.Value;

                    DatabaseManager.ExecuteNonQuery(
                        "UPDATE Products SET ModelName=@m, CategoryID=@c, Description=@d, BasePrice=@p, StockQuantity=@s WHERE ProductID=@id",
                        ("@id", id),
                        ("@m", txtModel.Text.Trim()),
                        ("@c", catValue),
                        ("@d", string.IsNullOrWhiteSpace(txtDesc.Text) ? DBNull.Value : (object)txtDesc.Text.Trim()),
                        ("@p", decimal.Parse(txtPrice.Text)),
                        ("@s", int.Parse(txtStock.Text)));
                    LoadProducts();
                    form.Close();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            };

            bottomPanel.Controls.Add(btnCancel);
            bottomPanel.Controls.Add(btnSave);
            form.Controls.Add(tlp);
            form.Controls.Add(bottomPanel);
            form.ShowDialog();
        }

        private void BtnDeleteProduct_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0) return;
            int id = Convert.ToInt32(dgvProducts.SelectedRows[0].Cells["ID"].Value);
            if (MessageBox.Show("Удалить товар?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    int pCount = Convert.ToInt32(DatabaseManager.ExecuteScalar("SELECT COUNT(*) FROM Purchases WHERE ProductID=@id", ("@id", id)));
                    int sCount = Convert.ToInt32(DatabaseManager.ExecuteScalar("SELECT COUNT(*) FROM Sales WHERE ProductID=@id", ("@id", id)));
                    if (pCount > 0 || sCount > 0) { MessageBox.Show("Нельзя удалить: есть связанные закупки или продажи!"); return; }
                    DatabaseManager.ExecuteNonQuery("DELETE FROM Products WHERE ProductID=@id", ("@id", id));
                    LoadProducts();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void BtnRefreshProducts_Click(object sender, EventArgs e) => LoadProducts();

        // ──────────────────── Поставщики ────────────────────

        private void LoadSuppliers()
        {
            dgvSuppliers.DataSource = DatabaseManager.ExecuteQuery(@"
                SELECT SupplierID AS 'ID', SupplierName AS 'Название', ContactName AS 'Контакт',
                       Phone AS 'Телефон', Email, Address AS 'Адрес' FROM Suppliers");
        }

        private void BtnAddSupplier_Click(object sender, EventArgs e)
        {
            var form = CreateDialogForm("Добавить поставщика", 520, 440, out var tlp, out var bottomPanel);
            var txtName = new ModernTextBox();
            var txtContact = new ModernTextBox();
            var txtPhone = new ModernTextBox();
            var txtEmail = new ModernTextBox();
            var txtAddress = new ModernTextBox { Multiline = true, Height = 70 };

            AddFormRow(tlp, "Название", txtName, 0);
            AddFormRow(tlp, "Контакт", txtContact, 1);
            AddFormRow(tlp, "Телефон", txtPhone, 2);
            AddFormRow(tlp, "Email", txtEmail, 3);
            AddFormRow(tlp, "Адрес", txtAddress, 4);

            var btnSave = CreateDialogButton("💾  Сохранить", Indigo600, Color.FromArgb(238, 242, 255), Indigo600, 140);
            var btnCancel = CreateDialogButton("Отмена", Slate500, Slate50, Slate700, 120);

            btnCancel.Click += (s, ev) => form.Close();
            btnSave.Click += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(txtName.Text)) { MessageBox.Show("Введите название!"); return; }
                try
                {
                    DatabaseManager.ExecuteNonQuery(
                        "INSERT INTO Suppliers (SupplierName, ContactName, Phone, Email, Address) VALUES (@n, @c, @p, @e, @a)",
                        ("@n", txtName.Text.Trim()), ("@c", txtContact.Text.Trim()),
                        ("@p", txtPhone.Text.Trim()), ("@e", txtEmail.Text.Trim()),
                        ("@a", txtAddress.Text.Trim()));
                    LoadSuppliers();
                    form.Close();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            };

            bottomPanel.Controls.Add(btnCancel);
            bottomPanel.Controls.Add(btnSave);
            form.Controls.Add(tlp);
            form.Controls.Add(bottomPanel);
            form.ShowDialog();
        }

        private void BtnEditSupplier_Click(object sender, EventArgs e)
        {
            if (dgvSuppliers.SelectedRows.Count == 0) return;
            int id = Convert.ToInt32(dgvSuppliers.SelectedRows[0].Cells["ID"].Value);
            var dt = DatabaseManager.ExecuteQuery("SELECT * FROM Suppliers WHERE SupplierID = @id", ("@id", id));
            if (dt.Rows.Count == 0) return;
            var row = dt.Rows[0];

            var form = CreateDialogForm("Изменить поставщика", 520, 440, out var tlp, out var bottomPanel);
            var txtName = new ModernTextBox { Text = row["SupplierName"].ToString() };
            var txtContact = new ModernTextBox { Text = row["ContactName"].ToString() };
            var txtPhone = new ModernTextBox { Text = row["Phone"].ToString() };
            var txtEmail = new ModernTextBox { Text = row["Email"].ToString() };
            var txtAddress = new ModernTextBox { Multiline = true, Height = 70, Text = row["Address"].ToString() };

            AddFormRow(tlp, "Название", txtName, 0);
            AddFormRow(tlp, "Контакт", txtContact, 1);
            AddFormRow(tlp, "Телефон", txtPhone, 2);
            AddFormRow(tlp, "Email", txtEmail, 3);
            AddFormRow(tlp, "Адрес", txtAddress, 4);

            var btnSave = CreateDialogButton("💾  Сохранить", Indigo600, Color.FromArgb(238, 242, 255), Indigo600, 140);
            var btnCancel = CreateDialogButton("Отмена", Slate500, Slate50, Slate700, 120);

            btnCancel.Click += (s, ev) => form.Close();
            btnSave.Click += (s, ev) =>
            {
                try
                {
                    DatabaseManager.ExecuteNonQuery(
                        "UPDATE Suppliers SET SupplierName=@n, ContactName=@c, Phone=@p, Email=@e, Address=@a WHERE SupplierID=@id",
                        ("@id", id), ("@n", txtName.Text.Trim()), ("@c", txtContact.Text.Trim()),
                        ("@p", txtPhone.Text.Trim()), ("@e", txtEmail.Text.Trim()),
                        ("@a", txtAddress.Text.Trim()));
                    LoadSuppliers();
                    form.Close();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            };

            bottomPanel.Controls.Add(btnCancel);
            bottomPanel.Controls.Add(btnSave);
            form.Controls.Add(tlp);
            form.Controls.Add(bottomPanel);
            form.ShowDialog();
        }

        private void BtnDeleteSupplier_Click(object sender, EventArgs e)
        {
            if (dgvSuppliers.SelectedRows.Count == 0) return;
            int id = Convert.ToInt32(dgvSuppliers.SelectedRows[0].Cells["ID"].Value);
            if (MessageBox.Show("Удалить поставщика?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    int pCount = Convert.ToInt32(DatabaseManager.ExecuteScalar("SELECT COUNT(*) FROM Purchases WHERE SupplierID=@id", ("@id", id)));
                    if (pCount > 0) { MessageBox.Show("Нельзя удалить: есть связанные закупки!"); return; }
                    DatabaseManager.ExecuteNonQuery("DELETE FROM Suppliers WHERE SupplierID=@id", ("@id", id));
                    LoadSuppliers();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void BtnRefreshSuppliers_Click(object sender, EventArgs e) => LoadSuppliers();

        // ──────────────────── Закупки ────────────────────

        private void LoadPurchases()
        {
            dgvPurchases.DataSource = DatabaseManager.ExecuteQuery(@"
                SELECT PurchaseID AS 'ID', ModelName AS 'Товар', SupplierName AS 'Поставщик',
                       PurchaseDate AS 'Дата', Quantity AS 'Кол-во', UnitCost AS 'Цена', TotalCost AS 'Сумма'
                FROM vw_PurchaseReport ORDER BY PurchaseDate DESC");
        }

        private void BtnAddPurchase_Click(object sender, EventArgs e)
        {
            var form = CreateDialogForm("Новая закупка", 480, 400, out var tlp, out var bottomPanel);
            var cmbProduct = new ComboBox();
            var cmbSupplier = new ComboBox();
            var dtpDate = new DateTimePicker { Format = DateTimePickerFormat.Short };
            var txtQty = new ModernTextBox();
            var txtCost = new ModernTextBox();

            AddFormRow(tlp, "Товар", cmbProduct, 0);
            AddFormRow(tlp, "Поставщик", cmbSupplier, 1);
            AddFormRow(tlp, "Дата", dtpDate, 2);
            AddFormRow(tlp, "Количество", txtQty, 3);
            AddFormRow(tlp, "Цена за ед.", txtCost, 4);

            LoadCombo(cmbProduct, "SELECT ProductID, ModelName FROM Products", "ModelName", "ProductID");
            LoadCombo(cmbSupplier, "SELECT SupplierID, SupplierName FROM Suppliers", "SupplierName", "SupplierID");

            var btnSave = CreateDialogButton("💾  Сохранить", Indigo600, Color.FromArgb(238, 242, 255), Indigo600, 140);
            var btnCancel = CreateDialogButton("Отмена", Slate500, Slate50, Slate700, 120);

            btnCancel.Click += (s, ev) => form.Close();
            btnSave.Click += (s, ev) =>
            {
                if (cmbProduct.SelectedValue == null || cmbSupplier.SelectedValue == null || string.IsNullOrWhiteSpace(txtQty.Text))
                { MessageBox.Show("Заполните все поля!"); return; }
                try
                {
                    DatabaseManager.ExecuteNonQuery(
                        "INSERT INTO Purchases (ProductID, SupplierID, PurchaseDate, Quantity, UnitCost) VALUES (@p, @s, @d, @q, @c)",
                        ("@p", Convert.ToInt32(cmbProduct.SelectedValue)),
                        ("@s", Convert.ToInt32(cmbSupplier.SelectedValue)),
                        ("@d", dtpDate.Value.ToString("yyyy-MM-dd")),
                        ("@q", int.Parse(txtQty.Text)),
                        ("@c", decimal.Parse(txtCost.Text)));
                    LoadPurchases();
                    LoadProducts();
                    form.Close();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            };

            bottomPanel.Controls.Add(btnCancel);
            bottomPanel.Controls.Add(btnSave);
            form.Controls.Add(tlp);
            form.Controls.Add(bottomPanel);
            form.ShowDialog();
        }

        private void BtnRefreshPurchases_Click(object sender, EventArgs e) => LoadPurchases();

        // ──────────────────── Продажи ────────────────────

        private void LoadSales()
        {
            dgvSales.DataSource = DatabaseManager.ExecuteQuery(@"
                SELECT SaleID AS 'ID', ModelName AS 'Товар', SaleDate AS 'Дата', Quantity AS 'Кол-во',
                       UnitPrice AS 'Цена', TotalPrice AS 'Сумма', CustomerName AS 'Клиент', CustomerPhone AS 'Телефон'
                FROM vw_SalesReport ORDER BY SaleDate DESC");
        }

        private void BtnAddSale_Click(object sender, EventArgs e)
        {
            var form = CreateDialogForm("Новая продажа", 480, 460, out var tlp, out var bottomPanel);
            var cmbProduct = new ComboBox();
            var dtpDate = new DateTimePicker { Format = DateTimePickerFormat.Short };
            var txtQty = new ModernTextBox();
            var txtPrice = new ModernTextBox();
            var txtCustomer = new ModernTextBox();
            var txtPhone = new ModernTextBox();

            AddFormRow(tlp, "Товар", cmbProduct, 0);
            AddFormRow(tlp, "Дата", dtpDate, 1);
            AddFormRow(tlp, "Количество", txtQty, 2);
            AddFormRow(tlp, "Цена за ед.", txtPrice, 3);
            AddFormRow(tlp, "Клиент", txtCustomer, 4);
            AddFormRow(tlp, "Телефон", txtPhone, 5);

            LoadCombo(cmbProduct, "SELECT ProductID, ModelName FROM Products", "ModelName", "ProductID");

            var btnSave = CreateDialogButton("💾  Сохранить", Indigo600, Color.FromArgb(238, 242, 255), Indigo600, 140);
            var btnCancel = CreateDialogButton("Отмена", Slate500, Slate50, Slate700, 120);

            btnCancel.Click += (s, ev) => form.Close();
            btnSave.Click += (s, ev) =>
            {
                if (cmbProduct.SelectedValue == null || string.IsNullOrWhiteSpace(txtQty.Text))
                { MessageBox.Show("Заполните обязательные поля!"); return; }
                try
                {
                    int productId = Convert.ToInt32(cmbProduct.SelectedValue);
                    int stock = Convert.ToInt32(DatabaseManager.ExecuteScalar("SELECT StockQuantity FROM Products WHERE ProductID = @id", ("@id", productId)));
                    int qty = int.Parse(txtQty.Text);
                    if (stock < qty) { MessageBox.Show($"Недостаточно товара на складе! Остаток: {stock} шт."); return; }

                    DatabaseManager.ExecuteNonQuery(
                        "INSERT INTO Sales (ProductID, SaleDate, Quantity, UnitPrice, CustomerName, CustomerPhone) VALUES (@p, @d, @q, @pr, @c, @ph)",
                        ("@p", productId), ("@d", dtpDate.Value.ToString("yyyy-MM-dd")),
                        ("@q", qty), ("@pr", decimal.Parse(txtPrice.Text)),
                        ("@c", txtCustomer.Text.Trim()), ("@ph", txtPhone.Text.Trim()));
                    LoadSales();
                    LoadProducts();
                    form.Close();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            };

            bottomPanel.Controls.Add(btnCancel);
            bottomPanel.Controls.Add(btnSave);
            form.Controls.Add(tlp);
            form.Controls.Add(bottomPanel);
            form.ShowDialog();
        }

        private void BtnPrintReceipt_Click(object sender, EventArgs e)
        {
            if (dgvSales.SelectedRows.Count == 0) { MessageBox.Show("Выберите продажу!"); return; }
            int saleId = Convert.ToInt32(dgvSales.SelectedRows[0].Cells["ID"].Value);
            var dt = DatabaseManager.ExecuteQuery("SELECT * FROM vw_SalesReport WHERE SaleID = @id", ("@id", saleId));
            if (dt.Rows.Count == 0) return;
            var row = dt.Rows[0];

            var sb = new StringBuilder();
            sb.AppendLine("════════════════════════════════════════");
            sb.AppendLine("          iStore — Apple Shop           ");
            sb.AppendLine("════════════════════════════════════════");
            sb.AppendLine($"  Дата:    {row["SaleDate"]}");
            sb.AppendLine($"  Клиент:  {row["CustomerName"]}");
            sb.AppendLine($"  Тел.:    {row["CustomerPhone"]}");
            sb.AppendLine("────────────────────────────────────────");
            sb.AppendLine($"  {row["ModelName"]}");
            sb.AppendLine($"  {row["Quantity"]} × {row["UnitPrice"]} ₽  =  {row["TotalPrice"]} ₽");
            sb.AppendLine("────────────────────────────────────────");
            sb.AppendLine($"  ИТОГО:   {row["TotalPrice"]} ₽");
            sb.AppendLine("════════════════════════════════════════");
            sb.AppendLine("        Спасибо за покупку! 🍎");
            sb.AppendLine("════════════════════════════════════════");

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Текстовые файлы (*.txt)|*.txt";
                sfd.FileName = $"Receipt_{saleId}.txt";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Чек сохранён!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnRefreshSales_Click(object sender, EventArgs e) => LoadSales();

        // ──────────────────── Отчёты ────────────────────

        private void BtnStockReport_Click(object sender, EventArgs e) =>
            dgvReports.DataSource = DatabaseManager.ExecuteQuery(
                "SELECT ModelName AS 'Модель', StockQuantity AS 'Остаток', BasePrice AS 'Цена', (StockQuantity * BasePrice) AS 'Сумма' FROM Products");

        private void BtnSalesReport_Click(object sender, EventArgs e) =>
            dgvReports.DataSource = DatabaseManager.ExecuteQuery("SELECT * FROM vw_SalesReport");

        private void BtnPurchaseReport_Click(object sender, EventArgs e) =>
            dgvReports.DataSource = DatabaseManager.ExecuteQuery("SELECT * FROM vw_PurchaseReport");

        private void BtnProfitReport_Click(object sender, EventArgs e)
        {
            decimal sales = Convert.ToDecimal(DatabaseManager.ExecuteScalar("SELECT COALESCE(SUM(TotalPrice), 0) FROM vw_SalesReport"));
            decimal purch = Convert.ToDecimal(DatabaseManager.ExecuteScalar("SELECT COALESCE(SUM(TotalCost), 0) FROM vw_PurchaseReport"));
            var dt = new DataTable();
            dt.Columns.Add("Показатель");
            dt.Columns.Add("Значение (₽)");
            dt.Rows.Add("Выручка (продажи)", sales.ToString("N2"));
            dt.Rows.Add("Расходы (закупки)", purch.ToString("N2"));
            dt.Rows.Add("ПРИБЫЛЬ", (sales - purch).ToString("N2"));
            dgvReports.DataSource = dt;
        }

        private void BtnExportToExcel_Click(object sender, EventArgs e)
        {
            if (dgvReports.DataSource == null) { MessageBox.Show("Сначала сформируйте отчёт!"); return; }
            var dt = dgvReports.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0) return;

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV файлы (*.csv)|*.csv";
                sfd.FileName = "Report.csv";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("sep=;");
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        sb.Append(dt.Columns[i].ColumnName);
                        if (i < dt.Columns.Count - 1) sb.Append(";");
                    }
                    sb.AppendLine();
                    foreach (DataRow row in dt.Rows)
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            sb.Append(row[i].ToString().Replace("\"", "\"\""));
                            if (i < dt.Columns.Count - 1) sb.Append(";");
                        }
                        sb.AppendLine();
                    }
                    File.WriteAllText(sfd.FileName, sb.ToString(), new UTF8Encoding(true));
                    MessageBox.Show("Экспорт завершён!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}