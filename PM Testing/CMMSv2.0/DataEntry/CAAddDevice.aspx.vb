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
Partial Class EntryData_CAAddDevice
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
    Dim ADevice As String = ""
    Dim DevDesc As String
    Dim SysCreator As String
    Dim SysModifier As String

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
        txtACode.Text = Code
        txtACodeName.Text = Name.Replace("&#241;", "ñ")
        txtAID.Text = ""
        txtAIDName.Text = ""
        ClearFieldsCategory()
        PInfo(txtAIDName.Text)
    End Sub

    Public Sub Branch_Selected(ByVal Code As String, ByVal Name As String)
        txtACode.Text = Code
        txtACodeName.Text = Name.Replace("&#241;", "ñ")
        txtAID.Text = ""
        txtAIDName.Text = ""
        ClearFieldsCategory()
        PInfo(txtAIDName.Text)
    End Sub

    Public Sub UserID_Selected(ByVal UserID As String, ByVal Name As String)
        txtAID.Text = UserID
        txtAIDName.Text = Name.Replace("&#241;", "ñ")
        PInfo(Name)
    End Sub

    Public Sub UserIDD_Selected(ByVal UserID As String, ByVal Name As String)
        txtAID.Text = UserID
        txtAIDName.Text = Name.Replace("&#241;", "ñ")
        PInfo(Name)
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
        If CheckAttachB() = True Then
            Dim AN As String = Me.Session("txtAsstInvNo")
            If AN = Me.Session("NewBasicAssetNo") Then
                Dim DelSql As String
                DelSql = "delete from cmms_entry_attachfiles where Asset_Inv_No ='" & AN & "';"
                Execute_Delete(DelSql)
            End If
        End If
        If Not Page.IsPostBack Then
            Me.Session("Click") = "AddDevices"
            rbADivision.Checked = True
            cbAActive.Checked = True
            cbAActive.Enabled = False
            txtAAsstInvNo.ReadOnly = True
            If Me.Session("AAttachTrue") = True Then
                TemporaryRevInfo()
                BindAttachment(Me.Session("txtAAsstInvNo"))
                If attList.Items.Count <> 0 Then
                    rbAAttach.Checked = True
                Else
                    rbAAttach.Checked = False
                End If
            Else
                If CheckAttach() = True Then
                    Dim AN As String = Me.Session("txtAAsstInvNo")
                    If AN = txtAAsstInvNo.Text Then
                        Dim DelSql As String
                        DelSql = "delete from cmms_entry_attachfiles where Asset_Inv_No ='" & AN & "';"
                        Execute_Delete(DelSql)
                    End If
                End If
            End If
            PInfo(txtAIDName.Text)
        Else
            txtADelDate.Text = Request.Form(txtADelDate.UniqueID)
        End If
        Me.Session.Add("Code", txtACode.Text)
        lblCountWord.Text = "" & (500 - txtAAstDescription.Text.Length) & "/500"
        btnANew.Visible = False
        Disable(True)
        DisableTextboxCtrl()
        EnableTextboxCtrl()
        If RemoveAttList(Me.Session("txtAAsstInvNo")) = True Then
            rbAAttach.Checked = True
        Else
            rbAAttach.Checked = False
        End If
        Me.Session.Add("NewAddAssetNo", txtAAsstInvNo.Text)
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
            PanelAInformation.Enabled = False
        Else
            PanelAInformation.Enabled = True
        End If
    End Sub

    Protected Sub ClearFieldsCategory()
        For Each c As Control In PanelAInformation.Controls
            If TypeOf c Is TextBox Then
                CType(c, TextBox).Text = ""
            End If
            If TypeOf c Is DropDownList Then
                CType(c, DropDownList).ClearSelection()
            End If
        Next
    End Sub

    Private Function CheckBlankFields(ByVal W As String) As Boolean
        If ddlDeviceName.Text = "0" Then
            W = "False"
            lblErrorSaving.Text = "Select device name!"
        ElseIf txtAPTPNo.Text = "" Then
            W = "False"
            lblErrorSaving.Text = "Fill in manual asset no.!"
        ElseIf txtAAstDescription.Text = "" Then
            W = "False"
            lblErrorSaving.Text = "Fill in description!"
        ElseIf txtAPONo.Text = "" Then
            W = "False"
            lblErrorSaving.Text = "Fill in P.O. no.!"
        ElseIf Request.Form(txtADelDate.UniqueID) = "" Then
            W = "False"
            lblErrorSaving.Text = "Fill in delivery date!"
        Else
            W = "True"
        End If
        Return W
    End Function

    Private Sub Disable(ByVal R As Boolean)
        txtACode.Enabled = R
        txtACodeName.Enabled = R
        txtAID.Enabled = R
        txtAIDName.Enabled = R

        If R = True Then
            txtACode.Attributes("class") = "ReadOnlyBackColor"
            txtACodeName.Attributes("class") = "ReadOnlyBackColor"
            txtAID.Attributes("class") = "ReadOnlyBackColor"
            txtAIDName.Attributes("class") = "ReadOnlyBackColor"
        Else
            txtACode.Attributes("class") = "ReadOnlyBackColor"
            txtACodeName.Attributes("class") = "ReadOnlyBackColor"
            txtAID.Attributes("class") = "ReadOnlyBackColor"
            txtAIDName.Attributes("class") = "ReadOnlyBackColor"
        End If
    End Sub

    Public Sub RemoveAtt(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim AssetNo As String = Me.Session("txtAAsstInvNo")
        Dim sqlDelPic As String
        Dim cmdArg As String = e.CommandArgument.ToString
        Dim str As String = cmdArg.ToString.Substring(cmdArg.IndexOf("/") + 1, cmdArg.Length - (cmdArg.IndexOf("/")) - 1)
        Dim idx As Integer = cmdArg.ToString.Substring(0, cmdArg.IndexOf("/")) - 1
        attList.Items(idx).Visible = False
        sqlDelPic = "delete from cmms_entry_attachfiles where Asset_Inv_No = '" & AssetNo & "' and File_Name ='" & str & "';"
        Execute_Delete(sqlDelPic)
        If RemoveAttList(AssetNo) = True Then
            rbAAttach.Checked = True
        Else
            rbAAttach.Checked = False
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

    Protected Sub rbABranch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbABranch.CheckedChanged
        If rbABranch.Checked = True Then
            rbADivision.Checked = False
            rbAAttach.Checked = False
            rbAScan.Checked = False
            lblABCCode.Text = "BC Code"
            lblABCName.Text = "BC Name"
            lblCountWord.Text = "500/500"
            EmptyFieldsCategory()
            EmptyFieldsInformation()
            DisableTextboxCtrl()
            ClearFieldsCategory()
            PInfo(txtAIDName.Text)
            attList.Visible = False
            If CheckAttach() = True Then
                Dim AN As String = Me.Session("txtAAsstInvNo")
                Dim DelSql As String
                DelSql = "delete from cmms_entry_attachfiles where Asset_Inv_No ='" & AN & "';"
                Execute_Delete(DelSql)
            End If
        End If
    End Sub

    Protected Sub rbADivision_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbADivision.CheckedChanged
        If rbADivision.Checked = True Then
            rbABranch.Checked = False
            rbAAttach.Checked = False
            rbAScan.Checked = False
            lblABCCode.Text = "Division Code"
            lblABCName.Text = "Division Name"
            lblCountWord.Text = "500/500"
            EmptyFieldsCategory()
            EmptyFieldsInformation()
            DisableTextboxCtrl()
            ClearFieldsCategory()
            PInfo(txtAIDName.Text)
            attList.Visible = False
            If CheckAttach() = True Then
                Dim AN As String = Me.Session("txtAAsstInvNo")
                Dim DelSql As String
                DelSql = "delete from cmms_entry_attachfiles where Asset_Inv_No ='" & AN & "';"
                Execute_Delete(DelSql)
            End If
        End If
    End Sub

    Protected Sub rbAAttach_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbAAttach.CheckedChanged
        If txtAAsstInvNo.Text = "" Then
            lblErrorSaving.Text = "Asset inventory number is empty."
            rbAAttach.Checked = False
            Exit Sub
        Else
            rbAScan.Checked = False
            TemporaryInfo()
            Response.Redirect("~/DataEntry/AttachFileA.aspx")
        End If
    End Sub

    Protected Sub rbAScan_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbAScan.CheckedChanged

        If rbAScan.Checked = True Then
            rbAAttach.Checked = False
        End If
    End Sub

    Protected Sub lbASearchCode_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbASearchCode.Click
        If rbADivision.Checked = True Then
            If Me.txtACode.Text <> Nothing And Me.txtAID.Text = Nothing Then
                If Me.txtACode.Text <> Nothing And Me.Session("flag") = Nothing Then
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
            If Me.txtACode.Text <> Nothing And Me.txtAID.Text = Nothing Then
                If Me.txtACode.Text <> Nothing And Me.Session("bflag") = Nothing Then
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
        'If txtAID.Text <> "" Then
        '    If rbADivision.Checked = True Then
        '        MPEDivison2.Show()
        '    Else
        '        MPEBranch.Show()
        '    End If
        '    Exit Sub
        'End If
        'If rbADivision.Checked = True Then
        '    If txtACode.Text <> "" Then
        '        MPEUsersIDD.Show()
        '    Else
        '        MPEDivison2.Show()
        '    End If
        'Else
        '    If txtACode.Text <> "" Then
        '        MPEUsersID.Show()
        '    Else
        '        MPEBranch.Show()
        '    End If
        'End If
    End Sub

    Protected Sub lbASearchID_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbASearchID.Click
        If Me.txtACode.Text = Nothing Then Exit Sub
        If rbADivision.Checked = True Then
            If Me.txtAID.Text <> Nothing And Me.txtACode.Text = Nothing Then
                If Me.txtACode.Text = Nothing And Me.Session("flag") <> Nothing Then
                    MPEUsersIDD.Show()
                Else
                    MPEDivison2.Show()
                End If
            Else
                MPEUsersIDD.Show()
            End If
        Else
            If Me.txtAID.Text <> Nothing And Me.txtACode.Text = Nothing Then
                If Me.txtACode.Text = Nothing And Me.Session("bflag") <> Nothing Then
                    MPEUsersID.Show()
                Else
                    MPEBranch.Show()
                End If
            Else
                MPEUsersID.Show()
            End If
        End If
        'If txtACode.Text = "" Then
        '    Exit Sub
        'End If
        'If rbADivision.Checked = True Then
        '    MPEUsersIDD.Show()
        'Else
        '    MPEUsersID.Show()
        'End If
    End Sub

    Protected Sub btnASave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnASave.Click
        Dim S As String = ""
        If CheckBlankFields(S) = "False" Then
            Exit Sub
        End If
        If Request.Form(txtADelDate.UniqueID) > Format(Date.Now, "yyyy-MM-dd") Then
            lblErrorSaving.Text = "Advance date is not allowed."
            Exit Sub
        End If
        If ddlDeviceName.Text = Me.Session("DeviceListOthers") AndAlso txtADeviceName.Text = "" Then
            lblErrorSaving.Text = "Fill in Device list Others!"
            Exit Sub
        End If
        If Page.IsValid Then
            SaveAdditionalDeviceEntry()
        End If
    End Sub

    Protected Sub btnAOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnANew.Click
        Response.Redirect("CAAddDevice.aspx")
    End Sub

    Protected Sub disableFieldsCategory()
        For Each c As Control In PanelACategory.Controls
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

    Protected Sub disableFieldsInforamtion()
        For Each c As Control In PanelAInformation.Controls
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
        For Each ctrl As Control In PanelACategory.Controls
            If TypeOf ctrl Is TextBox Then
                CType(ctrl, TextBox).Text = ""
            End If
        Next
    End Sub

    Protected Sub EmptyFieldsInformation()
        For Each ctrl As Control In PanelAInformation.Controls
            If TypeOf ctrl Is TextBox Then
                CType(ctrl, TextBox).Text = ""
            End If
        Next
    End Sub

    Protected Sub DisableTextboxCtrl()
        txtADeviceName.Visible = False
    End Sub

    Protected Sub EnableTextboxCtrl()
        If ddlDeviceName.Text = Me.Session("DeviceListOthers") Then
            txtADeviceName.Visible = True
        Else
            txtADeviceName.Visible = False
        End If
    End Sub

    Protected Sub ddlDeviceName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDeviceName.SelectedIndexChanged
        Create_trxNo()
        SelectDeviceCode()
        Dim ItemCode As String = Me.Session("IDevCODE")
        Dim trxNo As String = Me.Session("IncreTrxNo")
        Dim bcCode As String = txtACode.Text
        If txtACode.Text = "" Then
            txtAAsstInvNo.Text = "Pls. Select Code"
        Else
            If ddlDeviceName.Text = "0" Then
                txtAAsstInvNo.Text = ""
                lblErrorSaving.Text = ""
            Else
                txtAAsstInvNo.Text = ItemCode & "-" & bcCode & "-" & dateCreated & "-" & trxNo
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
                      & "and bc_code = '" & txtACode.Text & "' order by sys_created desc limit 1;"
            ElseIf numRegion = 2 Then
                mySql = "select Asset_Inv_No from cmms_entry_masterheader where Asset_Inv_No like '%" & dateCreated & "%'" _
                      & "and bc_code = '" & txtACode.Text & "' order by sys_created desc limit 1;"
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

    Private Sub SelectDeviceCode()
        Dim ds As New DataSet
        Dim ICode As String
        Dim NwCode As Integer
        Dim _NwCode As String = ""

        Try
            Dim mySql As String = ""
            mySql = "select item_code from cmms_devices_list where item_class = 'ADD' and item_Desc = '" & ddlDeviceName.Text & "';"
            ds = Execute_DataSet(mySql)

            If Not ds Is Nothing AndAlso ds.Tables(0).Rows(0).Item(0).ToString <> "" Then
                ICode = ds.Tables(0).Rows(0).Item(0).ToString
                Me.Session.Add("IDevCODE", ICode)
            Else
                _NwCode = NewCodeOthers(NwCode)
                Me.Session.Add("IDevCODE", _NwCode)
            End If
        Catch ex As Exception
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
        End Try
    End Sub

    Private Function NewCodeOthers(ByVal NwCode As String) As String
        Dim ds As DataSet
        Dim myCode As String

        Try
            myCode = "select max(item_code) + 1 as autnum from cmms_devices_list;"
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

    Private Sub SelectDeviceList()
        Dim ds As New DataSet

        Try
            Dim mySql As String
            mySql = "select item_Desc from cmms_devices_list where item_class = 'ADD' order by item_desc desc;"
            ds = Execute_DataSet(mySql)

            Dim RText As String = ""
            'Dim Others As String = "OTHERS"
            Dim i As Integer
            Dim r As String
            ddlDeviceName.Items.Clear()

            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    ddlDeviceName.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString))
                Next
                r = i + 1
                'ddlDeviceName.Items.Insert(i, New ListItem(Others, r))
                'Me.Session.Add("DeviceListOthers", r)
            Else
                RText = "Nothing Available"
            End If
            ddlDeviceName.Items.Insert(0, New ListItem(RText, "0"))

        Catch ex As Exception
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
        End Try
    End Sub

    Protected Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        If Not Page.IsPostBack Then
            CheckLogin()
            SelectDeviceList()
        End If
        Me.Session("isDivision") = Me.rbADivision.Checked
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
            'If Not sqlDataset Is Nothing Then
            If sqlDataset.Tables(0).Rows.Count <> 0 Then
                Execute_DataSet = sqlDataset
                sqlDataset.Dispose()
                sqlAdapter.Dispose()
            End If
            'End If
            Con.Close()
        Catch ex As Exception
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
            Con.Close()
            Com.Dispose()
        End Try
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

    Private Sub SaveAdditionalDeviceEntry()

        Code = txtACode.Text
        Name = txtACodeName.Text
        IdAssign = txtAID.Text
        NameAssign = txtAIDName.Text
        RegCode = "VISMIN"
        DevDesc = txtADeviceName.Text
        AssetInv = txtAAsstInvNo.Text
        PTPNum = txtAPTPNo.Text
        AssetDesc = txtAAstDescription.Text
        PONum = txtAPONo.Text
        DelDate = Request.Form(txtADelDate.UniqueID)
        If txtASerialNo.Text = "" Then
            SerNum = "-"
        Else
            SerNum = txtASerialNo.Text
        End If
        ItmCode = Me.Session("IDevCODE")
        SysCreator = Me.Session("res_id")
        SysModifier = Me.Session("res_id")

        If cbAActive.Checked = True Then
            Stat = "A"
        Else
            Stat = "I"
        End If

        If ddlDeviceName.Text <> Me.Session("DeviceListOthers") Then
            ItmCode = Me.Session("IDevCODE")
        Else
            Dim myNewCode As String
            myNewCode = "insert into cmms_devices_list(Item_Code, Item_Class, Item_Desc, Sys_Created, Sys_Modified," _
                      & "Sys_Creator, Sys_Modifier)values('" & ItmCode.Trim & "','ADD','" & DevDesc.Trim & "',now(),now()," _
                      & "'" & SysCreator.Trim & "','" & SysModifier.Trim & "');"
            If Execute_Insert(myNewCode) = False Then
                lblErrorSaving.Text = Me.Session("InsertError")
                Exit Sub
            End If
            ItmCode = Me.Session("IDevCode")
        End If


        If DataSaveTransaction() = False Then
            lblErrorSaving.Text = Me.Session("InsertError")
        Else
            lblErrorSaving.Text = "Saved Successfully!"
            disableFieldsCategory()
            disableFieldsInforamtion()
            btnASave.Visible = False
            btnANew.Visible = True
        End If
    End Sub

    Public Function EscapeApostrophe(ByVal as_string As String) As String
        EscapeApostrophe = Replace(as_string, "'", "`")
    End Function

    Private Function DataSaveTransaction() As Boolean
        Dim con As New MySqlConnection
        Dim com As New MySqlCommand
        Dim oTran As MySqlTransaction
        Dim mySqlInsertDetail As String

        mySqlInsertDetail = "insert into cmms_entry_masterheader(Asset_Inv_No,Bc_Code,Bc_Name,Res_ID_Assigned,Emp_Name_Assigned," _
            & "Zone_Code,PTP_No,Item_Code,Asset_Desc,PO_Number,Delivery_Date,Serial_No,Status," _
            & "Sys_Created,Sys_Modified,Sys_Creator,Sys_Modifier)values('" & AssetInv.Trim & "','" & Code.Trim & "','" & Name.Trim & "'," _
            & "'" & IdAssign.Trim & "','" & NameAssign.Trim & "','" & Me.Session("ZCode") & "','" & PTPNum.Trim & "'," _
            & "'" & ItmCode.Trim & "','" & EscapeApostrophe(AssetDesc.Trim) & "','" & PONum.Trim & "','" & DelDate & "'," _
            & "'" & EscapeApostrophe(SerNum.Trim) & "','" & Stat.Trim & "',now(),now(),'" & SysCreator.Trim & "','" & SysModifier.Trim & "');" _
            & "insert into cmms_entry_history(Asset_Inv_No,Bc_Code,Bc_Name,Res_ID_Assigned,Emp_Name_Assigned," _
            & "Zone_Code,Sys_Created,Sys_Modified,Sys_Creator,Sys_Modifier)values('" & AssetInv.Trim & "','" & Code.Trim & "'," _
            & "'" & Name.Trim & "','" & IdAssign.Trim & "','" & NameAssign.Trim & "','" & RegCode.Trim & "'," _
            & "now(),now(),'" & SysCreator.Trim & "','" & SysModifier.Trim & "');"

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

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            CheckUser()
            CheckLogin()
        End If
    End Sub

    Private Sub TemporaryInfo()
        Me.Session.Add("rbADivision", rbADivision.Checked)
        Me.Session.Add("rbABranch", rbABranch.Checked)
        Me.Session.Add("txtACode", txtACode.Text)
        Me.Session.Add("txtACodeName", txtACodeName.Text)
        Me.Session.Add("txtAID", txtAID.Text)
        Me.Session.Add("txtAIDName", txtAIDName.Text)
        Me.Session.Add("ddlDeviceName", ddlDeviceName.Text)
        Me.Session.Add("txtADeviceName", txtADeviceName.Text)
        Me.Session.Add("txtAAsstInvNo", txtAAsstInvNo.Text)
        Me.Session.Add("txtAPTPNo", txtAPTPNo.Text)
        Me.Session.Add("txtAAstDescription", txtAAstDescription.Text)
        Me.Session.Add("txtASerialNo", txtASerialNo.Text)
        Me.Session.Add("rbAAttach", rbAAttach.Checked)
        Me.Session.Add("txtAPONo", txtAPONo.Text)
        Me.Session.Add("txtADelDate", Request.Form(txtADelDate.UniqueID))
        Me.Session.Add("AAttachTrue", True)
    End Sub

    Private Sub TemporaryRevInfo()
        If Me.Session("rbADivision") = "True" Then
            rbADivision.Checked = True
        Else
            rbADivision.Checked = False
        End If
        If Me.Session("rbABranch") = "True" Then
            rbABranch.Checked = True
        Else
            rbABranch.Checked = False
        End If
        If Me.Session("txtACode") <> "" Then
            txtACode.Text = Me.Session("txtACode")
        Else
            txtACode.Text = ""
        End If
        If Me.Session("txtACodeName") <> "" Then
            txtACodeName.Text = Me.Session("txtACodeName")
        Else
            txtACodeName.Text = ""
        End If
        If Me.Session("txtAID") <> "" Then
            txtAID.Text = Me.Session("txtAID")
        Else
            txtAID.Text = ""
        End If
        If Me.Session("txtAIDName") <> "" Then
            txtAIDName.Text = Me.Session("txtAIDName")
        Else
            txtAIDName.Text = ""
        End If
        If Me.Session("ddlDeviceName") <> "" Then
            ddlDeviceName.Text = Me.Session("ddlDeviceName")
        Else
            ddlDeviceName.Text = ""
        End If
        If Me.Session("txtADeviceName") <> "" Then
            txtADeviceName.Text = Me.Session("txtADeviceName")
        Else
            txtADeviceName.Text = ""
        End If
        If Me.Session("txtAAsstInvNo") <> "" Then
            txtAAsstInvNo.Text = Me.Session("txtAAsstInvNo")
        Else
            txtAAsstInvNo.Text = ""
        End If
        If Me.Session("txtAPTPNo") <> "" Then
            txtAPTPNo.Text = Me.Session("txtAPTPNo")
        Else
            txtAPTPNo.Text = ""
        End If
        If Me.Session("txtAAstDescription") <> "" Then
            txtAAstDescription.Text = Me.Session("txtAAstDescription")
        Else
            txtAAstDescription.Text = ""
        End If
        If Me.Session("txtASerialNo") <> "" Then
            txtASerialNo.Text = Me.Session("txtASerialNo")
        Else
            txtASerialNo.Text = ""
        End If
        If Me.Session("rbAAttach") = "True" Then
            rbAAttach.Checked = True
        Else
            rbAAttach.Checked = False
        End If
        If Me.Session("txtAPONo") <> "" Then
            txtAPONo.Text = Me.Session("txtAPONo")
        Else
            txtAPONo.Text = ""
        End If
        If Me.Session("txtADelDate") <> "" Then
            txtADelDate.Text = Me.Session("txtADelDate")
        Else
            txtADelDate.Text = ""
        End If
        Me.Session.Add("AAttachTrue", False)
    End Sub

    Private Sub BindAttachment(ByVal AstNo As String)
        CheckLogin()
        Dim AssetNo As String = Me.Session("txtAAsstInvNo")
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

    Private Function CheckAttachB() As Boolean
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
        Me.Session("_txtEName") = Nothing
        Me.Session("_txtEID") = Nothing
        Me.Session("flag") = Nothing
    End Sub
End Class
