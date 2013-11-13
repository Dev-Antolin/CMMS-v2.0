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
Imports System.Configuration
Imports System.Collections
Imports System.Web.Security
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Text.RegularExpressions

Partial Class WorkOrder_CreateWO
    Inherits System.Web.UI.Page
    Dim CodeNo As String
    Dim BCCenter As String
    Dim RecWorkOrderAuthor As String = ""
    Dim _Increment As String
    Dim Year As String = Format(Date.Now, "yyyy")
    'Work Order Entry
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
    Dim SysCreator As String
    Dim SysModifier As String

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
        lblWOSelectEscal.Text = Name.Replace("&#241;", "ñ")
    End Sub

    Public Sub DIG2_Selected(ByVal Name As String)
        lblWOSelectEscal.Text = Name.Replace("&#241;", "ñ")
    End Sub

    Public Sub DIG3_Selected(ByVal Name As String)
        lblWOSelectEscal.Text = Name.Replace("&#241;", "ñ")
    End Sub

    Public Sub DIG4_Selected(ByVal Name As String)
        lblWOSelectEscal.Text = Name.Replace("&#241;", "ñ")
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

    Protected Sub lbWOSrchEscal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbWOSrchEscal.Click
        If ddlWOEscal.Text = "0" Then
            Exit Sub
        ElseIf ddlWOEscal.Text = "DIVISION" Then
            If ddlDesignation.Text = "0" Then
                Exit Sub
            Else
                MPEDesignation.Show()
            End If
        ElseIf ddlWOEscal.Text = "REGION" Then
            If ddlDesignation.Text = "0" Then
                Exit Sub
            Else
                MPEDesignation2.Show()
            End If
        ElseIf ddlWOEscal.Text = "LPT" Then
            If ddlWOEscal.Text = "0" Then
                Exit Sub
            Else
                MPEDesignation3.Show()
            End If
        ElseIf ddlWOEscal.Text = "RCT" Then
            If ddlWOEscal.Text = "0" Then
                Exit Sub
            Else
                MPEDesignation4.Show()
            End If
        End If
    End Sub

    Protected Sub ddlDesignation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDesignation.SelectedIndexChanged
        lblWOSelectEscal.Text = ""
        If ddlDesignation.Text = "0" Then
            lbWOSrchEscal.Visible = False
        Else
            lbWOSrchEscal.Visible = True
        End If
    End Sub

    Protected Sub ddlWOEscal_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWOEscal.SelectedIndexChanged
        If ddlWOEscal.Text = "0" Then
            lblWOSelectEscal.Visible = False
            lblWOSelectEscal.Text = ""
            ddlDesignation.Visible = False
            lbWOSrchEscal.Visible = False
            ddlDesignation.ClearSelection()
        Else
            If ddlWOEscal.Text = "DIVISION" Then
                lblWOSelectEscal.Visible = True
                lblWOSelectEscal.Text = ""
                ddlDesignation.Visible = True
                ddlDesignation.ClearSelection()
                findDivision()
            ElseIf ddlWOEscal.Text = "REGION" Then
                lblWOSelectEscal.Visible = True
                lblWOSelectEscal.Text = ""
                ddlDesignation.Visible = True
                ddlDesignation.ClearSelection()
                findRegion()
            ElseIf ddlWOEscal.Text = "LPT" Then
                lblWOSelectEscal.Visible = True
                lblWOSelectEscal.Text = ""
                ddlDesignation.Visible = False
                lbWOSrchEscal.Visible = True
                ddlDesignation.ClearSelection()
            ElseIf ddlWOEscal.Text = "RCT" Then
                lblWOSelectEscal.Visible = True
                lblWOSelectEscal.Text = ""
                ddlDesignation.Visible = False
                lbWOSrchEscal.Visible = True
                ddlDesignation.ClearSelection()
            End If
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Disable Cache
        Response.Buffer = True
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
        Response.Expires = -1500
        Response.CacheControl = "no-cache"
        CheckLogin()
        If CheckAttachA() = True Then
            Dim AN As String = Me.Session("txtAAsstInvNo")
            If AN = Me.Session("NewAddAssetNo") Then
                Dim DelSql As String
                DelSql = "delete from cmms_entry_attachfiles where Asset_Inv_No ='" & AN & "';"
                Execute_Delete(DelSql)
            End If
        End If
        If CheckAttachB() = True Then
            Dim AN As String = Me.Session("txtAsstInvNo")
            If AN = Me.Session("NewBasicAssetNo") Then
                Dim DelSql As String
                DelSql = "delete from cmms_entry_attachfiles where Asset_Inv_No ='" & AN & "';"
                Execute_Delete(DelSql)
            End If
        End If
        ReadOnlyFieldsCategory()
        If Not Page.IsPostBack Then
            Me.Session("Click") = "CWO"
            CodeNo = Me.Session("JCodeNo")
            txtWOCode.Text = Me.Session("JCode")
            txtWOName.Text = Me.Session("JCodeName")
            txtWOIDNo.Text = Me.Session("res_id")
            txtWOEmpName.Text = Me.Session("fName").Replace("&#209;", "Ñ")
            ddlDesignation.Visible = False
            lblWOSelectEscal.Visible = False
            lbWOSrchEscal.Visible = False
            'SelectCodeNo()
            If MainUsers(Me.Session("JDesc")) = True Then
                SelectBCCenter()
            Else
                BCCenter = txtWOCode.Text
            End If
            Create_trxNo()
            txtWONo.Text = Year + "-" + CodeNo + "-" + "0" + BCCenter + "-" + _Increment
            txtWOStartDate.Text = Format(Date.Now, "yyyy-MM-dd")
            If Me.Session("AttachWOTrue") = True Then
                WOTemporaryRevInfo()
                BindAttachment(Me.Session("WONo"))
                If attList.Items.Count <> 0 Then
                    rbAttachFile.Checked = True
                Else
                    rbAttachFile.Checked = False
                End If
            Else
                If CheckAttachWO() = True Then
                    Dim WO As String = Me.Session("WONo")
                    If WO = txtWONo.Text Then
                        Dim DelSql As String
                        DelSql = "delete from cmms_wo_attachfiles where WO_No ='" & WO & "';"
                        Execute_Delete(DelSql)
                    End If
                End If
            End If
        End If
        Try
            If divisionUsers(Me.Session("JDesc")) = True Then
                PanelAuthorInfo.GroupingText = "Division Information"
                lblBCodeAuthor.Text = "Division Code"
                lblBNameAuthor.Text = "Division Name"
            ElseIf divUsers(Me.Session("JDesc")) = True Then
                PanelAuthorInfo.GroupingText = "Division Information"
                lblBCodeAuthor.Text = "Division Code"
                lblBNameAuthor.Text = "Division Name"
            ElseIf SubDivUsers(Me.Session("JDesc")) = True Then
                PanelAuthorInfo.GroupingText = "Division Information"
                lblBCodeAuthor.Text = "Division Code"
                lblBNameAuthor.Text = "Division Name"
            ElseIf LLRUsers(Me.Session("JDesc")) = True Then
                PanelAuthorInfo.GroupingText = "Branch Information"
                lblBCodeAuthor.Text = "BC Code"
                lblBNameAuthor.Text = "BC Name"
            End If
        Catch ex As Exception
            Me.lblPromtError.Text = ex.Message
        End Try

        btnWOOk.Visible = False
        lblCountWord.Text = (500 - txtWODesc.Text.Length) & "/500"
        DisableTextboxCtrl()
        EnableTextboxCtrl()
        Me.Session.Add("Designation", ddlDesignation.Text)
        Me.Session.Add("RecAuthor", RecWorkOrderAuthor)
        Me.Session.Add("Desig", ddlWOEscal.Text)
    End Sub

    Private Sub CheckLogin()
        If Me.Session("fName") = "" Then
            Response.Redirect("~/login.aspx")
        End If
    End Sub

    Protected Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        If Not Page.IsPostBack Then
            CheckLogin()
            SelectWOType()
            Try
                If divisionUsers(Me.Session("JDesc")) = True Then
                    SelectEscalRegDiv()
                ElseIf divUsers(Me.Session("JDesc")) = True Then
                    SelectEscalRegDiv()
                ElseIf SubDivUsers(Me.Session("JDesc")) = True Then
                    SelectEscalRegDiv()
                ElseIf LLRUsers(Me.Session("JDesc")) = True Then
                    SelectEscalLPTRCTDiv()
                End If
                'SelectWOEscalation()
            Catch ex As Exception
                Me.lblPromtError.Text = ex.Message
            End Try            
        End If
    End Sub

    Public Sub RemoveAtt(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim WoNo As String = Me.Session("WONo")
        Dim sqlDelPic As String
        Dim cmdArg As String = e.CommandArgument.ToString
        Dim str As String = cmdArg.ToString.Substring(cmdArg.IndexOf("/") + 1, cmdArg.Length - (cmdArg.IndexOf("/")) - 1)
        Dim idx As Integer = cmdArg.ToString.Substring(0, cmdArg.IndexOf("/")) - 1
        attList.Items(idx).Visible = False
        sqlDelPic = "delete from cmms_wo_attachfiles where WO_No = '" & WoNo & "' and File_Name ='" & str & "';"
        Execute_Delete(sqlDelPic)
        If RemoveAttList(WoNo) = True Then
            rbAttachFile.Checked = True
        Else
            rbAttachFile.Checked = False
        End If
    End Sub

    Private Function RemoveAttList(ByVal WoNo As String) As Boolean
        Dim mySqlDes As String
        Dim ds As DataSet
        mySqlDes = "select File_Name,File_Pic from cmms_wo_attachfiles, (SELECT @rownum:=0) r  where WO_No = '" & WoNo & "'"
        ds = Execute_DataSetMySgl(mySqlDes)
        If Not ds Is Nothing Then
            Return True
        End If
        Return False
    End Function

    Protected Sub ReadOnlyFieldsCategory()
        For Each ctrl As Control In PanelAuthorInfo.Controls
            If TypeOf ctrl Is TextBox Then
                CType(ctrl, TextBox).ReadOnly = True
            End If
        Next
    End Sub
    Protected Sub disableFieldsAuthor()
        For Each c As Control In PanelAuthorInfo.Controls
            If TypeOf c Is TextBox Then
                CType(c, TextBox).Enabled = False
            End If
        Next
    End Sub

    Protected Sub disableFieldsWorkOrder()
        For Each c As Control In PanelWorkOrderInfo.Controls
            If TypeOf c Is TextBox Then
                CType(c, TextBox).Enabled = False
            End If
            If TypeOf c Is DropDownList Then
                CType(c, DropDownList).Enabled = False
            End If
            If TypeOf c Is ImageButton Then
                CType(c, ImageButton).Enabled = False
            End If
            If TypeOf c Is RadioButton Then
                CType(c, RadioButton).Enabled = False
            End If
        Next
        For Each rptItem As RepeaterItem In attList.Items
            If rptItem.FindControl("lnkRemove") IsNot Nothing Then
                DirectCast(rptItem.FindControl("lnkRemove"), LinkButton).Enabled = False
                DirectCast(rptItem.FindControl("attchmnt"), Label).Enabled = False
            End If
        Next
    End Sub

    Protected Sub DisableTextboxCtrl()
        txtWOTypeOthers.Visible = False
    End Sub
    Protected Sub EnableTextboxCtrl()
        If ddlWOType.Text = Me.Session("WOTypeOthers") Then
            txtWOTypeOthers.Visible = True
        Else
            txtWOTypeOthers.Visible = False
            txtWOTypeOthers.Text = ""
        End If
    End Sub

    Private Function MainUsers(ByVal jobDesc As String) As Boolean
        Dim mySqlDes As String
        Dim ds As DataSet
        mySqlDes = "select distinct task from webaccounts where task like '%BOS-CONT%' or task like '%MMD-STAFF%' or task like '%DIVMAN%' or task like '%DEPTMAN%';"
        ds = Execute_DataSet(mySqlDes)
        If Not ds Is Nothing Then
            For x = 0 To ds.Tables(0).Rows.Count - 1
                If jobDesc = ds.Tables(0).Rows(x)(0) Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

    Protected Sub SelectCodeNo()
        Dim ds As DataSet
        Dim mySqlCodeNO As String

        Try
            mySqlCodeNO = "select codeno from staff where res_id = '" & txtWOIDNo.Text & "';"
            ds = Execute_DataSet(mySqlCodeNO)

            CodeNo = ds.Tables(0).Rows(0).Item(0).ToString()
        Catch ex As Exception
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
            'MsgBox(ex.Message)
        End Try
    End Sub

    Protected Sub SelectBCCenter()
        Dim ds As DataSet
        Dim mySqlBCCenter As String

        Try
            mySqlBCCenter = "select distinct t1.divisionacro from division as t1 " _
                          & "inner join cmms_users as t2 on t1.divisioncode = t2.codeno " _
                          & "where t2.codeno = '" & CodeNo & "';"
            ds = Execute_DataSet(mySqlBCCenter)

            BCCenter = ds.Tables(0).Rows(0).Item(0).ToString()

        Catch ex As Exception
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
            'MsgBox(ex.Message)
        End Try
    End Sub

    Private Function CheckAttachA() As Boolean
        Dim AssetNo As String = Me.Session("txtAAsstInvNo")
        Dim CheckSql As String
        Dim ds As DataSet

        CheckSql = "select Asset_Inv_No from cmms_entry_attachfiles where Asset_Inv_No = '" & AssetNo & "';"

        ds = Execute_DataSetAttach(CheckSql)
        If Not ds Is Nothing AndAlso ds.Tables(0).Rows(0).Item(0).ToString <> "" Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function CheckAttachB() As Boolean
        Dim AssetNo As String = Me.Session("txtAsstInvNo")
        Dim CheckSql As String
        Dim ds As DataSet

        CheckSql = "select Asset_Inv_No from cmms_entry_attachfiles where Asset_Inv_No = '" & AssetNo & "';"

        ds = Execute_DataSetAttach(CheckSql)
        If Not ds Is Nothing AndAlso ds.Tables(0).Rows(0).Item(0).ToString <> "" Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function CheckAttachWO() As Boolean
        Dim WONo As String = Me.Session("WONo")
        Dim CheckSql As String
        Dim ds As DataSet

        CheckSql = "select WO_No from cmms_wo_attachfiles where WO_No = '" & WONo & "';"

        ds = Execute_DataSetAttach(CheckSql)
        If Not ds Is Nothing AndAlso ds.Tables(0).Rows(0).Item(0).ToString <> "" Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function Execute_Delete(ByVal mySql As String) As Boolean
        Dim con As New MySqlConnection
        Dim com As New MySqlCommand
        Dim oTran As MySqlTransaction

        con.ConnectionString = Me.Session("strCon")
        If con.State = ConnectionState.Closed Then
            con.Open()
            oTran = con.BeginTransaction()
            com = con.CreateCommand
            com.CommandTimeout = 0
            com.Transaction = oTran
            Try
                com.CommandText = mySql
                com.ExecuteNonQuery()
                oTran.Commit()
                com.Dispose()
                Return True
            Catch ex As Exception
                Me.Session.Add("InsertError", ex.Message)
                oTran.Rollback()
                con.Close()
                com.Dispose()
                Return False
            End Try
        End If
    End Function

    Private Sub SelectWOType()
        Dim ds As New DataSet

        Try
            Dim mySql As String
            mySql = "select wo_type_desc from cmms_wo_type order by wo_type_desc desc;"
            ds = Execute_DataSetCMMS(mySql)

            Dim RText As String = ""
            'Dim Others As String = "OTHERS"
            Dim i As Integer
            Dim r As String
            ddlWOType.Items.Clear()
            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    ddlWOType.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString))
                Next
                r = i + 1
                'ddlWOType.Items.Insert(i, New ListItem(Others, r))
                'Me.Session.Add("WOTypeOthers", r)
                'Me.Session.Add("WOOthers", Others)
            Else
                RText = "Nothing Available"
            End If
            ddlWOType.Items.Insert(0, New ListItem(RText, "0"))

        Catch ex As Exception
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
            'MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub SelectWOEscalation()
        Dim ds As New DataSet

        Try
            Dim mySql As String
            mySql = "select Escalate_Desc from cmms_wo_escalation limit 2;"
            ds = Execute_DataSetCMMS(mySql)

            Dim RText As String = ""
            Dim i As Integer
            ddlWOEscal.Items.Clear()
            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    ddlWOEscal.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString))
                Next
            Else
                RText = "Nothing Available"
            End If
            ddlWOEscal.Items.Insert(0, New ListItem(RText, "0"))

        Catch ex As Exception
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
            'MsgBox(ex.Message)
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
            ddlWOEscal.Items.Clear()
            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    ddlWOEscal.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString))
                Next
            Else
                RText = "Nothing Available"
            End If
            ddlWOEscal.Items.Insert(0, New ListItem(RText, "0"))

        Catch ex As Exception
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
            'MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub SelectEscalRegDiv()
        Dim ds As New DataSet

        Try
            Dim mySql As String
            mySql = "select Escalate_Desc from cmms_wo_escalation limit 2;"
            ds = Execute_DataSetCMMS(mySql)

            Dim RText As String = ""
            Dim i As Integer
            ddlWOEscal.Items.Clear()
            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    ddlWOEscal.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString))
                Next
            Else
                RText = "Nothing Available"
            End If
            ddlWOEscal.Items.Insert(0, New ListItem(RText, "0"))

        Catch ex As Exception
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
            'MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub SelectEscalLPTRCTDiv()
        Dim ds As New DataSet

        Try
            Dim mySql As String
            mySql = "select Escalate_Desc from cmms_wo_escalation order by escalate_desc limit 3;"
            ds = Execute_DataSetCMMS(mySql)

            Dim RText As String = ""
            Dim i As Integer
            ddlWOEscal.Items.Clear()
            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    ddlWOEscal.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString))
                Next
            Else
                RText = "Nothing Available"
            End If
            ddlWOEscal.Items.Insert(0, New ListItem(RText, "0"))

        Catch ex As Exception
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
            'MsgBox(ex.Message)
        End Try
    End Sub

    Private Function divisionUsers(ByVal jobDesc As String) As Boolean
        Dim mySqlDes As String
        Dim ds As DataSet
        mySqlDes = "select distinct task from webaccounts where task like '%DIVMAN%' or task like '%DEPTMAN%';"
        ds = Execute_DataSet(mySqlDes)
        If Not ds Is Nothing Then
            For x = 0 To ds.Tables(0).Rows.Count - 1
                If jobDesc.ToUpper = ds.Tables(0).Rows(x)(0).ToString.Trim.ToUpper Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

    Private Function divUsers(ByVal jobDesc As String) As Boolean
        Dim mySqlDes As String
        Dim ds As DataSet
        mySqlDes = "select distinct task from webaccounts where task like '%BOS-CONT%' or task like '%MMD-STAFF%';"
        ds = Execute_DataSet(mySqlDes)
        If Not ds Is Nothing Then
            For x = 0 To ds.Tables(0).Rows.Count - 1
                If jobDesc.ToUpper = ds.Tables(0).Rows(x)(0).ToString.Trim.ToUpper Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

    Private Function SubDivUsers(ByVal jobDesc As String) As Boolean
        Dim mySqlDes As String
        Dim ds As DataSet
        mySqlDes = "select distinct task from webaccounts where comp = '001' and task <> 'BOS-CONT' order by task;"
        ds = Execute_DataSet(mySqlDes)
        If Not ds Is Nothing Then
            For x = 0 To ds.Tables(0).Rows.Count - 1
                If jobDesc.ToUpper = ds.Tables(0).Rows(x)(0).ToString.Trim.ToUpper Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

    Private Function LLRUsers(ByVal jobDesc As String) As Boolean
        Dim mySqlDes As String
        Dim ds As DataSet
        mySqlDes = "select distinct task from webaccounts where task like '%/BM/%' or task like '%LPT%' or task like '%RCT-A%';"
        ds = Execute_DataSet(mySqlDes)
        If Not ds Is Nothing Then
            For x = 0 To ds.Tables(0).Rows.Count - 1
                If jobDesc.ToUpper = ds.Tables(0).Rows(x)(0).ToString.Trim.ToUpper Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

    Private Function LPTLToBOS(ByVal jobDesc As String) As Boolean
        Dim mySqlDes As String
        Dim ds As DataSet
        mySqlDes = "select distinct task from irlptl;"
        ds = Execute_DataSet(mySqlDes)
        If Not ds Is Nothing Then
            For x = 0 To ds.Tables(0).Rows.Count - 1
                If jobDesc.ToUpper = ds.Tables(0).Rows(x)(0).ToString.Trim.ToUpper Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

    Private Sub SelectNewTypeCode()
        Dim NwCodeType As String
        Dim OldCode As String
        Dim _NwCode As Integer
        Dim myNewCode As String = ""
        Dim ds As New DataSet

        If ddlWOType.Text = Me.Session("WOTypeOthers") Then
            myNewCode = "select comp_cpu_class_id from cmms_cpu_class where comp_cpu_class_id = '" & ddlWOType.Text & "';"
            ds = Execute_DataSet(myNewCode)
        End If

        If Not ds Is Nothing AndAlso ds.Tables(0).Rows(0).Item(0).ToString <> "" Then
            OldCode = ds.Tables(0).Rows(0).Item(0).ToString
            Me.Session.Add("ExistCode", OldCode)
        Else
            NwCodeType = NewTypeOthers(_NwCode)
            Me.Session.Add("NewCodeType", NwCodeType)
        End If
    End Sub

    Private Function NewTypeOthers(ByVal _NwCode As String) As String
        Dim ds As New DataSet
        Dim myCode As String = ""

        Try
            If ddlWOType.Text = Me.Session("WOTypeOthers") Then
                myCode = "select max(wo_type_code) + 1 as autnum from cmms_wo_type;"
                ds = Execute_DataSet(myCode)
            End If

            If Not ds Is Nothing AndAlso ds.Tables(0).Rows(0).Item(0).ToString <> "" Then
                Return ds.Tables(0).Rows(0).Item(0).ToString
            Else
                Return "error"
            End If
        Catch ex As Exception
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
            'MsgBox(ex.Message)
            Return "error"
        End Try

    End Function

    Private Sub Create_trxNo()
        Dim numRegion As Integer

        numRegion = 1

        _Increment = Auto_trxNo(numRegion)
        If _Increment = "error" Then
            Exit Sub
        End If
    End Sub

    Private Function Auto_trxNo(ByVal numRegion As Integer) As String
        Dim ds As New DataSet
        Dim intNumber As Integer
        Dim strAssetInvNo As String
        Dim strNewAssetInvNo As String
        Dim strNewestAssetInvNo As String

        Try
            Dim mySql As String = ""
            If numRegion = 1 Then
                mySql = "select WO_No from cmms_wo_masterheader where WO_No like '%" & Year & "%' " _
                      & "and bc_code_author = '" & txtWOCode.Text & "' order by sys_created desc limit 1;"
            ElseIf numRegion = 2 Then
                mySql = "select WO_No_No from cmms_wo_masterheader where WO_No like '%" & Year & "%' " _
                      & "and bc_code_author = '" & txtWOCode.Text & "' order by sys_created desc limit 1;"
            End If
            If mySql <> "" Then
                ds = Execute_DataSetCMMS(mySql)
                If Not ds Is Nothing AndAlso ds.Tables(0).Rows(0).Item(0).ToString.Trim <> "" Then
                    ds.Tables(0).Rows(0).Item(0).ToString()
                    strAssetInvNo = ds.Tables(0).Rows(0).Item(0).ToString.Substring(13, 5)
                    intNumber = Convert.ToInt32(strAssetInvNo)
                    intNumber += 1
                    strNewAssetInvNo = intNumber.ToString()
                    strNewAssetInvNo = strNewAssetInvNo.PadLeft(5, "0")
                    strNewestAssetInvNo = strNewAssetInvNo
                    Return strNewestAssetInvNo.ToString
                Else
                    Return "00001"
                End If
            Else
                Return "error"
            End If
        Catch ex As Exception
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
            'MsgBox(ex.Message)
            Return "error"
        End Try

    End Function

    Private Sub findDivision()
        Dim ds As New DataSet

        'Using con As New SqlConnection(Me.Session("strConf"))
        '    Dim sql As New StringBuilder()
        '    sql.Append("SELECT")
        '    sql.Append("    UPPER(Division) ")
        '    sql.Append("FROM")
        '    sql.Append("    irdivision ")
        '    sql.Append("WHERE")
        '    sql.Append("    ZoneCode = @ Zone ")
        '    sql.Append("AND division = ")
        '    sql.Append("ORDER BY")
        '    sql.Append("    Division ASC; ")
        '    sql.Append("")
        '    sql.Append("")
        '    Using cmd As New SqlCommand(sql.ToString, con)
        '        cmd.Parameters.Add("Zone", SqlDbType.VarChar, 10).Value = Me.Session("ZCode").ToString
        '        cmd.Parameters.Add("", SqlDbType.VarChar, 10).Value = ""
        '        Using DataRead As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
        '            If DataRead.HasRows Then
        '                Dim 
        '            Else

        '            End If
        '        End Using
        '    End Using
        'End Using
        Try
            Dim mySql As String
            If Me.Session("ZCode") = "VISMIN" Then
                'If LPTLToBOS(Me.Session("JDesc")) = True Then
                '    mySql = "select UPPER(Division) AS DIVISION from irdivision where zonecode = 'vismin' order by division desc;"
                'Else
                '    mySql = "select UPPER(Division) AS Division from irdivision where zonecode = 'vismin' and division <> '" & txtWOName.Text.Trim & "' order by division desc"
                'End If
                mySql = "select UPPER(Division) AS DIVISION from irdivision where zonecode = 'vismin' order by division desc;"
            Else
                'If LPTLToBOS(Me.Session("JDesc")) = True Then
                '    mySql = "select UPPER(Division) AS Division from irdivision where zonecode = 'luzon' order by division desc;"
                'Else
                '    mySql = "select UPPER(Division) AS Division from irdivision where zonecode = 'luzon' and division <> '" & txtWOName.Text.Trim & "' order by division desc"
                'End If
                mySql = "select UPPER(Division) AS Division from irdivision where zonecode = 'luzon' order by division desc;"
            End If
            ds = Execute_DataSet(mySql)

            Dim RText As String = ""
            Dim i As Integer
            ddlDesignation.Items.Clear()
            If Not ds Is Nothing Then
                If ds.Tables(0).Rows.Count > 0 Then
                    For i = 0 To ds.Tables(0).Rows.Count - 1
                        ddlDesignation.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString.Trim))
                    Next
                Else
                    RText = "Nothing Available"
                End If
                ddlDesignation.Items.Insert(0, New ListItem(RText, "0"))
            End If         
        Catch ex As Exception
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
            'MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub findRegion()
        Dim ds As New DataSet
        Try
            Dim mySql As String
            If Me.Session("ZCode") = "VISMIN" Then
                mySql = "select distinct UPPER(class_03) AS CLASS_03 from webbranches where zonecode = 'vismin' and class_03 <> 'HO' order by class_03 desc;"
            Else
                mySql = "select distinct UPPER(class_03) AS CLASS_03 from webbranches where zonecode = 'luzon' and class_03 <> 'HO' order by class_03 desc;"
            End If
            ds = Execute_DataSet(mySql)
            Dim RText As String = ""
            Dim i As Integer
            ddlDesignation.Items.Clear()
            If Not ds Is Nothing Then
                If ds.Tables(0).Rows.Count > 0 Then
                    For i = 0 To ds.Tables(0).Rows.Count - 1
                        ddlDesignation.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString.Trim))
                    Next
                Else
                    RText = "Nothing Available"
                End If
                ddlDesignation.Items.Insert(0, New ListItem(RText, "0"))
            End If            
        Catch ex As Exception
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
            'MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub findLPT()
        Dim ds As New DataSet
        Dim URegion As String = Me.Session("JRegion")

        Try
            Dim mySql As String
            mySql = "select distinct UPPER(fullname) AS FULLNAME from weblpts where class_03 = '" & URegion.Trim & "';"
            ds = Execute_DataSet(mySql)
            Dim RText As String = ""
            Dim i As Integer
            ddlDesignation.Items.Clear()
            If Not ds Is Nothing Then
                If ds.Tables(0).Rows.Count > 0 Then
                    For i = 0 To ds.Tables(0).Rows.Count - 1
                        ddlDesignation.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString.Trim))
                    Next
                Else
                    RText = "Nothing Available"
                End If
                ddlDesignation.Items.Insert(0, New ListItem(RText, "0").ToString.Trim)
            End If            
        Catch ex As Exception
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
            'MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub findRCT()
        Dim ds As New DataSet
        Dim URegion As String = Me.Session("JRegion")

        Try
            Dim mySql As String
            mySql = "select distinct UPPER(fullname) AS FULLNAME from irrcts where class_03 = '" & URegion.Trim & "';"
            ds = Execute_DataSet(mySql)

            Dim RText As String = ""
            Dim i As Integer
            If Not ds Is Nothing Then
                ddlDesignation.Items.Clear()
                If ds.Tables(0).Rows.Count > 0 Then
                    For i = 0 To ds.Tables(0).Rows.Count - 1
                        ddlDesignation.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString.Trim))
                    Next
                Else
                    RText = "Nothing Available"
                End If
                ddlDesignation.Items.Insert(0, New ListItem(RText, "0").ToString.Trim)
            End If            
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Function Execute_Insert(ByVal mySql As String) As Boolean
        Dim con As New MySqlConnection
        Dim com As New MySqlCommand
        Dim oTran As MySqlTransaction

        con.ConnectionString = Me.Session("strCon")
        If con.State = ConnectionState.Closed Then
            con.Open()
            oTran = con.BeginTransaction()
            com = con.CreateCommand
            com.CommandTimeout = 0
            com.Transaction = oTran
            Try
                com.CommandText = mySql
                com.ExecuteNonQuery()
                oTran.Commit()
                com.Dispose()
                Return True
            Catch ex As Exception
                Me.Session.Add("InsertError", ex.Message)
                oTran.Rollback()
                con.Close()
                com.Dispose()
                Return False
            End Try
        End If
    End Function

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

    Public Function Execute_DataSetAttach(ByVal as_mysql As String) As DataSet
        Dim Con As New MySqlConnection
        Dim Com As New MySqlCommand
        Dim sqlAdapter As MySqlDataAdapter
        Dim sqlDataset As New DataSet

        Execute_DataSetAttach = Nothing
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
                    Execute_DataSetAttach = sqlDataset
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

    Public Function Execute_DataSetMySgl(ByVal as_mysql As String) As DataSet
        Dim Con As New MySqlConnection
        Dim Com As New MySqlCommand
        Dim sqlAdapter As MySqlDataAdapter
        Dim sqlDataset As New DataSet

        Execute_DataSetMySgl = Nothing
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
                    Execute_DataSetMySgl = sqlDataset
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

    Protected Sub btnWOSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWOSave.Click
        If lblWOSelectEscal.Text = "" Then
            lblPromtError.Text = "Select user to escalate."
            Exit Sub
        End If
        If ddlWOType.Text = "0" Then
            lblPromtError.Text = "Fill in Work Order Type."
            Exit Sub
        ElseIf ddlWOType.Text = Me.Session("WOTypeOthers") AndAlso txtWOTypeOthers.Text = "" Then
            lblPromtError.Text = "Fill in Work Order Type Others."
            Exit Sub
        End If
        'If txtWOAsstInvNo.Text = "" Then
        '    lblPromtError.Text = "Fill in Assent No."
        '    Exit Sub
        'End If
        'If txtWOIRNo.Text = "" Then
        '    lblPromtError.Text = "Fill in IR No."
        '    Exit Sub
        'End If
        If txtWODesc.Text = "" Then
            lblPromtError.Text = "Fill in Work Order Description."
            Exit Sub
        End If

        If Page.IsValid = True Then
            If CheckDataWO() = True Then
                lblPromtError.Text = "Work order number already exist!"
                Exit Sub
            End If
            SaveWorkOrder()
        End If
    End Sub

    Private Function CheckBlankFields(ByVal W As String) As Boolean
        If Format(txtWOIRNo.Text, "##-###-####-####") Then
            W = "False"
        Else
            W = "True"
        End If
        Return W
    End Function

    Public Function getWOType() As String
        Dim sql As String
        Dim dr As MySqlDataReader = Nothing
        Dim connString As String = Me.Session("strCon")
        Dim cn As MySqlConnection = New MySqlConnection(connString)

        Try
            cn.ConnectionString = connString
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
        Catch exc As Exception
            Return "No connection"
        End Try

        Try
            sql = "select wo_type_Desc from cmms_wo_type where wo_type_desc = '" & txtWOTypeOthers.Text & "';"
            Dim cmd As MySqlCommand = New MySqlCommand(sql, cn)
            cmd.CommandTimeout = 0
            dr = cmd.ExecuteReader()
            If dr.Read() Then
                dr.Close()
                Return "True"
            Else
                Return "False"
            End If
        Catch ex As Exception
            Return "False"
        End Try
        cn.Close()
        cn.Dispose()
    End Function

    Private Sub SaveWorkOrder()
        Dim ds As DataSet
        Dim Type As String = getWOType()
        BCodeAuthor = txtWOCode.Text.Trim
        BNameAuthor = txtWOName.Text.Trim
        AuthorName = txtWOEmpName.Text.Trim
        WrkNo = txtWONo.Text.Trim
        EscalDesc = ddlWOEscal.Text.Trim & "_" & ddlDesignation.Text.Trim
        EscalName = lblWOSelectEscal.Text.Trim
        AstInvNo = txtWOAsstInvNo.Text.ToUpper.Trim
        WODate = txtWOStartDate.Text.Trim
        IRNo = txtWOIRNo.Text.ToUpper.Trim
        WODesc = txtWODesc.Text.Trim
        stat = "OPEN"
        zCode = "VISMIN"
        task = Me.Session("JDesc").ToString.Trim
        SysCreator = Me.Session("res_id").ToString.Trim
        SysModifier = Me.Session("res_id").ToString.Trim

        'If txtWOTypeOthers.Text.ToUpper = Me.Session("WOOthers") Then
        '    lblPromtError.Text = "Work Order Type '" & txtWOTypeOthers.Text.ToUpper & "' is a reserved word."
        '    Exit Sub
        'ElseIf Type = "True" Then
        '    lblPromtError.Text = "Work Order Type '" & txtWOTypeOthers.Text.ToUpper & "' already exist."
        '    Exit Sub
        'End If

        If ddlWOType.Text <> Me.Session("WOTypeOthers") Then
            Dim myWOtype As String
            Try
                myWOtype = "select wo_type_code from cmms_wo_type where wo_type_desc = '" & ddlWOType.Text & "';"
                ds = Execute_DataSetCMMS(myWOtype)
                WOTypeCode = ds.Tables(0).Rows(0).Item(0).ToString
            Catch ex As Exception
                Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
                'MsgBox(ex.Message)
                Exit Sub
            End Try
        Else
            Dim myWOtypeOthers As String
            myWOtypeOthers = "insert into cmms_wo_type(wo_type_code,wo_type_Desc,sys_created,sys_modified,sys_creator,sys_modifier)" _
                           & "values('" & ddlWOType.Text & "','" & txtWOTypeOthers.Text.ToUpper & "',now(),now(),'" & SysCreator & "','" & SysModifier & "')"
            If Execute_Insert(myWOtypeOthers) = False Then
                lblPromtError.Text = Me.Session("InsertError")
                Exit Sub
            End If
            WOTypeCode = ddlWOType.Text
        End If

        If DataSaveWorkOrder() = False Then
            lblPromtError.Text = Me.Session("InsertError")
        Else
            lblPromtError.Text = "Work order is successfully saved!"
            disableFieldsAuthor()
            disableFieldsWorkOrder()
            btnWOSave.Enabled = False
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
    Public Function SpecialNString(ByVal as_string As String) As String
        SpecialNString = Replace(as_string, "&#209;", "Ñ")
    End Function

    Private Function CheckDataWO() As Boolean
        Dim WO_No As String = txtWONo.Text
        Dim mySqlDes As String
        Dim ds As DataSet
        mySqlDes = "select WO_No from cmms_wo_masterheader where wo_no = '" & WO_No.Trim & "';"
        ds = Execute_DataSetMySgl(mySqlDes)
        If Not ds Is Nothing Then
            Return True
        End If
        Return False
    End Function

    Private Function DataSaveWorkOrder() As Boolean
        Dim con As New MySqlConnection
        Dim com As New MySqlCommand
        Dim oTran As MySqlTransaction
        Dim mySqlInsertDetail As String

        mySqlInsertDetail = "insert into cmms_wo_masterheader(bc_code_author,bc_name_author,author_name,wo_no,escalation_desc,escalated_name,wo_type_code," _
                         & "asset_inv_no,wo_date,ir_no,wo_desc,wo_status,zone_code,task,sys_created,sys_modified,sys_creator,sys_modifier)" _
                         & "values('" & BCodeAuthor.Trim & "','" & BNameAuthor.Trim & "','" & SpecialNString(AuthorName.Trim) & "','" & WrkNo.Trim & "','" & EscalDesc.Trim & "','" & SpecialNString(EscalName) & "','" & WOTypeCode.Trim & "'," _
                         & "'" & AstInvNo.Trim & "','" & WODate.Trim & "','" & IRNo.Trim & "','" & Trim(EscapeApostrophe(WODesc)) & "','" & stat.Trim & "','" & zCode.Trim & "','" & task.Trim & "',now(),now(),'" & SysCreator.Trim & "','" & SysModifier.Trim & "');" _
                         & "insert into cmms_wo_detail(wo_no,escalated_date,escalated_by,escalated_to,correctiveaction,sys_created,sys_modified,sys_creator,sys_modifier,resolve)" _
                         & "values('" & WrkNo.Trim & "','" & WODate.Trim & "','" & SpecialNString(AuthorName.Trim) & "','" & SpecialNString(EscalName.Trim) & "','',now(),now(),'" & SysCreator.Trim & "','" & SysModifier.Trim & "','N');" _
                         & "insert into cmms_wo_history(wo_no,escalated_date,escalated_by,escalated_to,resolve,correctiveaction,sys_created,sys_modified,sys_creator,sys_modifier)" _
                         & "values('" & WrkNo.Trim & "','" & WODate.Trim & "','" & SpecialNString(AuthorName.Trim) & "','" & SpecialNString(EscalName.Trim) & "','N','',now(),now(),'" & SysCreator.Trim & "','" & SysModifier.Trim & "');"



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
                Me.Session.Add("InsertError", ex.Message)
                oTran.Rollback()
                con.Close()
                com.Dispose()
                Return False
            End Try
        End If
    End Function

    Protected Sub btnWOOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWOOk.Click
        Response.Redirect("~/WorkOrder/CreateWO.aspx")
    End Sub

    Private Sub WOTemporaryInfo()
        Me.Session.Add("WOCode", txtWOCode.Text)
        Me.Session.Add("WOName", txtWOName.Text)
        Me.Session.Add("WOIDNo", txtWOIDNo.Text)
        Me.Session.Add("WOEmpName", txtWOEmpName.Text)
        Me.Session.Add("WONo", txtWONo.Text)
        Me.Session.Add("WOEscal", ddlWOEscal.Text)
        Me.Session.Add("WODesignation", ddlDesignation.Text)
        Me.Session.Add("WOSelectEscal", lblWOSelectEscal.Text)
        Me.Session.Add("WOType", ddlWOType.Text)
        Me.Session.Add("WOTypeOthers", txtWOTypeOthers.Text)
        Me.Session.Add("WOAsstInvNo", txtWOAsstInvNo.Text)
        Me.Session.Add("WOStartDate", txtWOStartDate.Text)
        Me.Session.Add("WOIRNo", txtWOIRNo.Text)
        Me.Session.Add("WODesc", txtWODesc.Text)
        Me.Session.Add("WOAttachFile", rbAttachFile.Checked = True)
        Me.Session.Add("AttachWOTrue", True)
    End Sub

    Private Sub WOTemporaryRevInfo()
        If Me.Session("WOCode") <> "" Then
            txtWOCode.Text = Me.Session("WOCode")
        Else
            txtWOCode.Text = ""
        End If
        If Me.Session("WOName") <> "" Then
            txtWOName.Text = Me.Session("WOName")
        Else
            txtWOName.Text = ""
        End If
        If Me.Session("WOIDNo") <> "" Then
            txtWOIDNo.Text = Me.Session("WOIDNo")
        Else
            txtWOIDNo.Text = ""
        End If
        If Me.Session("WOEmpName") <> "" Then
            txtWOEmpName.Text = Me.Session("WOEmpName")
        Else
            txtWOEmpName.Text = ""
        End If
        If Me.Session("WONo") <> "" Then
            txtWONo.Text = Me.Session("WONo")
        Else
            txtWONo.Text = ""
        End If
        If Me.Session("WOEscal") <> "" Then
            ddlWOEscal.Text = Me.Session("WOEscal")
        Else
            ddlWOEscal.Text = ""
        End If
        If Me.Session("WODesignation") <> "" Then
            ddlDesignation.Text = Me.Session("WODesignation")
        Else
            ddlDesignation.Text = ""
        End If
        If Me.Session("WOSelectEscal") <> "" Then
            lblWOSelectEscal.Visible = True
            lblWOSelectEscal.Text = Me.Session("WOSelectEscal")
        Else
            lblWOSelectEscal.Text = ""
        End If
        If Me.Session("WOType") <> "" Then
            ddlWOType.Text = Me.Session("WOType")
        Else
            ddlWOType.Text = ""
        End If
        If Me.Session("WOTypeOthers") <> "" Then
            txtWOTypeOthers.Text = Me.Session("WOTypeOthers")
        Else
            txtWOTypeOthers.Text = ""
        End If
        If Me.Session("WOAsstInvNo") <> "" Then
            txtWOAsstInvNo.Text = Me.Session("WOAsstInvNo")
        Else
            txtWOAsstInvNo.Text = ""
        End If
        If Me.Session("WOStartDate") <> "" Then
            txtWOStartDate.Text = Me.Session("WOStartDate")
        Else
            txtWOStartDate.Text = ""
        End If
        If Me.Session("WOIRNo") <> "" Then
            txtWOIRNo.Text = Me.Session("WOIRNo")
        Else
            txtWOIRNo.Text = ""
        End If
        If Me.Session("WODesc") <> "" Then
            txtWODesc.Text = Me.Session("WODesc")
        Else
            txtWODesc.Text = ""
        End If
        If Me.Session("WOAttachFile") = "True" Then
            rbAttachFile.Checked = True
        Else
            rbAttachFile.Checked = False
        End If
        Me.Session.Add("AttachWOTrue", False)
    End Sub

    Protected Sub rbAttachFile_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbAttachFile.CheckedChanged
        If rbAttachFile.Checked = True Then
            WOTemporaryInfo()
            Response.Redirect("~/WorkOrder/AttachFileWO.aspx")
        End If
    End Sub

    Private Sub BindAttachment(ByVal WONo As String)
        CheckLogin()
        Dim WONos As String = Me.Session("WONo")
        Dim cn As New MySqlConnection
        Dim sql As String

        sql = "select File_Name,File_Pic,@rownum:=@rownum+1 as rank,CONCAT(CONCAT(CONVERT(@rownum,CHAR(3)), '/'),File_Name) as RAttchFileName from cmms_wo_attachfiles, (SELECT @rownum:=0) r  where WO_No = '" & WONos & "'"

        Try
            cn.ConnectionString = Me.Session("strCon")
            Dim cmd As MySqlCommand = New MySqlCommand(sql, cn)
            cn.Open()
            Dim reader As MySqlDataReader = cmd.ExecuteReader

            attList.DataSource = reader
            attList.DataBind()
            Dim attachments As String = ""
            Dim _List As New ArrayList
            For Each item As RepeaterItem In attList.Items
                Dim lnk As LinkButton = DirectCast(item.FindControl("lnkRemove"), LinkButton)

                Dim lbl As Label = DirectCast(item.FindControl("attchmnt"), Label)
                Dim filename As String = lbl.Text
                If Not _List.Contains(filename) Then
                    _List.Add(filename)
                    attachments = attachments & filename & ","
                End If
            Next

            Session("AttList") = _List
            Session("attachments") = attachments

            reader.Close()

        Catch ex As Exception
            Throw New Exception(ex.ToString)
        Finally
            cn.Close()
            cn.Dispose()
        End Try
    End Sub
End Class