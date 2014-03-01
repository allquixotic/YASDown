using System;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommandLine;
using CommandLine.Text;
using System.Runtime.InteropServices;
using System.Text;
using TagLib;
using TagLib.Id3v2;
using TagLib.Id3v1;
using libspotifydotnet;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using Renci.SshNet.Security;
using Renci.SshNet.Common;
using Renci.SshNet.Compression;

namespace YASDown
{
    class Options
    {
        [Option('u', "username", Required = false,
          HelpText = "Spotify Username")]
        public string Username { get; set; }

        [Option('p', "password", Required = false,
          HelpText = "Spotify Password")]
        public string Password { get; set; }

        [Option('u', "url", Required = true, HelpText = "Spotify URL of track")]
        public string URL { get; set; }

        [Option('d', "localfolder", Required = false, HelpText = "Local folder")]
        public string LocalFolder { get; set; }

        [Option('e', "sftpusername", Required = false, HelpText = "SFTP Username")]
        public string SftpUsername { get; set; }

        [Option('f', "sftppassword", Required = false, HelpText = "SFTP Password or Privatekey Password")]
        public string SftpPassword { get; set; }

        [Option('s', "sftpserver", Required = false, HelpText = "SFTP Server")]
        public string SftpServer { get; set; }

        [Option('o', "sftpport", Required = false, DefaultValue=null, HelpText = "SFTP Port")]
        public Nullable<Int32> SftpPort { get; set; }

        [Option('k', "sftpkey", Required = false, HelpText = "SFTP Private Key")]
        public string SftpKey { get; set; }

        [Option('z', "sftpremotepath", Required = false, HelpText = "SFTP remote server destination folder")]
        public string SftpPath { get; set; }

        [Option('a', "save", Required = false, DefaultValue=null, HelpText = "Save to config.bin")]
        public Nullable<Boolean> Save { get; set; }

        [Option('t', "usesftp", Required = false, DefaultValue=null, HelpText = "Use SFTP?")]
        public Nullable<Boolean> UseSftp { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }


    static class Program
    {
        public static Form1 frm = null;
        public static bool audioStreamComplete = false;
        public static MemoryStream audioBuf = new MemoryStream();
        public static libspotify.sp_audioformat staticfmt;
        public static FileInfo outFile = null;
        static string _artist = null;
        static string _album = null;
        static string _song = null;
        static LibMp3Lame lame = new LibMp3Lame();
        static bool firstDelivery = true;
        static FileStream outStream = null;
        static AppConfig _confo = null;

        [STAThread]
        static void Main(string[] args)
        {
            Session.OnAudioDataArrived += Session_OnAudioDataArrived;
            Session.OnAudioStreamComplete += Session_OnAudioStreamComplete;
            try
            {
                Session.appkey = System.IO.File.ReadAllBytes(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "spotify_appkey.key"));
            }
            catch{}

            bool gui = (args == null || args.Length == 0);
            Log.gui = gui;

            if(Session.appkey == null || Session.appkey.Length == 0)
            {
                Log.Error("Error: Can't find app key file spotify_appkey.key!");
            }
            else
            {
                lame.LameInit();
                if (gui)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    frm = new Form1(Session.Login());
                    Application.Idle += Application_Idle;
                    Application.Run(frm);
                }
                else
                {
                    DoConsole(args);
                }
            }
        }

        static void Session_OnAudioStreamComplete(object obj)
        {
            if (audioStreamComplete)
            {
                Log.Debug("OnAudioStreamComplete AGAIN; deja vu!");
                return;
            }
            audioStreamComplete = true;
            Log.Debug("OnAudioStreamComplete!");
            Log.Debug("Sample rate: " + staticfmt.sample_rate);
            Log.Debug("Num channels: " + staticfmt.channels);
            Log.Debug("Format: " + staticfmt.sample_type);
            Log.Debug("MP3 encoding complete!");

            byte[] mymp3Out = new byte[64000];
            int outSize;
            outSize = lame.LameEncodeFlush(mymp3Out);
            outStream.Write(mymp3Out, 0, outSize);
            outStream.Flush();
            outStream.Close();
            Log.Debug("File written to disk! Trying to write tags...");
            TagLib.File tf = TagLib.File.Create(outFile.FullName);
            tf.Tag.Album = _album;
            tf.Tag.AlbumArtists = new string[1] { _artist };
            tf.Tag.Title = _song;
            tf.Save();
            Session.Pause();
            Session.UnloadPlayer();
            Log.Debug("Tags written! Finished!");
            new SftpUploader(_confo).go(outFile.FullName);
        }

