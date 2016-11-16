using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Threading;


namespace RosreestrPackage
{
    public class SignatureChecker
    {

        public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);
        public event ProgressEventHandler OnProgress;

        public SignatureChecker(List<FilePackage> files)
        {
            FilesToCheck = files;
        }

        public void VerifyAsyncThread()
        {
            var trd = new Thread(VerifyWork);
            trd.Name = "VerifySignatureThread";
            trd.IsBackground = true;
            trd.Start();
        }

        private void VerifyWork()
        {
            RaiseEvent(ProgressEventArgs.ProgressStatus.BEGIN, 0, FilesToCheck.Count, false, null);
            bool success = false;
            foreach (var myfile in FilesToCheck)
            {
                success = Verify(myfile.FullName, myfile.FullName + RosreestrPackageCreater.SIGNATURE_EXT);

            }
            RaiseEvent(ProgressEventArgs.ProgressStatus.COMPLETE, FilesToCheck.Count, FilesToCheck.Count, success, null);
        }

        private void RaiseEvent(ProgressEventArgs.ProgressStatus status, int number, int count, bool success, FilePackage file)
        {
            if (OnProgress != null)
            {
                OnProgress(this, new ProgressEventArgs(status, number, count, success, file));
            }
        }

        public static bool Verify(string filename, string filesig)
        {
            try
            {
                byte[] msg = File.ReadAllBytes(filename);
                byte[] sig = File.ReadAllBytes(filesig);

                var content = new ContentInfo(msg);
                var cms = new SignedCms(content, true);
                cms.Decode(sig);
            
                cms.CheckSignature(true);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public static X509Certificate2Collection GetCertificates(string filesig)
        {
            try
            {
                byte[] sig = File.ReadAllBytes(filesig);
                var cms = new SignedCms();
                cms.Decode(sig);
                return cms.Certificates;
            }
            catch (Exception)
            {
                return null;
            }

        }


        public List<FilePackage> FilesToCheck { get; set; }


        public class ProgressEventArgs
        {
            public ProgressStatus Status;
            public int Count;
            public int Number;
            public bool Success;
            public FilePackage CheckedFile;


            public ProgressEventArgs(ProgressStatus status, int number, int count, bool success, FilePackage file)
            {
                Status = status;
                Number = number;
                Count = count;
                Success = success;
                CheckedFile = file;
            }

            public enum ProgressStatus
            {
                BEGIN,
                COMPLETE
            }
        }
    }

}
