<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02_2.aspx.vb" Inherits="AppF1.Change02_2" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../controls/Kopfdaten.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema" />
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    
        
        
    <input type="hidden" value=" " name="txtStorno" />
    <input type="hidden" value=" " name="txtFreigeben" />
    <input type="hidden" value=" " name="txtAuftragsNummer" />
    <input type="hidden" value=" " name="txtVertragsnummer" />
    <input type="hidden" value=" " name="txtAngefordert" />
    <input type="hidden" value=" " name="txtFahrgestellnummer" />
    <input type="hidden" value=" " name="txtBriefnummer" />
    <input type="hidden" value=" " name="txtZulassungsart" />
    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>
                            <asp:Label ID="lblPageTitle" runat="server"> (Werte ändern)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                        </td>
                        <td>
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;<asp:HyperLink ID="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Change02.aspx">Händlersuche</asp:HyperLink>&nbsp;<asp:HyperLink
                                            ID="lnkVertragssuche" runat="server" CssClass="TaskTitle" NavigateUrl="Change02_1.aspx">Händlerauswahl</asp:HyperLink>&nbsp;
                                        <asp:HyperLink ID="lnkDistrikt" runat="server" NavigateUrl="Change02.aspx" CssClass="TaskTitle"
                                            Visible="False">Distriktsuche</asp:HyperLink>
                                        <asp:LinkButton ID="cmdSave" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Sichern</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                        <uc1:Kopfdaten ID="Kopfdaten1" runat="server"></uc1:Kopfdaten>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td class="" nowrap="nowrap" align="left" colspan="2">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap" align="left" colspan="2">
                                                    <asp:Label ID="lblNoData" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap" width="385">
                                                </td>
                                                <td nowrap="nowrap" align="right">
                                                    &nbsp;
                                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="Druck1.aspx" CssClass="StandardButton"
                                                        Target="_blank">•&nbsp;Druckversion</asp:HyperLink>&nbsp; <strong>
                                                            <img alt="" src="../../../images/excel.gif" style="width: 15px; height: 17px" />
                                                            <asp:LinkButton CssClass="ExcelButton" ID="lnkCreateExcel" runat="server">Excelformat</asp:LinkButton></strong>&nbsp;Ergebnisse/Seite:&nbsp;
                                                    <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" PageSize="50" bodyHeight="400"
                                            CssClass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" AllowSorting="True"
                                            AllowPaging="True" AutoGenerateColumns="False" BackColor="White">
                                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                            <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                            <Columns>
                                                <asp:TemplateColumn SortExpression="VBELN" HeaderText="col_Auftragsnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Auftragsnummer" runat="server" CommandName="Sort" CommandArgument="VBELN">col_Auftragsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.VBELN") %>'
                                                            ID="Label1">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="ZZREFNR" HeaderText="col_Vertragsnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="ZZREFNR">col_Vertragsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFNR") %>'
                                                            ID="Label2">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="ZZANFDT" HeaderText="col_Angefordertam">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Angefordertam" runat="server" CommandName="Sort" CommandArgument="ZZANFDT">col_Angefordertam</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZANFDT","{0:d}") %>'
                                                            ID="Label3">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="ZZFAHRG" HeaderText="col_Fahrgestellnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="ZZFAHRG">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZFAHRG") %>'
                                                            ID="Label4">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="ZZBRIEF" HeaderText="col_Briefnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Briefnummer" runat="server" CommandName="Sort" CommandArgument="ZZBRIEF">col_Briefnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZBRIEF") %>'
                                                            ID="Label5">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="ZZFINART" HeaderText="col_Finanzierungsart">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Finanzierungsart" runat="server" CommandName="Sort" CommandArgument="ZZFINART">col_Finanzierungsart</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZFINART") %>'
                                                            ID="Label7">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="BSTZD" HeaderText="col_Kontingentart">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kontingentart" runat="server" CommandName="Sort" CommandArgument="BSTZD">col_Kontingentart</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label8" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.ZZKKBERAnzeige") %>'>
                                                        </asp:Label>
                                                        <asp:Literal ID="Literal2" runat="server" Text='<%# "<a name=""" &amp; DataBinder.Eval(Container, "DataItem.VBELN") &amp; """>" &amp; DataBinder.Eval(Container, "DataItem.ZZKKBERAnzeige") &amp; "</a>" %>'>
                                                        </asp:Literal>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZKKBERAnzeige") %>'>
                                                        </asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn Visible="False" HeaderText="col_Zulassungsart">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Zulassungsart" runat="server" CommandName="Sort" CommandArgument="KVGR3">col_Zulassungsart</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt1" runat="server" Visible='<%# not DataBinder.Eval(Container, "DataItem.InAutorisierung") %>'
                                                            Width="75px" MaxLength="10" Text='<%# DataBinder.Eval(Container, "DataItem.KVGR3") %>'>
                                                        </asp:TextBox>
                                                        <asp:Label ID="Label10" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.InAutorisierung") %>'
                                                            Text='<%# DataBinder.Eval(Container, "DataItem.KVGR3") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Faelligkeit">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Faelligkeit" runat="server" CommandName="Sort" CommandArgument="ZZFAEDT">col_Faelligkeit</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtFaell" runat="server" Visible='<%# not DataBinder.Eval(Container, "DataItem.InAutorisierung") %>'
                                                            Width="60px" Text='<%# DataBinder.Eval(Container, "DataItem.ZZFAEDT", "{0:d}") %>'
                                                            MaxLength="10">
                                                        </asp:TextBox>
                                                        <asp:Label ID="Label12" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.InAutorisierung") %>'
                                                            Text='<%# DataBinder.Eval(Container, "DataItem.ZZFAEDT", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Kunde">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kunde" runat="server" CommandName="Sort" CommandArgument="TEXT50">col_Kunde</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtKunde" runat="server" Visible='<%# not DataBinder.Eval(Container, "DataItem.InAutorisierung") %>'
                                                            Width="75px" Text='<%# DataBinder.Eval(Container, "DataItem.TEXT50") %>' MaxLength="50">
                                                        </asp:TextBox>
                                                        <asp:Label ID="Label13" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.InAutorisierung") %>'
                                                            Text='<%# DataBinder.Eval(Container, "DataItem.TEXT50") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Aktion">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Aktion" runat="server">col_Aktion</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table id="Table11" cellspacing="1" cellpadding="1" width="100%" border="0">
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:LinkButton ID="Linkbutton3" runat="server" CssClass="StandardButtonSmall" Visible='<%# Not DataBinder.Eval(Container, "DataItem.InAutorisierung") %>'
                                                                        Text="Freigeben" CausesValidation="False" CommandName="Freigabe">Freigabe</asp:LinkButton>
                                                                </td>
                                                                <td width="20px">
                                                                </td>
                                                                <td align="left">
                                                                    <asp:LinkButton ID="Linkbutton4" runat="server" CssClass="StandardButtonStorno" Visible='<%# Not DataBinder.Eval(Container, "DataItem.InAutorisierung") %>'
                                                                        Text="Stornieren" CausesValidation="False" CommandName="Storno">Storno</asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" align="center">
                                                                    <asp:Literal ID="Literal4" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.InAutorisierung") %>'
                                                                        Text='<%# "Aut.(" &amp; DataBinder.Eval(Container, "DataItem.Initiator") &amp; ")" %>'>
                                                                    </asp:Literal>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite"
                                                HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td valign="top" align="left">
                            <asp:Label ID="lblLegende" runat="server" Visible="False">*N=Neufahrzeug, S=Selbstfahrervermietfahrzeug, V=Vorführfahrzeug</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td valign="top" align="left">
                            <!--#include File="../../../PageElements/Footer.html" -->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="ShowScript" runat="server" visible="False">
            <td>
                <script language="Javascript" type="text/javascript">
						<!--                    //
                    function FreigebenConfirm(Auftrag, Vertrag, Angefordert, Fahrgest, BriefNr) {
                        var Check = window.confirm("Wollen Sie diesen Kfz-Brief wirklich freigeben?\n\tVertrag\t\t" + Vertrag + "\n\tAngefordert am\t" + Angefordert + "\n\tFahrgestellnr.\t" + Fahrgest + "\t\n\tKfz-Briefnr.\t" + BriefNr);
                        if (Check) {
                            window.document.Form1.txtStorno.value = " ";
                            window.document.Form1.txtFreigeben.value = "X";
                            window.document.Form1.txtAuftragsNummer.value = Auftrag;
                            window.document.Form1.txtVertragsnummer.value = Vertrag;
                            window.document.Form1.txtAngefordert.value = Angefordert;
                            window.document.Form1.txtFahrgestellnummer.value = Fahrgest;
                            window.document.Form1.txtBriefnummer.value = BriefNr;
                            window.document.Form1.txtZulassungsart.value = Zulassungsart;
                        }
                        return (Check);
                    }

                    function DoFreigeben(Auftrag, Vertrag, Angefordert, Fahrgest, BriefNr, Zulassungsart) {
                        window.document.Form1.txtStorno.value = " ";
                        window.document.Form1.txtFreigeben.value = "X";
                        window.document.Form1.txtAuftragsNummer.value = Auftrag;
                        window.document.Form1.txtVertragsnummer.value = Vertrag;
                        window.document.Form1.txtAngefordert.value = Angefordert;
                        window.document.Form1.txtFahrgestellnummer.value = Fahrgest;
                        window.document.Form1.txtBriefnummer.value = BriefNr;
                        window.document.Form1.txtZulassungsart.value = Zulassungsart;
                    }

                    function StornoConfirm(Auftrag, Vertrag, Angefordert, Fahrgest, BriefNr) {
                        var Check = window.confirm("Wollen Sie diesen Kfz-Brief wirklich stornieren?\n\tVertrag\t\t" + Vertrag + "\n\tAngefordert am\t" + Angefordert + "\n\tFahrgestellnr.\t" + Fahrgest + "\t\n\tKfz-Briefnr.\t" + BriefNr);
                        if (Check) {
                            window.document.Form1.txtStorno.value = "X";
                            window.document.Form1.txtFreigeben.value = " ";
                            window.document.Form1.txtAuftragsNummer.value = Auftrag;
                            window.document.Form1.txtVertragsnummer.value = Vertrag;
                            window.document.Form1.txtAngefordert.value = Angefordert;
                            window.document.Form1.txtFahrgestellnummer.value = Fahrgest;
                            window.document.Form1.txtBriefnummer.value = BriefNr;
                        }
                        return (Check);
                    }

                    function DoStorno(Auftrag, Vertrag, Angefordert, Fahrgest, BriefNr) {
                        window.document.Form1.txtStorno.value = "X";
                        window.document.Form1.txtFreigeben.value = " ";
                        window.document.Form1.txtAuftragsNummer.value = Auftrag;
                        window.document.Form1.txtVertragsnummer.value = Vertrag;
                        window.document.Form1.txtAngefordert.value = Angefordert;
                        window.document.Form1.txtFahrgestellnummer.value = Fahrgest;
                        window.document.Form1.txtBriefnummer.value = BriefNr;
                    }
						//-->
                </script>
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