        static byte[] mp3Out = new byte[10000000];
        static void Session_OnAudioDataArrived(libspotify.sp_audioformat fmt, byte[] obj, int num_frame)
        {
            if(num_frame == fmt.sample_rate)
            {
                Log.Debug("IT'S A FAAAAKE!");
                Session.Pause();
                Session.UnloadPlayer();
            }
            if (audioStreamComplete)
                return;

            staticfmt = fmt;

            //Log.Debug("Got " + num_frame * staticfmt.channels + " samples");

            if(firstDelivery)
            {
                lame.LameSetInSampleRate(staticfmt.sample_rate);
                lame.LameSetNumChannels(staticfmt.channels);
                lame.LameSetBRate(192);
                lame.LameInitParams();
                outStream = outFile.OpenWrite();
            }

            short[] left = new short[obj.Length / 4],
                right = new short[obj.Length / 4];
            byte[] fourbytes = new byte[4];
            for (int i = 0; i < obj.Length; i += 4)
            {
                left[i / 4] = BitConverter.ToInt16(new byte[2] { obj[i], obj[i+1] }, 0);
                right[i / 4] = BitConverter.ToInt16(new byte[2] { obj[i+2], obj[i+3] }, 0);
            }
            int outSize = 0;
            if ((outSize = lame.LameEncodeBuffer(left, right, left.Length, mp3Out)) == 0)
            {
                audioStreamComplete = true;
                Log.Debug("Early exit because LAME couldn't encode the last bits");
                outSize = lame.LameEncodeFlush(mp3Out);
                outStream.Write(mp3Out, 0, outSize);
                outStream.Flush();
                outStream.Close();
                Log.Debug("File written to disk! Trying to write tags...");
                TagLib.File tf = TagLib.File.Create(outFile.FullName);
                tf.Tag.Album = _album;
                tf.Tag.AlbumArtists = new string[1] { _artist };
                tf.Tag.Title = _song;
                tf.Save();
                Session.Pause();
                Session.UnloadPlayer();
                Log.Debug("Tags written! Finished!");
                new SftpUploader(_confo).go(outFile.FullName);
                return;
            }

            outStream.Write(mp3Out, 0, outSize);

            firstDelivery = false;
        }

        static void Application_Idle(object sender, EventArgs e)
        {
            int timeout;
            libspotify.sp_session_process_events(Session.GetSessionPtr(), out timeout);
        }

        static void DoConsole(string[] args)
        {
            var options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                if(options.URL == null || options.URL.Length == 0)
                {
                    Log.Error("Error: No URL specified");
                    return;
                }

                if(!Session.Login(options.Username, options.Password))
                {
                    Log.Error("Error: Bad credentials");
                    return;
                }

                AppConfig config = AppConfig.Load();
                if(config == null)
                {
                    config = new AppConfig();
                }

                if(options.UseSftp != null)
                    config.sftpEnabled = options.UseSftp.Value;

                if (options.LocalFolder != null)
                    config.localBaseFolder = options.LocalFolder;

                if (options.SftpUsername != null)
                    config.sftpUsername = options.SftpUsername;

                if (options.SftpPassword != null)
                    config.sftpPassword = options.SftpPassword;

                if (options.SftpPort != null)
                    config.sftpPort = options.SftpPort.Value;

                if (options.SftpServer != null)
                    config.sftpRemoteServer = options.SftpServer;

                if (options.SftpKey != null)
                    config.sftpPrivateKeyPath = options.SftpKey;

                if (options.SftpPath != null)
                    config.sftpRemoteServer = options.SftpPath;

                if (options.Save == true)
                    config.Save();

                Download(options.URL, config);
            }
        }

        public static void Download(string url, AppConfig config)
        {
            if (url != null && url.Length > 0)
            {
                IntPtr sess = Session.GetSessionPtr();
                IntPtr splink = libspotify.sp_link_create_from_string(url);
                if (libspotify.sp_link_type(splink) == libspotify.sp_linktype.SP_LINKTYPE_TRACK)
                {
                    Log.Debug("Starting work on track " + url);
                    IntPtr sptrack = libspotify.sp_link_as_track(splink);
                    IntPtr sptrack2 = libspotify.sp_track_get_playable(sess, sptrack);
                    if (Session.LoadPlayer(sptrack2) != libspotify.sp_error.OK)
                    {
                        Log.Error("Unable to load track player for " + url);
                    }
                    else
                    {
                        audioStreamComplete = false;
                        firstDelivery = true;
                        string _artist = Utils.Utf8ToString(libspotify.sp_artist_name(libspotify.sp_track_artist(sptrack2, 0)));
                        string _album = Utils.Utf8ToString(libspotify.sp_album_name(libspotify.sp_track_album(sptrack2)));
                        string _song = Utils.Utf8ToString(libspotify.sp_track_name(sptrack2));
                        Log.Debug("Artist: " + _artist + "; Album: " + _album + "; Song: " + _song);
                        outFile = new FileInfo(Path.Combine(config.localBaseFolder, _artist, _album, _artist + " - " + _song + ".mp3"));
                        Log.Debug("Making directory " + Path.GetDirectoryName(outFile.FullName));
                        Directory.CreateDirectory(Path.GetDirectoryName(outFile.FullName));
                        Session.Play();
                        Log.Debug("Data is streaming!");
                        if (outFile.Exists)
                            outFile.Delete();
                        _confo = config;
                    }
                }
                else
                {
                    Log.Error("Error: Invalid link type!");
                }
            }
        }
    }
}
