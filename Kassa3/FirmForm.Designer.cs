namespace Kassa3
{
    partial class FirmForm
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
            this.pControls = new System.Windows.Forms.Panel();
            this.cbShowDeleted = new System.Windows.Forms.CheckBox();
            this.bPrint = new System.Windows.Forms.Button();
            this.bExport = new System.Windows.Forms.Button();
            this.gbColumns = new System.Windows.Forms.GroupBox();
            this.clbColumns = new System.Windows.Forms.CheckedListBox();
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.cbShowBalance = new System.Windows.Forms.CheckBox();
            this.cbShowInternal = new System.Windows.Forms.CheckBox();
            this.gbInterval = new System.Windows.Forms.GroupBox();
            this.cbEnd = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.bRefreshList = new System.Windows.Forms.Button();
            this.deEnd = new DEXExtendLib.DateEdit();
            this.deStart = new DEXExtendLib.DateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.ss = new System.Windows.Forms.StatusStrip();
            this.tspb = new System.Windows.Forms.ToolStripProgressBar();
            this.tssl = new System.Windows.Forms.ToolStripStatusLabel();
            this.gc = new DevExpress.XtraGrid.GridControl();
            this.bgv = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cmsExport = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiXls2000 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiXlsx2007 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsOptions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiDate = new System.Windows.Forms.ToolStripMenuItem();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.pControls.SuspendLayout();
            this.gbColumns.SuspendLayout();
            this.gbOptions.SuspendLayout();
            this.gbInterval.SuspendLayout();
            this.ss.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bgv)).BeginInit();
            this.cmsExport.SuspendLayout();
            this.cmsOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pControls
            // 
            this.pControls.Controls.Add(this.cbShowDeleted);
            this.pControls.Controls.Add(this.bPrint);
            this.pControls.Controls.Add(this.bExport);
            this.pControls.Controls.Add(this.gbColumns);
            this.pControls.Controls.Add(this.gbOptions);
            this.pControls.Controls.Add(this.gbInterval);
            this.pControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.pControls.Location = new System.Drawing.Point(0, 0);
            this.pControls.Name = "pControls";
            this.pControls.Size = new System.Drawing.Size(684, 115);
            this.pControls.TabIndex = 0;
            // 
            // cbShowDeleted
            // 
            this.cbShowDeleted.AutoSize = true;
            this.cbShowDeleted.Location = new System.Drawing.Point(232, 55);
            this.cbShowDeleted.Name = "cbShowDeleted";
            this.cbShowDeleted.Size = new System.Drawing.Size(147, 17);
            this.cbShowDeleted.TabIndex = 5;
            this.cbShowDeleted.Text = "Показывать удалённые";
            this.cbShowDeleted.UseVisualStyleBackColor = true;
            // 
            // bPrint
            // 
            this.bPrint.Enabled = false;
            this.bPrint.Location = new System.Drawing.Point(306, 76);
            this.bPrint.Name = "bPrint";
            this.bPrint.Size = new System.Drawing.Size(73, 23);
            this.bPrint.TabIndex = 4;
            this.bPrint.Text = "Печать";
            this.bPrint.UseVisualStyleBackColor = true;
            this.bPrint.Click += new System.EventHandler(this.bPrint_Click);
            // 
            // bExport
            // 
            this.bExport.Enabled = false;
            this.bExport.Location = new System.Drawing.Point(226, 76);
            this.bExport.Name = "bExport";
            this.bExport.Size = new System.Drawing.Size(72, 23);
            this.bExport.TabIndex = 3;
            this.bExport.Text = "Экспорт";
            this.bExport.UseVisualStyleBackColor = true;
            this.bExport.Click += new System.EventHandler(this.bExport_Click);
            // 
            // gbColumns
            // 
            this.gbColumns.Controls.Add(this.clbColumns);
            this.gbColumns.Location = new System.Drawing.Point(385, 3);
            this.gbColumns.Name = "gbColumns";
            this.gbColumns.Size = new System.Drawing.Size(164, 100);
            this.gbColumns.TabIndex = 2;
            this.gbColumns.TabStop = false;
            this.gbColumns.Text = "Отображать колонки";
            // 
            // clbColumns
            // 
            this.clbColumns.FormattingEnabled = true;
            this.clbColumns.Location = new System.Drawing.Point(6, 15);
            this.clbColumns.Name = "clbColumns";
            this.clbColumns.Size = new System.Drawing.Size(152, 79);
            this.clbColumns.TabIndex = 0;
            this.clbColumns.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbColumns_ItemCheck);
            this.clbColumns.MouseDown += new System.Windows.Forms.MouseEventHandler(this.clbColumns_MouseDown);
            // 
            // gbOptions
            // 
            this.gbOptions.Controls.Add(this.cbShowBalance);
            this.gbOptions.Controls.Add(this.cbShowInternal);
            this.gbOptions.Location = new System.Drawing.Point(226, 3);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Size = new System.Drawing.Size(153, 51);
            this.gbOptions.TabIndex = 1;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Отображать операции";
            // 
            // cbShowBalance
            // 
            this.cbShowBalance.AutoSize = true;
            this.cbShowBalance.Location = new System.Drawing.Point(6, 32);
            this.cbShowBalance.Name = "cbShowBalance";
            this.cbShowBalance.Size = new System.Drawing.Size(105, 17);
            this.cbShowBalance.TabIndex = 3;
            this.cbShowBalance.Text = "В виде баланса";
            this.cbShowBalance.UseVisualStyleBackColor = true;
            this.cbShowBalance.CheckedChanged += new System.EventHandler(this.cbShowBalance_CheckedChanged);
            // 
            // cbShowInternal
            // 
            this.cbShowInternal.AutoSize = true;
            this.cbShowInternal.Location = new System.Drawing.Point(6, 16);
            this.cbShowInternal.Name = "cbShowInternal";
            this.cbShowInternal.Size = new System.Drawing.Size(136, 17);
            this.cbShowInternal.TabIndex = 2;
            this.cbShowInternal.Text = "Внутренние операции";
            this.cbShowInternal.UseVisualStyleBackColor = true;
            // 
            // gbInterval
            // 
            this.gbInterval.Controls.Add(this.cbEnd);
            this.gbInterval.Controls.Add(this.button1);
            this.gbInterval.Controls.Add(this.bRefreshList);
            this.gbInterval.Controls.Add(this.deEnd);
            this.gbInterval.Controls.Add(this.deStart);
            this.gbInterval.Controls.Add(this.label1);
            this.gbInterval.Location = new System.Drawing.Point(3, 3);
            this.gbInterval.Name = "gbInterval";
            this.gbInterval.Size = new System.Drawing.Size(217, 102);
            this.gbInterval.TabIndex = 0;
            this.gbInterval.TabStop = false;
            this.gbInterval.Text = "Интервал";
            // 
            // cbEnd
            // 
            this.cbEnd.AutoSize = true;
            this.cbEnd.Location = new System.Drawing.Point(9, 48);
            this.cbEnd.Name = "cbEnd";
            this.cbEnd.Size = new System.Drawing.Size(100, 17);
            this.cbEnd.TabIndex = 5;
            this.cbEnd.Text = "Конечная дата";
            this.cbEnd.UseVisualStyleBackColor = true;
            this.cbEnd.CheckedChanged += new System.EventHandler(this.cbEnd_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(9, 73);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Ещё >>";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // bRefreshList
            // 
            this.bRefreshList.BackColor = System.Drawing.SystemColors.Control;
            this.bRefreshList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bRefreshList.Location = new System.Drawing.Point(115, 73);
            this.bRefreshList.Name = "bRefreshList";
            this.bRefreshList.Size = new System.Drawing.Size(92, 23);
            this.bRefreshList.TabIndex = 4;
            this.bRefreshList.Text = "Установить";
            this.bRefreshList.UseVisualStyleBackColor = true;
            this.bRefreshList.Click += new System.EventHandler(this.button3_Click);
            // 
            // deEnd
            // 
            this.deEnd.FormattingEnabled = true;
            this.deEnd.InputChar = '_';
            this.deEnd.Location = new System.Drawing.Point(115, 46);
            this.deEnd.MaxLength = 10;
            this.deEnd.Name = "deEnd";
            this.deEnd.Size = new System.Drawing.Size(92, 21);
            this.deEnd.TabIndex = 1;
            // 
            // deStart
            // 
            this.deStart.FormattingEnabled = true;
            this.deStart.InputChar = '_';
            this.deStart.Location = new System.Drawing.Point(115, 19);
            this.deStart.MaxLength = 10;
            this.deStart.Name = "deStart";
            this.deStart.Size = new System.Drawing.Size(92, 21);
            this.deStart.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Начальная дата";
            // 
            // ss
            // 
            this.ss.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tspb,
            this.tssl});
            this.ss.Location = new System.Drawing.Point(0, 440);
            this.ss.Name = "ss";
            this.ss.Size = new System.Drawing.Size(684, 22);
            this.ss.TabIndex = 1;
            this.ss.Text = "statusStrip1";
            // 
            // tspb
            // 
            this.tspb.Name = "tspb";
            this.tspb.Size = new System.Drawing.Size(100, 16);
            // 
            // tssl
            // 
            this.tssl.Name = "tssl";
            this.tssl.Size = new System.Drawing.Size(24, 17);
            this.tssl.Text = "tssl";
            // 
            // gc
            // 
            this.gc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gc.EmbeddedNavigator.Enabled = false;
            this.gc.Location = new System.Drawing.Point(0, 115);
            this.gc.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.gc.MainView = this.bgv;
            this.gc.Name = "gc";
            this.gc.Size = new System.Drawing.Size(684, 325);
            this.gc.TabIndex = 3;
            this.gc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.bgv});
            this.gc.DoubleClick += new System.EventHandler(this.gc_DoubleClick);
            // 
            // bgv
            // 
            this.bgv.Appearance.EvenRow.BackColor = System.Drawing.Color.White;
            this.bgv.Appearance.EvenRow.BackColor2 = System.Drawing.Color.White;
            this.bgv.Appearance.EvenRow.Options.UseBackColor = true;
            this.bgv.Appearance.OddRow.BackColor = System.Drawing.Color.White;
            this.bgv.Appearance.OddRow.BackColor2 = System.Drawing.Color.White;
            this.bgv.Appearance.OddRow.Options.UseBackColor = true;
            this.bgv.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.bgv.GridControl = this.gc;
            this.bgv.Name = "bgv";
            this.bgv.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.bgv.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.bgv.OptionsBehavior.AutoPopulateColumns = false;
            this.bgv.OptionsBehavior.Editable = false;
            this.bgv.OptionsBehavior.ReadOnly = true;
            this.bgv.OptionsBehavior.SummariesIgnoreNullValues = true;
            this.bgv.OptionsCustomization.AllowQuickHideColumns = false;
            this.bgv.OptionsCustomization.ShowBandsInCustomizationForm = false;
            this.bgv.OptionsFilter.UseNewCustomFilterDialog = true;
            this.bgv.OptionsMenu.EnableColumnMenu = false;
            this.bgv.OptionsNavigation.UseOfficePageNavigation = false;
            this.bgv.OptionsPrint.EnableAppearanceEvenRow = true;
            this.bgv.OptionsPrint.EnableAppearanceOddRow = true;
            this.bgv.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.bgv.OptionsView.RowAutoHeight = true;
            this.bgv.OptionsView.ShowFooter = true;
            this.bgv.OptionsView.ShowGroupPanel = false;
            this.bgv.OptionsView.ShowIndicator = false;
            this.bgv.PaintStyleName = "Skin";
            this.bgv.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.bgv_CustomDrawCell);
            this.bgv.CustomDrawFooterCell += new DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventHandler(this.GridControlViewController_CustomDrawFooterCell);
            this.bgv.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.bgv_FocusedRowChanged);
            this.bgv.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.formatCurrencyField);
            this.bgv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.bgv_KeyDown);
            this.bgv.DoubleClick += new System.EventHandler(this.bgv_DoubleClick);
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(0, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(684, 325);
            this.label3.TabIndex = 4;
            this.label3.Text = "Нет данных";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 300;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // cmsExport
            // 
            this.cmsExport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiXls2000,
            this.tsmiXlsx2007});
            this.cmsExport.Name = "cmsExport";
            this.cmsExport.Size = new System.Drawing.Size(171, 48);
            this.cmsExport.Opening += new System.ComponentModel.CancelEventHandler(this.cmsExport_Opening);
            // 
            // tsmiXls2000
            // 
            this.tsmiXls2000.Name = "tsmiXls2000";
            this.tsmiXls2000.Size = new System.Drawing.Size(170, 22);
            this.tsmiXls2000.Text = "Excel (Office 2000)";
            this.tsmiXls2000.Click += new System.EventHandler(this.tsmiXls2000_Click);
            // 
            // tsmiXlsx2007
            // 
            this.tsmiXlsx2007.Name = "tsmiXlsx2007";
            this.tsmiXlsx2007.Size = new System.Drawing.Size(170, 22);
            this.tsmiXlsx2007.Text = "Excel (Office 2007)";
            // 
            // cmsOptions
            // 
            this.cmsOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDate});
            this.cmsOptions.Name = "cmsOptions";
            this.cmsOptions.Size = new System.Drawing.Size(187, 26);
            // 
            // tsmiDate
            // 
            this.tsmiDate.Name = "tsmiDate";
            this.tsmiDate.Size = new System.Drawing.Size(186, 22);
            this.tsmiDate.Text = "Определенный день";
            // 
            // sfd
            // 
            this.sfd.DefaultExt = "xls";
            this.sfd.Filter = "Office 2000 XLS|*.xls|Office 2007 XLSX|*.xlsx";
            // 
            // FirmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 462);
            this.Controls.Add(this.gc);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ss);
            this.Controls.Add(this.pControls);
            this.Name = "FirmForm";
            this.Text = "Фирма";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FirmForm_FormClosing);
            this.Load += new System.EventHandler(this.FirmForm_Load);
            this.pControls.ResumeLayout(false);
            this.pControls.PerformLayout();
            this.gbColumns.ResumeLayout(false);
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            this.gbInterval.ResumeLayout(false);
            this.gbInterval.PerformLayout();
            this.ss.ResumeLayout(false);
            this.ss.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bgv)).EndInit();
            this.cmsExport.ResumeLayout(false);
            this.cmsOptions.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pControls;
        private System.Windows.Forms.StatusStrip ss;
        private System.Windows.Forms.GroupBox gbInterval;
        private System.Windows.Forms.Label label1;
        private DEXExtendLib.DateEdit deStart;
        private System.Windows.Forms.GroupBox gbOptions;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button bRefreshList;
        private DEXExtendLib.DateEdit deEnd;
        private System.Windows.Forms.GroupBox gbColumns;
        private System.Windows.Forms.CheckedListBox clbColumns;
        private System.Windows.Forms.CheckBox cbShowInternal;
        private System.Windows.Forms.CheckBox cbShowBalance;
        private DevExpress.XtraGrid.GridControl gc;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView bgv;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button bExport;
        private System.Windows.Forms.Button bPrint;
        private System.Windows.Forms.ContextMenuStrip cmsExport;
        private System.Windows.Forms.ToolStripMenuItem tsmiXls2000;
        private System.Windows.Forms.ToolStripMenuItem tsmiXlsx2007;
        private System.Windows.Forms.ToolStripProgressBar tspb;
        private System.Windows.Forms.ToolStripStatusLabel tssl;
        private System.Windows.Forms.ContextMenuStrip cmsOptions;
        private System.Windows.Forms.ToolStripMenuItem tsmiDate;
        private System.Windows.Forms.SaveFileDialog sfd;
        private System.Windows.Forms.CheckBox cbEnd;
        private System.Windows.Forms.CheckBox cbShowDeleted;
    }
}