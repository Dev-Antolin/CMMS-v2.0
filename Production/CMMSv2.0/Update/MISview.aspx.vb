Imports System
Imports System.IO
Imports System.Data

Imports INI_DLL
Imports MySql.Data
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

Imports System.Security.Permissions

Partial Class MISview
    Inherits System.Web.UI.Page
    Dim dateCreated As String = Format(Date.Now, "yyMM")
    Dim Code As String
    Dim Name As String
    Dim IdAssign As String
    Dim NameAssign As String
    Dim RegCode As String
    Dim PTPNum As String
    Dim AssetInv As String
    Dim ItmCode As String
    Dim AssetDesc As String
    Dim PONum As String
    Dim DelDate As String
    Dim SerNum As String
    Dim Stat As String
    Dim TCPU As String
    Dim MSize As String
    Dim HSize As String
    Dim SysCreator As String
    Dim SysModifier As String
    Dim flag As Boolean = False

    Protected Overrides Sub OnInit(ByVal e As EventArgs)
        MyBase.OnInit(e)
        DIV.DivisionSelected = New DivisionDelegate(AddressOf DIV2_Selected)
        DIV.DivisionToFind = New SearchDelegate(AddressOf DIV2_Search)
        Branch.BranchSelected = New BranchDelegate(AddressOf Branch_Selected)
        Branch.BranchToFind = New SearchDelegate(AddressOf Branch_Search)
        UsersID.UsersIDSelected = New UsersIDDelegate(AddressOf UserID_Selected)
        UsersID.UsersIDToFind = New SearchDelegate(AddressOf UsersID_Search)
        UsersIDD.UsersIDDSelected = New UsersIDDDelegate(AddressOf UserIDD_Selected)
        UsersIDD.UsersIDDToFind = New SearchDelegate(AddressOf UsersIDD_Search)
    End Sub

    Public Sub DIV2_Selected(ByVal Code As String, ByVal Name As String)
        
        txtCode.Text = Code
        txtCodeName.Text = Name.Replace("&#241;", "ñ")
        txtID.Text = ""
        txtIDName.Text = ""
        ClearFieldsCategory()
        Me.PanelInformation.Enabled = False
        Me.AssetInventory.Enabled = False
        Me.AssetInventory.Items.Clear()
        'getAssetInventory()
        'ClearFieldsCategory()
        'PInfo(txtIDName.Text)
    End Sub

    Public Sub Branch_Selected(ByVal Code As String, ByVal Name As String)
        txtCode.Text = Code
        '/-----NEW ELY CODE
        Dim STR As String = Name
        If STR.Contains("&#241;") Then
            txtCodeName.Text = Name.Replace("&#241;", "ñ")
        ElseIf STR.Contains("'") Then
            txtCodeName.Text = Name.Replace("'", "")
        Else
            txtCodeName.Text = Name.Replace("&#241;", "ñ")
        End If
        '\-------NEW ELY CODE
        'txtCodeName.Text = Name.Replace("&#241;", "ñ")
        'txtCodeName.Text = Name.Replace("'", "")
        txtID.Text = ""
        txtIDName.Text = ""
        ClearFieldsCategory()
        Me.PanelInformation.Enabled = False
        getAssetInventory()
        'ClearFieldsCategory()
        'PInfo(txtIDName.Text)
    End Sub

    Public Sub UserID_Selected(ByVal UserID As String, ByVal Name As String)
        txtID.Text = UserID
        txtIDName.Text = Name.Replace("&#209;", "Ñ")
        ClearFieldsCategory()
        Me.PanelInformation.Enabled = False
        getAssetInventory()
        'PInfo(Name)
    End Sub

    Public Sub UserIDD_Selected(ByVal UserID As String, ByVal Name As String)
        txtID.Text = UserID
        txtIDName.Text = Name.Replace("&#209;", "Ñ")
        ClearFieldsCategory()
        Me.PanelInformation.Enabled = False
        'disableFieldsInforamtion()
        getAssetInventory()
        'PInfo(Name)
    End Sub

    Public Sub DIV2_Search(ByVal JTtoSearch As String)
        If JTtoSearch = "Search" Then
            MPEDivison2.Show()
        End If
    End Sub

    Public Sub Branch_Search(ByVal JTtoSearch As String)
        If JTtoSearch = "Search" Then
            MPEBranch.Show()
        End If
    End Sub

    Public Sub UsersID_Search(ByVal JTtoSearch As String)
        If JTtoSearch = "Search" Then
            MPEUsersID.Show()
        End If
    End Sub

    Public Sub UsersIDD_Search(ByVal JTtoSearch As String)
        If JTtoSearch = "Search" Then
            MPEUsersIDD.Show()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load        
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
        If Not Page.IsPostBack Then
            Me.Session("Click") = "View"
            cbActive.Enabled = False
            txtAsstInvNo.ReadOnly = True
            If Me.Session("AttachTrue") = True Then
                TemporaryRevInfo()
                BindAttachment(Me.Session("txtAsstInvNo"))
                If attList.Items.Count <> 0 Then
                    rbAttach.Checked = True
                Else
                    rbAttach.Checked = False
                End If
            Else
                If CheckAttach() = True Then
                    Dim AN As String = Me.Session("txtAsstInvNo")
                    If AN = txtAsstInvNo.Text Then
                        Dim DelSql As String
                        DelSql = "delete from cmms_entry_attachfiles where Asset_Inv_No ='" & AN & "';"
                        Execute_Delete(DelSql)
                    End If
                End If
            End If
            PInfo(txtIDName.Text)
            'AssetInventory.Text = Txtbox()
        Else
            txtDelDate.Text = Request.Form(txtDelDate.UniqueID)
        End If
        Me.Session.Add("Code", Me.txtCode.Text)
        lblCountWord.Text = (500 - txtAstDescription.Text.Length) & "/500"
        btnNew.Visible = False
        Disable(True)
        DisableTextboxCtrl()
        EnableTextboxCtrl()
        'SelectCompType()
        RegisterDisableEnterKeyScript()
        DisEnter()
        'If RemoveAttList(Me.Session("txtAsstInvNo")) = True Then
        '    rbAttach.Checked = True
        'Else
        '    rbAttach.Checked = False
        'End If
        Me.Session.Add("NewBasicAssetNo", Me.txtAsstInvNo.Text)
        Me.Session.Add("SelectedBname", Me.txtCodeName.Text)

    End Sub

    Private Sub DisEnter()
        lbASearchCode.Attributes.Add("onkeypress", "return trapEnter(event, 'doPostback()')")
        txtCode.Attributes.Add("onkeypress", "return trapEnter(event, 'doPostback()')")
        txtCodeName.Attributes.Add("onkeypress", "return trapEnter(event, 'doPostback()')")
        txtID.Attributes.Add("onkeypress", "return trapEnter(event, 'doPostback()')")
        txtIDName.Attributes.Add("onkeypress", "return trapEnter(event, 'doPostback()')")
        txtPTPNo.Attributes.Add("onkeypress", "return trapEnter(event, 'doPostback()')")
        txtSerialNo.Attributes.Add("onkeypress", "return trapEnter(event, 'doPostback()')")
        txtPONo.Attributes.Add("onkeypress", "return trapEnter(event, 'doPostback()')")
        txtCPUOthers.Attributes.Add("onkeypress", "return trapEnter(event, 'doPostback()')")
        txtHardDiskOthers.Attributes.Add("onkeypress", "return trapEnter(event, 'doPostback()')")
        txtMemoSizeOthers.Attributes.Add("onkeypress", "return trapEnter(event, 'doPostback()')")
        txtAsstInvNo.Attributes.Add("onkeypress", "return trapEnter(event, 'doPostback()')")
        txtDelDate.Attributes.Add("onkeypress", "return trapEnter(event, 'doPostback()')")
        'txtIP.Attributes.Add("onkeypress", "return NumberDOT(event)")
        'txtIP.Attributes.Add("onkeypress", "return trapEnter(event, 'doPostback()')")        
    End Sub

    Private Sub CheckLogin()
        If Me.Session("fName") = "" Then
            Response.Redirect("~/login.aspx")
        End If
    End Sub

    Private Sub CheckUser()
        If Not Me.Session("JDesc").ToString.Contains("HELPDESK") AndAlso Not Me.Session("JDesc").ToString.Contains("MMD") AndAlso Not Me.Session("JDesc").ToString.Contains("LPTL") Then
            Response.Redirect("~/Unauthorized.aspx")
        End If
    End Sub

    Private Sub PInfo(ByRef name As String)
        If name = "" Then
            PanelInformation.Enabled = False
        Else
            PanelInformation.Enabled = True
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

    Private Function CheckBlankFields(ByVal W As String) As Boolean
        If ddlCompType.Text = "0" OrElse ddlCompType.Text = "" Then
            W = "False"
            lblErrorSaving.Text = "Select computer type!"
        ElseIf txtPTPNo.Text = "" Then
            W = "False"
            lblErrorSaving.Text = "Fill in manual asset no.!"
        ElseIf txtAstDescription.Text = "" Then
            W = "False"
            lblErrorSaving.Text = "Fill in description!"
        ElseIf ddlCPU.Text = "0" OrElse Me.ddlCPU.text = "" Then
            W = "False"
            lblErrorSaving.Text = "Select CPU type!"
        ElseIf ddlMemoSize.Text = "0" Then
            W = "False"
            lblErrorSaving.Text = "Select Memory type!"
        ElseIf ddlHardDisk.Text = "0" Then
            W = "False"
            lblErrorSaving.Text = "Select Hard Disk type!"
        ElseIf txtPONo.Text = "" Then
            W = "False"
            lblErrorSaving.Text = "Fill in P.O. no.!"
        ElseIf Request.Form(txtDelDate.UniqueID) = "" Then
            W = "False"
            lblErrorSaving.Text = "Fill in delivery date!"
        ElseIf Me.ddlIPType.SelectedIndex = 0 And Me.txtIP.Text <> Nothing Then
            W = "False"
            lblErrorSaving.Text = "Select IP Type"
        ElseIf Me.ddlIPType.SelectedIndex <> 0 And Me.txtIP.Text = Nothing Then
            W = "False"
            lblErrorSaving.Text = "Enter IP Address"
        ElseIf Me.ddlIPType.SelectedIndex <> 0 And Me.txtIP.text <> Nothing And IsIpValid(Trim(Me.txtIP.Text)) = False Then
            W = "False"
            lblErrorSaving.Text = "Invalid IP Address"
        Else
            W = "True"
        End If
        Return W
    End Function

    Private Sub Disable(ByVal R As Boolean)
        txtCode.Enabled = R
        txtCodeName.Enabled = R
        txtID.Enabled = R
        txtIDName.Enabled = R

        If R = True Then
            txtCode.Attributes("class") = "ReadOnlyBackColor"
            txtCodeName.Attributes("class") = "ReadOnlyBackColor"
            txtID.Attributes("class") = "ReadOnlyBackColor"
            txtIDName.Attributes("class") = "ReadOnlyBackColor"
        Else
            txtCode.Attributes("class") = "ReadOnlyBackColor"
            txtCodeName.Attributes("class") = "ReadOnlyBackColor"
            txtID.Attributes("class") = "ReadOnlyBackColor"
            txtIDName.Attributes("class") = "ReadOnlyBackColor"
        End If
    End Sub

    Public Sub RemoveAtt(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim AssetNo As String = Me.Session("txtAsstInvNo")
        Dim sqlDelPic As String
        Dim cmdArg As String = e.CommandArgument.ToString
        Dim str As String = cmdArg.ToString.Substring(cmdArg.IndexOf("/") + 1, cmdArg.Length - (cmdArg.IndexOf("/")) - 1)
        Dim idx As Integer = cmdArg.ToString.Substring(0, cmdArg.IndexOf("/")) - 1
        attList.Items(idx).Visible = False
        sqlDelPic = "delete from cmms_entry_attachfiles where Asset_Inv_No = '" & AssetNo & "' and File_Name ='" & str & "';"
        Execute_Delete(sqlDelPic)
        If RemoveAttList(AssetNo) = True Then
            rbAttach.Checked = True
        Else
            rbAttach.Checked = False
        End If
    End Sub

    Private Function RemoveAttList(ByVal AssetNo As String) As Boolean
        Dim mySqlDes As String
        Dim ds As DataSet
        mySqlDes = "select File_Name,File_Pic from cmms_entry_attachfiles, (SELECT @rownum:=0) r  where Asset_Inv_No = '" & AssetNo & "'"
        ds = Execute_DataSet(mySqlDes)
        If Not ds Is Nothing Then
            Return True
        End If
        Return False
    End Function

    Protected Sub rbBranch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbBranch.CheckedChanged
        If rbBranch.Checked = True Then
            rbDivision.Checked = False
            rbAttach.Checked = False
            rbScan.Checked = False
            Me.AssetInventory.Items.Clear()
            Me.AssetInventory.Enabled = False
            lblBCCode.Text = "BC Code"
            lblBCName.Text = "BC Name"
            lblCountWord.Text = "500/500"
            EmptyFieldsCategory()
            EmptyFieldsInformation()
            DisableTextboxCtrl()
            ClearFieldsCategory()
            PInfo(txtIDName.Text)
            attList.Visible = False
            If CheckAttach() = True Then
                Dim AN As String = Me.Session("txtAsstInvNo")
                Dim DelSql As String
                DelSql = "delete from cmms_entry_attachfiles where Asset_Inv_No ='" & AN & "';"
                Execute_Delete(DelSql)
            End If
        End If
    End Sub

    Protected Sub rbDivision_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbDivision.CheckedChanged
        If rbDivision.Checked = True Then
            rbBranch.Checked = False
            rbAttach.Checked = False
            rbScan.Checked = False
            Me.AssetInventory.Items.Clear()
            Me.AssetInventory.Enabled = False
            lblBCCode.Text = "Division Code"
            lblBCName.Text = "Division Name"
            lblCountWord.Text = "500/500"
            EmptyFieldsCategory()
            EmptyFieldsInformation()
            DisableTextboxCtrl()
            ClearFieldsCategory()
            PInfo(txtIDName.Text)
            attList.Visible = False
            If CheckAttach() = True Then
                Dim AN As String = Me.Session("txtAsstInvNo")
                Dim DelSql As String
                DelSql = "delete from cmms_entry_attachfiles where Asset_Inv_No ='" & AN & "';"
                Execute_Delete(DelSql)
            End If
        End If
    End Sub

    Protected Sub rbScan_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbScan.CheckedChanged
        If rbScan.Checked = True Then
            rbAttach.Checked = False
            TemporaryInfo()
            Response.Redirect("~/DataEntry/TwainUploadASPNET.htm")
        End If
    End Sub

    Protected Sub rbAttach_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbAttach.CheckedChanged
        If txtAsstInvNo.Text = "" Then
            lblErrorSaving.Text = "Asset inventory number is empty."
            rbAttach.Checked = False
            Exit Sub
        Else
            rbScan.Checked = False
            TemporaryInfo()
            Response.Redirect("~/DataEntry/AttachFileB.aspx")
        End If
    End Sub

    Protected Sub lbSearchCode_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles lbASearchCode.Click

        If rbDivision.Checked = True Then            
            If Me.txtCode.Text <> Nothing And Me.txtID.Text = Nothing Then
                If Me.txtCode.Text <> Nothing And Me.Session("flag") = Nothing Then
                    MPEDivison2.Show()
                Else
                    MPEUsersIDD.Show()
                End If
            Else
                If Me.Session("flag") <> Nothing Then
                    MPEUsersIDD.Show()
                Else
                    MPEDivison2.Show()
                End If
            End If
        Else
            If Me.txtCode.Text <> Nothing And Me.txtID.Text = Nothing Then
                If Me.txtCode.Text <> Nothing And Me.Session("bflag") = Nothing Then
                    MPEBranch.Show()
                Else
                    MPEUsersID.Show()
                End If
            Else
                If Me.Session("bflag") <> Nothing Then
                    MPEUsersID.Show()
                Else
                    MPEBranch.Show()
                End If
            End If
        End If
    End Sub

    Protected Sub lbSearchID_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles lbASearchID.Click
        If Me.txtCode.Text = Nothing Then Exit Sub
        If rbDivision.Checked = True Then
            If Me.txtID.Text <> Nothing And Me.txtCode.Text = Nothing Then
                If Me.txtCode.Text = Nothing And Me.Session("flag") <> Nothing Then
                    MPEUsersIDD.Show()
                Else
                    MPEDivison2.Show()
                End If
            Else
                MPEUsersIDD.Show()
            End If
        Else
            If Me.txtID.Text <> Nothing And Me.txtCode.Text = Nothing Then
                If Me.txtCode.Text = Nothing And Me.Session("bflag") <> Nothing Then
                    MPEUsersID.Show()
                Else
                    MPEBranch.Show()
                End If
            Else
                MPEUsersID.Show()
            End If
        End If
    End Sub

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Response.Redirect("MISview.aspx")
    End Sub

    Private Sub DisableControls(ByVal c As Control)
        If TypeOf c Is WebControl Then
            CType(c, WebControl).Enabled = False
        End If

        For Each child As Control In c.Controls
            DisableControls(child)
        Next
    End Sub

    Protected Sub disableFieldsCategory()
        For Each c As Control In PanelCategory.Controls
            If TypeOf c Is TextBox Then
                CType(c, TextBox).Enabled = False
            End If
            If TypeOf c Is ImageButton Then
                CType(c, ImageButton).Enabled = False
            End If
            If TypeOf c Is RadioButton Then
                CType(c, RadioButton).Enabled = False
            End If
        Next
    End Sub

    Protected Sub ClearFieldsCategory()
        For Each c As Control In PanelInformation.Controls
            If TypeOf c Is TextBox Then
                CType(c, TextBox).Text = ""
            End If
            If TypeOf c Is DropDownList Then
                CType(c, DropDownList).ClearSelection()
            End If
            If TypeOf c Is CheckBox Then
                CType(c, CheckBox).Checked = False
            End If
            Me.btnUpdate.Visible = False
            Me.lblCountWord.Text = "500/500"
            Me.lblErrorSaving.Text = String.Empty
        Next
    End Sub

    Protected Sub disableFieldsInforamtion()
        For Each c As Control In PanelInformation.Controls
            If TypeOf c Is TextBox Then
                CType(c, TextBox).Enabled = False
            End If
            If TypeOf c Is DropDownList Then
                CType(c, DropDownList).Enabled = False
            End If
            If TypeOf c Is CheckBox Then
                CType(c, CheckBox).Enabled = False
                CType(c, CheckBox).Checked = False
            End If
            'If TypeOf c Is RadioButton Then
            '    CType(c, RadioButton).Enabled = False
            'End If
            If TypeOf c Is Button Then
                CType(c, Button).Visible = False
            End If
        Next
        For Each rptItem As RepeaterItem In attList.Items
            If rptItem.FindControl("lnkRemove") IsNot Nothing Then
                DirectCast(rptItem.FindControl("lnkRemove"), LinkButton).Enabled = False
                DirectCast(rptItem.FindControl("attchmnt"), Label).Enabled = False
            End If
        Next
    End Sub

    Protected Sub EmptyFieldsCategory()
        For Each ctrl As Control In PanelCategory.Controls
            If TypeOf ctrl Is TextBox Then
                CType(ctrl, TextBox).Text = ""
            End If
        Next
    End Sub

    Protected Sub EmptyFieldsInformation()
        For Each ctrl As Control In PanelInformation.Controls
            If TypeOf ctrl Is TextBox Then
                CType(ctrl, TextBox).Text = ""
            End If
        Next
    End Sub

    Protected Sub DisableTextboxCtrl()
        Me.ddlCompType.Enabled = False
        Me.txtPTPNo.ReadOnly = True
        txtCPUOthers.Visible = False
        txtMemoSizeOthers.Visible = False
        txtHardDiskOthers.Visible = False

    End Sub

    Protected Sub EnableTextboxCtrl()
        If Me.Session("CPUOthers") IsNot Nothing Then
            If ddlCPU.Text = Me.Session("CPUOthers").ToString() Then
                txtCPUOthers.Visible = True
            Else
                Me.txtCPUOthers.Text = String.Empty
                txtCPUOthers.Visible = False
            End If
            If ddlMemoSize.Text = Me.Session("MemoryOthers").ToString() Then
                txtMemoSizeOthers.Visible = True
            Else
                Me.txtMemoSizeOthers.Text = String.Empty
                txtMemoSizeOthers.Visible = False
            End If
            If ddlHardDisk.Text = Me.Session("HardDiskOthers").ToString() Then
                txtHardDiskOthers.Visible = True
            Else
                Me.txtHardDiskOthers.Text = String.Empty
                txtHardDiskOthers.Visible = False
            End If
        End If
    End Sub

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
                Me.Session.Add("DeleteError", ex.Message)
                oTran.Rollback()
                con.Close()
                com.Dispose()
                Return False
            End Try
        End If
    End Function

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

    Public Function Execute_DataSet(ByVal as_mysql As String) As DataSet
        Dim Con As New MySqlConnection
        Dim Com As New MySqlCommand
        Dim sqlAdapter As MySqlDataAdapter
        Dim sqlDataset As New DataSet

        Execute_DataSet = Nothing
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
                    Execute_DataSet = sqlDataset
                    sqlDataset.Dispose()
                    sqlAdapter.Dispose()
                End If
            End If
            Con.Close()
        Catch ex As Exception
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
            Con.Close()
            Com.Dispose()
        End Try
    End Function

    Private Function DivMan(ByVal jobDesc As String) As Boolean
        Dim mySqlDes As String
        Dim ds As DataSet
        mySqlDes = "select distinct task from cmms_users where task like '%DIVMAN%';"
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

    Private Function DeptMan(ByVal jobDesc As String) As Boolean
        Dim mySqlDes As String
        Dim ds As DataSet
        mySqlDes = "select distinct task from cmms_users where task like '%DEPTMAN%';"
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

    Private Function LPTL(ByVal jobDesc As String) As Boolean
        Dim mySqlDes As String
        Dim ds As DataSet
        mySqlDes = "select distinct task from cmms_users where task like '%/BM-R/%';"
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

    Private Function BranchD(ByVal jobDesc As String) As Boolean
        Dim mySqlDes As String
        Dim ds As DataSet
        mySqlDes = "select distinct task from cmms_users where task like '%BM/BOSMAN%';"
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

    Private Function LPT(ByVal jobDesc As String) As Boolean
        Dim mySqlDes As String
        Dim ds As DataSet
        mySqlDes = "select distinct task from cmms_users where task like '%LPT%';"
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

    Private Function RCT(ByVal jobDesc As String) As Boolean
        Dim mySqlDes As String
        Dim ds As DataSet
        mySqlDes = "select task from cmms_users where task like '%RCT-A%';"
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

    Private Sub TemporaryInfo()
        Me.Session.Add("rbDivision", rbDivision.Checked)
        Me.Session.Add("rbBranch", rbBranch.Checked)
        Me.Session.Add("txtCode", txtCode.Text)
        Me.Session.Add("txtCodeName", txtCodeName.Text)
        Me.Session.Add("txtID", txtID.Text)
        Me.Session.Add("txtIDName", txtIDName.Text)
        Me.Session.Add("ddlCompType", ddlCompType.Text)
        Me.Session.Add("txtAsstInvNo", txtAsstInvNo.Text)
        Me.Session.Add("txtPTPNo", txtPTPNo.Text)
        Me.Session.Add("txtAstDesc", txtAstDescription.Text)
        Me.Session.Add("ddlCPU", ddlCPU.Text)
        Me.Session.Add("txtCPUOthers", txtCPUOthers.Text)
        Me.Session.Add("ddlMemo", ddlMemoSize.Text)
        Me.Session.Add("txtMemoOthers", txtMemoSizeOthers.Text)
        Me.Session.Add("ddlHard", ddlHardDisk.Text)
        Me.Session.Add("txtHardOthers", txtHardDiskOthers.Text)
        Me.Session.Add("txtSerialNo", txtSerialNo.Text)
        Me.Session.Add("rbAttach", rbAttach.Checked)
        Me.Session.Add("txtPONo", txtPONo.Text)
        Me.Session.Add("txtDelDate", Request.Form(txtDelDate.UniqueID))
        Me.Session.Add("AttachTrue", True)
    End Sub

    Private Sub TemporaryRevInfo()
        If Me.Session("rbDivision") = "True" Then
            rbDivision.Checked = True
        Else
            rbDivision.Checked = False
        End If
        If Me.Session("rbBranch") = "True" Then
            rbBranch.Checked = True
        Else
            rbBranch.Checked = False
        End If
        If Me.Session("txtCode") <> "" Then
            txtCode.Text = Me.Session("txtCode")
        Else
            txtCode.Text = ""
        End If
        If Me.Session("txtCodeName") <> "" Then
            txtCodeName.Text = Me.Session("txtCodeName")
        Else
            txtCodeName.Text = ""
        End If
        If Me.Session("txtID") <> "" Then
            txtID.Text = Me.Session("txtID")
        Else
            txtID.Text = ""
        End If
        If Me.Session("txtIDName") <> "" Then
            txtIDName.Text = Me.Session("txtIDName")
        Else
            txtIDName.Text = ""
        End If
        If Me.Session("ddlCompType") <> "" Then
            ddlCompType.Text = Me.Session("ddlCompType")
        Else
            ddlCompType.Text = ""
        End If
        If Me.Session("txtAsstInvNo") <> "" Then
            txtAsstInvNo.Text = Me.Session("txtAsstInvNo")
        Else
            txtAsstInvNo.Text = ""
        End If
        If Me.Session("txtPTPNo") <> "" Then
            txtPTPNo.Text = Me.Session("txtPTPNo")
        Else
            txtPTPNo.Text = ""
        End If
        If Me.Session("txtAstDesc") <> "" Then
            txtAstDescription.Text = Me.Session("txtAstDesc")
        Else
            txtAstDescription.Text = ""
        End If
        If Me.Session("ddlCPU") <> "" Then
            ddlCPU.Text = Me.Session("ddlCPU")
        Else
            ddlCPU.Text = ""
        End If
        If Me.Session("txtCPUOthers") <> "" Then
            txtCPUOthers.Text = Me.Session("txtCPUOthers")
        Else
            txtCPUOthers.Text = ""
        End If
        If Me.Session("ddlMemo") <> "" Then
            ddlMemoSize.Text = Me.Session("ddlMemo")
        Else
            ddlMemoSize.Text = ""
        End If
        If Me.Session("txtMemoOthers") <> "" Then
            txtMemoSizeOthers.Text = Me.Session("txtMemoOthers")
        Else
            txtMemoSizeOthers.Text = ""
        End If
        If Me.Session("ddlHard") <> "" Then
            ddlHardDisk.Text = Me.Session("ddlHard")
        Else
            ddlHardDisk.Text = ""
        End If
        If Me.Session("txtHardOthers") <> "" Then
            txtHardDiskOthers.Text = Me.Session("txtHardOthers")
        Else
            txtHardDiskOthers.Text = ""
        End If
        If Me.Session("txtSerialNo") <> "" Then
            txtSerialNo.Text = Me.Session("txtSerialNo")
        Else
            txtSerialNo.Text = ""
        End If
        If Me.Session("rbAttach") = "True" Then
            rbAttach.Checked = True
        Else
            rbAttach.Checked = False
        End If
        If Me.Session("txtPONo") <> "" Then
            txtPONo.Text = Me.Session("txtPONo")
        Else
            txtPONo.Text = ""
        End If
        If Me.Session("txtDelDate") <> "" Then
            txtDelDate.Text = Me.Session("txtDelDate")
        Else
            txtDelDate.Text = ""
        End If
        Me.Session.Add("AttachTrue", False)
    End Sub

    Private Sub BindAttachment(ByVal AstNo As String)
        CheckLogin()
        Dim AssetNo As String = Me.Session("txtAsstInvNo")
        Dim cn As New MySqlConnection
        Dim sql As String

        sql = "select File_Name,File_Pic,@rownum:=@rownum+1 as rank,CONCAT(CONCAT(CONVERT(@rownum,CHAR(3)), '/'),File_Name) as RAttchFileName from cmms_entry_attachfiles, (SELECT @rownum:=0) r  where Asset_Inv_No = '" & AssetNo & "'"

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

    Private Function CheckAttach() As Boolean
        Dim AssetNo As String = Me.Session("txtAsstInvNo")
        Dim CheckSql As String
        Dim ds As DataSet

        CheckSql = "select Asset_Inv_No from cmms_entry_attachfiles where Asset_Inv_No = '" & AssetNo & "';"

        ds = Execute_DataSet(CheckSql)
        If Not ds Is Nothing AndAlso ds.Tables(0).Rows(0).Item(0).ToString <> "" Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function CheckAttachA() As Boolean
        Dim AssetNo As String = Me.Session("txtAAsstInvNo")
        Dim CheckSql As String
        Dim ds As DataSet

        CheckSql = "select Asset_Inv_No from cmms_entry_attachfiles where Asset_Inv_No = '" & AssetNo & "';"

        ds = Execute_DataSet(CheckSql)
        If Not ds Is Nothing AndAlso ds.Tables(0).Rows(0).Item(0).ToString <> "" Then
            Return True
        Else
            Return False
        End If
    End Function

    Protected Sub RegisterDisableEnterKeyScript()
        Dim sb As New StringBuilder()
        sb.Append("<script language='JavaScript'>")
        sb.Append(" function trapEnter(e, enterFunction) ")
        sb.Append(" { ")
        sb.Append("if (!e) e = window.event; ")
        sb.Append("if (!e) return false; ")
        sb.Append("if (e.keyCode == 13) ")
        sb.Append("{ ")
        sb.Append("     e.cancelBubble = true; ")
        sb.Append("     if (e.returnValue) e.returnValue = false; ")
        sb.Append("     if (e.stopPropagation) e.stopPropagation(); ")
        sb.Append("     if (enterFunction) eval(enterFunction); ")
        sb.Append("     return false; ")
        sb.Append("} ")
        sb.Append("else ")
        sb.Append("     return true;   ")
        sb.Append("}")
        sb.Append("<")
        sb.Append("/script>")

        If Not ClientScript.IsClientScriptBlockRegistered(Me.[GetType](), "trapEnter") Then
            ClientScript.RegisterClientScriptBlock(Me.[GetType](), "trapEnter", sb.ToString())
        End If
    End Sub

    'Changes as for 09-07-13

    Protected Sub AssetInventory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles AssetInventory.SelectedIndexChanged
        If Page.IsPostBack Then

            If Me.AssetInventory.SelectedIndex = 0 Then
                ClearFieldsCategory()
                Me.PanelInformation.Enabled = False
                Me.AssetInventory.Enabled = False
                Me.AssetInventory.Items.Clear()
                getAssetInventory()
                Exit Sub
            End If
            Try
                LoadItems()
            Catch ex As Exception                
                ClearFieldsCategory()
                Me.PanelInformation.Enabled = False
                Me.AssetInventory.Enabled = False
                Me.AssetInventory.Items.Clear()
                getAssetInventory()                
            End Try
        End If
    End Sub

    Private Sub LoadItems()        
        Dim sql As New StringBuilder()
        sql.Append("SELECT")
        sql.Append("    m.Item_Code, ")
        sql.Append("    m.Asset_Inv_No, ")
        sql.Append("    m.PTP_No, ")
        sql.Append("    m.Asset_Desc, ")
        sql.Append("    m.PO_Number, ")
        sql.Append("    m.Delivery_Date, ")
        sql.Append("    IFNULL(m.Serial_No,''), ")
        sql.Append("    m.`Status`, ")
        sql.Append("    UPPER(cpu.CPU_Description), ")
        sql.Append("    UPPER(mmry.Memory_Description), ")
        sql.Append("    UPPER(hdd.HardDisk_Description), ")
        sql.Append("    IFNULL(e.Comp_IP,''), ")
        sql.Append("    IFNULL(UPPER(e.Comp_IP_Type),'') ")
        sql.Append("FROM")
        sql.Append("    cmms.cmms_entry_details e ")
        sql.Append("INNER JOIN")
        sql.Append("    cmms.cmms_cpu_class cpu ")
        sql.Append("ON")
        sql.Append("    e.Comp_CPU_Class_ID = cpu.comp_cpu_Class_ID ")
        sql.Append("INNER JOIN")
        sql.Append("    cmms.cmms_memory_class mmry ")
        sql.Append("ON")
        sql.Append("    e.Memory_Class_ID = mmry.Memory_Class_ID ")
        sql.Append("INNER JOIN")
        sql.Append("    cmms.cmms_harddisk_class hdd ")
        sql.Append("ON")
        sql.Append("    e.HardDisk_Class_ID = hdd.HardDisk_Class_ID ")
        sql.Append("INNER JOIN")
        sql.Append("    cmms.cmms_entry_masterheader m ")
        sql.Append("ON")
        sql.Append("    m.Asset_Inv_No = e.Asset_Inv_No ")
        sql.Append("WHERE")
        sql.Append("    e.Asset_Inv_No = @AssetNo;")
        Using con As New MySqlConnection(Me.Session("strCon").ToString())            
            Using cmd As New MySqlCommand(sql.ToString(), con)
                cmd.Parameters.Add(New MySqlParameter("AssetNo", MySqlDbType.VarString, 10)).Value() = Me.AssetInventory.SelectedItem.Text
                cmd.CommandType = CommandType.Text
                Try
                    con.Open()
                    Using DataRead As MySqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        If DataRead.HasRows Then
                            If DataRead.Read Then
                                Select Case DataRead.GetInt16(0)
                                    Case 500
                                        Me.ddlCompType.SelectedIndex = 1
                                        Exit Select
                                    Case 501
                                        Me.ddlCompType.SelectedIndex = 2
                                        Exit Select
                                    Case 502
                                        Me.ddlCompType.SelectedIndex = 3
                                    Case Else
                                        Me.ddlCompType.SelectedIndex = 0
                                        Exit Select
                                End Select
                                Me.txtAsstInvNo.Text = DataRead.GetString(1)
                                Me.txtPTPNo.Text = DataRead.GetString(2)
                                Me.txtAstDescription.Text = DataRead.GetString(3)
                                Me.txtPONo.Text = DataRead.GetString(4)
                                'Dim dt As Date = DataRead.GetDateTime(5)
                                Me.txtDelDate.Text = DataRead.GetDateTime(5).ToString("yyyy-MM-dd")
                                Me.txtSerialNo.Text = DataRead(6).ToString()
                                Me.cbActive.Checked = IIf(DataRead.GetString(7) = "A", True, False)
                                Me.ddlCPU.SelectedValue = DataRead.GetString(8)
                                Me.ddlMemoSize.SelectedValue = DataRead.GetString(9)
                                Me.ddlHardDisk.SelectedValue = DataRead.GetString(10)
                                Me.txtIP.Text = DataRead.GetString(11)
                                Me.ddlIPType.SelectedValue = DataRead.GetString(12)
                                lblCountWord.Text = (500 - CInt(Me.txtAstDescription.Text.Length)) & "/500"
                                Me.btnUpdate.Visible = True
                                Me.PanelInformation.Enabled = True
                                DataRead.Close()
                            Else
                                Me.PanelInformation.Enabled = False
                            End If
                        End If
                    End Using
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
        End Using
    End Sub

    Private Sub getAssetInventory()
        Dim sql As New StringBuilder()        
        Using con As New MySqlConnection(Me.Session("strCon").ToString())
            sql.Append("SELECT Asset_Inv_No FROM cmms.cmms_entry_masterheader ")
            sql.Append("WHERE")
            sql.Append("    Bc_Code = @Bcode ")
            sql.Append("AND Bc_Name = @Bname ")
            sql.Append("AND Res_Id_Assigned = @res_id ")
            sql.Append("AND Emp_Name_Assigned = @res_name ")
            sql.Append("AND Item_Code IN ('500','501','502') ")
            sql.Append("ORDER BY")
            sql.Append("    Asset_Inv_No ASC; ")
            sql.Append("SELECT UPPER(cpu_description) FROM cmms.cmms_cpu_class ORDER BY cpu_description ASC; ")
            sql.Append("SELECT UPPER(memory_description) FROM cmms.cmms_memory_class ORDER BY memory_description ASC; ")
            sql.Append("SELECT UPPER(harddisk_description) FROM cmms.cmms_harddisk_class ORDER BY harddisk_description ASC; ")
            Using cmd As New MySqlCommand(sql.ToString(), con)
                cmd.Parameters.Add("Bcode", MySqlDbType.VarString, 3).Value = Me.txtCode.Text
                cmd.Parameters.Add("Bname", MySqlDbType.VarString, 50).Value = Me.txtCodeName.Text
                cmd.Parameters.Add("res_id", MySqlDbType.VarString, 50).Value = Me.txtID.Text
                cmd.Parameters.Add("res_name", MySqlDbType.VarString, 50).Value = Me.txtIDName.Text
                cmd.CommandType = CommandType.Text
                Try
                    con.Open()
                    Using DataRead As MySqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        If DataRead.HasRows() Then
                            AssetInventory.Items.Clear()
                            AssetInventory.Items.Add("")
                            While DataRead.Read()
                                AssetInventory.Items.Add(DataRead.GetString(0))
                            End While
                            Me.AssetInventory.Enabled = True
                            DataRead.NextResult()
                            If DataRead.HasRows() Then
                                ddlCPU.Items.Clear()
                                While DataRead.Read()
                                    ddlCPU.Items.Add(DataRead.GetString(0))
                                End While
                                ddlCPU.Items.Add("OTHERS")
                                Me.Session.Add("CPUOthers", "OTHERS")
                            End If
                            DataRead.NextResult()
                            If DataRead.HasRows() Then
                                ddlMemoSize.Items.Clear()
                                While DataRead.Read()
                                    ddlMemoSize.Items.Add(DataRead.GetString(0))
                                End While
                                ddlMemoSize.Items.Add("OTHERS")
                                Me.Session.Add("MemoryOthers", "OTHERS")
                            End If
                            DataRead.NextResult()
                            If DataRead.HasRows() Then
                                ddlHardDisk.Items.Clear()
                                While DataRead.Read()
                                    ddlHardDisk.Items.Add(DataRead.GetString(0))
                                End While
                                ddlHardDisk.Items.Add("OTHERS")
                                Me.Session.Add("HardDiskOthers", "OTHERS")                                
                            End If
                            DataRead.Close()
                        End If
                    End Using
                Catch ex As Exception
                    Throw
                End Try
            End Using        
        End Using
    End Sub

    Private Function DataSaveTransaction() As Boolean
        Dim Trans As MySqlTransaction = Nothing
        Dim sql As New StringBuilder()
        Using con As New MySqlConnection(Me.Session("strCon").ToString())
            Try
                sql.Append("UPDATE cmms.cmms_entry_masterheader ")
                sql.Append("SET Asset_Desc = @AssetDesc, ")
                sql.Append("    Serial_No = @SerialNo, ")
                sql.Append("    PO_Number = @PO, ")
                sql.Append("    Sys_Modified = NOW(), ")
                sql.Append("    Sys_Modifier = @UpdateBy, ")
                sql.Append("    Delivery_Date = @delDate ")
                sql.Append("WHERE Asset_Inv_No = @AssetNo ;")

                sql.Append("UPDATE cmms.cmms_entry_details ")
                sql.Append("SET Comp_IP = @IP, ")
                sql.Append("    Comp_IP_Type = @IPType, ")
                sql.Append("    Comp_CPU_Class_ID = @CPU, ")
                sql.Append("    Memory_Class_ID = @Memory, ")
                sql.Append("    HardDisk_Class_ID = @HDD, ")
                sql.Append("    Sys_Modified = NOW(), ")
                sql.Append("    Sys_Modifier = @UpdateBy ")
                sql.Append("WHERE Asset_Inv_No = @AssetNo; ")

                sql.Append("UPDATE cmms.cmms_entry_history ")
                sql.Append("SET Sys_Modified = NOW(), ")
                sql.Append("    Sys_Modifier = @UpdateBy ")
                sql.Append("WHERE Asset_Inv_No = @AssetNo; ")

                Using cmd As New MySqlCommand(sql.ToString(), con, Trans)
                    Dim a As String = String.Format(Me.txtDelDate.Text, "yyyy-MM-dd HH:mm:ssttt")
                    cmd.Parameters.Add(New MySqlParameter("AssetDesc", MySqlDbType.VarChar, 500)).Value() = Trim(Me.txtAstDescription.Text)
                    cmd.Parameters.Add(New MySqlParameter("SerialNo", MySqlDbType.VarString, 50)).Value() = Trim(Me.txtSerialNo.Text.ToUpper())
                    cmd.Parameters.Add(New MySqlParameter("PO", MySqlDbType.VarString, 10)).Value() = Trim(Me.txtPONo.Text)
                    cmd.Parameters.Add(New MySqlParameter("UpdateBy", MySqlDbType.VarString, 20)).Value() = Me.Session("res_id").ToString()
                    cmd.Parameters.Add(New MySqlParameter("IP", MySqlDbType.VarString, 15)).Value() = Trim(Me.txtIP.Text)
                    cmd.Parameters.Add(New MySqlParameter("IPType", MySqlDbType.VarString, 10)).Value() = Me.ddlIPType.SelectedItem.ToString().ToUpper()
                    cmd.Parameters.Add(New MySqlParameter("AssetNo", MySqlDbType.VarString, 20)).Value() = Me.txtAsstInvNo.Text
                    cmd.Parameters.Add(New MySqlParameter("delDate", MySqlDbType.DateTime)).Value() = Request.Form(Me.txtDelDate.UniqueID)
                    cmd.Parameters.Add(New MySqlParameter("CPU", MySqlDbType.VarString, 10)).Value() = Me.TCPU
                    cmd.Parameters.Add(New MySqlParameter("Memory", MySqlDbType.VarString, 10)).Value() = Me.MSize
                    cmd.Parameters.Add(New MySqlParameter("HDD", MySqlDbType.VarString, 10)).Value() = Me.HSize
                    cmd.CommandType = CommandType.Text
                    con.Open()
                    Trans = con.BeginTransaction()
                    Dim i As Integer = cmd.ExecuteNonQuery()
                    If i > 0 Then
                        Trans.Commit()
                        Return True
                    Else
                        If con.State = ConnectionState.Open Then
                            Trans.Rollback()
                            con.Dispose()
                        End If
                        Me.Session.Add("InsertError", "Error on Update")
                        Return False
                    End If
                End Using
            Catch ex As Exception                
                If con.State = ConnectionState.Open Then
                    Trans.Rollback()
                    con.Dispose()
                End If
                Me.Session.Add("InsertError", ex.Message)
                Return False
            Finally
                If con.State = ConnectionState.Open Then
                    con.Dispose()
                End If
            End Try

        End Using
    End Function

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        If Not Me.Session("JDesc") Is Nothing Then
            If Me.Session("JDesc").ToString.Contains("HELPDESK") Or Me.Session("JDesc").ToString.Contains("LPTL") Then
                Me.MasterPageFile = "../LeftWOMainMasterPage.master"
            ElseIf Me.Session("JDesc").ToString.Contains("MMD") Then
                Me.MasterPageFile = "../LeftDEMainMasterPage.master"
            End If
        End If
    End Sub

    Protected Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad

        If Not Page.IsPostBack Then
            CheckLogin()
            CheckUser()
            If Me.Session("JDesc").ToString.Contains("HELPDESK") OrElse Me.Session("JDesc").ToString.Contains("MMD") Then
                rbBranch.Visible = True
                rbDivision.Visible = True
                rbDivision.Checked = True
            ElseIf Me.Session("JDesc").ToString.Contains("LPTL") Then
                rbBranch.Checked = True
                Me.lblBCCode.Text = "BC Code"
                Me.lblBCName.Text = "BC Name"
            End If
        End If
    End Sub

    Public Function IsIpValid(ByVal ipAddress As String) As Boolean
        Dim expr As String = "^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$"
        Dim reg As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex(expr)
        If (reg.IsMatch(ipAddress)) Then
            Dim parts() As String = ipAddress.Split(".")
            If Convert.ToInt32(parts(0)) = 0 Then
                Return False
            ElseIf Convert.ToInt32(parts(3)) = 0 Then
                Return False
            End If
            For i As Integer = 1 To 4
                If i > 255 Then
                    Return False
                End If
            Next
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub UpdateEntry()
        Dim Update As Boolean
        Dim sql As New StringBuilder()
        Using con As New MySqlConnection(Me.Session("strCon").ToString())
            Dim i As Integer = 0
            Dim Trans As MySqlTransaction = Nothing
            Try
                con.Open()
                Trans = con.BeginTransaction()
                '======================================================CPU====================================================================
                If ddlCPU.SelectedItem.Text <> Me.Session("CPUOthers").ToString Then
                    sql.Length = 0
                    sql.Append("SELECT comp_cpu_Class_ID FROM cmms_cpu_class WHERE cpu_description = @CPU; ")
                    Using cmd As New MySqlCommand(sql.ToString(), con, Trans)
                        cmd.Parameters.Add(New MySqlParameter("CPU", MySqlDbType.VarString, 50)).Value() = ddlCPU.SelectedItem.Text
                        cmd.CommandType = CommandType.Text
                        TCPU = cmd.ExecuteScalar()
                        Update = TCPU
                        If TCPU Is Nothing Then
                            lblErrorSaving.Text = "Error on saving CPU Type"
                            Exit Sub
                        End If
                    End Using
                Else
                    sql.Length = 0
                    sql.Append("INSERT INTO cmms.cmms_cpu_class( Comp_CPU_Class_ID, CPU_Description, Sys_Created, Sys_Modified,")
                    sql.Append("Sys_Creator, Sys_Modifier) ")
                    sql.Append("SELECT MAX(Comp_CPU_Class_ID) + 1 as x ,@NewCPU,NOW(),NOW(),@Creator,@Updater FROM cmms.cmms_cpu_class; ")
                    Using cmd As New MySqlCommand(sql.ToString(), con, Trans)
                        cmd.Parameters.Add(New MySqlParameter("NewCPU", MySqlDbType.VarString, 50)).Value() = Trim(Me.txtCPUOthers.Text.ToUpper())
                        cmd.Parameters.Add(New MySqlParameter("Creator", MySqlDbType.VarString, 15)).Value() = Me.Session("res_id")
                        cmd.Parameters.Add(New MySqlParameter("Updater", MySqlDbType.VarString, 15)).Value() = Me.Session("res_id")
                        cmd.CommandType = CommandType.Text
                        i = cmd.ExecuteNonQuery()
                        If i <= 0 Then
                            lblErrorSaving.Text = "Error on Inserting new CPU"
                            Exit Sub
                        Else
                            sql.Length = 0
                            sql.Append("SELECT comp_cpu_Class_ID FROM cmms_cpu_class WHERE cpu_description = @CPU; ")
                            Using cpuId As New MySqlCommand(sql.ToString(), con, Trans)
                                cpuId.Parameters.Add(New MySqlParameter("CPU", MySqlDbType.VarString, 50)).Value() = Trim(Me.txtCPUOthers.Text.ToUpper())
                                cpuId.CommandType = CommandType.Text
                                TCPU = cpuId.ExecuteScalar()
                                Update = TCPU
                            End Using
                        End If
                    End Using
                End If
                '======================================================Memory====================================================================
                If ddlMemoSize.SelectedItem.Text <> Me.Session("MemoryOthers").ToString Then
                    sql.Length = 0
                    sql.Append("SELECT Memory_Class_ID FROM cmms.cmms_memory_class WHERE Memory_Description = @Memory; ")
                    Using cmd As New MySqlCommand(sql.ToString(), con, Trans)
                        cmd.Parameters.Add(New MySqlParameter("Memory", MySqlDbType.VarString, 50)).Value() = ddlMemoSize.SelectedItem.Text
                        cmd.CommandType = CommandType.Text
                        MSize = cmd.ExecuteScalar()
                        Update = MSize
                        If MSize Is Nothing Then
                            lblErrorSaving.Text = "Error on Inserting Memory Size"
                            Exit Sub
                        End If
                    End Using
                Else
                    sql.Length = 0
                    sql.Append("INSERT INTO cmms.cmms_memory_class( Memory_Class_ID, Memory_Description, Sys_Created, Sys_Modified,")
                    sql.Append("Sys_Creator, Sys_Modifier) SELECT MAX(Memory_Class_ID) + 1 AS x ,")
                    sql.Append("@NewMemory,NOW(),NOW(),@Creator,@Modifier FROM cmms.cmms_memory_class")
                    Using cmd As New MySqlCommand(sql.ToString(), con, Trans)
                        cmd.Parameters.Add(New MySqlParameter("NewMemory", MySqlDbType.VarString, 50)).Value() = Trim(Me.txtMemoSizeOthers.Text.ToUpper())
                        cmd.Parameters.Add(New MySqlParameter("Creator", MySqlDbType.VarString, 15)).Value() = Me.Session("res_id")
                        cmd.Parameters.Add(New MySqlParameter("Modifier", MySqlDbType.VarString, 15)).Value() = Me.Session("res_id")
                        cmd.CommandType = CommandType.Text
                        i = cmd.ExecuteNonQuery()
                        If i <= 0 Then
                            lblErrorSaving.Text = "Error on Inserting new Memory Size"
                            Exit Sub
                        Else
                            sql.Length = 0
                            sql.Append("SELECT Memory_Class_ID FROM cmms.cmms_memory_class WHERE Memory_Description = @Memory; ")
                            Using Memorycmd As New MySqlCommand(sql.ToString(), con, Trans)
                                Memorycmd.Parameters.Add(New MySqlParameter("Memory", MySqlDbType.VarString, 50)).Value() = Trim(Me.txtMemoSizeOthers.Text.ToUpper())
                                Memorycmd.CommandType = CommandType.Text
                                MSize = Memorycmd.ExecuteScalar()
                                Update = MSize
                            End Using
                        End If
                    End Using
                End If
                '======================================================HDD====================================================================
                If ddlHardDisk.SelectedItem.Text <> Me.Session("HardDiskOthers").ToString Then
                    sql.Length = 0
                    sql.Append("SELECT HardDisk_Class_ID FROM cmms.cmms_harddisk_class WHERE HardDisk_Description = @HDD; ")
                    Using cmd As New MySqlCommand(sql.ToString(), con, Trans)
                        cmd.Parameters.Add(New MySqlParameter("HDD", MySqlDbType.VarString, 50)).Value() = ddlHardDisk.SelectedItem.Text
                        cmd.CommandType = CommandType.Text
                        HSize = cmd.ExecuteScalar()
                        Update = HSize
                        If Not Update Then
                            lblErrorSaving.Text = "Error on Saving Hard Disk"
                            Exit Sub
                        End If
                    End Using
                Else
                    sql.Length = 0
                    sql.Append("INSERT INTO cmms.cmms_harddisk_class( HardDisk_Class_ID, HardDisk_Description, Sys_Created, Sys_Modified,")
                    sql.Append("Sys_Creator, Sys_Modifier) SELECT MAX(HardDisk_Class_ID) + 1 AS x ,")
                    sql.Append("@NewHDD,NOW(),NOW(),@Creator,@Modifier FROM cmms.cmms_harddisk_class")
                    Using cmd As New MySqlCommand(sql.ToString(), con, Trans)
                        cmd.Parameters.Add(New MySqlParameter("NewHDD", MySqlDbType.VarString, 50)).Value() = Trim(Me.txtHardDiskOthers.Text.ToUpper())
                        cmd.Parameters.Add(New MySqlParameter("Creator", MySqlDbType.VarString, 15)).Value() = Me.Session("res_id")
                        cmd.Parameters.Add(New MySqlParameter("Modifier", MySqlDbType.VarString, 15)).Value() = Me.Session("res_id")
                        cmd.CommandType = CommandType.Text
                        i = cmd.ExecuteNonQuery()
                        If i <= 0 Then
                            lblErrorSaving.Text = "Error on Inserting new HDD"
                            Exit Sub
                        Else
                            sql.Length = 0
                            sql.Append("SELECT HardDisk_Class_ID FROM cmms.cmms_harddisk_class WHERE HardDisk_Description = @HDD; ")
                            Using HDDcmd As New MySqlCommand(sql.ToString(), con, Trans)
                                HDDcmd.Parameters.Add(New MySqlParameter("HDD", MySqlDbType.VarString, 50)).Value() = Trim(Me.txtHardDiskOthers.Text)
                                HDDcmd.CommandType = CommandType.Text
                                HSize = HDDcmd.ExecuteScalar()
                                Update = HSize
                            End Using
                        End If
                    End Using
                End If
                If Update Then
                    If DataSaveTransaction() = False Then
                        Trans.Rollback()
                        lblErrorSaving.Text = Me.Session("InsertError")
                    Else
                        Trans.Commit()
                        Me.lblErrorSaving.Text = String.Empty
                        lblErrorSaving.Text = "Update Successfully!"
                        disableFieldsCategory()
                        disableFieldsInforamtion()
                        btnUpdate.Visible = False
                        btnNew.Visible = True
                        Me.AssetInventory.Enabled = False
                        Me.cbActive.Checked = True
                    End If
                End If

            Catch ex As MySqlException
                If con.State = ConnectionState.Open Then
                    Trans.Rollback()
                End If
                lblErrorSaving.Text = ex.Message
            Catch ex As Exception
                If con.State = ConnectionState.Open Then
                    Trans.Rollback()
                End If
                lblErrorSaving.Text = ex.Message
            End Try
            con.Close()
        End Using

    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        lblErrorSaving.Text = String.Empty
        Dim CPUList As ListItem = Me.ddlCPU.Items.FindByValue(Trim(Me.txtCPUOthers.Text))
        Dim Memory As ListItem = Me.ddlCPU.Items.FindByValue(Trim(Me.txtMemoSizeOthers.Text))
        Dim HDD As ListItem = Me.ddlCPU.Items.FindByValue(Trim(Me.txtHardDiskOthers.Text))
        Dim S As String = ""
        If CheckBlankFields(S) = "False" Then
            Exit Sub
        End If
        If ddlCPU.Text = Me.Session("CPUOthers") AndAlso txtCPUOthers.Text = "" Then
            lblErrorSaving.Text = "Fill in CPU Others!"
            Exit Sub
        ElseIf ddlMemoSize.Text = Me.Session("MemoryOthers") AndAlso txtMemoSizeOthers.Text = "" Then
            lblErrorSaving.Text = "Fill in Memory Others!"
            Exit Sub
        ElseIf ddlHardDisk.Text = Me.Session("HardDiskOthers") AndAlso txtHardDiskOthers.Text = "" Then
            lblErrorSaving.Text = "Fill in Hard Disk Others!"
            Exit Sub
        ElseIf Not Me.ddlMemoSize.Items.FindByValue(Trim(Me.txtMemoSizeOthers.Text.ToUpper)) Is Nothing And ddlMemoSize.Text = Me.Session("MemoryOthers") Then
            lblErrorSaving.Text = Trim(Me.txtMemoSizeOthers.Text.ToUpper) & " Already Exist"
            Exit Sub
        ElseIf Not Me.ddlHardDisk.Items.FindByValue(Trim(Me.txtHardDiskOthers.Text.ToUpper)) Is Nothing And ddlHardDisk.Text = Me.Session("HardDiskOthers") Then
            lblErrorSaving.Text = Trim(Me.txtHardDiskOthers.Text.ToUpper) & " Already Exist"
            Exit Sub
        ElseIf Me.ddlCPU.Items.Contains(New ListItem(Trim(Me.txtCPUOthers.Text.ToUpper))) And Me.ddlCPU.Text = Me.Session("CPUOthers") Then
            lblErrorSaving.Text = Trim(Me.txtCPUOthers.Text.ToUpper) & " Already Exist"
            Exit Sub
        End If
        If Page.IsValid = True Then
            UpdateEntry()
        End If
    End Sub
    'Division Closed Buttons
    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Session("_txtdCode") = Nothing
        Me.Session("_txtdName") = Nothing
    End Sub

    Protected Sub btnClose4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose4.Click
        Me.Session("_txtEName") = Nothing
        Me.Session("_txtEID") = Nothing
        Me.Session("flag") = Nothing
    End Sub

    'Branch Closed Buttons
    Protected Sub btnClose2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose2.Click
        Me.Session("SBCode") = Nothing
        Me.Session("SBName") = Nothing
    End Sub

    Protected Sub btnClose3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose3.Click
        Me.Session("_txtBEName") = Nothing
        Me.Session("_txtBEID") = Nothing
        Me.Session("bflag") = Nothing
    End Sub

End Class