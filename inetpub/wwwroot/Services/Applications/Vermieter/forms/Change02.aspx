<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Change02.aspx.cs" Inherits="Vermieter.forms.Change02" MasterPageFile="../Master/AppMaster.Master" %>


<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" onclick="lbBack_Click"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery" style="width: 100%; display: block;">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="TableQuery" style="margin-bottom: 10px">
                        <table id="tab1" cellpadding="0" cellspacing="0">
                            <tfoot>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                            </tfoot>
                            <tbody>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td nowrap="nowrap" width="100%" style="padding-left:15px">
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                        <asp:Label ID="lblNoData" runat="server" EnableViewState="False" 
                                            ForeColor="#000099"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;</td>
                                    <td nowrap="nowrap" width="100%">
                                        &nbsp;</td>
                                </tr>
                                <tr class="formquery">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:LinkButton ID="btnConfirm" runat="server" CssClass="TablebuttonLarge" Height="16px"
                                            Width="130px" onclick="btnConfirm_Click">» Abfrage starten</asp:LinkButton>
                                            <asp:LinkButton ID="btnUebernahme" runat="server" 
                                            CssClass="TablebuttonLarge" Height="16px"
                                            Width="130px" Visible="false" onclick="btnUebernahme_Click">» &Uuml;bernehmen</asp:LinkButton>
                                        <asp:LinkButton ID="btnReset" runat="server" CssClass="TablebuttonLarge" Height="16px"
                                            Width="130px" Visible="false" onclick="btnReset_Click">» Verwerfen</asp:LinkButton>
                                        <asp:LinkButton ID="btnSave" runat="server" CssClass="TablebuttonLarge" Height="16px"
                                            Width="130px" Visible="false" onclick="btnSave_Click">» Absenden</asp:LinkButton>
                                            <asp:LinkButton ID="btnBack" runat="server" CssClass="TablebuttonLarge" Height="16px"
                                            Width="130px" Visible="false" onclick="btnBack_Click">» Zurück</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="DivPlaceholder" runat="server" style="height: 550px;">
                    </div>
                    <div id="Result" runat="Server" visible="false">
                        <div class="ExcelDiv">
                            <div align="right">
                                <img src="/services/Images/iconXLS.gif" alt="Excel herunterladen" />
                                <span class="ExcelSpan">
                                    <asp:LinkButton ID="lbCreateExcel" runat="server" 
                                    Text="Excel herunterladen" ForeColor="White" onclick="lbCreateExcel_Click"></asp:LinkButton>
                                </span>
                            </div>
                        </div>
                        <div id="pagination">
                            <uc2:GridNavigation ID="GridNavigation1" runat="server">
                            </uc2:GridNavigation>
                        </div>
                        <div id="data">
                            <table cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                                <tr>
                                    <td>
                                      
                                      <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="gvAusgabe"
                                CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" 
                                AllowSorting="True" onsorting="gvAusgabe_Sorting" Width="1200px">
                                <PagerSettings Visible="False" />
                                <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                <AlternatingRowStyle CssClass="GridTableAlternate" />
                                <RowStyle CssClass="ItemStyle" />
                                <Columns>
                                    <asp:TemplateField SortExpression="DADPDI" HeaderText="col_PDI">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_PDI" runat="server" CommandName="Sort" CommandArgument="DADPDI">col_PDI</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPDI" Text='<%# DataBinder.Eval(Container, "DataItem.DADPDI") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField Visible="False">
                                         <EditItemTemplate>
                                             <asp:TextBox ID="txtEQUNR" runat="server" Text='<%# Bind("EQUNR") %>'></asp:TextBox>
                                         </EditItemTemplate>
                                         <ItemTemplate>
                                             <asp:Label ID="lblEQUNR" runat="server" Text='<%# Bind("EQUNR") %>'></asp:Label>
                                         </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField SortExpression="ZZFAHRG" HeaderText="col_Fahrgestellnummer">
                                        <HeaderStyle Width="125px" />
                                        <ItemStyle Width="125px" />
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="ZZFAHRG">col_Fahrgestellnummer</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblFahrgestellnummer" Text='<%# DataBinder.Eval(Container, "DataItem.ZZFAHRG") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ZZKENN" HeaderText="col_Kennzeichen">
                                    <HeaderStyle Width="100px" />
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="ZZKENN">col_Kennzeichen</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            
                                            <asp:Label runat="server" ID="lblKennzeichen" Text='<%# DataBinder.Eval(Container, "DataItem.ZZKENN") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                
                                   
                                    <asp:TemplateField SortExpression="ZZREFERENZ1" HeaderText="col_Fahrzeugnummer">
                                        <HeaderStyle Width="110px" />
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Fahrzeugnummer" runat="server" CommandName="Sort" CommandArgument="ZZREFERENZ1">col_Fahrzeugnummer</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblFahrzeugnummer" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ1") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField SortExpression="Modellbezeichnung" HeaderText="col_Modellbezeichnung" >
                                        <HeaderStyle Width="120px" />
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Modellbezeichnung" runat="server" CommandName="Sort" CommandArgument="ZZBEZEI">col_Modellbezeichnung</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblModellbezeichnung" Text='<%# DataBinder.Eval(Container, "DataItem.ZZBEZEI") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField SortExpression="ZZHERST_TEXT" HeaderText="col_Hersteller">
                                     <HeaderStyle Width="110px" />
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Hersteller" runat="server" CommandName="Sort" CommandArgument="ZZHERST_TEXT">col_Hersteller</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblHersteller" Text='<%# DataBinder.Eval(Container, "DataItem.ZZHERST_TEXT") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField SortExpression="ZZLAUFZEIT" HeaderText="col_Laufzeit">
                                     <HeaderStyle Width="50px" />
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Laufzeit" runat="server" CommandName="Sort" CommandArgument="ZZLAUFZEIT">col_Laufzeit</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblLaufzeit" Text='<%# DataBinder.Eval(Container, "DataItem.ZZLAUFZEIT") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField SortExpression="REPLA_DATE" HeaderText="col_Abmeldedatum">
                                     <HeaderStyle Width="100px" />
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Abmeldedatum" runat="server" CommandName="Sort" CommandArgument="REPLA_DATE">col_Abmeldedatum</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblAbmeldedatum" Text='<%# DataBinder.Eval(Container, "DataItem.REPLA_DATE","{0:dd.MM.yyyy}") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField SortExpression="VDATU" HeaderText="col_Zulassungsdatum">
                                    <HeaderStyle Width="120px" />
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Zulassungsdatum" runat="server" CommandName="Sort" CommandArgument="VDATU">col_Zulassungsdatum</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblZulassungsdatum" Text='<%# DataBinder.Eval(Container, "DataItem.VDATU","{0:dd.MM.yyyy}") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Keine Auswahl">
                                    <HeaderStyle Width="70px" />
                                        <ItemTemplate>
                                           <asp:RadioButton id="chkActionNOTHING" runat="server" GroupName="grpAction" Checked='<%# DataBinder.Eval(Container, "DataItem.ActionNOTHING") %>'>
																</asp:RadioButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Haltefrist ignorieren und abmelden">
                                        <HeaderStyle  Width="100px"/>
                                        <ItemTemplate>
                                        
                                           <asp:RadioButton id="chkActionDELE" runat="server" GroupName="grpAction" Checked='<%# DataBinder.Eval(Container, "DataItem.ActionDELE") %>'>
																</asp:RadioButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:BoundField DataField="Action" HeaderText="Aktion" Visible="False" />
                                    <asp:BoundField DataField="Bemerkung" HeaderText="Bemerkung" Visible="False" />
                                    
                                </Columns>
                            </asp:GridView>
                                      
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="dataFooter">
                            &nbsp;</div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
