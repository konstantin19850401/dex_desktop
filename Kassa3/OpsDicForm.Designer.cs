namespace Kassa3
{
    partial class OpsDicForm
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
            this.lbOps = new System.Windows.Forms.ListBox();
            this.bNew = new System.Windows.Forms.Button();
            this.bEdit = new System.Windows.Forms.Button();
            this.bDelete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbOps
            // 
            this.lbOps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbOps.FormattingEnabled = true;
            this.lbOps.IntegralHeight = false;
            this.lbOps.Location = new System.Drawing.Point(12, 12);
            this.lbOps.Name = "lbOps";
            this.lbOps.Size = new System.Drawing.Size(276, 300);
            this.lbOps.TabIndex = 0;
            this.lbOps.DoubleClick += new System.EventHandler(this.bEdit_Click);
            this.lbOps.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lbOps_KeyUp);
            // 
            // bNew
            // 
            this.bNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bNew.Location = new System.Drawing.Point(12, 318);
            this.bNew.Name = "bNew";
            this.bNew.Size = new System.Drawing.Size(75, 23);
            this.bNew.TabIndex = 1;
            this.bNew.Text = "Новая";
            this.bNew.UseVisualStyleBackColor = true;
            this.bNew.Click += new System.EventHandler(this.bNew_Click);
            // 
            // bEdit
            // 
            this.bEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bEdit.Location = new System.Drawing.Point(93, 318);
            this.bEdit.Name = "bEdit";
            this.bEdit.Size = new System.Drawing.Size(115, 23);
            this.bEdit.TabIndex = 2;
            this.bEdit.Text = "Переименовать";
            this.bEdit.UseVisualStyleBackColor = true;
            this.bEdit.Click += new System.EventHandler(this.bEdit_Click);
            // 
            // bDelete
            // 
            this.bDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bDelete.Location = new System.Drawing.Point(214, 318);
            this.bDelete.Name = "bDelete";
            this.bDelete.Size = new System.Drawing.Size(75, 23);
            this.bDelete.TabIndex = 3;
            this.bDelete.Text = "Удалить";
            this.bDelete.UseVisualStyleBackColor = true;
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // OpsDicForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 353);
            this.Controls.Add(this.bDelete);
            this.Controls.Add(this.bEdit);
            this.Controls.Add(this.bNew);
            this.Controls.Add(this.lbOps);
            this.MinimumSize = new System.Drawing.Size(315, 390);
            this.Name = "OpsDicForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Справочник операций";
            this.Shown += new System.EventHandler(this.OpsDicForm_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OpsDicForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbOps;
        private System.Windows.Forms.Button bNew;
        private System.Windows.Forms.Button bEdit;
        private System.Windows.Forms.Button bDelete;
    }
}