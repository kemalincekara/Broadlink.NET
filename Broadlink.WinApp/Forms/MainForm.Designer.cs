namespace Broadlink.WinApp.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnLogClean = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmbDevices = new System.Windows.Forms.ToolStripComboBox();
            this.btnConnect = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnMenuOgren = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnIR_Learn = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRF_Learn = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLearnCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnKomutGonder = new System.Windows.Forms.ToolStripButton();
            this.cmbKomutListe = new System.Windows.Forms.ToolStripComboBox();
            this.txtIRCount = new System.Windows.Forms.ToolStripTextBox();
            this.btnKomutlariKaydet = new System.Windows.Forms.ToolStripButton();
            this.timerSicaklik = new System.Windows.Forms.Timer(this.components);
            this.txtLog = new BetterRichTextBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLogClean,
            this.toolStripSeparator2,
            this.cmbDevices,
            this.btnConnect,
            this.toolStripSeparator3,
            this.btnMenuOgren,
            this.toolStripSeparator1,
            this.btnKomutGonder,
            this.cmbKomutListe,
            this.txtIRCount,
            this.btnKomutlariKaydet});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(703, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnLogClean
            // 
            this.btnLogClean.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnLogClean.Image = ((System.Drawing.Image)(resources.GetObject("btnLogClean.Image")));
            this.btnLogClean.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLogClean.Name = "btnLogClean";
            this.btnLogClean.Size = new System.Drawing.Size(66, 22);
            this.btnLogClean.Text = "Log Temizle";
            this.btnLogClean.Click += new System.EventHandler(this.btnLogClean_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // cmbDevices
            // 
            this.cmbDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDevices.Name = "cmbDevices";
            this.cmbDevices.Size = new System.Drawing.Size(121, 25);
            // 
            // btnConnect
            // 
            this.btnConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnConnect.Image = ((System.Drawing.Image)(resources.GetObject("btnConnect.Image")));
            this.btnConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(72, 22);
            this.btnConnect.Text = "Tarama Yap";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnMenuOgren
            // 
            this.btnMenuOgren.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnMenuOgren.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnIR_Learn,
            this.btnRF_Learn,
            this.btnLearnCancel});
            this.btnMenuOgren.Enabled = false;
            this.btnMenuOgren.Image = ((System.Drawing.Image)(resources.GetObject("btnMenuOgren.Image")));
            this.btnMenuOgren.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMenuOgren.Name = "btnMenuOgren";
            this.btnMenuOgren.Size = new System.Drawing.Size(117, 22);
            this.btnMenuOgren.Text = "Yeni Komut Öğren";
            // 
            // btnIR_Learn
            // 
            this.btnIR_Learn.Name = "btnIR_Learn";
            this.btnIR_Learn.Size = new System.Drawing.Size(185, 22);
            this.btnIR_Learn.Text = "Kızılötesi Öğren";
            this.btnIR_Learn.Click += new System.EventHandler(this.btnIR_Learn_Click);
            // 
            // btnRF_Learn
            // 
            this.btnRF_Learn.Name = "btnRF_Learn";
            this.btnRF_Learn.Size = new System.Drawing.Size(185, 22);
            this.btnRF_Learn.Text = "RF Frekans Öğren";
            this.btnRF_Learn.Click += new System.EventHandler(this.btnRF_Learn_Click);
            // 
            // btnLearnCancel
            // 
            this.btnLearnCancel.Name = "btnLearnCancel";
            this.btnLearnCancel.Size = new System.Drawing.Size(185, 22);
            this.btnLearnCancel.Text = "Öğrenme Modu İptal";
            this.btnLearnCancel.Click += new System.EventHandler(this.btnLearnCancel_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnKomutGonder
            // 
            this.btnKomutGonder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnKomutGonder.Enabled = false;
            this.btnKomutGonder.Image = ((System.Drawing.Image)(resources.GetObject("btnKomutGonder.Image")));
            this.btnKomutGonder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnKomutGonder.Name = "btnKomutGonder";
            this.btnKomutGonder.Size = new System.Drawing.Size(89, 22);
            this.btnKomutGonder.Text = "Komut Gönder";
            this.btnKomutGonder.Click += new System.EventHandler(this.btnKomutGonder_Click);
            // 
            // cmbKomutListe
            // 
            this.cmbKomutListe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKomutListe.Enabled = false;
            this.cmbKomutListe.Name = "cmbKomutListe";
            this.cmbKomutListe.Size = new System.Drawing.Size(121, 25);
            // 
            // txtIRCount
            // 
            this.txtIRCount.Name = "txtIRCount";
            this.txtIRCount.Size = new System.Drawing.Size(20, 25);
            this.txtIRCount.Text = "1";
            // 
            // btnKomutlariKaydet
            // 
            this.btnKomutlariKaydet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnKomutlariKaydet.Enabled = false;
            this.btnKomutlariKaydet.Image = ((System.Drawing.Image)(resources.GetObject("btnKomutlariKaydet.Image")));
            this.btnKomutlariKaydet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnKomutlariKaydet.Name = "btnKomutlariKaydet";
            this.btnKomutlariKaydet.Size = new System.Drawing.Size(47, 22);
            this.btnKomutlariKaydet.Text = "Kaydet";
            this.btnKomutlariKaydet.Click += new System.EventHandler(this.btnKomutlariKaydet_Click);
            // 
            // timerSicaklik
            // 
            this.timerSicaklik.Interval = 10000;
            this.timerSicaklik.Tick += new System.EventHandler(this.timerSicaklik_Tick);
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.Black;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtLog.ForeColor = System.Drawing.Color.White;
            this.txtLog.Location = new System.Drawing.Point(0, 25);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(703, 359);
            this.txtLog.TabIndex = 2;
            this.txtLog.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 384);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "Broadlink .NET | Oda sıcaklığı : {0}°C";
            this.Text = "Broadlink .NET | Developed By K3M41";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnConnect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnKomutGonder;
        private System.Windows.Forms.ToolStripComboBox cmbKomutListe;
        private BetterRichTextBox txtLog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnLogClean;
        private System.Windows.Forms.Timer timerSicaklik;
        private System.Windows.Forms.ToolStripComboBox cmbDevices;
        private System.Windows.Forms.ToolStripButton btnKomutlariKaydet;
        private System.Windows.Forms.ToolStripDropDownButton btnMenuOgren;
        private System.Windows.Forms.ToolStripMenuItem btnIR_Learn;
        private System.Windows.Forms.ToolStripMenuItem btnRF_Learn;
        private System.Windows.Forms.ToolStripMenuItem btnLearnCancel;
        private System.Windows.Forms.ToolStripTextBox txtIRCount;
    }
}