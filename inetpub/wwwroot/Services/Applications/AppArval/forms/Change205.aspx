<%@ Page Language="vb" EnableEventValidation="false" AutoEventWireup="false" CodeBehind="Change205.aspx.vb"
    Inherits="AppArval.Change205" MasterPageFile="../../../MasterPage/Services.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="JavaScript">										
						<!--
        function showhide() {
            o = document.getElementById("trDateiauswahl").style;
            if (o.display != "none") {
                o.display = "none";
                document.forms[0].ctl00$ContentPlaceHolder1$txtOrdernummer.disabled = false;
                document.forms[0].ctl00$ContentPlaceHolder1$txtAmtlKennzeichen.disabled = false;
                document.forms[0].ctl00$ContentPlaceHolder1$cbxDatei.checked = false;
              
            } else {
                o.display = "block";
                document.forms[0].ctl00$ContentPlaceHolder1$txtOrdernummer.disabled = true;
                document.forms[0].ctl00$ContentPlaceHolder1$txtAmtlKennzeichen.disabled = true;
                document.forms[0].ctl00$ContentPlaceHolder1$cbxDatei.checked = true;
               
            }
        }
        function openinfo(url) {
            fenster = window.open(url, "Zulassungsdatum", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=650,height=250");
            fenster.focus();
        }
						-->
    </script>

    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                        <div id="paginationQuery">
                            <table cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr>
                                        <td class="firstLeft active">
                                            bitte geben Sie ihre Suchkriterien ein
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div id="TableQuery">
                            <table id="tblSelection" runat="server" cellpadding="0" cellspacing="0">
                                <tfoot>
                                    <tr>
                                        <td colspan="3">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tfoot>
                                <tbody>
                                    <tr class="formquery">
                                        <td colspan="3" class="firstLeft active">
                                            <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="Label1" runat="server">Leasingvertrags-Nr.</asp:Label>
                                        </td>
                                        <td >
                                            <asp:TextBox ID="txtOrdernummer"  runat="server" MaxLength="10" ></asp:TextBox>&nbsp;
                                               (1234567)
                                        </td>
                                        <td  width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="Label2" runat="server">Kfz-Kennzeichen*</asp:Label>
                                        </td>
                                        <td >
                                            <asp:TextBox ID="txtAmtlKennzeichen" runat="server" MaxLength="9" ></asp:TextBox>&nbsp;
                                                (XX-Y1234)
                                        </td>
                                        <td  width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td colspan="3" class="firstLeft active">
                                            *Eingabe von nachgestelltem Platzhalter möglich. Mindestens Kreis und
                                                ein Buchstabe (z.B. XX-Y*)
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:HyperLink Height="16px" Width="78px"  cssclass="Tablebutton"  runat="server" Text="Dateiauswahl" NavigateUrl="javascript:showhide()">Dateiauswahl</asp:HyperLink>&nbsp;
                                            <a href="javascript:openinfo('Info01.htm');"><img src="/Portal/Images/fragezeichen.gif"  /></a>
                                        </td>
                                        <td class="firstLeft active">
                                            <table id="Table3" style="border-style:none" >
                                           
                                                <tr id="trDateiauswahl" style="display:none">
                                                        <td>
                                                        <input  id="upFile" type="file"  size="40" name="File1" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="cmdContinue" runat="server" Height="16px" Width="78px" CssClass="Tablebutton"> Weiter</asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:Label ID="lblExcelfile" runat="server"></asp:Label>
                                        </td>
                                        <td class="firstLeft active" width="100%">
                                            <input id="cbxDatei" style="display: none" type="checkbox" name="cbxDatei" runat="server">
                                        </td>
                                    </tr>
                                        <tr class="formquery">
                                        <td colspan="3" class="firstLeft active">
                                         &nbsp;
                                        </td>
                                    </tr>
                                    
                                </tbody>
                            </table>
                        </div>
                        <div id="dataFooter">
                            <asp:LinkButton ID="cmdSearch" Text="Suchen" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton"></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
   </asp:Content>
