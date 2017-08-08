using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyFTP
{
    public partial class Form1 : Form
    {
        static string downloadFtpPath = string.Empty;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            downloadToolStripMenuItem.Text = Strings.DownloadFiles;
        }

        private static FtpWebRequest GetWebRequest(string user, string pass, string uri, string method)
        {
            Uri serverUri = new Uri(uri);
            if (serverUri.Scheme != Uri.UriSchemeFtp)
            {
                return null;
            }

            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)WebRequest.Create(serverUri);
            reqFTP.UseBinary = true;
            reqFTP.Credentials = new NetworkCredential(user, pass);
            reqFTP.Method = method;
            reqFTP.Proxy = null;
            reqFTP.KeepAlive = false;
            reqFTP.UsePassive = false;

            return reqFTP;
        }


        private static IEnumerable<FtpDetails> GetFileList(string user, string pass, string rootUri, string folder)
        {
            var CurrentRemoteDirectory = rootUri;
            StringBuilder result = new StringBuilder();

            if (!folder.EndsWith("/") && folder.Length > 0)
                folder += "/";

            FtpWebRequest request = GetWebRequest(user, pass, CurrentRemoteDirectory, WebRequestMethods.Ftp.ListDirectoryDetails);

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string line = reader.ReadLine();

                    while (line != null)
                    {
                        result.Append(line);
                        result.Append("\n");
                        line = reader.ReadLine();
                    }

                    result.Remove(result.ToString().LastIndexOf('\n'), 1);
                    var results = result.ToString().Split('\n');

                    var ftpDetails = new List<FtpDetails>();
                    foreach (var file in results)
                    {
                        string[] tokens = file.Split(new[] { ' ' }, 9, System.StringSplitOptions.RemoveEmptyEntries);
                        string fileTime = tokens[0];
                        string fileDate = tokens[1];
                        string fileType = tokens[2];
                        string fileName = tokens[3];

                        ftpDetails.Add(new FtpDetails()
                        {
                            Time = fileTime,
                            Date = fileDate,
                            Type = fileType,
                            Name = fileName,
                            FullPath = CurrentRemoteDirectory + "/" + fileName,
                        });
                    }

                    return ftpDetails;
                }
            }
        }

        static void DownloadFtpDirectory(string user, string pass, string uri, string localPath)
        {
            FtpWebRequest request = GetWebRequest(user, pass, uri, WebRequestMethods.Ftp.ListDirectoryDetails);

            List<string> lines = new List<string>();

            using (FtpWebResponse listResponse = (FtpWebResponse)request.GetResponse())
            using (Stream listStream = listResponse.GetResponseStream())
            using (StreamReader listReader = new StreamReader(listStream))
            {
                while (!listReader.EndOfStream)
                {
                    lines.Add(listReader.ReadLine());
                }
            }

            foreach (string line in lines)
            {
                // This gives problems when the folder/file has spaces in its name
                string[] tokens = line.Split(new[] { ' ' }, 9, System.StringSplitOptions.RemoveEmptyEntries);
                string name = tokens[3];
                string type = tokens[2];

                string localFilePath = Path.Combine(localPath, name);

                // This gives problems when trying to download a single file
                string fileUrl = fileUrl = uri + "/" + name; 

                if (type == "<DIR>")
                {
                    if (!Directory.Exists(localFilePath))
                    {
                        Directory.CreateDirectory(localFilePath);
                    }

                    DownloadFtpDirectory(user, pass, fileUrl + "/", localFilePath);
                }

                else
                {
                    if (!File.Exists(localFilePath))
                    {
                        FtpWebRequest downloadRequest = GetWebRequest(user, pass, fileUrl, WebRequestMethods.Ftp.DownloadFile);

                        using (FtpWebResponse downloadResponse = (FtpWebResponse)downloadRequest.GetResponse())
                        using (Stream sourceStream = downloadResponse.GetResponseStream())
                        using (Stream targetStream = File.Create(localFilePath))
                        {
                            byte[] buffer = new byte[10240];
                            int read;
                            while ((read = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                targetStream.Write(buffer, 0, read);
                            }
                        }
                    }
                }
            }
        }

        private void showFiles(object sender, EventArgs e)
        {
            string uri = textBox_uri.Text;

            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(CreateDirectoryNode(uri, uri));
        }

        private TreeNode CreateDirectoryNode(string path, string name)
        {
            string user = textBox_user.Text;
            string pass = textBox_pass.Text;
            string uri = path;

            TreeNode directoryNode = new TreeNode(name);
            var fileList = GetFileList(user, pass, uri, "");

            var directories = fileList.Where(f => f.IsDirectory);
            var files = fileList.Where(f => !f.IsDirectory);

            foreach(var dir in directories)
            {
                directoryNode.Nodes.Add(CreateDirectoryNode(dir.FullPath, dir.Name));
            }
            foreach (var file in files)
            {
                directoryNode.ImageIndex = 0;
                directoryNode.SelectedImageIndex = 0;
                directoryNode.Nodes.Add(new TreeNode(file.Name));
                
            }

            return directoryNode;
        }
        
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView1.SelectedNode = e.Node;

            if (e.Button == MouseButtons.Right)
            {
                if(treeView1.SelectedNode != null)
                {
                    downloadFtpPath = treeView1.GetNodeAt(e.Location).FullPath;
                    downloadFtpPath = downloadFtpPath.Replace('\\', '/');
                }
            }
        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string user = textBox_user.Text;
            string pass = textBox_pass.Text;
            string uri = textBox_uri.Text;

            folderBrowserDialog1.ShowDialog();
            string localPath = folderBrowserDialog1.SelectedPath;

            if(localPath != null)
            {
                DownloadFtpDirectory(user, pass, downloadFtpPath, localPath);
            }
        }
    }

    public class FtpDetails
    {
        public bool IsDirectory
        {
            get
            {
                return Type.Equals("<DIR>");
            }
        }

        internal string Type { get; set; }
        public string Name { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }

        public string FullPath { get; set; }
    }
}
