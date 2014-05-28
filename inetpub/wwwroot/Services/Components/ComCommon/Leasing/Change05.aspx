<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change05.aspx.vb" Inherits="CKG.Components.ComCommon.Change05"
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
                          <li style="margin-bottom: 0.5em;">Spalten für Fahrgestellnummer, Kennzeichen, Carportschlüssel
                            Logistikpartner, Bereitstellungsdatum, Bemerkungen, Dienstleistung1, Dienstleistung2, Dienstleistung3,
                            Dienstleistung4 und Dienstleistung5 müssen vorhanden sein.</li>
                          <li style="margin-bottom: 0.5em;">Übrige Spalten werden ignoriert.</li>
                          <li>Die Reihenfolge der Spalten ist unwichtig.</li>
                        </ul>
                        <table cellpadding="0" cellspacing="0" style="border-collapse: separate; border-spacing: 0.1em; border: 1px dotted black;">
                          <thead>
                            <tr>
                              <th style="border: 1px dotted black; padding: 0.3em;">&hellip;</th>
                              <th style="border: 1px dotted black; padding: 0.3em;">Fahrgestellnummer</th>
                              <th style="border: 1px dotted black; padding: 0.3em;">Kennzeichen</th>
                              <th style="border: 1px dotted black; padding: 0.3em;">Carportschlüssel</th>
                              <th style="border: 1px dotted black; padding: 0.3em;">Logistikpartner</th>
                              <th style="border: 1px dotted black; padding: 0.3em;">Bereitstellungsdatum</th>
                              <th style="border: 1px dotted black; padding: 0.3em;">Bemerkungen</th>
                              <th style="border: 1px dotted black; padding: 0.3em;">Dienstleistung1</th>
                              <th style="border: 1px dotted black; padding: 0.3em;">Dienstleistung2</th>
                              <th style="border: 1px dotted black; padding: 0.3em;">Dienstleistung3</th>
                              <th style="border: 1px dotted black; padding: 0.3em;">Dienstleistung4</th>
                              <th style="border: 1px dotted black; padding: 0.3em;">Dienstleistung5</th>
                              <th style="border: 1px dotted black; padding: 0.3em;">&hellip;</th>
                            </tr>
                          </thead>
                          <tbody>
                            <tr>
                              <td style="border: 1px dotted black; padding: 0.3em;">&hellip;</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">nummer</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">QQ-V123</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">schlüssel</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">partner</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">01.01.2001</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">beispiel</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">dl1</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">dl2</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">dl3</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">dl4</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">dl5</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">&hellip;</td>
                            </tr>
                            <tr>
                              <td style="border: 1px dotted black; padding: 0.3em;">&hellip;</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">nummer</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">QQ-V123</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">schlüssel</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">partner</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">01.01.2001</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">beispiel</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">dl1</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">dl2</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">dl3</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">dl4</td>
                              <td style="border: 1px dotted black; padding: 0.3em;">dl5</td>
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
                    <td>
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
                          <asp:Label ID="lblBeauftragt" runat="server" BackColor="LightSalmon" Visible="false"
                          EnableViewState="false" Text="Bereits beauftragt. Nochmaliges Speichern nicht möglich." style="margin-left:5px" />
                          <asp:Label ID="lblErrBereitstellungsdatum" runat="server" BackColor="LightBlue" Visible="False"
                          EnableViewState="False" 
                              Text="Bereitstellungsdatum nicht gefüllt oder in der Vergangenheit." 
                              style="margin-left:5px" />
                      </td>
                    </tr>
                    <tr>
                      <td style="text-align: right;">
                        <asp:Label ID="lblZusatzdienstleistungen" runat="server" Text="Zusätzliche Dienstleistungen:" AssociatedControlID="ddDienstleistung1" />
                        <asp:DropDownList ID="ddDienstleistung1" runat="server" stlye="width: inherit;">
                          <asp:ListItem />
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddDienstleistung2" runat="server" stlye="width: inherit;">
                          <asp:ListItem />
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddDienstleistung3" runat="server" stlye="width: inherit;">
                          <asp:ListItem />
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddDienstleistung4" runat="server" stlye="width: inherit;">
                          <asp:ListItem />
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddDienstleistung5" runat="server" stlye="width: inherit;">
                          <asp:ListItem />
                        </asp:DropDownList>
                      </td>
                    </tr>
                  </table>
                  <uc2:GridNavigation ID="GridNavigation1" runat="server" OnPagerChanged="OnPagerChanged"
                    OnPageSizeChanged="OnPageSizeChanged" PageSizeIndex="1" />
                </div>
                <div id="data" style="overflow-x: scroll;">
                  <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                    CellPadding="1" CellSpacing="1" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                    AllowSorting="True" AllowPaging="True" CssClass="GridView" PageSize="10" DataKeyNames="Fahrgestellnummer"
                    OnRowDataBound="GridRowDataBound" style="width: inherit;">
                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                    <PagerSettings Visible="False" />
                    <RowStyle CssClass="ItemStyle" />
                      <Columns>
                          <asp:TemplateField>
                              <HeaderTemplate>
                                  <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" ForeColor="White">col_Fahrgestellnummer</asp:LinkButton>
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
                                  <asp:LinkButton ID="col_Kennzeichen" runat="server" ForeColor="White">col_Kennzeichen</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                  <asp:Label ID="lblKennzeichen" runat="server" Text='<%# Eval("Kennzeichen") %>' />
                              </ItemTemplate>
                              <EditItemTemplate>
                                  <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ID") %>'></asp:TextBox>
                              </EditItemTemplate>
                          </asp:TemplateField>
                          <asp:TemplateField>
                              <HeaderTemplate>
                                  <asp:LinkButton ID="col_Carport" runat="server" ForeColor="White">col_Carport</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                  <asp:Label ID="lblCarport" runat="server" Text='<%# If(Eval("Carporttext"), Eval("Carportschlüssel")) %>' />
                              </ItemTemplate>
                              <EditItemTemplate>
                                  <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ID") %>'></asp:TextBox>
                              </EditItemTemplate>
                          </asp:TemplateField>
                          <asp:TemplateField>
                              <HeaderTemplate>
                                  <asp:LinkButton ID="col_Logistikpartner" runat="server" ForeColor="White">col_Logistikpartner</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                  <asp:Label ID="lblLogistikpartner" runat="server" Text='<%# Eval("Logistikpartner") %>' />
                              </ItemTemplate>
                              <EditItemTemplate>
                                  <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ID") %>'></asp:TextBox>
                              </EditItemTemplate>
                          </asp:TemplateField>
                          <asp:TemplateField>
                              <HeaderTemplate>
                                  <asp:LinkButton ID="col_Bereitstellungsdatum" runat="server" ForeColor="White">col_Bereitstellungsdatum</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                  <asp:Label ID="lblBereitstellungsdatum" runat="server" Text='<%# Eval("Bereitstellungsdatum", "{0:d}") %>' />
                              </ItemTemplate>
                              <EditItemTemplate>
                                  <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ID") %>'></asp:TextBox>
                              </EditItemTemplate>
                          </asp:TemplateField>
                          <asp:TemplateField>
                              <HeaderTemplate>
                                  <asp:LinkButton ID="col_Bemerkungstext" runat="server" ForeColor="White">col_Bemerkungstext</asp:LinkButton>
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
                                  <asp:LinkButton ID="col_Dienstleistung1" runat="server" ForeColor="White">col_Dienstleistung1</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                  <asp:Label ID="lblDienstleistung1" runat="server" Text='<%# Eval("Dienstleistung1Text") %>' />
                              </ItemTemplate>
                              <EditItemTemplate>
                                  <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ID") %>'></asp:TextBox>
                              </EditItemTemplate>
                          </asp:TemplateField>
                          <asp:TemplateField>
                              <HeaderTemplate>
                                  <asp:LinkButton ID="col_Dienstleistung2" runat="server" ForeColor="White">col_Dienstleistung2</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                  <asp:Label ID="lblDienstleistung2" runat="server" Text='<%# Eval("Dienstleistung2Text") %>' />
                              </ItemTemplate>
                              <EditItemTemplate>
                                  <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ID") %>'></asp:TextBox>
                              </EditItemTemplate>
                          </asp:TemplateField>
                          <asp:TemplateField>
                              <HeaderTemplate>
                                  <asp:LinkButton ID="col_Dienstleistung3" runat="server" ForeColor="White">col_Dienstleistung3</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                  <asp:Label ID="lblDienstleistung3" runat="server" Text='<%# Eval("Dienstleistung3Text") %>' />
                              </ItemTemplate>
                              <EditItemTemplate>
                                  <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ID") %>'></asp:TextBox>
                              </EditItemTemplate>
                          </asp:TemplateField>
                          <asp:TemplateField>
                              <HeaderTemplate>
                                  <asp:LinkButton ID="col_Dienstleistung4" runat="server" ForeColor="White">col_Dienstleistung4</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                  <asp:Label ID="lblDienstleistung4" runat="server" Text='<%# Eval("Dienstleistung4Text") %>' />
                              </ItemTemplate>
                              <EditItemTemplate>
                                  <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ID") %>'></asp:TextBox>
                              </EditItemTemplate>
                          </asp:TemplateField>
                          <asp:TemplateField>
                              <HeaderTemplate>
                                  <asp:LinkButton ID="col_Dienstleistung5" runat="server" ForeColor="White">col_Dienstleistung5</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                  <asp:Label ID="lblDienstleistung5" runat="server" Text='<%# Eval("Dienstleistung5Text") %>' />
                              </ItemTemplate>
                              <EditItemTemplate>
                                  <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ID") %>'></asp:TextBox>
                              </EditItemTemplate>
                          </asp:TemplateField>
                          <asp:TemplateField>
                              <HeaderTemplate>
                                  <asp:LinkButton ID="col_Statustext" runat="server" ForeColor="White">col_Statustext</asp:LinkButton>
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
                    &nbsp;</div>
                <div id="dataFooter" runat="server">
                  &nbsp;
                </div>
              </div>
              <div id="ButtonsResultTable" runat="server" style="float: right; margin-top: 4px;">
                <asp:LinkButton ID="lbPrüfen" runat="server" CssClass="TablebuttonMiddle"
                  Visible="False" Width="100px" Height="22px" Style="text-align: center;" Text="Prüfen"
                  OnClick="OnPrüfenClick" />
                <asp:LinkButton ID="lbBeauftragungSpeichern" runat="server" CssClass="TablebuttonMiddle"
                  Visible="False" Width="100px" Height="22px" Style="text-align: center;" Text="Speichern"
                  OnClick="OnBeauftragungSpeichernClick" />
              </div>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
      </div>
    </div>
  </div>
</asp:Content>
