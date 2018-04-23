Imports System.ComponentModel

Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering

Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Validation
Imports DevExpress.ExpressApp.Security
Imports DevExpress.ExpressApp.Security.Strategy

Namespace SecuredExportExample.Module.BusinessObjects
	<DefaultClassOptions, ImageName("BO_Employee")> _
	Public Class Employee
		Inherits SecuritySystemUser

		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		<Association("Employee-Task")> _
		Public ReadOnly Property Tasks() As XPCollection(Of Task)
			Get
				Return GetCollection(Of Task)("Tasks")
			End Get
		End Property

	End Class
	<DefaultClassOptions, ImageName("BO_Task")> _
	Public Class Task
		Inherits BaseObject

		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Private _subject As String
		Public Property Subject() As String
			Get
				Return _subject
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Subject", _subject, value)
			End Set
		End Property
		Private _dueDate As Date
		Public Property DueDate() As Date
			Get
				Return _dueDate
			End Get
			Set(ByVal value As Date)
				SetPropertyValue("DueDate", _dueDate, value)
			End Set
		End Property
		Private _assignedTo As Employee
		<Association("Employee-Task")> _
		Public Property AssignedTo() As Employee
			Get
				Return _assignedTo
			End Get
			Set(ByVal value As Employee)
				SetPropertyValue("AssignedTo", _assignedTo, value)
			End Set
		End Property
	End Class
End Namespace
