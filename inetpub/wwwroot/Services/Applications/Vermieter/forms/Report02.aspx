<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report02.aspx.cs" Inherits="Vermieter.forms.Report02" MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" OnClick="lbBack_Click"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
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
                                                    <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="/Services/Images/queryArrow.gif"
                                                        Width="17px" onclick="NewSearch_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div> 
                            
                            
                    <div id="divError" runat="server" enableviewstate="false" style="padding: 10px 0px 10px 15px;
                        margin-top: 10px">
                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                        <asp:Label ID="lblNoData" runat="server" ForeColor="Blue" EnableViewState="False"></asp:Label>
                    </div>  
                    <asp:Panel ID="Panel1" runat="server">                                             
                    <div id="TableQuery">
                    
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%;">
                            
                               <tr class="formquery" >
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_Auswahl" runat="server">lbl_Auswahl</asp:Label></td>
                                    <td class="active" style="width: 100%">
                                        <span>
                                        <asp:RadioButton ID="rbAlle" runat="server" 
                                          Checked="True" Text="Alles" GroupName="TypAuswahl" />
                                        </span>

                                    </td>
                                </tr>     
                                 <tr class="formquery" >
                                    <td class="firstLeft active">
                                      &nbsp;  
                                    </td>
                                    <td class="active" style="width: 100%">
                                        <span>
                                        <asp:RadioButton ID="rbBriefe" runat="server" Text="Nur Briefe" 
                                            GroupName="TypAuswahl" /></span>
                                    </td>
                                </tr>
                                <tr class="formquery" >
                                    <td class="firstLeft active">
                                        &nbsp;</td>
                                    <td class="active" style="width: 100%">
                                        <span>
                                        <asp:RadioButton ID="rbSchluessel" runat="server" 
                                            Text="Nur Schlüssel" GroupName="TypAuswahl" />
                                        </span>

                                    </td>
                                </tr>
                           
                            <tr class="formquery">
                                <td colspan="2" align="right" style="width: 100%">
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div id="dataQueryFooter">
                            <asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" Width="100px"
                                Height="16px" CausesValidation="False" Font-Underline="False" 
                                OnClick="cmdSearch_Click">» Erstellen</asp:LinkButton>
                               <asp:Button Style="display: none" UseSubmitBehavior="false" ID="btndefault" runat="server"
                                Text="Button" />
                            <asp:LinkButton ID="cmdUebersicht" runat="server" CausesValidation="False" 
                                CssClass="Tablebutton" Font-Underline="False" Height="16px" 
                                OnClick="cmdSearch_Click" Visible="False" Width="100px">» Übersicht</asp:LinkButton>
                            <asp:LinkButton ID="cmdDetails" runat="server" CausesValidation="False" 
                                CssClass="Tablebutton" Font-Underline="False" Height="16px" 
                                OnClick="cmdDetails_Click" Visible="False" Width="100px">» Details</asp:LinkButton>
                        </div>
                    </div>
                    </asp:Panel>
                      <div id="Result" runat="Server" visible="false">
                        <div class="ExcelDiv">
                            <div align="right" class="rightPadding">
                                <img src="/services/Images/iconXLS.gif" alt="Excel herunterladen" />
                                <span class="ExcelSpan">
                                    <asp:LinkButton ID="lnkCreateExcel" runat="server" OnClick="lnkCreateExcel_Click">Excel herunterladen</asp:LinkButton>
                                </span>
                            </div>
                        </div>
                        <div id="pagination">
                            <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                        </div>
                        <div id="data">
                            <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                                CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" 
                                AllowSorting="True" onsorting="GridView1_Sorting" Width="970px" >
                                <PagerSettings Visible="False" />
                                <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                <AlternatingRowStyle CssClass="GridTableAlternate" />
                                <RowStyle CssClass="ItemStyle" />
                                <Columns>
                                    <asp:TemplateField SortExpression="Typ" HeaderText="col_Typ" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Typ" runat="server" CommandName="Sort" CommandArgument="Typ">col_Typ</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblTyp" Text='<%# DataBinder.Eval(Container, "DataItem.Typ") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">col_Kennzeichen</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblKennzeichen" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                
                                    <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
                                        <HeaderStyle Width="120px" />
                                        <ItemStyle Width="120px" />
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblFahrgestellnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField SortExpression="Versandadresse" HeaderText="col_Versandadresse" HeaderStyle-Width="130px" ItemStyle-Width="130px">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Versandadresse" runat="server" CommandName="Sort" CommandArgument="Versandadresse">col_Versandadresse</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblVersandadresse" Text='<%# DataBinder.Eval(Container, "DataItem.Versandadresse") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Beauftragung" HeaderText="col_Beauftragung">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Beauftragung" runat="server" CommandName="Sort" CommandArgument="Beauftragung">col_Beauftragung</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblBeauftragung" Text='<%# DataBinder.Eval(Container, "DataItem.Beauftragung","{0:dd.MM.yyyy}") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Fehlertext" HeaderText="col_Fehlertext" HeaderStyle-Width="55px" ItemStyle-Width="55px">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Fehlertext" runat="server" CommandName="Sort" CommandArgument="Fehlertext">col_Fehlertext</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblFehlertext" Text='<%# DataBinder.Eval(Container, "DataItem.Fehlertext") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Treuhandgeber" HeaderText="col_Treuhandgeber">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Treuhandgeber" runat="server" CommandName="Sort" CommandArgument="Treuhandgeber">col_Treuhandgeber</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblTreuhandgeber" Text='<%# DataBinder.Eval(Container, "DataItem.Treuhandgeber") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Flag_Briefversand" Visible="False">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Flag_Briefversand") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Flag_Briefversand") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Flag_Schluesselversand" Visible="False">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Flag_Schluesselversand") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Flag_Schluesselversand") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Status" HeaderText="Status" Visible="False" />
                                    <asp:TemplateField HeaderText="L&amp;#246;schen">
                                        <ItemTemplate>
                                            <asp:CheckBox id="cbxDelete" runat="server"></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                </Columns>
                            </asp:GridView>
                           </div>
                    
                        <div id="Div1">
                        </div>
                    </div>
                    
                    <div id="dataFooter">
                        <asp:LinkButton ID="btnDelete" runat="server" CssClass="Tablebutton" Width="78px" 
                            Visible="false" onclick="btnDelete_Click">» Speichern</asp:LinkButton>
                    </div>
                    
                </div>
            </div>
        </div>
    </div>

 
</asp:Content>