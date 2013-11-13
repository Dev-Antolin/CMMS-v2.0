<%@ Page Title="" Language="VB" MasterPageFile="~/LeftRPTMainMasterPage.master" AutoEventWireup="false" CodeFile="BranchAllRPT.aspx.vb" Inherits="Report_BranchAllRPT" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CMMSMainContentPlaceHolder" Runat="Server">
<div id="main-copy">
<h1 id="introduction">Branch Asset Profile</h1>
       <div id="CrystalRPTView" style="overflow:scroll; width:775px;">
           <CR:CrystalReportViewer ID="rptViewer" runat="server" AutoDataBind="True"
            DisplayToolbar="True" DisplayGroupTree="False" EnableDrillDown="False" EnableParameterPrompt="False"
            HasCrystalLogo="False" HasDrillUpButton="False" 
            HasSearchButton="False" HasToggleGroupTreeButton="False"
            HasViewList="False" toolbarstyle-width="760px" 
            HasZoomFactorList="False" ReportSourceID="rptSource" BorderColor="Black" 
            BorderStyle="Solid" BorderWidth="1px" SkinID="0" 
            GroupTreeStyle-BackColor="White" BackColor="White"/>
        </div>
</div>
</asp:Content>

