namespace RosreestrPackage
{
    partial class frmSelectCertificate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectCertificate));
            this.lvCertificates = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.chbShowAllCerts = new System.Windows.Forms.CheckBox();
            this.chbShowOldCerts = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lvCertificates
            // 
            this.lvCertificates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvCertificates.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lvCertificates.FullRowSelect = true;
            this.lvCertificates.Location = new System.Drawing.Point(12, 36);
            this.lvCertificates.MultiSelect = false;
            this.lvCertificates.Name = "lvCertificates";
            this.lvCertificates.Size = new System.Drawing.Size(685, 277);
            this.lvCertificates.TabIndex = 0;
            this.lvCertificates.UseCompatibleStateImageBehavior = false;
            this.lvCertificates.View = System.Windows.Forms.View.Details;
            this.lvCertificates.SelectedIndexChanged += new System.EventHandler(this.lvCertificates_SelectedIndexChanged);
            this.lvCertificates.DoubleClick += new System.EventHandler(this.lvCertificates_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Субьект";
            this.columnHeader1.Width = 297;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Издатель";
            this.columnHeader2.Width = 146;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Дата выдачи";
            this.columnHeader3.Width = 115;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Дата окончания";
            this.columnHeader4.Width = 111;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(576, 319);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(121, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Выбрать";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.DarkViolet;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "Выберите сертификат";
            // 
            // chbShowAllCerts
            // 
            this.chbShowAllCerts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chbShowAllCerts.AutoSize = true;
            this.chbShowAllCerts.Location = new System.Drawing.Point(12, 325);
            this.chbShowAllCerts.Name = "chbShowAllCerts";
            this.chbShowAllCerts.Size = new System.Drawing.Size(181, 17);
            this.chbShowAllCerts.TabIndex = 6;
            this.chbShowAllCerts.Text = "Показывать все сертификаты";
            this.chbShowAllCerts.UseVisualStyleBackColor = true;
            this.chbShowAllCerts.CheckedChanged += new System.EventHandler(this.chbShowAllCerts_CheckedChanged);
            // 
            // chbShowOldCerts
            // 
            this.chbShowOldCerts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chbShowOldCerts.AutoSize = true;
            this.chbShowOldCerts.Location = new System.Drawing.Point(199, 325);
            this.chbShowOldCerts.Name = "chbShowOldCerts";
            this.chbShowOldCerts.Size = new System.Drawing.Size(223, 17);
            this.chbShowOldCerts.TabIndex = 7;
            this.chbShowOldCerts.Text = "Показывать устаревшие сертификаты";
            this.chbShowOldCerts.UseVisualStyleBackColor = true;
            this.chbShowOldCerts.CheckedChanged += new System.EventHandler(this.chbShowOldCerts_CheckedChanged);
            // 
            // frmSelectCertificate
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 354);
            this.Controls.Add(this.chbShowOldCerts);
            this.Controls.Add(this.chbShowAllCerts);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lvCertificates);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSelectCertificate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Выбор сертификата";
            this.Load += new System.EventHandler(this.frmSelectCertificate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvCertificates;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chbShowAllCerts;
        private System.Windows.Forms.CheckBox chbShowOldCerts;
    }
}