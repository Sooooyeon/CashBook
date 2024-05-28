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
    public partial class fMain : Form
    {
        string 현재열린파일명 = "";
        public fMain()
        {
            InitializeComponent();
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            // 메인폼을 화면에표시
            this.Show();

            현재열린파일명 = AppDomain.CurrentDomain.BaseDirectory + "Data\\" + DateTime.Now.ToString("yyyy-MM") + ".csv";

            userLogin();
        }

        void Summary()
        {
            // 입금액, 출금액, 합 계산하여 표시
            int 목록건수 = this.lv1.Items.Count;
            int 합계_입금 = 0;
            int 합계_출금 = 0;
            int 잔액 = 0;

            for (int i = 0; i < 목록건수; i++)
            {
                ListViewItem item = lv1.Items[i];
                string 입금 = item.SubItems[2].Text.Replace(",", "");
                string 출금 = item.SubItems[3].Text.Replace(",", "");

                if (입금 == "") 입금 = "0";
                if (출금 == "") 출금 = "0";

                int i입금 = int.Parse(입금);
                int i출금 = int.Parse(출금);

                잔액 += i입금 - i출금;

                합계_입금 += i입금;
                합계_출금 += i출금;
            }
            sbSumIn.Text = 합계_입금.ToString("N0");
            sbSumOut.Text = 합계_출금.ToString("N0");

            if (잔액 < 0) sbAmt.ForeColor = Color.Red;
            else sbAmt.ForeColor = Color.Blue;
            sbAmt.Text = 잔액.ToString("N0");
        }

        void userLogin()
        {
            // 로그인 창
            fLogin f = new fLogin();
            DialogResult result = f.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // 로그인 성공
                string 사용자명 = f.txtId.Text;
                sbUserName.Text = 사용자명;

                // 1. 자료를 불러와서 표시
                // 2. 입/출금 등록 버튼을 활성화
                btnIn.Enabled = true; // 입금버튼 비활성
                btnOut.Enabled = true; // 출금버튼 비활성

                loadData();
            }
            else
            {
                // 로그인 실패
                // 1. 현재 표시되는 목록 제거
                // 2. 입/출금 등록 버튼을 비활성
                btnIn.Enabled = false; // 입금버튼 비활성
                btnOut.Enabled = false; // 출금버튼 비활성
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            userLogin();
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            fIN f = new fIN();
            DialogResult result = f.ShowDialog();
            if(result == System.Windows.Forms.DialogResult.OK)
            {

                // 데이터 읽기
                DateTime 입력일 = f.dtDate.Value;
                string 분류 = f.txtType.Text;
                string 금액 = f.txtAmt.Text;
                string 비고 = f.txtMemo.Text;


                // 데이터를 추가한다.



                // 목록에 추가된 데이터를 표시한다. 
                ListViewItem lv = lv1.Items.Add(입력일.ToShortDateString());
                lv.SubItems.Add(분류);
                lv.SubItems.Add(금액);
                lv.SubItems.Add(""); // 출금
                lv.SubItems.Add(비고);
                Summary();
                saveData();
            }

        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            fOUT f = new fOUT();
            DialogResult result = f.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {

                // 데이터 읽기
                DateTime 입력일 = f.dtDate.Value;
                string 분류 = f.txtType.Text;
                string 금액 = f.txtAmt.Text;
                string 비고 = f.txtMemo.Text;


                // 데이터를 추가한다.



                // 목록에 추가된 데이터를 표시한다. 
                ListViewItem lv = lv1.Items.Add(입력일.ToShortDateString());
                lv.SubItems.Add(분류);
                lv.SubItems.Add(""); // 입금
                lv.SubItems.Add(금액);
                lv.SubItems.Add(비고);
                Summary();
                saveData();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            editData();

        }

        void editData() {

            // 편집메뉴
            if (lv1.SelectedItems.Count < 1)
            {
                MessageBox.Show("데이터를 선택하세요.");
                return;

            }
            // 선택한 자료의 구분을 확인한다.
            ListViewItem lv = lv1.SelectedItems[0];
            string 날짜 = lv.SubItems[0].Text;
            string 분류 = lv.SubItems[1].Text;
            string 입금액 = lv.SubItems[2].Text;
            string 출금액 = lv.SubItems[3].Text;
            string 비고 = lv.SubItems[4].Text;
            if (입금액 != "")
            {
                // 입금 화면을 호출하고 현재 데이터를 전송
                fIN f = new fIN(날짜, 분류, 입금액, 비고);
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // 현재 선택된 자료를 업데이트
                    lv.SubItems[0].Text = f.dtDate.Value.ToShortDateString();
                    lv.SubItems[1].Text = f.txtType.Text;

                    string 입력값 = f.txtAmt.Text.Replace(",", "");
                    int 숫자값 = int.Parse(입력값);
                    lv.SubItems[2].Text = 숫자값.ToString("N0");
                    lv.SubItems[3].Text = "";
                    lv.SubItems[4].Text = f.txtMemo.Text;
                }
            }
            else
            {
                // 출금 화면을 호출하고 현재 데이터를 전송
                fOUT f = new fOUT(날짜, 분류, 출금액, 비고);
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // 현재 선택된 자료를 업데이트
                    lv.SubItems[0].Text = f.dtDate.Value.ToShortDateString();
                    lv.SubItems[1].Text = f.txtType.Text;
                    lv.SubItems[2].Text = "";
                    string 입력값 = f.txtAmt.Text.Replace(",","");
                    int 숫자값 = int.Parse(입력값);
                    lv.SubItems[3].Text = 숫자값.ToString("N0");
                    lv.SubItems[4].Text = f.txtMemo.Text;
                }
            }
            saveData();
            Summary();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            delData();
        }

        void delData()
        {
            // 삭제메뉴
            if (lv1.SelectedItems.Count < 1)
            {
                MessageBox.Show("데이터를 선택하세요.");
                return;

            }
            ListViewItem lv = lv1.SelectedItems[0];
            string 분류 = lv.SubItems[1].Text;
            string 입금액 = lv.SubItems[2].Text;
            string 출금액 = lv.SubItems[3].Text;
            string 표시금액 = "";

            if (입금액 != "") 표시금액 = 입금액;
            else 표시금액 = 출금액;

            string 메세지 = "삭제하시겠습니까?\n\n" +
                            "분류 : " + 분류 + "\n" +
                            "금액 : " + 표시금액;


            DialogResult result = MessageBox.Show(메세지, "삭제확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                lv1.Items.Remove(lv);
                MessageBox.Show("삭제 완료");
                saveData();
                Summary();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveData();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            loadData();
        }

        void saveData()
        {
            // 저장하기
            // 날짜,분류,입금,출금,비고
            // 현재 폴더 경로를 가져와 Data폴더 생성해 csv 형식으로 파일 저장
            string 저장폴더 = AppDomain.CurrentDomain.BaseDirectory + "Data";
            string 파일명 = 현재열린파일명; //저장폴더 + "\\" + DateTime.Now.ToString("yyyy-MM") + ".csv";
            string 내용 = "날짜,분류,입금,출금,비고";

            // 저장폴더가 없는 경우에만 폴더 생성
            if (System.IO.Directory.Exists(저장폴더) == false)
                System.IO.Directory.CreateDirectory(저장폴더); // 폴더생성

            int 건수 = lv1.Items.Count;
            for (int i = 0; i < 건수; i++)
            {
                ListViewItem item = lv1.Items[i];
                string 날짜 = item.SubItems[0].Text.Replace(",", "");
                string 분류 = item.SubItems[1].Text.Replace(",", "");
                string 입금 = item.SubItems[2].Text.Replace(",","");
                string 출금 = item.SubItems[3].Text.Replace(",", "");
                string 비고 = item.SubItems[4].Text.Replace(",", "");
                내용 += "\r\n" + 날짜 + "," + 분류 + "," + 입금 + "," + 출금 + "," + 비고;
            }

            // 파일에 저장
            System.IO.File.WriteAllText(파일명, 내용, System.Text.Encoding.UTF8);
            Console.WriteLine("저장파일명=" + 파일명);
            Console.WriteLine(파일명);
        }

        void loadData()
        {
            // 불러오기
            string 저장폴더 = AppDomain.CurrentDomain.BaseDirectory + "Data";

            DateTime 현재시간 = DateTime.Now;
            string nowstr = 현재시간.ToString("yyyy-MM")+".csv";

            string 파일명 = 현재열린파일명; //저장폴더+ "\\" + nowstr;

            // 파일이 없으면 사용불가
            if (System.IO.File.Exists(파일명) == false)
            {
                MessageBox.Show("저장된 파일이 없습니다.\n\n" + 파일명, "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string 선택월 = 파일명.Substring(파일명.LastIndexOf("\\")+1,7);
            lbmonth.Text = 선택월;

            // 내용을 지운다.
            lv1.Items.Clear();

            // 파일을 읽어옴
            // 배열의 인덱스를 벗어났다는 오류가 있어서 .Where(line => !string.IsNullOrWhiteSpace(line)).ToArray() 추가해 해결.(빈줄이나 공백제거.)
            string[] 내용 = System.IO.File.ReadAllLines(파일명, System.Text.Encoding.UTF8).Where(line => !string.IsNullOrWhiteSpace(line))
                    .ToArray();

            int 건수 = 내용.Length;

            for (int i = 1; i < 건수; i++)
            {
                string 줄내용 = 내용[i];
                string[] 줄버퍼 = 줄내용.Split(',');


                // 필드 개수 확인 (최소 5개)
                if (줄버퍼.Length < 5)
                {
                    MessageBox.Show($"잘못된 데이터 형식: {줄내용}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }

                ListViewItem item = lv1.Items.Add(줄버퍼[0]); // 날짜
                item.SubItems.Add(줄버퍼[1]); // 분류

                if (줄버퍼[2] == "") 줄버퍼[2] = "0";
                if (줄버퍼[3] == "") 줄버퍼[3] = "0";

                int 입금액 = int.Parse(줄버퍼[2]);
                int 출금액 = int.Parse(줄버퍼[3]);

                if (입금액 != 0)
                    item.SubItems.Add(입금액.ToString("N0")); // 입금
                else
                    item.SubItems.Add("");

                if (출금액 != 0)
                    item.SubItems.Add(출금액.ToString("N0")); // 출금
                else
                    item.SubItems.Add("");

                item.SubItems.Add(줄버퍼[4]); // 비고
            }
            Summary();
            Console.WriteLine(내용);
        }

        private void lv1_DoubleClick(object sender, EventArgs e)
        {
            editData();
        }

        private void lv1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete) delData();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            fFileList f = new fFileList();
            if(f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                현재열린파일명 = f.선택된파일명;
                loadData();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            // 마감 확인
            var dlg = MessageBox.Show("마감하시겠습니까?\n\n마감 이후 월의 자료가 있다면 삭제됩니다.", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg != System.Windows.Forms.DialogResult.Yes) return;

            // 월마감
            string 선택월 = lbmonth.Text;
            string 저장폴더 = AppDomain.CurrentDomain.BaseDirectory + "Data";

            // 다음달로 잔액 이월 (2024 05 -> 2024 06)
            DateTime 현재월 = DateTime.Parse(선택월 + "-01"); // 2024-05-01
            DateTime 다음월 = 현재월.AddMonths(1); // 2024-05-01 -> 2024-06-01
            string 파일명 = 다음월.ToString("yyyy-MM")+".csv";
            string 전체파일명 = 저장폴더 + "\\" + 파일명; // ...\data\2024-06-04.csv

            // 잔액 
            int 잔액 = int.Parse(sbAmt.Text.Replace(",",""));
           
            // 신규파일에 6월 1일자로 잔액 이월

            string 내용 = "날짜,분류,입금,출금,비고";
            string 날짜 = 다음월.ToString("yyyy-MM-dd");
            string 분류 = "잔액이월";
            string 입금 = 잔액.ToString();
            string 출금 = "";
            string 비고 = string.Format("{0}월 잔액이월", 현재월.ToString("yyyy-MM"));
            string 새로운내용 = 날짜 + "," + 분류 + "," + 입금 + "," + 출금 + "," + 비고;

            // 파일이 존재하는지 확인
            if (System.IO.File.Exists(전체파일명))
            {
                // 기존 파일 내용을 읽어옴
                string 기존내용 = System.IO.File.ReadAllText(전체파일명, System.Text.Encoding.UTF8);
                // 기존 내용에 새로운 내용을 추가
                내용 = 기존내용 + "\r\n" + 새로운내용;
            }
            else
            {
                // 파일이 없으면 새로운 파일 내용 작성
                내용 += "\r\n" + 새로운내용;
            }

            // 파일에 저장
            System.IO.File.WriteAllText(전체파일명, 내용, System.Text.Encoding.UTF8);
            Console.WriteLine("저장파일명=" + 파일명);

            // 다음달 자료 자동 열기
            현재열린파일명 = 전체파일명;
            loadData();


        }
    }
}
