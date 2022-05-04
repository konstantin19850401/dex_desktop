namespace DEXPlugin.Dictionary.Beeline.Sim
{
    partial class FSimImport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FSimImport));
            this.dgvPreview = new System.Windows.Forms.DataGridView();
            this.clbFields = new System.Windows.Forms.CheckedListBox();
            this.gbTable = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.bFieldDown = new System.Windows.Forms.Button();
            this.bFieldUp = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbFS = new System.Windows.Forms.CheckBox();
            this.cbBalance = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbDublicateMsisdn = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbOldToCurrent = new System.Windows.Forms.CheckBox();
            this.cbRemoveQuotes = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbRegion = new System.Windows.Forms.ComboBox();
            this.cbPlan = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.bNewParty = new System.Windows.Forms.Button();
            this.nudPartyNum = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.cbSeparator = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cbLoadIntoTextEditor = new System.Windows.Forms.CheckBox();
            this.bSelectSrcFile = new System.Windows.Forms.Button();
            this.bLoadFromClipboard = new System.Windows.Forms.Button();
            this.tbSrcFile = new System.Windows.Forms.TextBox();
            this.bLoadFromSrcFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.bOk = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbCheckICC = new System.Windows.Forms.TextBox();
            this.tbCheckMSISDN = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pb = new System.Windows.Forms.ProgressBar();
            this.lOpProgress = new System.Windows.Forms.Label();
            this.cbShowOnlyCorrect = new System.Windows.Forms.CheckBox();
            this.btnGetSimFromServer = new System.Windows.Forms.Button();
            this.cbShowOnlyNew = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreview)).BeginInit();
            this.gbTable.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPartyNum)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvPreview
            // 
            this.dgvPreview.AllowUserToAddRows = false;
            this.dgvPreview.AllowUserToDeleteRows = false;
            this.dgvPreview.AllowUserToResizeRows = false;
            this.dgvPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPreview.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvPreview.Location = new System.Drawing.Point(6, 19);
            this.dgvPreview.MultiSelect = false;
            this.dgvPreview.Name = "dgvPreview";
            this.dgvPreview.ReadOnly = true;
            this.dgvPreview.RowHeadersVisible = false;
            this.dgvPreview.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPreview.ShowEditingIcon = false;
            this.dgvPreview.Size = new System.Drawing.Size(560, 173);
            this.dgvPreview.TabIndex = 1;
            this.dgvPreview.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvPreview_CellFormatting);
            this.dgvPreview.Sorted += new System.EventHandler(this.dgvPreview_Sorted);
            // 
            // clbFields
            // 
            this.clbFields.FormattingEnabled = true;
            this.clbFields.Location = new System.Drawing.Point(6, 19);
            this.clbFields.Name = "clbFields";
            this.clbFields.Size = new System.Drawing.Size(249, 184);
            this.clbFields.TabIndex = 2;
            this.clbFields.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbFields_ItemCheck);
            // 
            // gbTable
            // 
            this.gbTable.Controls.Add(this.dgvPreview);
            this.gbTable.Location = new System.Drawing.Point(12, 12);
            this.gbTable.Name = "gbTable";
            this.gbTable.Size = new System.Drawing.Size(572, 198);
            this.gbTable.TabIndex = 3;
            this.gbTable.TabStop = false;
            this.gbTable.Text = "Предпросмотр загружаемых карт";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.bFieldDown);
            this.groupBox2.Controls.Add(this.bFieldUp);
            this.groupBox2.Controls.Add(this.clbFields);
            this.groupBox2.Location = new System.Drawing.Point(12, 261);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(292, 214);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Список загружаемых полей";
            // 
            // bFieldDown
            // 
            this.bFieldDown.Image = ((System.Drawing.Image)(resources.GetObject("bFieldDown.Image")));
            this.bFieldDown.Location = new System.Drawing.Point(261, 47);
            this.bFieldDown.Name = "bFieldDown";
            this.bFieldDown.Size = new System.Drawing.Size(23, 23);
            this.bFieldDown.TabIndex = 4;
            this.bFieldDown.UseVisualStyleBackColor = true;
            this.bFieldDown.Click += new System.EventHandler(this.bFieldDown_Click);
            // 
            // bFieldUp
            // 
            this.bFieldUp.Image = ((System.Drawing.Image)(resources.GetObject("bFieldUp.Image")));
            this.bFieldUp.Location = new System.Drawing.Point(261, 18);
            this.bFieldUp.Name = "bFieldUp";
            this.bFieldUp.Size = new System.Drawing.Size(23, 23);
            this.bFieldUp.TabIndex = 3;
            this.bFieldUp.UseVisualStyleBackColor = true;
            this.bFieldUp.Click += new System.EventHandler(this.bFieldUp_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbFS);
            this.groupBox3.Controls.Add(this.cbBalance);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.cbDublicateMsisdn);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.cbOldToCurrent);
            this.groupBox3.Controls.Add(this.cbRemoveQuotes);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.cbRegion);
            this.groupBox3.Controls.Add(this.cbPlan);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.bNewParty);
            this.groupBox3.Controls.Add(this.nudPartyNum);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.cbSeparator);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(310, 229);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(274, 246);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Опции";
            // 
            // cbFS
            // 
            this.cbFS.AutoSize = true;
            this.cbFS.Location = new System.Drawing.Point(224, 71);
            this.cbFS.Name = "cbFS";
            this.cbFS.Size = new System.Drawing.Size(44, 17);
            this.cbFS.TabIndex = 15;
            this.cbFS.Text = "ФС";
            this.cbFS.UseVisualStyleBackColor = true;
            // 
            // cbBalance
            // 
            this.cbBalance.FormattingEnabled = true;
            this.cbBalance.Location = new System.Drawing.Point(56, 149);
            this.cbBalance.Name = "cbBalance";
            this.cbBalance.Size = new System.Drawing.Size(212, 21);
            this.cbBalance.TabIndex = 14;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 152);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Баланс";
            // 
            // cbDublicateMsisdn
            // 
            this.cbDublicateMsisdn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDublicateMsisdn.FormattingEnabled = true;
            this.cbDublicateMsisdn.Items.AddRange(new object[] {
            "Раннюю карту в архив",
            "Раннюю карту удалить"});
            this.cbDublicateMsisdn.Location = new System.Drawing.Point(9, 216);
            this.cbDublicateMsisdn.Name = "cbDublicateMsisdn";
            this.cbDublicateMsisdn.Size = new System.Drawing.Size(258, 21);
            this.cbDublicateMsisdn.TabIndex = 11;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 200);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(133, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Для повторных MSISDN:";
            // 
            // cbOldToCurrent
            // 
            this.cbOldToCurrent.AutoSize = true;
            this.cbOldToCurrent.Location = new System.Drawing.Point(10, 175);
            this.cbOldToCurrent.Name = "cbOldToCurrent";
            this.cbOldToCurrent.Size = new System.Drawing.Size(265, 17);
            this.cbOldToCurrent.TabIndex = 9;
            this.cbOldToCurrent.Text = "Импортировать повторные с новыми данными";
            this.cbOldToCurrent.UseVisualStyleBackColor = true;
            // 
            // cbRemoveQuotes
            // 
            this.cbRemoveQuotes.AutoSize = true;
            this.cbRemoveQuotes.Location = new System.Drawing.Point(119, 47);
            this.cbRemoveQuotes.Name = "cbRemoveQuotes";
            this.cbRemoveQuotes.Size = new System.Drawing.Size(115, 17);
            this.cbRemoveQuotes.TabIndex = 8;
            this.cbRemoveQuotes.Text = "Убирать кавычки";
            this.cbRemoveQuotes.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Регион";
            // 
            // cbRegion
            // 
            this.cbRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRegion.FormattingEnabled = true;
            this.cbRegion.Location = new System.Drawing.Point(56, 122);
            this.cbRegion.Name = "cbRegion";
            this.cbRegion.Size = new System.Drawing.Size(212, 21);
            this.cbRegion.TabIndex = 7;
            // 
            // cbPlan
            // 
            this.cbPlan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPlan.FormattingEnabled = true;
            this.cbPlan.Location = new System.Drawing.Point(34, 95);
            this.cbPlan.Name = "cbPlan";
            this.cbPlan.Size = new System.Drawing.Size(234, 21);
            this.cbPlan.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "ТП";
            // 
            // bNewParty
            // 
            this.bNewParty.Location = new System.Drawing.Point(165, 69);
            this.bNewParty.Name = "bNewParty";
            this.bNewParty.Size = new System.Drawing.Size(53, 20);
            this.bNewParty.TabIndex = 4;
            this.bNewParty.Text = "Новая";
            this.bNewParty.UseVisualStyleBackColor = true;
            this.bNewParty.Click += new System.EventHandler(this.bNewParty_Click);
            // 
            // nudPartyNum
            // 
            this.nudPartyNum.Location = new System.Drawing.Point(56, 69);
            this.nudPartyNum.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nudPartyNum.Name = "nudPartyNum";
            this.nudPartyNum.Size = new System.Drawing.Size(103, 20);
            this.nudPartyNum.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Партия";
            // 
            // cbSeparator
            // 
            this.cbSeparator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSeparator.FormattingEnabled = true;
            this.cbSeparator.Items.AddRange(new object[] {
            "<Tab>",
            "<;>",
            "<:>",
            "<|>",
            "<.>",
            "<,>",
            "<!>",
            "<&>"});
            this.cbSeparator.Location = new System.Drawing.Point(119, 20);
            this.cbSeparator.Name = "cbSeparator";
            this.cbSeparator.Size = new System.Drawing.Size(90, 21);
            this.cbSeparator.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Разделитель";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cbLoadIntoTextEditor);
            this.groupBox4.Controls.Add(this.bSelectSrcFile);
            this.groupBox4.Controls.Add(this.bLoadFromClipboard);
            this.groupBox4.Controls.Add(this.tbSrcFile);
            this.groupBox4.Controls.Add(this.bLoadFromSrcFile);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Location = new System.Drawing.Point(12, 538);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(572, 89);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Источник данных";
            // 
            // cbLoadIntoTextEditor
            // 
            this.cbLoadIntoTextEditor.AutoSize = true;
            this.cbLoadIntoTextEditor.Location = new System.Drawing.Point(367, 60);
            this.cbLoadIntoTextEditor.Name = "cbLoadIntoTextEditor";
            this.cbLoadIntoTextEditor.Size = new System.Drawing.Size(199, 17);
            this.cbLoadIntoTextEditor.TabIndex = 9;
            this.cbLoadIntoTextEditor.Text = "Просмотр в текстовом редакторе";
            this.cbLoadIntoTextEditor.UseVisualStyleBackColor = true;
            // 
            // bSelectSrcFile
            // 
            this.bSelectSrcFile.Location = new System.Drawing.Point(531, 19);
            this.bSelectSrcFile.Name = "bSelectSrcFile";
            this.bSelectSrcFile.Size = new System.Drawing.Size(35, 20);
            this.bSelectSrcFile.TabIndex = 2;
            this.bSelectSrcFile.Text = "...";
            this.bSelectSrcFile.UseVisualStyleBackColor = true;
            this.bSelectSrcFile.Click += new System.EventHandler(this.bSelectSrcFile_Click);
            // 
            // bLoadFromClipboard
            // 
            this.bLoadFromClipboard.Location = new System.Drawing.Point(145, 56);
            this.bLoadFromClipboard.Name = "bLoadFromClipboard";
            this.bLoadFromClipboard.Size = new System.Drawing.Size(174, 23);
            this.bLoadFromClipboard.TabIndex = 8;
            this.bLoadFromClipboard.Text = "Загрузить из буфера обмена";
            this.bLoadFromClipboard.UseVisualStyleBackColor = true;
            this.bLoadFromClipboard.Click += new System.EventHandler(this.bLoadFromClipboard_Click);
            // 
            // tbSrcFile
            // 
            this.tbSrcFile.Location = new System.Drawing.Point(48, 19);
            this.tbSrcFile.Name = "tbSrcFile";
            this.tbSrcFile.Size = new System.Drawing.Size(477, 20);
            this.tbSrcFile.TabIndex = 1;
            // 
            // bLoadFromSrcFile
            // 
            this.bLoadFromSrcFile.Location = new System.Drawing.Point(9, 56);
            this.bLoadFromSrcFile.Name = "bLoadFromSrcFile";
            this.bLoadFromSrcFile.Size = new System.Drawing.Size(130, 23);
            this.bLoadFromSrcFile.TabIndex = 7;
            this.bLoadFromSrcFile.Text = "Загрузить из файла";
            this.bLoadFromSrcFile.UseVisualStyleBackColor = true;
            this.bLoadFromSrcFile.Click += new System.EventHandler(this.bLoadFromSrcFile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Файл";
            // 
            // ofd
            // 
            this.ofd.SupportMultiDottedExtensions = true;
            this.ofd.Title = "Файл-источник";
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(379, 633);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(124, 23);
            this.bOk.TabIndex = 7;
            this.bOk.Text = "Принять партию";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(509, 633);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 8;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.tbCheckICC);
            this.groupBox5.Controls.Add(this.tbCheckMSISDN);
            this.groupBox5.Location = new System.Drawing.Point(12, 481);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(415, 51);
            this.groupBox5.TabIndex = 9;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Проверка значений MSISDN и ICC";
            this.toolTip1.SetToolTip(this.groupBox5, resources.GetString("groupBox5.ToolTip"));
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(220, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "ICC";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "MSISDN";
            // 
            // tbCheckICC
            // 
            this.tbCheckICC.Location = new System.Drawing.Point(250, 19);
            this.tbCheckICC.Name = "tbCheckICC";
            this.tbCheckICC.Size = new System.Drawing.Size(151, 20);
            this.tbCheckICC.TabIndex = 1;
            // 
            // tbCheckMSISDN
            // 
            this.tbCheckMSISDN.Location = new System.Drawing.Point(61, 19);
            this.tbCheckMSISDN.Name = "tbCheckMSISDN";
            this.tbCheckMSISDN.Size = new System.Drawing.Size(135, 20);
            this.tbCheckMSISDN.TabIndex = 0;
            // 
            // pb
            // 
            this.pb.Location = new System.Drawing.Point(136, 633);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(195, 23);
            this.pb.TabIndex = 10;
            // 
            // lOpProgress
            // 
            this.lOpProgress.AutoSize = true;
            this.lOpProgress.Location = new System.Drawing.Point(9, 638);
            this.lOpProgress.Name = "lOpProgress";
            this.lOpProgress.Size = new System.Drawing.Size(121, 13);
            this.lOpProgress.TabIndex = 11;
            this.lOpProgress.Text = "Выполнение операции";
            // 
            // cbShowOnlyCorrect
            // 
            this.cbShowOnlyCorrect.AutoSize = true;
            this.cbShowOnlyCorrect.Location = new System.Drawing.Point(12, 216);
            this.cbShowOnlyCorrect.Name = "cbShowOnlyCorrect";
            this.cbShowOnlyCorrect.Size = new System.Drawing.Size(224, 17);
            this.cbShowOnlyCorrect.TabIndex = 12;
            this.cbShowOnlyCorrect.Text = "Отображать только корректные карты";
            this.cbShowOnlyCorrect.UseVisualStyleBackColor = true;
            this.cbShowOnlyCorrect.CheckedChanged += new System.EventHandler(this.cbShowOnlyCorrect_CheckedChanged);
            // 
            // btnGetSimFromServer
            // 
            this.btnGetSimFromServer.Location = new System.Drawing.Point(433, 492);
            this.btnGetSimFromServer.Name = "btnGetSimFromServer";
            this.btnGetSimFromServer.Size = new System.Drawing.Size(144, 34);
            this.btnGetSimFromServer.TabIndex = 13;
            this.btnGetSimFromServer.Text = "Получить данные от сервера";
            this.btnGetSimFromServer.UseVisualStyleBackColor = true;
            this.btnGetSimFromServer.Click += new System.EventHandler(this.btnGetSimFromServer_Click);
            // 
            // cbShowOnlyNew
            // 
            this.cbShowOnlyNew.AutoSize = true;
            this.cbShowOnlyNew.Location = new System.Drawing.Point(12, 236);
            this.cbShowOnlyNew.Name = "cbShowOnlyNew";
            this.cbShowOnlyNew.Size = new System.Drawing.Size(161, 17);
            this.cbShowOnlyNew.TabIndex = 14;
            this.cbShowOnlyNew.Text = "Отображать только новые";
            this.cbShowOnlyNew.UseVisualStyleBackColor = true;
            this.cbShowOnlyNew.CheckedChanged += new System.EventHandler(this.cbShowOnlyNew_CheckedChanged);
            // 
            // FSimImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 666);
            this.Controls.Add(this.cbShowOnlyNew);
            this.Controls.Add(this.btnGetSimFromServer);
            this.Controls.Add(this.cbShowOnlyCorrect);
            this.Controls.Add(this.lOpProgress);
            this.Controls.Add(this.pb);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gbTable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FSimImport";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Загрузка SIM-карт";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreview)).EndInit();
            this.gbTable.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPartyNum)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPreview;
        private System.Windows.Forms.CheckedListBox clbFields;
        private System.Windows.Forms.GroupBox gbTable;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button bFieldDown;
        private System.Windows.Forms.Button bFieldUp;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cbSeparator;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.Button bSelectSrcFile;
        private System.Windows.Forms.TextBox tbSrcFile;
        private System.Windows.Forms.CheckBox cbLoadIntoTextEditor;
        private System.Windows.Forms.Button bLoadFromClipboard;
        private System.Windows.Forms.Button bLoadFromSrcFile;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bNewParty;
        private System.Windows.Forms.NumericUpDown nudPartyNum;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbRegion;
        private System.Windows.Forms.ComboBox cbPlan;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbRemoveQuotes;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbCheckICC;
        private System.Windows.Forms.TextBox tbCheckMSISDN;
        private System.Windows.Forms.ProgressBar pb;
        private System.Windows.Forms.Label lOpProgress;
        private System.Windows.Forms.CheckBox cbOldToCurrent;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbDublicateMsisdn;
        private System.Windows.Forms.CheckBox cbShowOnlyCorrect;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbBalance;
        private System.Windows.Forms.CheckBox cbFS;
        private System.Windows.Forms.Button btnGetSimFromServer;
        private System.Windows.Forms.CheckBox cbShowOnlyNew;
    }
}