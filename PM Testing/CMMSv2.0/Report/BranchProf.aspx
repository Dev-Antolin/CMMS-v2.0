<%@ Page Title="" Language="VB" MasterPageFile="~/LeftRPTMainMasterPage.master" AutoEventWireup="false"
    CodeFile="BranchProf.aspx.vb" Inherits="Report_BranchProf" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CMMSMainContentPlaceHolder" runat="Server">    
    <style type = "text/css">
    .Container
    {
        height:100%;
        width: 100%;
        background-color:#43768C;    	
   	}
    </style>
    <ajaxToolkit:ToolkitScriptManager runat="Server" ID="ScriptManager1" EnablePageMethods="true" />
        <asp:UpdatePanel runat="server" ID="UpdateFace">

        <ContentTemplate>
            <div id="main-copy">
                <h1 id="introduction">
                    Branch Asset Profile</h1>
                <br />
                <center>
                    <asp:Panel ID="PanelBranchProfile" runat="server" GroupingText="Asset Profile: Branch"
                        Width="485px" TabIndex="7" HorizontalAlign="Center">
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
                                    <asp:Label ID="lblRegion" runat="server" Text="Region"></asp:Label>&nbsp;<asp:Label
                                        ID="Label22" runat="server" Text=":"></asp:Label>
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
                                        ID="Label23" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlArea" runat="server" AutoPostBack="True" 
                                        CssClass="WriteBackColor" Enabled="false" Width="240px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 143px">
                                    <asp:Label ID="lblBranch" runat="server" Text="Branch"></asp:Label>
                                    &nbsp;Name <asp:Label ID="Label2colon" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlBranch" CssClass="WriteBackColor" runat="server" AutoPostBack="True"
                                        Width="240px" Enabled ="false" TabIndex="2">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 143px">
                                    Branch Employee&nbsp;<asp:Label ID="lblAreE" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlBraMem" CssClass="WriteBackColor" runat="server" Width="240px"
                                        AutoPostBack="True" Enabled ="false">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 143px">
                                    &nbsp;
                                </td>
                                <td align="left">
                                    <asp:Button ID="btnGeneRptBranch" runat="server" Text="Generate Report" 
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
<%--    <asp:UpdatePanel ID="UpdateReport" runat="server" UpdateMode="Conditional">            
        <ContentTemplate>
            <ajaxToolkit:ModalPopupExtender ID="ReportMPE" runat="server" TargetControlID="rptViewer" 
                PopupDragHandleControlID="Panel1" PopupControlID="Panel1" RepositionMode="RepositionOnWindowResize"
                DropShadow="true"
            />
            <asp:Panel ID="Panel1" runat="server" >
                <div id="Div1" style= "height: 800%; width:500%; background-color:Green; ">
                    <h1 id="H1">Branch Asset Profile</h1>
                       <div id="CrystalRPTView" style=" width:780px;  background-color:Gray;">
                           <CR:CrystalReportViewer ID="rptViewer" runat="server" AutoDataBind="false"
                            DisplayToolbar="True" DisplayGroupTree="False" EnableDrillDown="False" EnableParameterPrompt="False"
                            HasCrystalLogo="False" HasDrillUpButton="False" 
                            HasSearchButton="False" HasToggleGroupTreeButton="False"
                            HasViewList="False" toolbarstyle-width="760px" 
                            HasZoomFactorList="False" ReportSourceID="rptSource" BorderColor="Black" 
                            BorderStyle="Solid" BorderWidth="1px" SkinID="0" 
                            GroupTreeStyle-BackColor="White" BackColor="White"/>
                        </div>
                        <asp:Button ID="btnClose" Text="Close" runat ="server" />
                </div>
            </asp:Panel>
        </ContentTemplate>            
    </asp:UpdatePanel>--%>
</asp:Content>
