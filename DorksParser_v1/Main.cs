using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Net;
using System.Text.RegularExpressions;
using System.Diagnostics;


namespace DorkParser_v1
{
    public partial class Main : Form
    {
        StreamReader combo,proxy;
        int num,port,number,errors;
        string host;
        string apppath;
        int percent_total;
        
        public Main()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;
            apppath = Path.GetDirectoryName(Application.ExecutablePath);
            bunifuCircleProgressbar1.Value = 0;
        }

        private void bunifuCustomLabel5_Click(object sender, EventArgs e)
        {

        }

        private void bunifuCustomLabel13_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void bunifuSeparator4_Load(object sender, EventArgs e)
        {

        }

        private void bunifuCards2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Enter Dorks";
            dialog.Filter = "TXT FILES|*.txt";
            dialog.ShowDialog();
            combo = new StreamReader(dialog.FileName);
            StreamReader sv = new StreamReader(dialog.FileName);
            string text = sv.ReadToEnd();
            num = text.Count(c => c == '\n');
            percent_total = num;
            bunifuCustomLabel10.Text = num.ToString();
            sv.Close();
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
           this.WindowState = FormWindowState.Minimized;
        }

        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", apppath);
        }

        private void bunifuCustomTextbox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void bunifuGradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bunifuSeparator6_Load(object sender, EventArgs e)
        {

        }

        private void bunifuSeparator5_Load(object sender, EventArgs e)
        {

        }

        private void bunifuCustomLabel1_Click(object sender, EventArgs e)
        {

        }

        private void bunifuCustomLabel11_Click(object sender, EventArgs e)
        {

        }

        private void bunifuCheckbox2_OnChange(object sender, EventArgs e)
        {

        }

        private void bunifuCheckbox4_OnChange(object sender, EventArgs e)
        {

        }

        private void bunifuCheckbox5_OnChange(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void bunifuCustomLabel2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private  void bunifuFlatButton3_Click(object sender, EventArgs e)
        {           
            int percent_part = 0;
            string sep;            
            string filename = string.Format("{0:ddd, MMM d, yyyy}_{1}", DateTime.Now, "Result.txt");
            ThreadPool.SetMinThreads(1000, 1000);         
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.DefaultConnectionLimit = 100;         
            if (bunifuFlatButton3.Text == "Start")
            {
                bunifuCustomLabel12.Text = "Working";
                bunifuFlatButton3.Text = "Stop";
                while((sep = combo.ReadLine()) != null)
                   
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(scanner), sep);                                                                                                                 
                }
            }
           else
            {
                bunifuFlatButton3.Text = "Start";
            }
            
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Enter proxy";
            dialog.Filter = "TXT FILES|*.txt";
            dialog.ShowDialog();
            proxy = new StreamReader(dialog.FileName);
            StreamReader sv = new StreamReader(dialog.FileName);
            string text = sv.ReadToEnd();
            int num = text.Count(c => c == '\n');
            bunifuCustomLabel11.Text = num.ToString();
            sv.Close();
        }
       
        
        public void  scanner(object sep)
        {
            //textBox1.Text += "method called";                            
            try
            {
                
                bool ban = true;
                while (!proxy.EndOfStream && ban == true )
                {
                    string bingsearch = string.Format("https://www.bing.com/search?q={0}", sep.ToString());
                    string load = proxy.ReadLine();                   
                    //textBox1.Text += bingsearch;                    // ----------------- //
                    string[] split = load.Split(':');
                    host = split[0];
                    port = Int32.Parse(split[1]);
                    try
                    {
                        WebClient wc = new WebClient();
                        wc.Proxy = new WebProxy(host, port);
                        string response = wc.DownloadString(bingsearch);
                        //textBox1.Text += response;                    // ----------------- //
                        string PATTERN = @"(<a.*?>.*?</a>)";
                        MatchCollection collection = Regex.Matches(response, PATTERN, RegexOptions.IgnoreCase);
                        foreach (Match match in collection)
                        {
                            ban = false;
                            string a = match.Groups[1].Value;
                            string href;
                            //@"href=""(.*?)\"""
                            Match m2 = Regex.Match(a, @"href=""(.*?)\""", RegexOptions.IgnoreCase);
                            if (m2.Success)
                            {
                                href = m2.Groups[1].Value;
                                if (href.Contains("http"))
                                {
                                    if (href.Contains("php?"))
                                    {
                                        if (bunifuCustomTextbox1.Text.Contains(href))
                                        {

                                        }
                                        else
                                        {
                                            num = num - 1;
                                            number = number + 1;
                                            bunifuCustomLabel10.Text = num.ToString();
                                            bunifuCustomLabel13.Text =  number.ToString();
                                            bunifuCustomTextbox1.Text += href + "\r\n";
                                            File.WriteAllText(Path.Combine(apppath, "Result.txt"), bunifuCustomTextbox1.Text);
                                            bunifuCustomTextbox1.SelectionStart = bunifuCustomTextbox1.Text.Length;
                                            bunifuCustomTextbox1.ScrollToCaret();
                                            //textBox1.Text += "catch";             // ----------------- //

                                        }

                                    }
                                }

                            }
                        }
                    }
                    catch (Exception e)
                    {
                        ban = true;
                        errors = errors + 1;
                        bunifuCustomLabel15.Text =  errors.ToString();
                    }
                   
                       
                    

                }
                proxy.BaseStream.Position = 0;


            }
            catch (Exception)
            {
                //textBox1.Text += "catch";             // ----------------- //
            }

        }
    }
}
