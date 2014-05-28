<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="StatusMonitor.aspx.vb"
    Inherits="CKG.Admin.StatusMonitor" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        .style1
        {
            height: 19px;
        }
        .style2
        {
            height: 22px;
        }
        .style3
        {
            width: 100%;
            height: 22px;
        }
        .StandardButton
        {}
    </style>
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
                        <td class="PageNavigation" colspan="4">
                            <asp:Label ID="lblHead" runat="server">Administration</asp:Label>
                            <asp:Label ID="lblPageTitle" runat="server" Font-Bold="True">DAD-Statusmonitor</asp:Label>
                        </td>
                        <td class="PageNavigation">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblMessage" runat="server" ForeColor="#009933"></asp:Label>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>  
                    <tr>
                        <td valign="top"  rowspan="5" >
                            <table cellspacing="0" cellpadding="0" width="150" border="0">
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="lbZurueck" runat="server" CssClass="StandardButton">&#149;&nbsp;zurück</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                                                <tr>
                                    <td>
                                        &nbsp;
                                        </td>
                                </tr>
                            </table>
                        </td>
                        <td style="padding-left: 5px" >
                            <b>ServiceCenter</b> </td>
                        <td style="padding-left: 5px" colspan="2">
                            <b>Kunde/Abteilung</b> </td>
                    </tr>
                    <tr>
                        <td  style="width: 15%;vertical-align:top" rowspan="3">
                            <asp:RadioButtonList ID="rbServiceCenter" runat="server" AutoPostBack="True">
                            </asp:RadioButtonList>
                        </td>                    
                        <td style="padding-left: 5px;">
                            <asp:RadioButton ID="rbAuswahl" GroupName="Auswahl" Text="Auswahl"  
                                Checked="true" runat="server" AutoPostBack="True" /></td>
                        <td style="width: 100%; padding-left: 5px;">
                            <asp:DropDownList ID="ddlAbteilung" runat="server" Height="16px" Width="250px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 5px;" class="style2">
                             <asp:RadioButton ID="rbNeu" GroupName="Auswahl" Text="Neu" runat="server" 
                                 AutoPostBack="True" /></td>
                        <td style="padding-left: 5px;vertical-align:top" class="style3">
                                        <asp:TextBox runat="server" ID="txtKundenName" Enabled="false" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 5px;vertical-align:top">
                            &nbsp;</td>
                        <td style="width: 100%; padding-left: 5px; padding-bottom: 15px;vertical-align:top">
                                        <asp:LinkButton ID="lbHinzufuegen" runat="server" 
                                CssClass="StandardButton" Height="16px">•&nbsp;hinzufügen</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>

                        <td  id="tdGrid" colspan="3">
                            <asp:GridView AutoGenerateColumns="False" AllowSorting="true" BorderWidth="1" BorderStyle="Solid"
                                runat="server" Width="100%" ID="gv" Visible="false">
                                <AlternatingRowStyle BorderWidth="1" BorderStyle="solid" VerticalAlign="Middle" HorizontalAlign="Center"
                                    Font-Bold="true" Font-Size="Medium" BackColor="WhiteSmoke" />
                                <RowStyle Height="60px" Width="120px" BorderWidth="0" BorderStyle="none" VerticalAlign="Middle"
                                    HorizontalAlign="Center"  BackColor="White" Font-Bold="true" Font-Size="Medium" />
                                <HeaderStyle Font-Size="Large" BackColor="White" Font-Bold="true" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <Columns>
                                    <asp:BoundField Visible="false" HeaderText="Datensatz ID" DataField="id" ReadOnly="true" />
                                    <asp:TemplateField SortExpression="Abteilung">
                                        <HeaderTemplate>
                                            <asp:LinkButton runat="server" CommandArgument="Abteilung" CommandName="sort" Text="Kunde/Abteilung"></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Abteilung") %>'
                                                ID="lblAnzeigeKundenName" Visible="true"> </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label runat="server" Text="Berechtigungsreferenz" ID="lblAnzeigeReferenz"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtBerechtigungReferenz" Text='<%# DataBinder.Eval(Container, "DataItem.Benutzerreferenz") %>'></asp:TextBox>
                                            <asp:LinkButton runat="server" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ID") %>'
                                                CommandName="saveReferenz" Text="speichern" ID="lbSaveReferenz"> </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label runat="server" Text="Pfad zum Logo" ID="lblAnzeigeTextLogo"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtLogoPfad" Text='<%# DataBinder.Eval(Container, "DataItem.Logo") %>'></asp:TextBox>
                                            <asp:LinkButton runat="server" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ID") %>'
                                                CommandName="save" Text="speichern" ID="lbSave"> </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label runat="server" Text="Seite" ID="lblKunde"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DropDownList runat="server" ID="ddlPlatzierung" AutoPostBack="true" AppendDataBoundItems="false">
                                                <asp:ListItem Text="Rechts" Value="R"></asp:ListItem>
                                                <asp:ListItem Text="Links" Value="L"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        &nbsp;</td>

                        <td  id="tdGrid">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label><br />

                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                  
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
