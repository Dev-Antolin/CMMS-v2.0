<%@ Page Title="" Language="VB" MasterPageFile="~/LeftRPTMainMasterPage.master" AutoEventWireup="false" CodeFile="MaintenanceHistory.aspx.vb" Inherits="Report_MaintenanceHistory" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CMMSMainContentPlaceHolder" Runat="Server">
    <ajaxToolkit:ToolkitScriptManager runat="Server" ID="ScriptManager1" EnablePageMethods="true" />
   <asp:UpdatePanel runat="server" ID="UpdateFace">
    <ContentTemplate>
    <div id="main-copy">
        <h1 id="introduction">Maintenance History</h1>
            <br />
            <center>
                <asp:Panel ID="PanelBranchProfile" runat="server" 
            GroupingText="Branch Asset Maintenance History " Width="485px" TabIndex="7">
                    <table style="width: 100%">
                            <tr>
                                <td align="right" style="width: 143px">
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 143px">
                                    <asp:Label ID="Label1" runat="server" Text="Date From"></asp:Label>&nbsp;<asp:Label
                                        ID="Label14" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtDateFrom" CssClass="ReadOnlyBackColor" runat="server" Width="100px"
                                        TabIndex="1" MaxLength="10" ReadOnly="True"></asp:TextBox>
                                    <asp:ImageButton ID="ibDateFrom" CssClass="BtnCalendarSearch" runat="server" ImageUrl="~/Images/calendar-blue.png" />
                                    <ajaxToolkit:CalendarExtender ID="txtDateFrom_CalendarExtender" runat="server" Enabled="True"
                                        Format="yyyy-MM-dd" PopupPosition="Right" PopupButtonID="ibDateFrom" TargetControlID="txtDateFrom">
                                    </ajaxToolkit:CalendarExtender>
                                    <asp:HiddenField ID="hfFromAll" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 143px">
                                    <asp:Label ID="Label2" runat="server" Text="Date To"></asp:Label>&nbsp;<asp:Label
                                        ID="Label24" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtDateTo" CssClass="ReadOnlyBackColor" runat="server" MaxLength="10"
                                        TabIndex="1" Width="100px" ReadOnly="True"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="txtDateTo_CalendarExtender" runat="server" Enabled="True"
                                        Format="yyyy-MM-dd" PopupButtonID="ibDateTo" PopupPosition="Right" TargetControlID="txtDateTo">
                                    </ajaxToolkit:CalendarExtender>
                                    <asp:ImageButton ID="ibDateTo" runat="server" CssClass="BtnCalendarSearch" ImageUrl="~/Images/calendar-blue.png" />
                                    <asp:HiddenField ID="hfToAll" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 143px">
                                    <asp:Label ID="lblRegion" runat="server" Text="Region"></asp:Label>
                                    &nbsp;<asp:Label ID="Label22" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlRegion" CssClass="WriteBackColor" runat="server" AutoPostBack="True"
                                        Width="240px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 143px">
                                    <asp:Label ID="lblArea" runat="server" Text="Area"></asp:Label>
                                    &nbsp;<asp:Label ID="Label23" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlArea" CssClass="WriteBackColor" runat="server" Width="240px"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 143px">
                                    <asp:Label ID="lblBranch" runat="server" Text="Branch"></asp:Label>
                                    &nbsp;Name
                                    <asp:Label ID="Label19" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlBranch" CssClass="WriteBackColor" runat="server" AutoPostBack="True"
                                        Width="240px" TabIndex="2">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 143px">
                                    Branch Employee&nbsp;<asp:Label ID="Label21" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlBraMem" CssClass="WriteBackColor" runat="server" AutoPostBack="True"
                                        Width="240px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 143px">
                                    &nbsp;
                                </td>
                                <td align="left">
                                    <asp:Button ID="btnGeneRptHistory" runat="server" Text="Generate Report" 
                                        Width="120px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 143px">
                                    &nbsp;</td>
                                <td align="left">
                                    <asp:Label ID="lblErrorReport" runat="server" Font-Italic="True" 
                                        ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                </asp:Panel></center>
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

