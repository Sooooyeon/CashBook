using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoneyBook
{
    public partial class fIN : Form
    {
        public fIN()
        {
            InitializeComponent();
        }

        public fIN(string 날짜, string 분류, string 금액, string 비고)
        {
            InitializeComponent();
            dtDate.Value = DateTime.Parse(날짜);
            txtType.Text = 분류;
            txtAmt.Text = 금액;
            txtMemo.Text = 비고;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void fIN_Load(object sender, EventArgs e)
        {

        }
    }
}
