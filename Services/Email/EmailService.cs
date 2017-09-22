namespace WebAPI.Services.Email
{
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Gmail.v1;
    using Google.Apis.Gmail.v1.Data;
    using Google.Apis.Services;
    using Google.Apis.Util.Store;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class EmailService
    {
        #region Private Properties
        private static string[] Scopes = { GmailService.Scope.GmailReadonly };
        private static string ApplicationName = "Gmail API .NET Quickstart";

        #endregion Private Properties

        #region Public Methods

        public void TestMethod()
        {
            GmailService service = this.CreateGmailService();
            
            // Define parameters of request.
            UsersResource.LabelsResource.ListRequest request = service.Users.Labels.List("me");

            // List labels.
            IList<Label> labels = request.Execute().Labels;
            Console.WriteLine("Labels:");
            if (labels != null && labels.Count > 0)
            {
                foreach (var labelItem in labels)
                {
                    Console.WriteLine("{0}", labelItem.Name);
                }
            }
            else
            {
                Console.WriteLine("No labels found.");
            }
            Console.Read();
        }

        #endregion Public Methods

        #region Private Methods

        private GmailService CreateGmailService()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {

                string credPath = Environment.GetEnvironmentVariable("LocalAppData");
                credPath = Path.Combine(credPath, ".credentials/gmail-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Gmail API service.
            GmailService service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            return service;
        }

        #endregion Private Methods
    }

}