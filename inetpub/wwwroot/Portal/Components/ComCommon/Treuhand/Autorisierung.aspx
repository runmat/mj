<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Autorisierung.aspx.vb"
    Inherits="CKG.Components.ComCommon.Treuhand.Autorisierung" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td class="PageNavigation" colspan="2">
                <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;
                <asp:Label ID="lblPageTitle" runat="server">Autorisierungen</asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="TaskTitle" valign="top" width="120">
                            &nbsp;
                        </td>
                        <td class="TaskTitle" valign="top" colspan="2">
                                                        &nbsp;
                            </td>
                    </tr>
                    <tr id="trSaveButton" runat="server" visible="False">
                        <td valign="top">
                            <asp:LinkButton ID="cmdSave" runat="server" CssClass="StandardButton"> &#149;&nbsp;Autorisieren</asp:LinkButton>
                        </td>
                        <td valign="center" align="right" width="100%">
                            <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>&nbsp;
                        </td>
                        <td valign="top" align="right">
                            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trBackbutton" runat="server" visible="False">
                        <td valign="top" width="120">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <tr>
                                    <td valign="center">
                                        <asp:LinkButton ID="cmdBack" runat="server" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" colspan="2">
                            <table id="Table6" cellspacing="0" cellpadding="5" width="100%" border="0">
                                <tr>
                                    <td colspan="2">
                                        <asp:GridView ID="GridView1" runat="server" Width="100%" BackColor="White" PageSize="50"
                                            headerCSS="tableHeader" bodyCSS="tableBody" CssClass="tableMain" bodyHeight="400"
                                            AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False">
                                            <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                            <HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
                                            <PagerSettings Visible="False" />
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <Columns>
                                                <asp:TemplateField SortExpression="AppID" Visible="false" HeaderText="AppID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAppID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AppID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="AppName" Visible="false" HeaderText="AppName">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAppName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AppName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="AppURL" Visible="false" HeaderText="AppURL">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAppURL" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AppURL") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="AuthorizationID" Visible="false" HeaderText="AuthorizationID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAuthorizationID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AuthorizationID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="AppFriendlyName" HeaderText="col_Anwendung">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Anwendung" runat="server" CommandName="Sort" CommandArgument="AppFriendlyName">col_Anwendung</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblAnwendung" Text='<%# DataBinder.Eval(Container, "DataItem.AppFriendlyName") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="InitializedBy" HeaderText="col_Initiator">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Initiator" runat="server" CommandName="Sort" CommandArgument="InitializedBy">col_Initiator</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblInitiator" Text='<%# DataBinder.Eval(Container, "DataItem.InitializedBy") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="InitializedWhen" HeaderText="col_Angelegt">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Angelegt" runat="server" CommandName="Sort" CommandArgument="InitializedWhen">col_Angelegt</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblAngelegt" Text='<%# DataBinder.Eval(Container, "DataItem.InitializedWhen") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="CustomerReference" HeaderText="col_Referenz">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Referenz" runat="server" CommandName="Sort" CommandArgument="CustomerReference">col_Referenz</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblReferenz" Text='<%# DataBinder.Eval(Container, "DataItem.CustomerReference") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ProcessReference" HeaderText="col_Merkmal1">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Merkmal1" runat="server" CommandName="Sort" CommandArgument="ProcessReference">col_Merkmal1</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblMerkmal1" Text='<%# DataBinder.Eval(Container, "DataItem.ProcessReference") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ProcessReference2" HeaderText="col_Merkmal2">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Merkmal2" runat="server" CommandName="Sort" CommandArgument="ProcessReference2">col_Merkmal2</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblMerkmal2" Text='<%# DataBinder.Eval(Container, "DataItem.ProcessReference2") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="BatchAuthorization" HeaderText="col_SammelAuth">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_SammelAuth" runat="server" CommandName="Sort" CommandArgument="BatchAuthorization">col_SammelAuth</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkBatchAuth" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.BatchAuthorization") %>'>
                                                        </asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Auswahl">
                                                    <ItemTemplate>
                                                        <table id="Table11" cellspacing="1" cellpadding="1" width="100%" border="0">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:LinkButton ID="Linkbutton1" runat="server" CssClass="StandardButtonSmall" Text="Autorisieren"
                                                                        CommandName="Autorisieren" CausesValidation="false" CommandArgument='<%# Container.DataItemIndex %>' >Autorisieren</asp:LinkButton>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:LinkButton ID="Linkbutton4" runat="server" CommandArgument='<%# Container.DataItemIndex %>' Visible='<%# DataBinder.Eval(Container, "DataItem.BatchAuthorization") %>'
                                                                        CssClass="StandardButtonStorno" Text="Löschen" CommandName="Loeschen" CausesValidation="False">Löschen</asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                            <asp:GridView ID="GridView2" runat="server" Width="100%" BackColor="White" PageSize="50"
                                headerCSS="tableHeader" bodyCSS="tableBody" CssClass="tableMain" bodyHeight="400"
                                AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False">
                                <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                <HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
                                <Columns>
                                    <asp:TemplateField SortExpression="AppID" Visible="false" HeaderText="AppID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAppID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AppID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="AppName" Visible="false" HeaderText="AppName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAppName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AppName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="AppURL" Visible="false" HeaderText="AppURL">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAppURL" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AppURL") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="AuthorizationID" Visible="false" HeaderText="AuthorizationID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAuthorizationID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AuthorizationID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="AppFriendlyName" HeaderText="Anwendung">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lbtnAnwendung" runat="server" CommandName="Sort" CommandArgument="AppFriendlyName">Anwendung</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblAnwendung" Text='<%# DataBinder.Eval(Container, "DataItem.AppFriendlyName") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="InitializedBy" HeaderText="col_Initiator2">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Initiator2" runat="server" CommandName="Sort" CommandArgument="InitializedBy">col_Initiator2</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblInitiator" Text='<%# DataBinder.Eval(Container, "DataItem.InitializedBy") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="InitializedWhen" HeaderText="col_Angelegt2">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Angelegt2" runat="server" CommandName="Sort" CommandArgument="InitializedWhen">col_Angelegt2</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblAngelegt" Text='<%# DataBinder.Eval(Container, "DataItem.InitializedWhen", "{0:d}") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="CustomerReference" HeaderText="col_Referenz2">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Referenz2" runat="server" CommandName="Sort" CommandArgument="CustomerReference">col_Referenz2</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblReferenz" Text='<%# DataBinder.Eval(Container, "DataItem.CustomerReference") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ProcessReference" HeaderText="col_ProcessReference">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Merkmal1" runat="server" CommandName="Sort" CommandArgument="ProcessReference">col_ProcessReference</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblProcessReference" Text='<%# DataBinder.Eval(Container, "DataItem.ProcessReference") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Ergebnis" SortExpression="Ergebnis" HeaderText="Ergebnis">
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </td>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblLegende" runat="server" Visible="False">*Für Händlereigene Zulassung: N=Neufahrzeug, S=Selbstfahrervermietfahrzeug, V=Vorführfahrzeug</asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="ShowScript" runat="server" visible="False">
            <td>

                <script language="Javascript">
						<!--                    //
                    function AutorisierenConfirm(Anwendung, Initiator, Angelegt, Haendler, Merkmal) {
                        var Check = window.confirm("Wollen Sie für dieses Fahrzeug wirklich den Status 'Bezahlt' setzen?\t\n\tFahrgestellnr.\t" + Fahrgest + "\t\n\tVertrag\t\t" + Vertrag + "\n\tKfz-Kennzeichen\t" + Kennzeichen + "\t\n\tOrdernummer\t" + Ordernummer + "\t\n\tAngefordert\t" + Angefordert + "\t\n\tVersendet\t" + Versendet);
                        return (Check);
                    }
						//-->
                </script>

            </td>
        </tr>
    </table>
    </form>
</body>
</html>
