<%@ Page Title="" Language="VB" MasterPageFile="~/LeftWORABLRMainMasterPage.master"
    AutoEventWireup="false" CodeFile="OpenWORABLR.aspx.vb" Inherits="WorkOrderRABLR_OpenWORABLR" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CMMSRABLRMainContentPlaceHolder"
    runat="Server">
    <asp:ToolkitScriptManager runat="Server" ID="ScriptManager1" EnablePageMethods="true" />
    <div id="main-copy">
        <h1 id="introduction">
            Open Work Order</h1>
        <div id="Flexible">
            <table style="width: 100%">
                <tr>
                    <td style="width: 394px">
                        &nbsp;
                    </td>
                    <td align="right" style="width: 708px">
                        <asp:Label ID="Label6" runat="server" Text="Work Order No."></asp:Label>
                        <asp:Label ID="Label8" runat="server" Text=":"></asp:Label>
                    </td>
                    <td align="right" style="width: 122px">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtSearchWONo" CssClass="txtUppercase WriteBackColor" runat="server"
                                    MaxLength="18" AutoPostBack="True"></asp:TextBox>
                                <%--                                <asp:FilteredTextBoxExtender ID="txtSearchWONo_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txtSearchWONo" FilterType="LowercaseLetters,UppercaseLetters,Custom,Numbers"
                                    ValidChars="-">
                                </asp:FilteredTextBoxExtender>--%>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 75px" align="right">
                        <asp:Label ID="Label5" runat="server" Text="Month/Year"></asp:Label>
                        <asp:Label ID="Label7" runat="server" Text=":"></asp:Label>
                    </td>
                    <td align="left" style="width: 520px">
                        <asp:DropDownList ID="ddlSearchMonth" CssClass="WriteBackColor" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>January</asp:ListItem>
                            <asp:ListItem>February</asp:ListItem>
                            <asp:ListItem>March</asp:ListItem>
                            <asp:ListItem>April</asp:ListItem>
                            <asp:ListItem>May</asp:ListItem>
                            <asp:ListItem>June</asp:ListItem>
                            <asp:ListItem>July</asp:ListItem>
                            <asp:ListItem>August</asp:ListItem>
                            <asp:ListItem>September</asp:ListItem>
                            <asp:ListItem>October</asp:ListItem>
                            <asp:ListItem>November</asp:ListItem>
                            <asp:ListItem>December</asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="txtSearchYear" CssClass="WriteBackColor" runat="server" Width="30px"
                            MaxLength="4"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="txtSearchYear_FilteredTextBoxExtender" runat="server"
                            Enabled="True" FilterType="Custom,Numbers" TargetControlID="txtSearchYear">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnSearchWONo" runat="server" Text="Search" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblSearchError" runat="server" ForeColor="Blue" Font-Size="Small"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 75px" align="right">
                        &nbsp;
                    </td>
                    <td align="left" style="width: 520px">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="ReceivedGrid">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate >
        <asp:GridView ID="gvReceivedDetails" runat="server" AutoGenerateColumns="False" GridLines="None"
            CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
            PageSize="15">
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:BoundField DataField="wo_no" HeaderText="W.O No.">
                    <ItemStyle Width="110px" />
                </asp:BoundField>
                <asp:BoundField DataField="wo_date" HeaderText="W.O Date">
                    <ItemStyle Width="60px" />
                </asp:BoundField>
                <asp:BoundField DataField="bc_name_author" HeaderText="Division/Branch Name" />
                <asp:BoundField DataField="author_name" HeaderText="Author" />
                <asp:BoundField DataField="task" HeaderText="Position" />
                <asp:BoundField DataField="wo_desc" HeaderText="Description" />
                <asp:BoundField DataField="wo_status" HeaderText="Status">
                    <ItemStyle HorizontalAlign="Center" Width="30px" />
                </asp:BoundField>
                <asp:BoundField DataField="Escalated_To" HeaderText="Escalated To" />
                <asp:BoundField DataField="resolve" HeaderText="Resolve">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="30px" />
                </asp:BoundField>
            </Columns>
            <PagerStyle CssClass="pgr"></PagerStyle>
            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
        </asp:GridView>
        </ContentTemplate>
        </asp:UpdatePanel>
        <div id="NoReceivedField">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblPrompt" runat="server" Font-Bold="True" Font-Size="XX-Large"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
