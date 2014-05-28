<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01.aspx.vb" Inherits="CKG.Components.ComCommon.Logistik.Change01"
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" CausesValidation="false"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text=""></asp:Label>
                        </h1>
                    </div>
                    <div>
                        <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                    </div>
                    <div style="background-color: #dfdfdf; height: 20px">
                        <div style="float: left; padding-left: 15px; padding-top: 3px">
                            <asp:Label ID="lblNewSearch" runat="server" Text="Neue Abfrage" Visible="False"></asp:Label>
                        </div>
                        <div style="float: right; padding-right: 10px; padding-top: 3px">
                            <asp:ImageButton ID="ibtNewSearch" runat="server" ImageUrl="../../../Images/queryArrow.gif"
                                Visible="False" />
                        </div>
                    </div>
                    <asp:Panel ID="Panel2" DefaultButton="lbCreate" runat="server">
                    <asp:UpdatePanel runat="server" ID="UP1"><ContentTemplate>
                        <div id="TableQuery">
                            <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr class="formquery">
                                        <td nowrap="nowrap" class="firstLeft active">
                                        </td>
                                        <td class="firstLeft active" width="100%">
                                            <asp:Label ID="Label1" runat="server" CssClass="TextError"></asp:Label>
                                            <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td nowrap="nowrap" class="firstLeft active">
                                            <asp:Label ID="lbl_AmtlKennzeichen" runat="server" Width="130px"> lbl_AmtlKennzeichen</asp:Label>
                                        </td>
                                        <td class="active" width="100%">
                                            <asp:TextBox ID="txtAmtlKennzeichen" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td nowrap="nowrap" class="firstLeft active">
                                            <asp:Label ID="lbl_Fahrgestellnummer" runat="server" Width="130px">lbl_Fahrgestellnummer</asp:Label>
                                        </td>
                                        <td class="active" width="100%">
                                            <asp:TextBox ID="txtFahrgestellnummer" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td nowrap="nowrap" class="firstLeft active">
                                            <asp:Label ID="lbl_MietendeVon" runat="server" Width="130px">lbl_MietendeVon</asp:Label>
                                        </td>
                                        <td class="active" width="100%">
                                            <asp:TextBox ID="txtMietendeVon" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                            <cc1:CalendarExtender ID="MietendeVon_CE" runat="server" Enabled="True" TargetControlID="txtMietendeVon">
                                            </cc1:CalendarExtender>
                                            <asp:CompareValidator ID="cv_txtMietendeVon" runat="server" ErrorMessage="Falsches Datumsformat"
                                                Type="Date" ControlToValidate="txtMietendeVon" ControlToCompare="TextBox1" Operator="DataTypeCheck"
                                                CssClass="TextError" ForeColor=""></asp:CompareValidator>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td nowrap="nowrap" class="firstLeft active">
                                            <asp:Label ID="lbl_MietendeBis" runat="server" Width="130px">lbl_MietendeBis</asp:Label>
                                        </td>
                                        <td class="active" width="100%">
                                            <asp:TextBox ID="txtMietendeBis" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                            <cc1:CalendarExtender ID="MietendeBis_CE" runat="server" Enabled="True" TargetControlID="txtMietendeBis">
                                            </cc1:CalendarExtender>
                                            <asp:CompareValidator ID="cv_txtMietendeBis" runat="server" ErrorMessage="Falsches Datumsformat"
                                                Type="Date" ControlToValidate="txtMietendeBis" ControlToCompare="TextBox1" Operator="DataTypeCheck"
                                                CssClass="TextError" ForeColor=""></asp:CompareValidator>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td>
                                            <asp:TextBox ID="TextBox1" runat="server" Visible="false"></asp:TextBox>
                                            <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
                                                Width="1px" />
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td style="background-color: #dfdfdf;" colspan="2" width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        </ContentTemplate></asp:UpdatePanel>
                        <div id="dataQueryFooter">
                            <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">» Suchen </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
