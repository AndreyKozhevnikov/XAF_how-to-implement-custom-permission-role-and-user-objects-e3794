Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic

Imports DevExpress.ExpressApp
Imports System.Reflection
Imports DevExpress.ExpressApp.Security
Imports SecuredExportExample.Module.SecurityObjects


Namespace SecuredExportExample.Module
    Public NotInheritable Partial Class SecuredExportExampleModule
        Inherits ModuleBase
        Public Sub New()
            InitializeComponent()
        End Sub
        Public Overrides Overloads Sub Setup(ByVal application As XafApplication)
            MyBase.Setup(application)
            AddHandler application.SetupComplete, AddressOf application_SetupComplete
        End Sub
        Private Sub application_SetupComplete(ByVal sender As Object, ByVal e As EventArgs)
            If TypeOf SecuritySystem.Instance Is SecurityStrategy Then
                CType(SecuritySystem.Instance, SecurityStrategy).RequestProcessors.Register(New ExportPermissionRequestProcessor())
            End If
        End Sub
    End Class
End Namespace
