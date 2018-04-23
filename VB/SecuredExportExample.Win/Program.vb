Imports Microsoft.VisualBasic
Imports System
Imports System.Configuration
Imports System.Windows.Forms

Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Security
Imports DevExpress.ExpressApp.Win
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports SecuredExportExample.Module.SecurityObjects
Imports DevExpress.ExpressApp.Xpo
Imports System.Collections.Generic
Imports SecuredExportExample.Module.BusinessObjects

Namespace SecuredExportExample.Win
	Friend NotInheritable Class Program
		''' <summary>
		''' The main entry point for the application.
		''' </summary>
		Private Sub New()
		End Sub
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
				AddHandler (CType(winApplication.Security, SecurityStrategy)).CustomizeRequestProcessors, Function(sender, e) AnonymousMethod1(sender, e)
				winApplication.Setup()
				winApplication.Start()
			Catch e As Exception
				winApplication.HandleException(e)
			End Try
		End Sub
		
		Private Shared Function AnonymousMethod1(ByVal sender As Object, ByVal e As CustomizeRequestProcessorsEventArgs) As Boolean
				Dim result As New List(Of IOperationPermission)()
				Dim security As SecurityStrategyComplex = TryCast(sender, SecurityStrategyComplex)
				If security IsNot Nothing Then
					Dim user As Employee = TryCast(security.User, Employee)
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
