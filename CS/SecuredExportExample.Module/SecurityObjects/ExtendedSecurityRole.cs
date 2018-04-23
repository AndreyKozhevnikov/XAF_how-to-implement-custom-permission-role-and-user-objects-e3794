using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Security;

namespace SecuredExportExample.Module.SecurityObjects {
    [DefaultClassOptions]
    [ImageName("BO_Role")]
    public class ExtendedSecurityRole : SecurityRole {
        public ExtendedSecurityRole(Session session) : base(session) { }
        private ExportPermissionData FindExportPermissionData() {
            foreach (PermissionData permissionData in PersistentPermissions) {
                ExportPermissionData exportPermissionData = permissionData as ExportPermissionData;
                if (exportPermissionData != null) {
                    return exportPermissionData;
                }
            }
            return null;
        }
        [NonPersistent]
        public bool CanExport {
            get {
                ExportPermissionData exportPermissionData = FindExportPermissionData();
                return exportPermissionData != null && exportPermissionData.CanExport;
            }
            set {
                if (!IsLoading) {
                    ExportPermissionData exportPermissionData = FindExportPermissionData();
                    if (value) {
                        if (exportPermissionData == null) {
                            exportPermissionData = new ExportPermissionData(Session);
                            PersistentPermissions.Add(exportPermissionData);
                        }
                        exportPermissionData.CanExport = true;
                    }
                    else {
                        if (exportPermissionData != null) {
                            exportPermissionData.CanExport = false;
                            
                        }
                    }
                }
            }
        }
    }
}
