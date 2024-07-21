namespace TVShowChecker; 
partial class RemoveTVDialog {
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoveTVDialog));
        this.RemoveButton = new System.Windows.Forms.Button();
        this.listView1 = new System.Windows.Forms.ListView();
        this.DialogCancelButton = new System.Windows.Forms.Button();
        this.SuspendLayout();
        // 
        // RemoveButton
        // 
        this.RemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.RemoveButton.Location = new System.Drawing.Point(266, 403);
        this.RemoveButton.Name = "RemoveButton";
        this.RemoveButton.Size = new System.Drawing.Size(87, 23);
        this.RemoveButton.TabIndex = 0;
        this.RemoveButton.Text = "Remove";
        this.RemoveButton.UseVisualStyleBackColor = true;
        this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
        // 
        // listView1
        // 
        this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
        this.listView1.BackgroundImageTiled = true;
        this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.listView1.Location = new System.Drawing.Point(12, 12);
        this.listView1.Name = "listView1";
        this.listView1.Size = new System.Drawing.Size(341, 385);
        this.listView1.Sorting = System.Windows.Forms.SortOrder.Ascending;
        this.listView1.TabIndex = 2;
        this.listView1.TileSize = new System.Drawing.Size(170, 30);
        this.listView1.UseCompatibleStateImageBehavior = false;
        this.listView1.View = System.Windows.Forms.View.Tile;
        // 
        // DialogCancelButton
        // 
        this.DialogCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.DialogCancelButton.Location = new System.Drawing.Point(185, 403);
        this.DialogCancelButton.Name = "DialogCancelButton";
        this.DialogCancelButton.Size = new System.Drawing.Size(75, 23);
        this.DialogCancelButton.TabIndex = 1;
        this.DialogCancelButton.Text = "Cancel";
        this.DialogCancelButton.UseVisualStyleBackColor = true;
        this.DialogCancelButton.Click += new System.EventHandler(this.DialogCancelButton_Click);
        // 
        // RemoveTVDialog
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(365, 438);
        this.Controls.Add(this.DialogCancelButton);
        this.Controls.Add(this.listView1);
        this.Controls.Add(this.RemoveButton);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.MinimumSize = new System.Drawing.Size(250, 200);
        this.Name = "RemoveTVDialog";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Remove TV Show";
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button RemoveButton;
    private System.Windows.Forms.ListView listView1;
    private System.Windows.Forms.Button DialogCancelButton;
}