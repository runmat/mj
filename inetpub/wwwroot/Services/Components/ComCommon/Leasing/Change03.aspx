<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change03.aspx.vb" Inherits="CKG.Components.ComCommon.Change03"
  MasterPageFile="../../../MasterPage/Services.Master" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div id="site">
    <div id="content">
      <div id="navigationSubmenu">
          <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" />
      </div>
      <div id="innerContent">
        <div id="innerContentRight" style="width: 100%">
          <div id="innerContentRightHeading">
            <h1>
              <asp:Label ID="lblHead" runat="server" Text="Rückmeldungen" />
            </h1>
          </div>
          <div id="paginationQuery">
            <table id="Table1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%">
              <tr class="formquery">
                <td class="firstLeft active" style="width: 100%">
                  <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" Visible="true" EnableViewState="false" />
                </td>
              </tr>
            </table>
          </div>
          <asp:Panel ID="Panel1" runat="server">
            <div id="TableQuery">
              <table cellpadding="0" cellspacing="0">
                <tbody>
                  <tr class="formquery">
                    <td class="firstLeft active">
                      <asp:Label ID="lblDateisuchen" runat="server" Text="Datei auswählen:" AssociatedControlID="fuRückmeldungen" />
                    </td>
                    <td class="active">
                      <asp:FileUpload ID="fuRückmeldungen" runat="server" Width="320px" />
                    </td>
                    <td class="active">
                      <asp:LinkButton ID="lbDateiformat" runat="server" Text="Dateiformat" />
                      <AjaxToolkit:ModalPopupExtender ID="mpeDateiformat" runat="server" TargetControlID="lbDateiformat"
                        PopupControlID="pDateiformat" CancelControlID="lbDateiformatSchließen" BackgroundCssClass="modalBackground" DropShadow="true" />
                      <asp:Panel ID="pDateiformat" runat="server" BorderStyle="Solid" BorderColor="Black"
                        BorderWidth="1px" Style="padding: 1em;" BackColor="White">
                        <ul style="list-style: disc outside; width: 280px;">
                          <li style="margin-bottom: 0.5em;">Eine Exceldatei mit Daten in lediglich einer Tabelle.</li>
                          <li style="margin-bottom: 0.5em;">Die erste Zeile ist die Kopfzeile, alle weitere
                            Zeilen beinhalten Daten.</li>
                          <li style="margin-bottom: 0.5em;">Spalten für Fahrgestellnummer, Leistungsart, Prognose
                            Arbeitsende, Endrückmeldung (E/T) und Bemerkungen müssen vorhanden sein.</li>
                          <li style="margin-bottom: 0.5em;">Übrige Spalten werden ignoriert.</li>
                          <li>Die Reihenfolge der Spalten ist unwichtig.</li>
                        </ul>
                        <table cellpadding="0" cellspacing="0" style="border-collapse: separate; border-spacing: 0.1em; border: 1px dotted black;">
                          <thead>
                            <tr>
                              <th style="border: 1px dotted black; padding: 0.3em;">&hellip;</th>
                              <th style="border: 1px dotted black; padding: 0.3em;">Fahrgestellnummer</th>
                              <th style="border: 1px dotted black; padding: 0.3em;">Leistungsart</th>
                              <th style="border: 1px dotted black; padding: 0.3em;">Prognose Arbeitsende</th>
                              <th style="border: 1px dotted black; padding: 0.3em;">Endrückmeldung</th>
                              <th style="border: 1px dotted black; padding: 0.3em;">Bemerkungen</th>
                              <th style="border: 1px dotted black; padding: 0.3em;">&hellip;</th>
                            </tr>
                          </thead>
                          <tbody>
                            <tr>
                              <td style="border: 1px dotted black; padding: 0.3em;">&hellip;</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">nummer</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">art</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">01.01.2001</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">E</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">beispiel</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">&hellip;</td>
                            </tr>
                            <tr>
                              <td style="border: 1px dotted black; padding: 0.3em;">&hellip;</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">nummer</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">art</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">01.01.2001</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">T</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">beispiel</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">&hellip;</td>
                            </tr>
                          </tbody>
                          <caption style="font-size: larger; margin-bottom: 0.7em; color: Red; text-align: left;">Datenbeispiel</caption>
                        </table>
                        <p style="width: 100%; text-align: right;">
                          <asp:LinkButton ID="lbDateiformatSchließen" runat="server" Text="Schließen" />
                        </p>
                      </asp:Panel>
                    </td>
                  </tr>
                  <tr class="formquery">
                    <td>
                    </td>
                    <td class="active">
                      <asp:LinkButton ID="lbHochladen" runat="server" CssClass="Tablebutton" Width="78px"
                        Style="margin-left: 0;" Text="» Hochladen" OnClick="OnHochladenClick" />
                    </td>
                    <td class="active">
                      <asp:LinkButton ID="lbMöglicheLeistungsarten" runat="server" Text="Mögliche Leistungsarten" />
                      <AjaxToolkit:ModalPopupExtender ID="mpeMöglicheLeistungen" runat="server" TargetControlID="lbMöglicheLeistungsarten"
                        PopupControlID="pMöglicheLeistungen" CancelControlID="lbMöglicheLeistungsartenSchließen"
                        BackgroundCssClass="modalBackground" DropShadow="true" />
                      <asp:Panel ID="pMöglicheLeistungen" runat="server" BorderStyle="Solid" BorderColor="Black"
                        BorderWidth="1px" Style="padding: 1em;" BackColor="White">
                        <asp:Repeater ID="rMöglicheLeistungen" runat="server">
                          <HeaderTemplate>
                            <dl>
                          </HeaderTemplate>
                          <ItemTemplate>
                            <dt>
                              <%# Eval("ExterneKennung")%></dt>
                            <dd>
                              <%# Eval("Leistungsart")%></dd>
                          </ItemTemplate>
                          <FooterTemplate>
                            </dl>
                          </FooterTemplate>
                        </asp:Repeater>
                        <p style="width: 100%; text-align: right;">
                          <asp:LinkButton ID="lbMöglicheLeistungsartenSchließen" runat="server" Text="Schließen" />
                        </p>
                      </asp:Panel>
                    </td>
                  </tr>
                  <tr class="formquery">
                    <td class="firstLeft active" colspan="3">
                      &nbsp;
                    </td>
                  </tr>
                </tbody>
              </table>
              <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;
                clear: both;">
                &nbsp;
              </div>
            </div>
          </asp:Panel>
          <div id="dataQueryFooter" style="clear: both;">
          </div>
          <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
            <ContentTemplate>
              <div id="Result" runat="Server" visible="false">
                <div id="pagination">
                  <table id="Table2" runat="server" cellpadding="0" cellspacing="0" style="width: 100%;
                    padding-left: 15px;">
                    <tr class="formquery">
                      <td class="firstLeft active" style="width: 100%">
                        <asp:Label ID="lblErrorVorhanden" runat="server" ForeColor="#FF3300" Visible="false"
                          EnableViewState="false" Text="Es sind Fehler vorhanden." />
                      </td>
                    </tr>
                  </table>
                  <uc2:GridNavigation ID="GridNavigation1" runat="server" OnPagerChanged="OnPagerChanged"
                    OnPageSizeChanged="OnPageSizeChanged" PageSizeIndex="1" />
                </div>
                <div id="data">
                  <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False"
                    CellPadding="1" CellSpacing="1" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                    AllowSorting="True" AllowPaging="True" CssClass="GridView" PageSize="10" DataKeyNames="Fahrgestellnummer"
                    OnRowDataBound="GridRowDataBound">
                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                    <PagerSettings Visible="False" />
                    <RowStyle CssClass="ItemStyle" />
                    <Columns>
                      <asp:TemplateField>
                        <HeaderTemplate>
                          <asp:Label ID="lbl_Fahrgestellnummer" runat="server" Text="lbl_Fahrgestellnummer" />
                        </HeaderTemplate>
                        <ItemTemplate>
                          <asp:Label ID="lblFahrgestellnummer" runat="server" Text='<%# Eval("Fahrgestellnummer") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                          <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ID") %>'></asp:TextBox>
                        </EditItemTemplate>
                      </asp:TemplateField>
                      <asp:TemplateField>
                        <HeaderTemplate>
                          <asp:Label ID="lbl_Leistungsart" runat="server" Text="lbl_Leistungsart" />
                        </HeaderTemplate>
                        <ItemTemplate>
                          <asp:Label ID="lblLeistungsart" runat="server" Text='<%# If(Eval("LeistungsartText"),Eval("Leistungsart")) %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                          <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ID") %>'></asp:TextBox>
                        </EditItemTemplate>
                      </asp:TemplateField>
                      <asp:TemplateField>
                        <HeaderTemplate>
                          <asp:Label ID="lbl_PrognoseArbeitsende" runat="server" Text="lbl_PrognoseArbeitsende" />
                        </HeaderTemplate>
                        <ItemTemplate>
                          <asp:Label ID="lblPrognoseArbeitsende" runat="server" Text='<%# Eval("PrognoseArbeitsende", "{0:d}") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                          <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ID") %>'></asp:TextBox>
                        </EditItemTemplate>
                      </asp:TemplateField>
                      <asp:TemplateField>
                        <HeaderTemplate>
                          <asp:Label ID="lbl_Endrückmeldung" runat="server" Text="lbl_Endrückmeldung" />
                        </HeaderTemplate>
                        <ItemTemplate>
                          <asp:Label ID="lblEndrückmeldung" runat="server" Text='<%# Eval("Endrückmeldung") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                          <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ID") %>'></asp:TextBox>
                        </EditItemTemplate>
                      </asp:TemplateField>
                      <asp:TemplateField>
                        <HeaderTemplate>
                          <asp:Label ID="lbl_Bemerkungstext" runat="server" Text="lbl_Bemerkungstext" />
                        </HeaderTemplate>
                        <ItemTemplate>
                          <asp:Label ID="lblBemerkungstext" runat="server" Text='<%# Eval("Bemerkungstext") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                          <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ID") %>'></asp:TextBox>
                        </EditItemTemplate>
                      </asp:TemplateField>
                      <asp:TemplateField>
                        <HeaderTemplate>
                          <asp:Label ID="lbl_Statustext" runat="server" Text="lbl_Statustext" />
                        </HeaderTemplate>
                        <ItemTemplate>
                          <asp:Label ID="lblStatustext" runat="server" Text='<%# Eval("Statustext") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                          <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ID") %>'></asp:TextBox>
                        </EditItemTemplate>
                      </asp:TemplateField>
                    </Columns>
                  </asp:GridView>
                </div>
                <div id="dataFooter" runat="server">
                  &nbsp;
                </div>
              </div>
              <div id="ButtonsResultTable" runat="server" style="float: right; margin-top: 4px;">
                <asp:LinkButton ID="lbRückmeldungenVerbuchen" runat="server" CssClass="TablebuttonMiddle"
                  Visible="False" Width="100px" Height="22px" Style="text-align: center;" Text="Verbuchen"
                  OnClick="OnRückmeldungenVerbuchenClick" />
              </div>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
      </div>
    </div>
  </div>
</asp:Content>
