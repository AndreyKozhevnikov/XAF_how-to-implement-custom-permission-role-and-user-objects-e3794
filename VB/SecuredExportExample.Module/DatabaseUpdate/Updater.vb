Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Updating
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp.Security
Imports SecuredExportExample.Module.SecurityObjects
Imports SecuredExportExample.Module.BusinessObjects
Imports DevExpress.ExpressApp.Security.Strategy

Namespace SecuredExportExample.Module.DatabaseUpdate
    Public Class Updater
        Inherits ModuleUpdater

        Public Sub New(ByVal objectSpace As IObjectSpace, ByVal currentDBVersion As Version)
            MyBase.New(objectSpace, currentDBVersion)
        End Sub
        Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
            MyBase.UpdateDatabaseAfterUpdateSchema()
            Dim defaultRole As ExtendedSecurityRole = CreateUserRole()
            Dim administratorRole As ExtendedSecurityRole = CreateAdministratorRole()
            Dim exporterRole As ExtendedSecurityRole = CreateExporterRole()
            Dim userAdmin As Employee = ObjectSpace.FindObject(Of Employee)(New BinaryOperator("UserName", "Admin"))
            If userAdmin Is Nothing Then
                userAdmin = ObjectSpace.CreateObject(Of Employee)()
                userAdmin.UserName = "Admin"
                userAdmin.IsActive = True
                userAdmin.SetPassword("")
                userAdmin.Roles.Add(administratorRole)
                userAdmin.Save()
            End If
            Dim userSam As Employee = ObjectSpace.FindObject(Of Employee)(New BinaryOperator("UserName", "Sam"))
            If userSam Is Nothing Then
                userSam = ObjectSpace.CreateObject(Of Employee)()
                userSam.UserName = "Sam"
                userSam.IsActive = True
                userSam.SetPassword("")
                userSam.Roles.Add(exporterRole)
                userSam.Roles.Add(defaultRole)
                userSam.Save()
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
                        task.DueDate = Date.Today
                        task.Save()
                        userJohn.Tasks.Add(task)
                    End If
                Next i
                userJohn.Save()
            End If
        End Sub
        Private Function CreateAdministratorRole() As ExtendedSecurityRole
            Dim administratorRole As ExtendedSecurityRole = ObjectSpace.FindObject(Of ExtendedSecurityRole)(New BinaryOperator("Name", SecurityStrategyComplex.AdministratorRoleName))
            If administratorRole Is Nothing Then
                administratorRole = ObjectSpace.CreateObject(Of ExtendedSecurityRole)()
                administratorRole.Name = SecurityStrategyComplex.AdministratorRoleName
                administratorRole.IsAdministrative = True
            End If
            Return administratorRole
        End Function
        Private Function CreateExporterRole() As ExtendedSecurityRole
            Dim exporterRole As ExtendedSecurityRole = ObjectSpace.FindObject(Of ExtendedSecurityRole)(New BinaryOperator("Name", "Exporter"))
            If exporterRole Is Nothing Then
                exporterRole = ObjectSpace.CreateObject(Of ExtendedSecurityRole)()
                exporterRole.Name = "Exporter"
                exporterRole.CanExport = True
            End If
            Return exporterRole
        End Function
        Private Function CreateUserRole() As ExtendedSecurityRole
            Dim userRole As ExtendedSecurityRole = ObjectSpace.FindObject(Of ExtendedSecurityRole)(New BinaryOperator("Name", "Default"))
            If userRole Is Nothing Then
                userRole = ObjectSpace.CreateObject(Of ExtendedSecurityRole)()
                userRole.Name = "Default"
                userRole.SetTypePermissions(Of Task)(SecurityOperations.FullAccess, SecuritySystemModifier.Allow)
                userRole.SetTypePermissions(Of Employee)(SecurityOperations.ReadOnlyAccess, SecuritySystemModifier.Allow)
                userRole.AddObjectAccessPermission(Of SecuritySystemUser)("[Oid] = CurrentUserId()", SecurityOperations.ReadOnlyAccess)
            End If
            Return userRole
        End Function
    End Class
End Namespace
