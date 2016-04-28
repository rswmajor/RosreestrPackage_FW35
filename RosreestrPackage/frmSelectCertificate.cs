using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace RosreestrPackage
{
    public partial class frmSelectCertificate : Form
    {
        public frmSelectCertificate()
        {
            InitializeComponent();
        }

        public X509Certificate2Collection CertificateList
        {
            set
            {
                mCertList = value;
            }
            get
            {
                return mCertList;
            }
        }
        public X509Certificate2 SelectedCertificate
        {
            get
            {
                return mCertSelected;
            }
        }


        private X509Certificate2Collection mCertList;
        private X509Certificate2 mCertSelected;


        private void frmSelectCertificate_Load(object sender, EventArgs e)
        {
            btnOk.Enabled = false;

            FillListCert();
        }

        private void FillListCert()
        {
            foreach (var cert in mCertList)
            {
                var lvitem = new ListViewItem();
                lvitem.Text = cert.GetNameInfo(X509NameType.DnsName, false);
                lvitem.SubItems.Add(cert.GetNameInfo(X509NameType.DnsName, true));
                lvitem.SubItems.Add(cert.NotBefore.ToShortDateString());
                lvitem.SubItems.Add(cert.NotAfter.ToShortDateString());

                if (DateTime.Now > cert.NotAfter)
                {
                    lvitem.ForeColor = Color.Red;
                }

                lvCertificates.Items.Add(lvitem);
            }
        }

        private void lvCertificates_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOk.Enabled = (lvCertificates.SelectedIndices.Count > 0);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            OnCertificateSelected();
        }

        private void lvCertificates_DoubleClick(object sender, EventArgs e)
        {
            OnCertificateSelected();
        }

        private void OnCertificateSelected()
        {
            if (lvCertificates.SelectedIndices.Count > 0)
            {
                mCertSelected = CertificateList[lvCertificates.SelectedIndices[0]];
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                mCertSelected = null;
            }
        }
    }
}
