<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change11.aspx.vb" Inherits="AppEC.Change11" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<html>
    <head>
        <uc1:styles id="ucStyles" runat="server"></uc1:styles>
    </head>

    <body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
        <form id="Form1" method="post" enctype="multipart/form-data" runat="server">
        <table width="100%" align="center">
            <tr>
                <td colspan="2">
                    <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellspacing="0" cellpadding="0" width="100%" border="0" align="center">
                        <tbody>
                            <tr>
                                <td class="PageNavigation" colspan="2">
                                    <asp:Label ID="lblHead" runat="server"></asp:Label>
                                    <asp:Label ID="lblPageTitle" runat="server" Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="TaskTitle" colspan="2">
                                    Bitte wählen Sie eine lokal gespeicherte Excel-Datei zur Übertragung aus.
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" width="50" height="192">
                                </td>
                                <td valign="top" align="right">
                                    <div align="left">
                                        <table id="tblSelection" cellspacing="0" cellpadding="0" width="100%" border="0"
                                            runat="server" align="left">
                                            <tr>
                                                <td valign="top" align="left">
                                                    <table cellspacing="0" cellpadding="5" width="100%" border="0">
                                                        <tr>
                                                            <td valign="top" align="left" width="100%">
                                                                &nbsp;
                                                                <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" align="left" width="100%">
                                                                <table cellspacing="1" cellpadding="1" border="0" style="margin-left:150px">
                                                                    <tr id="trDateiauswahl">
                                                                        <td>
                                                                            <asp:Label id="lblMarke" runat="server">Marke:</asp:Label>
                                                                            <asp:DropDownList ID="ddlMarke" runat="server"></asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblUp" runat="server" Text="Upload: "></asp:Label>
                                                                            <input class="InfoBoxFlat" id="upFile" type="file" size="40" name="File1" runat="server" />
                                                                            &nbsp;
                                                                            <a>
                                                                                <img src="/Portal/Images/fragezeichen.gif" alt=""
                                                                                    border="0" title='Volkswagen: (Excel-Datei mit Spaltenüberschriften: Spalte I=Kennung, Spalte L=Auftragsnummer, Spalte N=Fahrgestellnummer, Mappenname="Bestellungen") Sonstige: (Excel-Datei mit Spaltenüberschriften: Spalte A=Fahrgestellnummer, Spalte B=Kennung, Spalte C=Auftragsnummer, Mappename="Tabelle1")' />
                                                                            </a>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <asp:LinkButton ID="cmdContinue" runat="server" CssClass="StandardButtonTable"> &#149;&nbsp;Weiter&nbsp;&#187;</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table width="70%">
                                                        <tr>
                                                            <td id="ExcelCell" runat="server" visible="false" align="right" style="padding-right:100px;padding-top:20px">
                                                                <strong>&nbsp;<img alt="" src="../../../images/excel.gif" style="width: 16px; height: 16px" />&nbsp;
                                                                    <asp:LinkButton CssClass="ExcelButton" ID="lnkCreateExcel" runat="server">Excelformat</asp:LinkButton>
                                                                    &nbsp;&nbsp; </strong>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                                                                    AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                                                                    bodyCSS="tableBody" CssClass="tableMain" DataKeyNames="CHASSIS_NUM" 
                                                                    headerCSS="tableHeader" PageSize="50" Visible ="False" 
                                                                    style="margin-left:150px" Width="69%">
                                                                    <HeaderStyle CssClass="GridTableHead" Wrap="False" />
                                                                    <PagerStyle CssClass="TextExtraLarge" Wrap="False" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="CHASSIS_NUM" HeaderText="Fahrgestellnummer" />
                                                                        <asp:BoundField DataField="ZAUFTRAGS_NR" HeaderText="Auftragsnummer" />
                                                                        <asp:BoundField DataField="LIZNR" HeaderText="Auftragsnummer Equi" />
                                                                        <asp:BoundField DataField="UNIT_NR" HeaderText="Unit-Nr." />
                                                                        <asp:BoundField DataField="ZMODELL" HeaderText="Model ID" ItemStyle-Wrap="False" />
                                                                        <asp:BoundField DataField="ZMODEL_ID" HeaderText="Model ID zur Unit-Nr." ItemStyle-Wrap="False" />
                                                                        <asp:BoundField DataField="REPLA_DATE" HeaderText="Datum Zulassung" />
                                                                        <asp:BoundField DataField="LICENSE_NUM" HeaderText="Kennzeichen" ItemStyle-Wrap="False" />
                                                                        <asp:BoundField DataField="BEM" HeaderText="Status" ItemStyle-Wrap="False" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="right">
                    <!--#include File="../../../PageElements/Footer.html" -->
                </td>
            </tr>
        </table>
        </form>
    </body>
    <asp:literal id="Literal1" runat="server"></asp:literal>
</html>