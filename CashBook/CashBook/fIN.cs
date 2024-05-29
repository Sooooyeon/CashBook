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

        public fIN(string date, string category, string amount, string memo)
        {
            InitializeComponent();
            dtDate.Value = DateTime.Parse(date);
            txtType.Text = category;
            txtAmt.Text = amount;
            txtMemo.Text = memo;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // 분류입력
            if (txtType.Text == "")
            {
                txtType.Focus();
                return;
            }

            // 금액입력체크
            if (txtAmt.Text == "")
            {
                txtAmt.Focus();
                return;
            }

            // 금액에 콤마 표시
            int iAmount = int.Parse(txtAmt.Text.Replace(",",""));
            txtAmt.Text = iAmount.ToString("N0");

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void fIN_Load(object sender, EventArgs e)
        {

        }
    }
}
