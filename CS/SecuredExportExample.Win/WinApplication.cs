using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp;

namespace SecuredExportExample.Win {
    public partial class SecuredExportExampleWindowsFormsApplication : WinApplication {
        public SecuredExportExampleWindowsFormsApplication() {
            InitializeComponent();
            DelayedViewItemsInitialization = true;
        }
        private void SecuredExportExampleWindowsFormsApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e) {
            e.Updater.Update();
            e.Handled = true;
        }
    }
}
