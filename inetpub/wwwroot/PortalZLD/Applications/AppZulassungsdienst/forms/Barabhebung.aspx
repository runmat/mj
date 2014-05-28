<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Barabhebung.aspx.cs" Inherits="AppZulassungsdienst.forms.Barabhebung"    MasterPageFile="../MasterPage/App.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück"></asp:LinkButton>
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
                            <asp:Panel ID="Panel1" runat="server">
                                <div id="TableQuery" style="margin-bottom: 10px">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="2" width="100%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>   
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblName" runat="server">Name:</asp:Label>
                                                </td>
                                                <td class="active" style="width: 100%">
                                                    <asp:TextBox ID="txtName" runat="server" CssClass="TextBoxNormal" 
                                                        MaxLength="30"></asp:TextBox>
                                                  </td>
                                            </tr>                                            
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblKst" runat="server">Kst.:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtKst" runat="server"  CssClass="TextBoxNormal" 
                                                        MaxLength="30"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblNummerEC" runat="server">Nr. der EC-Karte:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtNummerEC"  runat="server" CssClass="TextBoxNormal" 
                                                        MaxLength="30" />
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    &nbsp;</td>
                                                <td class="active">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblDatum" runat="server">Datum:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtDatum" runat="server" CssClass="TextBoxNormal" 
                                                        MaxLength="30" />
                                                </td>
                                            </tr>  
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblUhrzeit" runat="server">Uhrzeit:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtUhrzeit" runat="server" CssClass="TextBoxNormal" 
                                                        MaxLength="30" />
                                                </td>
                                            </tr> 
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblOrt" runat="server">Ort der Barabhebung:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtOrt" runat="server" CssClass="TextBoxNormal" 
                                                        MaxLength="30" />
                                                </td>
                                            </tr> 
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblBetrag" runat="server">Betrag:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtBetrag" runat="server" CssClass="TextBoxNormal" 
                                                        MaxLength="30" />
                                                </td>
                                            </tr>                                                                                                                                                                            
                                            <tr class="formquery">
                                                <td colspan="2">

                                                    <cc1:CalendarExtender ID="txtDatum_CalendarExtender" runat="server" Format="dd.MM.yyyy"
                                                        PopupPosition="BottomLeft" Animated="true" Enabled="True" TargetControlID="txtDatum">
                                                    </cc1:CalendarExtender>
                                                    <cc1:MaskedEditExtender ID="meeDatum" runat="server" TargetControlID="txtDatum"
                                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                    </cc1:MaskedEditExtender>  
                                                    <cc1:MaskedEditExtender ID="meeZeit" runat="server" TargetControlID="txtUhrzeit"
                                                        Mask="99:99" MaskType="Time" InputDirection="LeftToRight">
                                                    </cc1:MaskedEditExtender>                                                                                                           
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
                                    Width="78px" onclick="cmdCreate_Click">» Absenden </asp:LinkButton>
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
