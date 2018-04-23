Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel

Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering

Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Validation
Imports DevExpress.ExpressApp.Security
Imports System.Collections.Generic

Namespace SecuredExportExample.Module.SecurityObjects
    <System.ComponentModel.DisplayName("ExportOperationName Permission")> _
    Public Class ExportPermissionData
        Inherits PermissionData
        Private _canExport As Boolean = True
        Protected Overrides Function GetPermissionInfoCaption() As String
            Return "ExportOperationName"
        End Function
        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub
        Public Overrides Function GetPermissions() As IList(Of IOperationPermission)
            Dim result As IList(Of IOperationPermission) = New List(Of IOperationPermission)()
            If _canExport Then
                result.Add(New ExportPermission())
            End If
            Return result
        End Function
        Public Property CanExport() As Boolean
            Get
                Return _canExport
            End Get
            Set(ByVal value As Boolean)
                SetPropertyValue("CanExport", _canExport, value)
            End Set
        End Property
    End Class

End Namespace
