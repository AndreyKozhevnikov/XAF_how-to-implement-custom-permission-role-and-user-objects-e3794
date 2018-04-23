using System;
using System.Collections.Generic;

using DevExpress.ExpressApp;
using System.Reflection;
using DevExpress.ExpressApp.Security;
using SecuredExportExample.Module.SecurityObjects;


namespace SecuredExportExample.Module {
    public sealed partial class SecuredExportExampleModule : ModuleBase {
        public SecuredExportExampleModule() {
            InitializeComponent();
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            application.SetupComplete += application_SetupComplete;
        }
        void application_SetupComplete(object sender, EventArgs e) {
            if (SecuritySystem.Instance is SecurityStrategy) {
                ((SecurityStrategy)SecuritySystem.Instance).RequestProcessors.Register(
                    new ExportPermissionRequestProcessor());
            }
        }
    }
}
