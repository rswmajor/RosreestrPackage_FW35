using System;
using System.Windows.Forms;

namespace RosreestrPackage
{
    public partial class frmGuid : Form
    {
        public frmGuid()
        {
            InitializeComponent();
        }

        private void frmGuid_Load(object sender, EventArgs e)
        {
            Guid newGuid = Guid.NewGuid();
            tbNewGUID.Text = newGuid.ToString().ToUpper();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(tbNewGUID.Text);
            this.Close();
        }
    }
}
