<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report01.aspx.vb" Inherits="AppServicesCarRent.Report01"  MasterPageFile="../MasterPage/App.Master" %>


<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
                       </Triggers>
                        <ContentTemplate>
                            <div id="TableQuery">
                               <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width:100%">
                                    <tr  class="formquery" >
                                        <td class="firstLeft active" colspan="2"  style="width:100%">
                                            <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" 
                                                Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="height: 22px"  >
                                            <asp:Label ID="lblDateVon" runat="server" Text="Zulassungsdatum von"></asp:Label>
                                        </td>
                                        <td class="active" style="width:100%; height: 22px;">
                                            <asp:TextBox ID="txtDateVon" runat="server"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txtDateVon_CalendarExtender" runat="server" Enabled="True"
                                                TargetControlID="txtDateVon" Animated="False" >
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active"  >
                                            <asp:Label ID="lblDateBis" runat="server" Text="Zulassungsdatum bis"></asp:Label>
                                        </td>
                                        <td class="active" >
                                            <asp:TextBox ID="txtDateBis" runat="server"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txtDateBis_CalendarExtender" runat="server" Enabled="True"
                                                TargetControlID="txtDateBis" Animated="False">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                      <tr class="formquery">
                                        <td class="firstLeft active"  >
                                            <asp:Label ID="lbl_LizFzgAnzeige" runat="server" Text="LizFzgAnzeige"></asp:Label>
                                        </td>
                                        <td class="active" >
                                        <span><asp:CheckBox ID="cbxLizFzgAnzeige" runat="server"/></span>
                                            

                                        </td>
                                    </tr>
                                     <tr class="formquery">
                                         <td class="firstLeft active" colspan="2" style="width:100%;white-space:normal">
                                           Bei Selektionen über einen Zeitraum größer als 30 Tage muss mit längeren Laufzeiten gerechnet werden. (max. 180 Tage)  </td>

                                    </tr>
                                     <tr class="formquery">
                                         <td class="firstLeft active" colspan="2"  style="width:100%;">
                                             &nbsp;</td>
                                     </tr>
                                </table>
                                <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>
                            </div>
                             <div id="dataQueryFooter" >
                                <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">» 
                                 Suchen</asp:LinkButton>
                            </div>   
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel1" runat="server" Text="Excel herunterladen" />
                                        </span>
                                    </div>
                                </div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server" />
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                    CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0" AllowPaging="True" GridLines="None"
                                                    PageSize="10" CssClass="GridView">
                                                    <PagerSettings Visible="false" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle Wrap="false" CssClass="ItemStyle" />
                                                    <EditRowStyle Wrap="False"></EditRowStyle>
                                                    <Columns>
                                                        <asp:TemplateField SortExpression="Handelsname" HeaderText="col_Handelsname">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Handelsname" runat="server" CommandArgument="Handelsname"
                                                                    CommandName="sort">col_Handelsname</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Handelsname") %>'
                                                                    ID="lblHandelsname" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>                                                    
                                                        <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandArgument="Fahrgestellnummer"
                                                                    CommandName="sort">col_Fahrgestellnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'
                                                                    ID="lblFahrgestellnummer" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Unitnummer" HeaderText="col_Unitnummer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Unitnummer" runat="server" CommandArgument="Unitnummer"
                                                                    CommandName="sort">col_Unitnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Unitnummer") %>'
                                                                    ID="lblUnitnummer" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandArgument="Kennzeichen"
                                                                    CommandName="sort">col_Kennzeichen</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'
                                                                    ID="lblKennzeichen" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Zulassungsdatum" HeaderText="col_Zulassungsdatum">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Zulassungsdatum" runat="server" CommandArgument="Zulassungsdatum"
                                                                    CommandName="sort">col_Zulassungsdatum</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Zulassungsdatum","{0:d}") %>'
                                                                    ID="lblZulassungsdatum" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Farbe" HeaderText="col_Farbe">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Farbe" runat="server" CommandArgument="Farbe"
                                                                    CommandName="sort">col_Farbe</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Farbe") %>'
                                                                    ID="lblFarbe" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="TYPReferenz" HeaderText="col_TYPReferenz">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_TYPReferenz" runat="server" CommandArgument="TYPReferenz "
                                                                    CommandName="sort">col_TYPReferenz</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TYPReferenz") %>'
                                                                    ID="lblTYPReferenz" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="NummerZBII" HeaderText="col_NummerZBII">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_NummerZBII" runat="server" CommandArgument="NummerZBII"
                                                                    CommandName="sort">col_Kennzeichen</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NummerZBII") %>'
                                                                    ID="lblNummerZBII" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>                                                                                                                  
                                                        <asp:TemplateField SortExpression="ORT" HeaderText="col_Auslieferungsort">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Auslieferungsort" runat="server" CommandArgument="ORT"
                                                                    CommandName="sort">col_Auslieferungsort</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ORT") %>'
                                                                    ID="lblAuslieferungsort" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Referenz1" HeaderText="col_Referenz1">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Referenz1" runat="server" CommandArgument="Referenz1"
                                                                    CommandName="sort">col_Referenz1</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Referenz1") %>'
                                                                    ID="lblReferenz1" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Co2" HeaderText="col_Co2">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Co2" runat="server" CommandArgument="Co2"
                                                                    CommandName="sort">col_Co2</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Co2") %>'
                                                                    ID="lblCo2" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField> 
                                                        <asp:TemplateField SortExpression="KW" HeaderText="col_KW">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_KW" runat="server" CommandArgument="KW"
                                                                    CommandName="sort">col_KW</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.KW") %>'
                                                                    ID="lblKW" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Hubraum" HeaderText="col_Hubraum">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Hubraum" runat="server" CommandArgument="Hubraum"
                                                                    CommandName="sort">col_Hubraum</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Hubraum") %>'
                                                                    ID="lblHubraum" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>        
                                                        <asp:TemplateField SortExpression="Typenschluesselnummer" HeaderText="col_Typenschluesselnummer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Typenschluesselnummer" runat="server" CommandArgument="Typenschluesselnummer"
                                                                    CommandName="sort">col_Typenschluesselnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Typenschluesselnummer") %>'
                                                                    ID="lblTypenschluesselnummer" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>                                                                                                                                                                                                                          
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
