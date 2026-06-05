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
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();

            tabPage2 = new TabPage();
            tableLayoutPanel2 = new TableLayoutPanel();
            dataGridView2 = new DataGridView();
            flowLayoutPanel2 = new FlowLayoutPanel();
            btnAddCategory = new Button();
            btnEditCategory = new Button();
            btnDeleteCategory = new Button();
            btnRefreshCategory = new Button();

            tabPage3 = new TabPage();
            tableLayoutPanel3 = new TableLayoutPanel();
            dataGridView3 = new DataGridView();
            flowLayoutPanel3 = new FlowLayoutPanel();
            btnAddSupplier = new Button();
            btnEditSupplier = new Button();
            btnDeleteSupplier = new Button();
            btnRefreshSupplier = new Button();

            tabPage4 = new TabPage();
            tableLayoutPanel4 = new TableLayoutPanel();
            dataGridView4 = new DataGridView();
            flowLayoutPanel4 = new FlowLayoutPanel();
            btnAddPurchase = new Button();
            btnRefreshPurchase = new Button();

            tabPage5 = new TabPage();
            tableLayoutPanel5 = new TableLayoutPanel();
            dataGridView5 = new DataGridView();
            flowLayoutPanel5 = new FlowLayoutPanel();
            btnAddSale = new Button();
            btnRefreshSale = new Button();
            btnPrintCheck = new Button();

            tabPage6 = new TabPage();
            tableLayoutPanel6 = new TableLayoutPanel();
            dataGridView6 = new DataGridView();
            flowLayoutPanel6 = new FlowLayoutPanel();
            btnReportStock = new Button();
            btnReportSales = new Button();
            btnReportPurchases = new Button();
            btnReportProfit = new Button();
            btnExportExcel = new Button();

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

            SuspendLayout();

            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Controls.Add(tabPage6);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(800, 450);
            tabControl1.TabIndex = 0;

            // 
            // tabPage1 - Товары
            // 
            tabPage1.Controls.Add(tableLayoutPanel1);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(792, 422);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Товары";
            tabPage1.UseVisualStyleBackColor = true;

            // 
            // tableLayoutPanel1
            // 
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
            tableLayoutPanel1.Size = new Size(786, 416);
            tableLayoutPanel1.TabIndex = 3;

            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(3, 3);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(780, 365);
            dataGridView1.TabIndex = 1;

            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(button1);
            flowLayoutPanel1.Controls.Add(button2);
            flowLayoutPanel1.Controls.Add(button3);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(3, 374);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(780, 39);
            flowLayoutPanel1.TabIndex = 2;

            // 
            // button1
            // 
            button1.Location = new Point(3, 3);
            button1.Name = "button1";
            button1.Size = new Size(70, 30);
            button1.TabIndex = 0;
            button1.Text = "Добавить";
            button1.UseVisualStyleBackColor = true;

            // 
            // button2
            // 
            button2.Location = new Point(79, 3);
            button2.Name = "button2";
            button2.Size = new Size(70, 30);
            button2.TabIndex = 1;
            button2.Text = "Править";
            button2.UseVisualStyleBackColor = true;

            // 
            // button3
            // 
            button3.Location = new Point(155, 3);
            button3.Name = "button3";
            button3.Size = new Size(70, 30);
            button3.TabIndex = 2;
            button3.Text = "Удалить";
            button3.UseVisualStyleBackColor = true;

            // 
            // tabPage2 - Категории
            // 
            tabPage2.Controls.Add(tableLayoutPanel2);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(792, 422);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Категории";
            tabPage2.UseVisualStyleBackColor = true;

            // 
            // tableLayoutPanel2
            // 
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
            tableLayoutPanel2.Size = new Size(786, 416);
            tableLayoutPanel2.TabIndex = 0;

            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(3, 3);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.Size = new Size(780, 365);
            dataGridView2.TabIndex = 0;

            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Controls.Add(btnAddCategory);
            flowLayoutPanel2.Controls.Add(btnEditCategory);
            flowLayoutPanel2.Controls.Add(btnDeleteCategory);
            flowLayoutPanel2.Controls.Add(btnRefreshCategory);
            flowLayoutPanel2.Dock = DockStyle.Fill;
            flowLayoutPanel2.Location = new Point(3, 374);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(780, 39);
            flowLayoutPanel2.TabIndex = 1;

            // 
            // btnAddCategory
            // 
            btnAddCategory.Location = new Point(3, 3);
            btnAddCategory.Name = "btnAddCategory";
            btnAddCategory.Size = new Size(70, 30);
            btnAddCategory.TabIndex = 0;
            btnAddCategory.Text = "Добавить";
            btnAddCategory.UseVisualStyleBackColor = true;

            // 
            // btnEditCategory
            // 
            btnEditCategory.Location = new Point(79, 3);
            btnEditCategory.Name = "btnEditCategory";
            btnEditCategory.Size = new Size(70, 30);
            btnEditCategory.TabIndex = 1;
            btnEditCategory.Text = "Править";
            btnEditCategory.UseVisualStyleBackColor = true;

            // 
            // btnDeleteCategory
            // 
            btnDeleteCategory.Location = new Point(155, 3);
            btnDeleteCategory.Name = "btnDeleteCategory";
            btnDeleteCategory.Size = new Size(70, 30);
            btnDeleteCategory.TabIndex = 2;
            btnDeleteCategory.Text = "Удалить";
            btnDeleteCategory.UseVisualStyleBackColor = true;

            // 
            // btnRefreshCategory
            // 
            btnRefreshCategory.Location = new Point(231, 3);
            btnRefreshCategory.Name = "btnRefreshCategory";
            btnRefreshCategory.Size = new Size(70, 30);
            btnRefreshCategory.TabIndex = 3;
            btnRefreshCategory.Text = "Обновить";
            btnRefreshCategory.UseVisualStyleBackColor = true;

            // 
            // tabPage3 - Поставщики
            // 
            tabPage3.Controls.Add(tableLayoutPanel3);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(792, 422);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Поставщики";
            tabPage3.UseVisualStyleBackColor = true;

            // 
            // tableLayoutPanel3
            // 
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
            tableLayoutPanel3.Size = new Size(786, 416);
            tableLayoutPanel3.TabIndex = 0;

            // 
            // dataGridView3
            // 
            dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView3.Dock = DockStyle.Fill;
            dataGridView3.Location = new Point(3, 3);
            dataGridView3.Name = "dataGridView3";
            dataGridView3.Size = new Size(780, 365);
            dataGridView3.TabIndex = 0;

            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.Controls.Add(btnAddSupplier);
            flowLayoutPanel3.Controls.Add(btnEditSupplier);
            flowLayoutPanel3.Controls.Add(btnDeleteSupplier);
            flowLayoutPanel3.Controls.Add(btnRefreshSupplier);
            flowLayoutPanel3.Dock = DockStyle.Fill;
            flowLayoutPanel3.Location = new Point(3, 374);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new Size(780, 39);
            flowLayoutPanel3.TabIndex = 1;

            // 
            // btnAddSupplier
            // 
            btnAddSupplier.Location = new Point(3, 3);
            btnAddSupplier.Name = "btnAddSupplier";
            btnAddSupplier.Size = new Size(100, 30);
            btnAddSupplier.TabIndex = 0;
            btnAddSupplier.Text = "Добавить";
            btnAddSupplier.UseVisualStyleBackColor = true;

            // 
            // btnEditSupplier
            // 
            btnEditSupplier.Location = new Point(109, 3);
            btnEditSupplier.Name = "btnEditSupplier";
            btnEditSupplier.Size = new Size(70, 30);
            btnEditSupplier.TabIndex = 1;
            btnEditSupplier.Text = "Править";
            btnEditSupplier.UseVisualStyleBackColor = true;

            // 
            // btnDeleteSupplier
            // 
            btnDeleteSupplier.Location = new Point(185, 3);
            btnDeleteSupplier.Name = "btnDeleteSupplier";
            btnDeleteSupplier.Size = new Size(70, 30);
            btnDeleteSupplier.TabIndex = 2;
            btnDeleteSupplier.Text = "Удалить";
            btnDeleteSupplier.UseVisualStyleBackColor = true;

            // 
            // btnRefreshSupplier
            // 
            btnRefreshSupplier.Location = new Point(261, 3);
            btnRefreshSupplier.Name = "btnRefreshSupplier";
            btnRefreshSupplier.Size = new Size(70, 30);
            btnRefreshSupplier.TabIndex = 3;
            btnRefreshSupplier.Text = "Обновить";
            btnRefreshSupplier.UseVisualStyleBackColor = true;

            // 
            // tabPage4 - Закупки
            // 
            tabPage4.Controls.Add(tableLayoutPanel4);
            tabPage4.Location = new Point(4, 24);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(792, 422);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Закупки";
            tabPage4.UseVisualStyleBackColor = true;

            // 
            // tableLayoutPanel4
            // 
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
            tableLayoutPanel4.Size = new Size(786, 416);
            tableLayoutPanel4.TabIndex = 0;

            // 
            // dataGridView4
            // 
            dataGridView4.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView4.Dock = DockStyle.Fill;
            dataGridView4.Location = new Point(3, 3);
            dataGridView4.Name = "dataGridView4";
            dataGridView4.Size = new Size(780, 365);
            dataGridView4.TabIndex = 0;

            // 
            // flowLayoutPanel4
            // 
            flowLayoutPanel4.Controls.Add(btnAddPurchase);
            flowLayoutPanel4.Controls.Add(btnRefreshPurchase);
            flowLayoutPanel4.Dock = DockStyle.Fill;
            flowLayoutPanel4.Location = new Point(3, 374);
            flowLayoutPanel4.Name = "flowLayoutPanel4";
            flowLayoutPanel4.Size = new Size(780, 39);
            flowLayoutPanel4.TabIndex = 1;

            // 
            // btnAddPurchase
            // 
            btnAddPurchase.Location = new Point(3, 3);
            btnAddPurchase.Name = "btnAddPurchase";
            btnAddPurchase.Size = new Size(100, 30);
            btnAddPurchase.TabIndex = 0;
            btnAddPurchase.Text = "Новая закупка";
            btnAddPurchase.UseVisualStyleBackColor = true;

            // 
            // btnRefreshPurchase
            // 
            btnRefreshPurchase.Location = new Point(109, 3);
            btnRefreshPurchase.Name = "btnRefreshPurchase";
            btnRefreshPurchase.Size = new Size(70, 30);
            btnRefreshPurchase.TabIndex = 1;
            btnRefreshPurchase.Text = "Обновить";
            btnRefreshPurchase.UseVisualStyleBackColor = true;

            // 
            // tabPage5 - Продажи
            // 
            tabPage5.Controls.Add(tableLayoutPanel5);
            tabPage5.Location = new Point(4, 24);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(3);
            tabPage5.Size = new Size(792, 422);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "Продажи";
            tabPage5.UseVisualStyleBackColor = true;

            // 
            // tableLayoutPanel5
            // 
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
            tableLayoutPanel5.Size = new Size(786, 416);
            tableLayoutPanel5.TabIndex = 0;

            // 
            // dataGridView5
            // 
            dataGridView5.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView5.Dock = DockStyle.Fill;
            dataGridView5.Location = new Point(3, 3);
            dataGridView5.Name = "dataGridView5";
            dataGridView5.Size = new Size(780, 365);
            dataGridView5.TabIndex = 0;

            // 
            // flowLayoutPanel5
            // 
            flowLayoutPanel5.Controls.Add(btnAddSale);
            flowLayoutPanel5.Controls.Add(btnRefreshSale);
            flowLayoutPanel5.Controls.Add(btnPrintCheck);
            flowLayoutPanel5.Dock = DockStyle.Fill;
            flowLayoutPanel5.Location = new Point(3, 374);
            flowLayoutPanel5.Name = "flowLayoutPanel5";
            flowLayoutPanel5.Size = new Size(780, 39);
            flowLayoutPanel5.TabIndex = 1;

            // 
            // btnAddSale
            // 
            btnAddSale.Location = new Point(3, 3);
            btnAddSale.Name = "btnAddSale";
            btnAddSale.Size = new Size(100, 30);
            btnAddSale.TabIndex = 0;
            btnAddSale.Text = "Новая продажа";
            btnAddSale.UseVisualStyleBackColor = true;

            // 
            // btnRefreshSale
            // 
            btnRefreshSale.Location = new Point(109, 3);
            btnRefreshSale.Name = "btnRefreshSale";
            btnRefreshSale.Size = new Size(70, 30);
            btnRefreshSale.TabIndex = 1;
            btnRefreshSale.Text = "Обновить";
            btnRefreshSale.UseVisualStyleBackColor = true;

            // 
            // btnPrintCheck
            // 
            btnPrintCheck.Location = new Point(185, 3);
            btnPrintCheck.Name = "btnPrintCheck";
            btnPrintCheck.Size = new Size(100, 30);
            btnPrintCheck.TabIndex = 2;
            btnPrintCheck.Text = "Печать чека";
            btnPrintCheck.UseVisualStyleBackColor = true;

            // 
            // tabPage6 - Отчеты
            // 
            tabPage6.Controls.Add(tableLayoutPanel6);
            tabPage6.Location = new Point(4, 24);
            tabPage6.Name = "tabPage6";
            tabPage6.Padding = new Padding(3);
            tabPage6.Size = new Size(792, 422);
            tabPage6.TabIndex = 5;
            tabPage6.Text = "Отчеты";
            tabPage6.UseVisualStyleBackColor = true;

            // 
            // tableLayoutPanel6
            // 
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
            tableLayoutPanel6.Size = new Size(786, 416);
            tableLayoutPanel6.TabIndex = 0;

            // 
            // dataGridView6
            // 
            dataGridView6.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView6.Dock = DockStyle.Fill;
            dataGridView6.Location = new Point(3, 3);
            dataGridView6.Name = "dataGridView6";
            dataGridView6.Size = new Size(780, 365);
            dataGridView6.TabIndex = 0;

            // 
            // flowLayoutPanel6
            // 
            flowLayoutPanel6.Controls.Add(btnReportStock);
            flowLayoutPanel6.Controls.Add(btnReportSales);
            flowLayoutPanel6.Controls.Add(btnReportPurchases);
            flowLayoutPanel6.Controls.Add(btnReportProfit);
            flowLayoutPanel6.Controls.Add(btnExportExcel);
            flowLayoutPanel6.Dock = DockStyle.Fill;
            flowLayoutPanel6.Location = new Point(3, 374);
            flowLayoutPanel6.Name = "flowLayoutPanel6";
            flowLayoutPanel6.Size = new Size(780, 39);
            flowLayoutPanel6.TabIndex = 1;

            // 
            // btnReportStock
            // 
            btnReportStock.Location = new Point(3, 3);
            btnReportStock.Name = "btnReportStock";
            btnReportStock.Size = new Size(70, 30);
            btnReportStock.TabIndex = 0;
            btnReportStock.Text = "Остатки";
            btnReportStock.UseVisualStyleBackColor = true;

            // 
            // btnReportSales
            // 
            btnReportSales.Location = new Point(79, 3);
            btnReportSales.Name = "btnReportSales";
            btnReportSales.Size = new Size(70, 30);
            btnReportSales.TabIndex = 1;
            btnReportSales.Text = "Продажи";
            btnReportSales.UseVisualStyleBackColor = true;

            // 
            // btnReportPurchases
            // 
            btnReportPurchases.Location = new Point(155, 3);
            btnReportPurchases.Name = "btnReportPurchases";
            btnReportPurchases.Size = new Size(70, 30);
            btnReportPurchases.TabIndex = 2;
            btnReportPurchases.Text = "Закупки";
            btnReportPurchases.UseVisualStyleBackColor = true;

            // 
            // btnReportProfit
            // 
            btnReportProfit.Location = new Point(231, 3);
            btnReportProfit.Name = "btnReportProfit";
            btnReportProfit.Size = new Size(70, 30);
            btnReportProfit.TabIndex = 3;
            btnReportProfit.Text = "Прибыль";
            btnReportProfit.UseVisualStyleBackColor = true;

            // 
            // btnExportExcel
            // 
            btnExportExcel.Location = new Point(307, 3);
            btnExportExcel.Name = "btnExportExcel";
            btnExportExcel.Size = new Size(70, 30);
            btnExportExcel.TabIndex = 4;
            btnExportExcel.Text = "Excel";
            btnExportExcel.UseVisualStyleBackColor = true;

            // 
            // App
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tabControl1);
            Name = "App";
            Text = "App";

            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            flowLayoutPanel1.ResumeLayout(false);

            tabPage2.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            flowLayoutPanel2.ResumeLayout(false);

            tabPage3.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView3).EndInit();
            flowLayoutPanel3.ResumeLayout(false);

            tabPage4.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView4).EndInit();
            flowLayoutPanel4.ResumeLayout(false);

            tabPage5.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView5).EndInit();
            flowLayoutPanel5.ResumeLayout(false);

            tabPage6.ResumeLayout(false);
            tableLayoutPanel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView6).EndInit();
            flowLayoutPanel6.ResumeLayout(false);

            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private TabPage tabPage6;

        private TableLayoutPanel tableLayoutPanel1;
        private DataGridView dataGridView1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button button1;
        private Button button2;
        private Button button3;

        private TableLayoutPanel tableLayoutPanel2;
        private DataGridView dataGridView2;
        private FlowLayoutPanel flowLayoutPanel2;
        private Button btnAddCategory;
        private Button btnEditCategory;
        private Button btnDeleteCategory;
        private Button btnRefreshCategory;

        private TableLayoutPanel tableLayoutPanel3;
        private DataGridView dataGridView3;
        private FlowLayoutPanel flowLayoutPanel3;
        private Button btnAddSupplier;
        private Button btnEditSupplier;
        private Button btnDeleteSupplier;
        private Button btnRefreshSupplier;

        private TableLayoutPanel tableLayoutPanel4;
        private DataGridView dataGridView4;
        private FlowLayoutPanel flowLayoutPanel4;
        private Button btnAddPurchase;
        private Button btnRefreshPurchase;

        private TableLayoutPanel tableLayoutPanel5;
        private DataGridView dataGridView5;
        private FlowLayoutPanel flowLayoutPanel5;
        private Button btnAddSale;
        private Button btnRefreshSale;
        private Button btnPrintCheck;

        private TableLayoutPanel tableLayoutPanel6;
        private DataGridView dataGridView6;
        private FlowLayoutPanel flowLayoutPanel6;
        private Button btnReportStock;
        private Button btnReportSales;
        private Button btnReportPurchases;
        private Button btnReportProfit;
        private Button btnExportExcel;
    }
}