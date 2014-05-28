<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report08.aspx.vb" Inherits="AppServicesCarRent.Report08"
  MasterPageFile="../MasterPage/App.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="/services/PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
          <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <Triggers>
              <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
            </Triggers>
            <ContentTemplate>
                <div id="paginationQuery">
                    <table cellpadding="0" cellspacing="0">
                        <tbody>
                            <tr>
                                <td class="active" style="width: 25px;">
                                    <asp:ImageButton ID="NewSearch" runat="server" Width="17px" OnClick="NewSearch_Click" />
                                </td>
                                <td align="left" class="active" style="vertical-align: middle;">
                                    Abfrageoptionen
                                </td>
                                <td class="active" style="width: 25px;" align="right">
                                    <asp:ImageButton ID="NewSearch2" runat="server" Width="17px" OnClick="NewSearch_Click" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
              <asp:Panel ID="Panel1" runat="server">
                <div id="TableQuery" style="margin-bottom: 10px">
                  <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%">
                    <tbody>
                      <tr class="formquery">
                        <td class="firstLeft active" colspan="2">
                          <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                          <asp:Label ID="lblNoData" runat="server" Font-Bold="True" EnableViewState="False"
                            ForeColor="Blue"></asp:Label>
                        </td>
                      </tr>
                      <tr class="formquery">
                        <td class="firstLeft active" style="width: 200px">
                          Fahrgestellnummer:
                        </td>
                        <td class="firstLeft active">
                          <asp:TextBox ID="txtFahrgestellnummer" runat="server" CssClass="TextBoxNormal" MaxLength="17"
                            Width="200px"></asp:TextBox>
                        </td>
                      </tr>
                      <tr class="formquery">
                        <td class="firstLeft active">
                          Kennzeichen:
                        </td>
                        <td class="firstLeft active">
                          <asp:TextBox ID="txtKennzeichen" runat="server" CssClass="TextBoxNormal" MaxLength="10"
                            Width="200px"></asp:TextBox>
                          &nbsp;
                        </td>
                      </tr>
                      <tr class="formquery">
                        <td class="firstLeft active">
                          Zulassungsdatum von:
                        </td>
                        <td class="firstLeft active">
                          <asp:TextBox ID="txtDatumVon" runat="server" CssClass="TextBoxNormal" Width="200px"></asp:TextBox>
                          <ajaxToolkit:CalendarExtender ID="CE_DatumVon" runat="server" Format="dd.MM.yyyy"
                            PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtDatumVon">
                          </ajaxToolkit:CalendarExtender>
                          <ajaxToolkit:MaskedEditExtender ID="MEE_Datumvon" runat="server" TargetControlID="txtDatumvon"
                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                          </ajaxToolkit:MaskedEditExtender>
                        </td>
                      </tr>
                      <tr class="formquery">
                        <td class="firstLeft active">
                          Zulassungsdatum bis:
                        </td>
                        <td class="firstLeft active">
                          <asp:TextBox ID="txtDatumBis" runat="server" CssClass="TextBoxNormal" Width="200px"></asp:TextBox>
                          <ajaxToolkit:CalendarExtender ID="CE_DatumBis" runat="server" Format="dd.MM.yyyy"
                            PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtDatumBis">
                          </ajaxToolkit:CalendarExtender>
                          <ajaxToolkit:MaskedEditExtender ID="MEE_DatumBis" runat="server" TargetControlID="txtDatumBis"
                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                          </ajaxToolkit:MaskedEditExtender>
                        </td>
                      </tr>
                      <tr class="formquery">
                        <td class="firstLeft active">
                          Teileingang schon erfolgt:
                        </td>
                        <td class="firstLeft active">
                          <span>
                            <asp:CheckBox ID="chxTeileingang" runat="server" />
                          </span>
                        </td>
                      </tr>
                      <tr class="formquery">
                        <td class="firstLeft active">
                          Mahnstufe:
                        </td>
                        <td class="firstLeft active">
                          <asp:CheckBoxList ID="cblMahnstufe" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                          </asp:CheckBoxList>
                        </td>
                      </tr>
                      <tr class="formquery">
                        <td class="firstLeft active">
                          Mahnsperre schon gesetzt:
                        </td>
                        <td class="firstLeft active">
                          <span>
                            <asp:CheckBox ID="cbxMahnsperre" runat="server" />
                          </span>
                        </td>
                      </tr>
                      <tr class="formquery">
                        <td colspan="2">
                          <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
                            Width="1px" />&nbsp;
                        </td>
                      </tr>
                    </tbody>
                  </table>
                  <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                    &nbsp;
                  </div>
                </div>
                <div id="dataQueryFooter">
                    <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px"
                      OnClick="lbCreate_Click">» Suchen </asp:LinkButton>
                  </div>
              </asp:Panel>
              <div id="Result" runat="Server" visible="false">
                <div style="padding-left: 15px; padding-bottom: 10px">
                  <asp:Label ID="lblErrorResult" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                  <asp:Label ID="lblNoDataResult" runat="server" EnableViewState="False" Font-Bold="True"
                    ForeColor="Blue"></asp:Label>
                </div>
                <div style="padding-bottom: 10px; font-weight: bold; padding-left: 15px">
                  Für die ausgewählten Datensätze eine Mahnsperre &nbsp;
                  <asp:LinkButton ID="lbSetMahnsperre" runat="server" CssClass="Tablebutton" Width="78px">» setzen</asp:LinkButton>
                  <span style="font-weight: normal">Sperrgrund:</span>
                  <asp:TextBox ID="txtSperrgrund" runat="server" CssClass="TextBoxNormal" Width="200px" style="vertical-align:middle "></asp:TextBox>
                  &nbsp;/&nbsp;
                  <asp:LinkButton ID="lbDelMahnsperre" runat="server" CssClass="Tablebutton" Width="78px">» entfernen</asp:LinkButton>
                </div>
                <div class="ExcelDiv">
                  <div align="right" class="rightPadding">
                    <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                    <span class="ExcelSpan">
                      <asp:LinkButton ID="lnkCreateExcel1" runat="server" OnClick="lnkCreateExcel1_Click">Excel herunterladen</asp:LinkButton>
                    </span>
                  </div>
                </div>
                <div id="pagination">
                  <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                </div>
                <div id="data">
                  <table width="100%" bgcolor="white" border="0">
                    <tr>
                      <td>
                        <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                          CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True"
                          EnableModelValidation="True">
                          <PagerSettings Visible="False" />
                          <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                          <AlternatingRowStyle CssClass="GridTableAlternate" />
                          <RowStyle CssClass="ItemStyle" />
                          <Columns>
                            <asp:TemplateField Visible="false">
                              <ItemTemplate>
                                <asp:Label runat="server" ID="lblID" Text='<%# Eval("ID") %>'>
                                </asp:Label>
                              </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                              <ItemTemplate>
                                <asp:Label runat="server" ID="lblMatnr" Text='<%# Eval("MATNR") %>'>
                                </asp:Label>
                              </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                              <HeaderTemplate>
                                <asp:LinkButton ID="col_Auswahl" runat="server">Auswahl</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chkAuswahl" Checked='<%# Eval("Selected") = "X" %>' />
                              </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="CHASSIS_NUM" HeaderText="col_Fahrgestellnummer">
                              <HeaderTemplate>
                                <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="CHASSIS_NUM">col_Fahrgestellnummer</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                <asp:Label runat="server" ID="lblFin" Text='<%# Eval("CHASSIS_NUM") %>' />
                              </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="LICENSE_NUM" HeaderText="col_Kennzeichen">
                              <HeaderTemplate>
                                <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="LICENSE_NUM">col_Kennzeichen</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                <asp:Label runat="server" ID="lblKennzeichen" Text='<%# Eval("LICENSE_NUM") %>' />
                              </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Teileingang" HeaderText="col_Teileingang">
                              <HeaderTemplate>
                                <asp:LinkButton ID="col_Teileingang" runat="server" CommandName="Sort" CommandArgument="Teileingang">col_Teileingang</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                <asp:Label runat="server" ID="lblTeileingang" Text='<%# Eval("Teileingang") %>' />
                              </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="MAKTX" HeaderText="col_Materialbezeichnung">
                              <HeaderTemplate>
                                <asp:LinkButton ID="col_Materialbezeichnung" runat="server" CommandName="Sort" CommandArgument="MAKTX">col_Materialbezeichnung</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                <asp:Label runat="server" ID="lblMaterialbezeichnung" Text='<%# Eval("MAKTX") %>' />
                              </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="MAHNSP_GES_AM" HeaderText="col_Mahnsperre">
                              <HeaderTemplate>
                                <asp:LinkButton ID="col_Mahnsperre" runat="server" CommandName="Sort" CommandArgument="MAHNSP_GES_AM">col_Mahnsperre</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                <asp:Label runat="server" ID="lblMahnsperre" Text='<%# IIf(Eval("MAHNSP_GES_AM").Equals(DbNull.Value), "", "X") %>' />
                              </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="ZZMAHNS" HeaderText="col_Mahnstufe">
                              <HeaderTemplate>
                                <asp:LinkButton ID="col_Mahnstufe" runat="server" CommandName="Sort" CommandArgument="ZZMAHNS">col_Mahnstufe</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                <asp:Label runat="server" ID="lblMahnstufe" Text='<%# Eval("ZZMAHNS") %>' />
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
                &nbsp;
                <asp:HiddenField ID="hField" runat="server" Value="0" />
              </div>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
      </div>
    </div>
  </div>
</asp:Content>
