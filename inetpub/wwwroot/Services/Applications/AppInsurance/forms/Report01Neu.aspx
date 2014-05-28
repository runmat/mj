<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report01Neu.aspx.vb" Inherits="AppInsurance.Report01Neu"
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
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UPResult" UpdateMode="Conditional" runat="server">
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
                           
                            
                            <div id="TableQuery">
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                    <tr class="formquery">
                                        <td nowrap="nowrap" colspan="2" width="100%" class="firstLeft active">
                                            <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td nowrap="nowrap" class="firstLeft active">
                                            Verkehrsjahr:
                                        </td>
                                        <td class="active" width="100%">
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
                                        <td nowrap="nowrap" class="firstLeft active">
                                            Agenturnr.:
                                        </td>
                                        <td nowrap="nowrap" class="active">
                                            <asp:TextBox ID="txtOrgNr" runat="server" Width="120px" CssClass="InputTextbox"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblPlatzhaltersuche" runat="server">*(mit Platzhaltersuche)</asp:Label>
                                            <ajaxToolkit:MaskedEditExtender ID="txtOrgNr_MaskedEditExtender" runat="server" AutoComplete="False"
                                                ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True" Mask="CCCC-CCCC-C" TargetControlID="txtOrgNr"
                                                MessageValidatorTip="False" Filtered="1234567890*">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td colspan="2">
                                            <asp:TextBox ID="TextBox1" runat="server" Visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td style="background-color: #dfdfdf;" colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <div id="dataQueryFooter">
                                    <asp:LinkButton ID="btnConfirm" runat="server" CssClass="TablebuttonLarge" Height="16px"
                                        Width="130px">» Abfrage starten</asp:LinkButton>
                                </div>
                                    <div id="statistics">
                            <table id="tabStats" runat ="server" visible="false" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        Stand:
                                        <asp:Label ID="lblStand" runat="server" Width="116px"></asp:Label>
                                    </td>
                                    <td>
                                        Verkaufte Kennzeichen:
                                        <asp:Label ID="lblVerkaufteKennzeichen" runat="server" Width="65px"></asp:Label>
                                    </td>
                                    <td>
                                        Rückläufer:
                                        <asp:Label ID="lblRuecklaeufer" runat="server" Width="65px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Anzahl der Vermittler:
                                        <asp:Label ID="lblVermittlerAnzahl" runat="server" Width="57px"></asp:Label>
                                    </td>
                                    <td>
                                        Unverkaufte Kennzeichen:
                                        <asp:Label ID="lblUnverkaufteKennzeichen" runat="server" Width="65px"></asp:Label>
                                    </td>
                                    <td>
                                        Bestand DAD:
                                        <asp:Label ID="lblBestandDAD" runat="server" Width="116px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Anzahl der Kennzeichen:
                                        <asp:Label ID="lblKennzeichenGesamtbestand" runat="server" Width="65px"></asp:Label>
                                    </td>
                                    <td>
                                        Verlust/Storno:
                                        <asp:Label ID="lblVerlustKennzeichen" runat="server" Width="149px"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </div>                                
                            </div>



                                </ContentTemplate>
                    </asp:UpdatePanel>


                    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnConfirm" />
                            <asp:PostBackTrigger ControlID="lnkCreateExcel" />
                        </Triggers>
                        <ContentTemplate>
                               
                    <div id="Result" runat="Server" visible="false">

                        <div class="ExcelDiv">
                            <div align="right">
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" /><span class="ExcelSpan">
                                           <asp:LinkButton ID="lnkCreateExcel" runat="server" ForeColor="White">Excel herunterladen</asp:LinkButton></span>

                            </div>
                        </div>
                        <div id="pagination">
                            <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                        </div>
                        <div id="data">
                            <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False"
                                CellPadding="0" CellSpacing="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                AllowSorting="true" AllowPaging="True" CssClass="GridView" PageSize="20">
                                <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                <PagerSettings Visible="False" />
                                <RowStyle CssClass="ItemStyle" />
                                <EmptyDataRowStyle BackColor="#DFDFDF" />
                                <Columns>
                                    <asp:BoundField DataField="KUN_EXT_VM" SortExpression="KUN_EXT_VM" HeaderText="Agenturnr."
                                        ItemStyle-Wrap="False"></asp:BoundField>
                                    <asp:BoundField DataField="NAME1_VM" SortExpression="NAME1_VM" HeaderText="Name 1">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NAME2_VM" SortExpression="NAME2_VM" HeaderText="Name 2">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ANZ_GES" SortExpression="ANZ_GES" HeaderText="Kennz. Gesamt">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ANZ_VERK" SortExpression="ANZ_VERK" HeaderText="Kennz. Verkauft">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ANZ_UNVERK" SortExpression="ANZ_UNVERK" HeaderText="Kennz. Unverkauft">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ANZ_VERL" SortExpression="ANZ_VERL" HeaderText="Verlust/ Storno">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ANZ_RUECK" SortExpression="ANZ_RUECK" HeaderText="Kennz. R&#252;ckl&#228;ufer">
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Adresse anzeigen" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-VerticalAlign="Middle"
                                        ItemStyle-CssClass="BoundField" HeaderStyle-CssClass="BoundField">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="btnAdressenAnzeigen" CommandName="AdressenAnzeigen" runat="server"
                                                Text="» Adr. anzeigen" EnableTheming="True" Width="100px"></asp:LinkButton></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkAdresseAnzeigen" runat="server"></asp:CheckBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div id="dataFooter">
                            &nbsp;</div>
                    </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <asp:Literal ID="Literal1" runat="server"></asp:Literal></div>
        </div>
    </div>
</asp:Content>
