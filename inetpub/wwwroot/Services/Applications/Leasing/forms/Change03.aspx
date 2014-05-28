<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Change03.aspx.cs" Inherits="Leasing.forms.Change03"
    MasterPageFile="../Master/App.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="/services/PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .Watermark
        {
            color: Gray;
        }
        .DetailHeader
        {
            background-color: #dfdfdf;
            font-weight: bold;
            color: #4c4c4c;
            height: 22px;
            width: 100%;
            padding-left: 15px;
        }
        .CellStyle
        {
            font-weight: bold;
            color: #4c4c4c;
            padding-left: 15px;
            padding-top: 5px;
        }
        
        .style1
        {
            text-decoration: underline;
        }
      
    </style>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" OnClick="lbBack_Click" CausesValidation="False"></asp:LinkButton>
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
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td class="active">
                                                Neue Abfrage
                                            </td>
                                            <td align="right">
                                                <div id="queryImage">
                                                    <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="../../../Images/queryArrow.gif"
                                                        ToolTip="Abfrage öffnen" Visible="false" OnClick="NewSearch_Click" />
                                                    <asp:ImageButton ID="NewSearchUp" runat="server" ToolTip="Abfrage schließen" ImageUrl="../../../Images/queryArrowUp.gif"
                                                        Visible="false" OnClick="NewSearchUp_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <asp:Panel ID="Panel1" runat="server">
                                <div id="TableQuery" style="margin-bottom: 10px">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="2">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                    <asp:Label ID="lblNoData" runat="server" Font-Bold="True" Visible="False" EnableViewState="False"></asp:Label>
                                                    <asp:Label ID="lblInfo" runat="server" EnableViewState="False" Font-Bold="True" ForeColor="Blue"></asp:Label>
                                                </td>
                                                <td style="width: 100%">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Vertragsnummer:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtVertragsnummer" runat="server" CssClass="TextBoxNormal" Width="200px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    KFZ-Kennzeichen:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtKennzeichen" runat="server" CssClass="TextBoxNormal" Width="200px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Fahrgestellnummer:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtFahrgestellnummer" runat="server" CssClass="TextBoxNormal" Width="200px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td colspan="2">
                                                    <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
                                                        Width="1px" />&nbsp;
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
                                <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px"
                                    OnClick="lbCreate_Click">» Suchen </asp:LinkButton>
                            </div>
                            <div id="Result" runat="Server" visible="false">
                                <table width="100%">
                                    <tbody>
                                        <tr>
                                            <td class="DetailHeader">
                                                Details
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table width="100%" bgcolor="white" border="0" style="border-right-width: 1px;
                                    border-left-width: 1px; border-right-color: #dfdfdf; border-left-color: #dfdfdf;
                                    border-right-style: solid; border-left-style: solid;">
                                    <tr class="formquery">
                                        <td class="CellStyle" width="150">
                                            Vertragsnummer:
                                        </td>
                                        <td class="CellStyle">
                                            <asp:Label ID="lblVetragsnummer" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="CellStyle">
                                            Kfz-Kennzeichen:
                                        </td>
                                        <td class="CellStyle" colspan="3">
                                            <asp:Label ID="lblKennzeichen" runat="server"></asp:Label>&nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="CellStyle">
                                            Fahrgestellnummer:
                                        </td>
                                        <td class="CellStyle">
                                            <asp:Label ID="lblFahrgestellnummer" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="CellStyle">
                                            &nbsp;
                                        </td>
                                        <td class="CellStyle">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="CellStyle" style="border-top-width: 1px; border-top-color: #dfdfdf; border-top-style: solid;">
                                            <span class="style1">Halter:</span>
                                        </td>
                                        <td class="CellStyle" style="border-top-width: 1px; border-top-color: #dfdfdf; border-top-style: solid;">
                                            &nbsp;
                                        </td>
                                        <td class="CellStyle" style="border-top-width: 1px; border-top-color: #dfdfdf; border-top-style: solid;">
                                            <span class="style1">Leasingnehmer:</span>
                                        </td>
                                        <td class="CellStyle" style="border-top-width: 1px; border-top-color: #dfdfdf; border-top-style: solid;">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="CellStyle">
                                            Name1:
                                        </td>
                                        <td class="CellStyle">
                                            <asp:Label ID="lblHalterName1" runat="server"></asp:Label>
                                        </td>
                                        <td class="CellStyle">
                                            Name1:
                                        </td>
                                        <td class="CellStyle">
                                            <asp:Label ID="lblName1" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td class="CellStyle">
                                            Name2:
                                        </td>
                                        <td class="CellStyle">
                                            <asp:Label ID="lblName2" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td class="CellStyle">
                                            Strasse:
                                        </td>
                                        <td class="CellStyle">
                                            <asp:Label ID="lblStrasse" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="CellStyle">
                                            Ort:
                                        </td>
                                        <td class="CellStyle">
                                            <asp:Label ID="lblHalterOrt" runat="server"></asp:Label>
                                        </td>
                                        <td class="CellStyle">
                                            PLZ und Ort:
                                        </td>
                                        <td class="CellStyle">
                                            <asp:Label ID="lblPlzOrt" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table cellpadding="0" cellspacing="0" width="100%" style="border-right-width: 1px;
                                    border-left-width: 1px; border-right-color: #dfdfdf; border-left-color: #dfdfdf;
                                    border-right-style: solid; border-left-style: solid;">
                                    <tbody>
                                        <tr>
                                            <td class="DetailHeader">
                                                Vertragsdaten ändern
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0" style="border-right-width: 1px;
                                    border-left-width: 1px; border-right-color: #dfdfdf; border-left-color: #dfdfdf;
                                    border-right-style: solid; border-left-style: solid;">
                                    <tr>
                                        <td class="CellStyle" width="150">
                                            Neue Vertragsnummer:
                                        </td>
                                        <td class="CellStyle">
                                            <asp:TextBox ID="txtNewVertragsnummer" runat="server" MaxLength="7"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="CellStyle">
                                            Neuer Leasingnehmer:
                                        </td>
                                        <td class="CellStyle">
                                            <div>
                                                <asp:TextBox ID="txtLeasingnehmer" runat="server" Width="650px" AutoPostBack="true"
                                                    OnTextChanged="txtLeasingnehmer_TextChanged"></asp:TextBox>
                                                <ajaxToolkit:TextBoxWatermarkExtender ID="txtLeasingnehmer_TextBoxWatermarkExtender"
                                                    runat="server" Enabled="True" TargetControlID="txtLeasingnehmer" WatermarkCssClass="Watermark"
                                                    WatermarkText="Geben Sie mindestens 3 Zeichen ein um eine Auswahlliste zu erhalten.">
                                                </ajaxToolkit:TextBoxWatermarkExtender>
                                                <ajaxToolkit:AutoCompleteExtender ID="txtLeasingnehmer_AutoCompleteExtender" runat="server"
                                                    DelimiterCharacters="" Enabled="True" ServiceMethod="GetAdressList" ServicePath="ArvalService.asmx"
                                                    TargetControlID="txtLeasingnehmer" UseContextKey="True" MinimumPrefixLength="1" CompletionSetCount="20"
                                                    CompletionInterval="100" EnableCaching="true">
                                                </ajaxToolkit:AutoCompleteExtender>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="CellStyle">
                                            &nbsp;</td>
                                        <td class="CellStyle">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="CellStyle">
                                            &nbsp;</td>
                                        <td class="CellStyle">
                                            &nbsp;</td>
                                    </tr>
                                </table>
                                <div id="dataFooter" style="background-color: #dfdfdf; height: 22px; width: 100%">
                                    &nbsp;<asp:HiddenField ID="hField" runat="server" Value="0" />
                                </div>
                                <div style="float: right; margin-top: 10px; margin-bottom: 31px;">
                                    <asp:LinkButton ID="lbSave" runat="server" CssClass="Tablebutton" Width="78px" OnClick="lbSave_Click">» Speichern </asp:LinkButton>
                                </div>
                            </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="txtLeasingnehmer" EventName="TextChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
    </div>
    </div>
</asp:Content>
