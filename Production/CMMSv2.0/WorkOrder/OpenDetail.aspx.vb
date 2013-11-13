﻿Imports INI_DLL
Imports MYSQLDB_DLL
Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient
Imports System
Imports System.IO
Imports System.Data
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Xml.Linq
Imports System.Diagnostics
Imports System.Web.Services
Imports ENTech.WebControls
Partial Class WorkOrder_OpenDetail
    Inherits System.Web.UI.Page
    Dim OpenWorkOrderNumber As String
    Dim OpenWorkOrderAuthor As String
    Dim OpenWorkOrderPosition As String
    Dim OpenWorkOrderResolve As String
    Dim BCodeAuthor As String
    Dim BNameAuthor As String
    Dim AuthorName As String
    Dim WrkNo As String
    Dim EscalDesc As String
    Dim EscalName As String
    Dim WOTypeCode As String
    Dim AstInvNo As String
    Dim WODate As String
    Dim IRNo As String
    Dim WODesc As String
    Dim stat As String
    Dim zCode As String
    Dim task As String
    Dim EscalatedBy As String
    Dim SysCreatorID As String
    Dim SysModifierID As String
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
        Response.Buffer = True
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
        Response.Expires = -1500
        Response.CacheControl = "no-cache"
        CheckLogin()
        OpenWorkOrderNumber = Me.Session("WorkOrderNumber")
        OpenWorkOrderAuthor = Me.Session("WorkOrderAuthor")
        OpenWorkOrderPosition = Me.Session("WorkOrderPosition")
        OpenWorkOrderResolve = Me.Session("WorkOrderResolve")
        If Not Page.IsPostBack Then
            Me.Session("Click") = "OWO"
            disOpenWO()
            disAssesstment()
            disSCT()
            BindAttachment(OpenWorkOrderNumber)
        Else
            SelectedID = Request("__EVENTARGUMENT")
        End If
        If divisionUsers(OpenWorkOrderPosition) = True Then
            PanelAuthorDetail.GroupingText = "Division Information"
            lblBCodeAuthor.Text = "Division Code"
            lblBNameAuthor.Text = "Division Name"
        ElseIf divUsers(OpenWorkOrderPosition) = True Then
            PanelAuthorDetail.GroupingText = "Division Information"
            lblBCodeAuthor.Text = "Division Code"
            lblBNameAuthor.Text = "Division Name"
        ElseIf SubDivUsers(OpenWorkOrderPosition) = True Then
            PanelAuthorDetail.GroupingText = "Division Information"
            lblBCodeAuthor.Text = "Division Code"
            lblBNameAuthor.Text = "Division Name"
        ElseIf LLRUsers(OpenWorkOrderPosition) = True Then
            PanelAuthorDetail.GroupingText = "Branch Information"
            lblBCodeAuthor.Text = "BC Code"
            lblBNameAuthor.Text = "BC Name"
        End If
        If OpenWorkOrderResolve = "NO" Then
            txtAssessment.Attributes("class") = "ReadOnlyBackColor"
            txtAssessment.ReadOnly = True
            btnCloseAst.Enabled = False
        Else
            txtAssessment.Attributes("class") = "WriteBackColor"
            txtAssessment.ReadOnly = False
            btnCloseAst.Enabled = True
        End If
        btnWOOk.Visible = False
        lblCount.Text = "" & (500 - txtAssessment.Text.Length) & "/500"
    End Sub

    Private Sub CheckLogin()
        If Me.Session("fName") = "" Then
            Response.Redirect("~/login.aspx")
        End If
    End Sub

    Private Function divisionUsers(ByVal jobDesc As String) As Boolean
        Dim mySqlDes As String
        Dim ds As DataSet
        mySqlDes = "select distinct task from webaccounts where task like '%DIVMAN%' or task like '%DEPTMAN%';"
        ds = Execute_DataSet(mySqlDes)
        For x = 0 To ds.Tables(0).Rows.Count - 1
            If jobDesc.ToUpper = ds.Tables(0).Rows(x)(0).ToString.ToUpper.Trim Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Function divUsers(ByVal jobDesc As String) As Boolean
        Dim mySqlDes As String
        Dim ds As DataSet
        mySqlDes = "select distinct task from webaccounts where task like '%BOS-CONT%' or task like '%MMD-STAFF%';"
        ds = Execute_DataSet(mySqlDes)
        For x = 0 To ds.Tables(0).Rows.Count - 1
            If jobDesc.ToUpper = ds.Tables(0).Rows(x)(0).ToString.ToUpper.Trim Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Function SubDivUsers(ByVal jobDesc As String) As Boolean
        Dim mySqlDes As String
        Dim ds As DataSet
        mySqlDes = "select distinct task from webaccounts where comp = '001' and task <> 'BOS-CONT' order by task;"
        ds = Execute_DataSet(mySqlDes)
        For x = 0 To ds.Tables(0).Rows.Count - 1
            If jobDesc.ToUpper = ds.Tables(0).Rows(x)(0).ToString.ToUpper.Trim Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Function LLRUsers(ByVal jobDesc As String) As Boolean
        Dim mySqlDes As String
        Dim ds As DataSet
        mySqlDes = "select distinct task from webaccounts where task like '%/BM/%' or task like '%LPT%' or task like '%RCT-A%';"
        ds = Execute_DataSet(mySqlDes)
        For x = 0 To ds.Tables(0).Rows.Count - 1
            If jobDesc.ToUpper = ds.Tables(0).Rows(x)(0).ToString.ToUpper.Trim Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub disAssesstment()
        Dim bar As String = "=============================================="
        Dim ds As DataSet
        Dim mySQLconcat As String

        mySQLconcat = "SELECT escalated_by, correctiveAction, date_format(Sys_modified, '%Y/%m/%d') as Sys_modified FROM cmms_wo_history where wo_no = '" & OpenWorkOrderNumber.Trim & "' and escalated_to <> '" & OpenWorkOrderAuthor.Trim & "';"

        ds = Execute_DataSetCMMS(mySQLconcat)

        If Not ds Is Nothing Then
            For x = 0 To ds.Tables(0).Rows.Count - 1
                If ds.Tables(0).Rows(x)(1).ToString.Trim <> "" Then
                    txtAssessmentHist.Text += ds.Tables(0).Rows(x)(0) & ": " & vbNewLine & ds.Tables(0).Rows(x)(1) & " " & vbNewLine & ds.Tables(0).Rows(x)(2) & vbNewLine & bar & vbNewLine
                End If
            Next
        End If
    End Sub

    Private Sub disSCT()
        Dim bar As String = "=============================================="
        Dim ds As DataSet
        Dim mySQLconcat As String

        mySQLconcat = "SELECT escalated_by, correctiveAction, date_format(Sys_modified, '%Y/%m/%d') as Sys_modified FROM cmms_wo_detail where escalated_by = '" & EscalatedBy.Trim & "' and wo_no = '" & OpenWorkOrderNumber.Trim & "';"

        ds = Execute_DataSetCMMS(mySQLconcat)

        If Not ds Is Nothing Then
            For x = 0 To ds.Tables(0).Rows.Count - 1
                If ds.Tables(0).Rows(x)(1).ToString.Trim <> "" Then
                    txtSCTCorect.Text += ds.Tables(0).Rows(x)(0) & ": " & vbNewLine & ds.Tables(0).Rows(x)(1) & " " & vbNewLine & ds.Tables(0).Rows(x)(2) & vbNewLine & bar & vbNewLine
                End If
            Next
        End If
    End Sub

    Private Sub disOpenWO()
        Dim connString As String = Me.Session("strCon")
        Dim cn As MySqlConnection = New MySqlConnection(connString)
        Dim reader As MySqlDataReader = Nothing
        Dim mySqlRecWONum As String

        mySqlRecWONum = "select t1.bc_code_author, " _
                      & "t1.bc_name_author, " _
                      & "t2.sys_creator, " _
                      & "t1.Author_name, " _
                      & "t1.wo_no, " _
                      & "t1.escalated_name, " _
                      & "t3.wo_type_desc, " _
                      & "t1.asset_inv_no, " _
                      & "date_format(t2.escalated_date, '%Y-%m-%d')as escalated_date, " _
                      & "t1.ir_no, " _
                      & "t1.wo_desc, " _
                      & "t2.escalated_by " _
                      & "from cmms_wo_masterheader as t1 " _
                      & "inner join cmms_wo_detail as t2 " _
                      & "on t1.wo_no = t2.wo_no " _
                      & "inner join cmms_wo_type as t3 " _
                      & "on t1.wo_type_code = t3.wo_type_code " _
                      & "where t1.wo_no = '" & OpenWorkOrderNumber.Trim & "';"
        Try
            Dim cmd As MySqlCommand = New MySqlCommand(mySqlRecWONum, cn)
            cn.Open()
            cmd.CommandTimeout = 0
            reader = cmd.ExecuteReader()

            If Not reader Is Nothing Then
                While reader.Read
                    txtBCodeAuthor.Text = reader.Item("bc_code_author")
                    txtBNameAuthor.Text = reader.Item("bc_name_author")
                    txtAuthorID.Text = reader.Item("sys_creator")
                    txtAuthorName.Text = reader.Item("Author_name")
                    txtWorkOrderNo.Text = reader.Item("wo_no")
                    txtEscalationName.Text = reader.Item("escalated_name")
                    txtWorkOrderType.Text = reader.Item("wo_type_desc")
                    txtAssetInventory.Text = reader.Item("asset_inv_no")
                    txtDate.Text = reader.Item("escalated_date")
                    txtIRNumber.Text = reader.Item("ir_no")
                    txtWorkOrderDesc.Text = reader.Item("wo_desc")
                    EscalatedBy = reader.Item("escalated_by")
                End While
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            reader.Close()
            cn.Close()
            cn.Dispose()
        End Try
    End Sub

    Public Function Execute_DataSetCMMS(ByVal as_mysql As String) As DataSet
        Dim Con As New MySqlConnection
        Dim Com As New MySqlCommand
        Dim sqlAdapter As MySqlDataAdapter
        Dim sqlDataset As New DataSet

        Execute_DataSetCMMS = Nothing
        Try
            Try
                Con.ConnectionString = Me.Session("strCon")
                If Con.State = ConnectionState.Closed Then
                    Con.Open()
                End If
            Catch
            End Try
            sqlAdapter = New MySqlDataAdapter(as_mysql, Con)
            sqlAdapter.Fill(sqlDataset)
            If Not sqlDataset Is Nothing Then
                If sqlDataset.Tables(0).Rows.Count <> 0 Then
                    Execute_DataSetCMMS = sqlDataset
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

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            CheckLogin()
        End If
    End Sub

    Protected Sub btnCloseAst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseAst.Click
        WrkNo = txtWorkOrderNo.Text
        AuthorName = Me.Session("fname")
        SysCreatorID = Me.Session("res_id")
        SysModifierID = Me.Session("res_id")
        If txtAssessment.Text = "" Then
            lblWOError.Text = "Please specify your assessment!"
            Exit Sub
        End If

        If DataCloseWorkOrder() = False Then
            lblWOError.Text = Me.Session("InsertErrorRec")
        Else
            lblWOError.Text = "Work order is successfully closed!"
            btnCloseAst.Enabled = False
            txtAssessment.Enabled = False
            PanelWorkOrderInfo.Enabled = False
            btnWOOk.Visible = True
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

    Public Function EscapeApostrophe(ByVal as_string As String) As String
        EscapeApostrophe = Replace(as_string, "'", "`")
    End Function

    Private Function DataCloseWorkOrder() As Boolean
        Dim con As New MySqlConnection
        Dim com As New MySqlCommand
        Dim oTran As MySqlTransaction
        Dim mySqlInsertDetail As String
        mySqlInsertDetail = "update cmms_wo_masterheader set wo_status = 'CLOSE', sys_modified = now() where wo_no='" & WrkNo.Trim & "';" _
                          & "insert into cmms_wo_history(wo_no,escalated_date,escalated_by,escalated_to,resolve,correctiveaction,sys_created,sys_modified,sys_creator,sys_modifier)" _
                          & "values('" & WrkNo.Trim & "',now(),'" & AuthorName.Trim & "','" & AuthorName.Trim & "','Y','" & EscapeApostrophe(txtAssessment.Text.Trim) & "',now(),now(),'" & SysCreatorID.Trim & "','" & SysModifierID.Trim & "');"
        '& "update cmms_wo_history set correctiveaction = '" & EscapeApostrophe(txtAssessment.Text.Trim) & "', sys_modified = now(), sys_modifier = '" & SysModifierID.Trim & "'" _
        '& "where wo_no = '" & WrkNo.Trim & "' and resolve = 'Y';"
        con.ConnectionString = Me.Session("strCon")
        If con.State = ConnectionState.Closed Then
            con.Open()
            oTran = con.BeginTransaction()
            com = con.CreateCommand
            com.CommandTimeout = 0
            com.Transaction = oTran
            Try
                com.CommandText = mySqlInsertDetail
                com.ExecuteNonQuery()
                oTran.Commit()
                com.Dispose()
                Return True
            Catch ex As Exception
                Me.Session.Add("InsertErrorRec", ex.Message)
                oTran.Rollback()
                con.Close()
                com.Dispose()
                Return False
            End Try
        End If
    End Function

    Protected Sub btnWOOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWOOk.Click
        Response.Redirect("OpenWO.aspx")
    End Sub
    Private Sub BindAttachment(ByVal WONo As String)

        Dim strCon As String = ConfigurationManager.ConnectionStrings("CMMS").ConnectionString
        Dim sql As String
        Dim cn As MySqlConnection = New MySqlConnection(strCon)

        sql = "select File_Name,File_Pic,@rownum:=@rownum+1 as rank,CONCAT(CONCAT(CONVERT(@rownum,CHAR(3)), '/'),File_Name) as RAttchFileName from cmms_wo_attachfiles, (SELECT @rownum:=0) r  where WO_No = '" & WONo & "'"

        Try
            Dim cmd As MySqlCommand = New MySqlCommand(sql, cn)
            cn.Open()

            Dim reader As MySqlDataReader = cmd.ExecuteReader

            attList.DataSource = reader
            attList.DataBind()

            reader.Close()
        Catch ex As Exception
            Throw New Exception(ex.ToString)
        Finally
            cn.Close()
            cn.Dispose()
        End Try
    End Sub

    Protected Sub btnDownload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDownload.Click
        If Page.IsPostBack Then
            Dim retVal As String = SelectedID
            Dim fileName As String = retVal.Substring(InStrRev(retVal, "="))
            Dim WONo As String = txtWorkOrderNo.Text
            FileDownLoad(fileName, WONo)
        End If
    End Sub

    Private Sub FileDownLoad(ByVal FileName As String, ByVal WONo As String)
        Dim fileData As Byte() = GetFileFromDB(FileName, WONo)
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

    Public Shared Function GetFileFromDB(ByVal filename As String, ByVal WONo As String) As Byte()
        Dim file As Byte() = Nothing
        Dim _connString As String = ConfigurationManager.ConnectionStrings("CMMS").ConnectionString.ToString()
        Dim cn As New MySqlConnection(_connString)

        Dim sql As String = "select File_Pic from cmms_wo_attachfiles where WO_No = '" & WONo & "' and File_Name='" & filename & "'"

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
End Class
