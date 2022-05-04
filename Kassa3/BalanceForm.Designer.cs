namespace Kassa3
{
    partial class BalanceForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbItoCource = new System.Windows.Forms.GroupBox();
            this.lbCurr = new System.Windows.Forms.ListBox();
            this.gbFirmsAcc = new System.Windows.Forms.GroupBox();
            this.clbAccounts = new System.Windows.Forms.CheckedListBox();
            this.gbPeriod = new System.Windows.Forms.GroupBox();
            this.bRefresh = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.deEnd = new DEXExtendLib.DateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.deStart = new DEXExtendLib.DateEdit();
            this.cbFromDate = new System.Windows.Forms.CheckBox();
            this.ss = new System.Windows.Forms.StatusStrip();
            this.gc = new DevExpress.XtraGrid.GridControl();
            this.gv = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.gbItoCource.SuspendLayout();
            this.gbFirmsAcc.SuspendLayout();
            this.gbPeriod.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gbItoCource);
            this.panel1.Controls.Add(this.gbFirmsAcc);
            this.panel1.Controls.Add(this.gbPeriod);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(644, 129);
            this.panel1.TabIndex = 0;
            // 
            // gbItoCource
            // 
            this.gbItoCource.Controls.Add(this.lbCurr);
            this.gbItoCource.Location = new System.Drawing.Point(386, 3);
            this.gbItoCource.Name = "gbItoCource";
            this.gbItoCource.Size = new System.Drawing.Size(246, 118);
            this.gbItoCource.TabIndex = 2;
            this.gbItoCource.TabStop = false;
            this.gbItoCource.Text = "Курсы валют";
            // 
            // lbCurr
            // 
            this.lbCurr.FormattingEnabled = true;
            this.lbCurr.Location = new System.Drawing.Point(6, 19);
            this.lbCurr.Name = "lbCurr";
            this.lbCurr.Size = new System.Drawing.Size(234, 95);
            this.lbCurr.TabIndex = 0;
            this.lbCurr.DoubleClick += new System.EventHandler(this.lbCurr_DoubleClick);
            // 
            // gbFirmsAcc
            // 
            this.gbFirmsAcc.Controls.Add(this.clbAccounts);
            this.gbFirmsAcc.Location = new System.Drawing.Point(143, 3);
            this.gbFirmsAcc.Name = "gbFirmsAcc";
            this.gbFirmsAcc.Size = new System.Drawing.Size(237, 118);
            this.gbFirmsAcc.TabIndex = 1;
            this.gbFirmsAcc.TabStop = false;
            this.gbFirmsAcc.Text = "Фирмы и счета";
            // 
            // clbAccounts
            // 
            this.clbAccounts.FormattingEnabled = true;
            this.clbAccounts.Location = new System.Drawing.Point(6, 19);
            this.clbAccounts.Name = "clbAccounts";
            this.clbAccounts.Size = new System.Drawing.Size(223, 94);
            this.clbAccounts.TabIndex = 2;
            this.clbAccounts.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbAccounts_ItemCheck);
            this.clbAccounts.MouseDown += new System.Windows.Forms.MouseEventHandler(this.clbAccounts_MouseDown);
            // 
            // gbPeriod
            // 
            this.gbPeriod.Controls.Add(this.bRefresh);
            this.gbPeriod.Controls.Add(this.label2);
            this.gbPeriod.Controls.Add(this.deEnd);
            this.gbPeriod.Controls.Add(this.label1);
            this.gbPeriod.Controls.Add(this.deStart);
            this.gbPeriod.Controls.Add(this.cbFromDate);
            this.gbPeriod.Location = new System.Drawing.Point(3, 3);
            this.gbPeriod.Name = "gbPeriod";
            this.gbPeriod.Size = new System.Drawing.Size(134, 118);
            this.gbPeriod.TabIndex = 0;
            this.gbPeriod.TabStop = false;
            this.gbPeriod.Text = "Период";
            // 
            // bRefresh
            // 
            this.bRefresh.Location = new System.Drawing.Point(45, 84);
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Size = new System.Drawing.Size(79, 23);
            this.bRefresh.TabIndex = 1;
            this.bRefresh.Text = "Установить";
            this.bRefresh.UseVisualStyleBackColor = true;
            this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "(включительно)";
            // 
            // deEnd
            // 
            this.deEnd.FormattingEnabled = true;
            this.deEnd.InputChar = '_';
            this.deEnd.Location = new System.Drawing.Point(45, 44);
            this.deEnd.MaxLength = 10;
            this.deEnd.Name = "deEnd";
            this.deEnd.Size = new System.Drawing.Size(80, 21);
            this.deEnd.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "по";
            // 
            // deStart
            // 
            this.deStart.FormattingEnabled = true;
            this.deStart.InputChar = '_';
            this.deStart.Location = new System.Drawing.Point(45, 17);
            this.deStart.MaxLength = 10;
            this.deStart.Name = "deStart";
            this.deStart.Size = new System.Drawing.Size(80, 21);
            this.deStart.TabIndex = 1;
            // 
            // cbFromDate
            // 
            this.cbFromDate.AutoSize = true;
            this.cbFromDate.Location = new System.Drawing.Point(6, 19);
            this.cbFromDate.Name = "cbFromDate";
            this.cbFromDate.Size = new System.Drawing.Size(33, 17);
            this.cbFromDate.TabIndex = 1;
            this.cbFromDate.Text = "С";
            this.cbFromDate.UseVisualStyleBackColor = true;
            this.cbFromDate.CheckedChanged += new System.EventHandler(this.cbFromDate_CheckedChanged);
            // 
            // ss
            // 
            this.ss.Location = new System.Drawing.Point(0, 465);
            this.ss.Name = "ss";
            this.ss.Size = new System.Drawing.Size(644, 22);
            this.ss.TabIndex = 1;
            this.ss.Text = "statusStrip1";
            // 
            // gc
            // 
            this.gc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gc.EmbeddedNavigator.Enabled = false;
            this.gc.Location = new System.Drawing.Point(0, 129);
            this.gc.MainView = this.gv;
            this.gc.Name = "gc";
            this.gc.Size = new System.Drawing.Size(644, 336);
            this.gc.TabIndex = 2;
            this.gc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gv});
            // 
            // gv
            // 
            this.gv.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1});
            this.gv.GridControl = this.gc;
            this.gv.Name = "gv";
            this.gv.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gv.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gv.OptionsBehavior.AutoPopulateColumns = false;
            this.gv.OptionsBehavior.Editable = false;
            this.gv.OptionsBehavior.ReadOnly = true;
            this.gv.OptionsCustomization.AllowGroup = false;
            this.gv.OptionsCustomization.AllowQuickHideColumns = false;
            this.gv.OptionsMenu.EnableColumnMenu = false;
            this.gv.OptionsMenu.EnableGroupPanelMenu = false;
            this.gv.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gv.OptionsView.EnableAppearanceEvenRow = true;
            this.gv.OptionsView.EnableAppearanceOddRow = true;
            this.gv.OptionsView.ShowFooter = true;
            this.gv.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.gv.OptionsView.ShowGroupPanel = false;
            this.gv.OptionsView.ShowIndicator = false;
            this.gv.CustomDrawFooterCell += new DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventHandler(this.gv_CustomDrawFooterCell);
            this.gv.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.formatCurrencyField);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "gridColumn1";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 300;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // BalanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 487);
            this.Controls.Add(this.gc);
            this.Controls.Add(this.ss);
            this.Controls.Add(this.panel1);
            this.Name = "BalanceForm";
            this.Text = "Баланс";
            this.Load += new System.EventHandler(this.BalanceForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BalanceForm_FormClosing);
            this.panel1.ResumeLayout(false);
            this.gbItoCource.ResumeLayout(false);
            this.gbFirmsAcc.ResumeLayout(false);
            this.gbPeriod.ResumeLayout(false);
            this.gbPeriod.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip ss;
        private System.Windows.Forms.GroupBox gbPeriod;
        private System.Windows.Forms.CheckBox cbFromDate;
        private DEXExtendLib.DateEdit deEnd;
        private System.Windows.Forms.Label label1;
        private DEXExtendLib.DateEdit deStart;
        private System.Windows.Forms.GroupBox gbFirmsAcc;
        private System.Windows.Forms.Button bRefresh;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox clbAccounts;
        private System.Windows.Forms.GroupBox gbItoCource;
        private DevExpress.XtraGrid.GridControl gc;
        private DevExpress.XtraGrid.Views.Grid.GridView gv;
        private System.Windows.Forms.ListBox lbCurr;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private System.Windows.Forms.Timer timer1;
    }
}