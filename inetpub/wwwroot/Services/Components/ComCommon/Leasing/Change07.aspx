<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change07.aspx.vb" Inherits="CKG.Components.ComCommon.Change07"
  MasterPageFile="../../../MasterPage/Services.Master" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div id="site">
    <div id="content">
        <div id="navigationSubmenu">
            <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
          Text="zurück" CausesValidation="false" />
      </div>
        
      <div id="innerContent">
        <div id="innerContentRight" style="width: 100%">
          <div id="innerContentRightHeading">
            <h1>
              <asp:Label ID="lblHead" runat="server" Text="Label" />
            </h1>
          </div>
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
              <table id="tab1" runat="server" cellpadding="0" cellspacing="0" width="100%">
                <tr class="formquery">
                  <td class="firstLeft active" colspan="2">
                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="false"></asp:Label>
                  </td>
                </tr>
                <tr class="formquery">
                  <td class="firstLeft active">
                    <asp:Label ID="lblDatumVon" runat="server" Text="Bereitstellungsdatum von:" AssociatedControlID="txtDatumVon" />
                  </td>
                  <td class="active" style="width: 100%">
                    <asp:TextBox ID="txtDatumVon" runat="server" />
                    <ajaxToolkit:CalendarExtender ID="CE_DatumVon" runat="server" Format="dd.MM.yyyy"
                      PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtDatumVon" />
                    <ajaxToolkit:MaskedEditExtender ID="MEE_DatumVon" runat="server" TargetControlID="txtDatumVon"
                      Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight" />
                  </td>
                </tr>
                <tr class="formquery">
                  <td class="firstLeft active">
                    <asp:Label ID="lblDatumBis" runat="server" Text="Bereitstellungsdatum bis:" AssociatedControlID="txtDatumBis" />
                  </td>
                  <td class="active">
                    <asp:TextBox ID="txtDatumBis" runat="server" />
                    <asp:CompareValidator ID="cv_txtDatumVon" runat="server" ErrorMessage="'Datum bis' muß größer sein als 'Datum von' "
                      Type="Date" ControlToValidate="txtDatumVon" ControlToCompare="txtDatumBis" Operator="LessThan"
                      CssClass="TextError" />
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd.MM.yyyy"
                      PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtDatumBis" />
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtDatumBis"
                      Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight" />
                  </td>
                </tr>
                <tr class="formquery">
                  <td class="firstLeft active">
                    <asp:Label ID="lblFahrgestellnummer" runat="server" Text="Fahrgestellnummer" AssociatedControlID="txtFahrgestellnummer" />
                  </td>
                  <td class="active">
                    <asp:TextBox ID="txtFahrgestellnummer" runat="server" />
                  </td>
                </tr>
                <tr class="formquery">
                  <td class="firstLeft active">
                    <asp:Label ID="lblKennzeichen" runat="server" Text="KFZ Kennzeichen:" AssociatedControlID="txtKennzeichen" />
                  </td>
                  <td class="active">
                    <asp:TextBox ID="txtKennzeichen" runat="server" />
                  </td>
                </tr>
                <tr class="formquery">
                  <td class="firstLeft active">
                    <asp:Label ID="lblCarport" runat="server" Text="Carport:" AssociatedControlID="ddlCarport" />
                  </td>
                  <td class="active">
                    <asp:DropDownList ID="ddlCarport" runat="server">
                      <asp:ListItem Value="*" Text="Alle" />
                    </asp:DropDownList>
                  </td>
                </tr>
                <tr class="formquery">
                  <td class="firstLeft active">
                    <asp:Label ID="lblStatus" runat="server" Text="Status:" AssociatedControlID="txtStatus" />
                  </td>
                  <td class="active">
                    <asp:TextBox ID="txtStatus" runat="server" />
                  </td>
                </tr>
                <tr class="formquery">
                  <td class="firstLeft active" style="height: 19px" colspan="2">
                    &nbsp;
                  </td>
                </tr>
                <tr class="formquery">
                  <td colspan="2">
                    <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
                      Width="1px" />
                  </td>
                </tr>
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
            <div class="ExcelDiv">
              <div style="float: right; margin-bottom: 15px" class="rightPadding">
                <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                <span class="ExcelSpan">
                  <asp:LinkButton ID="lnkCreateExcel1" ForeColor="White" runat="server">Excel herunterladen</asp:LinkButton>
                </span>
              </div>
            </div>
            <div id="pagination">
              <uc2:GridNavigation ID="GridNavigation1" runat="server" OnPagerChanged="GridNavigation1_PagerChanged"
                OnPageSizeChanged="GridNavigation1_PageSizeChanged" />
            </div>
            <div id="data">
              <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                <tr>
                  <td>
                    <asp:GridView ID="gvBestand" Width="100%" runat="server" CellPadding="1" CellSpacing="1"
                      GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0" AllowSorting="True" AllowPaging="True"
                      PageSize="20" AutoGenerateColumns="false">
                      <HeaderStyle BackColor="#9b9b9b" ForeColor="White" Height="30px" />
                      <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                      <PagerSettings Visible="False" />
                      <RowStyle CssClass="ItemStyle" />
                      <Columns>
                        <asp:TemplateField>
                          <HeaderTemplate>
                            <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="CHASSIS_NUM"
                              Text="col_Fahrgestellnummer" />
                          </HeaderTemplate>
                          <ItemTemplate>
                            <asp:Label ID="lblFahrgestellnummer" runat="server" Text='<%# Eval("CHASSIS_NUM") %>' />
                          </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                          <HeaderTemplate>
                            <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="ETEXT4"
                              Text="col_Kennzeichen" />
                          </HeaderTemplate>
                          <ItemTemplate>
                            <asp:Label ID="lblKennzeichen" runat="server" Text='<%# Eval("ETEXT4") %>' />
                          </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                          <HeaderTemplate>
                            <asp:LinkButton ID="col_Carport" runat="server" CommandName="Sort" CommandArgument="ETEXT1"
                              Text="col_Carport" />
                          </HeaderTemplate>
                          <ItemTemplate>
                            <asp:Label ID="lblCarport" runat="server" Text='<%# Eval("ETEXT1") %>' />
                          </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                          <HeaderTemplate>
                            <asp:LinkButton ID="col_Bereitstellungsdatum" runat="server" CommandName="Sort" CommandArgument="TERMIN_ANFANG"
                              Text="col_Bereitstellungsdatum" />
                          </HeaderTemplate>
                          <ItemTemplate>
                            <asp:Label ID="lblBereitstellungsdatum" runat="server" Text='<%# Eval("TERMIN_ANFANG", "{0:d}") %>' />
                          </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                          <HeaderTemplate>
                            <asp:LinkButton ID="col_Bemerkung" runat="server" CommandName="Sort" CommandArgument="BEM"
                              Text="col_Bemerkung" />
                          </HeaderTemplate>
                          <ItemTemplate>
                            <asp:Label ID="lblBemerkung" runat="server" Text='<%# Eval("TEXTE") %>' />
                          </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                          <HeaderTemplate>
                            <asp:LinkButton ID="col_Dienstleistungen" runat="server" CommandName="Sort" CommandArgument=""
                              Text="col_Dienstleistungen" />
                          </HeaderTemplate>
                          <ItemTemplate>
                            <asp:Label ID="lblDienstleistungen" runat="server" Text='<%# Eval("DIENSTLEISTUNGEN") %>' />
                          </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                          <HeaderTemplate>
                            <asp:LinkButton ID="col_Eingang" runat="server" CommandName="Sort" CommandArgument="EINGANG"
                              Text="col_Eingang" />
                          </HeaderTemplate>
                          <ItemTemplate>
                            <asp:Label ID="lblEingang" runat="server" Text='<%# Eval("EINGANG", "{0:d}") %>' />
                          </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                          <HeaderTemplate>
                            <asp:LinkButton ID="col_Bereitmeldung" runat="server" CommandName="Sort" CommandArgument="BEREITMELDUNG"
                              Text="col_Bereitmeldung" />
                          </HeaderTemplate>
                          <ItemTemplate>
                            <asp:Label ID="lblBereitmeldung" runat="server" Text='<%# Eval("BEREITMELDUNG", "{0:d}") %>' />
                          </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                          <HeaderTemplate>
                            <asp:LinkButton ID="col_Status" runat="server" CommandName="Sort" CommandArgument="ANLZU"
                              Text="col_Status" />
                          </HeaderTemplate>
                          <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server" Text='<%# If(Eval("ANLZU"), "Neu") %>' />
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
        </div>
      </div>
    </div>
  </div>
</asp:Content>
