<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01_2.aspx.vb" Inherits="AppCommonCarRent.Change01_2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../../../PageElements/BusyIndicator.ascx" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head id="Head1" runat="server">
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:styles id="ucStyles" runat="server">
    </uc1:styles>
  
</head>
<body leftmargin="0" topmargin="0">
    
    <uc1:BusyIndicator runat="server" />

    <form id="Form1" method="post" runat="server">

    <script language="javascript" id="ScrollPosition">
<!--
		
  
		
	function sstchur_SmartScroller_GetCoords()
   {
		
      var scrollX, scrollY;
      
      if (document.all)
      {
         if (!document.documentElement.scrollLeft)
            scrollX = document.body.scrollLeft;
         else
            scrollX = document.documentElement.scrollLeft;
               
         if (!document.documentElement.scrollTop)
            scrollY = document.body.scrollTop;
         else
            scrollY = document.documentElement.scrollTop;
      }   
      else
      {
         scrollX = window.pageXOffset;
         scrollY = window.pageYOffset;
      }
      
      
		
      document.forms["Form1"].xCoordHolder.value = scrollX;
      document.forms["Form1"].yCoordHolder.value = scrollY;
     
   }
   
   function sstchur_SmartScroller_Scroll()
   {
  
			
      var x = document.forms["Form1"].xCoordHolder.value;
      var y = document.forms["Form1"].yCoordHolder.value;
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
    <asp:ScriptManager ID="scriptmanager1" EnableScriptGlobalization="true" runat="server">
    </asp:ScriptManager>
    <table id="Table4" width="100%" align="center">
        <tr>
            <td>
                <uc1:header id="ucHeader" runat="server">
                </uc1:header>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tbody>
                        <tr>
                            <td class="PageNavigation" colspan="2">
                                <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;<asp:Label ID="lbl_PageTitle"
                                    runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TaskTitle" valign="top" colspan="2">
                                &nbsp;<asp:LinkButton ID="lb_zurueck" runat="server" Visible="True">lb_zurueck</asp:LinkButton>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                &nbsp;
            </td>
        </tr>
       
       
        <tr>
            <td valign="top" colspan="3">
                <table width="100%" cellpadding="3" style="border-color: #f5f5f5; border-style: solid;
                    border-width: 3;" runat="server" visible="true" id="tableAuswahl">
                    <tr>
                        <td align="center">
                       
                            <asp:ImageButton ID="imgbExcel" ImageAlign="Right" runat="server" Height="20px" ImageUrl="../../../Images/excel.gif"
                                Visible="True" Width="20px" />
                            <asp:UpdatePanel ID="upGridAnzeige" runat="server">
                                <ContentTemplate>
                                                <p  align="left">
                                                    
                                                    <asp:Label ID="lblGvFahrzeugeNoData" Font-Bold="true" runat="server" Visible="false"
                                                        Text="keine Daten vorhanden"></asp:Label>&nbsp;
                                                    <asp:Label ID="lblAnzeigeAnzahlAusgewaehlt" Font-Bold="true" runat="server">Anzahl ausgewählter Fahrzeuge:</asp:Label>
                                                    &nbsp;
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                    
                                                </p>
                                  
                                     
                                        
                                   
                                   
                                                <asp:GridView ID="gvFahrzeuge" runat="server" AutoGenerateColumns="False" Visible="false"
                                                    AllowSorting="True" CssClass="tableMain" AllowPaging="false" BackColor="White"
                                                    Width="100%">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="White" CssClass="GridTableHead" />
                                                    <PagerStyle Font-Size="12pt" Font-Bold="True" HorizontalAlign="Left" Wrap="False">
                                                    </PagerStyle>
                                                    <PagerSettings Mode="Numeric" Position="Top" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <Columns>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEQUNR" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-Width="100px" HeaderText="col_Auswahl" ItemStyle-HorizontalAlign="Center"
                                                            HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                                                                             
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblZulassungskennzeichen" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Zulassungskennzeichen ") %>'>
                                                                </asp:Label>
                                                                <asp:Label ID="lblStatus" CssClass="texterror" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Status ") %>'>
                                                                </asp:Label>
                                                                <asp:ImageButton runat="server" ToolTip="Dieses Fahrzeug aus Zulassungstabelle entfernen" ID="imgbloesch" Height="14" CommandName="loeschen"
                                                                    Width="14" ImageUrl="../../../Images/loesch.gif" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Lizensnehmer" >
                                                            <HeaderTemplate>
                                                                <table cellspacing="0" style="font-weight: bold" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_LizensnehmerTEXT" runat="server" CommandName="Sort" CommandArgument="LizensnehmerTEXT ">col_LizensnehmerTEXT</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_BrandingTEXT" runat="server" CommandName="Sort" CommandArgument="BrandingTEXT">col_BrandingTEXT</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Zuldat" runat="server" CommandName="Sort" CommandArgument="Zuldat">col_Zuldat</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr>
                                                                        <td nowrap="nowrap">
                                                                            <asp:Label ID="Label21qa213" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LizensnehmerTEXT ") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td nowrap="nowrap">
                                                                            <asp:Label ID="Label21qwqws13e" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BrandingTEXT") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td nowrap="nowrap">
                                                                            <asp:Label ID="Label22paxevwq12a" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Zuldat","{0:d}") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Laufzeit">
                                                            <HeaderTemplate>
                                                                <table cellspacing="0" style="font-weight: bold" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Laufzeit" runat="server" CommandName="Sort" CommandArgument="Laufzeit">col_Laufzeit</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_EVB" runat="server" CommandName="Sort" CommandArgument="EVBTEXT">col_EVB</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <%--<tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Sicherungsscheinpflichtig" runat="server" CommandName="Sort"
                                                                                CommandArgument="Sicherungsscheinpflichtig">col_Sicherungsscheinpflichtig</asp:LinkButton>
                                                                        </td>
                                                                    </tr>--%>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Laufzeit") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label12aqq2q1ss1s21w" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.EVBTEXT") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <%--<tr>
                                                                        <td>
                                                                            <asp:CheckBox   runat="server" ID="cbx_Sicherungsscheinpflichtig"  Enabled="False"
                                                                                Checked='<%# DataBinder.Eval(Container, "DataItem.Sicherungsscheinpflichtig")="X" %>' />
                                                                        </td>
                                                                    </tr>--%>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Hersteller">
                                                            <HeaderTemplate>
                                                                <table cellspacing="0" style="font-weight: bold" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Hersteller" runat="server" CommandName="Sort" CommandArgument="Hersteller">col_Hersteller</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Typ" runat="server" CommandName="Sort" CommandArgument="Typ">col_Typ</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Farbe" runat="server" CommandName="Sort" CommandArgument="Farbe">col_Farbe</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr>
                                                                        <td nowrap="nowrap">
                                                                            <asp:Label ID="Label21q" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Hersteller") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td nowrap="nowrap">
                                                                            <asp:Label ID="Label21qwqwe" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Typ") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td nowrap="nowrap">
                                                                            <asp:Label ID="Label22paxevwq" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Farbe") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Fahrgestellnummer">
                                                            <HeaderTemplate>
                                                                <table cellspacing="0" style="font-weight: bold" cellpadding="0" border="0">
                                                                     <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                     <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Aufbauart" runat="server" CommandName="Sort" CommandArgument="Aufbauart">col_Aufbauart</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_ZBIINummer" runat="server" CommandName="Sort" CommandArgument="Navi">col_ZBIINummer</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:HyperLink ID="lnkFahrgestellnummer" Target="_blank" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'
                                                                                runat="server"></asp:HyperLink>
                                                                        </td>
                                                                    </tr>
                                                                    
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label12aqq2q1ss1w23" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Aufbauart") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label12aqq2q1ss1w" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZBIINummer") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Eingangsdatum">
                                                            <HeaderTemplate>
                                                                <table cellspacing="0" style="font-weight: bold" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Eingangsdatum" runat="server" CommandName="Sort" CommandArgument="Eingangsdatum">col_Eingangsdatum</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Referenz" runat="server" CommandName="Sort" CommandArgument="Referenz">col_Referenz</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr>
                                                                    </tr>
                                                                    <td>
                                                                        <asp:Label ID="Label21" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Eingangsdatum","{0:dd.MM.yyyy}") %>'>
                                                                        </asp:Label>
                                                                    </td>
                                                                    <tr>
                                                                    </tr>
                                                                    <td>
                                                                        <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Referenz") %>'>
                                                                        </asp:Label>
                                                                    </td>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Getriebe" ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Middle">
                                                            <HeaderTemplate>
                                                                <table cellspacing="0" style="font-weight: bold" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Getriebe" runat="server" CommandName="Sort" CommandArgument="Getriebe">col_Getriebe</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Kraftstoffart" runat="server" CommandName="Sort" CommandArgument="Kraftstoffart">col_Kraftstoffart</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label12aq" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Getriebe") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label12aqq2qsw" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kraftstoffart") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Bereifung" ItemStyle-HorizontalAlign="left"
                                                            ItemStyle-VerticalAlign="Middle">
                                                            <HeaderTemplate>
                                                                <table cellspacing="0" style="font-weight: bold" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Bereifung" runat="server" CommandName="Sort" CommandArgument="Bereifung">col_Bereifung</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="col_Reifengroesse" runat="server" CommandName="Sort" CommandArgument="Reifengroesse">col_Reifengroesse</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                          
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label12aqq2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Bereifung") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label12aqq2q" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Reifengroesse") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Navi" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Navi" runat="server" CommandName="Sort" CommandArgument="Navi">col_Navi</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label12aqq2q1sw" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Navi") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_CoC" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_CoC" runat="server" CommandName="Sort" CommandArgument="CoC">col_CoC</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox runat="server" ID="cbx_COC" Enabled="false" Checked='<%# DataBinder.Eval(Container, "DataItem.CoC")="X" %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                       
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Literal runat="server" ID="lvl2GridStart">
                                                        
                                                                </asp:Literal>
                                                                <table cellpadding="3" cellspacing="0" border="0" width="100%">
                                                                    <tr>
                                                                        <td width="93px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td width="100px">
                                                                            Standort:
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="Label12aqq2q1sw21a" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Standort") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td width="93px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td width="100px">
                                                                            Absender:
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Absender") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td width="93px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td width="100px">
                                                                            Versand Schein/Schilder:
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="Label5" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.AbwScheinSchilderTEXT")="-" %>'
                                                                                Text='<%# DataBinder.Eval(Container, "DataItem.Standort") %>'>
                                                                            </asp:Label>
                                                                            <asp:Label ID="Label4" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.AbwScheinSchilderTEXT")<>"-" %>' Text='<%# DataBinder.Eval(Container, "DataItem.AbwScheinSchilderTEXT") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3">
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <asp:Literal runat="server" ID="lbl3GridEnd" Text="</td></tr>">
                                                                </asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:LinkButton ID="lb_Zulassen" Text="zulassen" OnClientClick="Show_BusyBox1();"
                                            runat="server" CssClass="StandardButton"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <!--#include File="../../../PageElements/Footer.html" -->
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
