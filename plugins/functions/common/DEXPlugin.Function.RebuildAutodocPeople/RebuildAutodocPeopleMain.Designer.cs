using System;
namespace DEXPlugin.Function.RebuildAutodocPeople
{
    partial class RebuildAutodocPeopleMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RebuildAutodocPeopleMain));
            this.label2 = new System.Windows.Forms.Label();
            this.cbSource = new System.Windows.Forms.ComboBox();
            this.cbInterval = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.bBuild = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.bClear = new System.Windows.Forms.Button();
            this.gbUnits = new System.Windows.Forms.GroupBox();
            this.rbNotUse = new System.Windows.Forms.RadioButton();
            this.clbUnits = new System.Windows.Forms.CheckedListBox();
            this.rbExclude = new System.Windows.Forms.RadioButton();
            this.rbInclude = new System.Windows.Forms.RadioButton();
            this.gbRepeatControl = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nudDocCount = new System.Windows.Forms.NumericUpDown();
            this.cbRestrictDocCount = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bCheckBadDocs = new System.Windows.Forms.Button();
            this.cbBadDocsReport = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbUserToRegionBind = new System.Windows.Forms.CheckBox();
            this.tbNotReassign = new System.Windows.Forms.TextBox();
            this.bHelpNotReassign = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.tbFizDocType = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbDocType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.deFrom = new DEXExtendLib.DateEdit();
            this.deTo = new DEXExtendLib.DateEdit();
            this.gbUnits.SuspendLayout();
            this.gbRepeatControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDocCount)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Источник документов";
            // 
            // cbSource
            // 
            this.cbSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSource.FormattingEnabled = true;
            this.cbSource.Items.AddRange(new object[] {
            "Только архив",
            "Только активные документы",
            "Архив и активные документы"});
            this.cbSource.Location = new System.Drawing.Point(130, 13);
            this.cbSource.Name = "cbSource";
            this.cbSource.Size = new System.Drawing.Size(294, 21);
            this.cbSource.TabIndex = 3;
            // 
            // cbInterval
            // 
            this.cbInterval.AutoSize = true;
            this.cbInterval.Location = new System.Drawing.Point(9, 180);
            this.cbInterval.Name = "cbInterval";
            this.cbInterval.Size = new System.Drawing.Size(134, 17);
            this.cbInterval.TabIndex = 6;
            this.cbInterval.Text = "Интервал выборки: с";
            this.cbInterval.UseVisualStyleBackColor = true;
            this.cbInterval.CheckedChanged += new System.EventHandler(this.cbInterval_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(309, 181);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "по";
            // 
            // bBuild
            // 
            this.bBuild.Location = new System.Drawing.Point(186, 486);
            this.bBuild.Name = "bBuild";
            this.bBuild.Size = new System.Drawing.Size(112, 23);
            this.bBuild.TabIndex = 10;
            this.bBuild.Text = "Построить базу";
            this.bBuild.UseVisualStyleBackColor = true;
            this.bBuild.Click += new System.EventHandler(this.bBuild_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(343, 486);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 11;
            this.bCancel.Text = "Выход";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bClear
            // 
            this.bClear.Location = new System.Drawing.Point(6, 486);
            this.bClear.Name = "bClear";
            this.bClear.Size = new System.Drawing.Size(174, 23);
            this.bClear.TabIndex = 12;
            this.bClear.Text = "Очистить текущую базу";
            this.bClear.UseVisualStyleBackColor = true;
            this.bClear.Click += new System.EventHandler(this.bClear_Click);
            // 
            // gbUnits
            // 
            this.gbUnits.Controls.Add(this.rbNotUse);
            this.gbUnits.Controls.Add(this.clbUnits);
            this.gbUnits.Controls.Add(this.rbExclude);
            this.gbUnits.Controls.Add(this.rbInclude);
            this.gbUnits.Location = new System.Drawing.Point(9, 205);
            this.gbUnits.Name = "gbUnits";
            this.gbUnits.Size = new System.Drawing.Size(415, 191);
            this.gbUnits.TabIndex = 13;
            this.gbUnits.TabStop = false;
            this.gbUnits.Text = "Фильтр отделений";
            // 
            // rbNotUse
            // 
            this.rbNotUse.AutoSize = true;
            this.rbNotUse.Location = new System.Drawing.Point(6, 19);
            this.rbNotUse.Name = "rbNotUse";
            this.rbNotUse.Size = new System.Drawing.Size(113, 17);
            this.rbNotUse.TabIndex = 0;
            this.rbNotUse.TabStop = true;
            this.rbNotUse.Text = "Не использовать";
            this.rbNotUse.UseVisualStyleBackColor = true;
            this.rbNotUse.CheckedChanged += new System.EventHandler(this.rbNotUse_CheckedChanged);
            // 
            // clbUnits
            // 
            this.clbUnits.FormattingEnabled = true;
            this.clbUnits.Location = new System.Drawing.Point(6, 42);
            this.clbUnits.Name = "clbUnits";
            this.clbUnits.Size = new System.Drawing.Size(400, 139);
            this.clbUnits.TabIndex = 3;
            // 
            // rbExclude
            // 
            this.rbExclude.AutoSize = true;
            this.rbExclude.Location = new System.Drawing.Point(205, 19);
            this.rbExclude.Name = "rbExclude";
            this.rbExclude.Size = new System.Drawing.Size(201, 17);
            this.rbExclude.TabIndex = 2;
            this.rbExclude.TabStop = true;
            this.rbExclude.Text = "Исключить следующие отделения:";
            this.rbExclude.UseVisualStyleBackColor = true;
            // 
            // rbInclude
            // 
            this.rbInclude.AutoSize = true;
            this.rbInclude.Location = new System.Drawing.Point(125, 19);
            this.rbInclude.Name = "rbInclude";
            this.rbInclude.Size = new System.Drawing.Size(74, 17);
            this.rbInclude.TabIndex = 1;
            this.rbInclude.TabStop = true;
            this.rbInclude.Text = "Включить";
            this.rbInclude.UseVisualStyleBackColor = true;
            // 
            // gbRepeatControl
            // 
            this.gbRepeatControl.Controls.Add(this.label3);
            this.gbRepeatControl.Controls.Add(this.label1);
            this.gbRepeatControl.Controls.Add(this.nudDocCount);
            this.gbRepeatControl.Controls.Add(this.cbRestrictDocCount);
            this.gbRepeatControl.Location = new System.Drawing.Point(9, 402);
            this.gbRepeatControl.Name = "gbRepeatControl";
            this.gbRepeatControl.Size = new System.Drawing.Size(415, 78);
            this.gbRepeatControl.TabIndex = 14;
            this.gbRepeatControl.TabStop = false;
            this.gbRepeatControl.Text = "Контроль повторяемости по всей базе";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(6, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(409, 31);
            this.label3.TabIndex = 3;
            this.label3.Text = "Внимание! Использование этого ограничения может существенно увеличить продолжител" +
                "ьность обработки.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(349, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "договоров";
            // 
            // nudDocCount
            // 
            this.nudDocCount.Location = new System.Drawing.Point(238, 18);
            this.nudDocCount.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.nudDocCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDocCount.Name = "nudDocCount";
            this.nudDocCount.Size = new System.Drawing.Size(105, 20);
            this.nudDocCount.TabIndex = 1;
            this.nudDocCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cbRestrictDocCount
            // 
            this.cbRestrictDocCount.AutoSize = true;
            this.cbRestrictDocCount.Location = new System.Drawing.Point(6, 19);
            this.cbRestrictDocCount.Name = "cbRestrictDocCount";
            this.cbRestrictDocCount.Size = new System.Drawing.Size(226, 17);
            this.cbRestrictDocCount.TabIndex = 0;
            this.cbRestrictDocCount.Text = "Только абоненты, на которых не более";
            this.cbRestrictDocCount.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.bCheckBadDocs);
            this.groupBox1.Controls.Add(this.cbBadDocsReport);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(17, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(436, 137);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Проверка корректности паспортных данных";
            // 
            // bCheckBadDocs
            // 
            this.bCheckBadDocs.Location = new System.Drawing.Point(264, 105);
            this.bCheckBadDocs.Name = "bCheckBadDocs";
            this.bCheckBadDocs.Size = new System.Drawing.Size(160, 23);
            this.bCheckBadDocs.TabIndex = 6;
            this.bCheckBadDocs.Text = "Начать проверку";
            this.bCheckBadDocs.UseVisualStyleBackColor = true;
            this.bCheckBadDocs.Click += new System.EventHandler(this.bCheckBadDocs_Click);
            // 
            // cbBadDocsReport
            // 
            this.cbBadDocsReport.AutoSize = true;
            this.cbBadDocsReport.Checked = true;
            this.cbBadDocsReport.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbBadDocsReport.Location = new System.Drawing.Point(9, 109);
            this.cbBadDocsReport.Name = "cbBadDocsReport";
            this.cbBadDocsReport.Size = new System.Drawing.Size(249, 17);
            this.cbBadDocsReport.TabIndex = 5;
            this.cbBadDocsReport.Text = "Получить отчёт о некорректных документах";
            this.cbBadDocsReport.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(6, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(424, 86);
            this.label5.TabIndex = 4;
            this.label5.Text = resources.GetString("label5.Text");
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbUserToRegionBind);
            this.groupBox2.Controls.Add(this.tbNotReassign);
            this.groupBox2.Controls.Add(this.bHelpNotReassign);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.tbFizDocType);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.cbDocType);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cbSource);
            this.groupBox2.Controls.Add(this.gbRepeatControl);
            this.groupBox2.Controls.Add(this.cbInterval);
            this.groupBox2.Controls.Add(this.gbUnits);
            this.groupBox2.Controls.Add(this.deFrom);
            this.groupBox2.Controls.Add(this.bClear);
            this.groupBox2.Controls.Add(this.deTo);
            this.groupBox2.Controls.Add(this.bCancel);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.bBuild);
            this.groupBox2.Location = new System.Drawing.Point(17, 155);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(436, 518);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Построение базы автодока";
            // 
            // cbUserToRegionBind
            // 
            this.cbUserToRegionBind.AutoSize = true;
            this.cbUserToRegionBind.Checked = true;
            this.cbUserToRegionBind.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUserToRegionBind.Location = new System.Drawing.Point(9, 152);
            this.cbUserToRegionBind.Name = "cbUserToRegionBind";
            this.cbUserToRegionBind.Size = new System.Drawing.Size(350, 17);
            this.cbUserToRegionBind.TabIndex = 22;
            this.cbUserToRegionBind.Text = "Привязать абонента к региону согласно справочника сим-карт";
            this.cbUserToRegionBind.UseVisualStyleBackColor = true;
            // 
            // tbNotReassign
            // 
            this.tbNotReassign.Location = new System.Drawing.Point(9, 121);
            this.tbNotReassign.Name = "tbNotReassign";
            this.tbNotReassign.Size = new System.Drawing.Size(388, 20);
            this.tbNotReassign.TabIndex = 21;
            // 
            // bHelpNotReassign
            // 
            this.bHelpNotReassign.Location = new System.Drawing.Point(403, 104);
            this.bHelpNotReassign.Name = "bHelpNotReassign";
            this.bHelpNotReassign.Size = new System.Drawing.Size(21, 37);
            this.bHelpNotReassign.TabIndex = 20;
            this.bHelpNotReassign.Text = "?";
            this.bHelpNotReassign.UseVisualStyleBackColor = true;
            this.bHelpNotReassign.Click += new System.EventHandler(this.bHelpNotReassign_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 104);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(298, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "Не переприсваивать паспорта на следующие отделения:";
            // 
            // tbFizDocType
            // 
            this.tbFizDocType.Location = new System.Drawing.Point(198, 67);
            this.tbFizDocType.Name = "tbFizDocType";
            this.tbFizDocType.Size = new System.Drawing.Size(226, 20);
            this.tbFizDocType.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(6, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(180, 37);
            this.label7.TabIndex = 17;
            this.label7.Text = "Допустимые типы документов, через запятую (поле FizDocType)";
            // 
            // cbDocType
            // 
            this.cbDocType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDocType.FormattingEnabled = true;
            this.cbDocType.Location = new System.Drawing.Point(130, 40);
            this.cbDocType.Name = "cbDocType";
            this.cbDocType.Size = new System.Drawing.Size(294, 21);
            this.cbDocType.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Тип документов";
            // 
            // deFrom
            // 
            this.deFrom.FormattingEnabled = true;
            this.deFrom.InputChar = '_';
            this.deFrom.Location = new System.Drawing.Point(213, 178);
            this.deFrom.MaxLength = 10;
            this.deFrom.Name = "deFrom";
            this.deFrom.Size = new System.Drawing.Size(90, 21);
            this.deFrom.TabIndex = 7;
            // 
            // deTo
            // 
            this.deTo.FormattingEnabled = true;
            this.deTo.InputChar = '_';
            this.deTo.Location = new System.Drawing.Point(334, 178);
            this.deTo.MaxLength = 10;
            this.deTo.Name = "deTo";
            this.deTo.Size = new System.Drawing.Size(90, 21);
            this.deTo.TabIndex = 8;
            // 
            // RebuildAutodocPeopleMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(462, 685);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RebuildAutodocPeopleMain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Построение базы автодока";
            this.gbUnits.ResumeLayout(false);
            this.gbUnits.PerformLayout();
            this.gbRepeatControl.ResumeLayout(false);
            this.gbRepeatControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDocCount)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbSource;
        private System.Windows.Forms.CheckBox cbInterval;
        private DEXExtendLib.DateEdit deFrom;
        private DEXExtendLib.DateEdit deTo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button bBuild;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bClear;
        private System.Windows.Forms.GroupBox gbUnits;
        private System.Windows.Forms.RadioButton rbExclude;
        private System.Windows.Forms.RadioButton rbInclude;
        private System.Windows.Forms.CheckedListBox clbUnits;
        private System.Windows.Forms.RadioButton rbNotUse;
        private System.Windows.Forms.GroupBox gbRepeatControl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudDocCount;
        private System.Windows.Forms.CheckBox cbRestrictDocCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button bCheckBadDocs;
        private System.Windows.Forms.CheckBox cbBadDocsReport;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbFizDocType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbDocType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbNotReassign;
        private System.Windows.Forms.Button bHelpNotReassign;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cbUserToRegionBind;

    }
}