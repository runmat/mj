<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Versicherungsstatistik.aspx.vb"
    Inherits="KBS.Versicherungsstatistik" MasterPageFile="../KBS.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zur&uuml;ck"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Versicherungsstatistik"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        &nbsp;
                    </div>
                    <div id="TableQuery" style="margin-bottom: 0px">
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr class="formquery">
                                    <td class="firstLeft active" style="white-space:nowrap;">
                                        <asp:Label ID="Label1" runat="server">Suche:</asp:Label>
                                    </td>
                                    <td class="firstLeft" style="white-space:nowrap; vertical-align:bottom;">
                                        <asp:DropDownList ID="ddlFilter" runat="server" Width="80px" AutoPostBack="true">
                                            <asp:ListItem Selected="True">alle</asp:ListItem>
                                            <asp:ListItem>Artikel-Nr.</asp:ListItem>
                                            <asp:ListItem>EVB</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtEVB" runat="server" Visible="false"></asp:TextBox>
                                        <asp:DropDownList ID="ddlArtikel" runat="server" Visible="false">
                                        </asp:DropDownList>
                                        <asp:ImageButton ID="ibtnFilter" runat="server" Visible="false" ImageAlign="Top" ImageUrl="../Images/Filter.gif" Width="24px" Height="24px" />
                                        <asp:ImageButton ID="ibtnNoFilter" runat="server" Visible="false" ImageAlign="Top" ImageUrl="../Images/Unfilter.gif" Width="28px" Height="24px" />                                    
                                    </td>
                                    <td class="firstLeft active" style="white-space:nowrap;">
                                        <asp:Label runat="server">Datum Von:</asp:Label>
                                        <asp:TextBox ID="txtDatumVon" runat="server" Width="80px" Style="margin-left: 5px;"
                                            AutoPostBack="true">
                                        </asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CetxtDatumVon" runat="server" TargetControlID="txtDatumVon"
                                            Animated="False">
                                        </ajaxToolkit:CalendarExtender>
                                        <ajaxToolkit:MaskedEditExtender ID="MeetxtDatumVon" runat="server" TargetControlID="txtDatumVon"
                                            MaskType="Date" Mask="99/99/9999" CultureName="de-DE">
                                        </ajaxToolkit:MaskedEditExtender>
                                    </td>
                                    <td class="firstLeft active" style="white-space:nowrap;">
                                        <asp:Label ID="Label6" runat="server">Bis:</asp:Label>
                                        <asp:TextBox ID="txtDatumBis" runat="server" Width="80px" Style="margin-left: 5px;"
                                            AutoPostBack="true"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CetxtDatumBis" runat="server" TargetControlID="txtDatumBis"
                                            Animated="False">
                                        </ajaxToolkit:CalendarExtender>
                                        <ajaxToolkit:MaskedEditExtender ID="MeetxtDatumBis" runat="server" TargetControlID="txtDatumBis"
                                            MaskType="Date" Mask="99/99/9999" CultureName="de-DE">
                                        </ajaxToolkit:MaskedEditExtender>
                                    </td>
                                    <td width="100%">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="5" align="right" style="padding-right:15px;">
                                        <asp:LinkButton ID="cmdCreate" runat="server" CssClass="Tablebutton" Width="78px">» Suchen</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" colspan="5" style="padding-bottom:5px;">
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                        <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Style="color: Green;"></asp:Label>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="dataQueryFooter" class="dataQueryFooter">
                        &nbsp;
                    </div>
                    <div id="dataHeader" runat="server" style="background-image: url(../Images/overflow.png); color: #ffffff;
                        font-weight: bold; float: left; height: 22px; line-height: 22px; width: 892px;
                        white-space: nowrap; background-color: #2B4C91; padding-left: 15px;" visible="false">
                        Statistik
                    </div>
                    <div id="data">
                        <table cellspacing="0" width="100%" cellpadding="0" bgcolor="white" border="0">
                            <tr>
                                <td>
                                    <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True" Style="padding-left: 15px;"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="gvVersicherungen" runat="server" CssClass="GridView" Width="100%"
                                        AutoGenerateColumns="False" AllowPaging="False" AllowSorting="True" ShowFooter="False"
                                        GridLines="Vertical" Style="border-collapse: collapse ! important;">
                                        <PagerSettings Visible="false" />
                                        <HeaderStyle CssClass="GridTableHead" BorderColor="#595959"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="GridTableAlternate" />
                                        <RowStyle CssClass="ItemStyle" />
                                        <Columns>
                                            <asp:BoundField Visible="false" DataField="MATNR" />
                                            <asp:BoundField DataField="MAKTX" HeaderText="Artikel" />
                                            <asp:BoundField DataField="EVB" HeaderText="Versicherungsnummer" />
                                            <asp:BoundField DataField="PREIS" HeaderText="Preis in &#8364;" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="STORNO" HeaderText="Storno" />
                                            <asp:BoundField DataField="DATUM" HeaderText="Datum" />
                                        </Columns>
                                    </asp:GridView>
                                    <div>
                                        &nbsp;
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="dataFooter" class="dataQueryFooter">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
