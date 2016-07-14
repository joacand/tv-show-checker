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
    public partial class AddTVDialog : Form {

        private string statusMsgCallback = "";

        public AddTVDialog() {
            InitializeComponent();
            this.AcceptButton = button_add;
        }

        public DialogResult go(Point location) {
            this.Location = new Point(location.X, location.Y - this.Height);

            DialogResult dialogResult = this.ShowDialog();
            return dialogResult;
        }

        public string getStatusMsgCallback() {
            return statusMsgCallback;
        }

        private void button_add_Click(object sender, EventArgs e) {
            statusMsgCallback = textBox_addTV.Text;
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
