Imports System
Imports System.IO
Imports System.Data
Imports MySql.Data.MySqlClient
Imports MYSQLDB_DLL
Partial Class DataEntry_AttachFileA
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Buffer = True
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
        Response.Expires = -1500
        Response.CacheControl = "no-cache"
        CheckLogin()
        If Not Page.IsPostBack Then
            Me.Session("Click") = "AddDevices"
        End If
    End Sub

    Private Sub CheckLogin()
        If Me.Session("fName") = "" Then
            Response.Redirect("~/login.aspx")
        End If
    End Sub

    Public Function Enclose(ByVal str As String) As String
        If str Is Nothing Then
            Return "NULL"
        Else
            If str.Trim <> "" Then
                If str.ToUpper <> "NULL" Then
                    Return "'" & Replace(str, "'", "''") & "'"
                Else
                    Return str
                End If
            Else
                Return "NULL"
            End If
        End If
    End Function

    Protected Sub btnAttachFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAttachFile.Click
        CheckLogin()

        Dim cn As New MySqlConnection
        Dim sqlTrans As MySqlTransaction
        Dim SqlQry As String = ""
        Dim attachments As String = ""
        Dim AssetNo As String = Me.Session("txtAAsstInvNo")

        'Dim hfc As HttpFileCollection = Request.Files
        'Dim file As Byte
        'For i As Integer = 0 To hfc.Count - 1
        '    Dim hpf As HttpPostedFile = hfc(i)
        '    If hpf.ContentLength > 0 Then
        '        file = fuAttachFile.FileBytes(i)
        '        SqlQry = SqlQry & "insert into cmms_entry_attachfiles(Asset_Inv_No,File_Name,File_Pic)" & vbNewLine & _
        '                "Values(" & Enclose(AssetNo) & "," & Enclose(hpf.FileName) & "," & file & ");" & vbNewLine
        '        attachments = attachments & hpf.FileName & ","
        '    End If
        'Next

        Dim hfc As HttpFileCollection = Request.Files

        For i As Integer = 0 To hfc.Count - 1
            Dim hpf As HttpPostedFile = hfc(i)
            Dim fileName As String = Path.GetFileName(hpf.FileName)
            If hpf.ContentLength > 0 Then
                SqlQry = SqlQry & "Insert into cmms_entry_attachfiles(Asset_Inv_No,File_Name,File_Pic)" & vbNewLine & _
                        "Values(" & Enclose(AssetNo) & "," & Enclose(fileName) & ",?picturefile" & i & ");" & vbNewLine

                attachments = attachments & hpf.FileName & ","
            End If
        Next

        Dim cmd As MySqlCommand = New MySqlCommand(SqlQry, cn)
        Using cmd
            For i As Integer = 0 To hfc.Count - 1
                Dim hpf As HttpPostedFile = hfc(i)
                Dim fileLength As Integer = hpf.ContentLength
                If hpf.ContentLength > 0 Then
                    Dim imageBytes As Byte() = New Byte(fileLength - 1) {}
                    hpf.InputStream.Read(imageBytes, 0, fileLength)
                    cmd.Parameters.Add("?pictureFile" & i, MySqlDbType.Blob).Value = imageBytes
                End If
            Next
        End Using

        cn.ConnectionString = Me.Session("strCon")
        cn.Open()
        sqlTrans = cn.BeginTransaction

        Try
            cmd.Transaction = sqlTrans
            cmd.ExecuteNonQuery()

            sqlTrans.Commit()

            Session("attachments") = attachments
        Catch ex As Exception
            sqlTrans.Rollback()
            Exit Sub
        Finally
            cn.Close()
            cn.Dispose()
        End Try
        Response.Redirect("~/DataEntry/CAAddDevice.aspx")
    End Sub
End Class
