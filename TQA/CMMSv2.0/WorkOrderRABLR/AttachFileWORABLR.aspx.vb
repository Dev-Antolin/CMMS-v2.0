Imports System
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient
Imports MYSQLDB_DLL
Partial Class WorkOrder_AttachFileWORABLR
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Buffer = True
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
        Response.Expires = -1500
        Response.CacheControl = "no-cache"
        CheckLogin()
        If Not Page.IsPostBack Then
            Me.Session("Click") = "CWORABLR"
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
        Dim WONo As String = Me.Session("WONo")

        'Dim hfc As HttpFileCollection = Request.Files
        'Dim file As Byte
        'For i As Integer = 0 To hfc.Count - 1
        '    Dim hpf As HttpPostedFile = hfc(i)
        '    If hpf.ContentLength > 0 Then
        '        file = fuAttachFile.FileBytes(i)
        '        SqlQry = SqlQry & "insert into cmms_wo_attachfiles(Wo_No,File_Name,File_Pic)" & vbNewLine & _
        '                "Values(" & Enclose(WONo) & "," & Enclose(hpf.FileName) & "," & file & ");" & vbNewLine
        '        attachments = attachments & hpf.FileName & ","
        '    End If
        'Next

        Dim hfc As HttpFileCollection = Request.Files

        For i As Integer = 0 To hfc.Count - 1
            Dim hpf As HttpPostedFile = hfc(i)
            If hpf.ContentLength > 0 Then
                SqlQry = SqlQry & "Insert into cmms_wo_attachfiles(Wo_No,File_Name,File_Pic)" & vbNewLine & _
                        "Values(" & Enclose(WONo) & "," & Enclose(hpf.FileName) & ",?picturefile" & i & ");" & vbNewLine

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
        Response.Redirect("~/WorkOrderRABLR/CreateWORABLR.aspx")
    End Sub

    Private Function LLRUsers(ByVal jobDesc As String) As Boolean
        Dim mySqlDes As String
        Dim ds As DataSet
        mySqlDes = "select distinct task from webaccounts where task like '%/BM/%' or task like '%Regional%' or task like '%Area%' or task like '%LPT%' or task like '%RCT-A%' or task like '%BM/BOSMAN%';"
        ds = Execute_DataSet(mySqlDes)
        For x = 0 To ds.Tables(0).Rows.Count - 1
            If jobDesc = ds.Tables(0).Rows(x)(0).ToString.Trim Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Function Execute_DataSet(ByVal as_mysql As String) As DataSet
        Dim Con As New SqlConnection
        Dim Com As New SqlCommand
        Dim sqlAdapter As SqlDataAdapter
        Dim sqlDataset As New DataSet

        Execute_DataSet = Nothing
        Try
            Try
                Con.ConnectionString = Me.Session("strConf")
                If Con.State = ConnectionState.Closed Then
                    Con.Open()
                End If
            Catch
            End Try
            sqlAdapter = New SqlDataAdapter(as_mysql, Con)
            sqlAdapter.Fill(sqlDataset)
            If Not sqlDataset Is Nothing Then
                If sqlDataset.Tables(0).Rows.Count <> 0 Then
                    Execute_DataSet = sqlDataset
                    sqlDataset.Dispose()
                    sqlAdapter.Dispose()
                End If
            End If
            Con.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
            Con.Close()
            Com.Dispose()
        End Try
    End Function

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("~/WorkOrderRABLR/CreateWORABLR.aspx")
    End Sub
End Class
