using System;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
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
using Nito.Async;

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

        [Option('r', "url", Required = true, HelpText = "Spotify URL of track")]
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

        [Option('b', "spotbitrate", Required = false, DefaultValue = 160, HelpText = "Spotify download bitrate: 64, 160, or 320")]
        public int SpotifyBitrate { get; set; }

        [Option('l', "lamebitrate", Required = false, DefaultValue = 192, HelpText = "LAME encode bitrate")]
        public int LameBitrate { get; set; }

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
        public static readonly ActionThread nito = new ActionThread();
        public static Nito.Async.Timer timer = null;
        public static bool audioStreamComplete = false;
        public static MemoryStream audioBuf = new MemoryStream();
        public static libspotify.sp_audioformat staticfmt;
        public static FileInfo outFile = null;
        static string _artist = null;
        static string _album = null;
        static string _song = null;
        static LibMp3Lame lame = new LibMp3Lame();
        static FileStream outStream = null;
        static AppConfig _confo = null;
        static DateTime when = DateTime.Now;
        static MemoryStream buf = new MemoryStream(10000000);
        static byte[] mp3Out = new byte[100000000];

        [STAThread]
        static void Main(string[] args)
        {
            nito.Start();
            nito.DoSynchronously(() =>
            {
                timer = new Nito.Async.Timer();
                timer.AutoReset = false;
                timer.Elapsed += timer_Elapsed;
                Session.OnAudioDataArrived += Session_OnAudioDataArrived;
                Session.OnAudioStreamComplete += Session_OnAudioStreamComplete;
                Session.OnNotifyMainThread += Session_OnNotifyMainThread;
                try
                {
                    Session.appkey = System.IO.File.ReadAllBytes(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "spotify_appkey.key"));
                }
                catch { }

                bool gui = (args == null || args.Length == 0);
                Log.gui = gui;

                if (Session.appkey == null || Session.appkey.Length == 0)
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
                    }
                    else
                    {
                        DoConsole(args);
                    }
                }
            });
            if (Log.gui)
                Application.Run(frm);
        }

        static void timer_Elapsed()
        {
            Log.Debug("Timer fired");
            int timeout = 0;
            if (Session.GetSessionPtr() != null)
            {
                do
                {
                    libspotify.sp_session_process_events(Session.GetSessionPtr(), out timeout);
                } while (timeout == 0);
                timer.SetSingleShot(TimeSpan.FromMilliseconds(timeout > 0 ? timeout : 1000));
                timer.Enabled = true;
            }
        }

        static void Session_OnNotifyMainThread(IntPtr obj)
        {
            nito.Do(() =>
                {
                    Log.Debug("Notify called!");
                    timer_Elapsed();
                });
        }

        static void Session_OnAudioStreamComplete(object obj)
        {
            nito.Do(() =>
                {
                    if (audioStreamComplete)
                    {
                        Log.Debug("OnAudioStreamComplete AGAIN; deja vu!");
                        return;
                    }
                    audioStreamComplete = true;
                    Log.Debug("OnAudioStreamComplete!");

                    FinishEncode();
                });

        }

        static void FinishEncode()
        {
            audioStreamComplete = true;
            Session.Pause();
            Session.UnloadPlayer();
            lame.LameSetInSampleRate(staticfmt.sample_rate);
            lame.LameSetNumChannels(staticfmt.channels);
            lame.LameSetBRate(_confo.lameBitrate);
            Log.Debug("Encoding at " + _confo.lameBitrate + "kbps");
            lame.LameInitParams();
            outStream = outFile.OpenWrite();

            short[] left = new short[buf.Length / 4],
                right = new short[buf.Length / 4];
            byte[] fourbytes = new byte[4];
            for (int i = 0; i < buf.Length; i += 4)
            {
                buf.Read(fourbytes, 0, 4);
                left[i / 4] = BitConverter.ToInt16(new byte[2] { fourbytes[0], fourbytes[1] }, 0);
                right[i / 4] = BitConverter.ToInt16(new byte[2] { fourbytes[2], fourbytes[3] }, 0);
            }

            int outSize = lame.LameEncodeBuffer(left, right, left.Length, mp3Out);
            outStream.Write(mp3Out, 0, outSize);
            outSize = lame.LameEncodeFlush(mp3Out);
            outStream.Write(mp3Out, 0, outSize);
            outStream.Flush();
            outStream.Close();
            Log.Debug("File written to disk! Trying to write tags...");
            TagLib.File tf = TagLib.File.Create(outFile.FullName);
            tf.Tag.Album = _album;
            tf.Tag.AlbumArtists = null;
            tf.Tag.AlbumArtists = new string[1] { _artist };
            tf.Tag.Title = _song;
            tf.Tag.Performers = null;
            tf.Tag.Performers = new string[1] { _artist };
            tf.Tag.Artists = null;
            tf.Tag.Artists = new string[1] { _artist };
            tf.Save();
            Log.Debug("Tags written! Finished!");
            if (frm != null)
            {
                frm.BeginInvoke((Delegate) new MethodInvoker(() => frm.SetStatus("Download finished")));
            }
            new SftpUploader(_confo).go(outFile.FullName);
        }

        static void Session_OnAudioDataArrived(libspotify.sp_audioformat fmt, byte[] obj, int num_frame)
        {
            if (audioStreamComplete)
                return;

            if (num_frame == 22050 || num_frame > 44100 || num_frame == 0)
            {
                //This isn't normal. It's trying to write some kind of silence at the end of the file.
                audioStreamComplete = true;
                staticfmt = fmt;
                FinishEncode();
                return;
            }

            buf.Write(obj, 0, obj.Length);
            if (frm != null)
            {
                frm.BeginInvoke((Delegate)new MethodInvoker(() => frm.SetStatus("Fetched " + buf.Length + " bytes")));
            }
        }

        static void DoConsole(string[] args)
        {
            nito.Do(() =>
            {
                var options = new Options();
                if (CommandLine.Parser.Default.ParseArguments(args, options))
                {
                    if (options.URL == null || options.URL.Length == 0)
                    {
                        Log.Error("Error: No URL specified");
                        return;
                    }

                    if (!Session.Login(options.Username, options.Password))
                    {
                        Log.Error("Error: Bad credentials");
                        return;
                    }

                    AppConfig config = AppConfig.Load();
                    if (config == null)
                    {
                        config = new AppConfig();
                    }

                    if (options.UseSftp != null)
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

                    config.spotifyBitrate = options.SpotifyBitrate;
                    config.lameBitrate = options.LameBitrate;

                    if (options.Save == true)
                        config.Save();

                    Download(options.URL, config);
                }
            });
        }

        public static void Download(string url, AppConfig config)
        {
            nito.Do(() =>
            {
                _confo = config;
                if (frm != null)
                    frm.SetStatus("Download in progress...");
                if (url != null && url.Length > 0)
                {
                    IntPtr sess = Session.GetSessionPtr();
                    libspotify.sp_bitrate br;
                    int delta96 = Math.Abs(config.spotifyBitrate - 96),
                        delta160 = Math.Abs(config.spotifyBitrate - 160),
                        delta320 = Math.Abs(config.spotifyBitrate - 320);
                    br = libspotify.sp_bitrate.BITRATE_96k;
                    if (delta160 < delta96)
                        br = libspotify.sp_bitrate.BITRATE_160k;
                    if (delta320 < delta160)
                        br = libspotify.sp_bitrate.BITRATE_320k;

                    Log.Debug("Set download bitrate to " + br.ToString());
                    libspotify.sp_session_preferred_bitrate(sess, br);
                    IntPtr splink = Functions.StringToLinkPtr(url);
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
                            _artist = Utils.Utf8ToString(libspotify.sp_artist_name(libspotify.sp_track_artist(sptrack2, 0))).Replace(":", "");
                            _album = Utils.Utf8ToString(libspotify.sp_album_name(libspotify.sp_track_album(sptrack2))).Replace(":", "");
                            _song = Utils.Utf8ToString(libspotify.sp_track_name(sptrack2)).Replace(":", "");
                            Log.Debug("Artist: " + _artist + "; Album: " + _album + "; Song: " + _song);
                            outFile = new FileInfo(Path.Combine(config.localBaseFolder, _artist, _album, _artist + " - " + _song + ".mp3"));
                            if(outFile.Exists)
                            {
                                new SftpUploader(_confo).go(outFile.FullName);
                            }
                            else
                            {
                                Log.Debug("Making directory " + Path.GetDirectoryName(outFile.FullName));
                                Directory.CreateDirectory(Path.GetDirectoryName(outFile.FullName));
                                libspotify.sp_session_player_prefetch(Session.GetSessionPtr(), sptrack2);
                                Session.Play();
                                Log.Debug("Data is streaming!");
                                //if (outFile.Exists)
                                //    outFile.Delete();
                            }
                        }
                    }
                    else
                    {
                        Log.Error("Error: Invalid link type!");
                    }
                }
            });
        }
    }
}
