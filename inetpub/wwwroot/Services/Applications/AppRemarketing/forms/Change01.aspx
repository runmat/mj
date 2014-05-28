<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Change01.aspx.cs" Inherits="AppRemarketing.forms.Change01"
    MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UP1">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lbHaendler" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="txtNummerDetail" EventName="TextChanged" />
                            <asp:AsyncPostBackTrigger ControlID="txtName1" EventName="TextChanged" />
                            <asp:AsyncPostBackTrigger ControlID="txtPLZ" EventName="TextChanged" />
                            <asp:AsyncPostBackTrigger ControlID="txtOrt" EventName="TextChanged" />
                        </Triggers>
                        <ContentTemplate>
                            <div id="TableQuery">
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0" width="100%" style="width: 100%">
                                    <tbody>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="2">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError" 
                                                    EnableViewState="False"></asp:Label>
                                                <asp:Label ID="lblMessage" runat="server" Font-Bold="True" 
                                                    EnableViewState="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="trHaendlernummer" class="formquery" runat="server">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lbl_HaendlerNummer" runat="server"></asp:Label>
                                            </td>
                                            <td class="active" style="width: 100%">
                                            <asp:TextBox ID="txtNummerDetail" runat="server" MaxLength="10" AutoPostBack="True"
                                                    OnTextChanged="txtNummerDetail_TextChanged"></asp:TextBox>
                                                <asp:Label ID="lblSHistoryNR" runat="server" Font-Italic="True" Font-Size="10" ForeColor="#c1ccd9"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="trName1" runat="server" class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label runat="server" ID="lbl_Name1">Name:</asp:Label>
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtName1" runat="server" MaxLength="35" AutoPostBack="True" OnTextChanged="txtName1_TextChanged"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trPLz" runat="server" class="formquery">
                                            <td class="firstLeft active">
                                                PLZ:
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtPLZ" runat="server" MaxLength="35" AutoPostBack="True" OnTextChanged="txtPLZ_TextChanged"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trOrt" runat="server" class="formquery">
                                            <td class="firstLeft active">
                                                Ort:
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtOrt" runat="server" MaxLength="35" Width="200px" AutoPostBack="True"
                                                    OnTextChanged="txtOrt_TextChanged"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="Tr_SelectionButton" runat="server" class="formquery">
                                            <td colspan="2" class="firstLeft active" style="height: 57px">
                                                Anzahl Treffer:
                                                <asp:Label ID="lblErgebnissAnzahl" runat="server" Width="40"></asp:Label><br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="right" style="width: 100%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr id="tr_HaendlerAuswahl" runat="server" >
                                            <td colspan="2" class="firstLeft active">
                                                <div style="float: left">
                                                    <asp:ListBox ID="lbHaendler" runat="server" Width="500px" Height="126px" AutoPostBack="True"
                                                        OnSelectedIndexChanged="lbHaendler_SelectedIndexChanged"></asp:ListBox>
                                                </div>
                                                <div>
                                                    <b>&nbsp;&nbsp;
                                                        <asp:Label ID="lblHalter" runat="server" Font-Size="11pt"></asp:Label></b><br />
                                                    &nbsp;&nbsp;
                                                    <asp:Label ID="lblHaendlerName1" runat="server" Font-Size="10pt"></asp:Label><br />
                                                    &nbsp;&nbsp;
                                                    <asp:Label ID="lblHaendlerName2" runat="server" Font-Size="10pt"></asp:Label><br />
                                                    &nbsp;&nbsp;
                                                    <asp:Label ID="lblHaendlerStrasse" runat="server" Font-Size="10pt"></asp:Label><br />
                                                    <br />
                                                    <b>&nbsp;&nbsp;<asp:Label ID="lblHaendlerPLZ" runat="server" Font-Size="10pt"></asp:Label>
                                                        <br />
                                                        &nbsp;&nbsp;<asp:Label ID="lblHaendlerOrt" runat="server" Font-Size="10pt"></asp:Label>
                                                    </b>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="right" style="width: 100%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="right" style="width: 100%">
                                                <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                                    &nbsp;
                                                </div>
                                            </td>
                                        </tr>
                                </table>
                                <div id="dataQueryFooter">
                                    <asp:LinkButton ID="cmdSearch" runat="server" CssClass="TablebuttonLarge" Width="130px"
                                        Height="16px" OnClick="cmdSearch_Click">» Weiter</asp:LinkButton>&nbsp;
                                    <asp:LinkButton ID="lbSelektionZurueckSetzen" runat="server" CssClass="TablebuttonLarge"
                                        Width="130px" Height="16px" onclick="lbSelektionZurueckSetzen_Click1">» Neue Suche</asp:LinkButton></div>
                            </div>
                            <div id="dataFooter">
                                &nbsp;</div>
                            </div></ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
