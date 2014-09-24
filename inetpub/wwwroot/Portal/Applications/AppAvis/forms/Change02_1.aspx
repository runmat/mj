<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02_1.aspx.vb" Inherits="AppAvis.Change02_1" %>
<%@ Register TagPrefix="DBWC" Namespace="DBauer.Web.UI.WebControls" Assembly="DBauer.Web.UI.WebControls.HierarGrid" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">

    <script language="JavaScript" type="text/javascript" src="../Javascript/Slideup_Down.js"></script>

    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        div.transbox
        {
            width: 100%;
            height: 100%;
            background-color: #ffffff;
            filter: alpha(opacity=60);
            opacity: 0.6;
        }
        .styleInput
        {
            border-style: none;
            border-color: inherit;
            border-width: medium;
            width: 25px;
            height: 19px;
            text-align: center;
            bottom: 0px;
            background-color: #FFFFCC;
        }
    </style>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" name="Form1" method="post" runat="server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
<%--    <input type="hidden" value="" name="Zuldat" />--%>
    <input type="hidden" value="empty" name="PDINummer" />
    <input type="hidden" value="empty" name="NummerInGrid" />
    <table id="Table4" width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Auswahl von Modellen bzw. Fahrzeugen)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <tr>
                                    <td class="TaskTitle">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdWeiter" runat="server" CssClass="StandardButton">&#149;&nbsp;Weiter</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdAbsenden" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Absenden</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdVerwerfen" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Verwerfen</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdWeitereAuswahl" runat="server" CssClass="StandardButton" Visible="False">•&nbsp;Zurück</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdZurueck" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Zurück</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        <asp:HyperLink ID="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Change02.aspx">Carport - Suche</asp:HyperLink>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td class="BorderLeftBottom" width="100px">
                                                                        <asp:DropDownList ID="ddlDatumswahl" runat="server" AutoPostBack="True">
                                                                            <asp:ListItem Selected="True" Value="Zulassung">Zulassungsdatum</asp:ListItem>
                                                                            <asp:ListItem Value="Planzulassung">Planzulassungsdatum</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td width="75px" class="BorderLeftBottom" valign="middle">
                                                                        <asp:Label ID="lblZulassungsdatum" runat="server" Width="75px"/>
                                                                        <input type="hidden" id="HiddenZulassungsdatum" value="" runat="server" />
                                                                        <div id="DivCalendarZulassungsdatum" style="position:absolute; display:none">
                                                                            <asp:Calendar ID="calZulassungsdatum" runat="server" BackColor="White" BorderColor="#999999"
                                                                                CellPadding="1" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black">
                                                                                <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                                                                <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                                                <SelectorStyle BackColor="#CCCCCC" />
                                                                                <WeekendDayStyle BackColor="#FFFFCC" />
                                                                                <OtherMonthDayStyle ForeColor="#808080" />
                                                                                <NextPrevStyle VerticalAlign="Bottom" />
                                                                                <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                                                                                <TitleStyle BackColor="#999999" Font-Size="7pt" BorderColor="Black" Font-Bold="True" />
                                                                            </asp:Calendar>
                                                                        </div>
                                                                    </td>
                                                                    <td width="40px" class="BorderLeftBottom" valign="baseline">
                                                                        <%--Buttonname muss btn + Divname sein--%>
                                                                        <input type="button" id="btnDivCalendarZulassungsdatum" onclick='DisplayCalendar("DivCalendarZulassungsdatum")' 
                                                                            style="border: none; background-image: url(../../../Images/calendar_red.jpg);
                                                                            width: 16px; height: 16px; cursor: hand; position: static;" runat="server" />
                                                                        &nbsp;
                                                                        <asp:ImageButton ID="ibtnDelZulassungsdatum" runat="server" 
                                                                            ImageUrl="../../../Images/loesch.gif" Height="13px" Width="13px" 
                                                                            ToolTip="Datum entfernen!" ImageAlign="AbsMiddle" />
                                                                    </td>
                                                                    <td class="BorderLeftBottom" width="120px">
                                                                        <asp:Label runat="server" Text="Zulassungen ges."></asp:Label>
                                                                    </td>
                                                                    <td class="BorderLeftBottom" width="25px" align="center">
                                                                        <input type="text" class="styleInput" name="ZulassungGesamt" runat="server" id="GesamtAnzahl"
                                                                            readonly="readonly" />
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="BorderLeftBottom" width="100px">
                                                                        <asp:Label runat="server" ID="lblTitelVerarbeitungsdatum" Visible="False">
                                                                            Verarbeitungsdatum
                                                                        </asp:Label>
                                                                    </td>
                                                                    <td width="75px" class="BorderLeftBottom" valign="middle">
                                                                        <asp:Label ID="lblVerarbeitungsdatum" runat="server" Width="75px" Visible="False"/>
                                                                        <input type="hidden" id="HiddenVerarbeitungsdatum" value="" runat="server" />
                                                                        <div id="DivCalendarVerarbeitungsdatum" style="position:absolute; display:none">
                                                                            <asp:Calendar ID="calVerarbeitungsdatum" runat="server" BackColor="White" BorderColor="#999999"
                                                                                CellPadding="1" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black">
                                                                                <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                                                                <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                                                <SelectorStyle BackColor="#CCCCCC" />
                                                                                <WeekendDayStyle BackColor="#FFFFCC" />
                                                                                <OtherMonthDayStyle ForeColor="#808080" />
                                                                                <NextPrevStyle VerticalAlign="Bottom" />
                                                                                <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                                                                                <TitleStyle BackColor="#999999" Font-Size="7pt" BorderColor="Black" Font-Bold="True" />
                                                                            </asp:Calendar>
                                                                        </div>
                                                                    </td>
                                                                    <td width="40px" class="BorderLeftBottom" valign="baseline">
                                                                        <%--Buttonname muss btn + Divname sein--%>
                                                                        <input type="button" id="btnDivCalendarVerarbeitungsdatum" onclick='DisplayCalendar("DivCalendarVerarbeitungsdatum")' 
                                                                            style="border: none; background-image: url(../../../Images/calendar_red.jpg);
                                                                            width: 16px; height: 16px; cursor: hand; position: static;" runat="server" Visible="False"/>
                                                                        &nbsp;
                                                                        <asp:ImageButton ID="ibtnDelVerarbeitungsdatum" runat="server" 
                                                                            ImageUrl="../../../Images/loesch.gif" Height="13px" Width="13px" 
                                                                            ToolTip="Datum entfernen!" ImageAlign="AbsMiddle" Visible="False"/>
                                                                    </td>
                                                                    <td class="BorderLeftBottom" width="120px">
                                                                        <asp:Label runat="server" Text="aktuelle Auswahl:"></asp:Label>
                                                                    </td>
                                                                    <td class="BorderLeftBottom" width="25px" align="center">
                                                                        <input type="text" class="styleInput" name="Aktuell" runat="server" id="AktuelleAnzahl"
                                                                            readonly="readonly" />
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td class="BorderLeftBottom" width="120px">
                                                                        <asp:Label runat="server" Text="aktuell Gesamt:"></asp:Label>
                                                                
                                                                    </td>
                                                                    <td class="BorderLeftBottom" width="25px" align="center">
                                                                        <input type="text" class="styleInput" name="AktuellGesamt" readonly="readonly" runat="server"
                                                                            id="AktuelleSumme" />
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr id="rowExcelLink" runat="server">
                                    <td class="TextLarge" valign="center">
                                        <asp:ImageButton ID="lnkCreateExcel" ImageUrl="~/Images/excel.gif" runat="server"
                                            Height="16px" Width="16px" ImageAlign="Top" Visible="False"></asp:ImageButton>
                                        &nbsp;<asp:LinkButton ID="lnkCreateExcel2" class="TextLarge" Visible="False" runat="server">Excelformat</asp:LinkButton>
                                    </td>
                                </tr>
                                 <tr id="rowSelectionExcel" runat="server">
                                    <td class="TextLarge" valign="center">
                                        <asp:ImageButton ID="imgbExcel" ImageUrl="~/Images/excel.gif" runat="server"
                                            Height="16px" Width="16px" ImageAlign="Top" Visible="true"></asp:ImageButton>
                                        &nbsp;<asp:LinkButton ID="lnkExcel" class="TextLarge" Visible="true" runat="server">Excelformat</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left">
                                        <DBWC:HierarGrid ID="HG1" Style="z-index: 101" runat="server" TemplateDataMode="Table"
                                            LoadControlMode="UserControl" TemplateCachingBase="Tablename" CellPadding="0"
                                            BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#999999"
                                            Width="100%" AutoGenerateColumns="False">
                                            <AlternatingItemStyle CssClass="GridTableAlternate" />
                                            <ItemStyle CssClass="GridTableItem" />
                                            <HeaderStyle CssClass="GridTableHead" />
                                            <Columns>
                                                <asp:TemplateColumn Visible="False" SortExpression="Details" HeaderText="Details">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkDetails" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Details") %>'>
                                                        </asp:CheckBox>
                                                        <asp:CheckBox ID="chkLoaded" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Loaded") %>'>
                                                        </asp:CheckBox>
                                                    </ItemTemplate>
                                                    <FooterStyle Wrap="False"></FooterStyle>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Carportnr" HeaderText="Carportnr." SortExpression="Carportnr">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Carport" HeaderText="Carport" SortExpression="Carport">
                                                    <HeaderStyle Width="70%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Zulassungsf_Fahrzeuge" HeaderText="Zulassungs-&lt;br&gt;fähige&lt;br&gt;Fahrzeuge"
                                                    SortExpression="Zulassungsf_Fahrzeuge">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Gesperrte_Fahrzeuge" HeaderText="Gesperrte&lt;br&gt;Fahrzeuge"
                                                    SortExpression="Gesperrte_Fahrzeuge">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundColumn>
                                            </Columns>
                                        </DBWC:HierarGrid>
                                      
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td class="LabelExtraLarge">
                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td>
                            <!--#include File="../../../PageElements/Footer.html" -->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <input id="SelOpen2" type="hidden" size="1" runat="server" />
    </table>


    <script language="JavaScript" type="text/javascript">
		<!--        //
        function DisableControls() {
            for (var i = 0; i < window.document.Form1.elements.length; ++i) {
                if (window.document.Form1.elements[i].type == "checkbox") {
                    window.document.Form1.elements[i].disabled = true;
                }
                if (window.document.Form1.elements[i].type == "text") {
                    window.document.Form1.elements[i].disabled = true;
                }
                if (window.document.Form1.elements[i].name == "ImageButton1") {
                    window.document.Form1.elements[i].disabled = true;
                }
            }
        }

        function DetailsSuchen(PDI, Zeile, Task) {
            window.document.Form1.PDINummer.value = PDI;
            window.document.Form1.NummerInGrid.value = Zeile;
        }

        function CountChecked(obj, ID) {
            var count;
            var count2;


            if (obj.checked) {
                count = Number(document.getElementById("AktuelleAnzahl").value);
                count = count + 1;
                document.getElementById("AktuelleAnzahl").value = count;
                count2 = Number(document.getElementById("AktuelleSumme").value);
                count2 = count2 + 1;
                document.getElementById("AktuelleSumme").value = count2;
                if (window.document.Form1.HiddenZulassungsdatum.value != '') {
                    ID.value = window.document.Form1.HiddenZulassungsdatum.value;
                }

            }
            else {
                count = Number(document.getElementById("AktuelleAnzahl").value);
                count = count - 1;
                document.getElementById("AktuelleAnzahl").value = count;
                count2 = Number(document.getElementById("AktuelleSumme").value);
                count2 = count2 - 1;
                document.getElementById("AktuelleSumme").value = count2;
                ID.value = '';
            }

        }
 
		//-->
    </script>

    <asp:Literal ID="litAddScript" runat="server"></asp:Literal>
    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
    <asp:Literal ID="Literal2" runat="server"></asp:Literal>
    </form>
</body>
</html>
