<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report02Neu.aspx.vb" Inherits="AppInsurance.Report02Neu"
    MasterPageFile="../MasterPage/App.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                        Text="zurück" CausesValidation="false"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div id="paginationQuery">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td class="active">
                                                Abfrage starten
                                            </td>
                                            <td class="active" valign="top" align="right">
                                                <div id="queryImage">
                                                    <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="../../../Images/queryArrow.gif" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div id="TableQuery" style="margin-bottom: 10px">
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                <tr class="formquery">
                                    <td nowrap="nowrap" class="firstLeft active" colspan="4">
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        Verkehrsjahr:
                                    </td>
                                    <td nowrap="nowrap" valign="top" class="active" colspan="3"  width="100%">
                                        <asp:TextBox ID="txtVJahr" runat="server" Width="120px"  CssClass="InputTextbox"></asp:TextBox>
                                        <ajaxToolkit:MaskedEditExtender ID="txtVJahrMaskedEditExtender" runat="server" AutoComplete="False"
                                            ClearMaskOnLostFocus="true" Enabled="True" Mask="9999" TargetControlID="txtVJahr"
                                            MessageValidatorTip="False">
                                        </ajaxToolkit:MaskedEditExtender>
                                        <asp:RequiredFieldValidator ID="reqFieldValVersJahr" runat="server" ControlToValidate="txtVJahr"
                                            ErrorMessage="Eingabe des Verkehrsjahres erforderlich."></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        Agenturnr.:
                                    </td>
                                    <td class="active" colspan="3">
                                        <asp:TextBox ID="txtOrgNr" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                        <asp:Label ID="lblPlatzhaltersuche" runat="server" Height="16px">*(mit Platzhaltersuche)</asp:Label>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        Datum von:
                                    </td>
                                    <td class="active" colspan="1">
                                        <asp:TextBox CssClass="InputTextbox" runat="server" Width="83px" ToolTip="Datum von"
                                            ID="txtDateVon" MaxLength="10" autocomplete="off" />
                                        <ajaxToolkit:CalendarExtender ID="defaultCalendarExtenderVon" runat="server" TargetControlID="txtDateVon" />
                                    </td>
                                    <td class="active" nowrap="nowrap" style="padding-right: 5px; padding-left: 5px">
                                        bis:
                                    </td>
                                    <td class="active" width="100%">
                                        <asp:TextBox CssClass="InputTextbox" runat="server" Width="83px" ToolTip="Datum bis"
                                            ID="txtDateBis" MaxLength="10" autocomplete="off" />
                                        <ajaxToolkit:CalendarExtender ID="CE_DatumBis" runat="server" TargetControlID="txtDateBis" />
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        Kennzeichen:
                                    </td>
                                    <td class="active" colspan="3">
                                        <asp:TextBox ID="txtKennzeichen" runat="server" MaxLength="8" CssClass="InputTextbox"></asp:TextBox>
                                        &nbsp;*<asp:Label ID="Label1" runat="server" Text="(leer oder vollständiges Kennzeichen)"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td colspan="4">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td style="background-color: #dfdfdf;" colspan="4">
                                        &nbsp;
                                    </td>
                                </tr>
                        </table>
                    </div>
                    <div id="dataQueryFooter">
                        <asp:LinkButton ID="btnConfirm" runat="server" CssClass="TablebuttonLarge" Height="16px"
                            Width="130px">» Abfrage starten</asp:LinkButton>
                    </div>
                    <div id="DivPlaceholder" runat="server" style="height: 550px;">
                    </div>
                    <div id="Result" runat="Server" visible="false">
                        <div class="ExcelDiv">
                            <div align="right" class="rightPadding">
                                <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                <span class="ExcelSpan">
                                    <asp:HyperLink ID="lnkCreateExcel1" ForeColor="White" Target="_blank" runat="server">Excel runterladen</asp:HyperLink>
                                </span>
                            </div>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanelNavi" runat="server">
                            <ContentTemplate>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                    CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0" AllowPaging="True" GridLines="None"
                                                    PageSize="20" CssClass="gridview">
                                                    <PagerSettings Visible="false" />
                                                    <Columns>
                                                        <asp:BoundField DataField="VD-Bezirk" SortExpression="VD-Bezirk" HeaderText="VD-Bezirk">
                                                            <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                            <ItemStyle CssClass="BoundField" Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Name1" SortExpression="Name1" HeaderText="Name 1">
                                                            <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                            <ItemStyle CssClass="BoundField"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Name2" SortExpression="Name2" HeaderText="Name 2">
                                                            <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                            <ItemStyle CssClass="BoundField"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Auftragsnummer" SortExpression="Auftragsnummer" HeaderText="Auftragsnummer">
                                                            <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                            <ItemStyle CssClass="BoundField"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Versand am" DataFormatString="{0:dd.MM.yyyy}" SortExpression="Versand am"
                                                            HeaderText="Versand am">
                                                            <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                            <ItemStyle CssClass="BoundField"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Stueckzahl" SortExpression="Stueckzahl" HeaderText="Stueckzahl">
                                                            <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                            <ItemStyle CssClass="BoundField"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Kennz. anzeigen" ItemStyle-HorizontalAlign="Center"
                                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-VerticalAlign="Middle">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton CssClass="TablebuttonLarge" ID="btnAlleKennzAnzeigen" runat="server"
                                                                    CommandName="AlleKennzAnzeigen" Height="16px" Width="125px" ForeColor="#333333"
                                                                    Text="Alle Kennz. anzeigen" Font-Size="9px"></asp:LinkButton><br />
                                                                <asp:LinkButton CssClass="TablebuttonLarge" ID="btnKennzAnzeigen" runat="server"
                                                                    CommandName="KennzAnzeigen" Height="16px" Width="125px" ForeColor="#333333" Text="Kennz. anzeigen"
                                                                    Font-Size="9px"></asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkKennzAnzeigen" runat="server"></asp:CheckBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Adresse anzeigen" ItemStyle-HorizontalAlign="Center"
                                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-VerticalAlign="Middle">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="btnAlleAdressenAnzeigen" CommandName="AlleAdressenAnzeigen" runat="server"
                                                                    ForeColor="#333333" Text="Alle Adressen anzeigen" CssClass="TablebuttonLarge"
                                                                    Height="16px" Width="125px" Font-Size="9px"></asp:LinkButton><br />
                                                                <asp:LinkButton CssClass="TablebuttonLarge" ID="btnAdressenAnzeigen" runat="server"
                                                                    CommandName="AdressenAnzeigen" Height="16px" Width="125px" ForeColor="#333333"
                                                                    Text="Adressen anzeigen" Font-Size="9px"></asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkAdresseAnzeigen" runat="server"></asp:CheckBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="Tablehead" ForeColor="White" />
                                                    <AlternatingRowStyle BackColor="#DEE1E0"></AlternatingRowStyle>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr id="ShowScript" runat="server">
                                            <td>

                                                <script type="text/javascript">
										<!--                                                    //
                                                    // window.document.Form1.elements[window.document.Form1.length-3].focus();
										//-->

                                                </script>

                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div id="dataFooter">
                            &nbsp;</div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
