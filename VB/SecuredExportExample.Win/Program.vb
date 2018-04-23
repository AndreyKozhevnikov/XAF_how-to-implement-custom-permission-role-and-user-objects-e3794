Imports System.Configuration

Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Security
Imports DevExpress.ExpressApp.Win
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports SecuredExportExample.Module.SecurityObjects
Imports DevExpress.ExpressApp.Xpo

Namespace SecuredExportExample.Win
    Friend NotInheritable Class Program

        Private Sub New()
        End Sub

        ''' <summary>
        ''' The main entry point for the application.
        ''' </summary>
        <STAThread> _
        Shared Sub Main()
            Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)
            EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached
            Dim winApplication As New SecuredExportExampleWindowsFormsApplication()
#If EASYTEST Then
            DevExpress.ExpressApp.Win.EasyTest.EasyTestRemotingRegistration.Register()
#End If
            Try
                InMemoryDataStoreProvider.Register()
                winApplication.ConnectionString = InMemoryDataStoreProvider.ConnectionString
                AddHandler CType(winApplication.Security, SecurityStrategy).CustomizeRequestProcessors, Function(sender, e) AnonymousMethod1(sender, e)
                winApplication.Setup()
                winApplication.Start()
            Catch e As Exception
                winApplication.HandleException(e)
            End Try
        End Sub
        
        Private Shared Function AnonymousMethod1(ByVal sender As Object, ByVal e As CustomizeRequestProcessorsEventArgs) As Object
            e.Processors.Add(GetType(ExportPermissionRequest), New ExportPermissionRequestProcessor(e.Permissions))
            Return Nothing
        End Function

    End Class
End Namespace
