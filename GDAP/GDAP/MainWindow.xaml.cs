using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Google;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Microsoft.Toolkit.Wpf.UI.Controls;
using Exception = System.Exception;
using File = Google.Apis.Drive.v3.Data.File;

namespace GDAP
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        private List<Google.Apis.Drive.v3.Data.File> _fileCache = new List<File>();
        private Dictionary<string, List<File>> _folderTree = new Dictionary<string, List<File>> ();
        private List<Google.Apis.Drive.v3.Data.File> _playStack = new List<Google.Apis.Drive.v3.Data.File>();

        private bool _playStackIsDirty = false;
        private bool _playNext = false;
        private AudioPlayer _audio;
        System.Windows.Forms.Timer _myTimer = new System.Windows.Forms.Timer();
        public MainWindow()
        {

            if (Instance != null)
            {
                Close();
            }
            //ConsoleManager.Show();
            Instance = this;
            InitializeComponent();
            InitAudio();
            //Console("Boot");
            GoogleDriveApiManager.CompleteAuth += delegate
            {
                Task.Run(()=>GoogleDriveApiManager.StartSearchAllAudioFile(SearchAudioFiles));
                
            };
            GoogleDriveApiManager.CompleteGetFilelist += delegate { 
            
                MakeFolderTree(_fileCache);
                InvokeInOtherThread(()=>ShowFolders(_folderTree));
                //AddAudioFilesUI(_fileCache);
            };
            Loaded += delegate { GoogleDriveApiManager.Auth();  };
            Closed += delegate
            {
                AudioPlayer.WindowClosed = true;
                _myTimer.Stop();
                if (_audio != null)
                { 
                    _audio.Stop();
                    _audio.OnClose();
                }
            };

            _myTimer.Tick += delegate
            {
                PlayListTextUpdate();
                CheckStack();
            };
            _myTimer.Interval = 10;
            _myTimer.Start();
        }
        void InitAudio()
        {
            if (_audio == null)
            {
                _audio = new AudioPlayer();
                _audio.OnEndPlaying += delegate
                {
                    _playStackIsDirty = true;
                    CheckStack();
                };
            }
        }
        private void SearchAudioFiles(FileList files)
        {
            _fileCache.AddRange(files.Files);

        }

        void InvokeInOtherThread(Action action)
        {
            this.Dispatcher?.Invoke(action);
        }
        void MakeFolderTree(IList<Google.Apis.Drive.v3.Data.File> files)
        {
            foreach (var file in files)
            {

                var extension = System.IO.Path.GetExtension(file.Name)?.ToUpper();
                if (!AudioPlayer.StringToExtension.ContainsKey(extension))
                {
                    continue;
                }
                var parent = ".";
                if (file.Parents != null)
                {
                    if (file.Parents.Count > 0)
                    {
                        try
                        {
                            var name = _fileCache.First(f => f.Id == file.Parents[0]);
                            parent = name.Name;
                        }
                        catch (Exception e)
                        {
                            continue;
                        }
                       

                    }
                }

                if (_folderTree.ContainsKey(parent))
                {
                    _folderTree[parent].Add(file);
                }
                else
                {
                    _folderTree.Add(parent, new List<File>(){file});
                }

            }

        }

        Button CreateButton()
        {
            return new Button()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Background = new SolidColorBrush(Colors.White),
                FontSize = 10,
                
            };
        }
        void ShowFolders(Dictionary<string,List<File>> tree)
        {
            StackPanel.Children.Clear();
            var all2 = CreateButton();
            all2.Content = "すべてランダム再生";
            StackPanel.Children.Add(all2);
            all2.Click += (s2, e2) =>
            {
                var files = new List<File>();
                foreach (var keyValuePair in tree)
                {
                    files.AddRange(keyValuePair.Value);
                }
                AddPlayStackRandom(files);
            };
            foreach (var treeKey in tree.Keys)
            {
                if (tree[treeKey].Count(f =>
                {
                    var extension = System.IO.Path.GetExtension(f.Name)?.ToUpper();
                    return AudioPlayer.StringToExtension.ContainsKey(extension);

                }) <= 0)
                {
                    continue;
                }
                var button = CreateButton();
                button.Content = treeKey;
                button.Click += (s, e) =>
                {
                    StackPanel.Children.Clear();
                    var back = CreateButton();
                    back.Content = "もどる";
                    back.Click += (s2, e2) => ShowFolders(tree);
                    var all = CreateButton();
                    all.Content = "ランダム再生";
                    all.Click += (s2, e2) => AddPlayStackRandom(tree[treeKey]);
                    StackPanel.Children.Add(back);
                    StackPanel.Children.Add(all);
                    AddAudioFilesUI(tree[treeKey]);
                    //here is the video link you wanted
                    //string sourceURL = GoogleDriveApiManager.GetLink(file.Id);
                    //WebView1.Navigate(sourceURL);
                };
                StackPanel.Children.Add(button);
            }
        }
        public static List<int> RandomUniqueNunbers(int minNumber, int maxNumber, int count)
        {
            if (minNumber >= maxNumber)
            {
                return new List<int>() { minNumber };
            }

            var length = maxNumber - minNumber;
            var arr = new int[length];



            for (int k = 0; k < length; k++)
            {
                arr[k] = minNumber + k;
                //配列がきちんと初期化できているか確認
                //sw.Write(" {0} 回目：{1} \r\n", k + 1, r_arr[k]);
            }
            var rand = new System.Random();

            for (int i = 0; i < count; i++)
            {
                int r = rand.Next(i, length); // iからr_num-1の乱数を取得
                int temp = arr[i];
                arr[i] = arr[r];
                arr[r] = temp;
            }

            return arr.Take(count).ToList();
        }

        public void ShuffleQueue()
        {
            var li = RandomUniqueNunbers(0, _playStack.Count, _playStack.Count);
            var newList = new List<File>();
            foreach (var i in li)
            {
                newList.Add(_playStack[i]);
            }
            _playStack.Clear();
            foreach (var file in newList)
            {
                _playStack.Add(file);
            }
            
        }
        void AddPlayStackRandom(List<Google.Apis.Drive.v3.Data.File> queue)
        {
            var li = RandomUniqueNunbers(0, queue.Count, queue.Count);
            foreach (var i in li)
            {
                AddPlayStack(queue[i]);
            }
        }
        void AddPlayStack(Google.Apis.Drive.v3.Data.File id)
        {
            _playStack.Insert(0,id);
            _playStackIsDirty = true;
            PlayListUpdate();
        }
        void AddAudioFilesUI(IList<Google.Apis.Drive.v3.Data.File> files)
        {
            var audio = new List<Google.Apis.Drive.v3.Data.File>();

            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    
                    var extension = System.IO.Path.GetExtension(file.Name)?.ToUpper();
                    // MainWindow.Console($"{ file.Name}({extension}) ({file.Size }) ({file.Id})");

                    if (AudioPlayer.StringToExtension.ContainsKey(extension))
                    {
                        audio.Add(file);
                    }

                }
            }
            else
            {
                //MainWindow.Console("No files found.");
            }

            foreach (var file in audio)
            {
                var button = CreateButton();
                button.Content = file.Name;
                button.Click += (s, e) =>
                {
                    AddPlayStack(file);
                };
                StackPanel.Children.Add(button);
            }
        }

     
        //returns まだキャッシュできるか
        bool PlayInQueue()
        {
            if (!(_playStack.Count > 0))
            {
                return false;
            }
            var file = _playStack[_playStack.Count - 1];
            var ext = System.IO.Path.GetExtension(_playStack[_playStack.Count - 1].Name)?.ToUpper();
            //var removed = _playStack[_playStack.Count - 1];
            _playStack.RemoveAt(_playStack.Count - 1);
            PlayListUpdate();
            if (_audio.IsPlaying)
            {
                _audio.SetNext(file, ext);
 
                return false;
            }
            else
            {

                if (_audio.NextExists())
                {
                    try
                    {
                        _audio.Play();
                        _audio.SetNext(file, ext);
   
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine(e);
                        throw;
                    }
                    
                    return false;
                }
                else
                {
                    _audio.SetCurrent(file, ext);
                    _audio.Play();
                    _playNext = true;

                    return true;
                }
            }

        }

        void CheckStack(object o = null, object s = null)
        {
            if (!_playStackIsDirty)
            {
                return;
            }

            _playStackIsDirty = false;
                
            

            if (_audio.IsPlaying)
            {
                _audio.Stop();
            }

            while (PlayInQueue())
            {
                
            }
        }
        private string playListText = "";
        private void PlayListUpdate()
        {
            playListText = "";
            if (_audio?.Current != null)
            {
                playListText += $"play ... { _audio.Current.Name}\n";
                
            }

            if (_audio?.Next != null)
            {
                playListText += $"next ... { _audio.Next.Name}\n";
            } 
            for (var i = _playStack.Count - 1; i >= 0; i--)
            {
                playListText += $"  {_playStack[i].Name}\n";
            }
        }

        private void PlayListTextUpdate()
        {
            playList.Text = playListText;
        }
        //private string consoleLog = "";
        //public static void Console(string str)
        //{
        //    MainWindow.Instance.consoleLog += str + "\n";
        //}

        //public static void LabelUpdate()
        //{
        ////    MainWindow.Instance.console.Text = MainWindow.Instance.consoleLog;
        //}


    }
    //[SuppressUnmanagedCodeSecurity]
    //public static class ConsoleManager
    //{
    //    private const string Kernel32_DllName = "kernel32.dll";

    //    [DllImport(Kernel32_DllName)]
    //    private static extern bool AllocConsole();

    //    [DllImport(Kernel32_DllName)]
    //    private static extern bool FreeConsole();

    //    [DllImport(Kernel32_DllName)]
    //    private static extern IntPtr GetConsoleWindow();

    //    [DllImport(Kernel32_DllName)]
    //    private static extern int GetConsoleOutputCP();

    //    public static bool HasConsole
    //    {
    //        get { return GetConsoleWindow() != IntPtr.Zero; }
    //    }

    //    /// <summary>
    //    /// Creates a new console instance if the process is not attached to a console already.
    //    /// </summary>
    //    public static void Show()
    //    {
    //        //#if DEBUG
    //        if (!HasConsole)
    //        {
    //            AllocConsole();
    //            InvalidateOutAndError();
    //        }
    //        Console.WriteLine("Boot");
    //        //#endif
    //    }

    //    /// <summary>
    //    /// If the process has a console attached to it, it will be detached and no longer visible. Writing to the System.Console is still possible, but no output will be shown.
    //    /// </summary>
    //    public static void Hide()
    //    {
    //        //#if DEBUG
    //        if (HasConsole)
    //        {
    //            SetOutAndErrorNull();
    //            FreeConsole();
    //        }
    //        //#endif
    //    }

    //    public static void Toggle()
    //    {
    //        if (HasConsole)
    //        {
    //            Hide();
    //        }
    //        else
    //        {
    //            Show();
    //        }
    //    }

    //    static void InvalidateOutAndError()
    //    {
    //        Type type = typeof(System.Console);

    //        System.Reflection.FieldInfo _out = type.GetField("_out",
    //            System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

    //        System.Reflection.FieldInfo _error = type.GetField("_error",
    //            System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

    //        System.Reflection.MethodInfo _InitializeStdOutError = type.GetMethod("InitializeStdOutError",
    //            System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

    //        Debug.Assert(_out != null);
    //        Debug.Assert(_error != null);

    //        Debug.Assert(_InitializeStdOutError != null);

    //        _out.SetValue(null, null);
    //        _error.SetValue(null, null);

    //        _InitializeStdOutError.Invoke(null, new object[] { true });
    //    }

    //    static void SetOutAndErrorNull()
    //    {
    //        Console.SetOut(TextWriter.Null);
    //        Console.SetError(TextWriter.Null);
    //    }
    //}
}
