namespace Apple
{
    partial class App
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tableLayoutPanel1 = new TableLayoutPanel();
            dataGridView1 = new DataGridView();
            flowLayoutPanel1 = new FlowLayoutPanel();
            label1 = new Label();
            textBox1 = new TextBox();
            label2 = new Label();
            textBox2 = new TextBox();
            label3 = new Label();
            textBox3 = new TextBox();
            label4 = new Label();
            comboBox1 = new ComboBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            tabPage2 = new TabPage();
            tableLayoutPanel2 = new TableLayoutPanel();
            dataGridView2 = new DataGridView();
            flowLayoutPanel2 = new FlowLayoutPanel();
            labelCatSearch = new Label();
            textBoxCatSearch = new TextBox();
            btnAddCategory = new Button();
            btnEditCategory = new Button();
            btnDeleteCategory = new Button();
            tabPage3 = new TabPage();
            tableLayoutPanel3 = new TableLayoutPanel();
            dataGridView3 = new DataGridView();
            flowLayoutPanel3 = new FlowLayoutPanel();
            labelSupSearch = new Label();
            textBoxSupSearch = new TextBox();
            btnAddSupplier = new Button();
            btnEditSupplier = new Button();
            btnDeleteSupplier = new Button();
            tabPage4 = new TabPage();
            tableLayoutPanel4 = new TableLayoutPanel();
            dataGridView4 = new DataGridView();
            flowLayoutPanel4 = new FlowLayoutPanel();
            labelCustSearch = new Label();
            textBoxCustSearch = new TextBox();
            labelCustType = new Label();
            comboBoxCustType = new ComboBox();
            btnAddCustomer = new Button();
            btnEditCustomer = new Button();
            btnDeleteCustomer = new Button();
            tabPage5 = new TabPage();
            tableLayoutPanel5 = new TableLayoutPanel();
            dataGridView5 = new DataGridView();
            flowLayoutPanel5 = new FlowLayoutPanel();
            labelPurSearch = new Label();
            textBoxPurSearch = new TextBox();
            labelPurSupplier = new Label();
            comboBoxPurSupplier = new ComboBox();
            labelPurDateFrom = new Label();
            dateTimePickerPurFrom = new DateTimePicker();
            labelPurDateTo = new Label();
            dateTimePickerPurTo = new DateTimePicker();
            btnAddPurchase = new Button();
            btnEditPurchase = new Button();
            btnDeletePurchase = new Button();
            tabPage6 = new TabPage();
            tableLayoutPanel6 = new TableLayoutPanel();
            dataGridView6 = new DataGridView();
            flowLayoutPanel6 = new FlowLayoutPanel();
            labelSaleSearch = new Label();
            textBoxSaleSearch = new TextBox();
            labelSaleDateFrom = new Label();
            dateTimePickerSaleFrom = new DateTimePicker();
            labelSaleDateTo = new Label();
            dateTimePickerSaleTo = new DateTimePicker();
            labelSaleStatus = new Label();
            comboBoxSaleStatus = new ComboBox();
            btnAddSale = new Button();
            btnEditSale = new Button();
            btnDeleteSale = new Button();
            btnPrintCheck = new Button();
            tabPage7 = new TabPage();
            tableLayoutPanel7 = new TableLayoutPanel();
            dataGridView7 = new DataGridView();
            flowLayoutPanel7 = new FlowLayoutPanel();
            labelRepDateFrom = new Label();
            dateTimePickerRepFrom = new DateTimePicker();
            labelRepDateTo = new Label();
            dateTimePickerRepTo = new DateTimePicker();
            labelRepCategory = new Label();
            comboBoxRepCategory = new ComboBox();
            btnReportStock = new Button();
            btnReportSales = new Button();
            btnReportPurchases = new Button();
            btnReportProfit = new Button();
            btnExportExcel = new Button();

            // === Столбцы dataGridView1 (Товары) ===
            colProdId = new DataGridViewTextBoxColumn();
            colProdName = new DataGridViewTextBoxColumn();
            colProdCategory = new DataGridViewTextBoxColumn();
            colProdPurchasePrice = new DataGridViewTextBoxColumn();
            colProdSalePrice = new DataGridViewTextBoxColumn();
            colProdStock = new DataGridViewTextBoxColumn();

            // === Столбцы dataGridView2 (Категории) ===
            colCatId = new DataGridViewTextBoxColumn();
            colCatName = new DataGridViewTextBoxColumn();

            // === Столбцы dataGridView3 (Поставщики) ===
            colSupId = new DataGridViewTextBoxColumn();
            colSupName = new DataGridViewTextBoxColumn();
            colSupContact = new DataGridViewTextBoxColumn();
            colSupPhone = new DataGridViewTextBoxColumn();
            colSupEmail = new DataGridViewTextBoxColumn();
            colSupAddress = new DataGridViewTextBoxColumn();

            // === Столбцы dataGridView4 (Покупатели) ===
            colCustId = new DataGridViewTextBoxColumn();
            colCustName = new DataGridViewTextBoxColumn();
            colCustType = new DataGridViewTextBoxColumn();
            colCustPhone = new DataGridViewTextBoxColumn();
            colCustEmail = new DataGridViewTextBoxColumn();
            colCustAddress = new DataGridViewTextBoxColumn();

            // === Столбцы dataGridView5 (Закупки) ===
            colPurId = new DataGridViewTextBoxColumn();
            colPurProduct = new DataGridViewTextBoxColumn();
            colPurSupplier = new DataGridViewTextBoxColumn();
            colPurQuantity = new DataGridViewTextBoxColumn();
            colPurPrice = new DataGridViewTextBoxColumn();
            colPurTotal = new DataGridViewTextBoxColumn();
            colPurDate = new DataGridViewTextBoxColumn();

