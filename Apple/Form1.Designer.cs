namespace Apple
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            tabControl = new TabControl();
            tabProducts = new TabPage();
            dgvProducts = new DataGridView();
            panelProducts = new FlowLayoutPanel();
            btnAddProduct = new FlatButton();
            btnEditProduct = new FlatButton();
            btnDeleteProduct = new FlatButton();
            btnRefreshProducts = new FlatButton();
            tabCategories = new TabPage();
            dgvCategories = new DataGridView();
            panelCategories = new FlowLayoutPanel();
            btnAddCategory = new FlatButton();
            btnEditCategory = new FlatButton();
            btnDeleteCategory = new FlatButton();
            btnRefreshCategories = new FlatButton();
            tabSuppliers = new TabPage();
            dgvSuppliers = new DataGridView();
            panelSuppliers = new FlowLayoutPanel();
            btnAddSupplier = new FlatButton();
            btnEditSupplier = new FlatButton();
            btnDeleteSupplier = new FlatButton();
            btnRefreshSuppliers = new FlatButton();
            tabPurchases = new TabPage();
            dgvPurchases = new DataGridView();
            panelPurchases = new FlowLayoutPanel();
            btnAddPurchase = new FlatButton();
            btnRefreshPurchases = new FlatButton();
            tabSales = new TabPage();
            dgvSales = new DataGridView();
            panelSales = new FlowLayoutPanel();
            btnAddSale = new FlatButton();
            btnRefreshSales = new FlatButton();
            btnPrintCheck = new FlatButton();
            tabReports = new TabPage();
            dgvReports = new DataGridView();
            panelReports = new FlowLayoutPanel();
            btnReportStock = new FlatButton();
            btnReportSales = new FlatButton();
            btnReportPurchases = new FlatButton();
            btnReportProfit = new FlatButton();
            btnExportExcel = new FlatButton();
            tabControl.SuspendLayout();
            tabProducts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
            panelProducts.SuspendLayout();
            tabCategories.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCategories).BeginInit();
            panelCategories.SuspendLayout();
            tabSuppliers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSuppliers).BeginInit();
            panelSuppliers.SuspendLayout();
            tabPurchases.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPurchases).BeginInit();
            panelPurchases.SuspendLayout();
            tabSales.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSales).BeginInit();
            panelSales.SuspendLayout();
            tabReports.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReports).BeginInit();
            panelReports.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabProducts);
            tabControl.Controls.Add(tabCategories);
            tabControl.Controls.Add(tabSuppliers);
            tabControl.Controls.Add(tabPurchases);
            tabControl.Controls.Add(tabSales);
            tabControl.Controls.Add(tabReports);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Font = new Font("Segoe UI Semibold", 10F);
            tabControl.Location = new Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.Padding = new Point(16, 10);
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1300, 820);
            tabControl.TabIndex = 0;
            // 
            // ────── ТОВАРЫ ──────
            // 
            tabProducts.Controls.Add(dgvProducts);
            tabProducts.Controls.Add(panelProducts);
            tabProducts.BackColor = Color.White;
            tabProducts.Location = new Point(4, 38);
            tabProducts.Name = "tabProducts";
            tabProducts.Size = new Size(1292, 778);
            tabProducts.TabIndex = 0;
            tabProducts.Text = "  📱  Товары  ";
            // 
            dgvProducts.Dock = DockStyle.Fill;
            dgvProducts.Name = "dgvProducts";
            dgvProducts.TabIndex = 0;
            // 
            panelProducts.BackColor = Color.White;
            panelProducts.Controls.Add(btnAddProduct);
            panelProducts.Controls.Add(btnEditProduct);
            panelProducts.Controls.Add(btnDeleteProduct);
            panelProducts.Controls.Add(btnRefreshProducts);
            panelProducts.Dock = DockStyle.Top;
            panelProducts.Location = new Point(0, 0);
            panelProducts.Name = "panelProducts";
            panelProducts.Padding = new Padding(24, 20, 24, 16);
            panelProducts.Size = new Size(1292, 80);
            panelProducts.TabIndex = 1;
            panelProducts.WrapContents = false;
            // 
            // Все кнопки — белый фон, синяя полоска по умолчанию
            // Hover: серо-голубой фон + синяя полоска
            // 
            btnAddProduct.BackColor = Color.White;
            btnAddProduct.AccentColor = Color.FromArgb(79, 70, 229);
            btnAddProduct.HoverColor = Color.FromArgb(238, 242, 255);
            btnAddProduct.HoverAccentColor = Color.FromArgb(79, 70, 229);
            btnAddProduct.ForeColor = Color.FromArgb(15, 23, 42);
            btnAddProduct.Location = new Point(24, 20);
            btnAddProduct.Margin = new Padding(0, 0, 12, 0);
            btnAddProduct.Name = "btnAddProduct";
            btnAddProduct.Size = new Size(190, 44);
            btnAddProduct.TabIndex = 0;
            btnAddProduct.Text = "➕  Добавить товар";
            btnAddProduct.Click += BtnAddProduct_Click;
            // 
            btnEditProduct.BackColor = Color.White;
            btnEditProduct.AccentColor = Color.FromArgb(79, 70, 229);
            btnEditProduct.HoverColor = Color.FromArgb(238, 242, 255);
            btnEditProduct.HoverAccentColor = Color.FromArgb(79, 70, 229);
            btnEditProduct.ForeColor = Color.FromArgb(15, 23, 42);
            btnEditProduct.Location = new Point(226, 20);
            btnEditProduct.Margin = new Padding(0, 0, 12, 0);
            btnEditProduct.Name = "btnEditProduct";
            btnEditProduct.Size = new Size(150, 44);
            btnEditProduct.TabIndex = 1;
            btnEditProduct.Text = "✏️  Изменить";
            btnEditProduct.Click += BtnEditProduct_Click;
            // 
            // ⚠ УДАЛИТЬ: при hover становится красным
            // 
            btnDeleteProduct.BackColor = Color.White;
            btnDeleteProduct.AccentColor = Color.FromArgb(100, 116, 139);
            btnDeleteProduct.HoverColor = Color.FromArgb(254, 242, 242);
            btnDeleteProduct.HoverAccentColor = Color.FromArgb(220, 38, 38);
            btnDeleteProduct.ForeColor = Color.FromArgb(15, 23, 42);
            btnDeleteProduct.Location = new Point(388, 20);
            btnDeleteProduct.Margin = new Padding(0, 0, 12, 0);
            btnDeleteProduct.Name = "btnDeleteProduct";
            btnDeleteProduct.Size = new Size(140, 44);
            btnDeleteProduct.TabIndex = 2;
            btnDeleteProduct.Text = "🗑  Удалить";
            btnDeleteProduct.Click += BtnDeleteProduct_Click;
            // 
            btnRefreshProducts.BackColor = Color.White;
            btnRefreshProducts.AccentColor = Color.FromArgb(100, 116, 139);
            btnRefreshProducts.HoverColor = Color.FromArgb(241, 245, 249);
            btnRefreshProducts.HoverAccentColor = Color.FromArgb(51, 65, 85);
            btnRefreshProducts.ForeColor = Color.FromArgb(15, 23, 42);
            btnRefreshProducts.Location = new Point(540, 20);
            btnRefreshProducts.Margin = new Padding(0, 0, 12, 0);
            btnRefreshProducts.Name = "btnRefreshProducts";
            btnRefreshProducts.Size = new Size(140, 44);
            btnRefreshProducts.TabIndex = 3;
            btnRefreshProducts.Text = "🔄  Обновить";
            btnRefreshProducts.Click += BtnRefreshProducts_Click;
            // 
            // ────── КАТЕГОРИИ ──────
            // 
            tabCategories.Controls.Add(dgvCategories);
            tabCategories.Controls.Add(panelCategories);
            tabCategories.BackColor = Color.White;
            tabCategories.Location = new Point(4, 38);
            tabCategories.Name = "tabCategories";
            tabCategories.Size = new Size(1292, 778);
            tabCategories.TabIndex = 1;
            tabCategories.Text = "  📂  Категории  ";
            // 
            dgvCategories.Dock = DockStyle.Fill;
            dgvCategories.Name = "dgvCategories";
            dgvCategories.TabIndex = 0;
            // 
            panelCategories.BackColor = Color.White;
            panelCategories.Controls.Add(btnAddCategory);
            panelCategories.Controls.Add(btnEditCategory);
            panelCategories.Controls.Add(btnDeleteCategory);
            panelCategories.Controls.Add(btnRefreshCategories);
            panelCategories.Dock = DockStyle.Top;
            panelCategories.Location = new Point(0, 0);
            panelCategories.Name = "panelCategories";
            panelCategories.Padding = new Padding(24, 20, 24, 16);
            panelCategories.Size = new Size(1292, 80);
            panelCategories.TabIndex = 1;
            panelCategories.WrapContents = false;
            // 
            btnAddCategory.BackColor = Color.White;
            btnAddCategory.AccentColor = Color.FromArgb(79, 70, 229);
            btnAddCategory.HoverColor = Color.FromArgb(238, 242, 255);
            btnAddCategory.HoverAccentColor = Color.FromArgb(79, 70, 229);
            btnAddCategory.ForeColor = Color.FromArgb(15, 23, 42);
            btnAddCategory.Location = new Point(24, 20);
            btnAddCategory.Margin = new Padding(0, 0, 12, 0);
            btnAddCategory.Name = "btnAddCategory";
            btnAddCategory.Size = new Size(180, 44);
            btnAddCategory.TabIndex = 0;
            btnAddCategory.Text = "➕  Добавить";
            btnAddCategory.Click += BtnAddCategory_Click;
            // 
            btnEditCategory.BackColor = Color.White;
            btnEditCategory.AccentColor = Color.FromArgb(79, 70, 229);
            btnEditCategory.HoverColor = Color.FromArgb(238, 242, 255);
            btnEditCategory.HoverAccentColor = Color.FromArgb(79, 70, 229);
            btnEditCategory.ForeColor = Color.FromArgb(15, 23, 42);
            btnEditCategory.Location = new Point(216, 20);
            btnEditCategory.Margin = new Padding(0, 0, 12, 0);
            btnEditCategory.Name = "btnEditCategory";
            btnEditCategory.Size = new Size(150, 44);
            btnEditCategory.TabIndex = 1;
            btnEditCategory.Text = "✏️  Изменить";
            btnEditCategory.Click += BtnEditCategory_Click;
            // 
            btnDeleteCategory.BackColor = Color.White;
            btnDeleteCategory.AccentColor = Color.FromArgb(100, 116, 139);
            btnDeleteCategory.HoverColor = Color.FromArgb(254, 242, 242);
            btnDeleteCategory.HoverAccentColor = Color.FromArgb(220, 38, 38);
            btnDeleteCategory.ForeColor = Color.FromArgb(15, 23, 42);
            btnDeleteCategory.Location = new Point(378, 20);
            btnDeleteCategory.Margin = new Padding(0, 0, 12, 0);
            btnDeleteCategory.Name = "btnDeleteCategory";
            btnDeleteCategory.Size = new Size(140, 44);
            btnDeleteCategory.TabIndex = 2;
            btnDeleteCategory.Text = "🗑  Удалить";
            btnDeleteCategory.Click += BtnDeleteCategory_Click;
            // 
            btnRefreshCategories.BackColor = Color.White;
            btnRefreshCategories.AccentColor = Color.FromArgb(100, 116, 139);
            btnRefreshCategories.HoverColor = Color.FromArgb(241, 245, 249);
            btnRefreshCategories.HoverAccentColor = Color.FromArgb(51, 65, 85);
            btnRefreshCategories.ForeColor = Color.FromArgb(15, 23, 42);
            btnRefreshCategories.Location = new Point(530, 20);
            btnRefreshCategories.Margin = new Padding(0, 0, 12, 0);
            btnRefreshCategories.Name = "btnRefreshCategories";
            btnRefreshCategories.Size = new Size(140, 44);
            btnRefreshCategories.TabIndex = 3;
            btnRefreshCategories.Text = "🔄  Обновить";
            btnRefreshCategories.Click += BtnRefreshCategories_Click;
            // 
            // ────── ПОСТАВЩИКИ ──────
            //  ⚠ Ширина btnAddSupplier = 240px (полное название)
            // 
            tabSuppliers.Controls.Add(dgvSuppliers);
            tabSuppliers.Controls.Add(panelSuppliers);
            tabSuppliers.BackColor = Color.White;
            tabSuppliers.Location = new Point(4, 38);
            tabSuppliers.Name = "tabSuppliers";
            tabSuppliers.Size = new Size(1292, 778);
            tabSuppliers.TabIndex = 2;
            tabSuppliers.Text = "  🚚  Поставщики  ";
            // 
            dgvSuppliers.Dock = DockStyle.Fill;
            dgvSuppliers.Name = "dgvSuppliers";
            dgvSuppliers.TabIndex = 0;
            // 
            panelSuppliers.BackColor = Color.White;
            panelSuppliers.Controls.Add(btnAddSupplier);
            panelSuppliers.Controls.Add(btnEditSupplier);
            panelSuppliers.Controls.Add(btnDeleteSupplier);
            panelSuppliers.Controls.Add(btnRefreshSuppliers);
            panelSuppliers.Dock = DockStyle.Top;
            panelSuppliers.Location = new Point(0, 0);
            panelSuppliers.Name = "panelSuppliers";
            panelSuppliers.Padding = new Padding(24, 20, 24, 16);
            panelSuppliers.Size = new Size(1292, 80);
            panelSuppliers.TabIndex = 1;
            panelSuppliers.WrapContents = false;
            // 
            btnAddSupplier.BackColor = Color.White;
            btnAddSupplier.AccentColor = Color.FromArgb(79, 70, 229);
            btnAddSupplier.HoverColor = Color.FromArgb(238, 242, 255);
            btnAddSupplier.HoverAccentColor = Color.FromArgb(79, 70, 229);
            btnAddSupplier.ForeColor = Color.FromArgb(15, 23, 42);
            btnAddSupplier.Location = new Point(24, 20);
            btnAddSupplier.Margin = new Padding(0, 0, 12, 0);
            btnAddSupplier.Name = "btnAddSupplier";
            btnAddSupplier.Size = new Size(240, 44);
            btnAddSupplier.TabIndex = 0;
            btnAddSupplier.Text = "➕  Добавить поставщика";
            btnAddSupplier.Click += BtnAddSupplier_Click;
            // 
            btnEditSupplier.BackColor = Color.White;
            btnEditSupplier.AccentColor = Color.FromArgb(79, 70, 229);
            btnEditSupplier.HoverColor = Color.FromArgb(238, 242, 255);
            btnEditSupplier.HoverAccentColor = Color.FromArgb(79, 70, 229);
            btnEditSupplier.ForeColor = Color.FromArgb(15, 23, 42);
            btnEditSupplier.Location = new Point(276, 20);
            btnEditSupplier.Margin = new Padding(0, 0, 12, 0);
            btnEditSupplier.Name = "btnEditSupplier";
            btnEditSupplier.Size = new Size(150, 44);
            btnEditSupplier.TabIndex = 1;
            btnEditSupplier.Text = "✏️  Изменить";
            btnEditSupplier.Click += BtnEditSupplier_Click;
            // 
            btnDeleteSupplier.BackColor = Color.White;
            btnDeleteSupplier.AccentColor = Color.FromArgb(100, 116, 139);
            btnDeleteSupplier.HoverColor = Color.FromArgb(254, 242, 242);
            btnDeleteSupplier.HoverAccentColor = Color.FromArgb(220, 38, 38);
            btnDeleteSupplier.ForeColor = Color.FromArgb(15, 23, 42);
            btnDeleteSupplier.Location = new Point(438, 20);
            btnDeleteSupplier.Margin = new Padding(0, 0, 12, 0);
            btnDeleteSupplier.Name = "btnDeleteSupplier";
            btnDeleteSupplier.Size = new Size(140, 44);
            btnDeleteSupplier.TabIndex = 2;
            btnDeleteSupplier.Text = "🗑  Удалить";
            btnDeleteSupplier.Click += BtnDeleteSupplier_Click;
            // 
            btnRefreshSuppliers.BackColor = Color.White;
            btnRefreshSuppliers.AccentColor = Color.FromArgb(100, 116, 139);
            btnRefreshSuppliers.HoverColor = Color.FromArgb(241, 245, 249);
            btnRefreshSuppliers.HoverAccentColor = Color.FromArgb(51, 65, 85);
            btnRefreshSuppliers.ForeColor = Color.FromArgb(15, 23, 42);
            btnRefreshSuppliers.Location = new Point(590, 20);
            btnRefreshSuppliers.Margin = new Padding(0, 0, 12, 0);
            btnRefreshSuppliers.Name = "btnRefreshSuppliers";
            btnRefreshSuppliers.Size = new Size(140, 44);
            btnRefreshSuppliers.TabIndex = 3;
            btnRefreshSuppliers.Text = "🔄  Обновить";
            btnRefreshSuppliers.Click += BtnRefreshSuppliers_Click;
            // 
            // ────── ЗАКУПКИ ──────
            // 
            tabPurchases.Controls.Add(dgvPurchases);
            tabPurchases.Controls.Add(panelPurchases);
            tabPurchases.BackColor = Color.White;
            tabPurchases.Location = new Point(4, 38);
            tabPurchases.Name = "tabPurchases";
            tabPurchases.Size = new Size(1292, 778);
            tabPurchases.TabIndex = 3;
            tabPurchases.Text = "  📥  Закупки  ";
            // 
            dgvPurchases.Dock = DockStyle.Fill;
            dgvPurchases.Name = "dgvPurchases";
            dgvPurchases.TabIndex = 0;
            // 
            panelPurchases.BackColor = Color.White;
            panelPurchases.Controls.Add(btnAddPurchase);
            panelPurchases.Controls.Add(btnRefreshPurchases);
            panelPurchases.Dock = DockStyle.Top;
            panelPurchases.Location = new Point(0, 0);
            panelPurchases.Name = "panelPurchases";
            panelPurchases.Padding = new Padding(24, 20, 24, 16);
            panelPurchases.Size = new Size(1292, 80);
            panelPurchases.TabIndex = 1;
            panelPurchases.WrapContents = false;
            // 
            btnAddPurchase.BackColor = Color.White;
            btnAddPurchase.AccentColor = Color.FromArgb(79, 70, 229);
            btnAddPurchase.HoverColor = Color.FromArgb(238, 242, 255);
            btnAddPurchase.HoverAccentColor = Color.FromArgb(79, 70, 229);
            btnAddPurchase.ForeColor = Color.FromArgb(15, 23, 42);
            btnAddPurchase.Location = new Point(24, 20);
            btnAddPurchase.Margin = new Padding(0, 0, 12, 0);
            btnAddPurchase.Name = "btnAddPurchase";
            btnAddPurchase.Size = new Size(200, 44);
            btnAddPurchase.TabIndex = 0;
            btnAddPurchase.Text = "➕  Новая закупка";
            btnAddPurchase.Click += BtnAddPurchase_Click;
            // 
            btnRefreshPurchases.BackColor = Color.White;
            btnRefreshPurchases.AccentColor = Color.FromArgb(100, 116, 139);
            btnRefreshPurchases.HoverColor = Color.FromArgb(241, 245, 249);
            btnRefreshPurchases.HoverAccentColor = Color.FromArgb(51, 65, 85);
            btnRefreshPurchases.ForeColor = Color.FromArgb(15, 23, 42);
            btnRefreshPurchases.Location = new Point(236, 20);
            btnRefreshPurchases.Margin = new Padding(0, 0, 12, 0);
            btnRefreshPurchases.Name = "btnRefreshPurchases";
            btnRefreshPurchases.Size = new Size(140, 44);
            btnRefreshPurchases.TabIndex = 1;
            btnRefreshPurchases.Text = "🔄  Обновить";
            btnRefreshPurchases.Click += BtnRefreshPurchases_Click;
            // 
            // ────── ПРОДАЖИ ──────
            // 
            tabSales.Controls.Add(dgvSales);
            tabSales.Controls.Add(panelSales);
            tabSales.BackColor = Color.White;
            tabSales.Location = new Point(4, 38);
            tabSales.Name = "tabSales";
            tabSales.Size = new Size(1292, 778);
            tabSales.TabIndex = 4;
            tabSales.Text = "  📤  Продажи  ";
            // 
            dgvSales.Dock = DockStyle.Fill;
            dgvSales.Name = "dgvSales";
            dgvSales.TabIndex = 0;
            // 
            panelSales.BackColor = Color.White;
            panelSales.Controls.Add(btnAddSale);
            panelSales.Controls.Add(btnRefreshSales);
            panelSales.Controls.Add(btnPrintCheck);
            panelSales.Dock = DockStyle.Top;
            panelSales.Location = new Point(0, 0);
            panelSales.Name = "panelSales";
            panelSales.Padding = new Padding(24, 20, 24, 16);
            panelSales.Size = new Size(1292, 80);
            panelSales.TabIndex = 1;
            panelSales.WrapContents = false;
            // 
            btnAddSale.BackColor = Color.White;
            btnAddSale.AccentColor = Color.FromArgb(79, 70, 229);
            btnAddSale.HoverColor = Color.FromArgb(238, 242, 255);
            btnAddSale.HoverAccentColor = Color.FromArgb(79, 70, 229);
            btnAddSale.ForeColor = Color.FromArgb(15, 23, 42);
            btnAddSale.Location = new Point(24, 20);
            btnAddSale.Margin = new Padding(0, 0, 12, 0);
            btnAddSale.Name = "btnAddSale";
            btnAddSale.Size = new Size(200, 44);
            btnAddSale.TabIndex = 0;
            btnAddSale.Text = "➕  Новая продажа";
            btnAddSale.Click += BtnAddSale_Click;
            // 
            btnRefreshSales.BackColor = Color.White;
            btnRefreshSales.AccentColor = Color.FromArgb(100, 116, 139);
            btnRefreshSales.HoverColor = Color.FromArgb(241, 245, 249);
            btnRefreshSales.HoverAccentColor = Color.FromArgb(51, 65, 85);
            btnRefreshSales.ForeColor = Color.FromArgb(15, 23, 42);
            btnRefreshSales.Location = new Point(236, 20);
            btnRefreshSales.Margin = new Padding(0, 0, 12, 0);
            btnRefreshSales.Name = "btnRefreshSales";
            btnRefreshSales.Size = new Size(140, 44);
            btnRefreshSales.TabIndex = 1;
            btnRefreshSales.Text = "🔄  Обновить";
            btnRefreshSales.Click += BtnRefreshSales_Click;
            // 
            // ПЕЧАТЬ: при hover зелёный
            // 
            btnPrintCheck.BackColor = Color.White;
            btnPrintCheck.AccentColor = Color.FromArgb(100, 116, 139);
            btnPrintCheck.HoverColor = Color.FromArgb(240, 253, 250);
            btnPrintCheck.HoverAccentColor = Color.FromArgb(5, 150, 105);
            btnPrintCheck.ForeColor = Color.FromArgb(15, 23, 42);
            btnPrintCheck.Location = new Point(388, 20);
            btnPrintCheck.Margin = new Padding(0, 0, 12, 0);
            btnPrintCheck.Name = "btnPrintCheck";
            btnPrintCheck.Size = new Size(220, 44);
            btnPrintCheck.TabIndex = 2;
            btnPrintCheck.Text = "🖨  Печать чека (.txt)";
            btnPrintCheck.Click += BtnPrintReceipt_Click;
            // 
            // ────── ОТЧЁТЫ ──────
            // 
            tabReports.Controls.Add(dgvReports);
            tabReports.Controls.Add(panelReports);
            tabReports.BackColor = Color.White;
            tabReports.Location = new Point(4, 38);
            tabReports.Name = "tabReports";
            tabReports.Size = new Size(1292, 778);
            tabReports.TabIndex = 5;
            tabReports.Text = "  📊  Отчёты  ";
            // 
            dgvReports.Dock = DockStyle.Fill;
            dgvReports.Name = "dgvReports";
            dgvReports.TabIndex = 0;
            // 
            panelReports.BackColor = Color.White;
            panelReports.Controls.Add(btnReportStock);
            panelReports.Controls.Add(btnReportSales);
            panelReports.Controls.Add(btnReportPurchases);
            panelReports.Controls.Add(btnReportProfit);
            panelReports.Controls.Add(btnExportExcel);
            panelReports.Dock = DockStyle.Top;
            panelReports.Location = new Point(0, 0);
            panelReports.Name = "panelReports";
            panelReports.Padding = new Padding(24, 20, 24, 16);
            panelReports.Size = new Size(1292, 80);
            panelReports.TabIndex = 1;
            panelReports.WrapContents = false;
            // 
            btnReportStock.BackColor = Color.White;
            btnReportStock.AccentColor = Color.FromArgb(79, 70, 229);
            btnReportStock.HoverColor = Color.FromArgb(238, 242, 255);
            btnReportStock.HoverAccentColor = Color.FromArgb(79, 70, 229);
            btnReportStock.ForeColor = Color.FromArgb(15, 23, 42);
            btnReportStock.Location = new Point(24, 20);
            btnReportStock.Margin = new Padding(0, 0, 12, 0);
            btnReportStock.Name = "btnReportStock";
            btnReportStock.Size = new Size(160, 44);
            btnReportStock.TabIndex = 0;
            btnReportStock.Text = "📦  Остатки";
            btnReportStock.Click += BtnStockReport_Click;
            // 
            btnReportSales.BackColor = Color.White;
            btnReportSales.AccentColor = Color.FromArgb(79, 70, 229);
            btnReportSales.HoverColor = Color.FromArgb(238, 242, 255);
            btnReportSales.HoverAccentColor = Color.FromArgb(79, 70, 229);
            btnReportSales.ForeColor = Color.FromArgb(15, 23, 42);
            btnReportSales.Location = new Point(196, 20);
            btnReportSales.Margin = new Padding(0, 0, 12, 0);
            btnReportSales.Name = "btnReportSales";
            btnReportSales.Size = new Size(160, 44);
            btnReportSales.TabIndex = 1;
            btnReportSales.Text = "💰  Продажи";
            btnReportSales.Click += BtnSalesReport_Click;
            // 
            btnReportPurchases.BackColor = Color.White;
            btnReportPurchases.AccentColor = Color.FromArgb(79, 70, 229);
            btnReportPurchases.HoverColor = Color.FromArgb(238, 242, 255);
            btnReportPurchases.HoverAccentColor = Color.FromArgb(79, 70, 229);
            btnReportPurchases.ForeColor = Color.FromArgb(15, 23, 42);
            btnReportPurchases.Location = new Point(368, 20);
            btnReportPurchases.Margin = new Padding(0, 0, 12, 0);
            btnReportPurchases.Name = "btnReportPurchases";
            btnReportPurchases.Size = new Size(160, 44);
            btnReportPurchases.TabIndex = 2;
            btnReportPurchases.Text = "📥  Закупки";
            btnReportPurchases.Click += BtnPurchaseReport_Click;
            // 
            btnReportProfit.BackColor = Color.White;
            btnReportProfit.AccentColor = Color.FromArgb(79, 70, 229);
            btnReportProfit.HoverColor = Color.FromArgb(238, 242, 255);
            btnReportProfit.HoverAccentColor = Color.FromArgb(79, 70, 229);
            btnReportProfit.ForeColor = Color.FromArgb(15, 23, 42);
            btnReportProfit.Location = new Point(540, 20);
            btnReportProfit.Margin = new Padding(0, 0, 12, 0);
            btnReportProfit.Name = "btnReportProfit";
            btnReportProfit.Size = new Size(160, 44);
            btnReportProfit.TabIndex = 3;
            btnReportProfit.Text = "📈  Прибыль";
            btnReportProfit.Click += BtnProfitReport_Click;
            // 
            // ЭКСПОРТ: при hover зелёный
            // 
            btnExportExcel.BackColor = Color.White;
            btnExportExcel.AccentColor = Color.FromArgb(100, 116, 139);
            btnExportExcel.HoverColor = Color.FromArgb(240, 253, 250);
            btnExportExcel.HoverAccentColor = Color.FromArgb(5, 150, 105);
            btnExportExcel.ForeColor = Color.FromArgb(15, 23, 42);
            btnExportExcel.Location = new Point(712, 20);
            btnExportExcel.Margin = new Padding(0, 0, 12, 0);
            btnExportExcel.Name = "btnExportExcel";
            btnExportExcel.Size = new Size(260, 44);
            btnExportExcel.TabIndex = 4;
            btnExportExcel.Text = "💾  Экспорт в Excel (.csv)";
            btnExportExcel.Click += BtnExportToExcel_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1300, 820);
            Controls.Add(tabControl);
            Font = new Font("Segoe UI", 10F);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "iStore — Управление магазином Apple";
            Load += Form1_Load;
            tabControl.ResumeLayout(false);
            tabProducts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
            panelProducts.ResumeLayout(false);
            tabCategories.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCategories).EndInit();
            panelCategories.ResumeLayout(false);
            tabSuppliers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSuppliers).EndInit();
            panelSuppliers.ResumeLayout(false);
            tabPurchases.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPurchases).EndInit();
            panelPurchases.ResumeLayout(false);
            tabSales.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSales).EndInit();
            panelSales.ResumeLayout(false);
            tabReports.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvReports).EndInit();
            panelReports.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabProducts, tabCategories, tabSuppliers, tabPurchases, tabSales, tabReports;
        private System.Windows.Forms.DataGridView dgvProducts, dgvCategories, dgvSuppliers, dgvPurchases, dgvSales, dgvReports;
        private System.Windows.Forms.FlowLayoutPanel panelProducts, panelCategories, panelSuppliers, panelPurchases, panelSales, panelReports;

        private FlatButton btnAddProduct, btnEditProduct, btnDeleteProduct, btnRefreshProducts;
        private FlatButton btnAddCategory, btnEditCategory, btnDeleteCategory, btnRefreshCategories;
        private FlatButton btnAddSupplier, btnEditSupplier, btnDeleteSupplier, btnRefreshSuppliers;
        private FlatButton btnAddPurchase, btnRefreshPurchases;
        private FlatButton btnAddSale, btnRefreshSales, btnPrintCheck;
        private FlatButton btnReportStock, btnReportSales, btnReportPurchases, btnReportProfit, btnExportExcel;
    }
}