namespace TVShowChecker {
    partial class AddTVDialog {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddTVDialog));
            this.textBox_addTV = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.AddTvShowButton = new System.Windows.Forms.Button();
            this.AddDialogCancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_addTV
            // 
            this.textBox_addTV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_addTV.Location = new System.Drawing.Point(66, 6);
            this.textBox_addTV.Name = "textBox_addTV";
            this.textBox_addTV.Size = new System.Drawing.Size(300, 20);
            this.textBox_addTV.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "TV Show:";
            // 
            // AddTvShowButton
            // 
            this.AddTvShowButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddTvShowButton.Location = new System.Drawing.Point(291, 32);
            this.AddTvShowButton.Name = "AddTvShowButton";
            this.AddTvShowButton.Size = new System.Drawing.Size(75, 23);
            this.AddTvShowButton.TabIndex = 1;
            this.AddTvShowButton.Text = "Add";
            this.AddTvShowButton.UseVisualStyleBackColor = true;
            this.AddTvShowButton.Click += new System.EventHandler(this.AddTvShowButton_Click);
            // 
            // AddDialogCancelButton
            // 
            this.AddDialogCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddDialogCancelButton.Location = new System.Drawing.Point(210, 32);
            this.AddDialogCancelButton.Name = "AddDialogCancelButton";
            this.AddDialogCancelButton.Size = new System.Drawing.Size(75, 23);
            this.AddDialogCancelButton.TabIndex = 4;
            this.AddDialogCancelButton.Text = "Cancel";
            this.AddDialogCancelButton.UseVisualStyleBackColor = true;
            this.AddDialogCancelButton.Click += new System.EventHandler(this.AddDialogCancelButton_Click);
            // 
            // AddTVDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 64);
            this.Controls.Add(this.AddDialogCancelButton);
            this.Controls.Add(this.AddTvShowButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_addTV);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(3000, 103);
            this.MinimumSize = new System.Drawing.Size(16, 103);
            this.Name = "AddTVDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add TV Show";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_addTV;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button AddTvShowButton;
        private System.Windows.Forms.Button AddDialogCancelButton;
    }
}