            // === Столбцы dataGridView6 (Продажи) ===
            colSaleId = new DataGridViewTextBoxColumn();
            colSaleCustomer = new DataGridViewTextBoxColumn();
            colSaleDate = new DataGridViewTextBoxColumn();
            colSaleStatus = new DataGridViewTextBoxColumn();
            colSaleTotal = new DataGridViewTextBoxColumn();

            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            tabPage2.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            flowLayoutPanel2.SuspendLayout();
            tabPage3.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).BeginInit();
            flowLayoutPanel3.SuspendLayout();
            tabPage4.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView4).BeginInit();
            flowLayoutPanel4.SuspendLayout();
            tabPage5.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView5).BeginInit();
            flowLayoutPanel5.SuspendLayout();
            tabPage6.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView6).BeginInit();
            flowLayoutPanel6.SuspendLayout();
            tabPage7.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView7).BeginInit();
            flowLayoutPanel7.SuspendLayout();
            SuspendLayout();

            // =============================================
            // tabControl1
            // =============================================
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Controls.Add(tabPage6);
            tabControl1.Controls.Add(tabPage7);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1033, 450);
            tabControl1.TabIndex = 0;

            // =============================================
            // tabPage1 — Товары
            // =============================================
            tabPage1.Controls.Add(tableLayoutPanel1);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1025, 422);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Товары";
            tabPage1.UseVisualStyleBackColor = true;

            // tableLayoutPanel1
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(dataGridView1, 0, 0);
            tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 89.35667F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10.64333F));
            tableLayoutPanel1.Size = new Size(1019, 416);
            tableLayoutPanel1.TabIndex = 3;

            // dataGridView1 — Товары
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[]
            {
                colProdId, colProdName, colProdCategory,
                colProdPurchasePrice, colProdSalePrice, colProdStock
            });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(3, 3);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(1013, 365);
            dataGridView1.TabIndex = 1;

            // Столбцы dataGridView1
            colProdId.DataPropertyName = "ID";
            colProdId.HeaderText = "ID";
            colProdId.Name = "colProdId";
            colProdId.Width = 60;
            colProdId.FillWeight = 10;

            colProdName.DataPropertyName = "Название";
            colProdName.HeaderText = "Название";
            colProdName.Name = "colProdName";
            colProdName.FillWeight = 30;

            colProdCategory.DataPropertyName = "Категория";
            colProdCategory.HeaderText = "Категория";
            colProdCategory.Name = "colProdCategory";
            colProdCategory.FillWeight = 20;

            colProdPurchasePrice.DataPropertyName = "Закуп. цена";
            colProdPurchasePrice.HeaderText = "Закуп. цена";
            colProdPurchasePrice.Name = "colProdPurchasePrice";
            colProdPurchasePrice.DefaultCellStyle.Format = "N2";
            colProdPurchasePrice.FillWeight = 15;

            colProdSalePrice.DataPropertyName = "Цена продажи";
            colProdSalePrice.HeaderText = "Цена продажи";
            colProdSalePrice.Name = "colProdSalePrice";
            colProdSalePrice.DefaultCellStyle.Format = "N2";
            colProdSalePrice.FillWeight = 15;

            colProdStock.DataPropertyName = "Остаток";
            colProdStock.HeaderText = "Остаток";
            colProdStock.Name = "colProdStock";
            colProdStock.FillWeight = 10;

            // flowLayoutPanel1
            flowLayoutPanel1.Controls.Add(label1);
            flowLayoutPanel1.Controls.Add(textBox1);
            flowLayoutPanel1.Controls.Add(label2);
            flowLayoutPanel1.Controls.Add(textBox2);
            flowLayoutPanel1.Controls.Add(label3);
            flowLayoutPanel1.Controls.Add(textBox3);
            flowLayoutPanel1.Controls.Add(label4);
            flowLayoutPanel1.Controls.Add(comboBox1);
            flowLayoutPanel1.Controls.Add(button1);
            flowLayoutPanel1.Controls.Add(button2);
            flowLayoutPanel1.Controls.Add(button3);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(3, 374);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1013, 39);
            flowLayoutPanel1.TabIndex = 2;

            label1.Anchor = AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Location = new Point(3, 10);
            label1.Name = "label1";
            label1.Size = new Size(44, 15);
            label1.TabIndex = 3;
            label1.Text = "Найти:";

            textBox1.Anchor = AnchorStyles.Left;
            textBox1.Location = new Point(53, 6);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(150, 23);
            textBox1.TabIndex = 4;

            label2.Anchor = AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new Point(209, 10);
            label2.Name = "label2";
            label2.Size = new Size(64, 15);
            label2.TabIndex = 7;
            label2.Text = "Мин цена:";

            textBox2.Anchor = AnchorStyles.Left;
            textBox2.Location = new Point(279, 6);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(80, 23);
            textBox2.TabIndex = 5;

            label3.Anchor = AnchorStyles.Left;
            label3.AutoSize = true;
            label3.Location = new Point(365, 10);
            label3.Name = "label3";
            label3.Size = new Size(68, 15);
            label3.TabIndex = 8;
            label3.Text = "Макс цена:";

            textBox3.Anchor = AnchorStyles.Left;
            textBox3.Location = new Point(439, 6);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(80, 23);
            textBox3.TabIndex = 6;

            label4.Anchor = AnchorStyles.Left;
            label4.AutoSize = true;
            label4.Location = new Point(525, 10);
            label4.Name = "label4";
            label4.Size = new Size(66, 15);
            label4.TabIndex = 10;
            label4.Text = "Категория:";

            comboBox1.Anchor = AnchorStyles.Left;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(597, 6);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(121, 23);
            comboBox1.TabIndex = 9;

            button1.Location = new Point(724, 3);
            button1.Name = "button1";
            button1.Size = new Size(70, 30);
            button1.TabIndex = 0;
            button1.Text = "Добавить";
            button1.UseVisualStyleBackColor = true;

            button2.Location = new Point(800, 3);
            button2.Name = "button2";
            button2.Size = new Size(70, 30);
            button2.TabIndex = 1;
            button2.Text = "Править";
            button2.UseVisualStyleBackColor = true;

            button3.Location = new Point(876, 3);
            button3.Name = "button3";
            button3.Size = new Size(70, 30);
            button3.TabIndex = 2;
            button3.Text = "Удалить";
            button3.UseVisualStyleBackColor = true;

            // =============================================
            // tabPage2 — Категории
            // =============================================
            tabPage2.Controls.Add(tableLayoutPanel2);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1025, 422);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Категории";
            tabPage2.UseVisualStyleBackColor = true;

            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(dataGridView2, 0, 0);
            tableLayoutPanel2.Controls.Add(flowLayoutPanel2, 0, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 89.35667F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 10.64333F));
            tableLayoutPanel2.Size = new Size(1019, 416);
            tableLayoutPanel2.TabIndex = 0;

            // dataGridView2 — Категории
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.AllowUserToDeleteRows = false;
            dataGridView2.ReadOnly = true;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.MultiSelect = false;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.Columns.AddRange(new DataGridViewColumn[] { colCatId, colCatName });
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(3, 3);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.Size = new Size(1013, 365);
            dataGridView2.TabIndex = 0;

            colCatId.DataPropertyName = "ID";
            colCatId.HeaderText = "ID";
            colCatId.Name = "colCatId";
            colCatId.Width = 60;
            colCatId.FillWeight = 15;

            colCatName.DataPropertyName = "Название";
            colCatName.HeaderText = "Название";
            colCatName.Name = "colCatName";
            colCatName.FillWeight = 85;

            flowLayoutPanel2.Controls.Add(labelCatSearch);
            flowLayoutPanel2.Controls.Add(textBoxCatSearch);
            flowLayoutPanel2.Controls.Add(btnAddCategory);
            flowLayoutPanel2.Controls.Add(btnEditCategory);
            flowLayoutPanel2.Controls.Add(btnDeleteCategory);
            flowLayoutPanel2.Dock = DockStyle.Fill;
            flowLayoutPanel2.Location = new Point(3, 374);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(1013, 39);
            flowLayoutPanel2.TabIndex = 1;

            labelCatSearch.Anchor = AnchorStyles.Left;
            labelCatSearch.AutoSize = true;
            labelCatSearch.Location = new Point(3, 10);
            labelCatSearch.Name = "labelCatSearch";
            labelCatSearch.Size = new Size(44, 15);
            labelCatSearch.TabIndex = 0;
            labelCatSearch.Text = "Найти:";

            textBoxCatSearch.Anchor = AnchorStyles.Left;
            textBoxCatSearch.Location = new Point(53, 6);
            textBoxCatSearch.Name = "textBoxCatSearch";
            textBoxCatSearch.Size = new Size(200, 23);
            textBoxCatSearch.TabIndex = 1;

            btnAddCategory.Location = new Point(259, 3);
            btnAddCategory.Name = "btnAddCategory";
            btnAddCategory.Size = new Size(70, 30);
            btnAddCategory.TabIndex = 2;
            btnAddCategory.Text = "Добавить";
            btnAddCategory.UseVisualStyleBackColor = true;

            btnEditCategory.Location = new Point(335, 3);
            btnEditCategory.Name = "btnEditCategory";
            btnEditCategory.Size = new Size(70, 30);
            btnEditCategory.TabIndex = 3;
            btnEditCategory.Text = "Править";
            btnEditCategory.UseVisualStyleBackColor = true;

            btnDeleteCategory.Location = new Point(411, 3);
            btnDeleteCategory.Name = "btnDeleteCategory";
            btnDeleteCategory.Size = new Size(70, 30);
            btnDeleteCategory.TabIndex = 4;
            btnDeleteCategory.Text = "Удалить";
            btnDeleteCategory.UseVisualStyleBackColor = true;

            // =============================================
            // tabPage3 — Поставщики
            // =============================================
            tabPage3.Controls.Add(tableLayoutPanel3);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(1025, 422);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Поставщики";
            tabPage3.UseVisualStyleBackColor = true;

            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(dataGridView3, 0, 0);
            tableLayoutPanel3.Controls.Add(flowLayoutPanel3, 0, 1);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 89.35667F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 10.64333F));
            tableLayoutPanel3.Size = new Size(1019, 416);
            tableLayoutPanel3.TabIndex = 0;

            // dataGridView3 — Поставщики
            dataGridView3.AllowUserToAddRows = false;
            dataGridView3.AllowUserToDeleteRows = false;
            dataGridView3.ReadOnly = true;
            dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView3.MultiSelect = false;
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView3.AutoGenerateColumns = false;
            dataGridView3.Columns.AddRange(new DataGridViewColumn[]
            {
                colSupId, colSupName, colSupContact,
                colSupPhone, colSupEmail, colSupAddress
            });
            dataGridView3.Dock = DockStyle.Fill;
            dataGridView3.Location = new Point(3, 3);
            dataGridView3.Name = "dataGridView3";
            dataGridView3.Size = new Size(1013, 365);
            dataGridView3.TabIndex = 0;

            colSupId.DataPropertyName = "ID";
            colSupId.HeaderText = "ID";
            colSupId.Name = "colSupId";
            colSupId.Width = 50;
            colSupId.FillWeight = 8;

            colSupName.DataPropertyName = "Название";
            colSupName.HeaderText = "Название";
            colSupName.Name = "colSupName";
            colSupName.FillWeight = 22;

            colSupContact.DataPropertyName = "Контактное лицо";
            colSupContact.HeaderText = "Контактное лицо";
            colSupContact.Name = "colSupContact";
            colSupContact.FillWeight = 20;

            colSupPhone.DataPropertyName = "Телефон";
            colSupPhone.HeaderText = "Телефон";
            colSupPhone.Name = "colSupPhone";
            colSupPhone.FillWeight = 15;

            colSupEmail.DataPropertyName = "Email";
            colSupEmail.HeaderText = "Email";
            colSupEmail.Name = "colSupEmail";
            colSupEmail.FillWeight = 18;

            colSupAddress.DataPropertyName = "Адрес";
            colSupAddress.HeaderText = "Адрес";
            colSupAddress.Name = "colSupAddress";
            colSupAddress.FillWeight = 17;

            flowLayoutPanel3.Controls.Add(labelSupSearch);
            flowLayoutPanel3.Controls.Add(textBoxSupSearch);
            flowLayoutPanel3.Controls.Add(btnAddSupplier);
            flowLayoutPanel3.Controls.Add(btnEditSupplier);
            flowLayoutPanel3.Controls.Add(btnDeleteSupplier);
            flowLayoutPanel3.Dock = DockStyle.Fill;
            flowLayoutPanel3.Location = new Point(3, 374);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new Size(1013, 39);
            flowLayoutPanel3.TabIndex = 1;

            labelSupSearch.Anchor = AnchorStyles.Left;
            labelSupSearch.AutoSize = true;
            labelSupSearch.Location = new Point(3, 10);
            labelSupSearch.Name = "labelSupSearch";
            labelSupSearch.Size = new Size(44, 15);
            labelSupSearch.TabIndex = 0;
            labelSupSearch.Text = "Найти:";

            textBoxSupSearch.Anchor = AnchorStyles.Left;
            textBoxSupSearch.Location = new Point(53, 6);
            textBoxSupSearch.Name = "textBoxSupSearch";
            textBoxSupSearch.Size = new Size(200, 23);
            textBoxSupSearch.TabIndex = 1;

            btnAddSupplier.Location = new Point(259, 3);
            btnAddSupplier.Name = "btnAddSupplier";
            btnAddSupplier.Size = new Size(100, 30);
            btnAddSupplier.TabIndex = 2;
            btnAddSupplier.Text = "Добавить";
            btnAddSupplier.UseVisualStyleBackColor = true;

            btnEditSupplier.Location = new Point(365, 3);
            btnEditSupplier.Name = "btnEditSupplier";
            btnEditSupplier.Size = new Size(70, 30);
            btnEditSupplier.TabIndex = 3;
            btnEditSupplier.Text = "Править";
            btnEditSupplier.UseVisualStyleBackColor = true;

            btnDeleteSupplier.Location = new Point(441, 3);
            btnDeleteSupplier.Name = "btnDeleteSupplier";
            btnDeleteSupplier.Size = new Size(70, 30);
            btnDeleteSupplier.TabIndex = 4;
            btnDeleteSupplier.Text = "Удалить";
            btnDeleteSupplier.UseVisualStyleBackColor = true;

            // =============================================
            // tabPage4 — Покупатели
            // =============================================
            tabPage4.Controls.Add(tableLayoutPanel4);
            tabPage4.Location = new Point(4, 24);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(1025, 422);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Покупатели";
            tabPage4.UseVisualStyleBackColor = true;

            tableLayoutPanel4.ColumnCount = 1;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Controls.Add(dataGridView4, 0, 0);
            tableLayoutPanel4.Controls.Add(flowLayoutPanel4, 0, 1);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(3, 3);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 89.35667F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 10.64333F));
            tableLayoutPanel4.Size = new Size(1019, 416);
            tableLayoutPanel4.TabIndex = 0;

            // dataGridView4 — Покупатели
            dataGridView4.AllowUserToAddRows = false;
            dataGridView4.AllowUserToDeleteRows = false;
            dataGridView4.ReadOnly = true;
            dataGridView4.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView4.MultiSelect = false;
            dataGridView4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView4.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView4.AutoGenerateColumns = false;
            dataGridView4.Columns.AddRange(new DataGridViewColumn[]
            {
                colCustId, colCustName, colCustType,
                colCustPhone, colCustEmail, colCustAddress
            });
            dataGridView4.Dock = DockStyle.Fill;
            dataGridView4.Location = new Point(3, 3);
            dataGridView4.Name = "dataGridView4";
            dataGridView4.Size = new Size(1013, 365);
            dataGridView4.TabIndex = 0;

            colCustId.DataPropertyName = "ID";
            colCustId.HeaderText = "ID";
            colCustId.Name = "colCustId";
            colCustId.Width = 50;
            colCustId.FillWeight = 8;

            colCustName.DataPropertyName = "Имя";
            colCustName.HeaderText = "Имя/Название";
            colCustName.Name = "colCustName";
            colCustName.FillWeight = 22;

            colCustType.DataPropertyName = "Тип";
            colCustType.HeaderText = "Тип";
            colCustType.Name = "colCustType";
            colCustType.FillWeight = 12;

            colCustPhone.DataPropertyName = "Телефон";
            colCustPhone.HeaderText = "Телефон";
            colCustPhone.Name = "colCustPhone";
            colCustPhone.FillWeight = 18;

            colCustEmail.DataPropertyName = "Email";
            colCustEmail.HeaderText = "Email";
            colCustEmail.Name = "colCustEmail";
            colCustEmail.FillWeight = 20;

            colCustAddress.DataPropertyName = "Адрес";
            colCustAddress.HeaderText = "Адрес";
            colCustAddress.Name = "colCustAddress";
            colCustAddress.FillWeight = 20;

            flowLayoutPanel4.Controls.Add(labelCustSearch);
            flowLayoutPanel4.Controls.Add(textBoxCustSearch);
            flowLayoutPanel4.Controls.Add(labelCustType);
            flowLayoutPanel4.Controls.Add(comboBoxCustType);
            flowLayoutPanel4.Controls.Add(btnAddCustomer);
            flowLayoutPanel4.Controls.Add(btnEditCustomer);
            flowLayoutPanel4.Controls.Add(btnDeleteCustomer);
            flowLayoutPanel4.Dock = DockStyle.Fill;
            flowLayoutPanel4.Location = new Point(3, 374);
            flowLayoutPanel4.Name = "flowLayoutPanel4";
            flowLayoutPanel4.Size = new Size(1013, 39);
            flowLayoutPanel4.TabIndex = 1;

            labelCustSearch.Anchor = AnchorStyles.Left;
            labelCustSearch.AutoSize = true;
            labelCustSearch.Location = new Point(3, 10);
            labelCustSearch.Name = "labelCustSearch";
            labelCustSearch.Size = new Size(44, 15);
            labelCustSearch.TabIndex = 0;
            labelCustSearch.Text = "Найти:";

            textBoxCustSearch.Anchor = AnchorStyles.Left;
            textBoxCustSearch.Location = new Point(53, 6);
            textBoxCustSearch.Name = "textBoxCustSearch";
            textBoxCustSearch.Size = new Size(200, 23);
            textBoxCustSearch.TabIndex = 1;

            labelCustType.Anchor = AnchorStyles.Left;
            labelCustType.AutoSize = true;
            labelCustType.Location = new Point(259, 10);
            labelCustType.Name = "labelCustType";
            labelCustType.Size = new Size(31, 15);
            labelCustType.TabIndex = 2;
            labelCustType.Text = "Тип:";

            comboBoxCustType.Anchor = AnchorStyles.Left;
            comboBoxCustType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxCustType.FormattingEnabled = true;
            comboBoxCustType.Location = new Point(296, 6);
            comboBoxCustType.Name = "comboBoxCustType";
            comboBoxCustType.Size = new Size(121, 23);
            comboBoxCustType.TabIndex = 3;

            btnAddCustomer.Location = new Point(423, 3);
            btnAddCustomer.Name = "btnAddCustomer";
            btnAddCustomer.Size = new Size(100, 30);
            btnAddCustomer.TabIndex = 4;
            btnAddCustomer.Text = "Добавить";
            btnAddCustomer.UseVisualStyleBackColor = true;

            btnEditCustomer.Location = new Point(529, 3);
            btnEditCustomer.Name = "btnEditCustomer";
            btnEditCustomer.Size = new Size(70, 30);
            btnEditCustomer.TabIndex = 5;
            btnEditCustomer.Text = "Править";
            btnEditCustomer.UseVisualStyleBackColor = true;

            btnDeleteCustomer.Location = new Point(605, 3);
            btnDeleteCustomer.Name = "btnDeleteCustomer";
            btnDeleteCustomer.Size = new Size(70, 30);
            btnDeleteCustomer.TabIndex = 6;
            btnDeleteCustomer.Text = "Удалить";
            btnDeleteCustomer.UseVisualStyleBackColor = true;

            // =============================================
            // tabPage5 — Закупки
            // =============================================
            tabPage5.Controls.Add(tableLayoutPanel5);
            tabPage5.Location = new Point(4, 24);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(3);
            tabPage5.Size = new Size(1025, 422);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "Закупки";
            tabPage5.UseVisualStyleBackColor = true;

            tableLayoutPanel5.ColumnCount = 1;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.Controls.Add(dataGridView5, 0, 0);
            tableLayoutPanel5.Controls.Add(flowLayoutPanel5, 0, 1);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 3);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 2;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 89.35667F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10.64333F));
            tableLayoutPanel5.Size = new Size(1019, 416);
            tableLayoutPanel5.TabIndex = 0;

            // dataGridView5 — Закупки
            dataGridView5.AllowUserToAddRows = false;
            dataGridView5.AllowUserToDeleteRows = false;
            dataGridView5.ReadOnly = true;
            dataGridView5.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView5.MultiSelect = false;
            dataGridView5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView5.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView5.AutoGenerateColumns = false;
            dataGridView5.Columns.AddRange(new DataGridViewColumn[]
            {
                colPurId, colPurProduct, colPurSupplier,
                colPurQuantity, colPurPrice, colPurTotal, colPurDate
            });
            dataGridView5.Dock = DockStyle.Fill;
            dataGridView5.Location = new Point(3, 3);
            dataGridView5.Name = "dataGridView5";
            dataGridView5.Size = new Size(1013, 365);
            dataGridView5.TabIndex = 0;

            colPurId.DataPropertyName = "ID";
            colPurId.HeaderText = "ID";
            colPurId.Name = "colPurId";
            colPurId.Width = 50;
            colPurId.FillWeight = 8;

            colPurProduct.DataPropertyName = "Товар";
            colPurProduct.HeaderText = "Товар";
            colPurProduct.Name = "colPurProduct";
            colPurProduct.FillWeight = 22;

            colPurSupplier.DataPropertyName = "Поставщик";
            colPurSupplier.HeaderText = "Поставщик";
            colPurSupplier.Name = "colPurSupplier";
            colPurSupplier.FillWeight = 18;

            colPurQuantity.DataPropertyName = "Количество";
            colPurQuantity.HeaderText = "Кол-во";
            colPurQuantity.Name = "colPurQuantity";
            colPurQuantity.FillWeight = 10;

            colPurPrice.DataPropertyName = "Цена";
            colPurPrice.HeaderText = "Цена";
            colPurPrice.Name = "colPurPrice";
            colPurPrice.DefaultCellStyle.Format = "N2";
            colPurPrice.FillWeight = 12;

            colPurTotal.DataPropertyName = "Сумма";
            colPurTotal.HeaderText = "Сумма";
            colPurTotal.Name = "colPurTotal";
            colPurTotal.DefaultCellStyle.Format = "N2";
            colPurTotal.FillWeight = 12;

            colPurDate.DataPropertyName = "Дата";
            colPurDate.HeaderText = "Дата";
            colPurDate.Name = "colPurDate";
            colPurDate.DefaultCellStyle.Format = "d";
            colPurDate.FillWeight = 18;

            flowLayoutPanel5.Controls.Add(labelPurSearch);
            flowLayoutPanel5.Controls.Add(textBoxPurSearch);
            flowLayoutPanel5.Controls.Add(labelPurSupplier);
            flowLayoutPanel5.Controls.Add(comboBoxPurSupplier);
            flowLayoutPanel5.Controls.Add(labelPurDateFrom);
            flowLayoutPanel5.Controls.Add(dateTimePickerPurFrom);
            flowLayoutPanel5.Controls.Add(labelPurDateTo);
            flowLayoutPanel5.Controls.Add(dateTimePickerPurTo);
            flowLayoutPanel5.Controls.Add(btnAddPurchase);
            flowLayoutPanel5.Controls.Add(btnEditPurchase);
            flowLayoutPanel5.Controls.Add(btnDeletePurchase);
            flowLayoutPanel5.Dock = DockStyle.Fill;
            flowLayoutPanel5.Location = new Point(3, 374);
            flowLayoutPanel5.Name = "flowLayoutPanel5";
            flowLayoutPanel5.Size = new Size(1013, 39);
            flowLayoutPanel5.TabIndex = 1;

            labelPurSearch.Anchor = AnchorStyles.Left;
            labelPurSearch.AutoSize = true;
            labelPurSearch.Location = new Point(3, 10);
            labelPurSearch.Name = "labelPurSearch";
            labelPurSearch.Size = new Size(44, 15);
            labelPurSearch.TabIndex = 0;
            labelPurSearch.Text = "Найти:";

            textBoxPurSearch.Anchor = AnchorStyles.Left;
            textBoxPurSearch.Location = new Point(53, 6);
            textBoxPurSearch.Name = "textBoxPurSearch";
            textBoxPurSearch.Size = new Size(120, 23);
            textBoxPurSearch.TabIndex = 1;

            labelPurSupplier.Anchor = AnchorStyles.Left;
            labelPurSupplier.AutoSize = true;
            labelPurSupplier.Location = new Point(179, 10);
            labelPurSupplier.Name = "labelPurSupplier";
            labelPurSupplier.Size = new Size(73, 15);
            labelPurSupplier.TabIndex = 2;
            labelPurSupplier.Text = "Поставщик:";

            comboBoxPurSupplier.Anchor = AnchorStyles.Left;
            comboBoxPurSupplier.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxPurSupplier.FormattingEnabled = true;
            comboBoxPurSupplier.Location = new Point(258, 6);
            comboBoxPurSupplier.Name = "comboBoxPurSupplier";
            comboBoxPurSupplier.Size = new Size(121, 23);
            comboBoxPurSupplier.TabIndex = 3;

            labelPurDateFrom.Anchor = AnchorStyles.Left;
            labelPurDateFrom.AutoSize = true;
            labelPurDateFrom.Location = new Point(385, 10);
            labelPurDateFrom.Name = "labelPurDateFrom";
            labelPurDateFrom.Size = new Size(18, 15);
            labelPurDateFrom.TabIndex = 4;
            labelPurDateFrom.Text = "С:";

            dateTimePickerPurFrom.Anchor = AnchorStyles.Left;
            dateTimePickerPurFrom.Format = DateTimePickerFormat.Short;
            dateTimePickerPurFrom.Location = new Point(409, 6);
            dateTimePickerPurFrom.Name = "dateTimePickerPurFrom";
            dateTimePickerPurFrom.Size = new Size(110, 23);
            dateTimePickerPurFrom.TabIndex = 5;

            labelPurDateTo.Anchor = AnchorStyles.Left;
            labelPurDateTo.AutoSize = true;
            labelPurDateTo.Location = new Point(525, 10);
            labelPurDateTo.Name = "labelPurDateTo";
            labelPurDateTo.Size = new Size(26, 15);
            labelPurDateTo.TabIndex = 6;
            labelPurDateTo.Text = "По:";

            dateTimePickerPurTo.Anchor = AnchorStyles.Left;
            dateTimePickerPurTo.Format = DateTimePickerFormat.Short;
            dateTimePickerPurTo.Location = new Point(557, 6);
            dateTimePickerPurTo.Name = "dateTimePickerPurTo";
            dateTimePickerPurTo.Size = new Size(110, 23);
            dateTimePickerPurTo.TabIndex = 7;

            btnAddPurchase.Location = new Point(673, 3);
            btnAddPurchase.Name = "btnAddPurchase";
            btnAddPurchase.Size = new Size(100, 30);
            btnAddPurchase.TabIndex = 8;
            btnAddPurchase.Text = "Новая закупка";
            btnAddPurchase.UseVisualStyleBackColor = true;

            btnEditPurchase.Location = new Point(779, 3);
            btnEditPurchase.Name = "btnEditPurchase";
            btnEditPurchase.Size = new Size(70, 30);
            btnEditPurchase.TabIndex = 9;
            btnEditPurchase.Text = "Править";
            btnEditPurchase.UseVisualStyleBackColor = true;

            btnDeletePurchase.Location = new Point(855, 3);
            btnDeletePurchase.Name = "btnDeletePurchase";
            btnDeletePurchase.Size = new Size(70, 30);
            btnDeletePurchase.TabIndex = 10;
            btnDeletePurchase.Text = "Удалить";
            btnDeletePurchase.UseVisualStyleBackColor = true;

            // =============================================
            // tabPage6 — Продажи
            // =============================================
            tabPage6.Controls.Add(tableLayoutPanel6);
            tabPage6.Location = new Point(4, 24);
            tabPage6.Name = "tabPage6";
            tabPage6.Padding = new Padding(3);
            tabPage6.Size = new Size(1025, 422);
            tabPage6.TabIndex = 5;
            tabPage6.Text = "Продажи";
            tabPage6.UseVisualStyleBackColor = true;

            tableLayoutPanel6.ColumnCount = 1;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel6.Controls.Add(dataGridView6, 0, 0);
            tableLayoutPanel6.Controls.Add(flowLayoutPanel6, 0, 1);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(3, 3);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 2;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 89.35667F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 10.64333F));
            tableLayoutPanel6.Size = new Size(1019, 416);
            tableLayoutPanel6.TabIndex = 0;

            // dataGridView6 — Продажи
            dataGridView6.AllowUserToAddRows = false;
            dataGridView6.AllowUserToDeleteRows = false;
            dataGridView6.ReadOnly = true;
            dataGridView6.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView6.MultiSelect = false;
            dataGridView6.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView6.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView6.AutoGenerateColumns = false;
            dataGridView6.Columns.AddRange(new DataGridViewColumn[]
            {
                colSaleId, colSaleCustomer, colSaleDate,
                colSaleStatus, colSaleTotal
            });
            dataGridView6.Dock = DockStyle.Fill;
            dataGridView6.Location = new Point(3, 3);
            dataGridView6.Name = "dataGridView6";
            dataGridView6.Size = new Size(1013, 365);
            dataGridView6.TabIndex = 0;

            colSaleId.DataPropertyName = "ID";
            colSaleId.HeaderText = "ID";
            colSaleId.Name = "colSaleId";
            colSaleId.Width = 50;
            colSaleId.FillWeight = 8;

            colSaleCustomer.DataPropertyName = "Покупатель";
            colSaleCustomer.HeaderText = "Покупатель";
            colSaleCustomer.Name = "colSaleCustomer";
            colSaleCustomer.FillWeight = 25;

            colSaleDate.DataPropertyName = "Дата";
            colSaleDate.HeaderText = "Дата";
            colSaleDate.Name = "colSaleDate";
            colSaleDate.DefaultCellStyle.Format = "g";
            colSaleDate.FillWeight = 20;

            colSaleStatus.DataPropertyName = "Статус";
            colSaleStatus.HeaderText = "Статус";
            colSaleStatus.Name = "colSaleStatus";
            colSaleStatus.FillWeight = 15;

            colSaleTotal.DataPropertyName = "Сумма";
            colSaleTotal.HeaderText = "Сумма";
            colSaleTotal.Name = "colSaleTotal";
            colSaleTotal.DefaultCellStyle.Format = "N2";
            colSaleTotal.FillWeight = 15;

            flowLayoutPanel6.Controls.Add(labelSaleSearch);
            flowLayoutPanel6.Controls.Add(textBoxSaleSearch);
            flowLayoutPanel6.Controls.Add(labelSaleDateFrom);
            flowLayoutPanel6.Controls.Add(dateTimePickerSaleFrom);
            flowLayoutPanel6.Controls.Add(labelSaleDateTo);
            flowLayoutPanel6.Controls.Add(dateTimePickerSaleTo);
            flowLayoutPanel6.Controls.Add(labelSaleStatus);
            flowLayoutPanel6.Controls.Add(comboBoxSaleStatus);
            flowLayoutPanel6.Controls.Add(btnAddSale);
            flowLayoutPanel6.Controls.Add(btnEditSale);
            flowLayoutPanel6.Controls.Add(btnDeleteSale);
            flowLayoutPanel6.Controls.Add(btnPrintCheck);
            flowLayoutPanel6.Dock = DockStyle.Fill;
            flowLayoutPanel6.Location = new Point(3, 374);
            flowLayoutPanel6.Name = "flowLayoutPanel6";
            flowLayoutPanel6.Size = new Size(1013, 39);
            flowLayoutPanel6.TabIndex = 1;

            labelSaleSearch.Anchor = AnchorStyles.Left;
            labelSaleSearch.AutoSize = true;
            labelSaleSearch.Location = new Point(3, 10);
            labelSaleSearch.Name = "labelSaleSearch";
            labelSaleSearch.Size = new Size(44, 15);
            labelSaleSearch.TabIndex = 0;
            labelSaleSearch.Text = "Найти:";

            textBoxSaleSearch.Anchor = AnchorStyles.Left;
            textBoxSaleSearch.Location = new Point(53, 6);
            textBoxSaleSearch.Name = "textBoxSaleSearch";
            textBoxSaleSearch.Size = new Size(120, 23);
            textBoxSaleSearch.TabIndex = 1;

            labelSaleDateFrom.Anchor = AnchorStyles.Left;
            labelSaleDateFrom.AutoSize = true;
            labelSaleDateFrom.Location = new Point(179, 10);
            labelSaleDateFrom.Name = "labelSaleDateFrom";
            labelSaleDateFrom.Size = new Size(18, 15);
            labelSaleDateFrom.TabIndex = 2;
            labelSaleDateFrom.Text = "С:";

            dateTimePickerSaleFrom.Anchor = AnchorStyles.Left;
            dateTimePickerSaleFrom.Format = DateTimePickerFormat.Short;
            dateTimePickerSaleFrom.Location = new Point(203, 6);
            dateTimePickerSaleFrom.Name = "dateTimePickerSaleFrom";
            dateTimePickerSaleFrom.Size = new Size(110, 23);
            dateTimePickerSaleFrom.TabIndex = 3;

            labelSaleDateTo.Anchor = AnchorStyles.Left;
            labelSaleDateTo.AutoSize = true;
            labelSaleDateTo.Location = new Point(319, 10);
            labelSaleDateTo.Name = "labelSaleDateTo";
            labelSaleDateTo.Size = new Size(26, 15);
            labelSaleDateTo.TabIndex = 4;
            labelSaleDateTo.Text = "По:";

            dateTimePickerSaleTo.Anchor = AnchorStyles.Left;
            dateTimePickerSaleTo.Format = DateTimePickerFormat.Short;
            dateTimePickerSaleTo.Location = new Point(351, 6);
            dateTimePickerSaleTo.Name = "dateTimePickerSaleTo";
            dateTimePickerSaleTo.Size = new Size(110, 23);
            dateTimePickerSaleTo.TabIndex = 5;

            labelSaleStatus.Anchor = AnchorStyles.Left;
            labelSaleStatus.AutoSize = true;
            labelSaleStatus.Location = new Point(467, 10);
            labelSaleStatus.Name = "labelSaleStatus";
            labelSaleStatus.Size = new Size(46, 15);
            labelSaleStatus.TabIndex = 6;
            labelSaleStatus.Text = "Статус:";

            comboBoxSaleStatus.Anchor = AnchorStyles.Left;
            comboBoxSaleStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxSaleStatus.FormattingEnabled = true;
            comboBoxSaleStatus.Location = new Point(519, 6);
            comboBoxSaleStatus.Name = "comboBoxSaleStatus";
            comboBoxSaleStatus.Size = new Size(100, 23);
            comboBoxSaleStatus.TabIndex = 7;

            btnAddSale.Location = new Point(625, 3);
            btnAddSale.Name = "btnAddSale";
            btnAddSale.Size = new Size(100, 30);
            btnAddSale.TabIndex = 8;
            btnAddSale.Text = "Новая продажа";
            btnAddSale.UseVisualStyleBackColor = true;

            btnEditSale.Location = new Point(731, 3);
            btnEditSale.Name = "btnEditSale";
            btnEditSale.Size = new Size(70, 30);
            btnEditSale.TabIndex = 9;
            btnEditSale.Text = "Править";
            btnEditSale.UseVisualStyleBackColor = true;

            btnDeleteSale.Location = new Point(807, 3);
            btnDeleteSale.Name = "btnDeleteSale";
            btnDeleteSale.Size = new Size(70, 30);
            btnDeleteSale.TabIndex = 10;
            btnDeleteSale.Text = "Удалить";
            btnDeleteSale.UseVisualStyleBackColor = true;

            btnPrintCheck.Location = new Point(883, 3);
            btnPrintCheck.Name = "btnPrintCheck";
            btnPrintCheck.Size = new Size(100, 30);
            btnPrintCheck.TabIndex = 11;
            btnPrintCheck.Text = "Печать чека";
            btnPrintCheck.UseVisualStyleBackColor = true;

            // =============================================
            // tabPage7 — Отчеты (AutoGenerateColumns = true, т.к. столбцы динамические)
            // =============================================
            tabPage7.Controls.Add(tableLayoutPanel7);
            tabPage7.Location = new Point(4, 24);
            tabPage7.Name = "tabPage7";
            tabPage7.Padding = new Padding(3);
            tabPage7.Size = new Size(1025, 422);
            tabPage7.TabIndex = 6;
            tabPage7.Text = "Отчеты";
            tabPage7.UseVisualStyleBackColor = true;

            tableLayoutPanel7.ColumnCount = 1;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel7.Controls.Add(dataGridView7, 0, 0);
            tableLayoutPanel7.Controls.Add(flowLayoutPanel7, 0, 1);
            tableLayoutPanel7.Dock = DockStyle.Fill;
            tableLayoutPanel7.Location = new Point(3, 3);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowCount = 2;
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 89.35667F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 10.64333F));
            tableLayoutPanel7.Size = new Size(1019, 416);
            tableLayoutPanel7.TabIndex = 0;

            // dataGridView7 — Отчеты (динамические столбцы)
            dataGridView7.AllowUserToAddRows = false;
            dataGridView7.AllowUserToDeleteRows = false;
            dataGridView7.ReadOnly = true;
            dataGridView7.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView7.MultiSelect = false;
            dataGridView7.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView7.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView7.Dock = DockStyle.Fill;
            dataGridView7.Location = new Point(3, 3);
            dataGridView7.Name = "dataGridView7";
            dataGridView7.Size = new Size(1013, 365);
            dataGridView7.TabIndex = 0;

            flowLayoutPanel7.Controls.Add(labelRepDateFrom);
            flowLayoutPanel7.Controls.Add(dateTimePickerRepFrom);
            flowLayoutPanel7.Controls.Add(labelRepDateTo);
            flowLayoutPanel7.Controls.Add(dateTimePickerRepTo);
            flowLayoutPanel7.Controls.Add(labelRepCategory);
            flowLayoutPanel7.Controls.Add(comboBoxRepCategory);
            flowLayoutPanel7.Controls.Add(btnReportStock);
            flowLayoutPanel7.Controls.Add(btnReportSales);
            flowLayoutPanel7.Controls.Add(btnReportPurchases);
            flowLayoutPanel7.Controls.Add(btnReportProfit);
            flowLayoutPanel7.Controls.Add(btnExportExcel);
            flowLayoutPanel7.Dock = DockStyle.Fill;
            flowLayoutPanel7.Location = new Point(3, 374);
            flowLayoutPanel7.Name = "flowLayoutPanel7";
            flowLayoutPanel7.Size = new Size(1013, 39);
            flowLayoutPanel7.TabIndex = 1;

            labelRepDateFrom.Anchor = AnchorStyles.Left;
            labelRepDateFrom.AutoSize = true;
            labelRepDateFrom.Location = new Point(3, 10);
            labelRepDateFrom.Name = "labelRepDateFrom";
            labelRepDateFrom.Size = new Size(18, 15);
            labelRepDateFrom.TabIndex = 0;
            labelRepDateFrom.Text = "С:";

            dateTimePickerRepFrom.Anchor = AnchorStyles.Left;
            dateTimePickerRepFrom.Format = DateTimePickerFormat.Short;
            dateTimePickerRepFrom.Location = new Point(27, 6);
            dateTimePickerRepFrom.Name = "dateTimePickerRepFrom";
            dateTimePickerRepFrom.Size = new Size(110, 23);
            dateTimePickerRepFrom.TabIndex = 1;

            labelRepDateTo.Anchor = AnchorStyles.Left;
            labelRepDateTo.AutoSize = true;
            labelRepDateTo.Location = new Point(143, 10);
            labelRepDateTo.Name = "labelRepDateTo";
            labelRepDateTo.Size = new Size(26, 15);
            labelRepDateTo.TabIndex = 2;
            labelRepDateTo.Text = "По:";

            dateTimePickerRepTo.Anchor = AnchorStyles.Left;
            dateTimePickerRepTo.Format = DateTimePickerFormat.Short;
            dateTimePickerRepTo.Location = new Point(175, 6);
            dateTimePickerRepTo.Name = "dateTimePickerRepTo";
            dateTimePickerRepTo.Size = new Size(110, 23);
            dateTimePickerRepTo.TabIndex = 3;

            labelRepCategory.Anchor = AnchorStyles.Left;
            labelRepCategory.AutoSize = true;
            labelRepCategory.Location = new Point(291, 10);
            labelRepCategory.Name = "labelRepCategory";
            labelRepCategory.Size = new Size(66, 15);
            labelRepCategory.TabIndex = 4;
            labelRepCategory.Text = "Категория:";

            comboBoxRepCategory.Anchor = AnchorStyles.Left;
            comboBoxRepCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxRepCategory.FormattingEnabled = true;
            comboBoxRepCategory.Location = new Point(363, 6);
            comboBoxRepCategory.Name = "comboBoxRepCategory";
            comboBoxRepCategory.Size = new Size(121, 23);
            comboBoxRepCategory.TabIndex = 5;

            btnReportStock.Location = new Point(490, 3);
            btnReportStock.Name = "btnReportStock";
            btnReportStock.Size = new Size(70, 30);
            btnReportStock.TabIndex = 6;
            btnReportStock.Text = "Остатки";
            btnReportStock.UseVisualStyleBackColor = true;

            btnReportSales.Location = new Point(566, 3);
            btnReportSales.Name = "btnReportSales";
            btnReportSales.Size = new Size(70, 30);
            btnReportSales.TabIndex = 7;
            btnReportSales.Text = "Продажи";
            btnReportSales.UseVisualStyleBackColor = true;

            btnReportPurchases.Location = new Point(642, 3);
            btnReportPurchases.Name = "btnReportPurchases";
            btnReportPurchases.Size = new Size(70, 30);
            btnReportPurchases.TabIndex = 8;
            btnReportPurchases.Text = "Закупки";
            btnReportPurchases.UseVisualStyleBackColor = true;

            btnReportProfit.Location = new Point(718, 3);
            btnReportProfit.Name = "btnReportProfit";
            btnReportProfit.Size = new Size(70, 30);
            btnReportProfit.TabIndex = 9;
            btnReportProfit.Text = "Прибыль";
            btnReportProfit.UseVisualStyleBackColor = true;

            btnExportExcel.Location = new Point(794, 3);
            btnExportExcel.Name = "btnExportExcel";
            btnExportExcel.Size = new Size(70, 30);
            btnExportExcel.TabIndex = 10;
            btnExportExcel.Text = "Excel";
            btnExportExcel.UseVisualStyleBackColor = true;

            // =============================================
            // App (главная форма)
            // =============================================
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1033, 450);
            Controls.Add(tabControl1);
            Name = "App";
            Text = "Apple Store — Система учёта";

            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            tabPage3.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView3).EndInit();
            flowLayoutPanel3.ResumeLayout(false);
            flowLayoutPanel3.PerformLayout();
            tabPage4.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView4).EndInit();
            flowLayoutPanel4.ResumeLayout(false);
            flowLayoutPanel4.PerformLayout();
            tabPage5.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView5).EndInit();
            flowLayoutPanel5.ResumeLayout(false);
            flowLayoutPanel5.PerformLayout();
            tabPage6.ResumeLayout(false);
            tableLayoutPanel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView6).EndInit();
            flowLayoutPanel6.ResumeLayout(false);
            flowLayoutPanel6.PerformLayout();
            tabPage7.ResumeLayout(false);
            tableLayoutPanel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView7).EndInit();
            flowLayoutPanel7.ResumeLayout(false);
            flowLayoutPanel7.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        // === Основные элементы ===
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private TabPage tabPage6;
        private TabPage tabPage7;

        // === Вкладка 1: Товары ===
        private TableLayoutPanel tableLayoutPanel1;
        private DataGridView dataGridView1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label1;
        private TextBox textBox1;
        private Label label2;
        private TextBox textBox2;
        private Label label3;
        private TextBox textBox3;
        private Label label4;
        private ComboBox comboBox1;
        private Button button1;
        private Button button2;
        private Button button3;

        // Столбцы dataGridView1
        private DataGridViewTextBoxColumn colProdId;
        private DataGridViewTextBoxColumn colProdName;
        private DataGridViewTextBoxColumn colProdCategory;
        private DataGridViewTextBoxColumn colProdPurchasePrice;
        private DataGridViewTextBoxColumn colProdSalePrice;
        private DataGridViewTextBoxColumn colProdStock;

        // === Вкладка 2: Категории ===
        private TableLayoutPanel tableLayoutPanel2;
        private DataGridView dataGridView2;
        private FlowLayoutPanel flowLayoutPanel2;
        private Label labelCatSearch;
        private TextBox textBoxCatSearch;
        private Button btnAddCategory;
        private Button btnEditCategory;
        private Button btnDeleteCategory;

        // Столбцы dataGridView2
        private DataGridViewTextBoxColumn colCatId;
        private DataGridViewTextBoxColumn colCatName;

        // === Вкладка 3: Поставщики ===
        private TableLayoutPanel tableLayoutPanel3;
        private DataGridView dataGridView3;
        private FlowLayoutPanel flowLayoutPanel3;
        private Label labelSupSearch;
        private TextBox textBoxSupSearch;
        private Button btnAddSupplier;
        private Button btnEditSupplier;
        private Button btnDeleteSupplier;

        // Столбцы dataGridView3
        private DataGridViewTextBoxColumn colSupId;
        private DataGridViewTextBoxColumn colSupName;
        private DataGridViewTextBoxColumn colSupContact;
        private DataGridViewTextBoxColumn colSupPhone;
        private DataGridViewTextBoxColumn colSupEmail;
        private DataGridViewTextBoxColumn colSupAddress;

        // === Вкладка 4: Покупатели ===
        private TableLayoutPanel tableLayoutPanel4;
        private DataGridView dataGridView4;
        private FlowLayoutPanel flowLayoutPanel4;
        private Label labelCustSearch;
        private TextBox textBoxCustSearch;
        private Label labelCustType;
        private ComboBox comboBoxCustType;
        private Button btnAddCustomer;
        private Button btnEditCustomer;
        private Button btnDeleteCustomer;

        // Столбцы dataGridView4
        private DataGridViewTextBoxColumn colCustId;
        private DataGridViewTextBoxColumn colCustName;
        private DataGridViewTextBoxColumn colCustType;
        private DataGridViewTextBoxColumn colCustPhone;
        private DataGridViewTextBoxColumn colCustEmail;
        private DataGridViewTextBoxColumn colCustAddress;

        // === Вкладка 5: Закупки ===
        private TableLayoutPanel tableLayoutPanel5;
        private DataGridView dataGridView5;
        private FlowLayoutPanel flowLayoutPanel5;
        private Label labelPurSearch;
        private TextBox textBoxPurSearch;
        private Label labelPurSupplier;
        private ComboBox comboBoxPurSupplier;
        private Label labelPurDateFrom;
        private DateTimePicker dateTimePickerPurFrom;
        private Label labelPurDateTo;
        private DateTimePicker dateTimePickerPurTo;
        private Button btnAddPurchase;
        private Button btnEditPurchase;
        private Button btnDeletePurchase;

        // Столбцы dataGridView5
        private DataGridViewTextBoxColumn colPurId;
        private DataGridViewTextBoxColumn colPurProduct;
        private DataGridViewTextBoxColumn colPurSupplier;
        private DataGridViewTextBoxColumn colPurQuantity;
        private DataGridViewTextBoxColumn colPurPrice;
        private DataGridViewTextBoxColumn colPurTotal;
        private DataGridViewTextBoxColumn colPurDate;

        // === Вкладка 6: Продажи ===
        private TableLayoutPanel tableLayoutPanel6;
        private DataGridView dataGridView6;
        private FlowLayoutPanel flowLayoutPanel6;
        private Label labelSaleSearch;
        private TextBox textBoxSaleSearch;
        private Label labelSaleDateFrom;
        private DateTimePicker dateTimePickerSaleFrom;
        private Label labelSaleDateTo;
        private DateTimePicker dateTimePickerSaleTo;
        private Label labelSaleStatus;
        private ComboBox comboBoxSaleStatus;
        private Button btnAddSale;
        private Button btnEditSale;
        private Button btnDeleteSale;
        private Button btnPrintCheck;

        // Столбцы dataGridView6
        private DataGridViewTextBoxColumn colSaleId;
        private DataGridViewTextBoxColumn colSaleCustomer;
        private DataGridViewTextBoxColumn colSaleDate;
        private DataGridViewTextBoxColumn colSaleStatus;
        private DataGridViewTextBoxColumn colSaleTotal;

        // === Вкладка 7: Отчеты ===
        private TableLayoutPanel tableLayoutPanel7;
        private DataGridView dataGridView7;
        private FlowLayoutPanel flowLayoutPanel7;
        private Label labelRepDateFrom;
        private DateTimePicker dateTimePickerRepFrom;
        private Label labelRepDateTo;
        private DateTimePicker dateTimePickerRepTo;
        private Label labelRepCategory;
        private ComboBox comboBoxRepCategory;
        private Button btnReportStock;
        private Button btnReportSales;
        private Button btnReportPurchases;
        private Button btnReportProfit;
        private Button btnExportExcel;
    }
}