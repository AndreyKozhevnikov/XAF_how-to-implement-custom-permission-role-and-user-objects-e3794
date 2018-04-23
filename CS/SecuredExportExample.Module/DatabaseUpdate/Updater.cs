using System;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Security;
using SecuredExportExample.Module.SecurityObjects;
using SecuredExportExample.Module.BusinessObjects;

namespace SecuredExportExample.Module.DatabaseUpdate {
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            CreateAnonymousAccess();
            ExtendedSecurityRole defaultRole = CreateUserRole();
            ExtendedSecurityRole administratorRole = CreateAdministratorRole();
            ExtendedSecurityRole exporterRole = CreateExporterRole();
            Employee userAdmin = ObjectSpace.FindObject<Employee>(new BinaryOperator("UserName", "Sam"));
            if (userAdmin == null) {
                userAdmin = ObjectSpace.CreateObject<Employee>();
                userAdmin.UserName = "Sam";
                userAdmin.IsActive = true;
                userAdmin.SetPassword("");
                userAdmin.Roles.Add(defaultRole);
                userAdmin.Roles.Add(administratorRole);
                userAdmin.Roles.Add(exporterRole);
                userAdmin.Save();
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
                administratorRole.CanEditModel = true;
                administratorRole.BeginUpdate();
                administratorRole.Permissions.GrantRecursive(typeof(object), SecurityOperations.FullAccess);
                administratorRole.EndUpdate();
                administratorRole.Save();
            }
            return administratorRole;
        }

        private ExtendedSecurityRole CreateExporterRole() {
            ExtendedSecurityRole exporterRole = ObjectSpace.FindObject<ExtendedSecurityRole>(new BinaryOperator("Name", "Exporter"));
            if (exporterRole == null) {
                exporterRole = ObjectSpace.CreateObject<ExtendedSecurityRole>();
                exporterRole.Name = "Exporter";
                exporterRole.CanExport = true;
                exporterRole.Save();
            }
            return exporterRole;
        }

        private ExtendedSecurityRole CreateUserRole() {
            ExtendedSecurityRole userRole = ObjectSpace.FindObject<ExtendedSecurityRole>(new BinaryOperator("Name", "Default"));
            if (userRole == null) {
                userRole = ObjectSpace.CreateObject<ExtendedSecurityRole>();
                userRole.Name = "Default";
                ObjectOperationPermissionData myDetailsPermission = ObjectSpace.CreateObject<ObjectOperationPermissionData>();
                myDetailsPermission.TargetType = typeof(Employee);
                myDetailsPermission.Criteria = "[Oid] = CurrentUserId()";
                myDetailsPermission.AllowNavigate = true;
                myDetailsPermission.AllowRead = true;
                myDetailsPermission.Save();
                userRole.PersistentPermissions.Add(myDetailsPermission);
                MemberOperationPermissionData ownPasswordPermission = ObjectSpace.CreateObject<MemberOperationPermissionData>();
                ownPasswordPermission.TargetType = typeof(Employee);
                ownPasswordPermission.Members = "ChangePasswordOnFirstLogon, StoredPassword";
                ownPasswordPermission.AllowWrite = true;
                ownPasswordPermission.Save();
                userRole.PersistentPermissions.Add(ownPasswordPermission);
                ObjectOperationPermissionData defaultRolePermission = ObjectSpace.CreateObject<ObjectOperationPermissionData>();
                defaultRolePermission.TargetType = typeof(SecurityRole);
                defaultRolePermission.Criteria = "[Name] = 'Default'";
                defaultRolePermission.AllowNavigate = true;
                defaultRolePermission.AllowRead = true;
                defaultRolePermission.Save();
                userRole.PersistentPermissions.Add(defaultRolePermission);
                TypeOperationPermissionData taskPermission = ObjectSpace.CreateObject<TypeOperationPermissionData>();
                taskPermission.TargetType = typeof(Task);
                taskPermission.AllowCreate = true;
                taskPermission.AllowNavigate = true;
                taskPermission.AllowRead = true;
                taskPermission.AllowWrite = true;
                taskPermission.Save();
                userRole.PersistentPermissions.Add(taskPermission);
                TypeOperationPermissionData employeePermission = ObjectSpace.CreateObject<TypeOperationPermissionData>();
                employeePermission.TargetType = typeof(Employee);
                employeePermission.AllowNavigate = true;
                employeePermission.AllowRead = true;
                employeePermission.Save();
                userRole.PersistentPermissions.Add(employeePermission);
            }
            return userRole;
        }
        private void CreateAnonymousAccess() {
            SecurityRole anonymousRole = ObjectSpace.FindObject<ExtendedSecurityRole>(new BinaryOperator("Name", SecurityStrategy.AnonymousUserName));
            if (anonymousRole == null) {
                anonymousRole = ObjectSpace.CreateObject<SecurityRole>();
                anonymousRole.Name = SecurityStrategy.AnonymousUserName;
                anonymousRole.BeginUpdate();
                anonymousRole.Permissions[typeof(Employee)].Grant(SecurityOperations.Read);
                anonymousRole.EndUpdate();
                anonymousRole.Save();
            }
            Employee anonymousUser = ObjectSpace.FindObject<Employee>(new BinaryOperator("UserName", SecurityStrategy.AnonymousUserName));
            if (anonymousUser == null) {
                anonymousUser = ObjectSpace.CreateObject<Employee>();
                anonymousUser.UserName = SecurityStrategy.AnonymousUserName;
                anonymousUser.IsActive = true;
                anonymousUser.SetPassword("");
                anonymousUser.Roles.Add(anonymousRole);
                anonymousUser.Save();
            }
        }
    }
}
