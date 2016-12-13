using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace RosreestrPackage
{
    class RosreestrPackageCreater
    {

        public RosreestrPackageCreater(List<FilePackage> files, X509Certificate2 cert, bool onlySign = false, bool removeSourceFiles = false)
        {
            FilesToSigned = files;
            CertForSignature = cert;
            this.OnlySign = onlySign;
            this.RemoveSourceFiles = removeSourceFiles;
        }

        public List<FilePackage> FilesToSigned { get; set; }
        public X509Certificate2 CertForSignature { get; set; }
        public bool OnlySign { get; set; }
        public bool RemoveSourceFiles { get; set; }
        public bool OverWriteSign { get; set; } = false;
        public bool IsSignPackage { get; set; } = false;
        public bool NeedUnpack { get; set; } = false;
        public string PackageName { get; set; } = string.Empty;


        public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);
        public event ProgressEventHandler OnProgress;

        public const string SIGNATURE_EXT = ".sig";
        public const string PACKAGE_EXT = ".zip";


        private string tmpDir = "";

        public void createPackageAsync()
        {
            Thread trd = new Thread(createPackage);
            trd.Name = "createPackageAsync";
            trd.IsBackground = true;
            trd.Start();

        }


        public void createPackage()
        {
            List<FilePackage> filesToSign = FilesToSigned;
            X509Certificate2 cert = CertForSignature;
            bool notCreatePackage = OnlySign;
            bool removeFiles = RemoveSourceFiles;
            bool overwritesig = OverWriteSign;
            bool isSignPackage = IsSignPackage;
            bool needUnpack = NeedUnpack;
            string packageName = PackageName;

            ProgressEventArgs.ProgressError currentError = ProgressEventArgs.ProgressError.NO_ERROR;

            if (needUnpack)
            {
                filesToSign = UnzipPackage(packageName);
                if (filesToSign.Count > 0)
                {
                    tmpDir = filesToSign[0].DirectoryPath;
                }
                removeFiles = true;
                notCreatePackage = false;
            }

            if (filesToSign.Count > 0)
            {
                List<FilePackage> filesToPackage = new List<FilePackage>();

                raiseEvent(new ProgressEventArgs(ProgressEventArgs.ProgressStatus.SIGN_BEGIN, 0, 0));

                List<FilePackage> filesNeedToSign = new List<FilePackage>();
                List<FilePackage> existSignatures = new List<FilePackage>();

                filesNeedToSign.AddRange(filesToSign);

                if (!overwritesig)
                {
                    existSignatures = checkExistSignatures(filesNeedToSign);
                }

                List<FilePackage> signaturesFiles = signFiles(filesNeedToSign, cert, overwritesig);

                raiseEvent(new ProgressEventArgs(ProgressEventArgs.ProgressStatus.SIGN_END, signaturesFiles.Count, 0));


                if (!notCreatePackage)
                {

                    if (filesNeedToSign.Count == signaturesFiles.Count)
                    {

                        if (packageName != string.Empty)
                        {

                            bool packageCreated = false;

                            filesToPackage.AddRange(filesToSign);
                            filesToPackage.AddRange(signaturesFiles);
                            filesToPackage.AddRange(existSignatures);

                            raiseEvent(new ProgressEventArgs(ProgressEventArgs.ProgressStatus.CREATE_PACKAGE_BEGIN, signaturesFiles.Count, 0));

                            packageCreated = createZip(filesToPackage, packageName);

                            if (packageCreated)
                            {
                                raiseEvent(new ProgressEventArgs(ProgressEventArgs.ProgressStatus.CREATE_PACKAGE_SUCCESS, signaturesFiles.Count, filesToPackage.Count));

                                if (removeFiles)
                                {
                                    raiseEvent(new ProgressEventArgs(ProgressEventArgs.ProgressStatus.DELETE_FILES_BEGIN, signaturesFiles.Count, filesToPackage.Count));
                                    deleteSourceFiles(filesToPackage, tmpDir);
                                }

                                if (isSignPackage)
                                {
                                    raiseEvent(new ProgressEventArgs(ProgressEventArgs.ProgressStatus.SIGN_PACKAGE, signaturesFiles.Count, filesToPackage.Count));
                                    var list = new List<FilePackage>();
                                    list.Add(new FilePackage(packageName, ""));
                                    signFiles(list, cert, true);
                                }
                            }
                            else
                            {
                                filesToPackage.Clear();
                                raiseEvent(new ProgressEventArgs(ProgressEventArgs.ProgressStatus.CREATE_PACKAGE_FAILED, signaturesFiles.Count, filesToPackage.Count));
                                currentError = ProgressEventArgs.ProgressError.CREATE_PACKAGE_ERROR;
                            }

                        }
                        else { currentError = ProgressEventArgs.ProgressError.XMLFILE_NOTFOUND; }

                    }
                    else { currentError = ProgressEventArgs.ProgressError.SIGN_ERROR; }

                }

                raiseEvent(new ProgressEventArgs(ProgressEventArgs.ProgressStatus.COMPLETE,
                                        signaturesFiles.Count, filesToPackage.Count)
                { Error = currentError });
            }

        }

        private List<FilePackage> UnzipPackage(string zipfileName)
        {
            var filesUnzip = new List<FilePackage>();
            using (ZipFile zip = ZipFile.Read(zipfileName, new ReadOptions() { Encoding = Encoding.GetEncoding(866) }))
            {

                zip.AlternateEncoding = Encoding.UTF8;
                zip.AlternateEncodingUsage = ZipOption.Default;

                string directoryPackage = Directory.GetParent(zipfileName).FullName;
                string targetPath = directoryPackage + "\\tmp" + new Random().Next(1000000).ToString();
                zip.ExtractAll(targetPath);

                var files = zip.EntryFileNames;
                foreach (var myfile in files)
                {
                    string relativeName = myfile.Replace("/", "\\");
                    bool inSubDir = relativeName.IndexOf("\\") > -1;
                    string fullName = Path.Combine(targetPath, relativeName);
                    string basePath = inSubDir ? targetPath : string.Empty;

                    if (!fullName.EndsWith(SIGNATURE_EXT))
                    {
                        FilePackage file = new FilePackage(fullName, basePath);
                        file.IsSigned = File.Exists(myfile + SIGNATURE_EXT);
                        filesUnzip.Add(file);
                    }

                }
            }
            return filesUnzip;
        }

        private List<FilePackage> checkExistSignatures(List<FilePackage> files)
        {
            List<FilePackage> existSignatures = new List<FilePackage>();
            List<FilePackage> tmplist = new List<FilePackage>();
            tmplist.AddRange(files);
            foreach (var myfile in tmplist)
            {
                string signatureFileName = myfile.FullName + SIGNATURE_EXT;

                if (File.Exists(signatureFileName))
                {
                    existSignatures.Add(new FilePackage(signatureFileName, myfile.BasePath));
                    files.Remove(myfile);
                }
            }

            return existSignatures;
        }

        private List<FilePackage> signFiles(List<FilePackage> files, X509Certificate2 cert, bool overwritesig)
        {
            List<FilePackage> signedFiles = new List<FilePackage>();


            foreach (FilePackage myfile in files)
            {
                try
                {


                    //if (!overwritesig)
                    //{
                    //    if (fileSig.Exists)
                    //    {
                    //        signedFiles.Add(fileSig);
                    //        continue;
                    //    }
                    //}

                    byte[] bytesToSign = File.ReadAllBytes(myfile.FullName);
                    byte[] signedBytes = signBytes(bytesToSign, cert);

                    if (signedBytes != null)
                    {
                        FilePackage fileSig = new FilePackage(myfile.FullName + SIGNATURE_EXT, myfile.BasePath);

                        File.WriteAllBytes(fileSig.FullName, signedBytes);

                        myfile.IsSigned = true;

                        signedFiles.Add(fileSig);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            return signedFiles;
        }


        private bool createZip(List<FilePackage> files, string zipName)
        {

            try
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncoding = System.Text.Encoding.UTF8;
                    zip.AlternateEncodingUsage = ZipOption.Always;
                    zip.CompressionLevel = Ionic.Zlib.CompressionLevel.Default;

                    foreach (FilePackage myfile in files)
                    {
                        Debug.WriteLine("myfile.FullName: " + myfile.FullName);
                        zip.AddFile(myfile.FullName, myfile.RelativePath);

                    }

                    zip.Save(zipName);

                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }


        private byte[] signBytes(byte[] bytesToSign, X509Certificate2 cert)
        {
            byte[] signedBytes;

            try
            {
                ContentInfo content = new ContentInfo(bytesToSign);
                SignedCms signCms = new SignedCms(content, true);
                CmsSigner signer = new CmsSigner(SubjectIdentifierType.IssuerAndSerialNumber, cert);
                signCms.ComputeSignature(signer, false); //если silent = true -> неотображается окно ввода пароля контейнера
                signedBytes = signCms.Encode();

                return signedBytes;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        private FilePackage searchXmlCadastre(List<FilePackage> files)
        {
            string prefixXmlCadastre = "gkuzu|guoks|gkuoks|act|SchemaParcels";

            string pattern = "^(" + prefixXmlCadastre + @")_.+\.xml";

            foreach (FilePackage myfile in files)
            {
                if (!myfile.InSubDirectory)
                {
                    if (Regex.IsMatch(myfile.Name, pattern, RegexOptions.IgnoreCase))
                    {
                        return myfile;
                    }
                }
            }

            return null;
        }


        private void deleteSourceFiles(List<FilePackage> files, string tmpDir)
        {
            List<string> dirs = new List<string>();
            foreach (FilePackage myfile in files)
            {
                try
                {
                    File.Delete(myfile.FullName);

                    if (!dirs.Contains(myfile.DirectoryPath))
                    {
                        dirs.Add(myfile.DirectoryPath);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            if (tmpDir.Length > 0)
            {
                dirs.Add(tmpDir);
            }

            foreach (var dir in dirs)
            {
                try
                {
                    Directory.Delete(dir);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message + "\n" + dir);
                }
            }
        }


        private void raiseEvent(ProgressEventArgs args)
        {
            if (OnProgress != null)
            {
                OnProgress(this, args);
            }
        }


        public class ProgressEventArgs
        {

            public ProgressEventArgs(ProgressStatus status, int countSigned, int countInPackage)
            {
                this.Status = status;
                this.CountSigned = countSigned;
                this.CountInPackage = countInPackage;
            }


            public enum ProgressStatus
            {
                BEGIN,
                SIGN_BEGIN,
                SIGN_END,
                CREATE_PACKAGE_BEGIN,
                CREATE_PACKAGE_SUCCESS,
                CREATE_PACKAGE_FAILED,
                DELETE_FILES_BEGIN,
                DELETE_FILES_END,
                SIGN_PACKAGE,
                COMPLETE
            }

            public enum ProgressError
            {
                NO_ERROR,
                SIGN_ERROR,
                XMLFILE_NOTFOUND,
                CREATE_PACKAGE_ERROR
            }

            public ProgressStatus Status { get; set; }
            public int CountSigned { get; set; } = 0;
            public int CountInPackage { get; set; } = 0;
            public ProgressError Error { get; set; } = ProgressError.NO_ERROR;
            public string ErrorDetails { get; set; } = string.Empty; 

        }
    }
}
