using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace YASDown
{
    [Serializable()]
    public class AppConfig
    {
        public string localBaseFolder { get; set; }
        public bool sftpEnabled { get; set; }
        public string sftpRemoteServer { get; set; }
        public int sftpPort { get; set; }
        public string sftpUsername { get; set; }
        public string sftpPassword { get; set; }
        public string sftpPrivateKeyPath { get; set; }
        public string sftpRemoteFolder { get; set; }
        [DefaultValue(160)]
        public int spotifyBitrate { get; set; }
        [DefaultValue(192)]
        public int lameBitrate { get; set; }

        [NonSerialized]
        public static readonly string configbin = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "config.bin");

        public void Save()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using(Stream sr = new FileInfo(configbin).OpenWrite())
            {
                formatter.Serialize(sr, this);
            }
        }

        public static AppConfig Load()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileInfo fi = new FileInfo(configbin);
            Stream sr = null;
            try
            {
                sr = fi.OpenRead();
                AppConfig config = (AppConfig)formatter.Deserialize(sr);
                return config;
            }
            catch(Exception){}
            finally
            {
                if(sr != null)
                    sr.Dispose();
            }
            return null;
        }
    }
}
