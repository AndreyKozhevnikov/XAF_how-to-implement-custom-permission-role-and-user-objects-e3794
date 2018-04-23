using System;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Security;
using SecuredExportExample.Module.SecurityObjects;
using SecuredExportExample.Module.BusinessObjects;
using DevExpress.ExpressApp.Security.Strategy;

namespace SecuredExportExample.Module.DatabaseUpdate {
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            ExtendedSecurityRole defaultRole = CreateUserRole();
            ExtendedSecurityRole administratorRole = CreateAdministratorRole();
            ExtendedSecurityRole exporterRole = CreateExporterRole();
            Employee userAdmin = ObjectSpace.FindObject<Employee>(new BinaryOperator("UserName", "Admin"));
            if (userAdmin == null) {
                userAdmin = ObjectSpace.CreateObject<Employee>();
                userAdmin.UserName = "Admin";
                userAdmin.IsActive = true;
                userAdmin.SetPassword("");
                userAdmin.Roles.Add(administratorRole);
                userAdmin.Save();
            }
            Employee userSam = ObjectSpace.FindObject<Employee>(new BinaryOperator("UserName", "Sam"));
            if (userSam == null) {
                userSam = ObjectSpace.CreateObject<Employee>();
                userSam.UserName = "Sam";
                userSam.IsActive = true;
                userSam.SetPassword("");
                userSam.Roles.Add(exporterRole);
                userSam.Roles.Add(defaultRole);
                userSam.Save();
            }
            Employee userJohn = ObjectSpace.FindObject<Employee>(new BinaryOperator("UserName", "John"));
            if (userJohn == null) {
                userJohn = ObjectSpace.CreateObject<Employee>();
                userJohn.UserName = "John";
                userJohn.IsActive = true;
                userJohn.Roles.Add(defaultRole);
                for (int i = 1; i <= 10; i++) {
                    string subject = string.Format("Task {0}",i);
                    Task task = ObjectSpace.FindObject<Task>(new BinaryOperator("Subject", subject));
                    if (task == null) {
                        task = ObjectSpace.CreateObject<Task>();
                        task.Subject = subject;
                        task.DueDate = DateTime.Today;
                        task.Save();
                        userJohn.Tasks.Add(task);
                    }
                }
                userJohn.Save();
            }
            ObjectSpace.CommitChanges();
        }
        private ExtendedSecurityRole CreateAdministratorRole() {
            ExtendedSecurityRole administratorRole = ObjectSpace.FindObject<ExtendedSecurityRole>(
                new BinaryOperator("Name", SecurityStrategyComplex.AdministratorRoleName));
            if (administratorRole == null) {
                administratorRole = ObjectSpace.CreateObject<ExtendedSecurityRole>();
                administratorRole.Name = SecurityStrategyComplex.AdministratorRoleName;
                administratorRole.IsAdministrative = true;
            }
            return administratorRole;
        }
        private ExtendedSecurityRole CreateExporterRole() {
            ExtendedSecurityRole exporterRole = ObjectSpace.FindObject<ExtendedSecurityRole>(new BinaryOperator("Name", "Exporter"));
            if (exporterRole == null) {
                exporterRole = ObjectSpace.CreateObject<ExtendedSecurityRole>();
                exporterRole.Name = "Exporter";
                exporterRole.CanExport = true;
            }
            return exporterRole;
        }
        private ExtendedSecurityRole CreateUserRole() {
            ExtendedSecurityRole userRole = ObjectSpace.FindObject<ExtendedSecurityRole>(new BinaryOperator("Name", "Default"));
            if (userRole == null) {
                userRole = ObjectSpace.CreateObject<ExtendedSecurityRole>();
                userRole.Name = "Default";
                SecuritySystemTypePermissionObject userTypePermission =
                    ObjectSpace.CreateObject<SecuritySystemTypePermissionObject>();
                userTypePermission.TargetType = typeof(SecuritySystemUser);
                SecuritySystemObjectPermissionsObject currentUserObjectPermission =
                    ObjectSpace.CreateObject<SecuritySystemObjectPermissionsObject>();
                currentUserObjectPermission.Criteria = "[Oid] = CurrentUserId()";
                currentUserObjectPermission.AllowNavigate = true;
                currentUserObjectPermission.AllowRead = true;
                userTypePermission.ObjectPermissions.Add(currentUserObjectPermission);
                userRole.TypePermissions.Add(userTypePermission);
                SecuritySystemTypePermissionObject taskPermission = 
                    ObjectSpace.CreateObject<SecuritySystemTypePermissionObject>();
                taskPermission.TargetType = typeof(Task);
                taskPermission.AllowCreate = true;
                taskPermission.AllowNavigate = true;
                taskPermission.AllowRead = true;
                taskPermission.AllowWrite = true;
                userRole.TypePermissions.Add(taskPermission);
                SecuritySystemTypePermissionObject employeePermission = 
                    ObjectSpace.CreateObject<SecuritySystemTypePermissionObject>();
                employeePermission.TargetType = typeof(Employee);
                employeePermission.AllowNavigate = true;
                employeePermission.AllowRead = true;
                employeePermission.Save();
                userRole.TypePermissions.Add(employeePermission);
            }
            return userRole;
        }
    }
}
