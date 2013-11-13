Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports MySql
Imports MySql.Data
Imports MySql.Data.MySqlClient
Partial Class WorkOrder_CreateWORABLR
    Inherits System.Web.UI.Page
    Dim CodeNo As String
    Dim BCCenter As String
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

    Protected Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        If Not Page.IsPostBack Then
            CheckLogin()
            SelectWOType()
            RABLREscalName()
        End If
    End Sub

    Private Sub RABLREscalName()
        Dim ds As DataSet
        Dim RABLREmpLPTLName As String

        Try
            If Me.Session("ZCode") = "VISMIN" Then
                RABLREmpLPTLName = "select distinct upper(fullname)as fullname from irlptl where class_03 = '" & Me.Session("JRegion") & "';"
            Else
                RABLREmpLPTLName = "select distinct upper(fullname)as fullname from irlptl where class_03 = '" & Me.Session("JRegion") & "';"
            End If
            ds = Execute_DataSet(RABLREmpLPTLName)
            lblWOSelectEscal.Text = ds.Tables(0).Rows(0).Item(0).ToString.Replace("&#241;", "ñ").Trim
            Me.Session.Add("LPTLEscalation", lblWOSelectEscal.Text)
        Catch ex As Exception
            lblPromtError.Text = "No LPTL assigend in this Person."
            lblWOSelectEscal.Text = ""
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Disable Cache
        Response.Buffer = True
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
        Response.Expires = -1500
        Response.CacheControl = "no-cache"
        CheckLogin()
        ReadOnlyFieldsCategory()
        If Not Page.IsPostBack Then
            Me.Session("Click") = "CWORABLR"
            CodeNo = Me.Session("JCodeNo")
            txtWOCode.Text = Me.Session("JCode")
            txtWOName.Text = Me.Session("JCodeName")
            txtWOIDNo.Text = Me.Session("res_id")
            txtWOEmpName.Text = Me.Session("fName").Replace("&#209;", "Ñ").ToString.ToUpper
            'SelectCodeNo()
            BCCenter = txtWOCode.Text
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
        If LLRUsers(Me.Session("JDesc")) = True Then
            PanelAuthorInfo.GroupingText = "Branch Information"
            lblBCodeAuthor.Text = "BC Code"
            lblBNameAuthor.Text = "BC Name"
        End If
        btnWOOk.Visible = False
        lblCountWord.Text = "" & (500 - txtWODesc.Text.Length) & "/500"
        DisableTextboxCtrl()
        EnableTextboxCtrl()
    End Sub

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

    Protected Sub ReadOnlyFieldsCategory()
        For Each ctrl As Control In PanelAuthorInfo.Controls
            If TypeOf ctrl Is TextBox Then
                CType(ctrl, TextBox).ReadOnly = True
            End If
        Next
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

    Private Sub CheckLogin()
        If Me.Session("fName") = "" Then
            Response.Redirect("~/login.aspx")
        End If
    End Sub

    Private Sub Create_trxNo()
        Dim numRegion As Integer

        numRegion = 1

        _Increment = Auto_trxNo(numRegion)
        If _Increment = "error" Then
            Exit Sub
        End If
    End Sub

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
            MsgBox(ex.Message)
        End Try
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
            MsgBox(ex.Message)
            Return "error"
        End Try

    End Function

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

    Private Sub WOTemporaryInfo()
        Me.Session.Add("WOCode", txtWOCode.Text)
        Me.Session.Add("WOName", txtWOName.Text)
        Me.Session.Add("WOIDNo", txtWOIDNo.Text)
        Me.Session.Add("WOEmpName", txtWOEmpName.Text)
        Me.Session.Add("WONo", txtWONo.Text)
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
        If Me.Session("WOSelectEscal") <> "" Then
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

    Private Sub SaveWorkOrder()
        Dim ds As DataSet
        Dim Type As String = getWOType()
        BCodeAuthor = txtWOCode.Text.Trim
        BNameAuthor = txtWOName.Text.Trim
        AuthorName = txtWOEmpName.Text.Trim
        WrkNo = txtWONo.Text.Trim
        EscalDesc = "LPTL_"
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
                MsgBox(ex.Message)
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
            lblPromtError.Text = "Work Order is successfully saved!"
            disableFieldsAuthor()
            disableFieldsWorkOrder()
            btnWOSave.Enabled = False
            btnWOOk.Visible = True
        End If
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
                          & "values('" & BCodeAuthor.Trim & "','" & SpecialNString(BNameAuthor.Trim) & "','" & SpecialNString(AuthorName.Trim) & "','" & WrkNo.Trim & "','" & EscalDesc.Trim & "','" & SpecialNString(EscalName.Trim) & "','" & WOTypeCode.Trim & "'," _
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

    Protected Sub btnWOOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWOOk.Click
        Response.Redirect("~/WorkOrderRABLR/CreateWORABLR.aspx")
    End Sub

    Protected Sub rbAttachFile_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbAttachFile.CheckedChanged
        If rbAttachFile.Checked = True Then
            WOTemporaryInfo()
            Response.Redirect("~/WorkOrderRABLR/AttachFileWORABLR.aspx")
        End If
    End Sub
End Class
