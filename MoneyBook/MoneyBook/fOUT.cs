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
    public partial class fOUT : Form
    {
        public fOUT()
        {
            InitializeComponent();
        }

        public fOUT(string 날짜, string 분류, string 금액, string 비고)
        {
            InitializeComponent();
            dtDate.Value = DateTime.Parse(날짜);
            txtType.Text = 분류;
            txtAmt.Text = 금액;
            txtMemo.Text = 비고;
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
            int 금액 = int.Parse(txtAmt.Text.Replace(",", ""));
            txtAmt.Text = 금액.ToString("N0");

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
