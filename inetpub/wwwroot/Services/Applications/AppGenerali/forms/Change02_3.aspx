<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02_3.aspx.vb" Inherits="AppGenerali.Change02_3"
    MasterPageFile="../../../MasterPage/Services.Master" %>
<%@ Register Assembly="BusyBoxDotNet" Namespace="BusyBoxDotNet" TagPrefix="busyboxdotnet" %>
<%@ Register TagPrefix="uc1" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" id="ScrollPosition">
<!--

        function sstchur_SmartScroller_GetCoords() {

            var scrollX, scrollY;

            if (document.all) {
                if (!document.documentElement.scrollLeft)
                    scrollX = document.body.scrollLeft;
                else
                    scrollX = document.documentElement.scrollLeft;

                if (!document.documentElement.scrollTop)
                    scrollY = document.body.scrollTop;
                else
                    scrollY = document.documentElement.scrollTop;
            }
            else {
                scrollX = window.pageXOffset;
                scrollY = window.pageYOffset;
            }



            document.forms["aspnetForm"].ctl00_ContentPlaceHolder1_xCoordHolder.value = scrollX;
            document.forms["aspnetForm"].ctl00_ContentPlaceHolder1_xCoordHolder.value = scrollY;

        }

        function sstchur_SmartScroller_Scroll() {


            var x = document.forms["aspnetForm"].ctl00_ContentPlaceHolder1_xCoordHolder.value;
            var y = document.forms["aspnetForm"].ctl00_ContentPlaceHolder1_xCoordHolder.value;
            window.scrollTo(x, y);

        }

        window.onload = sstchur_SmartScroller_Scroll;
        window.onscroll = sstchur_SmartScroller_GetCoords;
        window.onkeypress = sstchur_SmartScroller_GetCoords;
        window.onclick = sstchur_SmartScroller_GetCoords;
  
// -->
    </script>

    <input type="hidden" id="xCoordHolder" runat="server" />
    <input type="hidden" id="yCoordHolder" runat="server" />
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton runat="server" ID="lb_zurueck" Text="zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div id="TableQuery">
                    <div id="statistics">
                        <table  id="tblAnzeigeVersandDaten" cellpadding="0" cellspacing="0">
                            <tr id="tr_VersandAdressArt">
                                <td>
                                    <asp:Label ID="lbl_VersandAdressArt" Text="lbl_VersandAdressArt" runat="server"></asp:Label>
                                </td>
                                <td  >
                                    <asp:Label ID="lblVersandAdressArtAnzeige" Font-Bold="true" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr id="tr_Versandadresse">
                                <td>
                                    <asp:Label ID="lbl_Versandadresse" Text="lbl_Versandadresse" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblVersandadresseAnzeige"  Font-Bold="true" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr id="tr_Versandart">
                                <td>
                                    <asp:Label ID="lbl_Versandart" Text="lbl_Versandart" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblVersandartAnzeige" Font-Bold="true" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr id="tr_Versandgrund">
                                <td>
                                    <asp:Label ID="lbl_Versandgrund" Text="lbl_Versandgrund" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblVersandgrundAnzeige" Font-Bold="true" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>


</div>


                    <div id="pagination">
                       <uc1:GridNavigation ID="GridNavigation1" runat="server" />
                    </div>
                    <div id="data">

                        <table cellspacing="0" width="100%" cellpadding="0" bgcolor="white" border="0">
                            <tbody>
                             <tr>
                            <td>
                              <asp:Label ID="lblError" runat="server" Visible="false" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                                                                          <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                       
                                        
                            </td>
                            </tr>
                                <tr>
                                    <td>
                                        <asp:DataGrid CssClass="GridView" ID="DataGrid1" runat="server" PageSize="50" Width="100%"
                                            AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" ShowFooter="False"
                                            GridLines="None">
                                            <PagerStyle  Visible="false"/>
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingItemStyle CssClass="GridTableAlternate" />
                                            <ItemStyle CssClass="ItemStyle" />
                                            <Columns>
                                                <asp:BoundColumn Visible="False" DataField="EQUNR"></asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="Leasingvertragsnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="Leasingvertragsnummer" runat="server" CommandName="Sort" CommandArgument="Leasingvertragsnummer">Leasingvertragsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label21" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Leasingvertragsnummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Suchname">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="Suchname" runat="server" CommandName="Sort" CommandArgument="Suchname">Suchname </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelq1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Suchname") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Leasingnehmer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="Leasingnehmer" runat="server" CommandName="Sort" CommandArgument="Leasingnehmer">Leasingnehmer </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelq11" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Leasingnehmer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Kennzeichen">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">Kennzeichen
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelxc1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Fahrgestellnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="lnkFahrgestellnummer" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'
                                                            ToolTip="Anzeige Fahrzeughistorie" Target="_blank">
                                                        </asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="NummerZBII">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="NummerZBII" CommandArgument="NummerZBII" CommandName="Sort" runat="server">NummerZBII</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2y" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NummerZBII") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Abmeldedatum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="Abmeldedatum" runat="server" CommandName="Sort" CommandArgument="Abmeldedatum">Abmeldedatum
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labelc1X" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Abmeldedatum","{0:dd.MM.yyyy}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="CoCvorhanden">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="CoCvorhanden" runat="server" CommandName="Sort" CommandArgument="CoCvorhanden">CoCvorhanden
                                                        </asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chbCoCvorhanden" Enabled="false" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.CoCvorhanden") ="X" %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Entfernen">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="Entfernen" runat="server">Entfernen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton Visible="true" ID="lbEntfernen" CommandName="entfernen" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>'
                                                            runat="server"><img src=../../../ApplicationImages/loesch.gif border=0> </asp:LinkButton>
                                                        <asp:Label Visible="false" ID="lblMessage" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="dataFooter">
                        <asp:LinkButton ID="lb_Weiter" Text="Anfordern" Height="16px" Width="78px" runat="server"
                            CssClass="Tablebutton" OnClientClick="Show_ctl00_ContentPlaceHolder1_BusyBox1();"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
