Imports System.Configuration

Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Security
Imports DevExpress.ExpressApp.Win
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports SecuredExportExample.Module.SecurityObjects
Imports DevExpress.ExpressApp.Xpo
Imports DevExpress.ExpressApp.Security.ClientServer.Wcf
Imports DevExpress.ExpressApp.Security.ClientServer
Imports System.ServiceModel

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
            Dim serverProcess = New System.Diagnostics.Process()
            serverProcess.StartInfo.FileName = "..\..\..\SecuredExportExample.MiddleTierSecurityServer\bin\EasyTest\SecuredExportExample.MiddleTierSecurityServer.exe"
            serverProcess.Start()
            System.Threading.Thread.Sleep(5000)
#End If
            Try

                WcfDataServerHelper.AddKnownType(GetType(ExportPermissionRequest))
                winApplication.ConnectionString = "http://localhost:1451/DataServer"
                Dim clientDataServer As New WcfSecuredDataServerClient(WcfDataServerHelper.CreateDefaultBinding(), New EndpointAddress(winApplication.ConnectionString))
                Dim securityClient As New ServerSecurityClient(clientDataServer, New ClientInfoFactory())
                securityClient.IsSupportChangePassword = True
                winApplication.ApplicationName = "SecuredExportExample"
                winApplication.Security = securityClient
                AddHandler winApplication.CreateCustomObjectSpaceProvider, Function(sender, e) AnonymousMethod1(sender, e, clientDataServer, securityClient)
                winApplication.Setup()
                winApplication.Start()
                clientDataServer.Close()
            Catch e As Exception
                winApplication.HandleException(e)
            End Try
        End Sub
        
        Private Shared Function AnonymousMethod1(ByVal sender As Object, ByVal e As CreateCustomObjectSpaceProviderEventArgs, ByVal clientDataServer As WcfSecuredDataServerClient, ByVal securityClient As ServerSecurityClient) As Object
                e.ObjectSpaceProvider = New DataServerObjectSpaceProvider(clientDataServer, securityClient)
            Return Nothing
        End Function

    End Class
End Namespace
