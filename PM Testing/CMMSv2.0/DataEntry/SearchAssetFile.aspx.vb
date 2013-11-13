Imports System
Imports System.IO
Imports System.Data

Imports INI_DLL
Imports MySql.Data.MySqlClient
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Xml.Linq

Imports MYSQLDB_DLL
Imports System.Data.SqlClient
Imports System.Diagnostics

Imports System.Web.Services
Imports ENTech.WebControls
Imports System.Text.RegularExpressions
Partial Class DataEntry_SearchAssetFile
    Inherits System.Web.UI.Page
    Dim AssetNoReq As String
    Dim SelectedID As String = String.Empty

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        AddLinkkSelectToRepeater()
        MyBase.Render(writer)
    End Sub

    Private Sub AddLinkkSelectToRepeater()
        For i As Integer = 0 To attList.Items.Count - 1
            Dim lnk As LinkButton = DirectCast(attList.Items(i).FindControl("attchmnt"), LinkButton)
            lnk.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(btnDownload, "Download=" & lnk.Text, True))
        Next
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnAttachSearch.Visible = False
        If Not Page.IsPostBack Then
            Me.Session("Click") = "SearchAAssetNo"
            BindAttachment(txtAttachSearchFile.Text)
        Else
            SelectedID = Request("__EVENTARGUMENT")
        End If
    End Sub

    Protected Sub btnAttachSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAttachSearch.Click
        If AssetNo() = "True" Then
            BindAttachment(AssetNoReq.Trim)
        Else
            Me.attList.DataSource = Nothing
            lblErrorSearch.Text = "No attachment(s) found!"
        End If
    End Sub

    Private Function AssetNo() As String
        Dim ds As DataSet
        Dim mySqlSel As String

        Try
            mySqlSel = "Select asset_inv_no from cmms_entry_attachfiles where asset_inv_no = '" & txtAttachSearchFile.Text.Trim & "';"

            ds = Execute_DataSet(mySqlSel)

            If Not ds Is Nothing Then
                AssetNoReq = ds.Tables(0).Rows(0).Item(0).ToString.Trim
                Return "True"
            Else
                Return "False"
            End If
        Catch ex As Exception
            Return "False"
        End Try
    End Function

    Public Function Execute_DataSet(ByVal as_mysql As String) As DataSet
        Dim Con As New MySqlConnection
        Dim Com As New MySqlCommand
        Dim sqlAdapter As MySqlDataAdapter
        Dim sqlDataset As New DataSet

        Execute_DataSet = Nothing
        Try
            Try
                Con.ConnectionString = ConfigurationManager.ConnectionStrings("CMMS").ConnectionString
                If Con.State = ConnectionState.Closed Then
                    Con.Open()
                End If
            Catch
            End Try
            sqlAdapter = New mySqlDataAdapter(as_mysql, Con)
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

    Private Sub BindAttachment(ByVal AssetNo As String)

        Dim strCon As String = ConfigurationManager.ConnectionStrings("CMMS").ConnectionString
        Dim sql As String
        Dim cn As MySqlConnection = New MySqlConnection(strCon)

        sql = "select File_Name,File_Pic,@rownum:=@rownum+1 as rank,CONCAT(CONCAT(CONVERT(@rownum,CHAR(3)), '/'),File_Name) as RAttchFileName from cmms_entry_attachfiles, (SELECT @rownum:=0) r  where asset_inv_no = '" & AssetNo & "'"

        Try
            Dim cmd As MySqlCommand = New MySqlCommand(sql, cn)
            cn.Open()
            Dim reader As MySqlDataReader = cmd.ExecuteReader()
            attList.DataSource = reader
            attList.DataBind()
            reader.Close()
        Catch ex As Exception
            Throw
        Finally
            cn.Close()
            cn.Dispose()
        End Try
    End Sub

    Protected Sub btnDownload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDownload.Click
        If Page.IsPostBack Then
            Dim retVal As String = SelectedID
            Dim fileName As String = retVal.Substring(InStrRev(retVal, "="))
            Dim AssetNo As String = txtAttachSearchFile.Text.Trim
            FileDownLoad(fileName, AssetNo)
        End If
    End Sub

    Private Sub FileDownLoad(ByVal FileName As String, ByVal AssetNo As String)
        Dim fileData As Byte() = GetFileFromDB(FileName, AssetNo)
        Dim sExtension As String = FileName.Substring(InStrRev(FileName, ".") - 1).ToLower
        Response.ClearContent()
        Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName)
        Dim bw As BinaryWriter = New BinaryWriter(Response.OutputStream)
        bw.Write(fileData)
        bw.Close()
        Response.ContentType = ReturnExtension(sExtension)
        Response.End()
    End Sub

