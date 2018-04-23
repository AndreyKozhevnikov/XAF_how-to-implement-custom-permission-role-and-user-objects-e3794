using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Security;
using System.Collections.Generic;

namespace SecuredExportExample.Module.SecurityObjects {
    [System.ComponentModel.DisplayName("ExportOperationName Permission")]
    public class ExportPermissionData : PermissionData {
        private bool canExport = true;
        protected override string GetPermissionInfoCaption() {
            return "ExportOperationName";
        }
        public ExportPermissionData(Session session)
            : base(session) {
        }
        public override IList<IOperationPermission> GetPermissions() {
            IList<IOperationPermission> result = new List<IOperationPermission>();
            if (canExport) {
                result.Add(new ExportPermission());
            }
            return result;
        }
        public bool CanExport {
            get { return canExport; }
            set { SetPropertyValue("CanExport", ref canExport, value); }
        }
    }

}
