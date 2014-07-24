<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MailPflege.aspx.vb"
    Inherits="Admin.MailPflege" MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                &nbsp;
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div>
                        <asp:LinkButton ID="Back" runat="server" Text="Zurück" CssClass="Tablebutton" Width="78px"></asp:LinkButton>
                    </div>
                    <div>
                        &nbsp;</div>
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Pflege Aktuelles"></asp:Label>
                        </h1>
                    </div>
                    <div id="TableQuery">
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%">
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="6" style="width: 100%">
                                    <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    <asp:Label ID="lblKunde" runat="server" Text="Kunde:"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlKundenliste" runat="server" AutoPostBack="False">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblKundenliste" runat="server" Visible="false" Width="200"></asp:Label>
                                </td>
                                <td class="firstLeft active">
                                    <asp:Label ID="lblBeitrag" runat="server" Text="Beitrag:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBeitrag" runat="server"></asp:TextBox>
                                    <asp:Label ID="lblBeitragname" runat="server" Visible="false" Width="200"></asp:Label>
                                </td>
                                <td width="100%" colspan="2">
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    <asp:Label ID="lblDateBis" runat="server" Text="Gültig Bis:"></asp:Label>
                                </td>
                                <td class="active">
                                    <asp:TextBox ID="txtDateBis" runat="server"></asp:TextBox><cc1:CalendarExtender ID="txtDateBis_CalendarExtender"
                                        runat="server" Enabled="True" TargetControlID="txtDateBis">
                                    </cc1:CalendarExtender>
                                </td>
                                <td colspan="4" width="100%">
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    <asp:Label ID="lblAktiv" runat="server" Text="Aktiv:"></asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkAktiv" runat="server" Checked="false" border="0" />
                                </td>
                                <td colspan="4" width="100%">
                                </td>
                            </tr>
                            <tr>
                                <td class="firstLeft active" style="vertical-align: top;">
                                    <asp:Label ID="lblEditor" runat="server" Text="Text:"></asp:Label>
                                </td>
                                <td colspan="5" width="100%" style="border: 0px; border-left: 1px; border-style: solid;
                                    border-color: #DFDFDF;">
                                    <cc2:Editor ID="Editor1" runat="server" Height="300px" Width="100%" />
                                </td>
                            </tr>
                            <tr id="trBildbehalten" runat="server" class="formquery">
                                <td class="firstLeft active">
                                    <asp:Label ID="lblBildbehalten" runat="server" Text="Bild behalten:" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkBildbehalten" runat="server" Checked="true" AutoPostBack="true"
                                        border="0" />
                                </td>
                                <td width="100%" colspan="4">
                                </td>
                            </tr>
                            <tr id="trFileupload" runat="server" class="formquery" visible="false">
                                <td class="firstLeft active">
                                    <asp:Label ID="lblBild" runat="server" Text="Bild:"></asp:Label>
                                </td>
                                <td colspan="4">
                                    <asp:FileUpload ID="fileUpload" runat="server" Style="width: 400px;" />&nbsp;
                                </td>
                                <td width="100%">
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="6" style="width: 100%">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;
                            text-align: bottom; width: 100%;">
                            &nbsp;
                        </div>
                        <div id="save" style="height: 22px; width: 100%;">
                            <asp:LinkButton ID="btnSpeichern" runat="server" Text="Speichern" CssClass="Tablebutton"
                                Width="78px" Style="float: right; margin-top: 4px;" />
                        </div>
                        <div id="Abstand" style="height: 16px; width: 100%;">
                            &nbsp;</div>
                        <div id="Vorschau" runat="server" style="vertical-align:top;">
                            <div id="VorschauHead" style="vertical-align: middle">
                                <h1>
                                    <asp:Label ID="Label1" runat="server" Text="Vorschau"></asp:Label>
                                </h1>
                            </div>
                            <br />
                            <div id="TableVorschau">                            
                                <div id="VorschauBody">
                                    <table>
                                        <tr>
                                            <td width="100%" align="left" valign="top">
                                                <asp:Image ID="imgVorschau" runat="server" AlternateText="Vorschaubild" Style="max-height: 350px;"
                                                    align="center" valign="middle" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="100%" align="left" valign="top">
                                                <asp:Literal ID="ltlVorschau" runat="server" Text=""></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="100%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="Vorschaufooter" runat="server" style="background-color: #FFFFFF; height: 22px;">
                                    &nbsp;
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
