Imports System
Imports System.IO

Imports INI_DLL
Imports MYSQLDB_DLL
Imports MySql.Data.MySqlClient
Imports Microsoft.VisualBasic

Public Class clsConDataBase
    Private Shared server As String
    Private Shared db As String
    Private Shared uName As String
    Private Shared pWord As String
    Private Shared rdr As New ReadWriteINI
    Private Shared strCon As String
    Private Shared ini_Path As String
    Private Shared Sub _DatabaseConnection()
        ini_Path = AppDomain.CurrentDomain.BaseDirectory & "CMMS.ini"
        server = rdr.readINI("CMMS SERVER VISMIN", "SERVER", False, ini_Path)
        db = rdr.readINI("CMMS SERVER VISMIN", "DBNAME", False, ini_Path)
        uName = rdr.readINI("CMMS SERVER VISMIN", "USERNAME", False, ini_Path)
        pWord = rdr.readINI("CMMS SERVER VISMIN", "PASSWORD", False, ini_Path)
        strCon = "user id=" & uName & ";password=" & pWord & ";data source=" & server & ";persist security info=False;initial catalog=" & db & " ;pooling=false "
        HttpContext.Current.Session("HO") = CStr("VISMIN")
        HttpContext.Current.Session("server") = CStr(server)
        HttpContext.Current.Session("db") = CStr(db)
        HttpContext.Current.Session("pWord") = CStr(pWord)
        HttpContext.Current.Session("uName") = CStr(uName)
        HttpContext.Current.Session("strCon") = CStr(strCon)
    End Sub
End Class
