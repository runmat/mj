<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>


<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Statusmonitor-StatusAenderung.aspx.vb" Inherits="CKG.Admin.Statusmonitor_StatusAenderung" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td>
<uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table4" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server">Administration</asp:Label><asp:Label ID="lblPageTitle" runat="server" Font-Bold="True">DAD-Statusmonitor</asp:Label></td></tr><tr>
                    <td colspan="2">
                    <asp:label id="lblError" runat="server" CssClass="TextError"></asp:label><br />
                                      <asp:label id="lblMessage" runat="server" ForeColor="#009933" ></asp:label></td></tr><tr>
                        <td rowspan="2" valign="top" width="120">
                            <table id="Table5" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                    <tr>
                                    <td valign="center"  width="150">
                                        <asp:LinkButton ID="lbZurueck" runat="server"  CssClass="StandardButton">&#149;&nbsp;zurück</asp:LinkButton></td></tr></table></td><td  style="width:100%; vertical-align:top">
                            <asp:RadioButtonList ID="rbServiceCenter" runat="server" AutoPostBack="True">
                            </asp:RadioButtonList>
                             <asp:Label ID="lblServiceCenter"  runat="server" Font-Size="16pt" 
                                 Font-Bold="True"></asp:Label></td></tr><tr>
                        <td>
                            <asp:GridView AutoGenerateColumns="False" AllowSorting="true" BorderWidth="1" BorderStyle="Solid" runat="server"
                                Width="100%" ID="gv">
                                <AlternatingRowStyle BorderWidth="1" BorderStyle="solid" VerticalAlign="Middle" HorizontalAlign="Center"
                                    Font-Bold="true" Font-Size="Medium" BackColor="WhiteSmoke" />
                                <RowStyle Height="60px" Width="120px" BorderWidth="0" BorderStyle="none" VerticalAlign="Middle"
                                    HorizontalAlign="Center" Font-Bold="true" Font-Size="Medium" />
                                <HeaderStyle Font-Size="Large"  Font-Bold="true"                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                                <Columns>
                                       <asp:BoundField  Visible="false" HeaderText="Datensatz ID"  DataField="id" ReadOnly="true" />                        
                                    <asp:TemplateField SortExpression="Abteilung" >
                                        <HeaderTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="Abteilung"  CommandName="sort" Text="Kunde/Abteilung"></asp:LinkButton></HeaderTemplate><ItemTemplate>
                                            <asp:Label  runat="server"  Text='<%# DataBinder.Eval(Container, "DataItem.Abteilung") %>' ID="lblAnzeigeKundenName"  Visible="true"> </asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField >
                                        <HeaderTemplate >
                                          <asp:Label  runat="server" Text="Seite"  ID="lblKunde"></asp:Label></HeaderTemplate><ItemTemplate>
                                        <asp:DropDownList runat="server" ID="ddlPlatzierung"  AutoPostBack="true"  AppendDataBoundItems="false"   > 
                                        <asp:ListItem Text="Rechts" Value="R" ></asp:ListItem><asp:ListItem Text="Links" Value="L" ></asp:ListItem></asp:DropDownList></ItemTemplate></asp:TemplateField><asp:TemplateField >
                                        <HeaderTemplate >
                                          <asp:Label  runat="server" Text="Informationstext" ID="lblAnzeigeTextInfoText"></asp:Label></HeaderTemplate><ItemTemplate>
                                            <asp:TextBox runat="server"   MaxLength="500"  Rows="3" Width="400" TextMode="MultiLine" ID="txtInfoText" Text='<%# DataBinder.Eval(Container, "DataItem.InfoText") %>'></asp:TextBox><asp:LinkButton runat="server"  CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ABT_ID") %>' CommandName="saveInfoText"  Text="speichern" ID="lbSaveInfoText"> </asp:LinkButton></ItemTemplate></asp:TemplateField><asp:TemplateField>
                                        <HeaderTemplate >
                                            <asp:Label runat="server" Text="Status" ID="lblStatusHeader"></asp:Label></HeaderTemplate><ItemTemplate >
                                            <asp:Image ID="ImgGut" runat="server" ImageUrl="../Images/allesOK2.jpg" Visible='<%# DataBinder.Eval(Container, "DataItem.StatusNeu")=0 %>' />
                                            <asp:Image ID="ImgSchlecht" runat="server" ImageUrl="../Images/Problem.jpg" Visible='<%# DataBinder.Eval(Container, "DataItem.StatusNeu")=1 %>' />
                                                <asp:LinkButton runat="server" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ABT_ID") %>' CommandName="change" Text="ändern" ID="lbChange"> </asp:LinkButton></ItemTemplate></asp:TemplateField></Columns></asp:GridView></td></tr></tr></table></td></tr></table></form></body></html>