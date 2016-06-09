<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadRechnungsanhang.aspx.cs" Inherits="AppZulassungsdienst.forms.UploadRechnungsanhang" MasterPageFile="../MasterPage/App.Master" %>
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
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="paginationQuery">

                            </div>

                            <div id="TableQuery" style="margin-bottom: 10px">
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="4">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" >
                                                <asp:Label ID="lblDatum" runat="server">Zulassungdatum von:</asp:Label>
                                            </td>
                                            <td class="firstLeft active" colspan="2" >
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
                                            <td class="firstLeft active" colspan="3" >
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
                                        <tr class="formquery" id="trVorgang" runat="server">
                                            <td class="firstLeft active" style="height: 36px">
                                                <asp:Label ID="Label3" runat="server">Vorgang:</asp:Label>
                                            </td>
                                            <td class="firstLeft active" colspan="3" >
                                                <asp:RadioButton ID="rbAH_ON_NZ" GroupName="Vorgang" runat="server" Text="Normal, Online und Autohaus" Checked="True" />
                                                <asp:RadioButton ID="rbNZ" GroupName="Vorgang" runat="server" Text="normale Vorgänge" />
                                                <asp:RadioButton ID="rbON" GroupName="Vorgang" runat="server" Text="Online" />
                                                <asp:RadioButton ID="rbAH" GroupName="Vorgang" runat="server" Text="Autohaus" />
                                                <asp:RadioButton ID="rbAH_NZ" GroupName="Vorgang" runat="server" Text="Normal und Autohaus" />
                                            </td>
                                        </tr>                             
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="Label8" runat="server">StVA von:</asp:Label>
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtStVavon" runat="server" CssClass="TextBoxNormal TextUpperCase" MaxLength="8"
                                                    Width="75px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:Label ID="Label9" runat="server">bis:</asp:Label>
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtStVaBis" runat="server" CssClass="TextBoxNormal TextUpperCase" MaxLength="8"
                                                    Width="75px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td colspan="4">

                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" style="height: 36px; vertical-align: top">
                                                <asp:Label ID="Label5" runat="server">Template für Upload-Datei:</asp:Label>
                                            </td>
                                            <td class="firstLeft active" colspan="3">
                                                <asp:RadioButtonList runat="server" ID="rblTemplate" RepeatDirection="Vertical" AutoPostBack="True" OnSelectedIndexChanged="rblTemplate_SelectedIndexChanged"/>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" style="height: 36px">
                                                <asp:Label ID="Label6" runat="server">Daten ab Zeile:</asp:Label>
                                            </td>
                                            <td class="firstLeft active" colspan="3">
                                                <asp:TextBox ID="txtDatenAbZeile" runat="server" CssClass="TextBoxNormal" MaxLength="2" Width="40px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" style="height: 36px">
                                                <asp:Label ID="Label7" runat="server">Spalte Kennzeichen:</asp:Label>
                                            </td>
                                            <td class="firstLeft active" colspan="3">
                                                <asp:TextBox ID="txtSpalteKennzeichen" runat="server" CssClass="TextBoxNormal TextUpperCase" MaxLength="2" Width="40px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" style="height: 36px">
                                                <asp:Label ID="Label10" runat="server">Spalte Gebühren:</asp:Label>
                                            </td>
                                            <td class="firstLeft active" colspan="3">
                                                <asp:TextBox ID="txtSpalteGebuehren" runat="server" CssClass="TextBoxNormal TextUpperCase" MaxLength="2" Width="40px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" style="height: 36px">
                                                <asp:Label ID="Label11" runat="server">Spalte Zulassungsdatum:</asp:Label>
                                            </td>
                                            <td class="firstLeft active" colspan="3">
                                                <asp:TextBox ID="txtSpalteZulassungsdatum" runat="server" CssClass="TextBoxNormal TextUpperCase" MaxLength="2" Width="40px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" style="height: 36px">
                                                <asp:Label ID="Label4" runat="server">Excel-Upload:</asp:Label>
                                            </td>
                                            <td class="firstLeft active" colspan="3">
                                                <asp:FileUpload ID="upFile" runat="server"/>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td colspan="4">

                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>
                            </div>

                            <div id="dataQueryFooter">
                                <asp:LinkButton ID="cmdCreate" runat="server" CssClass="Tablebutton" 
                                    Width="78px" onclick="cmdCreate_Click">» Erstellen </asp:LinkButton>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="cmdCreate" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <div id="dataFooter">
                        &nbsp;
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
