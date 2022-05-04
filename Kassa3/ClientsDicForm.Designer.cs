namespace Kassa3
{
    partial class ClientsDicForm
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
            this.pCat = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.bDelCat = new System.Windows.Forms.Button();
            this.bEditCat = new System.Windows.Forms.Button();
            this.bNewCat = new System.Windows.Forms.Button();
            this.tvCat = new System.Windows.Forms.TreeView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.bDelCli = new System.Windows.Forms.Button();
            this.bEditCli = new System.Windows.Forms.Button();
            this.bNewCli = new System.Windows.Forms.Button();
            this.lbClients = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.pCat.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pCat
            // 
            this.pCat.Controls.Add(this.label1);
            this.pCat.Controls.Add(this.bDelCat);
            this.pCat.Controls.Add(this.bEditCat);
            this.pCat.Controls.Add(this.bNewCat);
            this.pCat.Controls.Add(this.tvCat);
            this.pCat.Dock = System.Windows.Forms.DockStyle.Left;
            this.pCat.Location = new System.Drawing.Point(0, 0);
            this.pCat.Name = "pCat";
            this.pCat.Size = new System.Drawing.Size(259, 362);
            this.pCat.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Категории";
            // 
            // bDelCat
            // 
            this.bDelCat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bDelCat.Location = new System.Drawing.Point(174, 327);
            this.bDelCat.Name = "bDelCat";
            this.bDelCat.Size = new System.Drawing.Size(75, 23);
            this.bDelCat.TabIndex = 0;
            this.bDelCat.Text = "Удалить";
            this.bDelCat.UseVisualStyleBackColor = true;
            this.bDelCat.Click += new System.EventHandler(this.bDelCat_Click);
            // 
            // bEditCat
            // 
            this.bEditCat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bEditCat.Location = new System.Drawing.Point(93, 327);
            this.bEditCat.Name = "bEditCat";
            this.bEditCat.Size = new System.Drawing.Size(75, 23);
            this.bEditCat.TabIndex = 0;
            this.bEditCat.Text = "Изменить";
            this.bEditCat.UseVisualStyleBackColor = true;
            this.bEditCat.Click += new System.EventHandler(this.bEditCat_Click);
            // 
            // bNewCat
            // 
            this.bNewCat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bNewCat.Location = new System.Drawing.Point(12, 327);
            this.bNewCat.Name = "bNewCat";
            this.bNewCat.Size = new System.Drawing.Size(75, 23);
            this.bNewCat.TabIndex = 1;
            this.bNewCat.Text = "Новая";
            this.bNewCat.UseVisualStyleBackColor = true;
            this.bNewCat.Click += new System.EventHandler(this.bNewCat_Click);
            // 
            // tvCat
            // 
            this.tvCat.AllowDrop = true;
            this.tvCat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvCat.HideSelection = false;
            this.tvCat.LabelEdit = true;
            this.tvCat.Location = new System.Drawing.Point(12, 25);
            this.tvCat.Name = "tvCat";
            this.tvCat.Size = new System.Drawing.Size(237, 290);
            this.tvCat.TabIndex = 0;
            this.tvCat.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvCat_NodeMouseDoubleClick);
            this.tvCat.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvCat_AfterLabelEdit);
            this.tvCat.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvCat_DragDrop);
            this.tvCat.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvCat_AfterSelect);
            this.tvCat.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvCat_DragEnter);
            this.tvCat.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tvCat_KeyUp);
            this.tvCat.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvCat_ItemDrag);
            this.tvCat.DragOver += new System.Windows.Forms.DragEventHandler(this.tvCat_DragOver);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.bDelCli);
            this.panel2.Controls.Add(this.bEditCli);
            this.panel2.Controls.Add(this.bNewCli);
            this.panel2.Controls.Add(this.lbClients);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(259, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(325, 362);
            this.panel2.TabIndex = 1;
            // 
            // bDelCli
            // 
            this.bDelCli.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bDelCli.Location = new System.Drawing.Point(181, 327);
            this.bDelCli.Name = "bDelCli";
            this.bDelCli.Size = new System.Drawing.Size(75, 23);
            this.bDelCli.TabIndex = 4;
            this.bDelCli.Text = "Удалить";
            this.bDelCli.UseVisualStyleBackColor = true;
            this.bDelCli.Click += new System.EventHandler(this.bDelCli_Click);
            // 
            // bEditCli
            // 
            this.bEditCli.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bEditCli.Location = new System.Drawing.Point(100, 327);
            this.bEditCli.Name = "bEditCli";
            this.bEditCli.Size = new System.Drawing.Size(75, 23);
            this.bEditCli.TabIndex = 3;
            this.bEditCli.Text = "Изменить";
            this.bEditCli.UseVisualStyleBackColor = true;
            this.bEditCli.Click += new System.EventHandler(this.bEditCli_Click);
            // 
            // bNewCli
            // 
            this.bNewCli.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bNewCli.Location = new System.Drawing.Point(19, 327);
            this.bNewCli.Name = "bNewCli";
            this.bNewCli.Size = new System.Drawing.Size(75, 23);
            this.bNewCli.TabIndex = 2;
            this.bNewCli.Text = "Новый";
            this.bNewCli.UseVisualStyleBackColor = true;
            this.bNewCli.Click += new System.EventHandler(this.bNewCli_Click);
            // 
            // lbClients
            // 
            this.lbClients.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbClients.FormattingEnabled = true;
            this.lbClients.IntegralHeight = false;
            this.lbClients.Items.AddRange(new object[] {
            "1132"});
            this.lbClients.Location = new System.Drawing.Point(19, 25);
            this.lbClients.Name = "lbClients";
            this.lbClients.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbClients.Size = new System.Drawing.Size(294, 290);
            this.lbClients.Sorted = true;
            this.lbClients.TabIndex = 1;
            this.lbClients.DoubleClick += new System.EventHandler(this.bEditCli_Click);
            this.lbClients.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbClients_MouseDown);
            this.lbClients.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lbClients_KeyUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Контрагенты";
            // 
            // splitter1
            // 
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitter1.Location = new System.Drawing.Point(259, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(10, 362);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // ClientsDicForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 362);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pCat);
            this.Name = "ClientsDicForm";
            this.Text = "Справочник клиентов";
            this.Shown += new System.EventHandler(this.ClientsDicForm_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientsDicForm_FormClosing);
            this.pCat.ResumeLayout(false);
            this.pCat.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pCat;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bDelCat;
        private System.Windows.Forms.Button bEditCat;
        private System.Windows.Forms.Button bNewCat;
        private System.Windows.Forms.TreeView tvCat;
        private System.Windows.Forms.ListBox lbClients;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bDelCli;
        private System.Windows.Forms.Button bEditCli;
        private System.Windows.Forms.Button bNewCli;
    }
}