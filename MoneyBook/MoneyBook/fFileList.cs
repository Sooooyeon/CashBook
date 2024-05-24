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
    public partial class fFileList : Form
    {
        public fFileList()
        {
            InitializeComponent();
        }

        private void fFileList_Load(object sender, EventArgs e)
        {
            string 폴더위치 = AppDomain.CurrentDomain.BaseDirectory + "Data";
            if (System.IO.Directory.Exists(폴더위치) == false) return;

            string[] 파일목록 = System.IO.Directory.GetFiles(폴더위치, "*.csv");

            for(int i = 0; i < 파일목록.Length; i++)
            {
                string 파일명 = 파일목록[i];

                // 역슬래쉬 찾기 (못찾으면 -1)
                var 역슬래쉬위치 = 파일명.LastIndexOf('\\');
                var 확장자위치 = 파일명.LastIndexOf(".");

                var 파일이름 = 파일명.Substring(역슬래쉬위치+1, 확장자위치 - 역슬래쉬위치 -1);

                listBox1.Items.Add(파일이름);
            }

            if(listBox1.Items.Count < 0)
            {
                button1.Enabled = false;
            }
        }
        public string 선택된파일명 = "";
        private void button1_Click(object sender, EventArgs e)
        {

            string 저장폴더 = AppDomain.CurrentDomain.BaseDirectory + "Data\\";
            선택된파일명 = 저장폴더 + listBox1.Items[listBox1.SelectedIndex].ToString() + ".csv";
            DialogResult = System.Windows.Forms.DialogResult.OK;  
        }
    }
}
