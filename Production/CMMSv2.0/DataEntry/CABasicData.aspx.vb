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

Imports System.Security.Permissions

Partial Class EntryData_CABasicData
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
    Dim conStr As String = ConfigurationManager.ConnectionStrings("CMMS").ConnectionString

    Protected Overrides Sub OnInit(ByVal e As EventArgs)
        MyBase.OnInit(e)        
        DIV2.DivisionSelected = New DivisionDelegate(AddressOf DIV2_Selected)
        DIV2.DivisionToFind = New SearchDelegate(AddressOf DIV2_Search)
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
        PInfo(txtIDName.Text)
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
        PInfo(txtIDName.Text)
    End Sub

    Public Sub UserID_Selected(ByVal UserID As String, ByVal Name As String)
        txtID.Text = UserID
        txtIDName.Text = Name.Replace("&#209;", "Ñ")
        ClearFieldsCategory()
        PInfo(Name)
        LoadComponentSize()
    End Sub

    Public Sub UserIDD_Selected(ByVal UserID As String, ByVal Name As String)
        txtID.Text = UserID
        txtIDName.Text = Name.Replace("&#209;", "Ñ")
        ClearFieldsCategory()
        PInfo(Name)
        LoadComponentSize()
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
            Me.Session("Click") = "BasicData"
            rbDivision.Checked = True
            cbActive.Checked = True
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

        Else
            txtDelDate.Text = Request.Form(txtDelDate.UniqueID)
        End If
        Me.Session.Add("Code", Me.txtCode.Text)
        lblCountWord.Text = (500 - txtAstDescription.Text.Length) & "/500"
        btnNew.Visible = False
        Disable(True)
        DisableTextboxCtrl()
        EnableTextboxCtrl()
        SelectCompType()
        RegisterDisableEnterKeyScript()
        DisEnter()
        If Me.Session("txtAsstInvNo ") <> Nothing Then
            If RemoveAttList(Me.Session("txtAsstInvNo")) = True Then
                rbAttach.Checked = True
            Else
                rbAttach.Checked = False
            End If
        End If
        Me.Session.Add("NewBasicAssetNo", txtAsstInvNo.Text)
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
    End Sub

    Private Sub CheckLogin()
        If Me.Session("fName") = "" Then
            Response.Redirect("~/login.aspx")
        End If
    End Sub

    Private Sub CheckUser()
        If Me.Session("JDesc") = "MMD-STAFF" Then
            Exit Sub
        Else
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
            'Me.UpdatePanel1.Update()
            'lblmsg.Text = "Select computer type!"
            'Me.MPE.Show()
            lblErrorSaving.Text = "Select computer type!"
        ElseIf txtPTPNo.Text = "" Then
            W = "False"
            lblErrorSaving.Text = "Fill in manual asset no.!"
        ElseIf txtAstDescription.Text = "" Then
            W = "False"
            lblErrorSaving.Text = "Fill in description!"
        ElseIf ddlCPU.Text = "0" Then
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
        ElseIf Me.ddlIPType.SelectedIndex <> 0 And Me.txtIP.Text <> Nothing And IsIpValid(Trim(Me.txtIP.Text)) = False Then
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

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim SqlQry As String = ""
        Dim attachments As String = ""
        Dim S As String = ""
        If CheckBlankFields(S) = "False" Then
            Exit Sub
        End If
        If Request.Form(txtDelDate.UniqueID) > Format(Date.Now, "yyyy-MM-dd") Then
            lblErrorSaving.Text = "Advance date is not allowed."
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
        End If

        If Page.IsValid = True Then
            SaveDataEntry()
        End If
    End Sub

    Protected Sub rbBranch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbBranch.CheckedChanged
        If rbBranch.Checked = True Then
            rbDivision.Checked = False
            rbAttach.Checked = False
            rbScan.Checked = False
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

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Response.Redirect("CABasicData.aspx")
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
            lblCountWord.Text = "500/500"
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
        txtCPUOthers.Visible = False
        txtMemoSizeOthers.Visible = False
        txtHardDiskOthers.Visible = False
    End Sub

    Protected Sub EnableTextboxCtrl()
        If ddlCPU.Text = Me.Session("CPUOthers") Then
            txtCPUOthers.Visible = True
        Else
            Me.txtCPUOthers.Text = String.Empty
            txtCPUOthers.Visible = False
        End If
        If ddlMemoSize.Text = Me.Session("MemoryOthers") Then
            txtMemoSizeOthers.Visible = True
        Else
            Me.txtMemoSizeOthers.Text = String.Empty
            txtMemoSizeOthers.Visible = False
        End If
        If ddlHardDisk.Text = Me.Session("HardDiskOthers") Then
            txtHardDiskOthers.Visible = True
        Else
            Me.txtHardDiskOthers.Text = String.Empty
            txtHardDiskOthers.Visible = False
        End If
    End Sub

    Protected Sub ddlCompType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCompType.SelectedIndexChanged
        Create_trxNo()
        'Dim ItemCode As String = Me.Session("ICODE")
        Dim trxNo As String = Me.Session("IncreTrxNo")
        Dim bcCode As String = txtCode.Text
        If txtCode.Text = "" Then
            txtAsstInvNo.Text = "Pls. Select Code"
        Else
            If ddlCompType.Text = "DESKTOP" Then
                txtAsstInvNo.Text = "500" & "-" & bcCode & "-" & dateCreated & "-" & trxNo
            ElseIf ddlCompType.Text = "NOTEBOOK" Then
                txtAsstInvNo.Text = "501" & "-" & bcCode & "-" & dateCreated & "-" & trxNo
            ElseIf ddlCompType.Text = "SERVER" Then
                txtAsstInvNo.Text = "502" & "-" & bcCode & "-" & dateCreated & "-" & trxNo
            Else
                txtAsstInvNo.Text = ""
            End If
        End If
        lblErrorSaving.Text = ""
    End Sub

    Private Sub Create_trxNo()
        Dim numRegion As Integer
        Dim _Increment As String = ""
        Dim ds As New DataSet

        numRegion = 1

        _Increment = Auto_trxNo(numRegion)
        If _Increment = "error" Then
            Exit Sub
        End If

        Me.Session.Add("IncreTrxNo", _Increment)
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
                mySql = "select Asset_Inv_No from cmms_entry_masterheader where Asset_Inv_No like '%" & dateCreated & "%'" _
                      & "and bc_code = '" & txtCode.Text & "' order by sys_created desc limit 1;"
            ElseIf numRegion = 2 Then
                mySql = "select Asset_Inv_No from cmms_entry_masterheader where Asset_Inv_No like '%" & dateCreated & "%'" _
                      & "and bc_code = '" & txtCode.Text & "' order by sys_created desc limit 1;"
            End If
            If mySql <> "" Then
                ds = Execute_DataSet(mySql)
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
            Return "error"
        End Try

    End Function

    Private Sub SelectCompType()
        Dim ds As New DataSet
        Dim ICode As String

        Try
            Dim mySql As String = ""
            mySql = "select item_code from cmms_devices_list where item_class = 'BASIC';"
            ds = Execute_DataSet(mySql)

            ICode = ds.Tables(0).Rows(0).Item(0).ToString
            Me.Session.Add("ICODE", ICode)

        Catch ex As Exception
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
        End Try
    End Sub

    Private Sub SelectCPU()
        Dim ds As New DataSet

        Try
            Dim mySql As String
            mySql = "select UPPER(cpu_description) from cmms_cpu_class ORDER BY cpu_description DESC;"
            ds = Execute_DataSet(mySql)

            Dim RText As String = ""
            Dim Others As String = "OTHERS"
            Dim i As Integer
            Dim r As String
            ddlCPU.Items.Clear()
            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    ddlCPU.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString))
                Next
                r = i + 1
                ddlCPU.Items.Add(Others)
                'ddlCPU.Items.Insert(i, New ListItem(Others, r))
                'ddlCPU.Items.Insert(i, Others)
                Me.Session.Add("CPUOthers", Others)
            Else
                RText = "Nothing Available"
            End If
            ddlCPU.Items.Insert(0, New ListItem(RText, "0"))

        Catch ex As Exception
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
        End Try
    End Sub

    Private Sub SelectMemorySize()
        Dim ds As DataSet

        Try
            Dim mySql As String
            mySql = "select UPPER(memory_description) from cmms_memory_class ORDER BY memory_description DESC;"
            ds = Execute_DataSet(mySql)

            Dim RText As String = ""
            Dim Others As String = "OTHERS"
            Dim i As Integer
            Dim r As String
            ddlMemoSize.Items.Clear()
            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    ddlMemoSize.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString))
                Next
                r = i + 1
                'ddlMemoSize.Items.Insert(i, New ListItem(Others, r))
                ddlMemoSize.Items.Add(Others)
                Me.Session.Add("MemoryOthers", Others)
            Else
                RText = "Nothing Available"
            End If
            ddlMemoSize.Items.Insert(0, New ListItem(RText, "0"))

        Catch ex As Exception
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
        End Try
    End Sub

    Private Sub SelectHardDisk()
        Dim ds As DataSet

        Try
            Dim mySql As String
            mySql = "select UPPER(harddisk_description) from cmms_harddisk_class ORDER BY harddisk_description DESC;"
            ds = Execute_DataSet(mySql)

            Dim RText As String = ""
            Dim Others As String = "OTHERS"
            Dim i As Integer
            Dim r As String
            ddlHardDisk.Items.Clear()
            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    ddlHardDisk.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString))
                Next
                r = i + 1
                'ddlHardDisk.Items.Insert(i, New ListItem(Others, r))
                ddlHardDisk.Items.Add(Others)
                Me.Session.Add("HardDiskOthers", Others)
            Else
                RText = "Nothing Available"
            End If
            ddlHardDisk.Items.Insert(0, New ListItem(RText, "0"))

        Catch ex As Exception
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
        End Try
    End Sub

    Private Sub SelectNewCodeCPU()
        Dim NwCodeCPU As String
        Dim OldCode As String
        Dim _NwCode As Integer
        Dim myNewCode As String = ""
        Dim ds As DataSet

        If ddlCPU.Text = Me.Session("CPUOthers") Then
            myNewCode = "select comp_cpu_class_id from cmms_cpu_class where comp_cpu_class_id = '" & ddlCPU.Text & "';"
        End If
        ds = Execute_DataSet(myNewCode)
        If Not ds Is Nothing AndAlso ds.Tables(0).Rows(0).Item(0).ToString <> "" Then
            OldCode = ds.Tables(0).Rows(0).Item(0).ToString
            Me.Session.Add("ExistCode", OldCode)
        Else
            NwCodeCPU = NewCodeOthers1(_NwCode)
            Me.Session.Add("NewCodeCPU", NwCodeCPU)
        End If
    End Sub

    Private Sub SelectNewCodeMemory()
        Dim NwCodeMemory As String
        Dim OldCode As String
        Dim _NwCode As Integer
        Dim myNewCode As String = ""
        Dim ds As DataSet

        If ddlMemoSize.Text = Me.Session("MemoryOthers") Then
            myNewCode = "select Memory_Class_ID from cmms_memory_class where Memory_Class_ID = '" & ddlMemoSize.Text & "';"
        End If
        ds = Execute_DataSet(myNewCode)
        If Not ds Is Nothing AndAlso ds.Tables(0).Rows(0).Item(0).ToString <> "" Then
            OldCode = ds.Tables(0).Rows(0).Item(0).ToString
            Me.Session.Add("ExistCode", OldCode)
        Else
            NwCodeMemory = NewCodeOthers2(_NwCode)
            Me.Session.Add("NewCodeMemory", NwCodeMemory)
        End If
    End Sub

    Private Sub SelectNewCodeHardDisk()
        Dim NwCodeHardDisk As String
        Dim OldCode As String
        Dim _NwCode As Integer
        Dim myNewCode As String = ""
        Dim ds As DataSet

        If ddlHardDisk.Text = Me.Session("HardDiskOthers") Then
            myNewCode = "select HardDisk_Class_ID from cmms_harddisk_class where HardDisk_Class_ID = '" & ddlHardDisk.Text & "';"
        End If
        ds = Execute_DataSet(myNewCode)
        If Not ds Is Nothing AndAlso ds.Tables(0).Rows(0).Item(0).ToString <> "" Then
            OldCode = ds.Tables(0).Rows(0).Item(0).ToString
            Me.Session.Add("ExistCode", OldCode)
        Else
            NwCodeHardDisk = NewCodeOthers3(_NwCode)
            Me.Session.Add("NewCodeHardDisk", NwCodeHardDisk)
        End If
    End Sub

    Private Function NewCodeOthers1(ByVal _NwCode As String) As String
        Dim ds As DataSet
        Dim myCode As String = ""

        Try
            If ddlCPU.Text = Me.Session("CPUOthers") Then
                myCode = "select max(comp_cpu_class_id) + 1 as autnum from cmms_cpu_class;"
            End If
            ds = Execute_DataSet(myCode)
            If Not ds Is Nothing AndAlso ds.Tables(0).Rows(0).Item(0).ToString <> "" Then
                Return ds.Tables(0).Rows(0).Item(0).ToString
            Else
                Return "error"
            End If
        Catch ex As Exception
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
            Return "error"
        End Try

    End Function

    Private Function NewCodeOthers2(ByVal _NwCode As String) As String
        Dim ds As DataSet
        Dim myCode As String = ""

        Try
            If ddlMemoSize.Text = Me.Session("MemoryOthers") Then
                myCode = "select max(Memory_Class_ID) + 1 as autnum from cmms_memory_class;"
            End If
            ds = Execute_DataSet(myCode)
            If Not ds Is Nothing AndAlso ds.Tables(0).Rows(0).Item(0).ToString <> "" Then
                Return ds.Tables(0).Rows(0).Item(0).ToString
            Else
                Return "error"
            End If
        Catch ex As Exception
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
            Return "error"
        End Try

    End Function

    Private Function NewCodeOthers3(ByVal _NwCode As String) As String
        Dim ds As DataSet
        Dim myCode As String = ""

        Try
            If ddlHardDisk.Text = Me.Session("HardDiskOthers") Then
                myCode = "select max(HardDisk_Class_ID) + 1 as autnum from cmms_harddisk_class;"
            End If
            ds = Execute_DataSet(myCode)
            If Not ds Is Nothing AndAlso ds.Tables(0).Rows(0).Item(0).ToString <> "" Then
                Return ds.Tables(0).Rows(0).Item(0).ToString
            Else
                Return "error"
            End If
        Catch ex As Exception
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
            Return "error"
        End Try

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

    Protected Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        If Not Page.IsPostBack Then
            CheckLogin()
            CheckUser()            
            'SelectCPU()
            'SelectMemorySize()
            'SelectHardDisk()
            'LoadComponentSize()
        End If
        Me.Session("isDivision") = Me.rbDivision.Checked
        'If Me.rbDivision.Checked Then
        '    Me.Session("isDivision") = True
        'Else
        '    Me.Session("isDivision") = False
        'End If
    End Sub

    Private Sub LoadComponentSize()
        Dim Flag As Boolean = True
        Using con As New MySqlConnection(Me.Session("strCon").ToString)
            Dim sql As New StringBuilder
            sql.Append("SELECT UPPER(cpu_description) AS cpu FROM cmms_cpu_class ORDER BY cpu_description ASC; ")
            sql.Append("SELECT UPPER(memory_description) AS memory FROM cmms_memory_class ORDER BY memory_description ASC; ")
            sql.Append("SELECT UPPER(harddisk_description) AS hdd FROM cmms_harddisk_class ORDER BY harddisk_description ASC;")
            Using cmd As New MySqlCommand(sql.ToString, con)
                cmd.CommandType = CommandType.Text
                Try
                    con.Open()
                    Using DataRead As MySqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        If DataRead.HasRows Then                            
                            ddlCPU.Items.Clear()
                            Dim cpu As Int16 = DataRead.GetOrdinal("cpu")
                            While DataRead.Read
                                ddlCPU.Items.Add(DataRead.GetString(cpu))
                            End While
                            ddlCPU.Items.Add("OTHERS")
                            Me.Session.Add("CPUOthers", "OTHERS")
                            ddlCPU.Items.Insert(0, New ListItem("", "0"))
                        End If
                        DataRead.NextResult()
                        If DataRead.HasRows Then                            
                            ddlMemoSize.Items.Clear()
                            Dim memory As Int16 = DataRead.GetOrdinal("memory")
                            While DataRead.Read
                                ddlMemoSize.Items.Add(DataRead.GetString(memory))
                            End While
                            ddlMemoSize.Items.Add("OTHERS")
                            Me.Session.Add("MemoryOthers", "OTHERS")
                            ddlMemoSize.Items.Insert(0, New ListItem("", "0"))
                        End If
                        DataRead.NextResult()
                        If DataRead.HasRows Then                            
                            ddlHardDisk.Items.Clear()
                            Dim hdd As Int16 = DataRead.GetOrdinal("hdd")
                            While DataRead.Read
                                ddlHardDisk.Items.Add(DataRead.GetString(hdd))
                            End While
                            ddlHardDisk.Items.Add("OTHERS")
                            Me.Session.Add("HardDiskOthers", "OTHERS")
                            ddlHardDisk.Items.Insert(0, New ListItem("", "0"))
                        End If
                        DataRead.Close()
                    End Using
                Catch ex As Exception                    
                    Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
                End Try
            End Using
        End Using
    End Sub

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
        Me.Session.Add("txtIP", Me.txtIP.Text)
        Me.Session.Add("IPType", Me.ddlIPType.SelectedIndex.ToString)
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
        txtIP.Text = IIf(Me.Session("txtIP") <> "", Me.Session("txtIP"), "")
        Me.ddlIPType.SelectedIndex = IIf(Me.Session("IPType") <> "", Me.Session("IPType"), "")
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

    '09-07-13

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

    Private Sub SaveDataEntry()        
        Dim a As String = Me.Session("CPUOthers").ToString()
        Code = txtCode.Text
        Name = txtCodeName.Text
        IdAssign = txtID.Text
        NameAssign = txtIDName.Text
        'RegCode = "VISMIN"
        RegCode = Me.Session("ZCode") ' NEW ELY CODE 2-22-2012
        If ddlCompType.Text = "DESKTOP" Then
            ItmCode = "500"
        ElseIf ddlCompType.Text = "NOTEBOOK" Then
            ItmCode = "501"
        Else
            ItmCode = "502"
        End If
        AssetInv = txtAsstInvNo.Text
        PTPNum = txtPTPNo.Text
        AssetDesc = txtAstDescription.Text
        PONum = txtPONo.Text
        DelDate = Request.Form(txtDelDate.UniqueID)
        If txtSerialNo.Text = "" Then
            SerNum = "-"
        Else
            SerNum = txtSerialNo.Text
        End If
        SysCreator = Me.Session("res_id")
        SysModifier = Me.Session("res_id")

        If cbActive.Checked = True Then
            Stat = "A"
        Else
            Stat = "I"
        End If
        Dim sql As New StringBuilder()
        Dim Update As Boolean
        Using con As New MySqlConnection(conStr)
            Dim i As Integer = 0
            Dim Trans As MySqlTransaction = Nothing
            Try
                con.Open()
                Trans = con.BeginTransaction()
                If ddlCPU.SelectedItem.Text <> Me.Session("CPUOthers").ToString Then
                    sql.Length = 0
                    sql.Append("SELECT comp_cpu_Class_ID FROM cmms_cpu_class WHERE cpu_description = @CPU; ")
                    Using cmd As New MySqlCommand(sql.ToString(), con, Trans)
                        cmd.Parameters.Add("CPU", MySqlDbType.VarString, 50).Value = ddlCPU.SelectedItem.Text
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
                    sql.Append("SELECT CPU_Description FROM cmms.cmms_cpu_class WHERE CPU_Description = @NewCPU; ")
                    Using cmd As New MySqlCommand(sql.ToString(), con, Trans)
                        cmd.Parameters.Add("NewCPU", MySqlDbType.VarString, 50).Value = Trim(Me.txtCPUOthers.Text.ToUpper)
                        cmd.CommandType = CommandType.Text
                        Using DataRead As MySqlDataReader = cmd.ExecuteReader()
                            If DataRead.HasRows() Then
                                lblErrorSaving.Text = Trim(Me.txtCPUOthers.Text.ToUpper) & " Already exist"
                                DataRead.Close()
                                Exit Sub
                            Else
                                DataRead.Close()
                                sql.Length = 0
                                sql.Append("INSERT INTO cmms.cmms_cpu_class( Comp_CPU_Class_ID, CPU_Description, Sys_Created, Sys_Modified,")
                                sql.Append("Sys_Creator, Sys_Modifier) ")
                                sql.Append("SELECT MAX(Comp_CPU_Class_ID) + 1 as x ,@NewCPU,NOW(),NOW(),@Creator,@Updater FROM cmms.cmms_cpu_class; ")
                                Using addCmd As New MySqlCommand(sql.ToString(), con, Trans)
                                    addCmd.Parameters.Add("NewCPU", MySqlDbType.VarString, 50).Value = Trim(Me.txtCPUOthers.Text.ToUpper)
                                    addCmd.Parameters.Add("Creator", MySqlDbType.VarString, 15).Value = Me.Session("res_id")
                                    addCmd.Parameters.Add("Updater", MySqlDbType.VarString, 15).Value = Me.Session("res_id")
                                    addCmd.CommandType = CommandType.Text
                                    i = addCmd.ExecuteNonQuery()
                                    If i <= 0 Then
                                        lblErrorSaving.Text = "Error on Inserting new CPU"
                                        Exit Sub
                                    Else
                                        sql.Length = 0
                                        sql.Append("SELECT comp_cpu_Class_ID FROM cmms_cpu_class WHERE cpu_description = @CPU; ")
                                        Using cpuId As New MySqlCommand(sql.ToString(), con, Trans)
                                            cpuId.Parameters.Add("CPU", MySqlDbType.VarString, 50).Value = Trim(Me.txtCPUOthers.Text.ToUpper)
                                            cpuId.CommandType = CommandType.Text
                                            TCPU = cpuId.ExecuteScalar()
                                            Update = TCPU
                                        End Using
                                    End If
                                End Using
                            End If
                        End Using
                    End Using
                End If
                '======================================================Memory====================================================================
                If ddlMemoSize.SelectedItem.Text <> Me.Session("MemoryOthers").ToString Then
                    sql.Length = 0
                    sql.Append("SELECT Memory_Class_ID FROM cmms.cmms_memory_class WHERE Memory_Description = @Memory; ")
                    Using cmd As New MySqlCommand(sql.ToString(), con, Trans)
                        cmd.Parameters.Add("Memory", MySqlDbType.VarString, 50).Value = ddlMemoSize.SelectedItem.Text
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
                    sql.Append("SELECT Memory_Description FROM cmms.cmms_memory_class WHERE Memory_Description = @NewMemory; ")
                    Using cmd As New MySqlCommand(sql.ToString(), con, Trans)
                        cmd.Parameters.Add("NewMemory", MySqlDbType.VarString, 50).Value = Trim(Me.txtMemoSizeOthers.Text.ToUpper)
                        cmd.CommandType = CommandType.Text
                        Using DataRead As MySqlDataReader = cmd.ExecuteReader()
                            If DataRead.HasRows() Then
                                lblErrorSaving.Text = Trim(Me.txtMemoSizeOthers.Text.ToUpper) & " Already exist"
                                DataRead.Close()
                                Exit Sub
                            Else
                                DataRead.Close()
                                i = 0
                                sql.Length = 0
                                sql.Append("INSERT INTO cmms.cmms_memory_class( Memory_Class_ID, Memory_Description, Sys_Created, Sys_Modified,")
                                sql.Append("Sys_Creator, Sys_Modifier) SELECT MAX(Memory_Class_ID) + 1 AS x ,")
                                sql.Append("@NewMemory,NOW(),NOW(),@Creator,@Modifier FROM cmms.cmms_memory_class")
                                Using addCmd As New MySqlCommand(sql.ToString(), con, Trans)
                                    addCmd.Parameters.Add("NewMemory", MySqlDbType.VarString, 50).Value = Trim(Me.txtMemoSizeOthers.Text.ToUpper)
                                    addCmd.Parameters.Add("Creator", MySqlDbType.VarString, 15).Value = Me.Session("res_id")
                                    addCmd.Parameters.Add("Modifier", MySqlDbType.VarString, 15).Value = Me.Session("res_id")
                                    addCmd.CommandType = CommandType.Text
                                    i = addCmd.ExecuteNonQuery()
                                    If i <= 0 Then
                                        lblErrorSaving.Text = "Error on Inserting new Memory"
                                        Exit Sub
                                    Else
                                        sql.Length = 0
                                        sql.Append("SELECT Memory_Class_ID FROM cmms.cmms_memory_class WHERE Memory_Description = @Memory; ")
                                        Using Memorycmd As New MySqlCommand(sql.ToString(), con, Trans)
                                            Memorycmd.Parameters.Add("Memory", MySqlDbType.VarString, 50).Value = Trim(Me.txtMemoSizeOthers.Text.ToUpper())
                                            Memorycmd.CommandType = CommandType.Text
                                            MSize = Memorycmd.ExecuteScalar()
                                            Update = MSize
                                        End Using
                                    End If
                                End Using
                            End If
                        End Using
                    End Using
                End If
                '======================================================HDD====================================================================
                If ddlHardDisk.SelectedItem.Text <> Me.Session("HardDiskOthers").ToString Then
                    sql.Length = 0
                    sql.Append("SELECT HardDisk_Class_ID FROM cmms.cmms_harddisk_class WHERE HardDisk_Description = @HDD; ")
                    Using cmd As New MySqlCommand(sql.ToString(), con, Trans)
                        cmd.Parameters.Add("HDD", MySqlDbType.VarString, 50).Value = ddlHardDisk.SelectedItem.Text
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
                    sql.Append("SELECT HardDisk_Description FROM cmms.cmms_harddisk_class WHERE HardDisk_Description = @HDD; ")
                    Using cmd As New MySqlCommand(sql.ToString(), con, Trans)
                        cmd.Parameters.Add("HDD", MySqlDbType.VarString, 50).Value = Trim(Me.txtHardDiskOthers.Text.ToUpper)
                        cmd.CommandType = CommandType.Text
                        Using DataRead As MySqlDataReader = cmd.ExecuteReader()
                            If DataRead.HasRows() Then
                                lblErrorSaving.Text = Trim(Me.txtHardDiskOthers.Text.ToUpper) & " Already exist"
                                DataRead.Close()
                                Exit Sub
                            Else
                                DataRead.Close()
                                i = 0
                                sql.Length = 0
                                sql.Append("INSERT INTO cmms.cmms_harddisk_class( HardDisk_Class_ID, HardDisk_Description, Sys_Created, Sys_Modified,")
                                sql.Append("Sys_Creator, Sys_Modifier) SELECT MAX(HardDisk_Class_ID) + 1 AS x ,")
                                sql.Append("@NewHDD,NOW(),NOW(),@Creator,@Modifier FROM cmms.cmms_harddisk_class")
                                Using addCmd As New MySqlCommand(sql.ToString(), con, Trans)
                                    addCmd.Parameters.Add("NewHDD", MySqlDbType.VarString, 50).Value = Trim(Me.txtHardDiskOthers.Text.ToUpper)
                                    addCmd.Parameters.Add("Creator", MySqlDbType.VarString, 15).Value = Me.Session("res_id")
                                    addCmd.Parameters.Add("Modifier", MySqlDbType.VarString, 15).Value = Me.Session("res_id")
                                    addCmd.CommandType = CommandType.Text
                                    i = addCmd.ExecuteNonQuery()
                                    If i <= 0 Then
                                        lblErrorSaving.Text = "Error on Inserting new HDD"
                                        Exit Sub
                                    Else
                                        sql.Length = 0
                                        sql.Append("SELECT HardDisk_Class_ID FROM cmms.cmms_harddisk_class WHERE HardDisk_Description = @HDD; ")
                                        Using HDDcmd As New MySqlCommand(sql.ToString(), con, Trans)
                                            HDDcmd.Parameters.Add("HDD", MySqlDbType.VarString, 50).Value = Me.txtHardDiskOthers.Text.ToUpper
                                            HDDcmd.CommandType = CommandType.Text
                                            HSize = HDDcmd.ExecuteScalar()
                                            Update = HSize
                                        End Using
                                    End If
                                End Using
                            End If
                        End Using
                    End Using
                End If
                If Update Then
                    If DataSaveTransaction() = False Then
                        lblErrorSaving.Text = Me.Session("InsertError")
                        Trans.Rollback()
                    Else
                        Trans.Commit()
                        lblErrorSaving.Text = "Save Successfully!"
                        disableFieldsCategory()
                        disableFieldsInforamtion()
                        btnSave.Visible = False
                        btnNew.Visible = True
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

    Private Function DataSaveTransaction() As Boolean
        Using con As New MySqlConnection(conStr)
            Dim sql As New StringBuilder()
            Dim Tran As MySqlTransaction = Nothing
            Dim i As Integer = 0
            sql.Append("INSERT INTO cmms.cmms_entry_masterheader(")
            sql.Append("    Asset_Inv_No, Bc_Code, Bc_Name,  Res_ID_Assigned, Emp_Name_Assigned, Zone_Code, ")
            sql.Append("    PTP_No, Item_Code, Asset_Desc, PO_Number, Delivery_Date, Serial_No, Status, ")
            sql.Append("    Sys_Created, Sys_Modified, Sys_Creator, Sys_Modifier ) ")
            sql.Append("VALUES(@AssetNo, @Bcode, @Bname, @resID, @EmpName, @Zone, @PtpNo, @Item, @AssetDesc, ")
            sql.Append("    @PONo, @DelDate, @Serial, @Stats, NOW(), NOW(), @Creator, @Creator); ")

            sql.Append("INSERT INTO cmms.cmms_entry_details (")
            sql.Append("    Asset_Inv_No, Comp_CPU_Class_ID, Memory_Class_ID, HardDisk_Class_ID, ")
            sql.Append("    Sys_Created, Sys_Modified, Sys_Creator, Sys_Modifier, Comp_IP, Comp_IP_Type) ")
            sql.Append("VALUES(@AssetNo, @CPU, @Memory, @HDD, NOW(), NOW(), @Creator, @Creator, @IP, @IPType); ")

            sql.Append("INSERT INTO cmms.cmms_entry_history(")
            sql.Append("    Asset_Inv_No, Bc_Code, Bc_Name, Res_ID_Assigned, Emp_Name_Assigned,")
            sql.Append("    Zone_Code, Sys_Created, Sys_Modified, Sys_Creator, Sys_Modifier ) ")
            sql.Append("VALUES (@AssetNo, @Bcode, @Bname, @resID, @EmpName, @Zone, NOW(), NOW(), @Creator, @Creator);")

            Using cmd As New MySqlCommand(sql.ToString(), con, Tran)
                cmd.Parameters.Add("AssetNo", MySqlDbType.VarChar, 20).Value = AssetInv.Trim
                cmd.Parameters.Add("Bcode", MySqlDbType.VarChar, 3).Value = Code.Trim
                cmd.Parameters.Add("Bname", MySqlDbType.VarChar, 50).Value = Name.Trim
                cmd.Parameters.Add("resID", MySqlDbType.UInt32, 4).Value = IdAssign.Trim
                cmd.Parameters.Add("EmpName", MySqlDbType.VarChar, 50).Value = NameAssign.Trim
                cmd.Parameters.Add("Zone", MySqlDbType.VarChar, 6).Value = RegCode
                cmd.Parameters.Add("PtpNo", MySqlDbType.VarChar, 10).Value = PTPNum
                cmd.Parameters.Add("Item", MySqlDbType.UInt32, 3).Value = ItmCode
                cmd.Parameters.Add("AssetDesc", MySqlDbType.VarChar, 150).Value = EscapeApostrophe(AssetDesc.Trim)
                cmd.Parameters.Add("PONo", MySqlDbType.VarChar, 10).Value = PONum.Trim
                cmd.Parameters.Add("DelDate", MySqlDbType.DateTime).Value = Convert.ToDateTime(DelDate)
                cmd.Parameters.Add("Serial", MySqlDbType.VarChar, 30).Value = EscapeApostrophe(SerNum)
                cmd.Parameters.Add("Stats", MySqlDbType.VarChar, 1).Value = Stat
                cmd.Parameters.Add("Creator", MySqlDbType.Int32, 4).Value = SysCreator

                cmd.Parameters.Add("CPU", MySqlDbType.UInt32, 6).Value = Me.TCPU
                cmd.Parameters.Add("Memory", MySqlDbType.UInt32, 6).Value = Me.MSize
                cmd.Parameters.Add("HDD", MySqlDbType.UInt32, 6).Value = Me.HSize
                cmd.Parameters.Add("IP", MySqlDbType.VarChar, 15).Value = Me.txtIP.Text
                cmd.Parameters.Add("IPType", MySqlDbType.VarChar, 7).Value = Me.ddlIPType.SelectedItem.Text
                Try
                    con.Open()
                    Tran = con.BeginTransaction()
                    i = cmd.ExecuteNonQuery()
                    If i <= 0 Then
                        Tran.Rollback()
                        Return False
                    Else
                        Tran.Commit()
                        con.Close()
                        Return True
                    End If
                Catch ex As MySqlException
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                    Me.Session.Add("InsertError", ex.Message)
                Catch ex As Exception
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                    Me.Session.Add("InsertError", ex.Message)
                End Try
            End Using
        End Using
    End Function

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
        'If rbDivision.Checked = True Then            
        '    If Me.txtCode.Text <> Nothing And Me.txtID.Text = Nothing Then
        '        If Me.txtCode.Text <> Nothing And Me.Session("flag") = Nothing Then
        '            MPEDivison2.Show()
        '        Else
        '            MPEUsersIDD.Show()
        '        End If
        '    Else
        '        If Me.Session("flag") <> Nothing Then
        '            MPEUsersIDD.Show()
        '        Else
        '            MPEDivison2.Show()
        '        End If
        '    End If
        'Else
        '    If Me.txtCode.Text <> Nothing And Me.txtID.Text = Nothing Then
        '        If Me.txtCode.Text <> Nothing And Me.Session("bflag") = Nothing Then
        '            MPEBranch.Show()
        '        Else
        '            MPEUsersID.Show()
        '        End If
        '    Else
        '        If Me.Session("bflag") <> Nothing Then
        '            MPEUsersID.Show()
        '        Else
        '            MPEBranch.Show()
        '        End If
        '    End If
        'End If
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
        'If Me.txtCode.Text = Nothing Then
        '    Exit Sub
        'End If
        'If rbDivision.Checked = True Then
        '    If Me.txtID.Text <> Nothing And Me.txtCode.Text = Nothing Then
        '        If Me.txtCode.Text = Nothing And Me.Session("flage") <> Nothing Then
        '            MPEUsersIDD.Show()
        '        Else
        '            MPEDivison2.Show()
        '        End If
        '    Else
        '        MPEUsersIDD.Show()
        '    End If
        'Else
        '    If Me.txtID.Text <> Nothing And Me.txtCode.Text = Nothing Then
        '        If Me.txtCode.Text = Nothing And Me.Session("bflage") <> Nothing Then
        '            MPEUsersID.Show()
        '        Else
        '            MPEBranch.Show()
        '        End If
        '    Else
        '        MPEUsersID.Show()
        '    End If
        'End If
    End Sub

End Class

