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
            string directory = AppDomain.CurrentDomain.BaseDirectory + "Data";
            if (System.IO.Directory.Exists(directory) == false) return;

            string[] FileList = System.IO.Directory.GetFiles(directory, "*.csv");

            for(int i = 0; i < FileList.Length; i++)
            {
                string fileName = FileList[i];

                // 역슬래쉬 찾기 (못찾으면 -1)
                var findSlash = fileName.LastIndexOf('\\');
                var fingDot = fileName.LastIndexOf(".");

                var addFileName = fileName.Substring(findSlash+1, fingDot - findSlash -1);

                listBox1.Items.Add(addFileName);
            }

            if(listBox1.Items.Count < 0)
            {
                button1.Enabled = false;
            }
        }
        public string selectedFileName = "";
        private void button1_Click(object sender, EventArgs e)
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory + "Data\\";
            selectedFileName = directory + listBox1.Items[listBox1.SelectedIndex].ToString() + ".csv";
            DialogResult = System.Windows.Forms.DialogResult.OK;  
        }
    }
}
