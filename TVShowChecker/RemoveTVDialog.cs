using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TVShowChecker {
    public partial class RemoveTVDialog : Form {

        private List<String> subbedTVShows;

        public RemoveTVDialog() {
            InitializeComponent();
            this.AcceptButton = button_remove;
        }

        public DialogResult go(Point location, List<String> subbedTVShows) {
            this.Location = new Point(location.X, location.Y - this.Height);
            this.subbedTVShows = subbedTVShows;

            foreach (string show in subbedTVShows) {
                listView1.Items.Add(new ListViewItem(show));
            }

            DialogResult dialogResult = this.ShowDialog();
            return dialogResult;
        }

        private void button_remove_Click(object sender, EventArgs e) {
            ListView.SelectedListViewItemCollection selected = listView1.SelectedItems;

            foreach (ListViewItem item in selected) {
                subbedTVShows.Remove(item.Text);
            }

            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