#Region "ReturnExtension"
    Private Function ReturnExtension(ByVal fileExtension As String) As String
        Select Case fileExtension
            Case ".htm", ".html", ".log"
                Return "text/HTML"
            Case ".txt"
                Return "text/plain"
            Case ".doc"
                Return "application/ms-word"
            Case ".tiff", ".tif"
                Return "image/tiff"
            Case ".asf"
                Return "video/x-ms-asf"
            Case ".avi"
                Return "video/avi"
            Case ".zip"
                Return "application/zip"
            Case ".xls", ".csv"
                Return "application/vnd.ms-excel"
            Case ".gif"
                Return "image/gif"
            Case ".jpg", "jpeg"
                Return "image/jpeg"
            Case ".bmp"
                Return "image/bmp"
            Case ".wav"
                Return "audio/wav"
            Case ".mp3"
                Return "audio/mpeg3"
            Case ".mpg", "mpeg"
                Return "video/mpeg"
            Case ".rtf"
                Return "application/rtf"
            Case ".asp"
                Return "text/asp"
            Case ".pdf"
                Return "application/pdf"
            Case ".fdf"
                Return "application/vnd.fdf"
            Case ".ppt"
                Return "application/mspowerpoint"
            Case ".dwg"
                Return "image/vnd.dwg"
            Case ".msg"
                Return "application/msoutlook"
            Case ".xml", ".sdxl"
                Return "application/xml"
            Case ".xdp"
                Return "application/vnd.adobe.xdp+xml"
            Case Else
                Return "application/octet-stream"
        End Select
    End Function
#End Region

    Public Shared Function GetFileFromDB(ByVal filename As String, ByVal AssetNo As String) As Byte()
        Dim file As Byte() = Nothing
        Dim _connString As String = ConfigurationManager.ConnectionStrings("CMMS").ConnectionString.ToString()
        Dim cn As New MySqlConnection(_connString)

        Dim sql As String = "select File_Pic from cmms_entry_attachfiles where asset_inv_no = '" & AssetNo & "' and File_Name='" & filename & "'"

        Try
            Dim cmd As MySqlCommand = New MySqlCommand(sql, cn)
            cn.Open()

            Dim dr As MySqlDataReader = cmd.ExecuteReader
            If (dr.Read()) Then
                file = DirectCast(dr("File_Pic"), Byte())
            End If

        Catch ex As Exception

        Finally
            cn.Close()
            cn.Dispose()
        End Try

        Return file
    End Function

    Protected Sub ibSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibSearch.Click
        If txtAttachSearchFile.Text = "" Then
            lblErrorSearch.Text = "Fill in asset number to query!"
            Exit Sub
        End If
        If AssetNo() = "True" Then
            BindAttachment(AssetNoReq.Trim)
            Me.lblErrorSearch.Text = Nothing
        Else
            Using con As New MySqlConnection(Me.Session("strCon"))
                Using cmd As New MySqlCommand("SELECT Asset_Inv_No FROM cmms.cmms_entry_masterheader WHERE Asset_Inv_No = @Asset; ", con)
                    cmd.Parameters.Add("Asset", MySqlDbType.VarChar, 15).Value = Me.txtAttachSearchFile.Text
                    cmd.CommandType = CommandType.Text
                    Try
                        con.Open()
                        Dim result As String = cmd.ExecuteScalar
                        If result = Nothing Then
                            Me.lblErrorSearch.Text = "No attached file found on Asset number " & txtAttachSearchFile.Text.ToUpper
                        Else
                            Me.lblErrorSearch.Text = "Asset number " & txtAttachSearchFile.Text.ToUpper & " not found"
                        End If
                    Catch ex As Exception
                        Me.lblErrorSearch.Text = ex.Message
                    End Try
                End Using
            End Using
            Me.attList.DataSource = Nothing
            Me.attList.DataBind()
        End If
    End Sub

    Protected Sub txtAttachSearchFile_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAttachSearchFile.TextChanged
        If IsPostBack Then
            If Me.txtAttachSearchFile.Text = Nothing Then
                Me.lblErrorSearch.Text = Nothing
                Me.btnAttachSearch.Enabled = False
                Exit Sub
            End If
            If Regex.IsMatch(Me.txtAttachSearchFile.Text, "^\d{3}-[0-9]{2}[0-9a-zA-Z]{1}-\d{4}-\d{5}$") Then
                Me.lblErrorSearch.Text = Nothing
                Me.ibSearch.Enabled = True
            Else
                Me.lblErrorSearch.Text = "Incorrect format! AssetCode-BcCode-yyMM-00000"
                Me.ibSearch.Enabled = False
            End If
            Me.attList.DataSource = Nothing
            Me.attList.DataBind()
        End If
    End Sub

    Public Sub Reload()
        If txtAttachSearchFile.Text = "" Then
            lblErrorSearch.Text = "Fill in asset number to query!"
            Exit Sub
        End If
        If AssetNo() = "True" Then
            BindAttachment(AssetNoReq.Trim)
        Else
            lblErrorSearch.Text = "Asset number " & txtAttachSearchFile.Text.ToUpper & " not found."
        End If
    End Sub

End Class
