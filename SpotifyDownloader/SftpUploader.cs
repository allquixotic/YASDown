using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YASDown
{
    class SftpUploader
    {
        private ConnectionInfo connInfo = null;
        private AppConfig config = null;

        public SftpUploader(AppConfig conf)
        {
            this.config = conf;
            if (config.sftpEnabled)
            {
                AuthenticationMethod[] auths = new AuthenticationMethod[1];
                if (String.IsNullOrWhiteSpace(config.sftpPrivateKeyPath))
                {
                    auths[0] = new PasswordAuthenticationMethod(config.sftpUsername, Utils.GetBytes(config.sftpPassword));
                }
                else
                {
                    try
                    {
                        PrivateKeyFile pkf = null;
                        if (string.IsNullOrEmpty(config.sftpPassword))
                        {
                            pkf = new PrivateKeyFile(config.sftpPrivateKeyPath);
                        }
                        else
                        {
                            pkf = new PrivateKeyFile(config.sftpPrivateKeyPath, config.sftpPassword);
                        }
                        auths[0] = new PrivateKeyAuthenticationMethod(config.sftpUsername, pkf);
                    }
                    catch (IOException)
                    {
                        Log.Error("Unable to read private key file: " + config.sftpPrivateKeyPath);
                        return;
                    }
                }
                connInfo = new ConnectionInfo(config.sftpRemoteServer, config.sftpUsername, auths);
            }
        }

        public void go(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename) || connInfo == null || config.sftpEnabled == false)
            {
                Log.Debug("Not doing SFTP because it wasn't configured properly or at all");
                return;
            }
            
            FileInfo fi = new FileInfo(filename);
            if(!fi.Exists)
            {
                Log.Error("Can't open file for SFTPing: " + filename);
                return;
            }

            if(!fi.DirectoryName.StartsWith(config.localBaseFolder))
            {
                Log.Error("Can't figure out where the file " + filename + " is relative to the base dir");
                return;
            }

            string rel = fi.DirectoryName.Replace(config.localBaseFolder, "");
            if (rel.StartsWith(Path.DirectorySeparatorChar.ToString()))
                rel = rel.Substring(1);

            SftpClient client = new SftpClient(connInfo);
            string accum = "";
            try
            {
                client.Connect();
                string thedir = null;
                foreach (string str in rel.Split(Path.DirectorySeparatorChar))
                {
                    accum = accum + "/" + str;
                    thedir = config.sftpRemoteFolder + "/" + accum;
                    thedir = thedir.Replace("//", "/");
                    Log.Debug("Trying to create directory " + thedir);
                    try
                    {
                        client.CreateDirectory(thedir);
                    }
                    catch (SshException) { }
                }
                FileStream fis = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    client.BeginUploadFile(fis, thedir + "/" + fi.Name, true, (fini) => 
                    {
                        FileStream ffini = fini.AsyncState as FileStream;
                        if (ffini != null)
                            ffini.Close();
                        if (client != null && client.IsConnected)
                        {
                            client.Disconnect();
                        }
                        Log.Debug("Upload finished!");
                        if(Program.frm != null)
                            Program.frm.SetStatus("Upload finished! / Ready");
                    }, fis, (pct) => 
                    {  
                        if(Program.frm != null)
                        {
                            Program.frm.SetStatus("Uploaded " + pct.ToString() + " bytes");
                        }
                    });
            }
            catch(Exception aiee)
            {
                Log.Error("Error: " + aiee.Message);
                Log.Debug(aiee.StackTrace);
            }
        }
    }
}
