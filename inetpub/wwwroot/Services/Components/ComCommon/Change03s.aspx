<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change03s.aspx.vb" Inherits="CKG.Components.ComCommon.Change03s" MasterPageFile="../../MasterPage/Services.Master"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../PageElements/GridNavigation.ascx" %>


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
                            <asp:Label ID="lblHead" runat="server" Text="Erfassung Abmeldeunterlagen"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
                            
                        </Triggers>
                        <ContentTemplate>
                            <div id="paginationQuery">
                                <table id="Table1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="width: 100%">
                                            <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" 
                                                EnableViewState="False"></asp:Label>
                                            <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnConfirm">
                                <div id="TableQuery">
                                    <table id="tblSearch" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr id="tr_Kunde" runat="server" class="formquery">
                                                <td class="firstLeft active">
                                                    Kunde:
                                                </td>
                                                <td class="active">
                                                    <asp:DropDownList ID="ddlKunde" runat="server" Width="300px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left:15px;padding-top:5px;font-weight:bold;color:#595959;width:200px" 
                                                    valign="top">
                                                    <asp:Label ID="lblCarport" runat="server" Text="Auswahl:"></asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:RadioButtonList ID="rbAktion" runat="server" AutoPostBack="True" 
                                                        Width="100%">
                                                        <asp:ListItem Value="1">Klärfälle zur Bearbeitung</asp:ListItem>
                                                        <asp:ListItem Value="2">Bearbeitete Klärfälle</asp:ListItem>
                                                        <asp:ListItem Value="3">Abgeschlossene Klärfälle</asp:ListItem>
                                                        <asp:ListItem Value="A">Alle Klärfälle</asp:ListItem>
                                                        <asp:ListItem Value="E">Einzelvorgang</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr id = "trKennzeichen" runat=server visible="false" class="formquery">
                                                <td class="firstLeft active" style="white-space:nowrap;">
                                                    <asp:Label ID="Label111" runat="server" Text="Kennzeichen:"></asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtKennz" runat="server" MaxLength="15"></asp:TextBox>
                                                </td>

                                            </tr>
                                            <tr id = "trFahrgestellnummer" runat=server visible="false" class="formquery">
                                                <td class="firstLeft active" style="white-space:nowrap;">
                                                    <asp:Label ID="Label112" runat="server" Text="Fahrgestellnummer:"></asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtFahrgestellnummer" runat="server" MaxLength="17"></asp:TextBox>
                                                </td>

                                            </tr>                                            
                                           <tr id = "trLieferscheinnummer" runat=server visible="false" class="formquery">
                                                <td class="firstLeft active" >
                                                    <asp:Label ID="Label2" runat="server" Text="Lieferscheinnummer:"></asp:Label>
                                                </td>
                                                <td  class="active" >
                                                    <asp:TextBox ID="txtLieferscheinnummer" runat="server" MaxLength="20"></asp:TextBox>
                                                </td>                                           
                                           </tr>
                                          <tr class="formquery">
                                                <td class="firstLeft active" >
                                                    &nbsp;</td>
                                                <td class="active" >                                                
                                                    &nbsp;</td>
                                          </tr>
                                          

                                        </tbody>
                                    </table>
                                    
                                    <table  id="tbl_Query" runat="server" cellpadding="0" cellspacing="0" visible="false">
                                        <tbody>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    CarportID:
                                                </td>
                                                <td class="active">
                                                    <asp:Label ID="lblCarportID" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Lieferscheinnummer:
                                                </td>
                                                <td class="active">
                                                    <asp:Label ID="lblLiefnr" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Fahrgestellnummer:
                                                </td>
                                                <td class="active">
                                                    <asp:Label ID="lblFin" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Kennzeichen
                                                </td>
                                                <td class="active">
                                                    <asp:Label ID="lblKennzeichen" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Klärfalltext DAD:
                                                </td>
                                                <td class="active">
                                                    <asp:Label ID="lblKlarDAD" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    abgeschlossen durch DAD am:
                                                </td>
                                                <td class="active">
                                                    <asp:Label ID="lblEndDat" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Bearbeitung durch Carport am:
                                                </td>
                                                <td class="active">
                                                    <asp:Label ID="lblEditCarport" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" style="padding-left: 15px; padding-top: 10px; font-weight: bold;
                                                    color: #595959; width: 200px">
                                                    Klärfalltext:
                                                </td>
                                                <td class="active" style="padding-top: 10px;">
                                                    <asp:TextBox ID="txtKlaertext" runat="server" Height="90px" MaxLength="359" Width="320px"
                                                        TextMode="MultiLine"></asp:TextBox>
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
                                    
                                    
                                    
                                        <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px; clear:both;">
                                                &nbsp;
                                        </div>                                                                        
                                </div> </asp:Panel>
                            <div id="dataQueryFooter">
                                <asp:LinkButton ID="btnConfirm" runat="server" CssClass="Tablebutton" Width="78px" Height="16px" >» Weiter </asp:LinkButton>
                                <asp:LinkButton ID="lb_Weiter" runat="server" CssClass="Tablebutton" Width="78px" Height="16px" Visible="false" >» Erfassen </asp:LinkButton>
                                <asp:LinkButton ID="lb_Cancel" runat="server" CssClass="Tablebutton" Width="78px" Height="16px" Visible="false" >» Abbrechen</asp:LinkButton>
                            </div>       
                           

                            <div id="Result" runat="Server" visible="false">
                                <div id="DivPrint"  runat="server" visible="false"  class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel1" ForeColor="White" runat="server">Excel herunterladen</asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">

                                                <asp:GridView ID="GridView1"  runat="server" AutoGenerateColumns="False"
                                                    CellPadding="1" CellSpacing="1" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowSorting="True" AllowPaging="True" CssClass="GridView" PageSize="20" DataKeyNames="LICENSE_NUM" >
                                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                    <PagerSettings Visible="False" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                <asp:BoundField ItemStyle-HorizontalAlign="Center" ControlStyle-Width="20px" DataField="CARPORT_ID" HeaderText="Carport ID" />
                                                <asp:BoundField ItemStyle-HorizontalAlign="Center" DataField="LIEFNR" HeaderStyle-Width="115px" HeaderText="Lieferscheinnummer" />
                                                <asp:BoundField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" DataField="CHASSIS_NUM" HeaderText="Fahrgestellnummer" />
                                                <asp:BoundField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" DataField="LICENSE_NUM" HeaderText="Kennzeichen" />
                                                <asp:BoundField ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd.MM.yy}" DataField="DAT_IMP" HeaderText="Datum Meldung" />
                                                <asp:BoundField ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd.MM.yy}"
                                                    DataField="DAT_DEMONT" HeaderText="Datum Demontage" />
                                                <asp:BoundField ItemStyle-HorizontalAlign="Center" DataField="ANZ_KENNZ_CPL"
                                                    HeaderText="Anzahl Kennz." />
                                                <asp:TemplateField HeaderText="Vorlage ZBI">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVorlage0" runat="server" Text="Nein" Visible='<%# DataBinder.Eval(Container, "DataItem.VORLAGE_ZB1_CPL")= 0 %>'></asp:Label>
                                                        <asp:Label ID="lblVorlage1" runat="server" Text="Ja" Visible='<%# DataBinder.Eval(Container, "DataItem.VORLAGE_ZB1_CPL")= 1 %>'></asp:Label>
                                                        <asp:Label ID="lblVorlage2" runat="server" Text="Kopie" Visible='<%# DataBinder.Eval(Container, "DataItem.VORLAGE_ZB1_CPL")= 2 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd.MM.yy}"
                                                    DataField="DAT_ANLAGE_ABW" HeaderText="Anlage Abweichung" />                                                
                                                <asp:TemplateField HeaderText="Bearbeiten/ Details">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbShow" runat="server" Width="10px" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.LICENSE_NUM") %>'
                                                            CommandName="Show" ToolTip="Bearbeiten/Details" Height="10px">
																<img alt="Bearbeiten/Details"  src="../../Images/EditTableHS.png" border="0"/>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                                </asp:GridView>
                                   
                                </div>
                                <div id="dataFooter" runat="server">
                                    &nbsp;
                                </div>                                
                            </div>
                            <div id="ButtonsResultTable" runat="server" style="float:right;margin-top:4px;">                               
                            </div>
                        </contenttemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>