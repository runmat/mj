<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01_5.aspx.vb" Inherits="CKG.Components.ComCommon.Zulassung.Change01_5"
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:HyperLink ID="lnkFahrzeugsuche" runat="server" NavigateUrl="Change01.aspx">Fahrzeugsuche</asp:HyperLink>
                    <asp:HyperLink ID="lnkFahrzeugauswahl" runat="server" NavigateUrl="Change01_2.aspx">| Fahrzeugauswahl</asp:HyperLink>
                    <asp:HyperLink ID="lnkAdressen" runat="server" NavigateUrl="Change01_3.aspx">| Adressen/Zulassungsdaten</asp:HyperLink>
                    <asp:HyperLink ID="lnkWunschkennz" runat="server" NavigateUrl="Change01_4.aspx">| Wunschkennzeichen</asp:HyperLink>
                    <a class="active">| Absenden</a>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label" />
                            </h1>
                        </div>
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UP1">
                            <ContentTemplate>
                                <div id="TableQuery">
                                    <asp:Panel ID="pnlHalter" runat="server" Style="display: block">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td colspan="2" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                                    <span style="font-weight: bold">Halter:</span>
                                                </td>
                                            </tr>
                                            <tr class="formquery" id="trError" runat="server">
                                                <td colspan="2" class="firstLeft active">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="Label73" runat="server" Height="16px" Width="106px">Name:</asp:Label>
                                                </td>
                                                <td class="active" style="width: 100%">
                                                    <asp:Label ID="lblShName" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="Label17" runat="server" Width="106px">Straße, Nr:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:Label ID="lblShStrasse" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="Label18" runat="server" Width="106px">PLZ Ort:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:Label ID="lblShOrt" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="2">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlZulDaten" runat="server" Style="display: block">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td colspan="2" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                                    <span style="font-weight: bold">Zulassungsdaten:</span>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="Label52" runat="server" Width="150px">Zulassungsdatum:</asp:Label>
                                                </td>
                                                <td class="active" style="width: 100%">
                                                    <asp:Label ID="lblZulDat" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="tr_Zulassungsart" class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="Label2" runat="server" Width="150px">Zulassungsart:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:Label ID="lblZulassungsart" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="Label53" runat="server" Width="150px">Feinstaubplakette:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <span>
                                                        <asp:CheckBox ID="chkFeinstaub" runat="server" TabIndex="23" Enabled="False" /></span>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Tageszulassung:
                                                </td>
                                                <td class="active">
                                                    <span>
                                                        <asp:CheckBox ID="chkTagesZul" runat="server" Enabled="False" TabIndex="23" />
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="active" colspan="2">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlVersicherer" runat="server" Style="display: block">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td colspan="2" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                                    <span style="font-weight: bold">Versicherungsnehmer:</span>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="Label1" runat="server" Width="150px">Name:</asp:Label>
                                                </td>
                                                <td class="active" style="width: 100%">
                                                    <asp:Label ID="lblVersName" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery" id="tr1" runat="Server">
                                                <td class="firstLeft active" nowrap="nowrap">
                                                    <asp:Label ID="Label3" runat="server" Width="150px">Straße, Nr:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:Label ID="lblVersStrasse" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="Label5" runat="server" Width="150px">PLZ Ort:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:Label ID="lblVersOrt" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="Label102" runat="server" Width="135px">Versicherung:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:Label ID="lblVersGesellschaft" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="Label59" runat="server" Width="135px">eVB-Nummer:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:Label ID="lblEvbNr" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="Label60" runat="server" Width="135px">eVB-von:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:Label ID="lblEvbVon" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="Label56" runat="server" Width="135px">eVB-bis:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:Label ID="lblEvbBis" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="2">
                                                    <asp:Label ID="lblAnzahl" CssClass="TextError" Font-Size="10pt" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="active" colspan="2">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </div>
                                <div id="Result" runat="Server">
                                    <div class="ExcelDiv">
                                        <div align="right" class="rightPadding">
                                            &nbsp;</div>
                                    </div>
                                    <div id="pagination">
                                        <uc2:GridNavigation ID="GridNavigation1" runat="server">
                                        </uc2:GridNavigation>
                                    </div>
                                    <div id="data">
                                        <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                                            CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True">
                                            <PagerSettings Visible="False" />
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <EditRowStyle></EditRowStyle>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Equipment" SortExpression="Equnr" Visible="False">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("EQUNR") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEqunr" runat="server" Text='<%# Bind("EQUNR") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="CHASSIS_NUM" HeaderText="col_Fahrgestellnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="CHASSIS_NUM">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CHASSIS_NUM") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ZZHERST_TEXT" HeaderText="col_Hersteller">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Hersteller" runat="server" CommandName="Sort" CommandArgument="ZZHERST_TEXT">col_Hersteller</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZHERST_TEXT") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ZZKLARTEXT_TYP" HeaderText="col_Typ">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Typ" runat="server" CommandName="Sort" CommandArgument="ZZKLARTEXT_TYP">col_Typ</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZKLARTEXT_TYP") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Wunschkennz1" HeaderText="col_Wunschkennz1">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Wunschkennz1" runat="server" CommandName="Sort" CommandArgument="Wunschkennz1">col_Wunschkennz1</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWunschkennz1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Wunschkennz1") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Wunschkennz2" HeaderText="col_Wunschkennz2">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Wunschkennz2" runat="server" CommandName="Sort" CommandArgument="Wunschkennz2">col_Wunschkennz2</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWunschkennz2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Wunschkennz2") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Wunschkennz3" HeaderText="col_Wunschkennz3">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Wunschkennz3" runat="server" CommandName="Sort" CommandArgument="Wunschkennz3">col_Wunschkennz3</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWunschkennz3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Wunschkennz3") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ResNr" HeaderText="col_ResNr">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_ResNr" runat="server" CommandName="Sort" CommandArgument="ResNr">col_ResNr</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblResNr" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ResNr") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ResName" HeaderText="col_ResName">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_ResName" runat="server" CommandName="Sort" CommandArgument="ResName">col_ResName</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblResName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ResName") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Auftragsnr" HeaderText="col_Auftragnr" Visible="false">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Auftragnr" runat="server" CommandName="Sort" CommandArgument="Auftragsnr">col_Auftragnr</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label7" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Auftragsnr") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="STATUS" HeaderText="col_Status" Visible="false">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Status" runat="server" CommandName="Sort" CommandArgument="STATUS">col_Status</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label7" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Bem") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div id="dataFooter">
                                        <asp:LinkButton ID="cmdContinue" runat="server" CssClass="Tablebutton" Width="78px">» Absenden</asp:LinkButton>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
