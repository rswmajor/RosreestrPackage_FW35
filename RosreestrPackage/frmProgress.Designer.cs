namespace RosreestrPackage
{
    partial class frmProgress
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
            this.lblProgress = new System.Windows.Forms.Label();
            this.pbLoad = new System.Windows.Forms.PictureBox();
            this.lblProgressTop = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoad)).BeginInit();
            this.SuspendLayout();
            // 
            // lblProgress
            // 
            this.lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgress.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblProgress.Location = new System.Drawing.Point(67, 28);
            this.lblProgress.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(294, 39);
            this.lblProgress.TabIndex = 1;
            this.lblProgress.Text = "Заггрузка...";
            // 
            // pbLoad
            // 
            this.pbLoad.Image = global::RosreestrPackage.Properties.Resources.load;
            this.pbLoad.Location = new System.Drawing.Point(12, 12);
            this.pbLoad.Name = "pbLoad";
            this.pbLoad.Size = new System.Drawing.Size(49, 50);
            this.pbLoad.TabIndex = 0;
            this.pbLoad.TabStop = false;
            // 
            // lblProgressTop
            // 
            this.lblProgressTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgressTop.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblProgressTop.Location = new System.Drawing.Point(67, 9);
            this.lblProgressTop.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblProgressTop.Name = "lblProgressTop";
            this.lblProgressTop.Size = new System.Drawing.Size(294, 19);
            this.lblProgressTop.TabIndex = 2;
            // 
            // frmProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 76);
            this.Controls.Add(this.lblProgressTop);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.pbLoad);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmProgress";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Выполнение";
            ((System.ComponentModel.ISupportInitialize)(this.pbLoad)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbLoad;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Label lblProgressTop;
    }
}