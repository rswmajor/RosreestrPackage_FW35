using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace RosreestrPackage
{
    public partial class frmProgress : Form
    {
        Button btnOk = null;
        Label lblInfo = null;

        public frmProgress()
        {
            InitializeComponent();
        }

        public void setTextProgress(string text)
        {
            lblProgress.Text = text;
        }

        public void completeProgress(bool good, string textInfo)
        {
            if (good)
            {
                pbLoad.Image = Properties.Resources.iconCheck;
            }
            else
            {
                pbLoad.Image = Properties.Resources.iconCross;
            }
            
            if (textInfo.Length>0)
            {
                showInfo(textInfo);
            }

            if (btnOk == null)
            {
                btnOk = new Button();
                btnOk.Anchor = AnchorStyles.Top;
                btnOk.Name = "btnOk";
                btnOk.Size = new Size(120, 25);

                int topButtonOk;
                if (lblInfo == null)
                    topButtonOk = lblProgress.Top + lblProgress.Height + 10;
                else
                    topButtonOk = lblInfo.Top + lblInfo.Height + 10;

                btnOk.Location = new Point(this.Width / 2 - btnOk.Width / 2, topButtonOk);
                btnOk.Text = "Ок";
                btnOk.UseVisualStyleBackColor = true;
                btnOk.Click += BtnOk_Click;

                this.Controls.Add(btnOk);
                
                this.Height = btnOk.Top + btnOk.Height + 50;
            }

        }

        private void showInfo(string text)
        {
            if (lblInfo == null)
            {
                lblInfo = new Label();
                lblInfo.AutoSize = true;
                lblInfo.Anchor = AnchorStyles.Top;
                lblInfo.Name = "lblInfo";
                lblInfo.Location = new Point(pbLoad.Left, lblProgress.Top + lblProgress.Height + 10);
                this.Controls.Add(lblInfo);
            }

            lblInfo.Text = text;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void closeForm()
        {
            this.Close();
        }

    }
}
