<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report01s.aspx.vb" Inherits="CKG.Components.ComCommon.Report01s1"
    MasterPageFile="../../../MasterPage/Services.Master" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .Watermark
        {
            color: Gray;
        }
    </style>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
                            <asp:PostBackTrigger ControlID="lnkCreatePDF1" />
                        </Triggers>
                        <ContentTemplate>
                            <div id="paginationQuery">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td class="active">
                                                Neue Abfrage starten
                                            </td>
                                            <td align="right">
                                                <div id="queryImage">
                                                    <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="../../../Images/queryArrow.gif" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <asp:Panel ID="Panel1" runat="server">
                                <div id="TableQuery" style="margin-bottom: 10px">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="2">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                                </td>
                                                <td style="width: 100%">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Prozess:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:DropDownList ID="ddlPlangruppenzähler" runat="server" Width="280px" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlPlangruppenzähler_SelectedIndexChanged" />
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="2" style="font-weight: bold">
                                                    <asp:RadioButton runat="server" BorderColor="Transparent" Text="Qualifizierte Suche - Einzelvorgang:"
                                                        ID="rbEinzelsuche" AutoPostBack="true" GroupName="rblSuche" Checked="true" />
                                                </td>
                                            </tr>
                                            <tr class="formquery" id="Einzelsuche1">
                                                <td class="firstLeft active" colspan="2" style="font-weight: bold">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery" id="Einzelsuche2">
                                                <td class="firstLeft active">
                                                    Vertragsnummer:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtVertragsnummer" runat="server" CssClass="TextBoxNormal" MaxLength="17"
                                                        Width="150px"></asp:TextBox>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery" id="Einzelsuche3">
                                                <td class="firstLeft active">
                                                    Fahrgestellnummer:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtFahrgestellnummer" runat="server" CssClass="TextBoxNormal" MaxLength="17"
                                                        Width="150px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery" id="Einzelsuche4">
                                                <td class="firstLeft active">
                                                    Fahrername(Halter):
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtHalter" runat="server" CssClass="InputTextbox" Width="150px"
                                                        MaxLength="100"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery" id="Einzelsuche5">
                                                <td class="firstLeft active">
                                                    Mitarbeiternummer:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtMitarbeiter" runat="server" CssClass="InputTextbox" Width="150px"
                                                        MaxLength="10"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                                <td class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="2" style="font-weight: bold">
                                                    <asp:RadioButton runat="server" BorderColor="Transparent" Text="Mehrfachsuche - Selektion:"
                                                        ID="rbMehrfachsuche" AutoPostBack="true" GroupName="rblSuche" Checked="false" />
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="2" style="font-weight: bold">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery" id="Mehrfachsuche1">
                                                <td class="firstLeft active">
                                                    Dienstleistung erledigt:
                                                </td>
                                                <td class="firstLeft active">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="ddlBedingungPos" runat="server" Width="280px">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td style="padding-left: 20px">
                                                                <asp:DropDownList ID="ddlOperator" runat="server">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td style="padding-left: 20px">
                                                                Datum:
                                                            </td>
                                                            <td style="padding-left: 20px">
                                                                <asp:TextBox ID="txtDatum" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                                <ajaxToolkit:CalendarExtender ID="txtDatum_CalendarExtender" runat="server" Enabled="True"
                                                                    TargetControlID="txtDatum">
                                                                </ajaxToolkit:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr class="formquery" id="Mehrfachsuche2">
                                                <td class="firstLeft active">
                                                    Dienstleistung offen:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:DropDownList ID="ddlBedingungNeg" runat="server" Width="280px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr class="formquery" id="trKundenstatus" runat="server" visible="false">
                                                <td class="firstLeft active">
                                                    Kundenstatus:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtKundenStatus" runat="server" Width="270px"></asp:TextBox>
                                                    <ajaxToolkit:TextBoxWatermarkExtender ID="txtKundenStatus_TextBoxWatermarkExtender"
                                                        runat="server" Enabled="True" TargetControlID="txtKundenStatus" WatermarkCssClass="Watermark"
                                                        WatermarkText="Beispiel: A,B,C,D">
                                                    </ajaxToolkit:TextBoxWatermarkExtender>
                                                </td>
                                            </tr>
                                            <tr class="formquery" id="Mehrfachsuche3">
                                                <td class="firstLeft active" colspan="2">
                                                    <asp:RadioButton ID="rdbAlleVorgaenge" runat="server" Checked="True" GroupName="grpVorgaenge"
                                                        Text="alle Vorgänge" Visible="False" />
                                                    <asp:RadioButton ID="rdbOffVorgaenge" runat="server" GroupName="grpVorgaenge" Text="offene Vorgänge"
                                                        Visible="False" />
                                                    <asp:RadioButton ID="rdbErlVorgaenge" runat="server" GroupName="grpVorgaenge" Text="erledigte Vorgänge"
                                                        Visible="False" />
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="2">
                                                    <span>
                                                        <asp:CheckBox ID="chkOffene" runat="server" Checked="true" Text="nur offene Rückmeldungen oder Teilrückmeldungen" />
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                                <td class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td colspan="2">
                                                    <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
                                                        Width="1px" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>
                                </div>
                            </asp:Panel>
                            <div id="dataQueryFooter">
                                <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">» Suchen </asp:LinkButton>
                            </div>
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel1" ForeColor="White" runat="server">Excel herunterladen</asp:LinkButton>
                                        </span>
                                        <img src="../../../Images/iconPDF.gif" alt="PDF herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreatePDF1" ForeColor="White" runat="server">Prozessablauf</asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                        <tr>
                                            <td style="padding-left: 15px">
                                                <asp:Label runat="server" ID="lblLegende" ForeColor="Orange" Font-Bold="true">Teilrückmeldung (*TR*)</asp:Label>
                                                &nbsp;<asp:Label runat="server" ID="lblLegende2" ForeColor="#3366CC" Font-Bold="True">Prognosedatum (*PD*)</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvBestand" Width="100%" runat="server" CellPadding="1" CellSpacing="1"
                                                    GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0" AllowSorting="True"
                                                    AllowPaging="True" PageSize="20" EnableModelValidation="True">
                                                    <HeaderStyle BackColor="#9b9b9b" ForeColor="White" Height="30px" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                    <PagerSettings Visible="False" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgbRueckmeldungErfassen" ImageAlign="Middle" runat="server"
                                                                    Width="20" Height="20" ImageUrl="~/Images/Edit_02.jpg" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Auftragsnummer") %>'
                                                                    CommandName="RueckmeldungErfassen" ToolTip="Rückmeldung erfassen" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ibtAdresse" runat="server" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Auftragsnummer") %>'
                                                                    CommandName="Adresse" Height="20px" ImageUrl="/services/Images/Lupe_16x16.gif"
                                                                    ToolTip="Details anzeigen" Width="20px" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Status">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="True" Width="40px"
                                                                    OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                                                    <asp:ListItem Value="0" Text=""></asp:ListItem>
                                                                    <asp:ListItem>A</asp:ListItem>
                                                                    <asp:ListItem>B</asp:ListItem>
                                                                    <asp:ListItem>C</asp:ListItem>
                                                                    <asp:ListItem>D</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:Label ID="lblFin" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'></asp:Label>
                                                                <asp:Label ID="lblAuf" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.Auftragsnummer") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div id="dataFooter">
                                &nbsp;</div>
                            <div>
                                <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                                <asp:Button ID="Button1" runat="server" Text="BUTTON" OnClick="Button1_Click" Visible="False" />
                                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnFake"
                                    PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="true" CancelControlID="btnOK">
                                </ajaxToolkit:ModalPopupExtender>
                                <asp:Panel ID="mb" runat="server" Width="400px" Height="180px" BackColor="White"
                                    Style="display: none">
                                    <div style="padding: 20px 20px 20px 20px">
                                        <table>
                                            <tr>
                                                <td>
                                                    Mitarbeiternr:
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblMitarbeiternr" runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Haltername:
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblHaltername" runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Strasse:
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblHalterStrasse" runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    PLZ/Ort:
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblHalterPLZ" runat="server" Font-Bold="True"></asp:Label>/<asp:Label
                                                        ID="lblHalterOrt" runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Telefon:
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblHalterTelefon" runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    E-Mail:
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblHalterEMail" runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div style="text-align: center">
                                        <asp:Button ID="btnOK" runat="server" CssClass="TablebuttonLarge" Font-Bold="true"
                                            Text="Schließen" />
                                    </div>
                                </asp:Panel>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
