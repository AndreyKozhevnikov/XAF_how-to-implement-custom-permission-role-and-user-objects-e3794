Imports System.ComponentModel

Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering

Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Validation
Imports DevExpress.ExpressApp.Security
Imports DevExpress.ExpressApp.Security.Strategy

Namespace SecuredExportExample.Module.SecurityObjects
    <DefaultClassOptions, ImageName("BO_Role")> _
    Public Class ExtendedSecurityRole
        Inherits SecuritySystemRole

        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub
        Public Property CanExport() As Boolean
            Get
                Return GetPropertyValue(Of Boolean)("CanExport")
            End Get
            Set(ByVal value As Boolean)
                SetPropertyValue(Of Boolean)("CanExport", value)
            End Set
        End Property
        Protected Overrides Function GetPermissionsCore() As List(Of IOperationPermission)
            Dim result As List(Of IOperationPermission) = MyBase.GetPermissionsCore()
            If CanExport Then
                result.Add(New ExportPermission())
            End If
            Return result
        End Function
    End Class
End Namespace
