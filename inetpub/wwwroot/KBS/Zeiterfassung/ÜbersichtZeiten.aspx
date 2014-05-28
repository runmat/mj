<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ÜbersichtZeiten.aspx.vb"
    Inherits="KBS.ÜbersichtZeiten" MasterPageFile="~/KBS.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" CausesValidation="false" runat="server" Visible="True">zurück</asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%;">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Zeiterfassung"></asp:Label>&nbsp;
                            <asp:Label ID="lblPageTitle" Text="&#220;bersicht" runat="server"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery" style="text-align: right;">
                        <div style=" padding: 3px 8px 3px 10px; font-weight:bold; text-align: left; overflow:visible; white-space:nowrap; width:30%; float:left;">
                            <asp:Label ID="lblUsername" runat="server" ></asp:Label>
                        </div>
                    </div>
                    <div id="TableQuery" runat="server" style="border-right: solid 1px #DFDFDF; border-left: solid 1px #DFDFDF;">
                        <asp:UpdatePanel runat="server" ID="upEingabe">
                            <Triggers>
                            </Triggers>
                            <ContentTemplate>
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="Übersicht" style="border-bottom: solid 1px #DFDFDF; border-right: solid 1px #DFDFDF;
                        border-left: solid 1px #DFDFDF;">
                        <asp:UpdatePanel runat="server" ID="upGrid">
                            <ContentTemplate>                              
                                <div style="border: 1px solid #DFDFDF; margin: 1px 10px 5px 10px;">                                   
                                    <div id="Wochenübersicht" runat="server" style="margin:0px 0px 0px 0px; padding:0px 0px 0px 0px;">
                                        <table width="100%" cellpadding="0" cellspacing="0">                                           
                                            <tr class="formquery" >
                                                <td colspan="8" align="center">
                                                    <asp:GridView ID="GridWoche" runat="server" AutoGenerateColumns="false" Width="100%" 
                                                    AllowPaging="false" AllowSorting="false" ShowFooter="False"
                                                    GridLines="None">
                                                     <PagerSettings Visible="false" />
                                                    <HeaderStyle CssClass="GridTableHead Center"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                        <Columns>
                                                            <asp:BoundField DataField="Tag" HeaderText="Tag" DataFormatString="{0:dd.MM.yyyy}"/>
                                                            <asp:BoundField DataField="Kommen" HeaderText="Kommen" />
                                                            <asp:BoundField DataField="Gehen" HeaderText="Gehen" />                                                          
                                                             <asp:CheckBoxField DataField="RÖffnung" HeaderText="Rüst. Öffn."/>
                                                             <asp:CheckBoxField DataField="RAbrechnung" HeaderText="Rüst. Abrechn."/>
                                                             <asp:CheckBoxField DataField="REinzahlung" HeaderText="Rüst. Einza."/>                                                            
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="dataFooter">
                        <asp:HyperLink ID="hlPrint" runat="server" Text="Drucken" CssClass="Tablebutton" Height="16px" Width="78px" 
                            Target= "_blank" NavigateUrl="PrintZeitübersicht.aspx"></asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
