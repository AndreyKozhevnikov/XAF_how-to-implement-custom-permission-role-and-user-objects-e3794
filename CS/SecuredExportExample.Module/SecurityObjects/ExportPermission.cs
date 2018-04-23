using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.ExpressApp.Security;

namespace SecuredExportExample.Module.SecurityObjects {
    public class ExportPermission : OperationPermissionBase {
        public const string ExportOperationName = "ExportOperationName";
        public ExportPermission()
            : base(ExportOperationName) {
        }
        public override IList<string> GetSupportedOperations() {
            return new string[] { ExportOperationName };
        }
    }
    public class ExportPermissionRequest : OperationPermissionRequestBase {
        public ExportPermissionRequest()
            : base(ExportPermission.ExportOperationName) { }
        
    }
    public class ExportPermissionRequestProcessor : PermissionRequestProcessorBase<ExportPermissionRequest> {
        protected override bool IsRequestFit(
            ExportPermissionRequest permissionRequest, OperationPermissionBase permission, 
            IRequestSecurityStrategy securityInstance) {
            ExportPermission exportPermission = permission as ExportPermission;
            if (permissionRequest == null || exportPermission == null) {
                return false;
            }
            return permissionRequest.Operation == exportPermission.Operation;
        }
    }
}
