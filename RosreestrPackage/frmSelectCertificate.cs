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

        public X509Certificate2 SelectedCertificate
        {
            get
            {
                return mCertSelected;
            }
        }

        private X509Certificate2 mCertSelected;
        private bool isSettingsLoad = false;

        private void frmSelectCertificate_Load(object sender, EventArgs e)
        {
            btnOk.Enabled = false;

            LoadSettings();

            FillListCert();
        }

        private void LoadSettings()
        {
            isSettingsLoad = true;

            chbShowAllCerts.Checked = Properties.Settings.Default.ShowAllCerts;
            chbShowOldCerts.Checked = Properties.Settings.Default.ShowOldCerts;

            isSettingsLoad = false;
        }

        private void FillListCert()
        {
            lvCertificates.Items.Clear();

            X509Store myStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);

            try
            {
                myStore.Open(OpenFlags.ReadOnly);

                foreach (X509Certificate2 cert in myStore.Certificates)
                {
                    if (!chbShowAllCerts.Checked && !cert.SignatureAlgorithm.Value.Equals("1.2.643.2.2.3"))
                    {
                        continue;
                    }

                    bool isOldCert = DateTime.Now > cert.NotAfter;

                    if (!chbShowOldCerts.Checked && isOldCert)
                    {
                        continue;
                    }
                    else
                    {
                        var lvitem = new ListViewItem();
                        lvitem.Text = cert.GetNameInfo(X509NameType.DnsName, false);
                        lvitem.SubItems.Add(cert.GetNameInfo(X509NameType.DnsName, true));
                        lvitem.SubItems.Add(cert.NotBefore.ToShortDateString());
                        lvitem.SubItems.Add(cert.NotAfter.ToShortDateString());
                        lvitem.Tag = cert;

                        if (isOldCert)
                        {
                            lvitem.ForeColor = Color.Red;
                        }

                        lvCertificates.Items.Add(lvitem);
                    }
                }
            }
            catch (Exception){}
            finally
            {
                myStore.Close();
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
                mCertSelected = (X509Certificate2)lvCertificates.SelectedItems[0].Tag;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                mCertSelected = null;
            }
        }

        private void chbShowAllCerts_CheckedChanged(object sender, EventArgs e)
        {
            if (!isSettingsLoad)
            {
                FillListCert();

                Properties.Settings.Default.ShowAllCerts = chbShowAllCerts.Checked;
                Properties.Settings.Default.Save();
            }
        }

        private void chbShowOldCerts_CheckedChanged(object sender, EventArgs e)
        {
            if (!isSettingsLoad)
            {
                FillListCert();

                Properties.Settings.Default.ShowOldCerts = chbShowOldCerts.Checked;
                Properties.Settings.Default.Save();
            }
        }
    }
}
