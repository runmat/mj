<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportAuswertung.aspx.cs" Inherits="AppZulassungsdienst.forms.ReportAuswertung"  MasterPageFile="../MasterPage/App.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/helper.js?22042016"></script>

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
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div id="paginationQuery">

                            </div>
                            <asp:Panel ID="Panel1" DefaultButton="btnEmpty" runat="server">
                                <div id="TableQuery" style="margin-bottom: 10px">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="3" width="100%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="3" width="100%">
                                                <asp:RadioButton ID="rbAlle" runat="server" GroupName="Daten" Text="Alle Datensätze" Checked="True"/>
                                                <asp:RadioButton ID="rbAbgerechnet" Style="margin-left: 15px" GroupName="Daten" Text="Abgerechnete Zulassungen" runat="server" />
                                                <asp:RadioButton ID="rbnichtAbgerechnet" Style="margin-left: 15px" GroupName="Daten" Text="Noch nicht abgerechnete Zulassungen" runat="server" />
                                                <asp:RadioButton ID="rbnichtDurchgefuehrt" Style="margin-left: 15px" GroupName="Daten" Text="Noch nicht durchgeführte Zulassungen" runat="server" />
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" >
                                                    <asp:Label ID="lblDatum" runat="server">Zulassungdatum von:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" >
                                                    <asp:TextBox ID="txtZulDate" onKeyPress="return numbersonly(event, false)" runat="server" CssClass="TextBoxNormal" 
                                                        Width="75px" MaxLength="6"></asp:TextBox>
                                                    <asp:Label ID="txtZulDateFormate" Style="padding-left: 2px; font-weight: normal"
                                                        Height="15px" runat="server" Width="48px">(ttmmjj)</asp:Label>
                                                        <asp:ImageButton Style="margin-left: 10px" runat="Server" ID="ibtnDateVon" 
                                                        ImageUrl="/PortalZLD/images/Kalender02_07.jpg" 
                                                        AlternateText="Kalender zeigen" Height="22px" Width="27px" /><br />
                                                        <ajaxToolkit:CalendarExtender ID="calendarButtonExtender" runat="server" TargetControlID="txtZulDate" 
                                                            PopupButtonID="ibtnDateVon" Format="ddMMyy" />                                                        
                                                </td>
                                                <td>
                                                    <asp:Label runat="server">Zeitraum max. 92 Tage</asp:Label>
                                                </td>
                                            </tr>                                            
                                            <tr class="formquery">
                                                <td class="firstLeft active" >
                                                    <asp:Label ID="Label1" runat="server">Zulassungdatum bis:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" colspan="2" >
                                                    <asp:TextBox ID="txtZulDateBis" onKeyPress="return numbersonly(event, false)" runat="server" CssClass="TextBoxNormal" 
                                                        Width="75px"></asp:TextBox>
                                                    <asp:Label ID="Label2" Style="padding-left: 2px; font-weight: normal"
                                                        Height="15px" runat="server" Width="48px">(ttmmjj)</asp:Label>
                                                        <asp:ImageButton Style="margin-left: 10px" runat="Server" ID="ibtnDateBis" 
                                                        ImageUrl="/PortalZLD/images/Kalender02_07.jpg" 
                                                        AlternateText="Kalender zeigen" Height="22px" Width="27px" /><br />
                                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtZulDateBis" 
                                                            PopupButtonID="ibtnDateBis" Format="ddMMyy" />                                                        
                                                </td>
                                            </tr>                                             
                                            <tr class="formquery">
                                                <td class="firstLeft active" style="height: 30px">
                                                    <asp:Label ID="lblKunde" runat="server">Kunde:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" style="height: 30px">
                                                    <asp:TextBox ID="txtKunnr" runat="server" onKeyPress="return numbersonly(event, false)"  CssClass="TextBoxNormal" 
                                                        MaxLength="8" Width="75px"></asp:TextBox>
                                                </td>
                                                <td class="firstLeft active" style="width: 100%; height: 30px;">
                                                    <asp:DropDownList ID="ddlKunnr" runat="server" AutoPostBack="True" EnableViewState="False" 
                                                        OnSelectedIndexChanged="ddlKunnr_SelectedIndexChanged" Style="width: 375px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>                                          
                                            <tr class="formquery">
                                                <td class="firstLeft active" >
                                                   <asp:Label ID="lblGruppe" runat="server">Kundengruppe:</asp:Label></td>
                                                <td class="firstLeft active" colspan="3" >
                                                    <asp:DropDownList ID="ddlGruppe" runat="server" Style="width: 375px"></asp:DropDownList>
                                                </td>
                                            </tr>                                            
                                            <tr class="formquery">
                                                <td class="firstLeft active" >
                                                    <asp:Label ID="lblTour" runat="server">Tour:</asp:Label></td>
                                                <td class="firstLeft active" colspan="3" >
                                                   <asp:DropDownList ID="ddlTour" runat="server" Style="width: 375px"></asp:DropDownList>
                                                </td>
                                            </tr> 
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblStva" runat="server">StVA:</asp:Label>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtStVa" runat="server" CssClass="TextBoxNormal" MaxLength="8" 
                                                        Width="75px"></asp:TextBox>
                                                </td>
                                                <td class="firstLeft active" style="width: 100%;">
                                                    <asp:DropDownList ID="ddlStVa" runat="server" Style="width: 375px" AutoPostBack="True"
                                                       OnSelectedIndexChanged="ddlStVa_SelectedIndexChanged" EnableViewState="False" >
                                                    </asp:DropDownList>
                                                </td>

                                            </tr>
                                                   <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblDienst" runat="server">Dienstleistung:</asp:Label>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtMatnr" runat="server" CssClass="TextBoxNormal" 
                                                        MaxLength="8" Width="75px"></asp:TextBox>
                                                </td>
                                                <td class="firstLeft active" style="width: 100%;">
                                                    <asp:DropDownList ID="ddlDienst" runat="server" Style="width: 375px" 
                                                        AutoPostBack="True" onselectedindexchanged="ddlDienst_SelectedIndexChanged"
                                                      >
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>                                                                                    
                                            <tr class="formquery">
                                                <td class="firstLeft active" >
                                                    <asp:Label ID="lblID" runat="server">ID:</asp:Label></td>
                                                <td class="firstLeft active" colspan="2" >
                                                <asp:TextBox ID="txtID" runat="server" CssClass="TextBoxNormal" 
                                                MaxLength="8" Width="75px"></asp:TextBox></td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblRef1" runat="server">Referenz1:</asp:Label></td>
                                                <td class="firstLeft active" colspan="2">
                                                    <asp:TextBox ID="txtRef1" runat="server" CssClass="TextBoxNormal" 
                                                MaxLength="20" Width="100px"></asp:TextBox></td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblKennz" runat="server">Kennzeichen:</asp:Label></td>
                                                <td class="firstLeft active" colspan="2">
                                                <asp:TextBox ID="txtKennz" runat="server" CssClass="TextBoxNormal" 
                                                        MaxLength="20" Width="100px"></asp:TextBox></td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="3">
                                                    <asp:RadioButtonList ID="rbListZahlart" runat="server" 
                                                        RepeatDirection="Horizontal" Width="581px" CellPadding="3" CellSpacing="0">
                                                        <asp:ListItem  Text="Alle Kunden"  Value="" Selected = "true"></asp:ListItem>
                                                        <asp:ListItem  Text="Alle Barkunden"  Value="B"></asp:ListItem>
                                                        <asp:ListItem  Text="mit Einzugsermächtg."  Value="E" ></asp:ListItem>
                                                        <asp:ListItem  Text="mit Rechnung"  Value="R" ></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td colspan="3">
                                                    <asp:ImageButton
                                                        ID="btnEmpty" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
                                                        Width="1px"   />
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
                                <asp:LinkButton ID="cmdCreate" runat="server" CssClass="Tablebutton" 
                                    Width="78px" onclick="cmdCreate_Click">» Suchen </asp:LinkButton>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div id="dataFooter">
                        &nbsp;</div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
