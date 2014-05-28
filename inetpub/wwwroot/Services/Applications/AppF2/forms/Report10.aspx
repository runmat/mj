<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report10.aspx.vb" Inherits="AppF2.Report10"
  MasterPageFile="../MasterPage/AppMaster.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="/Services/PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
    <div id="content">
      <div id="navigationSubmenu">
        &nbsp;<asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
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
            <%--            <Triggers>
              <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
            </Triggers>
            --%>
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
                          <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="../../../Images/queryArrow.gif"
                            ToolTip="Abfrage öffnen" OnClick="NewSearch_Click" Visible="false" />
                          <asp:ImageButton ID="NewSearchUp" runat="server" ToolTip="Abfrage schließen" ImageUrl="../../../Images/queryArrowUp.gif"
                            Visible="false" OnClick="NewSearchUp_Click" />
                        </div>
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>
              <asp:Panel ID="Panel1" runat="server" DefaultButton="lbCreate">
                <div id="TableQuery" style="margin-bottom: 10px">
                  <table id="tab1" runat="server" cellpadding="0" cellspacing="0" width="100%">
                    <tbody>
                      <tr class="formquery">
                        <td class="firstLeft active" colspan="2">
                          <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                          <asp:Label ID="lblNoData" runat="server" Font-Bold="True" Visible="False" EnableViewState="False"></asp:Label>
                        </td>
                      </tr>
                      <tr class="formquery">
                        <td class="firstLeft active" style="width: 100px">
                          Kontonummer:
                        </td>
                        <td class="firstLeft active">
                          <asp:TextBox ID="txtKontonummer" runat="server" CssClass="long" />
                        </td>
                      </tr>
                      <tr class="formquery">
                        <td class="firstLeft active" style="width: 100px">
                          Paid-Nummer:
                        </td>
                        <td class="firstLeft active">
                          <asp:TextBox ID="txtPaidNummer" runat="server" CssClass="long" />
                        </td>
                      </tr>
                      <tr class="formquery">
                        <td class="firstLeft active" style="width: 100px">
                          CIN:
                        </td>
                        <td class="firstLeft active">
                          <asp:TextBox ID="txtCIN" runat="server" CssClass="long" />
                        </td>
                      </tr>
                      <tr class="formquery">
                        <td class="firstLeft active" style="width: 100px">
                          Name:
                        </td>
                        <td class="firstLeft active">
                          <asp:TextBox ID="txtName" runat="server" CssClass="long" />
                        </td>
                      </tr>
                      <tr class="formquery">
                        <td class="firstLeft active" style="width: 100px">
                          Vertragsart:
                        </td>
                        <td class="firstLeft active">
                          <asp:DropDownList ID="ddlVertragsart" runat="server" CssClass="DropDownLarge"/>
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
                <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px"
                  OnClick="lbCreate_Click">» Suchen </asp:LinkButton>
              </div>
              <div id="Result" runat="Server" visible="false">
                <%--                <div class="ExcelDiv">
                  <div align="right" class="rightPadding">
                    <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                    <span class="ExcelSpan">
                      <asp:LinkButton ID="lnkCreateExcel1" runat="server" OnClick="lnkCreateExcel1_Click">Excel herunterladen</asp:LinkButton>
                    </span>
                  </div>
                </div>
                --%>
                <div style="background-color: rgb(223, 223, 223); clear: both;">
                  <table cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 15px; padding-right: 17px;
                    color: #4c4c4c;">
                    <tbody>
                      <tr>
                        <td class="active">
                          Ergebnisliste
                        </td>
                        <td align="right">
                          <div>
                            <asp:ImageButton ID="ibShowList" runat="server" ImageUrl="../../../Images/queryArrow.gif"
                              ToolTip="Liste öffnen" Visible="true" OnClick="ShowListClick" />
                            <asp:ImageButton ID="ibHideList" runat="server" ToolTip="Liste schließen" ImageUrl="../../../Images/queryArrowUp.gif"
                              Visible="false" OnClick="HideListClick" />
                          </div>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
                <div id="pagination">
                  <uc2:GridNavigation ID="GridNavigation1" runat="server" OnPagerChanged="GridView1_PageIndexChanged"
                    OnPageSizeChanged="GridView1_ddlPageSizeChanged"></uc2:GridNavigation>
                </div>
                <div id="data">
                  <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                    <tr>
                      <td>
                        <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                          CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True"
                          OnSorting="GridView1_Sorting" OnRowCommand="GridView1_RowCommand">
                          <PagerSettings Visible="False" />
                          <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                          <AlternatingRowStyle CssClass="GridTableAlternate" />
                          <RowStyle CssClass="ItemStyle" />
                          <Columns>
                            <asp:TemplateField HeaderText="col_Details">
                              <HeaderTemplate>
                                <asp:LinkButton ID="col_Details" runat="server" Text="col_Details" />
                              </HeaderTemplate>
                              <ItemTemplate>
                                <asp:ImageButton ID="ibDetails" runat="server" CommandName="Details" CommandArgument='<%# DataBinder.Eval(Container, "DataItemIndex") %>'
                                  ImageUrl="/Services/Images/Lupe_16x16.gif" />
                              </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="KONTONR" HeaderText="col_Kontonummer">
                              <HeaderTemplate>
                                <asp:LinkButton ID="col_Kontonummer" runat="server" CommandName="Sort" CommandArgument="KONTONR">col_Kontonummer</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                <asp:Label runat="server" ID="lblKontonummer" Text='<%# Eval("KONTONR") %>'>
                                </asp:Label>
                              </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="PAID" HeaderText="col_PaidNummer">
                              <HeaderTemplate>
                                <asp:LinkButton ID="col_PaidNummer" runat="server" CommandName="Sort" CommandArgument="PAID">col_PaidNummer</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                <asp:Label runat="server" ID="lblPaidNummer" Text='<%# Eval("PAID") %>'>
                                </asp:Label>
                              </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="CIN" HeaderText="col_CIN">
                              <HeaderTemplate>
                                <asp:LinkButton ID="col_CIN" runat="server" CommandName="Sort" CommandArgument="CIN">col_CIN</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                <asp:Label runat="server" ID="lblCIN" Text='<%# Eval("CIN") %>'>
                                </asp:Label>
                              </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="ZVERT_ART" HeaderText="col_Vertragsart">
                              <HeaderTemplate>
                                <asp:LinkButton ID="col_Vertragsart" runat="server" CommandName="Sort" CommandArgument="ZVERT_ART">col_Vertragsart</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                <asp:Label runat="server" ID="lblVertragsart" Text='<%# Eval("ZVERT_ART") %>'>
                                </asp:Label>
                              </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="NAME_KRED" HeaderText="col_Name">
                              <HeaderTemplate>
                                <asp:LinkButton ID="col_Name" runat="server" CommandName="Sort" CommandArgument="NAME_KRED">col_Name</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                <asp:Label runat="server" ID="lblName" Text='<%# Eval("NAME_KRED") %>'>
                                </asp:Label>
                              </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="HAENDL_VERK_WERT" HeaderText="col_VkWert">
                              <HeaderTemplate>
                                <asp:LinkButton ID="col_VkWert" runat="server" CommandName="Sort" CommandArgument="HAENDL_VERK_WERT">col_VkWert</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                <asp:Label runat="server" ID="lblVkWert" Text='<%# Eval("HAENDL_VERK_WERT") %>'>
                                </asp:Label>
                              </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="PRUEFDAT_VERK_WERT" HeaderText="col_VkDatum">
                              <HeaderTemplate>
                                <asp:LinkButton ID="col_VkDatum" runat="server" CommandName="Sort" CommandArgument="PRUEFDAT_VERK_WERT">col_VkDatum</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                <asp:Label runat="server" ID="lblVkDatum" Text='<%# Eval("PRUEFDAT_VERK_WERT", "{0:d}") %>'>
                                </asp:Label>
                              </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="HAENDL_EINK_WERT" HeaderText="col_EkWert">
                              <HeaderTemplate>
                                <asp:LinkButton ID="col_EkWert" runat="server" CommandName="Sort" CommandArgument="HAENDL_EINK_WERT">col_EkWert</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                <asp:Label runat="server" ID="lblEkWert" Text='<%# Eval("HAENDL_EINK_WERT") %>'>
                                </asp:Label>
                              </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="PRUEFDAT_EINK_WER" HeaderText="col_EkDatum">
                              <HeaderTemplate>
                                <asp:LinkButton ID="col_EkDatum" runat="server" CommandName="Sort" CommandArgument="PRUEFDAT_EINK_WER">col_EkDatum</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                <asp:Label runat="server" ID="lblEkDatum" Text='<%# Eval("PRUEFDAT_EINK_WER", "{0:d}") %>'>
                                </asp:Label>
                              </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="CHASSIS_NUM" HeaderText="col_Vin">
                              <HeaderTemplate>
                                <asp:LinkButton ID="col_Vin" runat="server" CommandName="Sort" CommandArgument="CHASSIS_NUM">col_Vin</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                <asp:Label runat="server" ID="lblVin" Text='<%# Eval("CHASSIS_NUM") %>'>
                                </asp:Label>
                              </ItemTemplate>
                            </asp:TemplateField>
                          </Columns>
                        </asp:GridView>
                        <asp:HiddenField ID="hField" runat="server" Value="0" />
                      </td>
                    </tr>
                  </table>
                </div>
                <asp:FormView ID="FormView1" runat="server" Width="100%">
                  <ItemTemplate>
                    <table cellpadding="4" cellspacing="0" style="width: 100%;">
                      <tr style="font-weight: bold;">
                        <td style="width: 8%;">
                          Kontonummer:
                        </td>
                        <td style="width: 18%;">
                          <%# Eval("KONTONR")%>
                        </td>
                        <td style="width: 6%;">
                          CIN:
                        </td>
                        <td style="width: 17%;">
                          <%# Eval("CIN")%>
                        </td>
                        <td style="width: 8%;">
                          Vertragsart:
                        </td>
                        <td style="width: 12%;">
                          <%# Eval("ZVERT_ART")%>
                        </td>
                        <td style="width: 13%;">
                          Paid-Nummer:
                        </td>
                        <td style="width: 18%;">
                          <%# Eval("PAID")%>
                        </td>
                      </tr>
                    </table>
                    <table cellpadding="4" cellspacing="0" style="width: 100%;">
                      <tr>
                        <th colspan="2" style="text-align: left;">
                          Kreditnehmer I
                          <asp:LinkButton ID="lbKreditnehmer2" runat="server" Text="Kreditnehmer II (Mitschuldner)"
                            Visible='<%# Not String.IsNullOrempty(Eval("NAME_KRED2.Length")) %>' Style="font-weight: normal;
                            text-decoration: underline;" />
                          <ajaxToolkit:ModalPopupExtender ID="mpeKreditnehmer2" runat="server" TargetControlID="lbKreditnehmer2"
                            PopupControlID="pKreditnehmer2" CancelControlID="lbSchließen2" BackgroundCssClass="divProgress" />
                          <asp:Panel ID="pKreditnehmer2" runat="server" CssClass="divPopupDetail" HorizontalAlign="Left"
                            Style="padding: 15px; border: 1px solid black; background-color: White;">
                            <table cellpadding="0" cellspacing="0" style="border: 1px solid black; margin-bottom: 8px;
                              border-collapse: collapse;">
                              <tr style="background-color: #CCCCCC;">
                                <th colspan="2" style="text-align: left; padding: 4px; border: 1px solid black;">
                                  Kreditnehmer II (Mitschuldner)
                                </th>
                              </tr>
                              <tr>
                                <td style="padding: 4px;">
                                  Name:
                                </td>
                                <td style="padding: 4px;">
                                  <asp:TextBox ID="txtNameKred2" runat="server" Text='<%# Eval("NAME_KRED2") %>' Enabled="false"
                                    CssClass="long" />
                                </td>
                              </tr>
                              <tr>
                                <td style="padding: 4px;">
                                  Straße / Haus-NR.:
                                </td>
                                <td style="padding: 4px;">
                                  <asp:TextBox ID="txtStraßeKred2" runat="server" Text='<%# Eval("STREET_KRED2") %>'
                                    Enabled="false" CssClass="long" />
                                </td>
                              </tr>
                              <tr>
                                <td style="padding: 4px;">
                                  Postleitzahl / Ort:
                                </td>
                                <td style="padding: 4px;">
                                  <asp:TextBox ID="txtOrtKred2" runat="server" Text='<%# Eval("CITY1_KRED2") %>' Enabled="false"
                                    CssClass="long" />
                                </td>
                              </tr>
                            </table>
                            <asp:LinkButton ID="lbSchließen2" Text="Schließen" runat="server" />
                          </asp:Panel>
                        </th>
                        <th colspan="2" style="text-align: left;">
                          Fahrzeughalter
                        </th>
                      </tr>
                      <tr>
                        <td>
                          Name:
                        </td>
                        <td>
                          <asp:TextBox ID="txtNameKred" runat="server" Text='<%# Eval("NAME_KRED") %>' Enabled="false"
                            CssClass="long" />
                        </td>
                        <td>
                          Name:
                        </td>
                        <td>
                          <asp:TextBox ID="txtNameFh" runat="server" Text='<%# Eval("NAME_FH") %>' Enabled="false"
                            CssClass="long" />
                        </td>
                      </tr>
                      <tr>
                        <td>
                          Vorname:
                        </td>
                        <td>
                          <asp:TextBox ID="txtVornameKred" runat="server" Text='' Enabled="false" CssClass="long" />
                        </td>
                        <td>
                          Vorname:
                        </td>
                        <td>
                          <asp:TextBox ID="txtVornameFH" runat="server" Text='' Enabled="false" CssClass="long" />
                        </td>
                      </tr>
                      <tr>
                        <td>
                          Straße / Haus-NR.:
                        </td>
                        <td>
                          <asp:TextBox ID="txtStraßeKred" runat="server" Text='<%# Eval("STREET_KRED") %>'
                            Enabled="false" CssClass="long" />
                        </td>
                        <td>
                          Straße / Haus-NR.:
                        </td>
                        <td>
                          <asp:TextBox ID="txtStraßeFh" runat="server" Text='<%# Eval("STREET_FH") %>' Enabled="false"
                            CssClass="long" />
                        </td>
                      </tr>
                      <tr>
                        <td>
                          Postleitzahl / Ort:
                        </td>
                        <td>
                          <asp:TextBox ID="txtOrtKred" runat="server" Text='<%# Eval("CITY1_KRED") %>' Enabled="false"
                            CssClass="long" />
                        </td>
                        <td>
                          Postleitzahl / Ort:
                        </td>
                        <td>
                          <asp:TextBox ID="txtOrtFh" runat="server" Text='<%# Eval("CITY1_FH") %>' Enabled="false"
                            CssClass="long" />
                        </td>
                      </tr>
                      <tr>
                        <td colspan="4" style="border-bottom: 2px solid grey;">
                          &nbsp;
                        </td>
                      </tr>
                      <tr>
                        <th colspan="2" style="text-align: left;">
                          Historische Daten
                        </th>
                        <th colspan="2" style="text-align: left;">
                          Fahrzeugwerte
                        </th>
                      </tr>
                      <tr>
                        <td>
                          Dateneingan:
                        </td>
                        <td>
                          <%# Eval("DAT_ERST_IMP", "{0:d}")%>
                        </td>
                        <td>
                          Händlereinkauf:
                        </td>
                        <td>
                          <%# Eval("PRUEFDAT_EINK_WER", "{0:d}")%>
                          &#8194; &#8194;
                          <%# Eval("HAENDL_EINK_WERT")%>
                        </td>
                      </tr>
                      <tr>
                        <td>
                          Änderungsdatum:
                        </td>
                        <td>
                          <%# Eval("AENDAT", "{0:d}")%>
                        </td>
                        <td>
                          Händlerverkauf:
                        </td>
                        <td>
                          <%# Eval("PRUEFDAT_VERK_WERT", "{0:d}")%>
                          &#8194; &#8194;
                          <%# Eval("HAENDL_VERK_WERT")%>
                        </td>
                      </tr>
                      <tr>
                        <td style="vertical-align: top;">&nbsp;</td>
                        <td style="vertical-align: top;">
                            <div>
                                <span id="Span2" runat="server" Visible='<%# Eval("NotizAvailable") %>' style="display:inline-block; text-align: left;">
                                    
                                    <table>
                                        <tr>
                                            <td><img src="/Services/Images/note.png" alt="Notizen" title="Notizen" /></td>
                                            <td><asp:LinkButton ID="lbNotiz" runat="server" Text="Notizen" Style="text-decoration: underline;" /></td>
                                        </tr>
                                    </table>

                                      <ajaxToolkit:ModalPopupExtender ID="muNotiz" runat="server" TargetControlID="lbNotiz"
                                        PopupControlID="pNotiz" CancelControlID="lbNotizSchließen" BackgroundCssClass="divProgress" />
                                      <asp:Panel ID="pNotiz" runat="server" CssClass="divPopupDetail" HorizontalAlign="Left" Style="padding: 15px; border: 1px solid black; background-color: White;">
                                        
                                            <asp:Repeater ID="rbNotizen" runat="server" DataSource='<%# Eval("Notizen") %>'>
                                              <HeaderTemplate>
                                                <table cellspacing="0" cellpadding="0" style="border: 1px solid black; margin-bottom: 8px;
                                                  border-collapse: collapse;">
                                                  <tr style="background-color: #CCCCCC;">
                                                    <th style="text-align: left; padding: 4px; border: 1px solid black;">
                                                      Datum
                                                    </th>
                                                    <th style="text-align: left; padding: 4px; border: 1px solid black;">
                                                      User
                                                    </th>
                                                    <th style="text-align: left; padding: 4px; border: 1px solid black;">
                                                      Herkunft
                                                    </th>
                                                    <th style="text-align: left; padding: 4px; border: 1px solid black;">
                                                      Gesprächspartner
                                                    </th>
                                                    <th style="text-align: left; padding: 4px; border: 1px solid black;">
                                                      Notiz
                                                    </th>
                                                  </tr>
                                              </HeaderTemplate>
                                              <FooterTemplate>
                                                </table>
                                              </FooterTemplate>
                                              <ItemTemplate>
                                                <tr>
                                                  <td style="text-align: left; padding: 4px; border: 1px solid black;">
                                                    <%# Eval("Datum")%>
                                                  </td>
                                                  <td style="text-align: right; padding: 4px; border: 1px solid black;">
                                                    <%# Eval("User")%>
                                                  </td>
                                                  <td style="text-align: right; padding: 4px; border: 1px solid black;">
                                                    <%# Eval("Herkunft")%>
                                                  </td>
                                                  <td style="text-align: right; padding: 4px; border: 1px solid black;">
                                                    <%# Eval("Gespraechspartner")%>
                                                  </td>
                                                  <td style="text-align: right; padding: 4px; border: 1px solid black;">
                                                    <%# Eval("Notiz")%>
                                                  </td>
                                                </tr>
                                              </ItemTemplate>
                                            </asp:Repeater>
                                            <asp:LinkButton ID="lbNotizSchließen" Text="Schließen" runat="server" />

                                      </asp:Panel>
                                </span>
                            </div>
                        </td>

                        <td colspan="2" style="vertical-align: top;">
                          <asp:LinkButton ID="lbFahrzeugwertverlauf" runat="server" Text="Fahrzeugwertverlauf"
                            Enabled='<%# Eval("Fahrzeugwertverlauf.Rows.Count") > 0 %>' Style="text-decoration: underline;" />
                          <ajaxToolkit:ModalPopupExtender ID="mpeFahrzeugwertverlauf" runat="server" TargetControlID="lbFahrzeugwertverlauf"
                            PopupControlID="pFahrzeugwertverlauf" CancelControlID="lbSchließen" BackgroundCssClass="divProgress" />
                          <asp:Panel ID="pFahrzeugwertverlauf" runat="server" CssClass="divPopupDetail" HorizontalAlign="Left"
                            Style="padding: 15px; border: 1px solid black; background-color: White;">
                            <asp:Repeater ID="rbFahrzeugwertverlauf" runat="server" DataSource='<%# Eval("Fahrzeugwertverlauf") %>'>
                              <HeaderTemplate>
                                <table cellspacing="0" cellpadding="0" style="border: 1px solid black; margin-bottom: 8px;
                                  border-collapse: collapse;">
                                  <tr style="background-color: #CCCCCC;">
                                    <th style="text-align: left; padding: 4px; border: 1px solid black;">
                                      Datum
                                    </th>
                                    <th style="text-align: left; padding: 4px; border: 1px solid black;">
                                      HEK netto
                                    </th>
                                    <th style="text-align: left; padding: 4px; border: 1px solid black;">
                                      HVK netto
                                    </th>
                                    <th style="text-align: left; padding: 4px; border: 1px solid black;">
                                      HEK brutto
                                    </th>
                                    <th style="text-align: left; padding: 4px; border: 1px solid black;">
                                      HVK brutto
                                    </th>
                                    <th style="text-align: left; padding: 4px; border: 1px solid black;">
                                      Bezugs-<br/>strecke
                                    </th>
                                    <th style="text-align: left; padding: 4px; border: 1px solid black;">
                                      Abfrage-<br/>art
                                    </th>
                                    <th style="text-align: left; padding: 4px; border: 1px solid black;">
                                      Bewertungs-<br/>art
                                    </th>
                                    <th style="text-align: left; padding: 4px; border: 1px solid black;">
                                      Monate<br/>Abschreibung
                                    </th>
                                    <th style="text-align: left; padding: 4px; border: 1px solid black;">
                                      Abschreibungs-<br/>betrag EK
                                    </th>
                                    <th style="text-align: left; padding: 4px; border: 1px solid black;">
                                      Abschreibungs-<br/>betrag VK
                                    </th>
                                    <th style="text-align: left; padding: 4px; border: 1px solid black;">
                                      EkF-Basispr.<br/>netto
                                    </th>
                                    <th style="text-align: left; padding: 4px; border: 1px solid black;">
                                      Kosten
                                    </th>
                                    <th style="text-align: left; padding: 4px; border: 1px solid black;">
                                      zweite<br/>FB2?
                                    </th>
                                  </tr>
                              </HeaderTemplate>
                              <FooterTemplate>
                                </table>
                              </FooterTemplate>
                              <ItemTemplate>
                                <tr>
                                  <td style="text-align: left; padding: 4px; border: 1px solid black;">
                                    <%# Eval("PRUEFDAT", "{0:d}")%>
                                  </td>
                                  <td style="text-align: right; padding: 4px; border: 1px solid black;">
                                    <%# Eval("HAENDL_EINK_WERT")%>
                                  </td>
                                  <td style="text-align: right; padding: 4px; border: 1px solid black;">
                                    <%# Eval("HAENDL_VERK_WERT")%>
                                  </td>
                                  <td style="text-align: right; padding: 4px; border: 1px solid black;">
                                    <%# Eval("HAENDL_EINK_WERT_BRU")%>
                                  </td>
                                  <td style="text-align: right; padding: 4px; border: 1px solid black;">
                                    <%# Eval("HAENDL_VERK_WERT_BRU")%>
                                  </td>
                                  <td style="text-align: right; padding: 4px; border: 1px solid black;">
                                    <%# Eval("BEZUGSSTRECKE")%>
                                  </td>
                                  <td style="text-align: right; padding: 4px; border: 1px solid black;">
                                    <%# Eval("ABFRAGEART")%>
                                  </td>
                                  <td style="text-align: left; padding: 4px; border: 1px solid black;">
                                    <%# Eval("BEWERTUNGSART")%>
                                  </td>
                                  <td style="text-align: right; padding: 4px; border: 1px solid black;">
                                    <%# Eval("ANZ_MONATE")%>
                                  </td>
                                  <td style="text-align: right; padding: 4px; border: 1px solid black;">
                                    <%# Eval("ANT_ABSCHR_BETRAG_EK")%>
                                  </td>
                                  <td style="text-align: right; padding: 4px; border: 1px solid black;">
                                    <%# Eval("ANT_ABSCHR_BETRAG_VK")%>
                                  </td>
                                  <td style="text-align: right; padding: 4px; border: 1px solid black;">
                                    <%# Eval("EKF_BASISPREIS_NETTO")%>
                                  </td>
                                  <td style="text-align: right; padding: 4px; border: 1px solid black;">
                                    <%# Eval("KOSTEN")%>
                                  </td>
                                  <td style="text-align: left; padding: 4px; border: 1px solid black;">
                                    <%# Eval("FB2_MIT_KM")%>
                                  </td>
                                </tr>
                              </ItemTemplate>
                            </asp:Repeater>
                            <asp:LinkButton ID="lbSchließen" Text="Schließen" runat="server" />
                          </asp:Panel>
                        </td>

                      </tr>
                      <tr>
                        <th style="text-align: left; vertical-align: top;" rowspan="2">
                          Dokumenteneingang
                        </th>
                        <th style="text-align: left;">
                          <span class="mahntitel">Mahnung</span>
                        </th>

                        <td colspan="2" style="vertical-align: top;">
                          <asp:HyperLink Target="_blank" ID="hlHistorie" runat="server" NavigateUrl='<%# "Report02.aspx?AppID=" & Me.Report02AppID & "&VIN=" & CStr(Eval("CHASSIS_NUM")) & "&cw=true" %>'
                            Enabled='<%# Not TypeOf Eval("EINGANG_ZB2") Is DBNull %>' Text="Fahrzeughistorie"
                            Style="text-decoration: underline;" />
                        </td>

                      </tr>
                      <tr>
                        <th style="text-align: left;">
                          <div class="mahntitel">
                              
                            <span style="display: inline-block; width: 32px; text-align: center;">1</span>
                            <span style="display: inline-block; width: 31px; text-align: center;">2</span>
                            <span style="display: inline-block; width: 31px; text-align: center;">3</span>

                            <span id="Span1" runat="server" Visible='<%# Not TypeOf Eval("MAHNSP_GES_AM") Is DbNull %>' style="display:inline-block; text-align: left;">
                                
                                    <table>
                                        <tr>
                                            <td><img src="/Services/Images/stop.png" alt="Mahnstop" title="Mahnstop" /></td>
                                            <td><asp:LinkButton ID="lbMahnstop" runat="server" Text="Mahnstop" Style="text-decoration: underline;" /></td>
                                        </tr>
                                    </table>

                                  <ajaxToolkit:ModalPopupExtender ID="puMahnstop" runat="server" TargetControlID="lbMahnstop"
                                    PopupControlID="pMahnstop" CancelControlID="lbMahnstopSchließen" BackgroundCssClass="divProgress" />
                                  <asp:Panel ID="pMahnstop" runat="server" CssClass="divPopupDetail" HorizontalAlign="Left"
                                    Style="padding: 15px; border: 1px solid black; background-color: White;">
                                        <table style="border: 0px; margin-bottom: 8px; border-collapse: collapse; width: 400px;">
                                            <tr>
                                                <td colspan="2">
                                                    <table>
                                                        <tr>
                                                            <td><img src="/Services/Images/stop.png" alt="Mahnstop" title="Mahnstop" /></td>
                                                            <td style="font-weight: bold; font-size: 8pt; padding-left: 5px;">Mahnstop</td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; padding: 4px; border: 0px">
                                                    Datum der Sperre
                                                </td>
                                                <td style="text-align: left; padding: 4px; border: 0px; font-weight: bold;">
                                                    <%# Eval("MAHNSP_GES_AM", "{0:d}")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; padding: 4px; border: 0px">
                                                    Gesperrt von
                                                </td>
                                                <td style="text-align: left; padding: 4px; border: 0px; font-weight: bold;">
                                                    <%# Eval("MAHNSP_GES_US")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; padding: 4px; border: 0px">
                                                    Grund
                                                </td>
                                                <td style="text-align: left; padding: 4px; border: 0px; font-weight: bold;">
                                                    <%# Eval("BEM", "{0:d}")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; padding: 4px; border: 0px">
                                                    Ende der Sperre
                                                </td>
                                                <td style="text-align: left; padding: 4px; border: 0px; font-weight: bold;">
                                                    <%# Eval("MAHNDATUM_AB", "{0:d}")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="text-align: left; padding: 4px; border: 0px; ">
                                                    <br/>
                                                    <asp:LinkButton ID="lbMahnstopSchließen" Text="Schließen" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                  </asp:Panel>
                            </span>
                          </div>
                        </th>                        
                        <td colspan="2">
                            <table cellpadding="4">
                                <tr>
                                    <td style="padding-left: 0">
                                         Versand Infopaket
                                    </td>
                                    <td>
                                        <%# Eval("VERS_INFO_PAK", "{0:d}")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 0">
                                        Maximale Erreichungsfrist
                                    </td>
                                    <td>
                                        <%# Eval("MAX_EINR_FRIST", "{0:d}")%>
                                    </td>
                                </tr>
                            </table>
 </td>
                      </tr>
                      <tr>
                        <td>
                          ZBII / Fahrzeugbrief
                        </td>
                        <td>
                          <%# Eval("EINGANG_ZB2", "{0:d}")%>
                          <span class='<%# Eval("MahnstufeZBII", "mahnstufe{0}")%>'>
                          </span>
                          <%# Eval("MahndatumZBII", "{0:d}")%>
                        </td>
                        <td>
                          Ende DAD-Vertragsakte
                        </td>
                        <td>
                          <%# Eval("DAT_VERTR_ENDE", "{0:d}")%>
                        </td>
                      </tr>
                      <tr>
                        <td>
                          CoC
                        </td>
                        <td>
                          <%# Eval("EINGANG_COC", "{0:d}")%>
                          <span class='<%# Eval("MahnstufeCoC", "mahnstufe{0}")%>'>
                          </span>
                          <%# Eval("MahndatumCoC", "{0:d}")%>
                        </td>
                        <td>
                          Vertragsreaktivierung
                        </td>
                        <td>
                          <%# Eval("DAT_VERTR_REAKT", "{0:d}")%>
                          &#8194; &#8194;
                          <%# Eval("WEB_USER_REAKT")%>
                        </td>
                      </tr>

                      <tr>
                        <td style="vertical-align: top;">
                          Sicherungsübereignung
                        </td>
                        <td style="vertical-align: top;">
                          <%# Eval("EINGANG_SUE", "{0:d}")%>
                          <span class='<%# Eval("MahnstufeSÜ", "mahnstufe{0}")%>'>
                          </span>
                          <%# Eval("MahndatumSÜ", "{0:d}")%>
                        </td>
                        <td>
                          Zinsswitch
                        </td>
                        <td>
                          <%# Eval("MELD_ABWEICH", "{0:d}")%>
                        </td>
                      </tr>

                      <tr>
                        <td style="vertical-align: top;">&nbsp;</td>
                        <td style="vertical-align: top;">&nbsp;</td>
                        <td>
                          Grund Zinsswitch
                        </td>
                        <td>
                          <%# Eval("GRUND_ABW_TEXT")%>
                        </td>
                      </tr>
                      
                      <tr>
                        <td style="vertical-align: top;">&nbsp;</td>
                        <td style="vertical-align: top;">&nbsp;</td>
                        <td>
                          Korrekturfaktor
                        </td>
                        <td>
                          <%# Eval("KORREKTURFAKTOR")%>
                        </td>
                      </tr>

                      <tr>
                        <td colspan="4" style="border-bottom: 2px solid grey;">
                          &nbsp;
                        </td>
                      </tr>
                      <tr>
                        <th colspan="2" style="text-align: left;">
                          Letzte Versandadresse
                        </th>
                        <th colspan="2" style="text-align: left;">
                          Fahrzeugstatus
                        </th>
                      </tr>
                      <tr>
                        <td>
                          Name:
                        </td>
                        <td>
                          <%# Eval("NAME1_VA") %>
                        </td>
                        <td>
                          Standort:
                        </td>
                        <td>
                          <%# Eval("FZG_STANDORT") %>
                        </td>
                      </tr>
                      <tr>
                        <td>
                          Name 2:
                        </td>
                        <td>
                          <%# Eval("NAME2_VA") %>
                        </td>
                        <td rowspan="3" style="vertical-align: top;">
                          Letzter Versand am:
                        </td>
                        <td rowspan="3" style="vertical-align: top;">
                          <%# Eval("ZZTMPDT", "{0:d}") %>
                        </td>
                      </tr>
                      <tr>
                        <td>
                          Straße / Hausnummer:
                        </td>
                        <td>
                          <%# Eval("STREET_VA")%>
                        </td>
                      </tr>
                      <tr>
                        <td>
                          Postleitzahl / Ort:
                        </td>
                        <td>
                          <%# Eval("CITY1_VA") %>
                        </td>
                      </tr>
                    </table>
                  </ItemTemplate>
                </asp:FormView>
              </div>
            </ContentTemplate>
          </asp:UpdatePanel>
          <div style="float: right; width: 100%; text-align: right; padding-top: 15px">
            <asp:LinkButton ID="lbtnBack" runat="server" CssClass="Tablebutton" Width="78px"
              OnClick="lbtnBack_Click" Visible="false">» Zurück </asp:LinkButton>
          </div>
          <div id="dataFooter">
            &nbsp;
          </div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>
