namespace KitchenCalculator
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;

    public class appAboutDialog : Form
    {
        private IContainer components;
        private Label CopyrightLabel;
        private Button OKButton;
        private LinkLabel ProductLinkLabel;
        private LinkLabel RadiumLinkLabel;
        private Label TitleLabel;

        public appAboutDialog()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void HandleSystemError(Exception aoException)
        {
            MessageBox.Show(aoException.Message);
        }

        private void InitializeComponent()
        {
            this.OKButton = new Button();
            this.TitleLabel = new Label();
            this.CopyrightLabel = new Label();
            this.ProductLinkLabel = new LinkLabel();
            this.RadiumLinkLabel = new LinkLabel();
            base.SuspendLayout();
            this.OKButton.DialogResult = DialogResult.OK;
            this.OKButton.Location = new Point(0xc0, 0x63);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new Size(0x4b, 0x17);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new EventHandler(this.OKButton_Click);
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.TitleLabel.Location = new Point(0x19, 0x18);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new Size(0x97, 13);
            this.TitleLabel.TabIndex = 1;
            this.TitleLabel.Text = "Pub Grub v1.0";
            this.CopyrightLabel.AutoSize = true;
            this.CopyrightLabel.Location = new Point(0x19, 40);
            this.CopyrightLabel.Margin = new Padding(3, 3, 3, 0);
            this.CopyrightLabel.Name = "CopyrightLabel";
            this.CopyrightLabel.Size = new Size(220, 13);
            this.CopyrightLabel.TabIndex = 2;
            this.CopyrightLabel.Text = "Copyright \x00a9 2012 Lolliesoft, Inc.";
            this.ProductLinkLabel.AutoSize = true;
            this.ProductLinkLabel.Location = new Point(0x19, 0x38);
            this.ProductLinkLabel.Margin = new Padding(3, 3, 3, 0);
            this.ProductLinkLabel.Name = "ProductLinkLabel";
            this.ProductLinkLabel.Size = new Size(0xa1, 13);
            this.ProductLinkLabel.TabIndex = 3;
            this.ProductLinkLabel.TabStop = true;
            this.ProductLinkLabel.Text = "www.lolliesoft.com";
            this.ProductLinkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(this.ProductLinkLabel_LinkClicked);
            this.RadiumLinkLabel.AutoSize = true;
            this.RadiumLinkLabel.Location = new Point(0x19, 0x48);
            this.RadiumLinkLabel.Margin = new Padding(3, 3, 3, 0);
            this.RadiumLinkLabel.Name = "RadiumLinkLabel";
            this.RadiumLinkLabel.Size = new Size(0x9d, 13);
            this.RadiumLinkLabel.TabIndex = 4;
            this.RadiumLinkLabel.TabStop = true;
            // this.RadiumLinkLabel.Text = "www.lolliesoft.com";
            this.RadiumLinkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(this.RadiumLinkLabel_LinkClicked);
            base.AcceptButton = this.OKButton;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x117, 0x86);
            base.Controls.Add(this.RadiumLinkLabel);
            base.Controls.Add(this.ProductLinkLabel);
            base.Controls.Add(this.CopyrightLabel);
            base.Controls.Add(this.TitleLabel);
            base.Controls.Add(this.OKButton);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "appAboutDialog";
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "About Pub Grub Calculator";
            base.TopMost = true;
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void ProductLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("http://www.lolliesoft.com");
            }
            catch (Exception exception)
            {
                this.HandleSystemError(exception);
            }
        }

        private void RadiumLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("http://www.lolliesoft.com/pubgrub");
            }
            catch (Exception exception)
            {
                this.HandleSystemError(exception);
            }
        }
    }
}

