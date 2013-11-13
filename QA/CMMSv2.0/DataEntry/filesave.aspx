<%@ Page language="vb" debug="true" %>
<%@ Import NameSpace = "csNetUpload" %>
<%@ Import Namespace = "System.Data.SqlClient" %>
<%@ Import Namespace = "MySql.Data.MySqlClient" %>
<%
    'Change the NameSpace to "csNetUpload" if used with the full version
    If Me.Session("ZCode") = "VISMIN" Then
        Dim Upload As New UploadClass
        Dim d As String
        d = Format(Date.Now, "01MMddyyyyhhmmsstt").ToString
        Upload.ReadUpload()
        Upload.SaveFile(0, Server.MapPath("./CMMSVisScan/" & d.Trim & "") & Upload.FileName(0))
        
    End If
    
    If Trim(Me.Session("ZCode")) = "LUZON" Then
        Dim Upload As New UploadClass
        Dim d As String
        d = Format(Date.Now, "02MMddyyyyhhmmsstt").ToString
        Upload.ReadUpload()
        Upload.SaveFile(0, Server.MapPath("./CMMSLuzScan/" & d.Trim & "") & Upload.FileName(0))
    End If
%>
