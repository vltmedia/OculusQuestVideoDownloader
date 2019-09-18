using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Management;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Oculus_Quest_Video_Downloader
{
    public partial class Form1 : Form
    {
        public string Location = @"Quest\Internal shared storage\Oculus\VideoShots";
        public string[] VideoFiles;
        public string commandd = "adb pull /sdcard/oculus/VideoShots \"LOCATION\"" + Environment.NewLine + "Exit" + Environment.NewLine  ;
        public string commanddsingle = "adb pull /sdcard/oculus/VideoShots/REPFILE \"LOCATION\"" + Environment.NewLine + "Exit" + Environment.NewLine  ;

        public void SendCommand()
        {
            string comm = commandd.Replace("LOCATION", Location);
            Console.WriteLine(checkBox1.CheckState);

            if (checkBox1.CheckState == CheckState.Unchecked)
            {
                string filn = textBox2.Text;
                if (!filn.Contains(".mp4"))
                {
                    filn = textBox2.Text + ".mp4";
                }

                comm = commanddsingle.Replace("LOCATION", Location).Replace("REPFILE", filn);
                Console.WriteLine(comm);
            }
            else
            {
                comm = commandd.Replace("LOCATION", Location);
                Console.WriteLine(checkBox1.CheckState);


            }
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + comm;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            process.Close();
            MessageBox.Show("Completed Video Pull");

        }

        public void ProcessFiles()
        {
            try
            {
                string comm = commandd.Replace("LOCATION", Location);
                Console.WriteLine(checkBox1.CheckState);

                if (checkBox1.CheckState == CheckState.Unchecked) {
                    string filn = textBox2.Text;
                    if (!filn.Contains(".mp4")) { 
                    filn = textBox2.Text + ".mp4";
                    }

                    comm = commanddsingle.Replace("LOCATION", Location).Replace("REPFILE", filn);
                    Console.WriteLine(comm);
                }
                else
                {
                     comm = commandd.Replace("LOCATION", Location);
                    Console.WriteLine(checkBox1.CheckState);


                }
                var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "cmd.exe",
                            UseShellExecute = true,
                            RedirectStandardInput = true,
                            //CreateNoWindow = true,
                            WindowStyle = ProcessWindowStyle.Normal
                        }
                    };
                    Console.WriteLine(comm);
                    process.Start();
                    // For output data directly from command line.
           
                    // For error output directly from command line.
                    //process.ErrorDataReceived += errorReceivedHandler;
                    //process.BeginErrorReadLine();

                    // For providing an command to command line.
                    var command = comm;
                    var writer = process.StandardInput;
                    writer.WriteLine(command);
                    //Thread.Sleep(3000);
                    process.WaitForExit();
                    process.Close();
          

            }
            catch
            {

            }


        }
        public void FindQuest()
        {

            //Dealing with them in terms of them being removable disks is relatively easy, firstly finding the removable disks using DriveInfo

            // Find removable disks
            var drives = DriveInfo.GetDrives().Where(x => x.DriveType == DriveType.Removable && x.IsReady);

            List<FileInfo[]> pictureHandles = new List<FileInfo[]>();

            // Find all files of a certain type on each drive
            foreach (var drive in drives)
            {
                pictureHandles.Add(
                            new DirectoryInfo(drive.Name)
                            .GetFiles("*.jpg", SearchOption.AllDirectories)
                            .ToArray());
            }
            //var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_USB");


            // Do stuff with pictureHandles and identify


        }
        public void GetFirstFiles()
        {
            try { 
            VideoFiles = Directory.GetFiles(Location);
                foreach(var f in VideoFiles)
                {
                    listBox1.Items.Add(Path.GetFileName(f));


                }
            }
            catch
            {

                MessageBox.Show("Oculus Quest is not Connected");
            }
        }

        public Form1()
        {
            InitializeComponent();
        }
    
        private void button1_Click(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
               if(textBox2.Text == "")
                {

                    MessageBox.Show("Please enter in a file name to pull or check off All");
                }
                else
                {
                    Oculus_Quest_Video_Downloader.Properties.Settings.Default.ExportsFolder = textBox1.Text;
                    Oculus_Quest_Video_Downloader.Properties.Settings.Default.Save();
                    Location = textBox1.Text;
                    SendCommand();

                }
               
            }
            else
            {
                Oculus_Quest_Video_Downloader.Properties.Settings.Default.ExportsFolder = textBox1.Text;
            Oculus_Quest_Video_Downloader.Properties.Settings.Default.Save();
            Location = textBox1.Text;
            SendCommand();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text =  Oculus_Quest_Video_Downloader.Properties.Settings.Default.ExportsFolder ;
            if (checkBox1.Checked)
            {
                button1.Text = "Pull Video Files";
            }
            else
            {

                button1.Text = "Pull Video File";

            }
            //GetFirstFiles();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) {
                button1.Text = "Pull Video Files";
            }
            else
            {

                button1.Text = "Pull Video File";

            }
        }
    }
}
