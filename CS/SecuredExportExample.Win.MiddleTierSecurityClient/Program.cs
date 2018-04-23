using System;
using System.Configuration;
using System.Windows.Forms;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using SecuredExportExample.Module.SecurityObjects;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Security.ClientServer.Wcf;
using DevExpress.ExpressApp.Security.ClientServer;
using System.ServiceModel;

namespace SecuredExportExample.Win {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached;
            SecuredExportExampleWindowsFormsApplication winApplication = new SecuredExportExampleWindowsFormsApplication();
#if EASYTEST
            DevExpress.ExpressApp.Win.EasyTest.EasyTestRemotingRegistration.Register();
            var serverProcess = new System.Diagnostics.Process();
            serverProcess.StartInfo.FileName = @"..\..\..\SecuredExportExample.MiddleTierSecurityServer\bin\EasyTest\SecuredExportExample.MiddleTierSecurityServer.exe";
            serverProcess.Start();
            System.Threading.Thread.Sleep(5000);
#endif
            try {

                WcfDataServerHelper.AddKnownType(typeof(ExportPermissionRequest));
                winApplication.ConnectionString = "http://localhost:1451/DataServer";
                WcfSecuredDataServerClient clientDataServer = new WcfSecuredDataServerClient(
                    WcfDataServerHelper.CreateDefaultBinding(),
                    new EndpointAddress(winApplication.ConnectionString));
                ServerSecurityClient securityClient =
                    new ServerSecurityClient(clientDataServer, new ClientInfoFactory());
                securityClient.IsSupportChangePassword = true;
                winApplication.ApplicationName = "SecuredExportExample";
                winApplication.Security = securityClient;
                winApplication.CreateCustomObjectSpaceProvider +=
                    delegate(object sender, CreateCustomObjectSpaceProviderEventArgs e) {
                        e.ObjectSpaceProvider =
                            new DataServerObjectSpaceProvider(clientDataServer, securityClient);
                    };
                winApplication.Setup();
                winApplication.Start();
                clientDataServer.Close();
            }
            catch (Exception e) {
                winApplication.HandleException(e);
            }
        }

    }
}
