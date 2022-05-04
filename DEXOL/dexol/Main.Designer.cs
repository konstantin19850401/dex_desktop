namespace dexol
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.tStatus = new System.Windows.Forms.Timer(this.components);
            this.pNav = new System.Windows.Forms.Panel();
            this.cbAutoRefresh = new System.Windows.Forms.CheckBox();
            this.bRefreshDics = new System.Windows.Forms.Button();
            this.deDate = new DEXExtendLib.DateEdit();
            this.bQuery = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbDb = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvJournal = new System.Windows.Forms.DataGridView();
            this.vdocdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vdocnum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.digest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsddbNewDoc = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsbEditDoc = new System.Windows.Forms.ToolStripButton();
            this.tsbDeleteDoc = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsddbSetup = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsddbFunctions = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmiTTCExport = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDocsReport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsddbPrint = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmiPreviewDoc = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiPrintDoc = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiPrinterSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSchemesSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.pNav.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJournal)).BeginInit();
            this.panel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tStatus
            // 
            this.tStatus.Interval = 1000;
            this.tStatus.Tick += new System.EventHandler(this.tStatus_Tick);
            // 
            // pNav
            // 
            this.pNav.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pNav.Controls.Add(this.cbAutoRefresh);
            this.pNav.Controls.Add(this.bRefreshDics);
            this.pNav.Controls.Add(this.deDate);
            this.pNav.Controls.Add(this.bQuery);
            this.pNav.Controls.Add(this.label2);
            this.pNav.Controls.Add(this.cbDb);
            this.pNav.Controls.Add(this.label1);
            this.pNav.Dock = System.Windows.Forms.DockStyle.Top;
            this.pNav.Location = new System.Drawing.Point(0, 0);
            this.pNav.Name = "pNav";
            this.pNav.Size = new System.Drawing.Size(952, 62);
            this.pNav.TabIndex = 0;
            // 
            // cbAutoRefresh
            // 
            this.cbAutoRefresh.AutoSize = true;
            this.cbAutoRefresh.Location = new System.Drawing.Point(480, 38);
            this.cbAutoRefresh.Name = "cbAutoRefresh";
            this.cbAutoRefresh.Size = new System.Drawing.Size(89, 17);
            this.cbAutoRefresh.TabIndex = 7;
            this.cbAutoRefresh.Text = "Авто-запрос";
            this.cbAutoRefresh.UseVisualStyleBackColor = true;
            // 
            // bRefreshDics
            // 
            this.bRefreshDics.Location = new System.Drawing.Point(577, 9);
            this.bRefreshDics.Name = "bRefreshDics";
            this.bRefreshDics.Size = new System.Drawing.Size(151, 23);
            this.bRefreshDics.TabIndex = 6;
            this.bRefreshDics.Text = "Обновить справочники";
            this.bRefreshDics.UseVisualStyleBackColor = true;
            this.bRefreshDics.Click += new System.EventHandler(this.bRefreshDics_Click);
            // 
            // deDate
            // 
            this.deDate.FormattingEnabled = true;
            this.deDate.InputChar = '_';
            this.deDate.Location = new System.Drawing.Point(384, 11);
            this.deDate.MaxLength = 10;
            this.deDate.Name = "deDate";
            this.deDate.Size = new System.Drawing.Size(90, 21);
            this.deDate.TabIndex = 4;
            // 
            // bQuery
            // 
            this.bQuery.Location = new System.Drawing.Point(480, 9);
            this.bQuery.Name = "bQuery";
            this.bQuery.Size = new System.Drawing.Size(91, 23);
            this.bQuery.TabIndex = 3;
            this.bQuery.Text = "Запрос";
            this.bQuery.UseVisualStyleBackColor = true;
            this.bQuery.Click += new System.EventHandler(this.bQuery_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(345, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Дата";
            // 
            // cbDb
            // 
            this.cbDb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDb.FormattingEnabled = true;
            this.cbDb.Location = new System.Drawing.Point(89, 11);
            this.cbDb.Name = "cbDb";
            this.cbDb.Size = new System.Drawing.Size(237, 21);
            this.cbDb.TabIndex = 1;
            this.cbDb.SelectedIndexChanged += new System.EventHandler(this.cbDb_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "База данных";
            // 
            // dgvJournal
            // 
            this.dgvJournal.AllowUserToAddRows = false;
            this.dgvJournal.AllowUserToDeleteRows = false;
            this.dgvJournal.AllowUserToResizeRows = false;
            this.dgvJournal.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvJournal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvJournal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.vdocdate,
            this.vdocnum,
            this.status,
            this.digest});
            this.dgvJournal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvJournal.Location = new System.Drawing.Point(0, 0);
            this.dgvJournal.Name = "dgvJournal";
            this.dgvJournal.ReadOnly = true;
            this.dgvJournal.RowHeadersVisible = false;
            this.dgvJournal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvJournal.ShowEditingIcon = false;
            this.dgvJournal.Size = new System.Drawing.Size(952, 434);
            this.dgvJournal.TabIndex = 1;
            this.dgvJournal.Visible = false;
            this.dgvJournal.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvJournal_CellFormatting);
            this.dgvJournal.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvJournal_ColumnWidthChanged);
            this.dgvJournal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvJournal_KeyDown);
            this.dgvJournal.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvJournal_MouseDoubleClick);
            // 
            // vdocdate
            // 
            this.vdocdate.DataPropertyName = "vdocdate";
            this.vdocdate.HeaderText = "Дата документа";
            this.vdocdate.Name = "vdocdate";
            this.vdocdate.ReadOnly = true;
            // 
            // vdocnum
            // 
            this.vdocnum.DataPropertyName = "vdocnum";
            this.vdocnum.HeaderText = "№ документа";
            this.vdocnum.Name = "vdocnum";
            this.vdocnum.ReadOnly = true;
            // 
            // status
            // 
            this.status.DataPropertyName = "status";
            this.status.HeaderText = "Статус";
            this.status.Name = "status";
            this.status.ReadOnly = true;
            // 
            // digest
            // 
            this.digest.DataPropertyName = "digest";
            this.digest.HeaderText = "Предмет документа";
            this.digest.Name = "digest";
            this.digest.ReadOnly = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 496);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(952, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvJournal);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 62);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(952, 434);
            this.panel2.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(952, 434);
            this.label3.TabIndex = 2;
            this.label3.Text = "Нет документов";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsddbNewDoc,
            this.tsbEditDoc,
            this.tsbDeleteDoc,
            this.toolStripSeparator1,
            this.tsddbSetup,
            this.tsddbFunctions,
            this.toolStripSeparator2,
            this.tsddbPrint});
            this.toolStrip1.Location = new System.Drawing.Point(0, 471);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(952, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsddbNewDoc
            // 
            this.tsddbNewDoc.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsddbNewDoc.Image = ((System.Drawing.Image)(resources.GetObject("tsddbNewDoc.Image")));
            this.tsddbNewDoc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbNewDoc.Name = "tsddbNewDoc";
            this.tsddbNewDoc.Size = new System.Drawing.Size(113, 22);
            this.tsddbNewDoc.Text = "Новый документ";
            // 
            // tsbEditDoc
            // 
            this.tsbEditDoc.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbEditDoc.Image = ((System.Drawing.Image)(resources.GetObject("tsbEditDoc.Image")));
            this.tsbEditDoc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEditDoc.Name = "tsbEditDoc";
            this.tsbEditDoc.Size = new System.Drawing.Size(120, 22);
            this.tsbEditDoc.Text = "Изменить документ";
            this.tsbEditDoc.Click += new System.EventHandler(this.tsbEditDoc_Click);
            // 
            // tsbDeleteDoc
            // 
            this.tsbDeleteDoc.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbDeleteDoc.Image = ((System.Drawing.Image)(resources.GetObject("tsbDeleteDoc.Image")));
            this.tsbDeleteDoc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDeleteDoc.Name = "tsbDeleteDoc";
            this.tsbDeleteDoc.Size = new System.Drawing.Size(110, 22);
            this.tsbDeleteDoc.Text = "Удалить документ";
            this.tsbDeleteDoc.Click += new System.EventHandler(this.tsbDeleteDoc_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsddbSetup
            // 
            this.tsddbSetup.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsddbSetup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsddbSetup.Image = ((System.Drawing.Image)(resources.GetObject("tsddbSetup.Image")));
            this.tsddbSetup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbSetup.Name = "tsddbSetup";
            this.tsddbSetup.Size = new System.Drawing.Size(80, 22);
            this.tsddbSetup.Text = "Настройки";
            // 
            // tsddbFunctions
            // 
            this.tsddbFunctions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsddbFunctions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiTTCExport,
            this.tsmiDocsReport});
            this.tsddbFunctions.Image = ((System.Drawing.Image)(resources.GetObject("tsddbFunctions.Image")));
            this.tsddbFunctions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbFunctions.Name = "tsddbFunctions";
            this.tsddbFunctions.Size = new System.Drawing.Size(69, 22);
            this.tsddbFunctions.Text = "Функции";
            // 
            // tsmiTTCExport
            // 
            this.tsmiTTCExport.Name = "tsmiTTCExport";
            this.tsmiTTCExport.Size = new System.Drawing.Size(259, 22);
            this.tsmiTTCExport.Text = "Экспорт в TinyTrade";
            this.tsmiTTCExport.Click += new System.EventHandler(this.tsmiTTCExport_Click);
            // 
            // tsmiDocsReport
            // 
            this.tsmiDocsReport.Name = "tsmiDocsReport";
            this.tsmiDocsReport.Size = new System.Drawing.Size(259, 22);
            this.tsmiDocsReport.Text = "Отчёт по внесённым документам";
            this.tsmiDocsReport.Click += new System.EventHandler(this.tsmiDocsReport_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsddbPrint
            // 
            this.tsddbPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsddbPrint.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiPreviewDoc,
            this.toolStripMenuItem1,
            this.tsmiPrintDoc,
            this.toolStripMenuItem2,
            this.tsmiPrinterSettings,
            this.tsmiSchemesSetup});
            this.tsddbPrint.Image = ((System.Drawing.Image)(resources.GetObject("tsddbPrint.Image")));
            this.tsddbPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbPrint.Name = "tsddbPrint";
            this.tsddbPrint.Size = new System.Drawing.Size(127, 22);
            this.tsddbPrint.Text = "Печать документов";
            // 
            // tsmiPreviewDoc
            // 
            this.tsmiPreviewDoc.Name = "tsmiPreviewDoc";
            this.tsmiPreviewDoc.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.tsmiPreviewDoc.Size = new System.Drawing.Size(241, 22);
            this.tsmiPreviewDoc.Text = "Просмотр перед печатью";
            this.tsmiPreviewDoc.Click += new System.EventHandler(this.tsmiPreviewDoc_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(238, 6);
            // 
            // tsmiPrintDoc
            // 
            this.tsmiPrintDoc.Name = "tsmiPrintDoc";
            this.tsmiPrintDoc.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F12)));
            this.tsmiPrintDoc.Size = new System.Drawing.Size(241, 22);
            this.tsmiPrintDoc.Text = "Печать";
            this.tsmiPrintDoc.Click += new System.EventHandler(this.tsmiPrintDoc_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(238, 6);
            // 
            // tsmiPrinterSettings
            // 
            this.tsmiPrinterSettings.Name = "tsmiPrinterSettings";
            this.tsmiPrinterSettings.Size = new System.Drawing.Size(241, 22);
            this.tsmiPrinterSettings.Text = "Настройка принтера";
            this.tsmiPrinterSettings.Click += new System.EventHandler(this.tsmiPrinterSettings_Click);
            // 
            // tsmiSchemesSetup
            // 
            this.tsmiSchemesSetup.Name = "tsmiSchemesSetup";
            this.tsmiSchemesSetup.Size = new System.Drawing.Size(241, 22);
            this.tsmiSchemesSetup.Text = "Настройка схем печати";
            this.tsmiSchemesSetup.Click += new System.EventHandler(this.tsmiSchemesSetup_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 518);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pNav);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "DEX Онлайн";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.pNav.ResumeLayout(false);
            this.pNav.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJournal)).EndInit();
            this.panel2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tStatus;
        private System.Windows.Forms.Panel pNav;
        private System.Windows.Forms.ComboBox cbDb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bQuery;
        private System.Windows.Forms.Label label2;
        private DEXExtendLib.DateEdit deDate;
        private System.Windows.Forms.DataGridView dgvJournal;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bRefreshDics;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton tsddbNewDoc;
        private System.Windows.Forms.ToolStripButton tsbEditDoc;
        private System.Windows.Forms.ToolStripButton tsbDeleteDoc;
        private System.Windows.Forms.CheckBox cbAutoRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripDropDownButton tsddbSetup;
        private System.Windows.Forms.ToolStripDropDownButton tsddbFunctions;
        private System.Windows.Forms.ToolStripMenuItem tsmiTTCExport;
        private System.Windows.Forms.ToolStripMenuItem tsmiDocsReport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripDropDownButton tsddbPrint;
        private System.Windows.Forms.ToolStripMenuItem tsmiPreviewDoc;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsmiPrintDoc;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem tsmiPrinterSettings;
        private System.Windows.Forms.ToolStripMenuItem tsmiSchemesSetup;
        private System.Windows.Forms.DataGridViewTextBoxColumn vdocdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn vdocnum;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridViewTextBoxColumn digest;
    }
}