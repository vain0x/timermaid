namespace VainZero.Timermaid.Desktop
{
    partial class AppNotifyIcon
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyIconMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.notifyIconMenuOk = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIconMenuShow = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIconMenuSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.notifyIconMenuQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIconMenu.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.notifyIconMenu;
            this.notifyIcon.Text = "Timermaid";
            this.notifyIcon.Visible = true;
            // 
            // notifyIconMenu
            // 
            this.notifyIconMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.notifyIconMenuOk,
            this.notifyIconMenuShow,
            this.notifyIconMenuSeparator,
            this.notifyIconMenuQuit});
            this.notifyIconMenu.Name = "notifyIconMenu";
            this.notifyIconMenu.Size = new System.Drawing.Size(121, 76);
            // 
            // notifyIconMenuOk
            // 
            this.notifyIconMenuOk.Name = "notifyIconMenuOk";
            this.notifyIconMenuOk.Size = new System.Drawing.Size(120, 22);
            this.notifyIconMenuOk.Text = "OK (& )";
            // 
            // notifyIconMenuShow
            // 
            this.notifyIconMenuShow.Name = "notifyIconMenuShow";
            this.notifyIconMenuShow.Size = new System.Drawing.Size(120, 22);
            this.notifyIconMenuShow.Text = "Show (&S)";
            // 
            // notifyIconMenuSeparator
            // 
            this.notifyIconMenuSeparator.Name = "notifyIconMenuSeparator";
            this.notifyIconMenuSeparator.Size = new System.Drawing.Size(117, 6);
            // 
            // notifyIconMenuQuit
            // 
            this.notifyIconMenuQuit.Name = "notifyIconMenuQuit";
            this.notifyIconMenuQuit.Size = new System.Drawing.Size(120, 22);
            this.notifyIconMenuQuit.Text = "Quit (&Q)";
            this.notifyIconMenu.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip notifyIconMenu;
        private System.Windows.Forms.ToolStripMenuItem notifyIconMenuOk;
        private System.Windows.Forms.ToolStripMenuItem notifyIconMenuShow;
        private System.Windows.Forms.ToolStripSeparator notifyIconMenuSeparator;
        private System.Windows.Forms.ToolStripMenuItem notifyIconMenuQuit;
    }
}
