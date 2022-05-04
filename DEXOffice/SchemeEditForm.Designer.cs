namespace DEXOffice
{
    partial class SchemeEditForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lPrinterTitle = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lDocTitle = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbOrientation = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.nudFieldsScaleY = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.nudFieldsScaleX = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.nudFieldsOffsetY = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.nudFieldsOffsetX = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.bFont = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.nudCoverScale = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.nudCoverOffsetY = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.nudCoverOffsetX = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.cbUseCover = new System.Windows.Forms.CheckBox();
            this.cbCover = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.fd = new System.Windows.Forms.FontDialog();
            this.bRestoreDefaults = new System.Windows.Forms.Button();
            this.bPreview = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.bUpdateTemplate = new System.Windows.Forms.Button();
            this.cbRotate = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFieldsScaleY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFieldsScaleX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFieldsOffsetY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFieldsOffsetX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCoverScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCoverOffsetY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCoverOffsetX)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lPrinterTitle);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lDocTitle);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(583, 68);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Описание";
            // 
            // lPrinterTitle
            // 
            this.lPrinterTitle.AutoSize = true;
            this.lPrinterTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lPrinterTitle.Location = new System.Drawing.Point(74, 41);
            this.lPrinterTitle.Name = "lPrinterTitle";
            this.lPrinterTitle.Size = new System.Drawing.Size(72, 13);
            this.lPrinterTitle.TabIndex = 3;
            this.lPrinterTitle.Text = "lPrinterTitle";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Принтер:";
            // 
            // lDocTitle
            // 
            this.lDocTitle.AutoSize = true;
            this.lDocTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lDocTitle.Location = new System.Drawing.Point(74, 20);
            this.lDocTitle.Name = "lDocTitle";
            this.lDocTitle.Size = new System.Drawing.Size(58, 13);
            this.lDocTitle.TabIndex = 1;
            this.lDocTitle.Text = "lDocTitle";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Документ:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbRotate);
            this.groupBox2.Controls.Add(this.cbOrientation);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.nudFieldsScaleY);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.nudFieldsScaleX);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.nudFieldsOffsetY);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.nudFieldsOffsetX);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.bFont);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.nudCoverScale);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.nudCoverOffsetY);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.nudCoverOffsetX);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cbUseCover);
            this.groupBox2.Controls.Add(this.cbCover);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(12, 86);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(583, 166);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Параметры листа";
            // 
            // cbOrientation
            // 
            this.cbOrientation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOrientation.FormattingEnabled = true;
            this.cbOrientation.Items.AddRange(new object[] {
            "Книжная",
            "Альбомная"});
            this.cbOrientation.Location = new System.Drawing.Point(83, 74);
            this.cbOrientation.Name = "cbOrientation";
            this.cbOrientation.Size = new System.Drawing.Size(135, 21);
            this.cbOrientation.TabIndex = 23;
            this.cbOrientation.SelectedIndexChanged += new System.EventHandler(this.cbOrientation_SelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(9, 77);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(68, 13);
            this.label15.TabIndex = 22;
            this.label15.Text = "Ориентация";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(390, 48);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(23, 13);
            this.label14.TabIndex = 21;
            this.label14.Text = "мм";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(503, 133);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(15, 13);
            this.label13.TabIndex = 20;
            this.label13.Text = "%";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(503, 103);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(23, 13);
            this.label12.TabIndex = 19;
            this.label12.Text = "мм";
            // 
            // nudFieldsScaleY
            // 
            this.nudFieldsScaleY.DecimalPlaces = 1;
            this.nudFieldsScaleY.Location = new System.Drawing.Point(421, 131);
            this.nudFieldsScaleY.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudFieldsScaleY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFieldsScaleY.Name = "nudFieldsScaleY";
            this.nudFieldsScaleY.Size = new System.Drawing.Size(76, 20);
            this.nudFieldsScaleY.TabIndex = 18;
            this.nudFieldsScaleY.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFieldsScaleY.ValueChanged += new System.EventHandler(this.nudCoverOffsetX_ValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(340, 133);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(75, 13);
            this.label11.TabIndex = 17;
            this.label11.Text = "по вертикали";
            // 
            // nudFieldsScaleX
            // 
            this.nudFieldsScaleX.DecimalPlaces = 1;
            this.nudFieldsScaleX.Location = new System.Drawing.Point(258, 131);
            this.nudFieldsScaleX.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudFieldsScaleX.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFieldsScaleX.Name = "nudFieldsScaleX";
            this.nudFieldsScaleX.Size = new System.Drawing.Size(76, 20);
            this.nudFieldsScaleX.TabIndex = 16;
            this.nudFieldsScaleX.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFieldsScaleX.ValueChanged += new System.EventHandler(this.nudCoverOffsetX_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 133);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(243, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "Растяжение координат полей по горизонтали:";
            // 
            // nudFieldsOffsetY
            // 
            this.nudFieldsOffsetY.DecimalPlaces = 1;
            this.nudFieldsOffsetY.Location = new System.Drawing.Point(421, 101);
            this.nudFieldsOffsetY.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudFieldsOffsetY.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.nudFieldsOffsetY.Name = "nudFieldsOffsetY";
            this.nudFieldsOffsetY.Size = new System.Drawing.Size(76, 20);
            this.nudFieldsOffsetY.TabIndex = 14;
            this.nudFieldsOffsetY.ValueChanged += new System.EventHandler(this.nudCoverOffsetX_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(340, 103);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "по вертикали";
            // 
            // nudFieldsOffsetX
            // 
            this.nudFieldsOffsetX.DecimalPlaces = 1;
            this.nudFieldsOffsetX.Location = new System.Drawing.Point(258, 101);
            this.nudFieldsOffsetX.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudFieldsOffsetX.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.nudFieldsOffsetX.Name = "nudFieldsOffsetX";
            this.nudFieldsOffsetX.Size = new System.Drawing.Size(76, 20);
            this.nudFieldsOffsetX.TabIndex = 12;
            this.nudFieldsOffsetX.ValueChanged += new System.EventHandler(this.nudCoverOffsetX_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 103);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(222, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Общий отступ всех полей по горизонтали:";
            // 
            // bFont
            // 
            this.bFont.Location = new System.Drawing.Point(308, 72);
            this.bFont.Name = "bFont";
            this.bFont.Size = new System.Drawing.Size(269, 23);
            this.bFont.TabIndex = 10;
            this.bFont.Text = "Шрифт";
            this.bFont.UseVisualStyleBackColor = true;
            this.bFont.Click += new System.EventHandler(this.bFont_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(562, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(15, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "%";
            // 
            // nudCoverScale
            // 
            this.nudCoverScale.DecimalPlaces = 1;
            this.nudCoverScale.Location = new System.Drawing.Point(485, 46);
            this.nudCoverScale.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudCoverScale.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudCoverScale.Name = "nudCoverScale";
            this.nudCoverScale.Size = new System.Drawing.Size(71, 20);
            this.nudCoverScale.TabIndex = 8;
            this.nudCoverScale.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudCoverScale.ValueChanged += new System.EventHandler(this.nudCoverOffsetX_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(423, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Масштаб:";
            // 
            // nudCoverOffsetY
            // 
            this.nudCoverOffsetY.DecimalPlaces = 1;
            this.nudCoverOffsetY.Location = new System.Drawing.Point(308, 46);
            this.nudCoverOffsetY.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudCoverOffsetY.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.nudCoverOffsetY.Name = "nudCoverOffsetY";
            this.nudCoverOffsetY.Size = new System.Drawing.Size(76, 20);
            this.nudCoverOffsetY.TabIndex = 6;
            this.nudCoverOffsetY.ValueChanged += new System.EventHandler(this.nudCoverOffsetX_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(224, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "по вертикали:";
            // 
            // nudCoverOffsetX
            // 
            this.nudCoverOffsetX.DecimalPlaces = 1;
            this.nudCoverOffsetX.Location = new System.Drawing.Point(142, 46);
            this.nudCoverOffsetX.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudCoverOffsetX.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.nudCoverOffsetX.Name = "nudCoverOffsetX";
            this.nudCoverOffsetX.Size = new System.Drawing.Size(76, 20);
            this.nudCoverOffsetX.TabIndex = 4;
            this.nudCoverOffsetX.ValueChanged += new System.EventHandler(this.nudCoverOffsetX_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Отступ по горизонтали:";
            // 
            // cbUseCover
            // 
            this.cbUseCover.AutoSize = true;
            this.cbUseCover.Location = new System.Drawing.Point(426, 21);
            this.cbUseCover.Name = "cbUseCover";
            this.cbUseCover.Size = new System.Drawing.Size(151, 17);
            this.cbUseCover.TabIndex = 2;
            this.cbUseCover.Text = "Использовать подложку";
            this.cbUseCover.UseVisualStyleBackColor = true;
            this.cbUseCover.CheckedChanged += new System.EventHandler(this.cbUseCover_CheckedChanged);
            // 
            // cbCover
            // 
            this.cbCover.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCover.FormattingEnabled = true;
            this.cbCover.Location = new System.Drawing.Point(74, 19);
            this.cbCover.Name = "cbCover";
            this.cbCover.Size = new System.Drawing.Size(346, 21);
            this.cbCover.TabIndex = 1;
            this.cbCover.SelectedIndexChanged += new System.EventHandler(this.cbCover_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Подложка";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgv);
            this.groupBox3.Location = new System.Drawing.Point(12, 258);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(583, 191);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Параметры полей";
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Location = new System.Drawing.Point(6, 19);
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgv.Size = new System.Drawing.Size(571, 166);
            this.dgv.TabIndex = 0;
            this.dgv.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellValueChanged);
            // 
            // fd
            // 
            this.fd.FontMustExist = true;
            this.fd.ShowEffects = false;
            // 
            // bRestoreDefaults
            // 
            this.bRestoreDefaults.Location = new System.Drawing.Point(12, 455);
            this.bRestoreDefaults.Name = "bRestoreDefaults";
            this.bRestoreDefaults.Size = new System.Drawing.Size(115, 23);
            this.bRestoreDefaults.TabIndex = 3;
            this.bRestoreDefaults.Text = "Сбросить шаблон";
            this.bRestoreDefaults.UseVisualStyleBackColor = true;
            this.bRestoreDefaults.Click += new System.EventHandler(this.bRestoreDefaults_Click);
            // 
            // bPreview
            // 
            this.bPreview.Location = new System.Drawing.Point(330, 455);
            this.bPreview.Name = "bPreview";
            this.bPreview.Size = new System.Drawing.Size(102, 23);
            this.bPreview.TabIndex = 4;
            this.bPreview.Text = "F3 - Просмотр";
            this.bPreview.UseVisualStyleBackColor = true;
            this.bPreview.Click += new System.EventHandler(this.bPreview_Click);
            // 
            // bOK
            // 
            this.bOK.Location = new System.Drawing.Point(438, 455);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 5;
            this.bOK.Text = "Сохранить";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(520, 455);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 6;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bUpdateTemplate
            // 
            this.bUpdateTemplate.Location = new System.Drawing.Point(133, 455);
            this.bUpdateTemplate.Name = "bUpdateTemplate";
            this.bUpdateTemplate.Size = new System.Drawing.Size(110, 23);
            this.bUpdateTemplate.TabIndex = 7;
            this.bUpdateTemplate.Text = "Обновить шаблон";
            this.bUpdateTemplate.UseVisualStyleBackColor = true;
            this.bUpdateTemplate.Click += new System.EventHandler(this.bUpdateTemplate_Click);
            // 
            // cbRotate
            // 
            this.cbRotate.AutoSize = true;
            this.cbRotate.Location = new System.Drawing.Point(227, 76);
            this.cbRotate.Name = "cbRotate";
            this.cbRotate.Size = new System.Drawing.Size(81, 17);
            this.cbRotate.TabIndex = 24;
            this.cbRotate.Text = "Переворот";
            this.cbRotate.UseVisualStyleBackColor = true;
            this.cbRotate.CheckedChanged += new System.EventHandler(this.cbRotate_CheckedChanged);
            // 
            // SchemeEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(607, 490);
            this.Controls.Add(this.bUpdateTemplate);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.bPreview);
            this.Controls.Add(this.bRestoreDefaults);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SchemeEditForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Редактор схемы";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SchemeEditForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFieldsScaleY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFieldsScaleX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFieldsOffsetY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFieldsOffsetX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCoverScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCoverOffsetY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCoverOffsetX)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lDocTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lPrinterTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cbCover;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbUseCover;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudCoverScale;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudCoverOffsetY;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudCoverOffsetX;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown nudFieldsOffsetY;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nudFieldsOffsetX;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button bFont;
        private System.Windows.Forms.FontDialog fd;
        private System.Windows.Forms.NumericUpDown nudFieldsScaleY;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown nudFieldsScaleX;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button bRestoreDefaults;
        private System.Windows.Forms.Button bPreview;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cbOrientation;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button bUpdateTemplate;
        private System.Windows.Forms.CheckBox cbRotate;
    }
}