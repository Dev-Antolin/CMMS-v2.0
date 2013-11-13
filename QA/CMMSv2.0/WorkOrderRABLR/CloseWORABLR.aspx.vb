Imports INI_DLL
Imports MYSQLDB_DLL
Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient
Imports System
Imports System.Data
Partial Class WorkOrderRABLR_CloseWORABLR
    Inherits System.Web.UI.Page
    Dim UsersCloseWO As String
    Dim UserReceivable As String
    Dim RecordCount As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Disable Cache
        Response.Buffer = True
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
        Response.Expires = -1500
        Response.CacheControl = "no-cache"
        CheckLogin()
        If Not Page.IsPostBack Then
            Me.Session("Click") = "XWORABLR"
            'txtSearchYear.Text = Format(Date.Now, "yyyy")
        End If
        UsersCloseWO = Me.Session("fName")
        If disCloseWO() = False Then
            lblSearchError.Text = "No record(s) found!"
        End If
    End Sub

    Private Sub CheckLogin()
        If Me.Session("fName") = "" Then
            Response.Redirect("~/login.aspx")
        End If
    End Sub


    Protected Sub btnSearchWO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchWO.Click
        If SearchWO() = False Then
            If txtSearchWONo.Text = "" Then
                lblSearchError.Text = "No record(s) found!"
                gvReceivedDetails.Visible = False
                'lblPrompt.Visible = True
                'lblPrompt.Text = "WORK ORDER NOT FOUND!"
            Else
                lblSearchError.Text = "Work Order No. " & txtSearchWONo.Text.ToUpper & " not found!"
                gvReceivedDetails.Visible = False
                'lblPrompt.Visible = True
                'lblPrompt.Text = "WORK ORDER NOT FOUND!"
            End If
        Else
            If ddlSearchMonth.Text = "" Then
                If txtSearchWONo.Text = "" AndAlso ddlSearchMonth.Text = "" Then
                    'lblSearchError.Text = "Select work order number and month to inquire."
                    lblPrompt.Visible = False
                    Exit Sub
                Else
                    lblSearchError.Text = RecordCount & " file(s) found."
                    lblPrompt.Visible = False
                    Exit Sub
                End If
            ElseIf txtSearchWONo.Text = "" AndAlso ddlSearchMonth.Text = "" Then
                lblSearchError.Text = RecordCount & " file(s) found."
                lblPrompt.Visible = False
                Exit Sub
            ElseIf txtSearchWONo.Text = "" Then
                lblSearchError.Text = RecordCount & " file(s) found."
                lblPrompt.Visible = False
                Exit Sub
            ElseIf txtSearchWONo.Text = txtSearchWONo.Text AndAlso ddlSearchMonth.Text = ddlSearchMonth.Text Then
                lblSearchError.Text = RecordCount & " file(s) found."
                lblPrompt.Visible = False
                Exit Sub
            Else
                lblSearchError.Text = "No record(s) found!"
                gvReceivedDetails.Visible = False
                'lblPrompt.Visible = True
                'lblPrompt.Text = "WORK ORDER NOT FOUND!"
            End If
        End If
    End Sub

    Protected Sub gvReceivedDetails_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvReceivedDetails.SelectedIndexChanged
        Me.Session.Add("CloseWorkOrderNumber", gvReceivedDetails.SelectedRow.Cells(1).Text)
        Me.Session.Add("CloseWorkOrderAuthor", gvReceivedDetails.SelectedRow.Cells(4).Text)
        Me.Session.Add("CloseWorkOrderPosition", gvReceivedDetails.SelectedRow.Cells(5).Text)
        Response.Redirect("CloseDetailRABLR.aspx")
    End Sub

    Public Function SpecialNString(ByVal as_string As String) As String
        SpecialNString = Replace(as_string, "&#209;", "Ñ")
    End Function

    Private Function disCloseWO() As Boolean
        Dim ds As DataSet
        Dim mySqlClose As String

        Try
            mySqlClose = "CALL SP_CMMS_WOCloseDisplay(0, '" & SpecialNString(UsersCloseWO.Trim) & "', '', '', '' )"

            ds = Execute_DataSetCMMS(mySqlClose)
            If Not ds Is Nothing AndAlso ds.Tables(0).Rows(0).Item(0).ToString.Trim <> "" Then
                gvReceivedDetails.DataSource = ds
                gvReceivedDetails.DataBind()
                For x = 0 To gvReceivedDetails.Rows.Count - 1
                    gvReceivedDetails.Rows(x).Cells(4).Text.ToString.Replace("&#241;", "Ñ")
                Next
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Private Function SearchWO() As Boolean
        Dim ds As DataSet
        Dim MonthYear As String
        Dim mySqlSearchWO As String
        MonthYear = ddlSearchMonth.Text & " " & txtSearchYear.Text

        Try
            If txtSearchWONo.Text <> "" AndAlso ddlSearchMonth.SelectedIndex <> 0 AndAlso txtSearchYear.Text <> "" Then
                mySqlSearchWO = "CALL SP_CMMS_WOCloseDisplay(1, '" & SpecialNString(UsersCloseWO.Trim) & "', '" & txtSearchWONo.Text & "', '" & ddlSearchMonth.SelectedIndex & "', '" & txtSearchYear.Text & "' )"
            ElseIf txtSearchWONo.Text = "" AndAlso ddlSearchMonth.SelectedIndex = 0 AndAlso txtSearchYear.Text <> "" Then
                mySqlSearchWO = "CALL SP_CMMS_WOCloseDisplay(2, '" & SpecialNString(UsersCloseWO.Trim) & "', '', '', '" & txtSearchYear.Text & "' )"
            ElseIf txtSearchWONo.Text = "" AndAlso ddlSearchMonth.SelectedIndex <> 0 AndAlso txtSearchYear.Text = "" Then
                mySqlSearchWO = "CALL SP_CMMS_WOCloseDisplay(3, '" & SpecialNString(UsersCloseWO.Trim) & "', '', '" & ddlSearchMonth.SelectedIndex & "', '' )"
            ElseIf txtSearchWONo.Text <> "" AndAlso ddlSearchMonth.SelectedIndex = 0 AndAlso txtSearchYear.Text = "" Then
                mySqlSearchWO = "CALL SP_CMMS_WOCloseDisplay(4, '" & SpecialNString(UsersCloseWO.Trim) & "', '" & txtSearchWONo.Text & "', '', '' )"
            ElseIf txtSearchWONo.Text = "" AndAlso ddlSearchMonth.SelectedIndex <> 0 AndAlso txtSearchYear.Text <> "" Then
                mySqlSearchWO = "CALL SP_CMMS_WOCloseDisplay(5, '" & SpecialNString(UsersCloseWO.Trim) & "', '', '" & ddlSearchMonth.SelectedIndex & "', '" & txtSearchYear.Text & "' )"
            ElseIf txtSearchWONo.Text <> "" AndAlso ddlSearchMonth.SelectedIndex = 0 AndAlso txtSearchYear.Text <> "" Then
                mySqlSearchWO = "CALL SP_CMMS_WOCloseDisplay(6, '" & SpecialNString(UsersCloseWO.Trim) & "', '" & txtSearchWONo.Text & "', '', '" & txtSearchYear.Text & "' )"
            ElseIf txtSearchWONo.Text <> "" AndAlso ddlSearchMonth.SelectedIndex <> 0 AndAlso txtSearchYear.Text = "" Then
                mySqlSearchWO = "CALL SP_CMMS_WOCloseDisplay(7, '" & SpecialNString(UsersCloseWO.Trim) & "', '" & txtSearchWONo.Text & "', '" & ddlSearchMonth.SelectedIndex & "', '' )"
            Else
                mySqlSearchWO = "CALL SP_CMMS_WOCloseDisplay(0, '" & SpecialNString(UsersCloseWO.Trim) & "', '', '', '' )"
            End If

            ds = Execute_DataSetCMMS(mySqlSearchWO)
            If Not ds Is Nothing AndAlso ds.Tables(0).Rows(0).Item(0).ToString.Trim <> "" Then
                gvReceivedDetails.DataSource = ds
                gvReceivedDetails.DataBind()
                For x = 0 To gvReceivedDetails.Rows.Count - 1
                    gvReceivedDetails.Rows(x).Cells(4).Text.ToString.Replace("&amp;#209;", "Ñ")
                Next
                gvReceivedDetails.Visible = True
                RecordCount = gvReceivedDetails.Rows.Count
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
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

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            CheckLogin()
        End If
    End Sub

    Protected Sub txtSearchWONo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearchWONo.TextChanged
        If SearchWO() = False Then
            If txtSearchWONo.Text = "" Then
                lblSearchError.Text = "No record(s) found!"
                gvReceivedDetails.Visible = False
                'lblPrompt.Visible = True
                'lblPrompt.Text = "WORK ORDER NOT FOUND!"
            Else
                lblSearchError.Text = "Work Order No. " & txtSearchWONo.Text & " not found!"
                gvReceivedDetails.Visible = False
                'lblPrompt.Visible = True
                'lblPrompt.Text = "WORK ORDER NOT FOUND!"
            End If
        Else
            If ddlSearchMonth.Text = "" Then
                If txtSearchWONo.Text = "" AndAlso ddlSearchMonth.Text = "" Then
                    lblSearchError.Text = "Select work order number and month to inquire."
                    lblPrompt.Visible = False
                    Exit Sub
                Else
                    lblSearchError.Text = RecordCount & " file(s) found."
                    lblPrompt.Visible = False
                    Exit Sub
                End If
            ElseIf txtSearchWONo.Text = "" AndAlso ddlSearchMonth.Text = "" Then
                lblSearchError.Text = RecordCount & " file(s) found."
                lblPrompt.Visible = False
                Exit Sub
            ElseIf txtSearchWONo.Text = "" Then
                lblSearchError.Text = RecordCount & " file(s) found."
                lblPrompt.Visible = False
                Exit Sub
            ElseIf txtSearchWONo.Text = txtSearchWONo.Text AndAlso ddlSearchMonth.Text = ddlSearchMonth.Text Then
                lblSearchError.Text = RecordCount & " file(s) found."
                lblPrompt.Visible = False
                Exit Sub
            Else
                lblSearchError.Text = "No record(s) found!"
                gvReceivedDetails.Visible = False
                'lblPrompt.Visible = True
                'lblPrompt.Text = "WORK ORDER NOT FOUND!"
            End If
        End If
    End Sub
End Class
