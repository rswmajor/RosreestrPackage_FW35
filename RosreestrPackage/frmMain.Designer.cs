﻿namespace RosreestrPackage
{
    partial class frmMain
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.lvListFiles = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ClearListFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteItemListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCreatePackage = new System.Windows.Forms.Button();
            this.chbDeleteFiles = new System.Windows.Forms.CheckBox();
            this.btnNewGuid = new System.Windows.Forms.Button();
            this.chbShowAllCerts = new System.Windows.Forms.CheckBox();
            this.chbNotCreatePackage = new System.Windows.Forms.CheckBox();
            this.chbOverwriteSign = new System.Windows.Forms.CheckBox();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvListFiles
            // 
            this.lvListFiles.AllowDrop = true;
            this.lvListFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvListFiles.ContextMenuStrip = this.contextMenuStrip1;
            this.lvListFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lvListFiles.FullRowSelect = true;
            this.lvListFiles.Location = new System.Drawing.Point(12, 12);
            this.lvListFiles.Name = "lvListFiles";
            this.lvListFiles.Size = new System.Drawing.Size(660, 311);
            this.lvListFiles.TabIndex = 0;
            this.lvListFiles.UseCompatibleStateImageBehavior = false;
            this.lvListFiles.View = System.Windows.Forms.View.Details;
            this.lvListFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvListFiles_DragDrop);
            this.lvListFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvListFiles_DragEnter);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ClearListFilesToolStripMenuItem,
            this.DeleteItemListToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(169, 48);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // ClearListFilesToolStripMenuItem
            // 
            this.ClearListFilesToolStripMenuItem.Name = "ClearListFilesToolStripMenuItem";
            this.ClearListFilesToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.ClearListFilesToolStripMenuItem.Text = "Очистить список";
            this.ClearListFilesToolStripMenuItem.Click += new System.EventHandler(this.ClearListFilesToolStripMenuItem_Click);
            // 
            // DeleteItemListToolStripMenuItem
            // 
            this.DeleteItemListToolStripMenuItem.Name = "DeleteItemListToolStripMenuItem";
            this.DeleteItemListToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.DeleteItemListToolStripMenuItem.Text = "Удалить";
            this.DeleteItemListToolStripMenuItem.Click += new System.EventHandler(this.DeleteItemListToolStripMenuItem_Click);
            // 
            // btnCreatePackage
            // 
            this.btnCreatePackage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreatePackage.Location = new System.Drawing.Point(523, 346);
            this.btnCreatePackage.Name = "btnCreatePackage";
            this.btnCreatePackage.Size = new System.Drawing.Size(149, 23);
            this.btnCreatePackage.TabIndex = 1;
            this.btnCreatePackage.Text = "Подписать и упаковать";
            this.btnCreatePackage.UseVisualStyleBackColor = true;
            this.btnCreatePackage.Click += new System.EventHandler(this.btnCreatePackage_Click);
            // 
            // chbDeleteFiles
            // 
            this.chbDeleteFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chbDeleteFiles.AutoSize = true;
            this.chbDeleteFiles.Checked = true;
            this.chbDeleteFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbDeleteFiles.Location = new System.Drawing.Point(12, 329);
            this.chbDeleteFiles.Name = "chbDeleteFiles";
            this.chbDeleteFiles.Size = new System.Drawing.Size(189, 17);
            this.chbDeleteFiles.TabIndex = 2;
            this.chbDeleteFiles.Text = "Удалить файлы после упаковки";
            this.chbDeleteFiles.UseVisualStyleBackColor = true;
            // 
            // btnNewGuid
            // 
            this.btnNewGuid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNewGuid.Location = new System.Drawing.Point(397, 346);
            this.btnNewGuid.Name = "btnNewGuid";
            this.btnNewGuid.Size = new System.Drawing.Size(120, 23);
            this.btnNewGuid.TabIndex = 4;
            this.btnNewGuid.Text = "Новый GUID";
            this.btnNewGuid.UseVisualStyleBackColor = true;
            this.btnNewGuid.Click += new System.EventHandler(this.btnNewGuid_Click);
            // 
            // chbShowAllCerts
            // 
            this.chbShowAllCerts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chbShowAllCerts.AutoSize = true;
            this.chbShowAllCerts.Location = new System.Drawing.Point(12, 352);
            this.chbShowAllCerts.Name = "chbShowAllCerts";
            this.chbShowAllCerts.Size = new System.Drawing.Size(181, 17);
            this.chbShowAllCerts.TabIndex = 5;
            this.chbShowAllCerts.Text = "Показывать все сертификаты";
            this.chbShowAllCerts.UseVisualStyleBackColor = true;
            // 
            // chbNotCreatePackage
            // 
            this.chbNotCreatePackage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chbNotCreatePackage.AutoSize = true;
            this.chbNotCreatePackage.Location = new System.Drawing.Point(207, 329);
            this.chbNotCreatePackage.Name = "chbNotCreatePackage";
            this.chbNotCreatePackage.Size = new System.Drawing.Size(128, 17);
            this.chbNotCreatePackage.TabIndex = 6;
            this.chbNotCreatePackage.Text = "Не создавать архив";
            this.chbNotCreatePackage.UseVisualStyleBackColor = true;
            // 
            // chbOverwriteSign
            // 
            this.chbOverwriteSign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chbOverwriteSign.AutoSize = true;
            this.chbOverwriteSign.Location = new System.Drawing.Point(207, 352);
            this.chbOverwriteSign.Name = "chbOverwriteSign";
            this.chbOverwriteSign.Size = new System.Drawing.Size(119, 17);
            this.chbOverwriteSign.TabIndex = 7;
            this.chbOverwriteSign.Text = "Переподписывать";
            this.chbOverwriteSign.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 381);
            this.Controls.Add(this.chbOverwriteSign);
            this.Controls.Add(this.chbNotCreatePackage);
            this.Controls.Add(this.chbShowAllCerts);
            this.Controls.Add(this.btnNewGuid);
            this.Controls.Add(this.chbDeleteFiles);
            this.Controls.Add(this.btnCreatePackage);
            this.Controls.Add(this.lvListFiles);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(640, 200);
            this.Name = "frmMain";
            this.Text = "Пакет для Росреестра";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvListFiles;
        private System.Windows.Forms.Button btnCreatePackage;
        private System.Windows.Forms.CheckBox chbDeleteFiles;
        private System.Windows.Forms.Button btnNewGuid;
        private System.Windows.Forms.CheckBox chbShowAllCerts;
        private System.Windows.Forms.CheckBox chbNotCreatePackage;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ClearListFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteItemListToolStripMenuItem;
        private System.Windows.Forms.CheckBox chbOverwriteSign;
    }
}

