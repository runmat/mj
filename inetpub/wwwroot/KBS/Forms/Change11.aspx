<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change11.aspx.vb" Inherits="KBS.Change11" MasterPageFile="~/KBS.Master" %>
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
                            <asp:Label ID="lblHead" runat="server" Text="Label">Inventur Sonstige Lagerware</asp:Label>
                            <asp:Label ID="lblPageTitle" runat="server"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="firstLeft active">
                                        &nbsp;
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="TableQuery"> 
                        <table cellspacing="0" width="100%" cellpadding="0" bgcolor="white" border="0">
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="2">
                                    <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" style="font-size:14px; font-weight:normal" align="left" colspan="2">
                                    Bitte erfassen Sie hier in der vorgegebenen Tabelle die Lagerware. In die erste Spalte tragen Sie die Artikelbezeichnung ein.<br />
                                    Halten Sie sich bei der Artikelbezeichnung und  Reihenfolge bitte genau an die Vorlage (Blatt Sonstige Lagerware), die Ihnen<br /> 
                                    mit den Jahresinventurunterlagen zugeschickt wurde. In der zweiten Spalte tragen Sie bitte die gezählte Menge zu dem<br /> 
                                    jeweiligen Artikel ein. Wenn Sie einen der aufgeführten Artikel nicht im Sortiment haben, tragen Sie diesen bitte trotzdem ein,<br /> 
                                    mit der Menge &#8222;0&#8220;. Sollten Sie Lagerware besitzen, die Sie nicht einem der vorgegebenen Artikel zuordnen können, setzen<br /> 
                                    Sie sich mit Ahrensburg in Verbindung. Nach Erfassung aller Artikel drücken Sie auf Speichern und  bestätigen Sie den<br /> 
                                    Speichervorgang im darauf erscheinenden Fenster. Anschließend wird ein Inventur-Formular erzeugt. <b>Bitte drucken Sie<br />
                                    dieses in zweifacher Ausfertigung aus und unterschreiben es mit allen an der Inventur teilgenommenen<br />
                                    Mitarbeitern/innen! </b> Ein Exemplar ist für Ihre Unterlagen und ein Exemplar schicken Sie nach Ahrensburg. 
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="2">         
                                    <asp:GridView CssClass="GridView" ID="GridView2" runat="server" Width="50%" 
                                        AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false">
                                        <PagerSettings Visible="false" />
                                        <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="GridTableAlternate" />
                                        <RowStyle CssClass="ItemStyle" />
                                        <Columns>
                                            <asp:BoundField DataField="Position" HeaderText="Position" />
                                            <asp:TemplateField HeaderText="Artikelbezeichnung" HeaderStyle-HorizontalAlign="Left">         
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtArtikel" Width="200px" onFocus="Javascript:this.select();" runat="server"
                                                        Text='<%# Eval("Artikel") %>' Enabled='<%# Eval("Vorbelegt") = False %>' MaxLength="40" ></asp:TextBox>
                                                </ItemTemplate>     
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Menge" HeaderStyle-HorizontalAlign="Left">        
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtMenge"  Width="50px" onFocus="Javascript:this.select();" runat="server"
                                                        Text='<%# Eval("Menge") %>' onKeyPress="return numbersonly(event, false)" ></asp:TextBox>
                                                </ItemTemplate>  
                                            </asp:TemplateField>
                                        </Columns>                                                      
                                    </asp:GridView>
                                </td>
                            </tr>                                 
                        </table>
                        <div id="dataFooter">
                            <asp:LinkButton ID="lbAbsenden" 
                                Text="Speichern" Height="16px" Width="78px" runat="server" CssClass="Tablebutton"></asp:LinkButton>
                            <asp:LinkButton ID="lbNachdruck"
                                Text="Nachdruck" Height="16px" Width="78px" runat="server" CssClass="Tablebutton"
                                Visible="false"></asp:LinkButton>
                        </div>
                        <asp:Panel ID="PLCheck" runat="server" style="overflow:auto;height:500px;width:500px;display:none;" >
                            <table cellspacing="0" id="tblCheck" runat="server" width="100%" bgcolor="#FFFFFF"
                                cellpadding="0" border="0" style="border: solid 1px #000000" > 
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>                              
                                <tr>
                                    <td align="center" class="firstLeft active" >
                                        <asp:Label ID="Info" runat="server">Bitte überprüfen Sie Ihre Eingaben, ungewöhnliche Werte sind Rot markiert!<br />
                                            Bitte korrigieren Sie gegebenenfalls!<br /></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>                                 
                                <tr>
                                    <td>
                                        <asp:GridView ID="GridView1" runat="server" AllowPaging="False" AllowSorting="True"
                                            AutoGenerateColumns="False" BackColor="White" CssClass="GridView" GridLines="None"
                                            HorizontalAlign="Center" ShowFooter="False"  style="overflow:auto; width:75%" >
                                            <PagerSettings Visible="false" />
                                            <HeaderStyle CssClass="GridTableHead" />
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <Columns>
                                                <asp:BoundField HeaderStyle-HorizontalAlign="Left" DataField="Artikel" HeaderText="Artikel" />
                                                <asp:BoundField HeaderStyle-HorizontalAlign="Left" DataField="Menge" HeaderText="Menge" />  
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>                                 
                                <tr>
                                    <td align="center">
                                        <asp:LinkButton ID="lbCancel" Text="Korrektur" Height="16px" Width="78px"
                                            runat="server" CssClass="Tablebutton"></asp:LinkButton>
                                        &nbsp; &nbsp;                                    
                                        <asp:LinkButton ID="lbOk" Text="Weiter" Height="16px" Width="78px" runat="server"
                                            CssClass="Tablebutton"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Button ID="MPEDummy" Width="0" Height="0" style="display:none" runat="server" />
                        <cc1:ModalPopupExtender runat="server" ID="mpeCheck" BackgroundCssClass="divProgress"
                            Enabled="true" PopupControlID="PLCheck" TargetControlID="MPEDummy" CancelControlID="lbCancel">
                        </cc1:ModalPopupExtender>
                   </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
