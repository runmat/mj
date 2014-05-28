<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change10.aspx.vb" Inherits="AppF2.Change10"
  MasterPageFile="../MasterPage/AppMaster.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="/Services/PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                            <asp:TemplateField HeaderText="col_Edit">
                              <HeaderTemplate>
                                <asp:LinkButton ID="col_Edit" runat="server" Text="col_Edit" />
                              </HeaderTemplate>
                              <ItemTemplate>
                                <asp:ImageButton ID="ibEdit" runat="server" CommandName="Details" CommandArgument='<%# DataBinder.Eval(Container, "DataItemIndex") %>'
                                  ImageUrl="/Services/Images/Edit_01.gif" Height="24" Width="22" />
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
                            <asp:TemplateField SortExpression="NAME_KRED" HeaderText="col_Name">
                              <HeaderTemplate>
                                <asp:LinkButton ID="col_Name" runat="server" CommandName="Sort" CommandArgument="NAME_KRED">col_Name</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                <asp:Label runat="server" ID="lblName" Text='<%# Eval("NAME_KRED") %>'>
                                </asp:Label>
                              </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="STREET_KRED" HeaderText="col_Street">
                              <HeaderTemplate>
                                <asp:LinkButton ID="col_Street" runat="server" CommandName="Sort" CommandArgument="STREET_KRED">col_Street</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                <asp:Label runat="server" ID="lblStreet" Text='<%# Eval("STREET_KRED") %>'>
                                </asp:Label>
                              </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="CITY1_KRED" HeaderText="col_City">
                              <HeaderTemplate>
                                <asp:LinkButton ID="col_City" runat="server" CommandName="Sort" CommandArgument="CITY1_KRED">col_City</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                <asp:Label runat="server" ID="lblCity" Text='<%# Eval("CITY1_KRED") %>'>
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
                <asp:LinkButton ID="lbDummy" runat="server" Text="Dummy" Style="display: none;" />
                <ajaxToolkit:ModalPopupExtender ID="mpeEdit" runat="server" TargetControlID="lbDummy"
                  PopupControlID="pEdit" CancelControlID="lbVerwerfen" BackgroundCssClass="divProgress" />
                <asp:Panel ID="pEdit" runat="server" CssClass="divPopupDetail" HorizontalAlign="Left"
                  Style="padding: 15px; border: 1px solid black; background-color: White;">
                  <asp:HiddenField ID="hfIndex" runat="server" />
                  <table cellpadding="4" cellspacing="0">
                    <thead>
                      <tr>
                      <th colspan="2">Abweichende Adresse</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr>
                        <td>
                          <asp:Label ID="lblKontonummer" runat="server" Text="Kontonummer" />
                        </td>
                        <td>
                          <asp:label ID="lblKontonummer1" runat="server" Text="" />
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <asp:Label ID="lblCIN" runat="server" Text="CIN" />
                        </td>
                        <td>
                          <asp:label ID="lblCIN1" runat="server" Text="" />
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <asp:Label ID="lblPAID" runat="server" Text="PAID" />
                        </td>
                        <td>
                          <asp:label ID="lblPAID1" runat="server" Text="" />
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <asp:Label ID="lblNewTitle" runat="server" Text="Anrede" AssociatedControlID="txtNewTitle" />
                        </td>
                        <td>
                          <asp:TextBox ID="txtNewTitle" runat="server" MaxLength="80" />
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <asp:Label ID="lblNewName" runat="server" Text="Name" AssociatedControlID="txtNewName" />
                        </td>
                        <td>
                          <asp:TextBox ID="txtNewName" runat="server" MaxLength="80" />
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <asp:Label ID="lblNewStreet" runat="server" Text="Straße" AssociatedControlID="txtNewStreet" />
                        </td>
                        <td>
                          <asp:TextBox ID="txtNewStreet" runat="server" MaxLength="80" />
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <asp:Label ID="lblNewPostcode" runat="server" Text="Postleitzahl" AssociatedControlID="txtNewPostcode" />
                        </td>
                        <td>
                          <asp:TextBox ID="txtNewPostcode" runat="server" MaxLength="80" />
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <asp:Label ID="lblNewCity" runat="server" Text="Ort" AssociatedControlID="txtNewCity" />
                        </td>
                        <td>
                          <asp:TextBox ID="txtNewCity" runat="server" MaxLength="80" />
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <asp:Label ID="lblNewCountry" runat="server" Text="Land" AssociatedControlID="ddlNewCountry" />
                        </td>
                        <td>
                          <asp:DropDownList ID="ddlNewCountry" runat="server" DataValueField="LAND1" DataTextField="LANDX" />
                        </td>
                      </tr>
                    </tbody>
                    <tfoot>
                      <tr>
                        <td>
                          <asp:LinkButton ID="lbVerwerfen" Text="Verwerfen" runat="server" />
                        </td>
                        <td style="text-align: right;">
                          <asp:LinkButton ID="lbSave" Text="Speichern" runat="server" OnClick="lbSave_Click" />
                        </td>
                      </tr>
                    </tfoot>
                  </table>
                </asp:Panel>
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
