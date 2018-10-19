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
            this.OKButton = new System.Windows.Forms.Button();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.CopyrightLabel = new System.Windows.Forms.Label();
            this.ProductLinkLabel = new System.Windows.Forms.LinkLabel();
            this.RadiumLinkLabel = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // OKButton
            // 
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKButton.Location = new System.Drawing.Point(192, 99);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.Location = new System.Drawing.Point(25, 24);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(133, 13);
            this.TitleLabel.TabIndex = 1;
            this.TitleLabel.Text = "Bartender Express Pro";
            // 
            // CopyrightLabel
            // 
            this.CopyrightLabel.AutoSize = true;
            this.CopyrightLabel.Location = new System.Drawing.Point(25, 40);
            this.CopyrightLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.CopyrightLabel.Name = "CopyrightLabel";
            this.CopyrightLabel.Size = new System.Drawing.Size(158, 13);
            this.CopyrightLabel.TabIndex = 2;
            this.CopyrightLabel.Text = "Copyright © 2018 Lolliesoft, Inc.";
            // 
            // ProductLinkLabel
            // 
            this.ProductLinkLabel.AutoSize = true;
            this.ProductLinkLabel.Location = new System.Drawing.Point(25, 56);
            this.ProductLinkLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.ProductLinkLabel.Name = "ProductLinkLabel";
            this.ProductLinkLabel.Size = new System.Drawing.Size(130, 13);
            this.ProductLinkLabel.TabIndex = 3;
            this.ProductLinkLabel.TabStop = true;
            this.ProductLinkLabel.Text = "https://www.lolliesoft.com";
            this.ProductLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ProductLinkLabel_LinkClicked);
            // 
            // RadiumLinkLabel
            // 
            this.RadiumLinkLabel.AutoSize = true;
            this.RadiumLinkLabel.Location = new System.Drawing.Point(25, 72);
            this.RadiumLinkLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.RadiumLinkLabel.Name = "RadiumLinkLabel";
            this.RadiumLinkLabel.Size = new System.Drawing.Size(0, 13);
            this.RadiumLinkLabel.TabIndex = 4;
            this.RadiumLinkLabel.TabStop = true;
            this.RadiumLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.RadiumLinkLabel_LinkClicked);
            // 
            // appAboutDialog
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 134);
            this.Controls.Add(this.RadiumLinkLabel);
            this.Controls.Add(this.ProductLinkLabel);
            this.Controls.Add(this.CopyrightLabel);
            this.Controls.Add(this.TitleLabel);
            this.Controls.Add(this.OKButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "appAboutDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About Bartender Express Calculator";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

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

