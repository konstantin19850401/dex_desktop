namespace DEXOL_NEW
{
    partial class DictForm
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
            this.gs_table = new DevExpress.XtraGrid.GridControl();
            this.gv_table = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.sb_addItem = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gs_table)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_table)).BeginInit();
            this.SuspendLayout();
            // 
            // gs_table
            // 
            this.gs_table.Location = new System.Drawing.Point(12, 12);
            this.gs_table.MainView = this.gv_table;
            this.gs_table.Name = "gs_table";
            this.gs_table.Size = new System.Drawing.Size(381, 509);
            this.gs_table.TabIndex = 1;
            this.gs_table.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gv_table});
            this.gs_table.DoubleClick += new System.EventHandler(this.gs_table_DoubleClick);
            // 
            // gv_table
            // 
            this.gv_table.GridControl = this.gs_table;
            this.gv_table.Name = "gv_table";
            this.gv_table.OptionsBehavior.Editable = false;
            this.gv_table.OptionsView.ShowGroupPanel = false;
            // 
            // sb_addItem
            // 
            this.sb_addItem.Location = new System.Drawing.Point(278, 527);
            this.sb_addItem.Name = "sb_addItem";
            this.sb_addItem.Size = new System.Drawing.Size(115, 23);
            this.sb_addItem.TabIndex = 2;
            this.sb_addItem.Text = "Добавить элемент";
            this.sb_addItem.Click += new System.EventHandler(this.sb_addItem_Click);
            // 
            // DictForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 562);
            this.Controls.Add(this.sb_addItem);
            this.Controls.Add(this.gs_table);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DictForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DictForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DictForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.gs_table)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_table)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraGrid.GridControl gs_table;
        private DevExpress.XtraGrid.Views.Grid.GridView gv_table;
        private DevExpress.XtraEditors.SimpleButton sb_addItem;
    }
}