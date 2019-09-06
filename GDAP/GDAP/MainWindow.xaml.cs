using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
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
            Console("Boot");
            GoogleDriveApiManager.CompleteAuth += delegate
            {
                GoogleDriveApiManager.StartSearchAllAudioFile(SearchAudioFiles);
                
            };
            GoogleDriveApiManager.CompleteGetFilelist += delegate { 
            
                MakeFolderTree(_fileCache);
                ShowFolders(_folderTree);
                //AddAudioFilesUI(_fileCache);
            };
            Loaded += delegate { GoogleDriveApiManager.Auth();  };
            Closed += delegate
            {
                AudioPlayer.WindowClosed = true;
                _myTimer.Stop();

            };

            _myTimer.Tick += delegate { LabelUpdate(); };
            _myTimer.Interval = 10;
            _myTimer.Start();
        }

        private void SearchAudioFiles(FileList files)
        {
            _fileCache.AddRange(files.Files);
            foreach (var file in files.Files)
            {
                var extension = System.IO.Path.GetExtension(file.Name)?.ToUpper();
                Console($" {extension}");
            }
            
        }

        void MakeFolderTree(IList<Google.Apis.Drive.v3.Data.File> files)
        {
            foreach (var file in files)
            {
                var parent = ".";
                if (file.Parents.Count > 0)
                {
                    parent = file.Parents[0];
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
        void ShowFolders(Dictionary<string,List<File>> tree)
        {
            StackPanel.Children.Clear();
            foreach (var treeKey in tree.Keys)
            {
                var button = new Button();
                button.Content = treeKey;
                button.Click += (s, e) =>
                {
                    StackPanel.Children.Clear();
                    var back = new Button();
                    back.Content = "もどる";
                    back.Click += (s2, e2) => ShowFolders(tree);
                    var all = new Button();
                    all.Content = "ランダム再生";
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
                var button = new Button();
                button.Content = file.Name;
                button.Click += (s, e) =>
                {
                    var extension = System.IO.Path.GetExtension(file.Name)?.ToUpper();
                    //var stream = GoogleDriveApiManager.GetAudioFile(file.Id);
                    var audioPlayer = new AudioPlayer();
                    if (audioPlayer.IsPlaying)
                    {
                        audioPlayer.SetNext(file.Id, extension);
                    }
                    else
                    {
                        audioPlayer.SetCurrent(file.Id, extension);
                        audioPlayer.Play();
                    }

                    //here is the video link you wanted
                    //string sourceURL = GoogleDriveApiManager.GetLink(file.Id);
                    //WebView1.Navigate(sourceURL);
                };
                StackPanel.Children.Add(button);
            }
        }
        public void ShowAudioFiles()
        {

            var audio = new List<Google.Apis.Drive.v3.Data.File>();
            var files = GoogleDriveApiManager.GetFileList();
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
                var button = new Button();
                button.Content = file.Name;
                button.Click += (s, e) =>
                {
                    var extension = System.IO.Path.GetExtension(file.Name)?.ToUpper();
                    //var stream = GoogleDriveApiManager.GetAudioFile(file.Id);
                    var audioPlayer = new AudioPlayer();
                    if (audioPlayer.IsPlaying)
                    {
                        audioPlayer.SetNext(file.Id, extension);
                    }
                    else
                    {
                        audioPlayer.SetCurrent(file.Id, extension);
                        audioPlayer.Play();
                    }
                        
                    //here is the video link you wanted
                    //string sourceURL = GoogleDriveApiManager.GetLink(file.Id);
                    //WebView1.Navigate(sourceURL);
                };
                StackPanel.Children.Add(button);
            }
        }

        private string consoleLog = "";
        public static void Console(string str)
        {
            MainWindow.Instance.consoleLog += str + "\n";
        }

        public static void LabelUpdate()
        {
            MainWindow.Instance.console.Text = MainWindow.Instance.consoleLog;
        }


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
