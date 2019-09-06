using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Download;

namespace GDAP
{
    class GoogleDriveApiManager
    {
        public static GoogleDriveApiManager Instance { get; } = new GoogleDriveApiManager();
        private string ClientId = "84806902217-9ecgdr9b6s1p7ag98ot4thl1mkal3k8o.apps.googleusercontent.com";
        private string ClientSecret = "liVkt5cEFF-LY1swi4G9NFOu";
        private string ApiKey = "AIzaSyBQzeasOTbyir2id-xyV7OS45_a9nMMLoE";
        static string[] Scopes = { DriveService.Scope.DriveReadonly };
        static string ApplicationName = "Drive API .NET Quickstart";
        private DriveService _service;
        public delegate void SubThreadFileSearchHandler(FileList files);
        public static DriveService Service => Instance._service;
        public static event EventHandler CompleteAuth;
        private static event SubThreadFileSearchHandler GetResult;
        public static event EventHandler CompleteGetFilelist;
        public static void Auth()
        {
            MainWindow.Console("Run");
            UserCredential credential;

            using (var stream =
                new FileStream("Assets/credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "Assets/token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                MainWindow.Console("Credential file saved to: " + credPath);
            }

            // Create Drive API service.
            Instance._service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            OnCompleteAuth();
            //FilesResource.GetRequest getRequest = service.Files.Get("0B6zj9fZgMGr7dXl3Z3VxSGRadU0");

        }

        public static IList<Google.Apis.Drive.v3.Data.File> GetFileList()
        {


            // Define parameters of request.
            FilesResource.ListRequest listRequest = Instance._service.Files.List();
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name)";

            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                .Files;
            MainWindow.Console("Files:");
            
            return files;
        }

        public static async Task StartSearchAllAudioFile(SubThreadFileSearchHandler cb)
        {
            
            GetResult += cb;
            await Task.Run(async () =>
            {
                String pageToken = null;
                do
                {
                    FilesResource.ListRequest listRequest = Instance._service.Files.List();
                    //listRequest.Q = "mimeType = 'application/vnd.google-apps.folder'";
                    listRequest.Spaces = "drive";
                    listRequest.Fields = "nextPageToken, files(id, name),Parents";
                    listRequest.PageToken = pageToken;
                    FileList li = await listRequest.ExecuteAsync();
                    GetResult?.Invoke(li);
                    pageToken = li.NextPageToken;
                } while (pageToken != null);
                GetResult -= cb;

            });
            CompleteGetFilelist?.Invoke(null,null);



        }
        public static string GetLink(string id)
        {
            FilesResource.GetRequest getRequest = Instance._service.Files.Get(id);
            getRequest.Fields = "webViewLink";
            Google.Apis.Drive.v3.Data.File f = getRequest.Execute();
            return f.WebViewLink;
        }
        public static MemoryStream GetAudioFile(string id)
        {
            
            FilesResource.GetRequest getRequest = Instance._service.Files.Get(id);
            var stream = new MemoryStream();
  
            getRequest.Download(stream);
            return stream;
        }
        protected static void OnCompleteAuth()
        {
            CompleteAuth?.Invoke(null, EventArgs.Empty);
        }
    }
}
