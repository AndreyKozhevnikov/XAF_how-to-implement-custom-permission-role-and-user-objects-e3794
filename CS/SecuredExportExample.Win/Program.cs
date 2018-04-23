using System;
using System.Configuration;
using System.Windows.Forms;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using SecuredExportExample.Module.SecurityObjects;

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
            try {
                // Uncomment this line when using the Middle Tier application server:
                // new DevExpress.ExpressApp.MiddleTier.MiddleTierClientApplicationConfigurator(winApplication);
                DevExpress.ExpressApp.InMemoryDataStoreProvider.Register();
                winApplication.ConnectionString = DevExpress.ExpressApp.InMemoryDataStoreProvider.ConnectionString;
                winApplication.Setup();
                winApplication.Start();
            }
            catch (Exception e) {
                winApplication.HandleException(e);
            }
        }

        
        

        
    }
}
