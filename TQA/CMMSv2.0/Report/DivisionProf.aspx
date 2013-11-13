<%@ Page Title="" Language="VB" MasterPageFile="~/LeftRPTMainMasterPage.master" AutoEventWireup="false"
    CodeFile="DivisionProf.aspx.vb" Inherits="Report_DivisionProf" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CMMSMainContentPlaceHolder" runat="Server">
    <ajaxToolkit:ToolkitScriptManager runat="Server" ID="ScriptManager1" EnablePageMethods="true" />
    <asp:UpdatePanel runat="server" ID="UpdateFace">
        <ContentTemplate>
            <div id="main-copy">
                <h1 id="introduction">
                    Division Asset Profile</h1>
                <br />
                <br />
                <center>
                    <asp:Panel ID="PanelDivisionAstProf" runat="server" GroupingText="Division Asset Profile"
                        Width="485px" TabIndex="7">
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
                                    <asp:Label ID="lblDivName" runat="server" Text="Division Name" TabIndex="1"></asp:Label>
                                    &nbsp;<asp:Label ID="Label4" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlDivName" runat="server" AutoPostBack="True" 
                                        CssClass="WriteBackColor" TabIndex="5" Width="190px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 143px">
                                    <asp:Label ID="lblDivManager" runat="server" Text="Division Manager"></asp:Label>
                                    &nbsp;<asp:Label ID="Label20" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtDivManager" CssClass="ReadOnlyBackColor" runat="server" 
                                        Width="186px" Enabled="False"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 143px">
                                    &nbsp;
                                </td>
                                <td align="left">
                                    <asp:Button ID="btnGeneRptAll" runat="server" Text="Generate Report" 
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
                    </asp:Panel>
                </center>
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
