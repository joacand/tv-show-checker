using System;
using System.Drawing;
using System.Windows.Forms;

namespace TVShowChecker;

public partial class AddTVDialog : Form
{
    public string StatusMsgCallback { get; private set; }

    public AddTVDialog()
    {
        InitializeComponent();
        AcceptButton = AddTvShowButton;
    }

    public DialogResult Go(Point location)
    {
        Location = new Point(location.X, location.Y - Height);

        var dialogResult = ShowDialog();
        return dialogResult;
    }

    private void AddTvShowButton_Click(object sender, EventArgs e)
    {
        StatusMsgCallback = textBox_addTV.Text;
        Close();
    }

    private void AddDialogCancelButton_Click(object sender, EventArgs e)
    {
        Close();
    }
}
