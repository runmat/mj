<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Change10.aspx.cs" Inherits="AppRemarketing.Change10" MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück" onclick="lb_zurueck_Click"></asp:LinkButton>
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
                                            <asp:ImageButton ID="NewSearch" runat="server" 
                                                ImageUrl="../../../Images/queryArrow.gif" onclick="NewSearch_Click1" />
                                            
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div>
                        <table>
                        <tr class="formquery">
                                        <td class="firstLeft active" colspan="2" >
                                            <asp:Label ID="lblError" runat="server" CssClass="TextError" 
                                                EnableViewState="False" style="padding-left:15px"></asp:Label>
                                            <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False" 
                                                EnableViewState="False" style="padding-left:15px" ForeColor="#009900"></asp:Label>
                                        </td>
                                        
                                    </tr>
                        
                        </table>
                    </div>
                    
                    <asp:Panel ID="Panel1" runat="server">
                        <div id="TableQuery" style="margin-bottom: 10px">
                            <table id="tab1" runat="server" cellpadding="0" cellspacing="0" width="100%">
                                <tbody>
                                    
                                   
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            &nbsp;</td>
                                        <td class="firstLeft active">
                                            &nbsp;</td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Upload:
                                        </td>
                                        <td class="firstLeft active">
                                            <input ID="upFileFin" runat="server" name="upFileFin" size="49" type="file" />
                                            <a href="javascript:openinfo('InfoHC.htm');">
                                            <img alt="Struktur Uploaddatei" border="0" height="16px" 
                                                src="/Services/Images/info.gif" title="Struktur Uploaddatei" 
                                                width="16px" /></a> &nbsp; * max. 
                                            900 Datensätze
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            &nbsp;</td>
                                        <td class="firstLeft active">
                                            &nbsp;</td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            &nbsp;</td>
                                        <td class="firstLeft active">
                                            &nbsp;</td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            &nbsp;</td>
                                        <td class="firstLeft active">
                                            &nbsp;</td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            &nbsp;</td>
                                        <td class="firstLeft active">
                                            &nbsp;</td>
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
                        <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" 
                            Width="78px" onclick="lbCreate_Click">» Laden </asp:LinkButton>
                        
                    </div>
                    <div style="float:right;width:100%;text-align:right">
                        <asp:LinkButton ID="lbSend" runat="server" CssClass="Tablebutton"
                            Width="78px" onclick="lbSend_Click" Visible ="false" style="margin-top:10px;margin-bottom:5px">» Senden</asp:LinkButton>
                    </div>
                    
                    <div id="Result" runat="Server" visible="false">
                        <div class="ExcelDiv">
                            <div align="right" class="rightPadding">
                                &nbsp;</div>
                        </div>
                        <div id="pagination">
                            <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                        </div>
                        <div id="data">
                            <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                <tr>
                                    <td>
                                        <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                                CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" 
                                AllowSorting="True" onsorting="GridView1_Sorting">
                                <PagerSettings Visible="False" />
                                <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                <AlternatingRowStyle CssClass="GridTableAlternate" />
                                <RowStyle CssClass="ItemStyle" />
                                <Columns>
                                    <asp:TemplateField SortExpression="Fahrgestellnummer" 
                                        HeaderText="Fahrgestellnummer">
                                        <HeaderStyle Width="120px" />
                                        <ItemStyle Width="120px" />
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">Fahrgestellnummer</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblFahrgestellnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                            </asp:Label>
                                            <asp:TextBox ID="txtFin" runat="server" Visible="False" 
                                                Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>' 
                                                BorderColor="Red" Width="160px" BorderWidth="1px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField SortExpression="Kennzeichen" HeaderText="Kennzeichen">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">Kennzeichen</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblKennzeichen" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                
                                    <asp:TemplateField SortExpression="Ereignisart" 
                                        HeaderText="Ereignisart">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Ereignisart" runat="server" CommandName="Sort" CommandArgument="Ereignisart">Ereignisart</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblEreignisart" Text='<%# DataBinder.Eval(Container, "DataItem.Ereignisart") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Ereignisdatum" HeaderText="Ereignisdatum">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Ereignisdatum" runat="server" CommandName="Sort" CommandArgument="Ereignisdatum">Ereignisdatum</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblEreignisdatum" Text='<%# DataBinder.Eval(Container, "DataItem.Ereignisdatum") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Schadensbetrag ohne Währung" 
                                        SortExpression="Schadensbetrag">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Schadensbetrag" runat="server" CommandName="Sort" CommandArgument="Schadensbetrag">Schadensbetrag</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSchadensbetrag" runat="server" Text='<%# Bind("Schadensbetrag") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ZBEM" HeaderText="Bemerkung" SortExpression="ZBEM" 
                                        Visible="False" />
                                    <asp:TemplateField Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
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
   
   <script type="text/javascript">
       function openinfo(url) {
           fenster = window.open(url, "Uploadstruktur", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=750,height=350");
           fenster.focus();
       }
 
    </script>
   
</asp:Content>