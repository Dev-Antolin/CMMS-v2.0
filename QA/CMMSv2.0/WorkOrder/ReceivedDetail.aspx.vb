Imports INI_DLL
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
Partial Class WorkOrder_ReceivedDetail
    Inherits System.Web.UI.Page
    Dim RecWorkOrderNumber As String
    Dim RecWorkOrderAuthor As String
    Dim RecWorkOrderPosition As String
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
    Dim RecException As String
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

    Protected Overrides Sub OnInit(ByVal e As EventArgs)
        MyBase.OnInit(e)
        DIG.DesignationDiv = New DivDesigDelegate(AddressOf DIG_Selected)
        DIG2.DesignationReg = New RegDesigLPTLDelegate(AddressOf DIG2_Selected)
        DIG3.DesignationLPT = New LPTDesigDelegate(AddressOf DIG3_Selected)
        DIG4.DesignationRCT = New RCTDesigDelegate(AddressOf DIG4_Selected)
        DIG.DesigToFind = New SearchDelegate(AddressOf DIG_Search)
        DIG2.DesigToFindR = New SearchDelegate(AddressOf DIG2_Search)
        DIG3.DesigToFindL = New SearchDelegate(AddressOf DIG3_Search)
        DIG4.DesigToFindT = New SearchDelegate(AddressOf DIG4_Search)
    End Sub

    Public Sub DIG_Selected(ByVal Name As String)
        lblWOSelectEscal.Text = Name
        ddlEscalateTo.Enabled = True
    End Sub

    Public Sub DIG2_Selected(ByVal Name As String)
        lblWOSelectEscal.Text = Name
        ddlEscalateTo.Enabled = True
    End Sub

    Public Sub DIG3_Selected(ByVal Name As String)
        lblWOSelectEscal.Text = Name
        ddlEscalateTo.Enabled = True
    End Sub

    Public Sub DIG4_Selected(ByVal Name As String)
        lblWOSelectEscal.Text = Name
        ddlEscalateTo.Enabled = True
    End Sub

    Public Sub DIG_Search(ByVal JTtoSearch As String)
        If JTtoSearch = "Search" Then
            MPEDesignation.Show()
        End If
    End Sub
    Public Sub DIG2_Search(ByVal JTtoSearch As String)
        If JTtoSearch = "Search" Then
            MPEDesignation2.Show()
        End If
    End Sub

    Public Sub DIG3_Search(ByVal JTtoSearch As String)
        If JTtoSearch = "Search" Then
            MPEDesignation3.Show()
        End If
    End Sub

    Public Sub DIG4_Search(ByVal JTtoSearch As String)
        If JTtoSearch = "Search" Then
            MPEDesignation4.Show()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Disable Cache
        Response.Buffer = True
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
        Response.Expires = -1500
        Response.CacheControl = "no-cache"
        CheckLogin()
        RecWorkOrderNumber = Me.Session("WorkOrderNumber")
        RecWorkOrderAuthor = Me.Session("WorkOrderAuthor")
        RecWorkOrderPosition = Me.Session("WorkOrderPosition")
        RecException = Me.Session("JCodeName")
        If Not Page.IsPostBack Then
            Me.Session("Click") = "RWO"
            ddlDesignationTo.Visible = False
            lblWOSelectEscal.Visible = False
            lbAssignToSrch.Visible = False
            disRecWO()
            disAssesstment()
            BindAttachment(RecWorkOrderNumber)
        Else
            SelectedID = Request("__EVENTARGUMENT")
        End If
    
        If divisionUsers(RecWorkOrderPosition) = True Then
            PanelAuthorDetail.GroupingText = "Division Information"
            lblBCodeAuthor.Text = "Division Code"
            lblBNameAuthor.Text = "Division Name"
        ElseIf divUsers(RecWorkOrderPosition) = True Then
            PanelAuthorDetail.GroupingText = "Division Information"
            lblBCodeAuthor.Text = "Division Code"
            lblBNameAuthor.Text = "Division Name"
        ElseIf SubDivUsers(RecWorkOrderPosition) = True Then
            PanelAuthorDetail.GroupingText = "Division Information"
            lblBCodeAuthor.Text = "Division Code"
            lblBNameAuthor.Text = "Division Name"
        ElseIf LLRUsers(RecWorkOrderPosition) = True Then
            PanelAuthorDetail.GroupingText = "Branch Information"
            lblBCodeAuthor.Text = "BC Code"
            lblBNameAuthor.Text = "BC Name"
        End If
        ddlEscalateTo.Enabled = False
        btnWOOk.Visible = False
        lblCountHist.Text = "" & (500 - txtAssessmentHist.Text.Length) & "/500"
        lblCount.Text = "" & (500 - txtAssessment.Text.Length + 19) & "/500"
        Me.Session.Add("Designation", ddlDesignationTo.Text)
        Me.Session.Add("RecAuthor", RecWorkOrderAuthor)
        Me.Session.Add("Desig", ddlEscalateTo.Text)
    End Sub

    Private Sub CheckLogin()
        If Me.Session("fName") = "" Then
            Response.Redirect("~/login.aspx")
        End If
    End Sub

    Protected Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        If Not Page.IsPostBack Then
            If divisionUsers(Me.Session("JDesc")) = True Then
                SelectEscalDiv()
            ElseIf divUsers(Me.Session("JDesc")) = True Then
                SelectEscalRegDiv()
            ElseIf SubDivUsers(Me.Session("JDesc")) = True Then
                SelectEscalDiv()
            ElseIf LLRUsers(Me.Session("JDesc")) = True Then
                SelectEscalLPTRCTDiv()
            End If
        End If
    End Sub

    Protected Sub ddlDesignationTo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDesignationTo.SelectedIndexChanged
        lblWOSelectEscal.Text = ""
        ddlEscalateTo.Enabled = True
        If ddlDesignationTo.Text = "0" Then
            lbAssignToSrch.Visible = False
        Else
            lbAssignToSrch.Visible = True
        End If
    End Sub

    Protected Sub lbReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbReturn.Click
        Response.Redirect("ReceivedWO.aspx")
    End Sub

    Protected Sub lbAssignToSrch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles lbAssignToSrch.Click
        If ddlEscalateTo.Text = "0" Then
            Exit Sub
        ElseIf ddlEscalateTo.Text = "DIVISION" Then
            If ddlDesignationTo.Text = "0" Then
                Exit Sub
            Else
                MPEDesignation.Show()
            End If
        ElseIf ddlEscalateTo.Text = "REGION" Then
            If ddlDesignationTo.Text = "0" Then
                Exit Sub
            Else
                MPEDesignation2.Show()
            End If
        ElseIf ddlEscalateTo.Text = "LPT" Then
            If ddlEscalateTo.Text = "0" Then
                Exit Sub
            Else
                MPEDesignation3.Show()
            End If
        ElseIf ddlEscalateTo.Text = "RCT" Then
            If ddlEscalateTo.Text = "0" Then
                Exit Sub
            Else
                MPEDesignation4.Show()
            End If
        End If
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            CheckLogin()
        End If
    End Sub


    Protected Sub btnCloseAst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseAst.Click
        WrkNo = txtWorkOrderNo.Text
        AuthorName = Me.Session("fname")
        EscalName = lblWOSelectEscal.Text.Replace("&#209;", "Ñ")
        SysCreatorID = Me.Session("res_id")
        SysModifierID = Me.Session("res_id")

        If ddlEscalateTo.Text = "0" Then
            EscalName = txtAuthorName.Text
        Else
            lblWOError.Text = "To resolve work order you must clear the escalation!"
            Exit Sub
        End If

        If txtAssessment.Text = "" Then
            lblWOError.Text = "Please fill in Corrective Action!"
            Exit Sub
        End If

        If DataCloseWorkOrder() = False Then
            lblWOError.Text = Me.Session("InsertErrorRec")
        Else
            lblWOError.Text = "Work order is successfully resolved!"
            btnSaveAst.Enabled = False
            btnCloseAst.Enabled = False
            txtAssessment.Enabled = False
            PanelWorkOrderInfo.Enabled = False
            btnWOOk.Visible = True
        End If
    End Sub
   
    Protected Sub btnSaveAst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAst.Click
        WrkNo = txtWorkOrderNo.Text
        AuthorName = Me.Session("fname")
        EscalName = lblWOSelectEscal.Text.Replace("&#209;", "Ñ")
        SysCreatorID = Me.Session("res_id")
        SysModifierID = Me.Session("res_id")
        btnCloseAst.Enabled = False
        txtAssessment.Enabled = False

        If ddlEscalateTo.Text = "0" Then
            lblWOError.Text = "Please specify your escalation!"
            ddlEscalateTo.Enabled = True
            Exit Sub
        End If

        If lblWOSelectEscal.Text = "" Then
            lblWOError.Text = "Please specify your escalation!"
            Exit Sub
        End If

        If txtAssessmentHist.Text = "" Then
            lblWOError.Text = "Please specify your work management"
            Exit Sub
        End If

        If DataSaveWorkOrder() = False Then
            lblWOError.Text = Me.Session("InsertErrorRec")
        Else
            lblWOError.Text = "Work order is successfully escalated!"
            btnSaveAst.Enabled = False
            btnCloseAst.Enabled = False
            txtAssessment.Enabled = False
            txtAssessmentHist.Enabled = False
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
        mySqlInsertDetail = "update cmms_wo_detail set escalated_date = now(), escalated_by = '" & AuthorName.Trim & "', escalated_to = '" & EscalName.Trim & "', correctiveaction = '" & EscapeApostrophe(txtAssessment.Text.Trim) & "', sys_modified = now(), sys_modifier = '" & SysModifierID.Trim & "', " _
                          & "resolve = 'Y' " _
                          & "where wo_no = '" & WrkNo.Trim & "';" _
                          & "insert into cmms_wo_history(wo_no,escalated_date,escalated_by,escalated_to,resolve,correctiveaction,sys_created,sys_modified,sys_creator,sys_modifier)" _
                          & "values('" & WrkNo.Trim & "',now(),'" & AuthorName.Trim & "','" & EscalName.Trim & "','Y','" & EscapeApostrophe(txtAssessment.Text.Trim) & "',now(),now(),'" & SysCreatorID.Trim & "','" & SysModifierID.Trim & "');"

        '"update cmms_wo_history set escalated_to = '" & EscalName.Trim & "', resolve = 'Y', sys_created = now(), sys_modified = now(), sys_creator = '" & SysCreatorID.Trim & "', " _
        '                  & "sys_modifier = '" & SysModifierID.Trim & "' where wo_no = '" & WrkNo.Trim & "'; " _
        '                  &
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

    Private Function DataSaveWorkOrder() As Boolean
        Dim con As New MySqlConnection
        Dim com As New MySqlCommand
        Dim oTran As MySqlTransaction
        Dim mySqlInsertDetail As String
        'mySqlInsertDetail = "update cmms_wo_masterheader set escalated_name = '" + EscalName + "' where wo_no = '" + WrkNo + "';" _
        '                  & "insert into cmms_wo_detail(wo_no,escalated_date,escalated_by,escalated_to,correctiveaction,sys_created,sys_modified,sys_creator,sys_modifier)" _
        '                  & "values('" & WrkNo & "',now(),'" & AuthorName & "','" & EscalName & "','" + txtAssessment.Text + "',now(),now(),'" & SysCreatorID & "','" & SysModifierID & "');" _
        '                  & "insert into cmms_wo_history(wo_no,escalated_date,escalated_by,escalated_to,resolve,correctiveaction,sys_created,sys_modified,sys_creator,sys_modifier)" _
        '                  & "values('" & WrkNo & "',now(),'" & AuthorName & "','" & EscalName & "','N','" + txtAssessment.Text + "',now(),now(),'" & SysCreatorID & "','" & SysModifierID & "');"
        mySqlInsertDetail = "update cmms_wo_detail set escalated_date = now(), escalated_by = '" & AuthorName.Trim & "', escalated_to = '" & EscalName.Trim & "', correctiveaction = '" & txtAssessment.Text.Trim & "', sys_modified = now(), sys_modifier = '" & SysModifierID.Trim & "', " _
                          & "resolve = 'N' " _
                          & "where wo_no = '" & WrkNo.Trim & "';" _
                          & "insert into cmms_wo_history(wo_no,escalated_date,escalated_by,escalated_to,resolve,correctiveaction,sys_created,sys_modified,sys_creator,sys_modifier)" _
                          & "values('" & WrkNo.Trim & "',now(),'" & AuthorName.Trim & "','" & EscalName.Trim & "','N','" & EscapeApostrophe(txtAssessmentHist.Text.Trim) & "',now(),now(),'" & SysCreatorID.Trim & "','" & SysModifierID.Trim & "');"
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

    Protected Sub ddlEscalateTo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlEscalateTo.SelectedIndexChanged
        If ddlEscalateTo.Text = "0" Then
            lblWOSelectEscal.Visible = False
            lblWOSelectEscal.Text = ""
            ddlEscalateTo.Enabled = True
            ddlDesignationTo.Visible = False
            lbAssignToSrch.Visible = False
            ddlDesignationTo.ClearSelection()
        Else
            If ddlEscalateTo.Text = "DIVISION" Then
                lblWOSelectEscal.Visible = True
                lblWOSelectEscal.Text = ""
                ddlEscalateTo.Enabled = True
                ddlDesignationTo.Visible = True
                lbAssignToSrch.Visible = False
                ddlDesignationTo.ClearSelection()
                findDivision()
            ElseIf ddlEscalateTo.Text = "REGION" Then
                lblWOSelectEscal.Visible = True
                lblWOSelectEscal.Text = ""
                ddlEscalateTo.Enabled = True
                ddlDesignationTo.Visible = True
                ddlDesignationTo.ClearSelection()
                findRegion()
            ElseIf ddlEscalateTo.Text = "LPT" Then
                lblWOSelectEscal.Visible = True
                lblWOSelectEscal.Text = ""
                ddlEscalateTo.Enabled = True
                ddlDesignationTo.Visible = False
                lbAssignToSrch.Visible = True
                ddlDesignationTo.ClearSelection()
            ElseIf ddlEscalateTo.Text = "RCT" Then
                lblWOSelectEscal.Visible = True
                lblWOSelectEscal.Text = ""
                ddlEscalateTo.Enabled = True
                ddlDesignationTo.Visible = False
                lbAssignToSrch.Visible = True
                ddlDesignationTo.ClearSelection()
            End If
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

    Private Function LLRUsers(ByVal jobDesc As String) As Boolean
        Dim mySqlDes As String
        Dim ds As DataSet
        mySqlDes = "select distinct task from webaccounts where task like '%/BM/%' or task like '%Regional%' or task like '%Area%' or task like '%LPT%' or task like '%RCT-A%' or task like '%BM/BOSMAN%';"
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

        mySQLconcat = "SELECT escalated_by, correctiveAction, date_format(Sys_modified, '%Y/%m/%d') as Sys_modified FROM cmms_wo_history where wo_no = '" & RecWorkOrderNumber.Trim & "';"

        ds = Execute_DataSetCMMS(mySQLconcat)

        If Not ds Is Nothing Then
            For x = 0 To ds.Tables(0).Rows.Count - 1
                If ds.Tables(0).Rows(x)(1).ToString.Trim <> "" Then
                    txtWorkManagementHist.Text += ds.Tables(0).Rows(x)(0) & ": " & vbNewLine & ds.Tables(0).Rows(x)(1) & " " & vbNewLine & ds.Tables(0).Rows(x)(2) & vbNewLine & bar & vbNewLine
                End If
            Next
        End If
    End Sub

    Private Sub disRecWO()
        Dim connString As String = Me.Session("strCon")
        Dim cn As MySqlConnection = New MySqlConnection(connString)
        Dim reader As MySqlDataReader = Nothing
        Dim mySqlRecWONum As String

        mySqlRecWONum = "select t1.bc_code_author, " _
                      & "t1.bc_name_author, " _
                      & "t2.sys_creator, " _
                      & "t1.author_name, " _
                      & "t1.wo_no, " _
                      & "t3.wo_type_desc, " _
                      & "t1.asset_inv_no, " _
                      & "date_format(t2.escalated_date, '%Y-%m-%d')as escalated_date, " _
                      & "t1.ir_no, " _
                      & "t1.wo_desc " _
                      & "from cmms_wo_masterheader as t1 " _
                      & "inner join cmms_wo_detail as t2 " _
                      & "on t1.wo_no = t2.wo_no " _
                      & "inner join cmms_wo_type as t3 " _
                      & "on t1.wo_type_code = t3.wo_type_code " _
                      & "where t1.wo_no = '" & RecWorkOrderNumber.Trim & "';"
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
                    txtAuthorName.Text = reader.Item("author_name")
                    txtWorkOrderNo.Text = reader.Item("wo_no")
                    txtWorkOrderType.Text = reader.Item("wo_type_desc")
                    txtAssetInventory.Text = reader.Item("asset_inv_no")
                    txtDate.Text = reader.Item("escalated_date")
                    txtIRNumber.Text = reader.Item("ir_no")
                    txtWorkOrderDesc.Text = reader.Item("wo_desc")
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

    Private Sub SelectEscalDiv()
        Dim ds As New DataSet

        Try
            Dim mySql As String
            mySql = "select Escalate_Desc from cmms_wo_escalation limit 1;"
            ds = Execute_DataSetCMMS(mySql)

            Dim RText As String = ""
            Dim i As Integer
            ddlEscalateTo.Items.Clear()
            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    ddlEscalateTo.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString))
                Next
            Else
                RText = "Nothing Available"
            End If
            ddlEscalateTo.Items.Insert(0, New ListItem(RText, "0"))

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub SelectEscalRegDiv()
        Dim ds As New DataSet

        Try
            Dim mySql As String
            mySql = "select Escalate_Desc from cmms_wo_escalation where Escalate_desc <> 'REGION' and Escalate_desc <> 'LPT' and Escalate_desc <> 'RCT';"
            ds = Execute_DataSetCMMS(mySql)

            Dim RText As String = ""
            Dim i As Integer
            ddlEscalateTo.Items.Clear()
            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    ddlEscalateTo.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString))
                Next
            Else
                RText = "Nothing Available"
            End If
            ddlEscalateTo.Items.Insert(0, New ListItem(RText, "0"))

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub SelectEscalLPTRCTDiv()
        Dim ds As New DataSet

        Try
            Dim mySql As String
            mySql = "select Escalate_Desc from cmms_wo_escalation where escalate_desc like '%LPT%' or escalate_desc like '%RCT%' or escalate_desc like '%DIVISION%' order by Escalate_Desc;"
            ds = Execute_DataSetCMMS(mySql)

            Dim RText As String = ""
            Dim i As Integer
            ddlEscalateTo.Items.Clear()
            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    ddlEscalateTo.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString))
                Next
            Else
                RText = "Nothing Available"
            End If
            ddlEscalateTo.Items.Insert(0, New ListItem(RText, "0"))

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub findDivision()
        Dim ds As New DataSet

        Try
            Dim mySql As String
            If Me.Session("ZCode") = "VISMIN" Then
                'mySql = "select UPPER(Division) AS Division from irdivision where zonecode = 'vismin' and division <> '" & RecException.Trim & "' order by division desc"
                mySql = "select UPPER(Division) AS Division from irdivision where zonecode = 'vismin' order by division desc"
            Else
                'mySql = "select UPPER(Division) AS Division from irdivision where zonecode = 'luzon' and division <> '" & RecException.Trim & "' order by division desc"
                mySql = "select UPPER(Division) AS Division from irdivision where zonecode = 'luzon' order by division desc"
            End If
            ds = Execute_DataSet(mySql)

            Dim RText As String = ""
            Dim i As Integer
            ddlDesignationTo.Items.Clear()
            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    ddlDesignationTo.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString))
                Next
            Else
                RText = "Nothing Available"
            End If
            ddlDesignationTo.Items.Insert(0, New ListItem(RText, "0"))

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub findRegion()
        Dim ds As New DataSet

        Try
            Dim mySql As String
            mySql = "select distinct class_03 from webbranches where zonecode = 'vismin' and class_03 <> 'HO' order by class_03 desc;"
            ds = Execute_DataSet(mySql)

            Dim RText As String = ""
            Dim i As Integer
            ddlDesignationTo.Items.Clear()
            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    ddlDesignationTo.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString))
                Next
            Else
                RText = "Nothing Available"
            End If
            ddlDesignationTo.Items.Insert(0, New ListItem(RText, "0"))

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub findLPT()
        Dim ds As New DataSet
        Dim URegion As String = Me.Session("JRegion")

        Try
            Dim mySql As String
            mySql = "select distinct fullname from cmms_users where task like '%LPT%' and region = '" & URegion.Trim & "';"
            ds = Execute_DataSet(mySql)

            Dim RText As String = ""
            Dim i As Integer
            ddlDesignationTo.Items.Clear()
            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    ddlDesignationTo.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString))
                Next
            Else
                RText = "Nothing Available"
            End If
            ddlDesignationTo.Items.Insert(0, New ListItem(RText, "0"))

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub findRCT()
        Dim ds As New DataSet
        Dim URegion As String = Me.Session("JRegion")

        Try
            Dim mySql As String
            mySql = "select distinct fullname from cmms_users where task like '%RCT-A%' and region = '" & URegion.Trim & "';"
            ds = Execute_DataSet(mySql)

            Dim RText As String = ""
            Dim i As Integer
            ddlDesignationTo.Items.Clear()
            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    ddlDesignationTo.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString))
                Next
            Else
                RText = "Nothing Available"
            End If
            ddlDesignationTo.Items.Insert(0, New ListItem(RText, "0"))

        Catch ex As Exception
            MsgBox(ex.Message)
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

    Protected Sub btnWOOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWOOk.Click
        Response.Redirect("ReceivedWO.aspx")
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
