namespace DEXOL_NEW
{
    partial class AppsList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppsList));
            this.sb_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.gс_apps_list = new DevExpress.XtraGrid.GridControl();
            this.gv_list = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.gс_apps_list)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_list)).BeginInit();
            this.SuspendLayout();
            // 
            // sb_cancel
            // 
            this.sb_cancel.Location = new System.Drawing.Point(152, 328);
            this.sb_cancel.Name = "sb_cancel";
            this.sb_cancel.Size = new System.Drawing.Size(75, 23);
            this.sb_cancel.TabIndex = 0;
            this.sb_cancel.Text = "Отмена";
            this.sb_cancel.Click += new System.EventHandler(this.sb_cancel_Click);
            // 
            // gс_apps_list
            // 
            this.gс_apps_list.Location = new System.Drawing.Point(12, 12);
            this.gс_apps_list.MainView = this.gv_list;
            this.gс_apps_list.Name = "gс_apps_list";
            this.gс_apps_list.Size = new System.Drawing.Size(215, 310);
            this.gс_apps_list.TabIndex = 1;
            this.gс_apps_list.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gv_list});
            // 
            // gv_list
            // 
            this.gv_list.GridControl = this.gс_apps_list;
            this.gv_list.Name = "gv_list";
            this.gv_list.OptionsBehavior.Editable = false;
            this.gv_list.OptionsCustomization.AllowColumnMoving = false;
            this.gv_list.OptionsCustomization.AllowColumnResizing = false;
            this.gv_list.OptionsCustomization.AllowFilter = false;
            this.gv_list.OptionsCustomization.AllowGroup = false;
            this.gv_list.OptionsCustomization.AllowQuickHideColumns = false;
            this.gv_list.OptionsCustomization.AllowSort = false;
            this.gv_list.OptionsMenu.EnableColumnMenu = false;
            this.gv_list.OptionsMenu.EnableFooterMenu = false;
            this.gv_list.OptionsMenu.EnableGroupPanelMenu = false;
            this.gv_list.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gv_list.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gv_list.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gv_list.OptionsView.ShowGroupPanel = false;
            this.gv_list.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            this.gv_list.DoubleClick += new System.EventHandler(this.gv_list_DoubleClick);
            // 
            // AppsList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(239, 363);
            this.ControlBox = false;
            this.Controls.Add(this.gс_apps_list);
            this.Controls.Add(this.sb_cancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AppsList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Доступные приложения";
            ((System.ComponentModel.ISupportInitialize)(this.gс_apps_list)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_list)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton sb_cancel;
        public DevExpress.XtraGrid.Views.Grid.GridView gv_list;
        public DevExpress.XtraGrid.GridControl gс_apps_list;
    }
}