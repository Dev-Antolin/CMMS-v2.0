<%@ Page Title="" Language="VB" MasterPageFile="~/LeftRPTMainMasterPage.master" AutoEventWireup="false"
    CodeFile="AreaProf.aspx.vb" Inherits="Report_AreaProf" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CMMSMainContentPlaceHolder" runat="Server">
    <ajaxToolkit:ToolkitScriptManager runat="Server" ID="ScriptManager1" EnablePageMethods="true" />
    <asp:UpdatePanel runat="server" ID="UpdateFace">
        <ContentTemplate>
            <div id="main-copy">
                <h1 id="introduction">
                    Area Asset Profile</h1>
                <br />
                <center>
                    <asp:Panel ID="PanelAreaProfile" runat="server" GroupingText="Asset Profile: Area"
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
                                    <asp:Label ID="lblSelRegion" runat="server" Text="Region"></asp:Label>&nbsp;<asp:Label
                                        ID="Label23" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlRegion" runat="server" AutoPostBack="True" 
                                        CssClass="WriteBackColor" Enabled="false" Width="240px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 143px">
                                    <asp:Label ID="lblArea" runat="server" Text="Area"></asp:Label>&nbsp;<asp:Label
                                        ID="Label19" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlArea" runat="server" AutoPostBack="True" 
                                        CssClass="WriteBackColor" Enabled="false" TabIndex="2" Width="240px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 143px">
                                    <asp:Label ID="lblAreaMan" runat="server" Text="Area Manager"></asp:Label>
                                    &nbsp;<asp:Label ID="Label21" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtAreaManager" runat="server" CssClass="ReadOnlyBackColor" 
                                        Width="186px" Enabled="False"></asp:TextBox>
                                    <asp:HiddenField ID="hfAreaMan" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 143px">
                                    &nbsp;
                                </td>
                                <td align="left">
                                    &nbsp;
                                    <asp:Button ID="btnGeneRptArea" runat="server" Text="Generate Report" 
                                        Width="120px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 143px">
                                    &nbsp;
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblErrorReport" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <br />
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
