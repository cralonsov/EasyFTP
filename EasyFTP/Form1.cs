using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyFTP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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

                    var myResult = new List<FtpDetails>();
                    foreach (var file in results)
                    {
                        string[] tokens = file.Split(new[] { ' ' }, 9, System.StringSplitOptions.RemoveEmptyEntries);
                        string fileTime = tokens[0];
                        string fileDate = tokens[1];
                        string fileType = tokens[2];
                        string fileName = tokens[3];

                        myResult.Add(new FtpDetails()
                        {
                            Time = fileTime,
                            Date = fileDate,
                            Type = fileType,
                            Name = fileName,
                            FullPath = CurrentRemoteDirectory + "/" + fileName,
                        });
                    }

                    return myResult;
                }
            }
        }

        private void showFiles(object sender, EventArgs e)
        {
            string ip = textBox_ip.Text;

            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(CreateDirectoryNode(ip, "Ftp Test"));
        }

        private TreeNode CreateDirectoryNode(string path, string name)
        {
            string user = textBox_user.Text;
            string pass = textBox_pass.Text;
            string ip = path;

            TreeNode directoryNode = new TreeNode(name);
            var fileList = GetFileList(user, pass, ip, "");

            var directories = fileList.Where(f => f.IsDirectory);
            var files = fileList.Where(f => !f.IsDirectory);

            foreach(var dir in directories)
            {
                directoryNode.Nodes.Add(CreateDirectoryNode(dir.FullPath, dir.Name));
            }
            foreach(var file in files)
            {
                directoryNode.Nodes.Add(new TreeNode(file.Name));
            }

            return directoryNode;
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
