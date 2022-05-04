namespace DEXPlugin.Document.Mega.EAD
{
    partial class DocumentForm
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
            this.lDocUnit = new System.Windows.Forms.Label();
            this.cbDocUnit = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbAddrState = new System.Windows.Forms.TextBox();
            this.tbAddrStreet = new System.Windows.Forms.TextBox();
            this.tbFizDocType = new System.Windows.Forms.TextBox();
            this.tbFizDocOrg = new System.Windows.Forms.TextBox();
            this.tbSecondName = new System.Windows.Forms.TextBox();
            this.tbFirstName = new System.Windows.Forms.TextBox();
            this.tbLastName = new System.Windows.Forms.TextBox();
            this.tbAddrRegion = new System.Windows.Forms.TextBox();
            this.tbAddrCity = new System.Windows.Forms.TextBox();
            this.tbDocCity = new System.Windows.Forms.TextBox();
            this.deJournalDate = new DEXExtendLib.DateEdit();
            this.label27 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.cbFizDocScan = new System.Windows.Forms.CheckBox();
            this.deFizDocDate = new DEXExtendLib.DateEdit();
            this.deBirth = new DEXExtendLib.DateEdit();
            this.deDocDate = new DEXExtendLib.DateEdit();
            this.bSearchSIM = new System.Windows.Forms.Button();
            this.lOwner = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.tbAddrApartment = new System.Windows.Forms.TextBox();
            this.tbAddrBuilding = new System.Windows.Forms.TextBox();
            this.tbAddrHouse = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.tbAddrZip = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tbFizDocNumber = new System.Windows.Forms.TextBox();
            this.tbFizDocSeries = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbPlan = new System.Windows.Forms.ComboBox();
            this.tbICCCTL = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.mtbICC = new System.Windows.Forms.MaskedTextBox();
            this.mtbMSISDN = new System.Windows.Forms.MaskedTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbDocCategory = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbDocNum = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.bSaveDefaults = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOk = new System.Windows.Forms.Button();
            this.cbDocStatus = new System.Windows.Forms.ComboBox();
            this.label26 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lDocUnit
            // 
            this.lDocUnit.AutoSize = true;
            this.lDocUnit.Location = new System.Drawing.Point(226, 9);
            this.lDocUnit.Name = "lDocUnit";
            this.lDocUnit.Size = new System.Drawing.Size(62, 13);
            this.lDocUnit.TabIndex = 0;
            this.lDocUnit.Text = "Отделение";
            // 
            // cbDocUnit
            // 
            this.cbDocUnit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbDocUnit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbDocUnit.FormattingEnabled = true;
            this.cbDocUnit.Location = new System.Drawing.Point(294, 6);
            this.cbDocUnit.Name = "cbDocUnit";
            this.cbDocUnit.Size = new System.Drawing.Size(355, 21);
            this.cbDocUnit.TabIndex = 2;
            this.cbDocUnit.Enter += new System.EventHandler(this.cbDocUnit_Enter);
            this.cbDocUnit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbDocUnit_KeyDown);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbAddrState);
            this.panel1.Controls.Add(this.tbAddrStreet);
            this.panel1.Controls.Add(this.tbFizDocType);
            this.panel1.Controls.Add(this.tbFizDocOrg);
            this.panel1.Controls.Add(this.tbSecondName);
            this.panel1.Controls.Add(this.tbFirstName);
            this.panel1.Controls.Add(this.tbLastName);
            this.panel1.Controls.Add(this.tbAddrRegion);
            this.panel1.Controls.Add(this.tbAddrCity);
            this.panel1.Controls.Add(this.tbDocCity);
            this.panel1.Controls.Add(this.deJournalDate);
            this.panel1.Controls.Add(this.label27);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.cbFizDocScan);
            this.panel1.Controls.Add(this.deFizDocDate);
            this.panel1.Controls.Add(this.deBirth);
            this.panel1.Controls.Add(this.deDocDate);
            this.panel1.Controls.Add(this.bSearchSIM);
            this.panel1.Controls.Add(this.lOwner);
            this.panel1.Controls.Add(this.label25);
            this.panel1.Controls.Add(this.label24);
            this.panel1.Controls.Add(this.tbAddrApartment);
            this.panel1.Controls.Add(this.tbAddrBuilding);
            this.panel1.Controls.Add(this.tbAddrHouse);
            this.panel1.Controls.Add(this.label23);
            this.panel1.Controls.Add(this.label22);
            this.panel1.Controls.Add(this.label21);
            this.panel1.Controls.Add(this.label20);
            this.panel1.Controls.Add(this.label19);
            this.panel1.Controls.Add(this.tbAddrZip);
            this.panel1.Controls.Add(this.label18);
            this.panel1.Controls.Add(this.label17);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.tbFizDocNumber);
            this.panel1.Controls.Add(this.tbFizDocSeries);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.cbPlan);
            this.panel1.Controls.Add(this.tbICCCTL);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.mtbICC);
            this.panel1.Controls.Add(this.mtbMSISDN);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.cbDocCategory);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.tbDocNum);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lDocUnit);
            this.panel1.Controls.Add(this.cbDocUnit);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(661, 421);
            this.panel1.TabIndex = 2;
            // 
            // tbAddrState
            // 
            this.tbAddrState.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbAddrState.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbAddrState.Location = new System.Drawing.Point(18, 294);
            this.tbAddrState.Name = "tbAddrState";
            this.tbAddrState.Size = new System.Drawing.Size(172, 20);
            this.tbAddrState.TabIndex = 38;
            this.tbAddrState.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDocCity_KeyDown);
            this.tbAddrState.Enter += new System.EventHandler(this.cbDocUnit_Enter);
            // 
            // tbAddrStreet
            // 
            this.tbAddrStreet.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbAddrStreet.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbAddrStreet.Location = new System.Drawing.Point(18, 343);
            this.tbAddrStreet.Name = "tbAddrStreet";
            this.tbAddrStreet.Size = new System.Drawing.Size(313, 20);
            this.tbAddrStreet.TabIndex = 46;
            this.tbAddrStreet.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbDocUnit_KeyDown);
            this.tbAddrStreet.Enter += new System.EventHandler(this.cbDocUnit_Enter);
            // 
            // tbFizDocType
            // 
            this.tbFizDocType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbFizDocType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbFizDocType.Location = new System.Drawing.Point(15, 224);
            this.tbFizDocType.Name = "tbFizDocType";
            this.tbFizDocType.Size = new System.Drawing.Size(132, 20);
            this.tbFizDocType.TabIndex = 28;
            this.tbFizDocType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDocCity_KeyDown);
            this.tbFizDocType.Leave += new System.EventHandler(this.cbFizDocType_Leave);
            this.tbFizDocType.Enter += new System.EventHandler(this.cbDocUnit_Enter);
            // 
            // tbFizDocOrg
            // 
            this.tbFizDocOrg.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbFizDocOrg.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbFizDocOrg.Location = new System.Drawing.Point(294, 223);
            this.tbFizDocOrg.Name = "tbFizDocOrg";
            this.tbFizDocOrg.Size = new System.Drawing.Size(254, 20);
            this.tbFizDocOrg.TabIndex = 34;
            this.tbFizDocOrg.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbDocUnit_KeyDown);
            this.tbFizDocOrg.Enter += new System.EventHandler(this.cbDocUnit_Enter);
            // 
            // tbSecondName
            // 
            this.tbSecondName.Location = new System.Drawing.Point(356, 173);
            this.tbSecondName.Name = "tbSecondName";
            this.tbSecondName.Size = new System.Drawing.Size(166, 20);
            this.tbSecondName.TabIndex = 24;
            this.tbSecondName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDocCity_KeyDown);
            this.tbSecondName.Enter += new System.EventHandler(this.cbDocUnit_Enter);
            // 
            // tbFirstName
            // 
            this.tbFirstName.Location = new System.Drawing.Point(196, 173);
            this.tbFirstName.Name = "tbFirstName";
            this.tbFirstName.Size = new System.Drawing.Size(154, 20);
            this.tbFirstName.TabIndex = 22;
            this.tbFirstName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDocCity_KeyDown);
            this.tbFirstName.Enter += new System.EventHandler(this.cbDocUnit_Enter);
            // 
            // tbLastName
            // 
            this.tbLastName.Location = new System.Drawing.Point(15, 173);
            this.tbLastName.Name = "tbLastName";
            this.tbLastName.Size = new System.Drawing.Size(175, 20);
            this.tbLastName.TabIndex = 20;
            this.tbLastName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDocCity_KeyDown);
            this.tbLastName.Enter += new System.EventHandler(this.cbDocUnit_Enter);
            // 
            // tbAddrRegion
            // 
            this.tbAddrRegion.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbAddrRegion.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbAddrRegion.Location = new System.Drawing.Point(196, 294);
            this.tbAddrRegion.Name = "tbAddrRegion";
            this.tbAddrRegion.Size = new System.Drawing.Size(194, 20);
            this.tbAddrRegion.TabIndex = 40;
            this.tbAddrRegion.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDocCity_KeyDown);
            this.tbAddrRegion.Enter += new System.EventHandler(this.cbDocUnit_Enter);
            // 
            // tbAddrCity
            // 
            this.tbAddrCity.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbAddrCity.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbAddrCity.Location = new System.Drawing.Point(396, 294);
            this.tbAddrCity.Name = "tbAddrCity";
            this.tbAddrCity.Size = new System.Drawing.Size(180, 20);
            this.tbAddrCity.TabIndex = 42;
            this.tbAddrCity.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbAddrCity_KeyDown);
            this.tbAddrCity.Enter += new System.EventHandler(this.cbDocUnit_Enter);
            // 
            // tbDocCity
            // 
            this.tbDocCity.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbDocCity.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbDocCity.Location = new System.Drawing.Point(254, 57);
            this.tbDocCity.Name = "tbDocCity";
            this.tbDocCity.Size = new System.Drawing.Size(232, 20);
            this.tbDocCity.TabIndex = 7;
            this.tbDocCity.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDocCity_KeyDown);
            this.tbDocCity.Enter += new System.EventHandler(this.cbDocUnit_Enter);
            // 
            // deJournalDate
            // 
            this.deJournalDate.FormattingEnabled = true;
            this.deJournalDate.InputChar = '*';
            this.deJournalDate.Location = new System.Drawing.Point(108, 6);
            this.deJournalDate.MaxLength = 10;
            this.deJournalDate.Name = "deJournalDate";
            this.deJournalDate.Size = new System.Drawing.Size(90, 21);
            this.deJournalDate.TabIndex = 1;
            this.deJournalDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbDocUnit_KeyDown);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(12, 9);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(90, 13);
            this.label27.TabIndex = 53;
            this.label27.Text = "Дата документа";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(629, 171);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(20, 23);
            this.button1.TabIndex = 27;
            this.button1.Text = "?";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbFizDocScan
            // 
            this.cbFizDocScan.AutoSize = true;
            this.cbFizDocScan.Location = new System.Drawing.Point(15, 250);
            this.cbFizDocScan.Name = "cbFizDocScan";
            this.cbFizDocScan.Size = new System.Drawing.Size(175, 17);
            this.cbFizDocScan.TabIndex = 37;
            this.cbFizDocScan.Text = "Скан паспорта предоставлен";
            this.cbFizDocScan.UseVisualStyleBackColor = true;
            this.cbFizDocScan.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbDocUnit_KeyDown);
            // 
            // deFizDocDate
            // 
            this.deFizDocDate.FormattingEnabled = true;
            this.deFizDocDate.InputChar = '*';
            this.deFizDocDate.Location = new System.Drawing.Point(554, 223);
            this.deFizDocDate.MaxLength = 10;
            this.deFizDocDate.Name = "deFizDocDate";
            this.deFizDocDate.Size = new System.Drawing.Size(95, 21);
            this.deFizDocDate.TabIndex = 36;
            this.deFizDocDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbDocUnit_KeyDown);
            // 
            // deBirth
            // 
            this.deBirth.FormattingEnabled = true;
            this.deBirth.InputChar = '*';
            this.deBirth.Location = new System.Drawing.Point(528, 173);
            this.deBirth.MaxLength = 10;
            this.deBirth.Name = "deBirth";
            this.deBirth.Size = new System.Drawing.Size(95, 21);
            this.deBirth.TabIndex = 26;
            this.deBirth.KeyDown += new System.Windows.Forms.KeyEventHandler(this.deBirth_KeyDown);
            // 
            // deDocDate
            // 
            this.deDocDate.FormattingEnabled = true;
            this.deDocDate.InputChar = '*';
            this.deDocDate.Location = new System.Drawing.Point(153, 56);
            this.deDocDate.MaxLength = 10;
            this.deDocDate.Name = "deDocDate";
            this.deDocDate.Size = new System.Drawing.Size(95, 21);
            this.deDocDate.TabIndex = 5;
            this.deDocDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbDocUnit_KeyDown);
            // 
            // bSearchSIM
            // 
            this.bSearchSIM.Location = new System.Drawing.Point(396, 106);
            this.bSearchSIM.Name = "bSearchSIM";
            this.bSearchSIM.Size = new System.Drawing.Size(34, 21);
            this.bSearchSIM.TabIndex = 15;
            this.bSearchSIM.TabStop = false;
            this.bSearchSIM.Text = "SIM";
            this.bSearchSIM.UseVisualStyleBackColor = true;
            this.bSearchSIM.Click += new System.EventHandler(this.bSearchSIM_Click);
            // 
            // lOwner
            // 
            this.lOwner.AutoSize = true;
            this.lOwner.ForeColor = System.Drawing.SystemColors.Desktop;
            this.lOwner.Location = new System.Drawing.Point(150, 130);
            this.lOwner.Name = "lOwner";
            this.lOwner.Size = new System.Drawing.Size(62, 13);
            this.lOwner.TabIndex = 18;
            this.lOwner.Text = "Отделение";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(549, 326);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(55, 13);
            this.label25.TabIndex = 51;
            this.label25.Text = "Квартира";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(443, 326);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(43, 13);
            this.label24.TabIndex = 49;
            this.label24.Text = "Корпус";
            // 
            // tbAddrApartment
            // 
            this.tbAddrApartment.Location = new System.Drawing.Point(552, 343);
            this.tbAddrApartment.Name = "tbAddrApartment";
            this.tbAddrApartment.Size = new System.Drawing.Size(100, 20);
            this.tbAddrApartment.TabIndex = 52;
            this.tbAddrApartment.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbDocUnit_KeyDown);
            // 
            // tbAddrBuilding
            // 
            this.tbAddrBuilding.Location = new System.Drawing.Point(446, 343);
            this.tbAddrBuilding.Name = "tbAddrBuilding";
            this.tbAddrBuilding.Size = new System.Drawing.Size(100, 20);
            this.tbAddrBuilding.TabIndex = 50;
            this.tbAddrBuilding.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbDocUnit_KeyDown);
            // 
            // tbAddrHouse
            // 
            this.tbAddrHouse.Location = new System.Drawing.Point(340, 343);
            this.tbAddrHouse.Name = "tbAddrHouse";
            this.tbAddrHouse.Size = new System.Drawing.Size(100, 20);
            this.tbAddrHouse.TabIndex = 48;
            this.tbAddrHouse.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbDocUnit_KeyDown);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(337, 326);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(30, 13);
            this.label23.TabIndex = 47;
            this.label23.Text = "Дом";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(193, 276);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(38, 13);
            this.label22.TabIndex = 39;
            this.label22.Text = "Район";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(15, 276);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(50, 13);
            this.label21.TabIndex = 37;
            this.label21.Text = "Область";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(15, 326);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(39, 13);
            this.label20.TabIndex = 45;
            this.label20.Text = "Улица";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(584, 276);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(45, 13);
            this.label19.TabIndex = 43;
            this.label19.Text = "Индекс";
            // 
            // tbAddrZip
            // 
            this.tbAddrZip.Location = new System.Drawing.Point(585, 293);
            this.tbAddrZip.Name = "tbAddrZip";
            this.tbAddrZip.Size = new System.Drawing.Size(67, 20);
            this.tbAddrZip.TabIndex = 44;
            this.tbAddrZip.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbAddrZip_KeyDown);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(393, 276);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(37, 13);
            this.label18.TabIndex = 41;
            this.label18.Text = "Город";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(551, 207);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(73, 13);
            this.label17.TabIndex = 35;
            this.label17.Text = "Дата выдачи";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(291, 207);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(69, 13);
            this.label16.TabIndex = 33;
            this.label16.Text = "Кем выдано";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(212, 207);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(41, 13);
            this.label15.TabIndex = 31;
            this.label15.Text = "Номер";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(153, 207);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(38, 13);
            this.label14.TabIndex = 29;
            this.label14.Text = "Серия";
            // 
            // tbFizDocNumber
            // 
            this.tbFizDocNumber.Location = new System.Drawing.Point(215, 223);
            this.tbFizDocNumber.Name = "tbFizDocNumber";
            this.tbFizDocNumber.Size = new System.Drawing.Size(73, 20);
            this.tbFizDocNumber.TabIndex = 32;
            this.tbFizDocNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbDocUnit_KeyDown);
            // 
            // tbFizDocSeries
            // 
            this.tbFizDocSeries.Location = new System.Drawing.Point(156, 223);
            this.tbFizDocSeries.MaxLength = 4;
            this.tbFizDocSeries.Name = "tbFizDocSeries";
            this.tbFizDocSeries.Size = new System.Drawing.Size(53, 20);
            this.tbFizDocSeries.TabIndex = 30;
            this.tbFizDocSeries.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbDocUnit_KeyDown);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 207);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(135, 13);
            this.label13.TabIndex = 27;
            this.label13.Text = "Удостоверение личности";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(525, 157);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(86, 13);
            this.label12.TabIndex = 25;
            this.label12.Text = "Дата рождения";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(353, 157);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(54, 13);
            this.label11.TabIndex = 23;
            this.label11.Text = "Отчество";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(193, 157);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 13);
            this.label10.TabIndex = 21;
            this.label10.Text = "Имя";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 157);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Фамилия";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(440, 90);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Тарифный план";
            // 
            // cbPlan
            // 
            this.cbPlan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPlan.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbPlan.FormattingEnabled = true;
            this.cbPlan.Location = new System.Drawing.Point(443, 106);
            this.cbPlan.Name = "cbPlan";
            this.cbPlan.Size = new System.Drawing.Size(206, 21);
            this.cbPlan.TabIndex = 17;
            this.cbPlan.Enter += new System.EventHandler(this.cbDocUnit_Enter);
            this.cbPlan.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbDocUnit_KeyDown);
            // 
            // tbICCCTL
            // 
            this.tbICCCTL.Location = new System.Drawing.Point(347, 107);
            this.tbICCCTL.MaxLength = 1;
            this.tbICCCTL.Name = "tbICCCTL";
            this.tbICCCTL.Size = new System.Drawing.Size(31, 20);
            this.tbICCCTL.TabIndex = 14;
            this.tbICCCTL.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbDocUnit_KeyDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(153, 91);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "ICC SIM-карты";
            // 
            // mtbICC
            // 
            this.mtbICC.Location = new System.Drawing.Point(153, 107);
            this.mtbICC.Mask = "00000000000000000";
            this.mtbICC.Name = "mtbICC";
            this.mtbICC.PromptChar = '*';
            this.mtbICC.Size = new System.Drawing.Size(188, 20);
            this.mtbICC.TabIndex = 13;
            this.mtbICC.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mtbICC_KeyDown);
            // 
            // mtbMSISDN
            // 
            this.mtbMSISDN.Location = new System.Drawing.Point(15, 107);
            this.mtbMSISDN.Mask = "0000000000";
            this.mtbMSISDN.Name = "mtbMSISDN";
            this.mtbMSISDN.PromptChar = '*';
            this.mtbMSISDN.Size = new System.Drawing.Size(132, 20);
            this.mtbMSISDN.TabIndex = 11;
            this.mtbMSISDN.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mtbMSISDN_KeyDown);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 91);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Абонентский №";
            // 
            // cbDocCategory
            // 
            this.cbDocCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDocCategory.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbDocCategory.FormattingEnabled = true;
            this.cbDocCategory.Items.AddRange(new object[] {
            "Резидент",
            "Нерезидент"});
            this.cbDocCategory.Location = new System.Drawing.Point(498, 56);
            this.cbDocCategory.Name = "cbDocCategory";
            this.cbDocCategory.Size = new System.Drawing.Size(151, 21);
            this.cbDocCategory.TabIndex = 9;
            this.cbDocCategory.Enter += new System.EventHandler(this.cbDocUnit_Enter);
            this.cbDocCategory.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbDocUnit_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(495, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Категория";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(251, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Город";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(150, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Дата договора";
            // 
            // tbDocNum
            // 
            this.tbDocNum.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbDocNum.Location = new System.Drawing.Point(15, 57);
            this.tbDocNum.Name = "tbDocNum";
            this.tbDocNum.Size = new System.Drawing.Size(132, 20);
            this.tbDocNum.TabIndex = 3;
            this.tbDocNum.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbDocUnit_KeyDown);
            this.tbDocNum.Enter += new System.EventHandler(this.tbDocNum_Enter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "№ договора";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.bSaveDefaults);
            this.panel2.Controls.Add(this.bCancel);
            this.panel2.Controls.Add(this.bOk);
            this.panel2.Controls.Add(this.cbDocStatus);
            this.panel2.Controls.Add(this.label26);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 382);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(661, 39);
            this.panel2.TabIndex = 3;
            // 
            // bSaveDefaults
            // 
            this.bSaveDefaults.Location = new System.Drawing.Point(4, 6);
            this.bSaveDefaults.Name = "bSaveDefaults";
            this.bSaveDefaults.Size = new System.Drawing.Size(75, 23);
            this.bSaveDefaults.TabIndex = 4;
            this.bSaveDefaults.Text = "Запомнить";
            this.toolTip1.SetToolTip(this.bSaveDefaults, "Запомнить содержимое полей документа. При создании нового документа эти значения " +
                    "будут проставляться автоматически.");
            this.bSaveDefaults.UseVisualStyleBackColor = true;
            this.bSaveDefaults.Click += new System.EventHandler(this.bSaveDefaults_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(581, 6);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 3;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(500, 6);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 23);
            this.bOk.TabIndex = 2;
            this.bOk.Text = "Сохранить";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // cbDocStatus
            // 
            this.cbDocStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDocStatus.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbDocStatus.FormattingEnabled = true;
            this.cbDocStatus.Items.AddRange(new object[] {
            "Черновик",
            "На подтверждение",
            "Подтверждён",
            "На отправку"});
            this.cbDocStatus.Location = new System.Drawing.Point(279, 8);
            this.cbDocStatus.Name = "cbDocStatus";
            this.cbDocStatus.Size = new System.Drawing.Size(215, 21);
            this.cbDocStatus.TabIndex = 1;
            this.cbDocStatus.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbDocStatus_KeyDown);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(175, 11);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(98, 13);
            this.label26.TabIndex = 0;
            this.label26.Text = "Статус документа";
            // 
            // DocumentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(661, 421);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DocumentForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "МегаФон ЕАД";
            this.Shown += new System.EventHandler(this.DocumentForm_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DocumentForm_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lDocUnit;
        private System.Windows.Forms.ComboBox cbDocUnit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cbDocCategory;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbDocNum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbPlan;
        private System.Windows.Forms.TextBox tbICCCTL;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.MaskedTextBox mtbICC;
        private System.Windows.Forms.MaskedTextBox mtbMSISDN;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tbFizDocNumber;
        private System.Windows.Forms.TextBox tbFizDocSeries;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox tbAddrZip;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox tbAddrApartment;
        private System.Windows.Forms.TextBox tbAddrBuilding;
        private System.Windows.Forms.TextBox tbAddrHouse;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label lOwner;
        private System.Windows.Forms.Button bSearchSIM;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.ComboBox cbDocStatus;
        private System.Windows.Forms.Label label26;
        private DEXExtendLib.DateEdit deDocDate;
        private DEXExtendLib.DateEdit deBirth;
        private DEXExtendLib.DateEdit deFizDocDate;
        private System.Windows.Forms.CheckBox cbFizDocScan;
        private System.Windows.Forms.Button button1;
        private DEXExtendLib.DateEdit deJournalDate;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox tbDocCity;
        private System.Windows.Forms.TextBox tbAddrCity;
        private System.Windows.Forms.TextBox tbAddrRegion;
        private System.Windows.Forms.TextBox tbSecondName;
        private System.Windows.Forms.TextBox tbFirstName;
        private System.Windows.Forms.TextBox tbLastName;
        private System.Windows.Forms.TextBox tbFizDocOrg;
        private System.Windows.Forms.TextBox tbFizDocType;
        private System.Windows.Forms.TextBox tbAddrStreet;
        private System.Windows.Forms.TextBox tbAddrState;
        private System.Windows.Forms.Button bSaveDefaults;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}