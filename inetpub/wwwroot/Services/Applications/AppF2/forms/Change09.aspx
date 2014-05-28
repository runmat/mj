<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change09.aspx.vb" Inherits="AppF2.Change09"
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
          <asp:Button ID="bDummy" runat="server" Text="Dummy" style="display: none;" />
          <ajaxToolkit:ModalPopupExtender ID="mpeReaktiviert" runat="server" BehaviorID="mpeReaktiviert"
            TargetControlID="bDummy" BackgroundCssClass="divProgress" PopupControlID="pReaktiviert"
            CancelControlID="lbSchließen" />
          <asp:Panel ID="pReaktiviert" runat="server" CssClass="divPopupDetail" HorizontalAlign="Left"
            Style="padding: 15px; border: 1px solid black; background-color: White;">
            <p>
              Der Vertrag wurde erfolgreich reaktiviert.
            </p>
            <asp:LinkButton ID="lbSchließen" runat="server" Text="Schließen" />
          </asp:Panel>
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
              <asp:Panel ID="Panel1" runat="server">
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
                            <asp:TemplateField HeaderText="col_Reactivate">
                              <HeaderTemplate>
                                <asp:LinkButton ID="col_Reactivate" runat="server" Text="col_Reactivate" />
                              </HeaderTemplate>
                              <ItemTemplate>
                                <asp:ImageButton ID="ibReactivate" runat="server" CommandName="Reactivate" CommandArgument='<%# DataBinder.Eval(Container, "DataItemIndex") %>'
                                  ImageUrl="/Services/Images/Reactivate.png" AlternateText="Reaktivieren" />
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
                            <asp:TemplateField SortExpression="CIN" HeaderText="col_CIN">
                              <HeaderTemplate>
                                <asp:LinkButton ID="col_CIN" runat="server" CommandName="Sort" CommandArgument="CIN">col_CIN</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                <asp:Label runat="server" ID="lblCIN" Text='<%# Eval("CIN") %>'>
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
                            <asp:TemplateField SortExpression="CHASSIS_NUM" HeaderText="col_Vin">
                              <HeaderTemplate>
                                <asp:LinkButton ID="col_Vin" runat="server" CommandName="Sort" CommandArgument="CHASSIS_NUM">col_Vin</asp:LinkButton>
                              </HeaderTemplate>
                              <ItemTemplate>
                                <asp:Label runat="server" ID="lblVin" Text='<%# Eval("CHASSIS_NUM") %>'>
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
                          </Columns>
                        </asp:GridView>
                        <asp:HiddenField ID="hField" runat="server" Value="0" />
                      </td>
                    </tr>
                  </table>
                </div>
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
