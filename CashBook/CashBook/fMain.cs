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
        string currentFile = "";
        public fMain()
        {
            InitializeComponent();
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            // 메인폼을 화면에표시
            this.Show();

            currentFile = AppDomain.CurrentDomain.BaseDirectory + "Data\\" + DateTime.Now.ToString("yyyy-MM") + ".csv";

            userLogin();
        }

        void Summary()
        {
            // 입금액, 출금액, 합 계산하여 표시
            int ListCount = this.lv1.Items.Count;
            int sumIncome = 0;
            int sumSpending = 0;
            int balance = 0;

            for (int i = 0; i < ListCount; i++)
            {
                ListViewItem item = lv1.Items[i];
                string income = item.SubItems[2].Text.Replace(",", "");
                string spending = item.SubItems[3].Text.Replace(",", "");

                if (income == "") income = "0";
                if (spending == "") spending = "0";

                int iIncome = int.Parse(income);
                int iSpending = int.Parse(spending);

                sumIncome += iIncome;
                sumSpending += iSpending;

                balance += iIncome - iSpending;
            }
            sbSumIn.Text = sumIncome.ToString("N0");
            sbSumOut.Text = sumSpending.ToString("N0");

            sbAmt.Text = balance.ToString("N0");
            if (balance < 0) sbAmt.ForeColor = Color.Red;
            else sbAmt.ForeColor = Color.Blue;
        }

        void userLogin()
        {
            // 로그인 창
            fLogin f = new fLogin();
            DialogResult result = f.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // 로그인 성공
                string userId = f.txtId.Text;
                sbUserName.Text = userId;

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
                DateTime date = f.dtDate.Value;
                string category = f.txtType.Text;
                string amount = f.txtAmt.Text;
                string momo = f.txtMemo.Text;


                // 데이터를 추가한다.



                // 목록에 추가된 데이터를 표시한다. 
                ListViewItem lv = lv1.Items.Add(date.ToShortDateString());
                lv.SubItems.Add(category);
                lv.SubItems.Add(amount);
                lv.SubItems.Add(""); // 출금
                lv.SubItems.Add(momo);
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
                DateTime date = f.dtDate.Value;
                string category = f.txtType.Text;
                string amount = f.txtAmt.Text;
                string memo = f.txtMemo.Text;


                // 데이터를 추가한다.



                // 목록에 추가된 데이터를 표시한다. 
                ListViewItem lv = lv1.Items.Add(date.ToShortDateString());
                lv.SubItems.Add(category);
                lv.SubItems.Add(""); // 입금
                lv.SubItems.Add(amount);
                lv.SubItems.Add(memo);
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
            string date = lv.SubItems[0].Text;
            string category = lv.SubItems[1].Text;
            string income = lv.SubItems[2].Text;
            string spending = lv.SubItems[3].Text;
            string memo = lv.SubItems[4].Text;

            if (income != "")
            {
                // 입금 화면을 호출하고 현재 데이터를 전송
                fIN f = new fIN(date, category, income, memo);
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // 현재 선택된 자료를 업데이트
                    lv.SubItems[0].Text = f.dtDate.Value.ToShortDateString();
                    lv.SubItems[1].Text = f.txtType.Text;

                    string amount = f.txtAmt.Text.Replace(",", "");
                    int iAmout = int.Parse(amount);
                    lv.SubItems[2].Text = iAmout.ToString("N0");
                    lv.SubItems[3].Text = "";
                    lv.SubItems[4].Text = f.txtMemo.Text;
                }
            }
            else
            {
                // 출금 화면을 호출하고 현재 데이터를 전송
                fOUT f = new fOUT(date, category, spending, memo);
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // 현재 선택된 자료를 업데이트
                    lv.SubItems[0].Text = f.dtDate.Value.ToShortDateString();
                    lv.SubItems[1].Text = f.txtType.Text;
                    lv.SubItems[2].Text = "";
                    string amount = f.txtAmt.Text.Replace(",","");
                    int iAmount = int.Parse(amount);
                    lv.SubItems[3].Text = iAmount.ToString("N0");
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
            string category = lv.SubItems[1].Text;
            string income = lv.SubItems[2].Text;
            string spending = lv.SubItems[3].Text;
            string deleteAmount = "";

            if (income != "") deleteAmount = income;
            else deleteAmount = spending;

            string deleteMsg = "삭제하시겠습니까?\n\n" +
                            "분류 : " + category + "\n" +
                            "금액 : " + deleteAmount;


            DialogResult result = MessageBox.Show(deleteMsg, "삭제확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
            string directory = AppDomain.CurrentDomain.BaseDirectory + "Data";


            // 저장폴더가 없는 경우에만 폴더 생성
            if (System.IO.Directory.Exists(directory) == false)
                System.IO.Directory.CreateDirectory(directory); // 폴더생성

            string fileName = currentFile; //저장폴더 + "\\" + DateTime.Now.ToString("yyyy-MM") + ".csv";
            string content = "날짜,분류,입금,출금,비고";


            int listCount = lv1.Items.Count;
            for (int i = 0; i < listCount; i++)
            {
                ListViewItem item = lv1.Items[i];
                string date = item.SubItems[0].Text.Replace(",", "");
                string category = item.SubItems[1].Text.Replace(",", "");
                string income = item.SubItems[2].Text.Replace(",","");
                string spending = item.SubItems[3].Text.Replace(",", "");
                string memo = item.SubItems[4].Text.Replace(",", "");
                content += "\r\n" + date + "," + category + "," + income + "," + spending + "," + memo;
            }

            // 파일에 저장
            System.IO.File.WriteAllText(fileName, content, System.Text.Encoding.UTF8);
            Console.WriteLine("저장파일명=" + fileName);
            Console.WriteLine(fileName);
        }

        void loadData()
        {
            // 불러오기
            string fileName = currentFile; //저장폴더+ "\\" + nowstr;

            // 파일이 없으면 사용불가
            if (System.IO.File.Exists(fileName) == false)
            {
                MessageBox.Show("저장된 파일이 없습니다.\n\n" + fileName, "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string selectedMonth = fileName.Substring(fileName.LastIndexOf("\\")+1,7);
            lbmonth.Text = selectedMonth;

            // 내용을 지운다.
            lv1.Items.Clear();

            // 파일을 읽어옴
            // 배열의 인덱스를 벗어났다는 오류가 있어서
            // Where(line => !string.IsNullOrWhiteSpace(line)).ToArray() 추가해 해결.(빈줄이나 공백제거.)
            string[] content = System.IO.File.ReadAllLines(fileName, System.Text.Encoding.UTF8).Where(line => !string.IsNullOrWhiteSpace(line))
                    .ToArray();

            int contentsCount = content.Length;

            for (int i = 1; i < contentsCount; i++)
            {
                string lineContent = content[i];
                string[] lineDesc = lineContent.Split(',');


                // 필드 개수 확인 (최소 5개)
                if (lineDesc.Length < 5)
                {
                    MessageBox.Show($"잘못된 데이터 형식: {lineContent}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }

                ListViewItem item = lv1.Items.Add(lineDesc[0]); // 날짜
                item.SubItems.Add(lineDesc[1]); // 분류

                if (lineDesc[2] == "") lineDesc[2] = "0";
                if (lineDesc[3] == "") lineDesc[3] = "0";

                int income = int.Parse(lineDesc[2]);
                int spending = int.Parse(lineDesc[3]);

                if (income != 0)
                    item.SubItems.Add(income.ToString("N0")); // 입금
                else
                    item.SubItems.Add("");

                if (spending != 0)
                    item.SubItems.Add(spending.ToString("N0")); // 출금
                else
                    item.SubItems.Add("");

                item.SubItems.Add(lineDesc[4]); // 비고
            }
            Summary();
            Console.WriteLine(content);
        }

        private void lv1_DoubleClick(object sender, EventArgs e)
        {
            editData();
        }

        private void lv1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete) delData();
        }

        private void changeMonth_Click(object sender, EventArgs e)
        {
            fFileList f = new fFileList();
            if(f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                currentFile = f.selectedFileName;
                loadData();
            }
        }

        private void closeMonth_Click(object sender, EventArgs e)
        {
            // 마감 확인
            var dlg = MessageBox.Show("마감하시겠습니까?\n\n마감 이후 월의 자료가 있다면 삭제됩니다.", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg != System.Windows.Forms.DialogResult.Yes) return;

            // 월마감
            string selectedMonth = lbmonth.Text;
            string directory = AppDomain.CurrentDomain.BaseDirectory + "Data";

            // 다음달로 잔액 이월 (ex : 2024 05 -> 2024 06)
            DateTime currentMonth = DateTime.Parse(selectedMonth + "-01"); // 2024-05-01
            DateTime nextMonth = currentMonth.AddMonths(1); // 2024-05-01 -> 2024-06-01
            string fileName = nextMonth.ToString("yyyy-MM")+".csv";
            string fullFileName = directory + "\\" + fileName; // ...\data\2024-06-04.csv

            // 잔액 
            int balance = int.Parse(sbAmt.Text.Replace(",",""));
           
            // 신규파일에 6월 1일자로 잔액 이월

            string contents = "날짜,분류,입금,출금,비고";
            string date = nextMonth.ToString("yyyy-MM-dd");
            string category = "잔액이월";
            string income = balance.ToString();
            string spending = "";
            string memo = string.Format("{0}월 잔액이월", currentMonth.ToString("yyyy-MM"));
            string newContent = date + "," + category + "," + income + "," + spending + "," + memo;

            // 파일이 존재하는지 확인
            if (System.IO.File.Exists(fullFileName))
            {
                // 기존 파일 내용을 읽어옴
                string savedContent = System.IO.File.ReadAllText(fullFileName, System.Text.Encoding.UTF8);
                // 기존 내용에 새로운 내용을 추가
                contents = savedContent + "\r\n" + newContent;
            }
            else
            {
                // 파일이 없으면 새로운 파일 내용 작성
                contents += "\r\n" + newContent;
            }

            // 파일에 저장
            System.IO.File.WriteAllText(fullFileName, contents, System.Text.Encoding.UTF8);
            Console.WriteLine("저장파일명=" + fileName);

            // 다음달 자료 자동 열기
            currentFile = fullFileName;
            loadData();


        }

        private void conEditBtn_Click(object sender, EventArgs e)
        {
            editData();
        }

        private void conDeleteBtn_Click(object sender, EventArgs e)
        {
            delData();
        }
    }
}
