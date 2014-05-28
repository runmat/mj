<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AktuellesPflege.aspx.vb"
    Inherits="Admin.AktuellesPflege" MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        div.thumbnailContainer
        {
            height: 100px;
            width: 100%;
            overflow-y: hidden;
            overflow-x: auto;
            border: 1px solid #999;
        }
        div.thumbnail
        {
            height: 75px;
            width: 75px;
            line-height: 75px;
            text-align: center;
            padding: 4px;
            float: left;
        }
        div.thumbnail img
        {
            max-height: 75px;
            max-width: 75px;
            border: 1px solid #000;
            vertical-align: middle;
        }
    </style>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                &nbsp;
            </div>
            <div id="innerContent">
                <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" />
                <div id="innerContentRight" style="width: 100%">
                    <div>
                        <asp:LinkButton ID="Back" runat="server" Text="Zurück" CssClass="Tablebutton" Width="78px" />
                    </div>
                    <div>
                        &nbsp;</div>
                    <div id="innerContentRightHeading">
                        <h1>
                            Pflege Aktuelles</h1>
                    </div>
                    <div id="adminInput">
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%">
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    Kunde:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlKundenliste" runat="server" AutoPostBack="False">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblKundenliste" runat="server" Visible="false" Width="200" />
                                </td>
                                <td class="firstLeft active">
                                    Beitrag:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBeitrag" runat="server" />
                                    <asp:Label ID="lblBeitragname" runat="server" Visible="false" Width="200" />
                                </td>
                                <td width="100%" colspan="2">
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    Gültig Bis:
                                </td>
                                <td class="active">
                                    <asp:TextBox ID="txtDateBis" runat="server" />
                                    <cc1:CalendarExtender ID="txtDateBis_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtDateBis" />
                                </td>
                                <td colspan="4" width="100%">
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    Aktiv:
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkAktiv" runat="server" Checked="false" border="0" />
                                </td>
                                <td colspan="4" width="100%">
                                </td>
                            </tr>
                            <tr>
                                <td class="firstLeft active" style="vertical-align: top;">
                                    Text:
                                </td>
                                <td colspan="5" width="100%" style="border: 0px; border-left: 1px; border-style: solid;
                                    border-color: #DFDFDF;">
                                    <cc2:Editor ID="Editor1" runat="server" Height="300px" Width="100%" />
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" style="vertical-align: top;">
                                    Vorhandene Bilder:
                                </td>
                                <td colspan="4">
                                    <asp:Repeater runat="server" ID="customerImages">
                                        <HeaderTemplate>
                                            <div class="thumbnailContainer">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div class="thumbnail">
                                                <asp:LinkButton runat="server" OnClick="AddImageClick" CommandArgument='<%# Container.DataItem %>' ToolTip="Bild in Text einfügen">
                                                    <img src='<%# Container.DataItem %>' />
                                                </asp:LinkButton>
                                            </div>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </div></FooterTemplate>
                                    </asp:Repeater>
                                </td>
                                <td width="100%">
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" style="vertical-align: top;">
                                    Neues Bild:
                                </td>
                                <td colspan="4">
                                    <asp:FileUpload ID="fileUpload" runat="server" />
                                    &nbsp;
                                    <asp:LinkButton ID="btnAddImage" runat="server" Text="Hinzufügen" CssClass="Tablebutton" OnClick="AddNewImageClick"
                                        Width="78px" />
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
                            width: 100%;">
                            &nbsp;
                        </div>
                        <div id="save" style="height: 22px; width: 100%;">
                            <asp:LinkButton ID="btnSpeichern" runat="server" Text="Speichern" CssClass="Tablebutton"
                                Width="78px" Style="float: right; margin-top: 4px;" />
                        </div>
                        <div id="Abstand" style="height: 16px; width: 100%;">
                            &nbsp;</div>
                        <div id="Vorschau" runat="server" style="vertical-align: top;">
                            <div id="VorschauHead" style="vertical-align: middle">
                                <h1>
                                    Vorschau</h1>
                            </div>
                            <br />
                            <div id="TableVorschau">
                                <div id="VorschauBody">
                                    <table>
                                        <tr>
                                            <td width="100%" align="left" valign="top">
                                                <asp:Literal ID="ltlVorschau" runat="server" Text="" />
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
