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

Namespace SecuredExportExample.Module.SecurityObjects
    <DefaultClassOptions, ImageName("BO_Role")> _
    Public Class ExtendedSecurityRole
        Inherits SecurityRole
        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub
        Private Function FindExportPermissionData() As ExportPermissionData
            For Each permissionData As PermissionData In PersistentPermissions
                Dim exportPermissionData As ExportPermissionData = TryCast(permissionData, ExportPermissionData)
                If exportPermissionData IsNot Nothing Then
                    Return exportPermissionData
                End If
            Next permissionData
            Return Nothing
        End Function
        <NonPersistent> _
        Public Property CanExport() As Boolean
            Get
                Dim exportPermissionData As ExportPermissionData = FindExportPermissionData()
                Return exportPermissionData IsNot Nothing AndAlso exportPermissionData.CanExport
            End Get
            Set(ByVal value As Boolean)
                If (Not IsLoading) Then
                    Dim exportPermissionData As ExportPermissionData = FindExportPermissionData()
                    If value Then
                        If exportPermissionData Is Nothing Then
                            exportPermissionData = New ExportPermissionData(Session)
                            PersistentPermissions.Add(exportPermissionData)
                        End If
                        exportPermissionData.CanExport = True
                    Else
                        If exportPermissionData IsNot Nothing Then
                            exportPermissionData.CanExport = False

                        End If
                    End If
                End If
            End Set
        End Property
    End Class
End Namespace
