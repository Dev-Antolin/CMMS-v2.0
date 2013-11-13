<%@ Page Title="" Language="VB" MasterPageFile="~/LeftRPTMainMasterPage.master" AutoEventWireup="false" CodeFile="RegionProf.aspx.vb" Inherits="Report_RegionProf" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>    
<asp:Content ID="Content1" ContentPlaceHolderID="CMMSMainContentPlaceHolder" Runat="Server">
    <ajaxToolkit:ToolkitScriptManager runat="Server" ID="ScriptManager1" EnablePageMethods="true" />
    <asp:UpdatePanel runat="server" ID="UpdateFace">
    <ContentTemplate>
    <div id="main-copy">
        <h1 id="introduction">Region Asset Profile</h1>
                    <br />
            <center>
                <asp:Panel ID="PanelRegionProfile" runat="server" 
            GroupingText="Asset Profile: Region" Width="485px" TabIndex="7">
                    <table style="width: 100%">
                        <tr>
                            <td align="right" style="width: 143px">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="lblRegion" runat="server" Text="Region"></asp:Label>
                                &nbsp;<asp:Label ID="Label19" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlRegion" CssClass="WriteBackColor" runat="server" AutoPostBack="True" 
                                    TabIndex="2" Width="240px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="lblRegionMan" runat="server" Text="Region Manager"></asp:Label>
                                &nbsp;<asp:Label ID="Label21" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtRegMan" CssClass="ReadOnlyBackColor" runat="server" 
                                    Width="186px" Enabled="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                &nbsp;</td>
                            <td align="left">
                                <asp:Button ID="btnGeneRptRegion" runat="server" Text="Generate Report" 
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
    </div>
    </ContentTemplate> 
    </asp:UpdatePanel>     
</asp:Content>

