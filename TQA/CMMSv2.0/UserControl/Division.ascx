<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Division.ascx.vb" Inherits="usercontrol_Division" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<link href="../CSS/ModalPopUpExtender.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    .style1
    {
        width: 100%;
    }
    .style2
    {
        width: 65px;
    }
</style>
<div class="GVtxtSearch">
    <table class="style1">
        <tr>
            <td colspan="2">
                <asp:Label ID="Label2" runat="server" Text="Search" Font-Bold="True" Font-Size="Smaller"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style2">
                <asp:TextBox ID="txtSearchDCode" CssClass="WriteBackColor" runat="server" Style="text-transform: uppercase;
                    width:60px;" Width="89px" MaxLength="3"></asp:TextBox>
                <ajaxToolkit:TextBoxWatermarkExtender ID="txtSearchDCode_TextBoxWatermarkExtender"
                    runat="server" Enabled="True" TargetControlID="txtSearchDCode" WatermarkCssClass="watermarked"
                    WatermarkText="Code">
                </ajaxToolkit:TextBoxWatermarkExtender>
                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                    TargetControlID="txtSearchDCode" FilterType="LowercaseLetters,UppercaseLetters,Custom,Numbers"
                    ValidChars=" " />
            </td>
            <td>
                <asp:TextBox ID="txtSearchDName" CssClass="WriteBackColor" Style="text-transform: uppercase;" runat="server"
                    Width="225px" MaxLength="30"></asp:TextBox>
                <ajaxToolkit:TextBoxWatermarkExtender ID="txtSearchDName_TextBoxWatermarkExtender"
                    runat="server" Enabled="True" TargetControlID="txtSearchDName" WatermarkCssClass="watermarked"
                    WatermarkText="Division Name">
                </ajaxToolkit:TextBoxWatermarkExtender>
                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                    TargetControlID="txtSearchDName" FilterType="LowercaseLetters,UppercaseLetters,Custom"
                    ValidChars=" " />
            </td>
        </tr>
    </table>
    <%--    <asp:Label ID="Label1" runat="server" Text="Name"></asp:Label>
    <asp:TextBox ID="txtSearchBranch" runat="server" Style="width: 220px;"></asp:TextBox>
    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server"
        TargetControlID="txtSearchBranch" FilterType="UppercaseLetters,LowercaseLetters,Custom"
        ValidChars=" " />
    <asp:Button ID="btnSearchBranch" runat="server" Text="Search" Font-Names="Tahoma"
        OnClick="btnSearchBranch_Clicked" />--%>
</div>
<div class="GVData">
    <asp:GridView ID="gvBranch" runat="server" AutoGenerateColumns="False" CellPadding="4"
        ForeColor="#333333" GridLines="None" Width="370px" Font-Names="Tahoma" Font-Overline="False"
        Font-Size="Small" EnableViewState="False">
        <RowStyle BackColor="#EFF3FB" />
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton OnClick="Select_Clicked" ID="btnSelect" runat="server">Select</asp:LinkButton>&nbsp;
                </ItemTemplate>
                <HeaderStyle Width="50px" />
            </asp:TemplateField>
            <asp:BoundField DataField="DivisionAcro" HeaderText="Cost Center">
                <HeaderStyle HorizontalAlign="Left" Width="100px" />
            </asp:BoundField>
            <asp:BoundField DataField="Division" HeaderText="Division Name">
                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
            </asp:BoundField>
        </Columns>
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#2461BF" Font-Names="Tahoma" Font-Size="Small" />
        <AlternatingRowStyle BackColor="White" Font-Italic="False" Font-Overline="False"
            Font-Strikeout="False" />
    </asp:GridView>
    <asp:Label ID="lblNoRec" runat="server" Font-Bold="True" Font-Size="Larger" 
        Text="NO RECORD FOUND!"></asp:Label>
</div>
