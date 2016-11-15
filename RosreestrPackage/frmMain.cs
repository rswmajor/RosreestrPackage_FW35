using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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

        private bool isSettingsLoad = false;

        private const string NAME_SIGNPACK = "SchemaParcels";

        private void frmMain_Load(object sender, EventArgs e)
        {
            InitUI();
            LoadSettings();
        }

        private void InitUI()
        {
            lvListFiles.View = View.Details;
            lvListFiles.Columns.Add("Файл", lvListFiles.Width - 10);
            lvListFiles.SmallImageList = new ImageList();
            lvListFiles.SmallImageList.Images.Add(Properties.Resources.iconPen);

            this.Text += " " + Application.ProductVersion;

            //chbNotCreatePackage_CheckedChanged(null, null);

            openFileDialog1.Filter = "Все файлы|*.*";
            openFileDialog1.FileName = "";

        }

        private void LoadSettings()
        {
            isSettingsLoad = true;

            chbDeleteFiles.Checked = Properties.Settings.Default.DeleteSourceFiles;
            chbNotCreatePackage.Checked = Properties.Settings.Default.NotCreatePackage;
            chbOverwriteSign.Checked = Properties.Settings.Default.OverwriteSign;

            isSettingsLoad = false;
        }


        private void btnCreatePackage_Click(object sender, EventArgs e)
        {
            if (lvListFiles.Items.Count > 0)
            {

                mPickedCert = pickCertificate();
                if (mPickedCert != null)
                {

                    //Unpack zip if count files 1
                    if (lvListFiles.Items.Count == 1)
                    {
                        var file = (FilePackage)lvListFiles.Items[0].Tag;
                        if (IsCadastrePackage(file))
                        {
                            string filesig = file.FullName + RosreestrPackageCreater.SIGNATURE_EXT;
                            bool sigExists = File.Exists(filesig);
                            string comment = sigExists ? "\n\nПодпись архива будет удалена, т.к. перестанет быть корректной." : "";

                            if (MessageBox.Show("Подписать файлы внутри архива?" + comment, "Подтверждение",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                            {
                                //If the signature is there, it will be incorrect
                                if (sigExists)
                                {
                                    File.Delete(filesig);
                                }

                                lvListFiles.Items[0].Remove();
                                UnpackPackageAndAddFiles(file);
                            }
                        }
                    }


                    List<FilePackage> filesToSign = new List<FilePackage>();

                    foreach (ListViewItem item in lvListFiles.Items)
                    {
                        filesToSign.Add((FilePackage)item.Tag);
                    }

                    //sign package if shema parcels
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
                        isSignPack = MessageBox.Show("Подписать создаваемый пакет выбранной подписью?", "Подтверждение",
                                                    MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes;
                    }

                    //
                    var rp = new RosreestrPackageCreater(filesToSign, mPickedCert, chbNotCreatePackage.Checked, chbDeleteFiles.Checked);
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

        private void btnAddFiles_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                LoadFiles(openFileDialog1.FileNames);
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
                    string textComplete = string.Format("Подписано файлов: {0}/{1}", args.CountSigned, lvListFiles.Items.Count);

                    string certSubjectName = getCertSubjectName(mPickedCert);
                    if (certSubjectName.Length > 0)
                        formProgress.setTextProgressTop(certSubjectName);
                        //textComplete = certSubjectName + "\n" + textComplete;

                    if (!chbNotCreatePackage.Checked)
                    {
                        textComplete += string.Format("\nДобавлено в пакет: {0}", args.CountInPackage);
                    }

                    formProgress.setTextProgress(textComplete);


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
                    AddFileToList(filename, "");
                }
                else if (Directory.Exists(filename))
                {
                    var filesdirs = Directory.GetFiles(filename, "*.*", SearchOption.AllDirectories);
                    foreach (string fileIndir in filesdirs)
                    {
                        AddFileToList(fileIndir, Directory.GetParent(filename).FullName);
                    }
                }
            }

        }

        private void AddFileToList(string filepath, string basePath)
        {
            if (isNewFile(filepath))
            {
                if (!filepath.EndsWith(RosreestrPackageCreater.SIGNATURE_EXT))
                {
                    FilePackage file = new FilePackage(filepath, basePath);
                    file.IsSigned = IsSignExist(filepath);

                    ListViewItem item = new ListViewItem();
                    item.Text = file.RelativeName;
                    item.Tag = file;

                    if (file.IsSigned)
                    {
                        item.ImageIndex = 0;
                    }
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

        private void UnpackPackageAndAddFiles(FilePackage filePackage)
        {
            using (ZipFile zip = ZipFile.Read(filePackage.FullName, new ReadOptions() { Encoding = Encoding.GetEncoding(866) }))
            {

                zip.AlternateEncoding = Encoding.UTF8;
                zip.AlternateEncodingUsage = ZipOption.Default;

                zip.ExtractAll(filePackage.DirectoryPath);

                var files = zip.EntryFileNames;
                foreach (var myfile in files)
                {
                    string relativeName = myfile.Replace("/", "\\");
                    bool inSubDir = relativeName.IndexOf("\\") > -1;
                    string fullName = Path.Combine(filePackage.DirectoryPath, relativeName);
                    string basePath = inSubDir ? filePackage.DirectoryPath : string.Empty;
                    AddFileToList(fullName, basePath);

                }
            }
        }

        private List<FilePackage> searchXmlCadastre(List<FilePackage> files)
        {
            var xmlfiles = new List<FilePackage>();

            string prefixXmlCadastre = "gkuzu|guoks|gkuoks|act|SchemaParcels|req";

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

        private bool IsCadastrePackage(FilePackage file)
        {
            string prefixXmlCadastre = "gkuzu|guoks|gkuoks|act|SchemaParcels|req";

            string pattern = "^(" + prefixXmlCadastre + @")_.+\.zip";

            return (Regex.IsMatch(file.Name, pattern, RegexOptions.IgnoreCase));
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

        private bool IsSignExist(string filename)
        {
            string signatureFileName = filename + RosreestrPackageCreater.SIGNATURE_EXT;

            return File.Exists(signatureFileName);
        }

        private X509Certificate2 pickCertificate()
        {

            ////X509Certificate2Collection certsSelected = X509Certificate2UI.SelectFromCollection(certsForOpen, "Выбор сертификата",
            //                                                                                    "Выберите сертификат для подписи", 
            //                                                                                    X509SelectionFlag.SingleSelection);
            var frmcert = new frmSelectCertificate();
            frmcert.ShowDialog();
            var certSelected = frmcert.SelectedCertificate;
            frmcert.Dispose();

            return certSelected;
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

        private void CheckSignature(List<FilePackage> files)
        {
            var checker = new SignatureChecker(files);
            checker.OnProgress += Checker_OnProgress;
            checker.VerifyAsyncThread();

            formProgress = new frmProgress();
            formProgress.Location = new Point(Location.X + Width / 2, Location.Y + Height / 2);
            formProgress.ShowDialog();
        }

        private void Checker_OnProgress(object sender, SignatureChecker.ProgressEventArgs e)
        {
            showProgressCheck(e);
        }

        private delegate void delegateShowProgressCheck(SignatureChecker.ProgressEventArgs args);
        private void showProgressCheck(SignatureChecker.ProgressEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new delegateShowProgressCheck(showProgressCheck), args);
                return;
            }

            if (formProgress == null)
                return;
            else
                if (!formProgress.Visible)
                formProgress.Visible = true;

            switch (args.Status)
            {
                case SignatureChecker.ProgressEventArgs.ProgressStatus.BEGIN:
                    formProgress.setTextProgress("Проверка...");
                    break;
                case SignatureChecker.ProgressEventArgs.ProgressStatus.COMPLETE:
                    if (args.Success)
                    {
                        formProgress.setTextProgress("Успешно. Подпись корректна.");
                    }
                    else
                    {
                        formProgress.setTextProgress("Неудача. Подпись не корректна.");
                    }
                    formProgress.completeProgress(args.Success, "");
                    break;
            }
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

        private void CheckSignToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var files = new List<FilePackage>();
            foreach (ListViewItem lvitem in lvListFiles.SelectedItems)
            {
                files.Add((FilePackage)lvitem.Tag);
            }
            CheckSignature(files);
        }

        private void ViewCertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var myfile = (FilePackage)lvListFiles.SelectedItems[0].Tag;
            string filesig = myfile.FullName + RosreestrPackageCreater.SIGNATURE_EXT;
            var certsForOpen = SignatureChecker.GetCertificates(filesig);
            if (certsForOpen != null)
            {
                X509Certificate2UI.SelectFromCollection(certsForOpen, "Список сертификатов подписи", "", X509SelectionFlag.SingleSelection);
            }

        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClearListFilesToolStripMenuItem.Enabled = lvListFiles.Items.Count > 0;
            DeleteItemListToolStripMenuItem.Enabled = lvListFiles.SelectedItems.Count > 0;

            if (lvListFiles.SelectedItems.Count == 1)
            {
                var myfile = (FilePackage)lvListFiles.SelectedItems[0].Tag;
                CheckSignToolStripMenuItem.Visible = myfile.IsSigned;
                ViewCertToolStripMenuItem.Visible = myfile.IsSigned;
            }
            else
            {
                CheckSignToolStripMenuItem.Visible = false;
                ViewCertToolStripMenuItem.Visible = false;
            }

        }

        private void chbNotCreatePackage_CheckedChanged(object sender, EventArgs e)
        {
            btnCreatePackage.Text = chbNotCreatePackage.Checked ? "Подписать" : "Подписать и упаковать";

            if (!isSettingsLoad)
            {
                Properties.Settings.Default.NotCreatePackage = chbNotCreatePackage.Checked;
                Properties.Settings.Default.Save();
            }
        }

        private void chbDeleteFiles_CheckedChanged(object sender, EventArgs e)
        {
            if (!isSettingsLoad)
            {
                Properties.Settings.Default.DeleteSourceFiles = chbDeleteFiles.Checked;
                Properties.Settings.Default.Save();
            }
        }

        private void chbOverwriteSign_CheckedChanged(object sender, EventArgs e)
        {
            if (!isSettingsLoad)
            {
                Properties.Settings.Default.OverwriteSign = chbOverwriteSign.Checked;
                Properties.Settings.Default.Save();
            }
        }


    }




}
