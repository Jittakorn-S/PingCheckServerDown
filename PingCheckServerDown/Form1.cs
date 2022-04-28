using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace PingCheckServerDown
{
    public partial class Form1 : Form
    {
        string C_CheckState = "Online";
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void LineNotify(string msg)
        {
            string token = ConfigurationManager.AppSettings.Get("Token");
            try
            {
                //Initial LINE API
                var request = (HttpWebRequest)WebRequest.Create("https://notify-api.line.me/api/notify");
                var postData = string.Format("message={0}", msg);
                var data = Encoding.UTF8.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Add("Authorization", "Bearer " + token);
                using (var stream = request.GetRequestStream()) stream.Write(data, 0, data.Length);
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                //
            }
            catch (Exception Error)
            {
                //Create Log File
                string LogFilePath = ConfigurationManager.AppSettings.Get("LogFile");
                using (FileStream FileLog = new FileStream(LogFilePath, FileMode.Append, FileAccess.Write))
                using (StreamWriter WriteFile = new StreamWriter(FileLog))
                {
                    WriteFile.WriteLine(DateTime.Now);
                    WriteFile.WriteLine(Error.Message);
                    WriteFile.WriteLine("{0} Exception caught.", Error);
                    WriteFile.WriteLine("-----------------------------------------------------------------");
                }
            }
        }
        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string DateNow = DateTime.Now.ToString();
            string[] GetText;
            List<string> DataList = new List<string>();
            try
            {
                string CheckStatus = "Online";
                GetText = System.IO.File.ReadAllLines(ConfigurationManager.AppSettings.Get("ReadFilePath"));
                DataList = GetText.ToList();
                var GetTextOffline = DataList.Where(stringToCheck => stringToCheck.Contains("Offline"));
                if (GetTextOffline.Count() > 0)
                {
                    CheckStatus = "Offline";
                }
                if (CheckStatus != C_CheckState)
                {
                    C_CheckState = CheckStatus;
                    if (C_CheckState == "Online")
                    {
                        LineNotify("ประจำวันที่: " + DateNow + "\n" + "Server กลับมาทำงานเป็นปกติ");
                    }
                    else
                    {
                        LineNotify("ประจำวันที่: " + DateNow + "\n" + "ตรวจพบ Server Offline กรุณาตรวจสอบ: " + "\n" + "\n" + (String.Join("\n", GetTextOffline)));
                    }
                }
            }
            catch (Exception Error)
            {
                //Create Log File
                string LogFilePath = ConfigurationManager.AppSettings.Get("LogFile");
                using (FileStream FileLog = new FileStream(LogFilePath, FileMode.Append, FileAccess.Write))
                using (StreamWriter WriteFile = new StreamWriter(FileLog))
                {
                    WriteFile.WriteLine(DateTime.Now);
                    WriteFile.WriteLine(Error.Message);
                    WriteFile.WriteLine("{0} Exception caught.", Error);
                    WriteFile.WriteLine("-----------------------------------------------------------------");
                }
            }
        }
    }
}
