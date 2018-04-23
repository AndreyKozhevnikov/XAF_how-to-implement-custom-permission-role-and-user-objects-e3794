Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports DevExpress.ExpressApp.Security

Namespace SecuredExportExample.Module.SecurityObjects
    Public Class ExportPermission
        Inherits OperationPermissionBase
        Public Const ExportOperationName As String = "ExportOperationName"
        Public Sub New()
            MyBase.New(ExportOperationName)
        End Sub
        Public Overrides Function GetSupportedOperations() As IList(Of String)
            Return New String() { ExportOperationName }
        End Function
    End Class
    Public Class ExportPermissionRequest
        Inherits OperationPermissionRequestBase
        Public Sub New()
            MyBase.New(ExportPermission.ExportOperationName)
        End Sub

    End Class
    Public Class ExportPermissionRequestProcessor
        Inherits PermissionRequestProcessorBase(Of ExportPermissionRequest)
        Protected Overrides Function IsRequestFit(ByVal permissionRequest As ExportPermissionRequest, ByVal permission As OperationPermissionBase, ByVal securityInstance As IRequestSecurityStrategy) As Boolean
            Dim exportPermission As ExportPermission = TryCast(permission, ExportPermission)
            If permissionRequest Is Nothing OrElse exportPermission Is Nothing Then
                Return False
            End If
            Return permissionRequest.Operation = exportPermission.Operation
        End Function
    End Class
End Namespace
