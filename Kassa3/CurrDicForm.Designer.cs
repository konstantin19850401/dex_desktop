namespace Kassa3
{
    partial class CurrDicForm
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
            this.tc = new System.Windows.Forms.TabControl();
            this.tpValues = new System.Windows.Forms.TabPage();
            this.bDellCurrDate = new System.Windows.Forms.Button();
            this.bShowDay = new System.Windows.Forms.Button();
            this.deCurrDate = new DEXExtendLib.DateEdit();
            this.bUpdateWeb = new System.Windows.Forms.Button();
            this.dgvCV = new System.Windows.Forms.DataGridView();
            this.cdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cvalue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpTypes = new System.Windows.Forms.TabPage();
            this.bCurrStatus = new System.Windows.Forms.Button();
            this.bDeleteCurrency = new System.Windows.Forms.Button();
            this.bAddCurrency = new System.Windows.Forms.Button();
            this.bRefreshCurrTitles = new System.Windows.Forms.Button();
            this.dgvCT = new System.Windows.Forms.DataGridView();
            this.ttHint = new System.Windows.Forms.ToolTip(this.components);
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.active = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tc.SuspendLayout();
            this.tpValues.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCV)).BeginInit();
            this.tpTypes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCT)).BeginInit();
            this.SuspendLayout();
            // 
            // tc
            // 
            this.tc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tc.Controls.Add(this.tpValues);
            this.tc.Controls.Add(this.tpTypes);
            this.tc.Location = new System.Drawing.Point(12, 12);
            this.tc.Name = "tc";
            this.tc.SelectedIndex = 0;
            this.tc.Size = new System.Drawing.Size(460, 338);
            this.tc.TabIndex = 0;
            // 
            // tpValues
            // 
            this.tpValues.Controls.Add(this.bDellCurrDate);
            this.tpValues.Controls.Add(this.bShowDay);
            this.tpValues.Controls.Add(this.deCurrDate);
            this.tpValues.Controls.Add(this.bUpdateWeb);
            this.tpValues.Controls.Add(this.dgvCV);
            this.tpValues.Location = new System.Drawing.Point(4, 22);
            this.tpValues.Name = "tpValues";
            this.tpValues.Padding = new System.Windows.Forms.Padding(3);
            this.tpValues.Size = new System.Drawing.Size(452, 312);
            this.tpValues.TabIndex = 0;
            this.tpValues.Text = "Курсы валют";
            this.tpValues.UseVisualStyleBackColor = true;
            // 
            // bDellCurrDate
            // 
            this.bDellCurrDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bDellCurrDate.Location = new System.Drawing.Point(206, 283);
            this.bDellCurrDate.Name = "bDellCurrDate";
            this.bDellCurrDate.Size = new System.Drawing.Size(96, 23);
            this.bDellCurrDate.TabIndex = 4;
            this.bDellCurrDate.Text = "Удалить курсы";
            this.ttHint.SetToolTip(this.bDellCurrDate, "Удалить все курсы на указанную дату.");
            this.bDellCurrDate.UseVisualStyleBackColor = true;
            this.bDellCurrDate.Click += new System.EventHandler(this.bDellCurrDate_Click);
            // 
            // bShowDay
            // 
            this.bShowDay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bShowDay.Location = new System.Drawing.Point(104, 283);
            this.bShowDay.Name = "bShowDay";
            this.bShowDay.Size = new System.Drawing.Size(96, 23);
            this.bShowDay.TabIndex = 3;
            this.bShowDay.Text = "Курсы на дату";
            this.ttHint.SetToolTip(this.bShowDay, "Произвести выборку курсов валют на указанную дату.\r\nЕсли на заданную дату нет кур" +
                    "са, будет использован более ранний курс.");
            this.bShowDay.UseVisualStyleBackColor = true;
            this.bShowDay.Click += new System.EventHandler(this.bShowDay_Click);
            // 
            // deCurrDate
            // 
            this.deCurrDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deCurrDate.FormattingEnabled = true;
            this.deCurrDate.InputChar = '_';
            this.deCurrDate.Location = new System.Drawing.Point(6, 285);
            this.deCurrDate.MaxLength = 10;
            this.deCurrDate.Name = "deCurrDate";
            this.deCurrDate.Size = new System.Drawing.Size(92, 21);
            this.deCurrDate.TabIndex = 2;
            // 
            // bUpdateWeb
            // 
            this.bUpdateWeb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bUpdateWeb.Location = new System.Drawing.Point(308, 283);
            this.bUpdateWeb.Name = "bUpdateWeb";
            this.bUpdateWeb.Size = new System.Drawing.Size(138, 23);
            this.bUpdateWeb.TabIndex = 1;
            this.bUpdateWeb.Text = "Получить из Интернета";
            this.ttHint.SetToolTip(this.bUpdateWeb, "Получить с сайта ЦБ курсы валют на указанную дату.\r\nКурсы, уже имеющиеся в БД, пе" +
                    "резаписаны не будут.");
            this.bUpdateWeb.UseVisualStyleBackColor = true;
            this.bUpdateWeb.Click += new System.EventHandler(this.bUpdateWeb_Click);
            // 
            // dgvCV
            // 
            this.dgvCV.AllowUserToAddRows = false;
            this.dgvCV.AllowUserToDeleteRows = false;
            this.dgvCV.AllowUserToResizeColumns = false;
            this.dgvCV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cdate,
            this.cvalue,
            this.cname});
            this.dgvCV.Location = new System.Drawing.Point(6, 6);
            this.dgvCV.Name = "dgvCV";
            this.dgvCV.RowHeadersVisible = false;
            this.dgvCV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCV.Size = new System.Drawing.Size(440, 271);
            this.dgvCV.TabIndex = 0;
            this.dgvCV.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvCV_CellFormatting);
            // 
            // cdate
            // 
            this.cdate.DataPropertyName = "cdate";
            this.cdate.FillWeight = 20F;
            this.cdate.HeaderText = "Дата";
            this.cdate.Name = "cdate";
            this.cdate.ReadOnly = true;
            // 
            // cvalue
            // 
            this.cvalue.DataPropertyName = "cvalue";
            this.cvalue.FillWeight = 20F;
            this.cvalue.HeaderText = "Номинал";
            this.cvalue.Name = "cvalue";
            this.cvalue.ReadOnly = true;
            // 
            // cname
            // 
            this.cname.DataPropertyName = "cname";
            this.cname.HeaderText = "Наименование валюты";
            this.cname.Name = "cname";
            this.cname.ReadOnly = true;
            // 
            // tpTypes
            // 
            this.tpTypes.Controls.Add(this.bCurrStatus);
            this.tpTypes.Controls.Add(this.bDeleteCurrency);
            this.tpTypes.Controls.Add(this.bAddCurrency);
            this.tpTypes.Controls.Add(this.bRefreshCurrTitles);
            this.tpTypes.Controls.Add(this.dgvCT);
            this.tpTypes.Location = new System.Drawing.Point(4, 22);
            this.tpTypes.Name = "tpTypes";
            this.tpTypes.Padding = new System.Windows.Forms.Padding(3);
            this.tpTypes.Size = new System.Drawing.Size(452, 312);
            this.tpTypes.TabIndex = 1;
            this.tpTypes.Text = "Виды валют";
            this.tpTypes.UseVisualStyleBackColor = true;
            // 
            // bCurrStatus
            // 
            this.bCurrStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bCurrStatus.Location = new System.Drawing.Point(168, 283);
            this.bCurrStatus.Name = "bCurrStatus";
            this.bCurrStatus.Size = new System.Drawing.Size(75, 23);
            this.bCurrStatus.TabIndex = 4;
            this.bCurrStatus.Text = "Статус";
            this.bCurrStatus.UseVisualStyleBackColor = true;
            this.bCurrStatus.Click += new System.EventHandler(this.bCurrStatus_Click);
            // 
            // bDeleteCurrency
            // 
            this.bDeleteCurrency.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bDeleteCurrency.Location = new System.Drawing.Point(87, 283);
            this.bDeleteCurrency.Name = "bDeleteCurrency";
            this.bDeleteCurrency.Size = new System.Drawing.Size(75, 23);
            this.bDeleteCurrency.TabIndex = 3;
            this.bDeleteCurrency.Text = "Удалить";
            this.bDeleteCurrency.UseVisualStyleBackColor = true;
            this.bDeleteCurrency.Click += new System.EventHandler(this.bDeleteCurrency_Click);
            // 
            // bAddCurrency
            // 
            this.bAddCurrency.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bAddCurrency.Location = new System.Drawing.Point(6, 283);
            this.bAddCurrency.Name = "bAddCurrency";
            this.bAddCurrency.Size = new System.Drawing.Size(75, 23);
            this.bAddCurrency.TabIndex = 2;
            this.bAddCurrency.Text = "Добавить";
            this.bAddCurrency.UseVisualStyleBackColor = true;
            this.bAddCurrency.Click += new System.EventHandler(this.bAddCurrency_Click);
            // 
            // bRefreshCurrTitles
            // 
            this.bRefreshCurrTitles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bRefreshCurrTitles.Location = new System.Drawing.Point(273, 283);
            this.bRefreshCurrTitles.Name = "bRefreshCurrTitles";
            this.bRefreshCurrTitles.Size = new System.Drawing.Size(173, 23);
            this.bRefreshCurrTitles.TabIndex = 1;
            this.bRefreshCurrTitles.Text = "Получить виды валют с ЦБ РФ";
            this.bRefreshCurrTitles.UseVisualStyleBackColor = true;
            this.bRefreshCurrTitles.Click += new System.EventHandler(this.bRefreshCurrTitles_Click);
            // 
            // dgvCT
            // 
            this.dgvCT.AllowUserToAddRows = false;
            this.dgvCT.AllowUserToDeleteRows = false;
            this.dgvCT.AllowUserToResizeRows = false;
            this.dgvCT.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCT.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCT.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.code,
            this.name,
            this.active});
            this.dgvCT.Location = new System.Drawing.Point(6, 6);
            this.dgvCT.Name = "dgvCT";
            this.dgvCT.ReadOnly = true;
            this.dgvCT.RowHeadersVisible = false;
            this.dgvCT.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCT.Size = new System.Drawing.Size(440, 271);
            this.dgvCT.TabIndex = 0;
            this.dgvCT.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCT_CellDoubleClick);
            // 
            // code
            // 
            this.code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.code.DataPropertyName = "code";
            this.code.FillWeight = 30F;
            this.code.Frozen = true;
            this.code.HeaderText = "Код";
            this.code.Name = "code";
            this.code.ReadOnly = true;
            this.code.Width = 146;
            // 
            // name
            // 
            this.name.DataPropertyName = "name";
            this.name.FillWeight = 60F;
            this.name.HeaderText = "Наименование";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // active
            // 
            this.active.DataPropertyName = "active";
            this.active.FillWeight = 20F;
            this.active.HeaderText = "Активна";
            this.active.Name = "active";
            this.active.ReadOnly = true;
            this.active.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.active.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // CurrDicForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 362);
            this.Controls.Add(this.tc);
            this.MinimizeBox = false;
            this.Name = "CurrDicForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Справочник валют";
            this.Shown += new System.EventHandler(this.CurrDicForm_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CurrDicForm_FormClosing);
            this.tc.ResumeLayout(false);
            this.tpValues.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCV)).EndInit();
            this.tpTypes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCT)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tc;
        private System.Windows.Forms.TabPage tpValues;
        private System.Windows.Forms.TabPage tpTypes;
        private System.Windows.Forms.Button bRefreshCurrTitles;
        private System.Windows.Forms.DataGridView dgvCT;
        private System.Windows.Forms.Button bDeleteCurrency;
        private System.Windows.Forms.Button bAddCurrency;
        private System.Windows.Forms.Button bCurrStatus;
        private System.Windows.Forms.Button bShowDay;
        private DEXExtendLib.DateEdit deCurrDate;
        private System.Windows.Forms.Button bUpdateWeb;
        private System.Windows.Forms.DataGridView dgvCV;
        private System.Windows.Forms.Button bDellCurrDate;
        private System.Windows.Forms.ToolTip ttHint;
        private System.Windows.Forms.DataGridViewTextBoxColumn cdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn cvalue;
        private System.Windows.Forms.DataGridViewTextBoxColumn cname;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewCheckBoxColumn active;
    }
}