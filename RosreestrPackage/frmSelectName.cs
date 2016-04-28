using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace RosreestrPackage
{
    public partial class frmSelectName : Form
    {
        public frmSelectName(List<FilePackage> fileNameList)
        {
            InitializeComponent();
            this.fileNameList = fileNameList;
        }

        public int SelectedNameIndex { get; set; } = -1;

        private List<FilePackage> fileNameList = null;

        private void frmSelectName_Load(object sender, EventArgs e)
        {
            btnSelect.Enabled = false;
            FillList();
        }

        private void FillList()
        {
            if (fileNameList != null)
            {
                foreach (var item in fileNameList)
                {
                    lbFiles.Items.Add(System.IO.Path.GetFileNameWithoutExtension(item.FullName));
                }
            }
        }

        private void SelectName()
        {
            if (lbFiles.SelectedIndex >= 0)
            {
                SelectedNameIndex = lbFiles.SelectedIndex;
                Close();
            }
        }

        private void lbFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DialogResult = DialogResult.OK;
            SelectName();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            SelectName();
        }

        private void lbFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSelect.Enabled = lbFiles.SelectedIndex >= 0;
        }
    }
}
