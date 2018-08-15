using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TVShowChecker
{
    public partial class RemoveTVDialog : Form
    {
        private List<String> subbedTVShows;

        public RemoveTVDialog()
        {
            InitializeComponent();
            AcceptButton = RemoveButton;
        }

        public DialogResult Go(Point location, List<String> subbedTVShows)
        {
            Location = new Point(location.X, location.Y - Height);
            this.subbedTVShows = subbedTVShows;

            foreach (string show in subbedTVShows)
            {
                listView1.Items.Add(new ListViewItem(show));
            }

            DialogResult dialogResult = ShowDialog();
            return dialogResult;
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection selected = listView1.SelectedItems;

            foreach (ListViewItem item in selected)
            {
                subbedTVShows.Remove(item.Text);
            }

            Close();
        }

        private void DialogCancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
