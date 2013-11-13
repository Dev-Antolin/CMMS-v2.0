<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UsersID.ascx.vb" Inherits="UserControl_UsersID" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<link href="../CSS/ModalPopUpExtender.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    .style1
    {
        width: 100%;
    }
    .style2
    {
        width: 95px;
    }
</style>
<div class="GVtxtSearch">
    <table class="style1">
        <tr>
            <td colspan="2">
                <asp:Label ID="Label1" runat="server" Text="Search" Font-Bold="True" Font-Size="Smaller"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style2">
                <asp:TextBox ID="txtSearchID" CssClass="WriteBackColor" runat="server" Style="width: 90px;"
                    Width="89px" MaxLength="10"></asp:TextBox>
                <ajaxToolkit:TextBoxWatermarkExtender ID="txtSearchID_TextBoxWatermarkExtender" runat="server"
                    Enabled="True" TargetControlID="txtSearchID" WatermarkCssClass="watermarked"
                    WatermarkText="ID No.">
                </ajaxToolkit:TextBoxWatermarkExtender>
                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server"
                    TargetControlID="txtSearchID" FilterType="Numbers"
                    ValidChars="" />
            </td>
            <td>
                <asp:TextBox ID="txtSearchEName" CssClass="WriteBackColor" Style="text-transform: uppercase;" runat="server" Width="225px"></asp:TextBox>
                <ajaxToolkit:FilteredTextBoxExtender ID="txtSearchEName_FilteredTextBoxExtender"
                    runat="server" Enabled="True" TargetControlID="txtSearchEName" FilterType="Custom, UppercaseLetters, LowercaseLetters"
                    ValidChars=" .ñ-">
                </ajaxToolkit:FilteredTextBoxExtender>
                <ajaxToolkit:TextBoxWatermarkExtender ID="txtSearchEName_TextBoxWatermarkExtender"
                    runat="server" Enabled="True" TargetControlID="txtSearchEName" WatermarkCssClass="watermarked"
                    WatermarkText="Employee Name">
                </ajaxToolkit:TextBoxWatermarkExtender>
            </td>
        </tr>
    </table>
    <%--<asp:Button ID="btnSearchBranch" runat="server" Text="Search" Font-Names="Tahoma"
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
            </asp:TemplateField>
            <asp:BoundField DataField="res_id" HeaderText="ID No.">
                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="fullname" HeaderText="Employee Name">
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
    <asp:Label ID="lblNoRec" runat="server" Font-Bold="True" Font-Size="Larger" Text="NO RECORD FOUND!"></asp:Label>
</div>
