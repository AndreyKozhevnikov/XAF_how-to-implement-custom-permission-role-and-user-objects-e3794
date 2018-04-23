Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports DevExpress.ExpressApp.Security.ClientServer
Imports System.Configuration
Imports DevExpress.Persistent.Base
Imports DevExpress.ExpressApp.Security
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Xpo
Imports DevExpress.ExpressApp.MiddleTier
Imports SecuredExportExample.Module.BusinessObjects
Imports SecuredExportExample.Module.SecurityObjects
Imports System.ServiceModel
Imports DevExpress.ExpressApp.Security.ClientServer.Wcf

Namespace SecuredExportExample.MiddleTierSecurityServer
	Friend Class Program
		Private Shared Sub serverApplication_DatabaseVersionMismatch(ByVal sender As Object, ByVal e As DatabaseVersionMismatchEventArgs)
			e.Updater.Update()
			e.Handled = True
		End Sub
		Private Shared Sub serverApplication_CreateCustomObjectSpaceProvider(ByVal sender As Object, ByVal e As CreateCustomObjectSpaceProviderEventArgs)
			e.ObjectSpaceProvider = New XPObjectSpaceProvider(e.ConnectionString, e.Connection)
		End Sub
		Shared Sub Main(ByVal args() As String)
			Try

				ValueManager.ValueManagerType = GetType(MultiThreadValueManager(Of )).GetGenericTypeDefinition()
				InMemoryDataStoreProvider.Register()
				Dim connectionString As String = InMemoryDataStoreProvider.ConnectionString

				Console.WriteLine("Starting...")

				Dim serverApplication As New ServerApplication()
				' Change the ServerApplication.ApplicationName property value. It should be the same as your client application name. 
				serverApplication.ApplicationName = "SecuredExportExample"

				' Add your client application's modules to the ServerApplication.Modules collection here. 
				serverApplication.Modules.BeginInit()
				serverApplication.Modules.Add(New DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule())
				serverApplication.Modules.Add(New DevExpress.ExpressApp.Security.SecurityModule())
				serverApplication.Modules.Add(New SecuredExportExample.Module.SecuredExportExampleModule())
				serverApplication.Modules.EndInit()

				AddHandler serverApplication.DatabaseVersionMismatch, AddressOf serverApplication_DatabaseVersionMismatch
				AddHandler serverApplication.CreateCustomObjectSpaceProvider, AddressOf serverApplication_CreateCustomObjectSpaceProvider

				serverApplication.ConnectionString = connectionString

				Console.WriteLine("Setup...")
				serverApplication.Setup()
				Console.WriteLine("CheckCompatibility...")
				serverApplication.CheckCompatibility()
				serverApplication.Dispose()

				Console.WriteLine("Starting server...")
				Dim securityProviderHandler As QueryRequestSecurityStrategyHandler = Function() AnonymousMethod1()

				WcfDataServerHelper.AddKnownType(GetType(ExportPermissionRequest))
				Dim dataServer As New SecuredDataServer(connectionString, XpoTypesInfoHelper.GetXpoTypeInfoSource().XPDictionary, securityProviderHandler)
				Dim serviceHost As New ServiceHost(New WcfSecuredDataServer(dataServer))
				serviceHost.AddServiceEndpoint(GetType(IWcfSecuredDataServer), WcfDataServerHelper.CreateNetTcpBinding(), "net.tcp://127.0.0.1:1451/DataServer")
				serviceHost.Open()
				Console.WriteLine("Server is started. Press Enter to stop.")
#If (Not EASYTEST) Then
				Console.ReadLine()
#Else
				' 20 seconds is enough to pass all tests:
				System.Threading.Thread.Sleep(20000)
#End If
				Console.WriteLine("Stopping...")
				serviceHost.Close()
				Console.WriteLine("Server is stopped.")
			Catch e As Exception
				Console.WriteLine("Exception occurs: " & e.Message)
				Console.WriteLine("Press Enter to close.")
				Console.ReadLine()
			End Try
		End Sub
		
		'INSTANT VB TODO TASK: The return type of this anonymous method could not be determined by Instant VB:
		Private Shared Function AnonymousMethod1() As Object
			Dim security As New SecurityStrategyComplex(GetType(Employee), GetType(ExtendedSecurityRole), New AuthenticationStandard())
			AddHandler security.CustomizeRequestProcessors, Function(sender, e) AnonymousMethod2(sender, e, security, security)
			Return security
		End Function

        Private Shared Function AnonymousMethod2(ByVal sender As Object, ByVal e As CustomizeRequestProcessorsEventArgs, ByVal security As SecurityStrategyComplex, ByVal security2 As SecurityStrategyComplex) As Boolean
            Dim result As New List(Of IOperationPermission)()
            If security2 IsNot Nothing Then
                Dim user As Employee = TryCast(security2.User, Employee)
                If user IsNot Nothing Then
                    For Each role As ExtendedSecurityRole In user.Roles
                        If role.CanExport Then
                            result.Add(New ExportPermission())
                        End If
                    Next role
                End If
            End If
            Dim permissionDictionary As IPermissionDictionary = New PermissionDictionary(CType(result, IEnumerable(Of IOperationPermission)))
            e.Processors.Add(GetType(ExportPermissionRequest), New ExportPermissionRequestProcessor(permissionDictionary))
            Return True
        End Function
    End Class
End Namespace
