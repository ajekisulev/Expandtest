Imports System
Imports System.Configuration
Imports System.Windows.Forms
Imports DevExpress.ExpressApp.Security
Imports eXpandTest.Win
Imports DevExpress.ExpressApp
Imports System.Security.Principal
Imports DevExpress.Persistent.AuditTrail
Imports eXpandTest.Module.Classes

Public Class Program

    <STAThread()> _
    Public Shared Sub Main(ByVal arguments() As String)
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached
        Dim _application As eXpandTestWindowsFormsApplication = New eXpandTestWindowsFormsApplication()

        If (Not ConfigurationManager.ConnectionStrings.Item("ConnectionString") Is Nothing) Then
            _application.ConnectionString = ConfigurationManager.ConnectionStrings.Item("ConnectionString").ConnectionString
        End If
        Try
            AuditTrailService.Instance.ObjectAuditingMode = ObjectAuditingMode.Full
            'Subscribe to QueryCurrentUserName event of the AuditTrailService's instance 
            AddHandler AuditTrailService.Instance.QueryCurrentUserName, _
            AddressOf Instance_QueryCurrentUserName
            Dim timeStampStrategy As IAuditTimestampStrategy = New MSSqlServerTimestampStrategy()
            AuditTrailService.Instance.TimestampStrategy = timeStampStrategy
            _application.Setup()
            _application.Start()
        Catch e As Exception
            _application.HandleException(e)
        End Try

    End Sub
    Shared Sub Instance_QueryCurrentUserName(ByVal sender As Object, ByVal e As QueryCurrentUserNameEventArgs)
        e.CurrentUserName = SecuritySystem.CurrentUserName & " (" & WindowsIdentity.GetCurrent().Name & ")"
    End Sub
End Class
