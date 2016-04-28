using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RosreestrPackage
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private X509Certificate2 mPickedCert = null;
        private frmProgress formProgress = null;

        private const string NAME_SIGNPACK = "SchemaParcels";

        private void frmMain_Load(object sender, EventArgs e)
        {
            lvListFiles.View = View.Details;
            lvListFiles.Columns.Add("Файл", lvListFiles.Width - 10);

            this.Text += " " + Application.ProductVersion;
        }


        private void btnCreatePackage_Click(object sender, EventArgs e)
        {
            if (lvListFiles.Items.Count > 0)
            {
                mPickedCert = null;
                mPickedCert = pickCertificate();
                if (mPickedCert != null)
                {
                    getCertSubjectName(mPickedCert);
                    List<FilePackage> filesToSign = new List<FilePackage>();

                    foreach (ListViewItem item in lvListFiles.Items)
                    {
                        filesToSign.Add((FilePackage)item.Tag);
                    }

                    bool isSignPack;
                    string packageName;
                    var xmlfile = SelectPackageXML(searchXmlCadastre(filesToSign));
                    if (xmlfile != null)
                    {
                        isSignPack = xmlfile.Name.StartsWith(NAME_SIGNPACK);
                        packageName = xmlfile.DirectoryPath + "\\" + Path.GetFileNameWithoutExtension(xmlfile.Name) + RosreestrPackageCreater.PACKAGE_EXT;
                    }
                    else
                    {
                        isSignPack = false;
                        packageName = "";
                    }

                    if (isSignPack)
                    {
                        isSignPack = MessageBox.Show("Подписать создаваемый пакет выбранной подписью?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes;
                    }

                    RosreestrPackageCreater rp = new RosreestrPackageCreater(filesToSign, mPickedCert, chbNotCreatePackage.Checked, chbDeleteFiles.Checked);
                    rp.PackageName = packageName;
                    rp.IsSignPackage = isSignPack;
                    rp.OverWriteSign = chbOverwriteSign.Checked;
                    rp.OnProgress += Rp_OnProgress;
                    rp.createPackageAsync();


                    formProgress = new frmProgress();
                    formProgress.Location = new Point(Location.X + Width / 2, Location.Y + Height / 2);
                    formProgress.ShowDialog();

                }
            }

        }


        private void Rp_OnProgress(object sender, RosreestrPackageCreater.ProgressEventArgs e)
        {

            //throw new NotImplementedException();
            showProgress(e);
        }


        private delegate void delegateShowProgress(RosreestrPackageCreater.ProgressEventArgs args);
        private void showProgress(RosreestrPackageCreater.ProgressEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new delegateShowProgress(showProgress), args);
                return;
            }

            if (formProgress == null)
                return;
            else
                if (!formProgress.Visible)
                formProgress.Visible = true;



            switch (args.Status)
            {
                case RosreestrPackageCreater.ProgressEventArgs.ProgressStatus.BEGIN:
                    break;
                case RosreestrPackageCreater.ProgressEventArgs.ProgressStatus.SIGN_BEGIN:
                    formProgress.setTextProgress("Создание подписей...");
                    break;
                case RosreestrPackageCreater.ProgressEventArgs.ProgressStatus.SIGN_END:
                    break;
                case RosreestrPackageCreater.ProgressEventArgs.ProgressStatus.CREATE_PACKAGE_BEGIN:
                    formProgress.setTextProgress("Создание пакета...");
                    break;
                case RosreestrPackageCreater.ProgressEventArgs.ProgressStatus.CREATE_PACKAGE_SUCCESS:
                    break;
                case RosreestrPackageCreater.ProgressEventArgs.ProgressStatus.CREATE_PACKAGE_FAILED:
                    break;
                case RosreestrPackageCreater.ProgressEventArgs.ProgressStatus.DELETE_FILES_BEGIN:
                    formProgress.setTextProgress("Удаление файлов...");
                    break;
                case RosreestrPackageCreater.ProgressEventArgs.ProgressStatus.DELETE_FILES_END:
                    break;
                case RosreestrPackageCreater.ProgressEventArgs.ProgressStatus.SIGN_PACKAGE:
                    formProgress.setTextProgress("Подпись пакета...");
                    break;
                case RosreestrPackageCreater.ProgressEventArgs.ProgressStatus.COMPLETE:
                    string certSubjectName = getCertSubjectName(mPickedCert);
                    if (certSubjectName.Length > 0)
                        certSubjectName += "\n";

                    formProgress.setTextProgress(certSubjectName +
                                                    "Подписано файлов: " + args.CountSigned.ToString() + "/" + lvListFiles.Items.Count.ToString() + "\n" +
                                                    "Добавлено в пакет: " + args.CountInPackage.ToString());


                    string textError = "";
                    switch (args.Error)
                    {
                        case RosreestrPackageCreater.ProgressEventArgs.ProgressError.SIGN_ERROR:
                            textError = "Ошибка подписи одного или нескольких файлов.";
                            break;
                        case RosreestrPackageCreater.ProgressEventArgs.ProgressError.XMLFILE_NOTFOUND:
                            textError = "XML файл не найден в корневой папке.";
                            break;
                        case RosreestrPackageCreater.ProgressEventArgs.ProgressError.CREATE_PACKAGE_ERROR:
                            textError = "Ошибка при создании архива.";
                            break;
                        default:
                            break;
                    }


                    formProgress.completeProgress(args.Error == RosreestrPackageCreater.ProgressEventArgs.ProgressError.NO_ERROR, textError);
                    break;
                default:
                    break;
            }
        }


        private void lvListFiles_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        private void lvListFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] myfiles = (string[])e.Data.GetData(DataFormats.FileDrop);
            LoadFiles(myfiles);

            //foreach (string myfileName in myfiles)
            //{

            //    FileInfo fileToPacket = new FileInfo(myfileName);
            //    if (fileToPacket.Exists)
            //    {
            //        if (isNewFile(fileToPacket.Name))
            //        {
            //            ListViewItem item = new ListViewItem();
            //            item.Text = fileToPacket.Name;
            //            item.Tag = fileToPacket;
            //            lvListFiles.Items.Add(item);
            //        }
            //    }else if (Directory.Exists(fileToPacket.FullName))
            //    {

            //        DirectoryInfo dir = new DirectoryInfo(fileToPacket.FullName);
            //        FileInfo[] filesdir = dir.GetFiles();
            //        foreach (var filedir in filesdir)
            //        {
            //            ListViewItem item = new ListViewItem();
            //            item.Text =  "/" + dir.Name + "/" + filedir.Name;
            //            item.Tag = fileToPacket;
            //            lvListFiles.Items.Add(item);
            //        }

            //    }

            //}
        }

        private void LoadFiles(string[] files)
        {

            foreach (string filename in files)
            {
                if (File.Exists(filename))
                {
                    AddFileToList(filename, false, "");
                }
                else if (Directory.Exists(filename))
                {
                    var filesdirs = Directory.GetFiles(filename, "*.*", SearchOption.AllDirectories);
                    foreach (string fileIndir in filesdirs)
                    {
                        AddFileToList(fileIndir, true, Directory.GetParent(filename).FullName);
                    }
                }
            }

        }

        private void AddFileToList(string filepath, bool inSubDirectory, string basePath)
        {
            if (isNewFile(filepath))
            {
                if (!filepath.EndsWith(RosreestrPackageCreater.SIGNATURE_EXT))
                {
                    FilePackage file = new FilePackage(filepath, inSubDirectory, basePath);
                    ListViewItem item = new ListViewItem();
                    item.Text = file.RelativeName;
                    item.Tag = file;
                    lvListFiles.Items.Add(item);
                }
            }
        }


        private bool isNewFile(string fileName)
        {
            foreach (ListViewItem item in lvListFiles.Items)
            {
                if (((FilePackage)item.Tag).FullName.Equals(fileName))
                {
                    return false;
                }
            }

            return true;
        }

        private List<FilePackage> searchXmlCadastre(List<FilePackage> files)
        {
            var xmlfiles = new List<FilePackage>();

            string prefixXmlCadastre = "gkuzu|guoks|gkuoks|act|SchemaParcels";

            string pattern = "^(" + prefixXmlCadastre + @")_.+\.xml";

            foreach (FilePackage myfile in files)
            {
                if (!myfile.InSubDirectory)
                {
                    if (Regex.IsMatch(myfile.Name, pattern, RegexOptions.IgnoreCase))
                    {
                        xmlfiles.Add(myfile);
                    }
                }
            }

            return xmlfiles;
        }

        private FilePackage SelectPackageXML(List<FilePackage> filelist)
        {
            if (filelist.Count > 0)
            {
                int index = -1;
                if (filelist.Count == 1)
                {
                    index = 0;
                }
                else
                {
                    frmSelectName frmSel = new frmSelectName(filelist);
                    if (frmSel.ShowDialog() == DialogResult.OK)
                    {
                        index = frmSel.SelectedNameIndex;
                    }
                    else
                    {
                        return null;
                    }
                }

                return filelist[index];
            }

            return null;
        }

        //private bool isNewFile(string filename)
        //{

        //    foreach (ListViewItem item in lvListFiles.Items)
        //    {
        //        if ( item.Text.Equals(filename) )
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        private X509Certificate2 pickCertificate()
        {
            X509Store myStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);

            try
            {
                myStore.Open(OpenFlags.ReadOnly);

                X509Certificate2Collection certsForOpen;

                if (!chbShowAllCerts.Checked)
                {
                    certsForOpen = new X509Certificate2Collection();
                    foreach (X509Certificate2 cert in myStore.Certificates)
                    {
                        if (cert.SignatureAlgorithm.Value.Equals("1.2.643.2.2.3"))
                            certsForOpen.Add(cert);
                    }
                }
                else
                {
                    certsForOpen = myStore.Certificates;
                }

                //X509Certificate2Collection certsSelected = X509Certificate2UI.SelectFromCollection(certsForOpen, "Выбор сертификата",
                //                                                                                    "Выберите сертификат для подписи",
                //                                                                                    X509SelectionFlag.SingleSelection);
                //if (certsSelected.Count > 0)
                //{
                //    return certsSelected[0];
                //}

                var frmcert = new frmSelectCertificate();
                frmcert.CertificateList = certsForOpen;
                frmcert.ShowDialog();
                var certSelected = frmcert.SelectedCertificate;
                frmcert.Dispose();

                return certSelected;

            }
            catch (Exception)
            {

            }

            myStore.Close();

            return null;
        }


        private string getCertSubjectName(X509Certificate2 cert)
        {
            //SN=Рихмайер, G=Сергей Владимирович, STREET="Советская улица, 91", OID.1.2.840.113549.1.9.8=24-13-609, CN=Рихмайер Сергей Владимирович, L="Енисейский район, село Верхнепашино", S=24 Красноярский край, C=RU, E=1243008h@technokad.rosreestr.ru, ИНН=244703398425, СНИЛС=12144807021
            return cert.GetNameInfo(X509NameType.DnsName, false);
        }

        private void btnNewGuid_Click(object sender, EventArgs e)
        {
            frmGuid frm = new frmGuid();
            frm.ShowDialog();
            frm.Dispose();
            frm = null;
        }


        private void ClearListFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvListFiles.Items.Clear();
        }

        private void DeleteItemListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvListFiles.SelectedItems)
            {
                lvListFiles.Items.Remove(item);
            }
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClearListFilesToolStripMenuItem.Enabled = lvListFiles.Items.Count > 0;
            DeleteItemListToolStripMenuItem.Enabled = lvListFiles.SelectedItems.Count > 0;
        }
    }




}
