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
    public partial class fLogin : Form
    {
        public fLogin()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string 입력아이디 = txtId.Text;
            string 입력암호 = txtPw.Text;

            if(입력아이디 == "장수연" && 입력암호 == "1234")
            {
                // 다이얼로그창에서 결과를 내보냄
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                MessageBox.Show("아이디 또는 비밀번호가 일치하지 않습니다.", "로그인실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtId.Focus();
                txtId.SelectAll();
            }
        }

        private void txtId_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                // 입력값 체크
                if(txtId.Text != "") txtPw.Focus();
            }
        }

        private void txtPw_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // 입력값 체크
                if (txtPw.Text != "") btnOk.Focus();
            }
        }
    }
}
