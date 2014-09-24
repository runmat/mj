<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change09_1.aspx.vb" Inherits="KBS.Change09_1"
    MasterPageFile="~/KBS.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="JavaScript" type="text/javascript" src="../Java/JScript.js"></script>

    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" CausesValidation="false" runat="server" Visible="True">zurück</asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%;">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label">Inventur</asp:Label>
                            <asp:Label ID="lblPageTitle" runat="server"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="firstLeft active">
                                        Bitte geben Sie hier die vorhandende(n) Menge(n) ein und drücken Sie "Speichern".
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="TableQuery">
                        <asp:UpdatePanel runat="server" ID="upEingabe">
                            <ContentTemplate>
                            
                                <table cellpadding="0" cellspacing="0">
                                    <tfoot>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tfoot>
                                    <tbody>
                                        <tr class="formquery">
                                            <td colspan="2" class="firstLeft active">
                                                <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblProdHBezeichnung" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="width: 100%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr id="trTyp1" runat="server" class="formquery">
                                            <td  colspan="2">
                                                <asp:GridView CssClass="GridView" ID="GridView3" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    AllowPaging="False" AllowSorting="True" ShowFooter="False" 
                                                    GridLines="None" PageSize="999999">
                                                    <PagerSettings Visible="false" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMatnr" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.MATNR") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField  HeaderText="Bezeichnung" DataField="MAKTX" 
                                                            HeaderStyle-HorizontalAlign="Left" >
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField  HeaderText="Artikelnummer" DataField="MATNR"  
                                                            HeaderStyle-HorizontalAlign="Left" >
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField  HeaderText="Verpackungseinheit" DataField="MEINS"  
                                                            HeaderStyle-HorizontalAlign="Left" >
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Menge">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:TextBox MaxLength="4"  style="text-align:right" runat="server" Width="50px" ID="txtMenge"
                                                                 Text='<%# DataBinder.Eval(Container, "DataItem.ERFMG","{0:0}") %>' onFocus="Javascript:this.select();" onKeyPress="return numbersonly(event, false)" ></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="firstLeft active">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" class="firstLeft active" style="width:100%">
                                                <asp:Label ID="lblGesamt" runat="server" Text="Label"></asp:Label>
                                            </td>
                                            <td align="right" class="firstLeft active" style="padding-right:57px">
                                                <asp:Label ID="lblGesamtShow" runat="server"></asp:Label>
                                            </td>                                            
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="firstLeft active">
                                                &nbsp;
                                            </td>
                                        </tr>                                                                                
                                        <tr class="formquery">
                                            <td colspan="2" class="firstLeft active">
                                                <asp:Label ID="lblMessage" CssClass="TextError" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div id="dataFooter">
                            <asp:LinkButton ID="lbAbsenden" Text="Speichern" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton"></asp:LinkButton>
                        </div>
                        <asp:Button ID="MPEDummy" style="display: none" runat="server" />
                         <cc1:ModalPopupExtender runat="server" ID="mpeBestellungsCheck" BackgroundCssClass="divProgress"
                            Enabled="true" PopupControlID="BestellungsCheck" TargetControlID="MPEDummy" 
                            BehaviorID="BestellCheck">
                        </cc1:ModalPopupExtender>                                
                        <asp:Panel ID="BestellungsCheck" runat="server"  style="overflow:auto;height:150px;width:450px;display:none">
                            <table cellspacing="0" id="tblBestellungscheck" runat="server" bgcolor="white"
                                cellpadding="0" style="overflow:auto;height:150px;width:450px;border: solid 1px #646464" >
                                <tr >
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr >
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>                                       
                                <tr>
                                    <td align="center" class="firstLeft active">
                                        <asp:Label ID="lblErrorMenge" runat="server" ></asp:Label> 
                                    </td>
                                </tr>
                                                           
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr> 
                                
                                <tr>
                                    <td align="center">
                                       <asp:LinkButton ID="lbBestellungKorrektur" Text="Korrektur" Height="16px" Width="78px"
                                            runat="server" CssClass="Tablebutton"></asp:LinkButton>
                                            &nbsp; &nbsp;
                                        <asp:LinkButton ID="lbBestellungOk" Text="Ja" Height="16px" Width="78px" runat="server"
                                            CssClass="Tablebutton" ></asp:LinkButton>                                            
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>                                
                            </table>
                        </asp:Panel>                           
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
