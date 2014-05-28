<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report05.aspx.vb" Inherits="AppF2.Report05" MasterPageFile="../MasterPage/AppMaster.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="server">
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
                            <asp:Panel ID="Panel1" DefaultButton="btnEmpty" runat="server">
                          
                            <div id="TableQuery" style="margin-bottom: 10px">
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr class="formquery">
                                            <td nowrap="nowrap" class="firstLeft active">
                                            </td>
                                            <td class="firstLeft active" width="100%">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                                <asp:CompareValidator ID="CompareValidator5" runat="server" ErrorMessage="'Datum von' kann darf nicht größer als 'Datum bis' sein!"
                                                    Type="Date" ControlToValidate="txtDatumVon" ControlToCompare="txtDatumBis" Operator="LessThanEqual"
                                                    Font-Bold="True" CssClass="TextError" ForeColor=""></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Abmeldedatum von:
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtDatumVon" runat="server" CssClass="InputTextbox" Width="150px"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="txtDatumVon_CalendarExtender" runat="server" Enabled="True"
                                                    TargetControlID="txtDatumVon">
                                                </ajaxToolkit:CalendarExtender>
                                                <asp:CompareValidator ID="cv_txtDatumVon" runat="server" ErrorMessage="Falsches Datumsformat"
                                                    Type="Date" ControlToValidate="txtDatumVon" ControlToCompare="TextBox1" Operator="DataTypeCheck"
                                                    CssClass="TextError" ForeColor=""></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Abmeldedatum bis:
                                            </td>
                                            <td  class="firstLeft active">
                                               
                                                <asp:TextBox ID="txtDatumBis" runat="server" CssClass="InputTextbox" 
                                                    Width="150px"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="txtDatumBis_CalendarExtender" runat="server" 
                                                    Enabled="True" TargetControlID="txtDatumBis">
                                                </ajaxToolkit:CalendarExtender>
                                                <asp:CompareValidator ID="cv_txtDatumBis" runat="server" 
                                                    ControlToCompare="TextBox1" ControlToValidate="txtDatumBis" 
                                                    CssClass="TextError" ErrorMessage="Falsches Datumsformat" ForeColor="" 
                                                    Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td colspan="2">
                                                <asp:TextBox ID="TextBox1" runat="server" Visible="false"></asp:TextBox>
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
                             <div id="dataQueryFooter" >
                                <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">» Suchen </asp:LinkButton>
                            </div>                   

                    
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="../../../Images/iconPDF.gif" alt="PDF herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreatePDF1" ForeColor="White" runat="server">PDF herunterladen</asp:LinkButton>
                                        </span>
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                              <asp:LinkButton ID="lnkCreateExcel1" ForeColor="White" runat="server">Excel herunterladen</asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                        
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvAbmeldungen" Width="100%" runat="server" AutoGenerateColumns="False"
                                                    CellPadding="1" CellSpacing="1" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowSorting="true" AllowPaging="True" CssClass="GridView" PageSize="20">
                                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                    <PagerSettings Visible="False" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                        <asp:BoundField Visible="false" HeaderText="EQUNR" DataField="EQUNR" ReadOnly="true" />
                                                        <asp:TemplateField SortExpression="Vertragsnummer" HeaderText="col_Vertragsnummer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandArgument="Vertragsnummer"
                                                                    CommandName="sort">col_Vertragsnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'
                                                                    ID="lblVertragsnummer" Visible="true"> </asp:Label>
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
                                                        <asp:TemplateField SortExpression="Abmeldedatum" HeaderText="col_Abmeldedatum">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Abmeldedatum" runat="server" CommandArgument="Abmeldedatum"
                                                                    CommandName="sort">col_Abmeldedatum</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Abmeldedatum","{0:d}") %>'
                                                                    ID="lblAbmeldedatum" Visible="true" > </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField> 
                                                        <asp:TemplateField SortExpression="WebUser" HeaderText="col_WebUser">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_WebUser" runat="server" CommandArgument="WebUser" CommandName="sort">col_WebUser</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.WebUser") %>'
                                                                    ID="lblWebUser" Visible="true"> </asp:Label>
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
                    <div id="dataFooter">
                        &nbsp;</div>
                </div>
            </div>
        </div>
    </div>
</asp:content>
