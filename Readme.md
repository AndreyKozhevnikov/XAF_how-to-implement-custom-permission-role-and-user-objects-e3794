<!-- default file list -->
*Files to look at*:

* [Employee.cs](./CS/SecuredExportExample.Module/BusinessObjects/Employee.cs) (VB: [Employee.vb](./VB/SecuredExportExample.Module/BusinessObjects/Employee.vb))
* [SecuredExportController.cs](./CS/SecuredExportExample.Module/Controllers/SecuredExportController.cs) (VB: [SecuredExportController.vb](./VB/SecuredExportExample.Module/Controllers/SecuredExportController.vb))
* [Updater.cs](./CS/SecuredExportExample.Module/DatabaseUpdate/Updater.cs) (VB: [Updater.vb](./VB/SecuredExportExample.Module/DatabaseUpdate/Updater.vb))
* [Model.DesignedDiffs.xafml](./CS/SecuredExportExample.Module/Model.DesignedDiffs.xafml) (VB: [Model.DesignedDiffs.xafml](./VB/SecuredExportExample.Module/Model.DesignedDiffs.xafml))
* [Module.cs](./CS/SecuredExportExample.Module/Module.cs) (VB: [Module.vb](./VB/SecuredExportExample.Module/Module.vb))
* [ExportPermission.cs](./CS/SecuredExportExample.Module/SecurityObjects/ExportPermission.cs) (VB: [ExportPermission.vb](./VB/SecuredExportExample.Module/SecurityObjects/ExportPermission.vb))
* [ExtendedSecurityRole.cs](./CS/SecuredExportExample.Module/SecurityObjects/ExtendedSecurityRole.cs) (VB: [ExtendedSecurityRole.vb](./VB/SecuredExportExample.Module/SecurityObjects/ExtendedSecurityRole.vb))
* [Default.aspx](./CS/SecuredExportExample.Web/Default.aspx) (VB: [Default.aspx.vb](./VB/SecuredExportExample.Web/Default.aspx.vb))
* [Default.aspx.cs](./CS/SecuredExportExample.Web/Default.aspx.cs) (VB: [Default.aspx.vb](./VB/SecuredExportExample.Web/Default.aspx.vb))
* [DefaultVertical.aspx](./CS/SecuredExportExample.Web/DefaultVertical.aspx) (VB: [DefaultVertical.aspx.vb](./VB/SecuredExportExample.Web/DefaultVertical.aspx.vb))
* [DefaultVertical.aspx.cs](./CS/SecuredExportExample.Web/DefaultVertical.aspx.cs) (VB: [DefaultVertical.aspx.vb](./VB/SecuredExportExample.Web/DefaultVertical.aspx.vb))
* [Dialog.aspx](./CS/SecuredExportExample.Web/Dialog.aspx) (VB: [Dialog.aspx](./VB/SecuredExportExample.Web/Dialog.aspx))
* [Dialog.aspx.cs](./CS/SecuredExportExample.Web/Dialog.aspx.cs) (VB: [Dialog.aspx](./VB/SecuredExportExample.Web/Dialog.aspx))
* [Error.aspx](./CS/SecuredExportExample.Web/Error.aspx) (VB: [Error.aspx.vb](./VB/SecuredExportExample.Web/Error.aspx.vb))
* [Error.aspx.cs](./CS/SecuredExportExample.Web/Error.aspx.cs) (VB: [Error.aspx.vb](./VB/SecuredExportExample.Web/Error.aspx.vb))
* [Global.asax](./CS/SecuredExportExample.Web/Global.asax) (VB: [Global.asax](./VB/SecuredExportExample.Web/Global.asax))
* [Global.asax.cs](./CS/SecuredExportExample.Web/Global.asax.cs) (VB: [Global.asax](./VB/SecuredExportExample.Web/Global.asax))
* [Login.aspx](./CS/SecuredExportExample.Web/Login.aspx) (VB: [Login.aspx.vb](./VB/SecuredExportExample.Web/Login.aspx.vb))
* [Login.aspx.cs](./CS/SecuredExportExample.Web/Login.aspx.cs) (VB: [Login.aspx.vb](./VB/SecuredExportExample.Web/Login.aspx.vb))
* [Model.xafml](./CS/SecuredExportExample.Web/Model.xafml) (VB: [Model.xafml](./VB/SecuredExportExample.Web/Model.xafml))
* [MoveFooter.js](./CS/SecuredExportExample.Web/MoveFooter.js) (VB: [MoveFooter.js](./VB/SecuredExportExample.Web/MoveFooter.js))
* [NestedFrameControl.ascx](./CS/SecuredExportExample.Web/NestedFrameControl.ascx) (VB: [NestedFrameControl.ascx](./VB/SecuredExportExample.Web/NestedFrameControl.ascx))
* [NestedFrameControl.ascx.cs](./CS/SecuredExportExample.Web/NestedFrameControl.ascx.cs) (VB: [NestedFrameControl.ascx](./VB/SecuredExportExample.Web/NestedFrameControl.ascx))
* [SessionKeepAliveReconnect.aspx](./CS/SecuredExportExample.Web/SessionKeepAliveReconnect.aspx) (VB: [SessionKeepAliveReconnect.aspx.vb](./VB/SecuredExportExample.Web/SessionKeepAliveReconnect.aspx.vb))
* [SessionKeepAliveReconnect.aspx.cs](./CS/SecuredExportExample.Web/SessionKeepAliveReconnect.aspx.cs) (VB: [SessionKeepAliveReconnect.aspx.vb](./VB/SecuredExportExample.Web/SessionKeepAliveReconnect.aspx.vb))
* [TemplateScripts.js](./CS/SecuredExportExample.Web/TemplateScripts.js) (VB: [TemplateScripts.js](./VB/SecuredExportExample.Web/TemplateScripts.js))
* [WebApplication.cs](./CS/SecuredExportExample.Web/WebApplication.cs) (VB: [WebApplication.vb](./VB/SecuredExportExample.Web/WebApplication.vb))
* [Model.xafml](./CS/SecuredExportExample.Win/Model.xafml) (VB: [Model.xafml](./VB/SecuredExportExample.Win/Model.xafml))
* [Program.cs](./CS/SecuredExportExample.Win/Program.cs) (VB: [Program.vb](./VB/SecuredExportExample.Win/Program.vb))
* [WinApplication.cs](./CS/SecuredExportExample.Win/WinApplication.cs) (VB: [WinApplication.vb](./VB/SecuredExportExample.Win/WinApplication.vb))
<!-- default file list end -->
# How to: Implement Custom Permission, Role and User Objects


<p>This example illustrates how to create custom security objects, such as permissions, roles and users. We will implement a permission that allows administrators to secure the exporting functionality in an XAF application. The complete description is available in the <a href="http://documentation.devexpress.com/#Xaf/CustomDocument3384"><u>How to: Implement Custom Permission, Role and User Objects</u></a> topic.<br><img src="https://raw.githubusercontent.com/DevExpress-Examples/how-to-implement-custom-permission-role-and-user-objects-e3794/12.1.7+/media/00ffc31d-8a0d-47e5-a763-d7f07e79e52d.png"></p>

<br/>


