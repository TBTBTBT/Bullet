using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Drive.v3;
using NAudio.Wave;
using File = Google.Apis.Drive.v3.Data.File;
namespace GDAP
{
    public enum TrackState
    {
        None,
        Loading,
        Playable,
        
    }

    public enum ExtensionType
    {
        None,
        MP3,
        WAV,
        M4A,
    }
    
    sealed class Track
    {
        public File FileCache;
        public MemoryStream Data = new MemoryStream();
        public TrackState State;
        public ExtensionType Extension;
    }
    sealed class AudioPlayer
    {
        public static Dictionary<string, ExtensionType> StringToExtension = new Dictionary<string, ExtensionType>()
        {
            {".MP3",ExtensionType.MP3},
            {".WAV",ExtensionType.WAV},
            {".M4A",ExtensionType.M4A }
        };
        private IWavePlayer _waveOut;
        private Track currentTrack = new Track();
        private Track nextTrack = new Track();
        public static bool WindowClosed = false;
        private bool _stop = false;
        public event EventHandler OnEndPlaying;

        public File Current => currentTrack.FileCache;
        public File Next => nextTrack.FileCache;
        /// <summary>
        /// 再生を開始します。
        /// </summary>
        public void Play()
        {
            if (_waveOut == null)
            {
                IsPlaying = true;
                _stop = false;
                new Thread(delegate (object o)
                {
                    
                    while (currentTrack.State != TrackState.Playable)
                    {
                        Thread.Sleep(100);
                    }

                    switch (currentTrack.Extension)
                    {
                        case ExtensionType.MP3:
                            PlayMp3();
                            break;
                        case ExtensionType.WAV:
                            PlayWav();
                            break;
                        case ExtensionType.M4A:
                            break;
                        default:
                            break;
                            
                    }
                    
                    IsPlaying = false;
                    currentTrack.Data.Dispose();
                    
                    if (!WindowClosed)
                    {
                        currentTrack = new Track();
                        if (nextTrack.State == TrackState.Playable)
                        {
                            currentTrack = nextTrack;
                            nextTrack = new Track();

                        }
                        _waveOut = null;
                        OnEndPlaying.Invoke(null, null);
                    }
                   
                }).Start();
            }

            //byte[] buffer = new byte[65536]; // 64KB chunks

            //int read;
            //    破棄されてる


            //while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            //{
            //    var pos = ms.Position;
            //    ms.Position = ms.Length;
            //    ms.Write(buffer, 0, read);
            //    ms.Position = pos;
            //}

            // Pre-buffering some data to allow NAudio to start playing
            //while (ms.Length < 65536 * 10)
            //    Thread.Sleep(1000);

            //ms.Position = 0;

        }

        private void PlayWav()
        {
            try
            {
                using (WaveFileReader reader = new WaveFileReader(currentTrack.Data))
                using (WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(reader))
                using (WaveStream blockAlignedStream = new BlockAlignReductionStream(pcm))
                {
                    using (_waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
                    {
                        _waveOut.Init(blockAlignedStream);

                        _waveOut.Play();
                        while (
                            !WindowClosed &&
                            !_stop &&
                            _waveOut != null &&
                            _waveOut.PlaybackState == PlaybackState.Playing)
                        {
                            System.Threading.Thread.Sleep(100);
                        }

                        _waveOut.Stop();

                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        private void PlayMp3()
        {
            try
            {


                using (Mp3FileReader reader = new Mp3FileReader(currentTrack.Data))
                using (WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(reader))
                using (WaveStream blockAlignedStream = new BlockAlignReductionStream(pcm))
                {
                    using (_waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
                    {
                        _waveOut.Init(blockAlignedStream);

                        _waveOut.Play();
                        while (
                            !WindowClosed &&
                            !_stop &&
                            _waveOut != null &&
                            _waveOut.PlaybackState == PlaybackState.Playing)
                        {
                            System.Threading.Thread.Sleep(100);
                        }

                        _waveOut.Stop();

                    }
                }
            }
            catch (Exception e)
            {

            }

        }
        /// <summary>
        /// 再生を一時停止します。
        /// </summary>
        public void Pause()
        {
            this._waveOut.Pause();
        }

        public bool NextExists()
        {
            //再生終了直後にnextがcurに入る
            return currentTrack.State == TrackState.Playable;
        }


        public void OnClose()
        {
            currentTrack?.Data?.Dispose();
            nextTrack?.Data?.Dispose();
        }
        public bool IsPlaying { get; private set; } = false;
        public void SetCurrent(File file,string ext)
        {
            if (IsPlaying)
            {
                return;
                
            }

            if (StringToExtension.ContainsKey(ext))
            {
                currentTrack.Extension = StringToExtension[ext];
            }
            currentTrack.FileCache = file;
            currentTrack.State = TrackState.Loading;
            currentTrack.Data = new MemoryStream();
            new Thread(delegate(object o)
            {
                //var response = WebRequest.Create(url).GetResponse();
                using (var stream = GoogleDriveApiManager.GetAudioFile(file.Id))//response.GetResponseStream())
                {
                    byte[] buffer = new byte[65536]; // 64KB chunks
                    stream.Position = 0;
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        currentTrack.Data.Position = currentTrack.Data.Length;
                        currentTrack.Data.Write(buffer, 0, read);
                    }
                }
                //int size = 65536;

                currentTrack.Data.Position = 0; 
                currentTrack.State = TrackState.Playable;
                
            }).Start();
        }

        public void SetNext(File file,string ext)
        {
            if (StringToExtension.ContainsKey(ext))
            {
                currentTrack.Extension = StringToExtension[ext];
            }

            nextTrack.FileCache = file; 
            nextTrack.State = TrackState.Loading;
            new Thread(delegate (object o)
            {
                //var response = WebRequest.Create(url).GetResponse();
                using (var stream = GoogleDriveApiManager.GetAudioFile(file.Id)) //response.GetResponseStream())
                {
                    byte[] buffer = new byte[65536]; // 64KB chunks
                    stream.Position = 0;
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        nextTrack.Data.Position = nextTrack.Data.Length;
                        nextTrack.Data.Write(buffer, 0, read);
                    }
                }
                nextTrack.Data.Position = 0;
                nextTrack.State = TrackState.Playable;
            }).Start();
        }
        /// <summary>
        /// 再生を停止します。
        /// </summary>
        public void Stop()
        {
            _stop = true;
            //this._waveOut.Stop();
            //this.ms.Position = 0;
        }
    }
}
