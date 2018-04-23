Imports Microsoft.VisualBasic
Imports System

Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Updating
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp.Security
Imports SecuredExportExample.Module.SecurityObjects
Imports SecuredExportExample.Module.BusinessObjects

Namespace SecuredExportExample.Module.DatabaseUpdate
    Public Class Updater
        Inherits ModuleUpdater
        Public Sub New(ByVal objectSpace As IObjectSpace, ByVal currentDBVersion As Version)
            MyBase.New(objectSpace, currentDBVersion)
        End Sub
        Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
            MyBase.UpdateDatabaseAfterUpdateSchema()
            CreateAnonymousAccess()
            Dim defaultRole As ExtendedSecurityRole = CreateUserRole()
            Dim administratorRole As ExtendedSecurityRole = CreateAdministratorRole()
            Dim exporterRole As ExtendedSecurityRole = CreateExporterRole()
            Dim userAdmin As Employee = ObjectSpace.FindObject(Of Employee)(New BinaryOperator("UserName", "Sam"))
            If userAdmin Is Nothing Then
                userAdmin = ObjectSpace.CreateObject(Of Employee)()
                userAdmin.UserName = "Sam"
                userAdmin.IsActive = True
                userAdmin.SetPassword("")
                userAdmin.Roles.Add(defaultRole)
                userAdmin.Roles.Add(administratorRole)
                userAdmin.Roles.Add(exporterRole)
                userAdmin.Save()
            End If
            Dim userJohn As Employee = ObjectSpace.FindObject(Of Employee)(New BinaryOperator("UserName", "John"))
            If userJohn Is Nothing Then
                userJohn = ObjectSpace.CreateObject(Of Employee)()
                userJohn.UserName = "John"
                userJohn.IsActive = True
                userJohn.Roles.Add(defaultRole)
                For i As Integer = 1 To 10
                    Dim subject As String = String.Format("Task {0}",i)
                    Dim task As Task = ObjectSpace.FindObject(Of Task)(New BinaryOperator("Subject", subject))
                    If task Is Nothing Then
                        task = ObjectSpace.CreateObject(Of Task)()
                        task.Subject = subject
                        task.DueDate = DateTime.Today
                        task.Save()
                        userJohn.Tasks.Add(task)
                    End If
                Next i
                userJohn.Save()
            End If
            ObjectSpace.CommitChanges()
        End Sub
        Private Function CreateAdministratorRole() As ExtendedSecurityRole
            Dim administratorRole As ExtendedSecurityRole = ObjectSpace.FindObject(Of ExtendedSecurityRole)(New BinaryOperator("Name", SecurityStrategyComplex.AdministratorRoleName))
            If administratorRole Is Nothing Then
                administratorRole = ObjectSpace.CreateObject(Of ExtendedSecurityRole)()
                administratorRole.Name = SecurityStrategyComplex.AdministratorRoleName
                administratorRole.CanEditModel = True
                administratorRole.BeginUpdate()
                administratorRole.Permissions.GrantRecursive(GetType(Object), SecurityOperations.FullAccess)
                administratorRole.EndUpdate()
                administratorRole.Save()
            End If
            Return administratorRole
        End Function
        Private Function CreateExporterRole() As ExtendedSecurityRole
            Dim exporterRole As ExtendedSecurityRole = ObjectSpace.FindObject(Of ExtendedSecurityRole)(New BinaryOperator("Name", "Exporter"))
            If exporterRole Is Nothing Then
                exporterRole = ObjectSpace.CreateObject(Of ExtendedSecurityRole)()
                exporterRole.Name = "Exporter"
                exporterRole.CanExport = True
                exporterRole.Save()
            End If
            Return exporterRole
        End Function
        Private Function CreateUserRole() As ExtendedSecurityRole
            Dim userRole As ExtendedSecurityRole = ObjectSpace.FindObject(Of ExtendedSecurityRole)(New BinaryOperator("Name", "Default"))
            If userRole Is Nothing Then
                userRole = ObjectSpace.CreateObject(Of ExtendedSecurityRole)()
                userRole.Name = "Default"
                Dim myDetailsPermission As ObjectOperationPermissionData = ObjectSpace.CreateObject(Of ObjectOperationPermissionData)()
                myDetailsPermission.TargetType = GetType(Employee)
                myDetailsPermission.Criteria = "[Oid] = CurrentUserId()"
                myDetailsPermission.AllowNavigate = True
                myDetailsPermission.AllowRead = True
                myDetailsPermission.Save()
                userRole.PersistentPermissions.Add(myDetailsPermission)
                Dim ownPasswordPermission As MemberOperationPermissionData = ObjectSpace.CreateObject(Of MemberOperationPermissionData)()
                ownPasswordPermission.TargetType = GetType(Employee)
                ownPasswordPermission.Members = "ChangePasswordOnFirstLogon, StoredPassword"
                ownPasswordPermission.AllowWrite = True
                ownPasswordPermission.Save()
                userRole.PersistentPermissions.Add(ownPasswordPermission)
                Dim defaultRolePermission As ObjectOperationPermissionData = ObjectSpace.CreateObject(Of ObjectOperationPermissionData)()
                defaultRolePermission.TargetType = GetType(SecurityRole)
                defaultRolePermission.Criteria = "[Name] = 'Default'"
                defaultRolePermission.AllowNavigate = True
                defaultRolePermission.AllowRead = True
                defaultRolePermission.Save()
                userRole.PersistentPermissions.Add(defaultRolePermission)
                Dim taskPermission As TypeOperationPermissionData = ObjectSpace.CreateObject(Of TypeOperationPermissionData)()
                taskPermission.TargetType = GetType(Task)
                taskPermission.AllowCreate = True
                taskPermission.AllowNavigate = True
                taskPermission.AllowRead = True
                taskPermission.AllowWrite = True
                taskPermission.Save()
                userRole.PersistentPermissions.Add(taskPermission)
                Dim employeePermission As TypeOperationPermissionData = ObjectSpace.CreateObject(Of TypeOperationPermissionData)()
                employeePermission.TargetType = GetType(Employee)
                employeePermission.AllowNavigate = True
                employeePermission.AllowRead = True
                employeePermission.Save()
                userRole.PersistentPermissions.Add(employeePermission)
            End If
            Return userRole
        End Function
        Private Sub CreateAnonymousAccess()
            Dim anonymousRole As SecurityRole = ObjectSpace.FindObject(Of ExtendedSecurityRole)(New BinaryOperator("Name", SecurityStrategy.AnonymousUserName))
            If anonymousRole Is Nothing Then
                anonymousRole = ObjectSpace.CreateObject(Of SecurityRole)()
                anonymousRole.Name = SecurityStrategy.AnonymousUserName
                anonymousRole.BeginUpdate()
                anonymousRole.Permissions(GetType(Employee)).Grant(SecurityOperations.Read)
                anonymousRole.EndUpdate()
                anonymousRole.Save()
            End If
            Dim anonymousUser As Employee = ObjectSpace.FindObject(Of Employee)(New BinaryOperator("UserName", SecurityStrategy.AnonymousUserName))
            If anonymousUser Is Nothing Then
                anonymousUser = ObjectSpace.CreateObject(Of Employee)()
                anonymousUser.UserName = SecurityStrategy.AnonymousUserName
                anonymousUser.IsActive = True
                anonymousUser.SetPassword("")
                anonymousUser.Roles.Add(anonymousRole)
                anonymousUser.Save()
            End If
        End Sub
    End Class
End Namespace
