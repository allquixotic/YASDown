YASDown - Yet Another Spotify Downloader
=========

YASDown is a flexible Spotify song downloader for Spotify Premium members.

###Features
  - Downloads Spotify songs to disk and encodes them in MP3 format. Just paste in the URI and go.
  - Works either as a console program, or as a GUI.
  - Remembers your settings and login information, so you don't have to always re-type your password.
  - Uses the officially supported `libspotify` from Spotify themselves for maximum possible future-proofing.
  - Cross-platform (Windows and modern desktop Linux, primarily)
  - Dual-interface: headless mode (no GUI, runs on server/VPS, scriptable in the shell) and GUI mode (for you point and clickers)
  - Support for optionally uploading downloaded files to an SFTP server using the SSH protocol (for backups, etc.)

###System Requirements
  - Microsoft .NET Framework 4.5 or Mono 3.2 or newer
  - Your ISP/Network must support the Spotify protocol (to test: if the Spotify desktop player works, you're good)
  - One of the [supported platforms for libspotify](https://developer.spotify.com/technologies/libspotify/#libspotify-downloads) which happens to also run a valid .NET implementation (see first bullet above)
  - Optional: An SSH/SFTP server for uploads. If you don't have an SFTP server and just want to download music, you can do that, too.

###User Requirements
  - You must have a Spotify Premium account. Note that "Unlimited" accounts are not supported. If you do not have a Spotify Premium account and are not willing to pay for one, **you will not be able to use this software.** I will not provide you access to my account, so don't ask.
  - You must have a Spotify Developer Key. I am not going to give you mine, so don't ask. Go [here](https://devaccount.spotify.com/my-account/keys/) to apply to get one (it's free, as of this writing, for Premium accounts.)
  - You must be able to follow basic instructions and understand generally how application programs work on whatever platform you're using. However, you do not have to be a programmer to use this program.

###Library Dependencies
Here are the third-party software components that YASDown depends on. These components are **not** distributed with the YASDown source code, so you will need to download them yourself.
  - [libspotify](https://developer.spotify.com/technologies/libspotify) 12.1.51 or later **native**
   - Note, Spotify sometimes breaks backwards compatibility with updates to libspotify, which will require code changes to YASDown, or downgrading your version of libspotify.
  - [SSH.NET](http://sshnet.codeplex.com/) (tested with verison 2013.4.7) **pure .NET**
  - [libmp3lame](http://lame.sourceforge.net/download.php) (tested with version 3.99.3) **native**
   - Windows Binaries: http://www.rarewares.org/mp3-lame-libraries.php
  - [CommandLineParser](http://commandline.codeplex.com/) (tested with version 1.9.71.2) **pure .NET**
  - [libspotifynet](http://libspotifydotnet.codeplex.com/) (tested with version 3.1) **P/Invoke glue for native libspotify**
  - [taglib-sharp](https://github.com/mono/taglib-sharp) (tested with version 2.1.0.0) **pure .NET**
   - Source and binary tarballs: http://download.banshee.fm/taglib-sharp/

###Technical Overview
  - Written in C#.NET, targeting Microsoft .NET Framework 4.5 and Mono 3.2 or later
  - Platforms: the goal is to support the latest Ubuntu release and latest three Windows desktop releases
   - Currently that's Ubuntu 13.10 and Windows 7/8/8.1
   - Ubuntu doesn't ship an up-to-date Mono, so user will need to download an updated version or compile from source
   - Recent OpenSUSE and Fedora should work too and will be considered for distro-specific fixes if there are problems
  - Bitness (32/64):
   - Only tested on the 32-bit .NET runtime on Windows, because there is no 64-bit Windows libspotify native DLL
   - Should work on either 32-bit or 64-bit Mono on GNU/Linux
  - It is within the realm of plausibility that this will run on top of Mono on iOS or Android, but don't be surprised if some changes are required.
  
###Compiling
1. Should be possible to compile this with Mono 3.2 or .NET Framework 4.5 or later.
2. To compile the project, first assemble the required DLLs (libmp3lame.dll, libspotify.dll, libspotifynet.dll, CommandLineParser.dll, taglib-sharp.dll). The pure .NET dependencies should always have the exact file names specified; however, on Mono on non-Windows platforms, the two native DLLs (libmp3lame.dll and libspotify.dll) can have the extensions .so (Linux) or .dylib (OS X/iOS).
3. You will need one of: MonoDevelop, Xamarin Studio, SharpDevelop, or Visual Studio 2012 or later. 
4. Open up the Solution file "YASDown.sln" and "Build" the project, either in Release or Debug mode as you prefer.
5. If you get build-time errors about the dependencies, make sure you have resolved all pure .NET references.
6. Copy the required native dependencies to the output directory if the build didn't do it for you.
7. Install your app key that you downloaded from Spotify as "spotify_appkey.key" in the same directory as yasdown.exe.
8. Run! If you specify any command line arguments, it will be invoked in CLI mode; if you omit command line arguments, it will be invoked in GUI mode.

###Binaries?
I will not provide publicly accessible binary downloads. However, if I know you, and ask you me very nicely, I may create a build for your platform and provide it to you individually. But since this build isn't rocket science, please try it on your own first.

###TO DO
 - Linux support needs love.
  - Need to write a dllmap in yasdown.exe.config for Mono.
  - Needs testing to make sure the WinForms code works, and probably fixes.
  - Needs testing on 32-bit and 64-bit Linux, and probably fixes.
 - Need to see if I can downgrade the runtime requirement to .NET 4.0 Client Profile for wider compatibility. Probably doable.
 - The guts of the SFTP support is not currently implemented, just stubbed out UI.
 - Program.cs needs major refactoring to move the engine out of the main class and decouple it from the GUI/CLI.
 - Probably need to implement our own mainloop and do all spotify API calls within that thread, to support both CLI and GUI equally well.
 - CLI mode is currently very unlikely to work well (or at all) even on Windows due to the GUI-first design so far.
 - Make an option for configuring Spotify download quality.
 - Make an option for configuring LAME encoding bitrate.
 - Better error handling.
 - Progress bar / status updates to both the CLI and GUI as the song is downloaded.

###Legal and Licensing
"This product uses SPOTIFY CORE but is not endorsed, certified or otherwise approved in any way by Spotify. Spotify is the registered trade mark of the Spotify Group."

Overall, this source code complies with the licenses set out by the components it depends on, and the source code of YASDown itself is licensed under the New BSD license.
For additional licensing details, see the file LICENSE.TXT in this repository.

    