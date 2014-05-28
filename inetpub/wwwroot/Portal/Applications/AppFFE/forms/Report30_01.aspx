<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report30_01.aspx.vb" Inherits="AppFFE.Report30_01" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    
    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td width="1586">
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td width="1587">
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Auswählen)</asp:label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp; &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr id="trVorgangsArt" runat="server">
                                    <td colspan="2">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="" >
                                        <strong>
                                            <asp:label id="lblNoData" runat="server" visible="False"></asp:label></strong>
                                    </td>
                                    <td class="LabelExtraLarge" align="right">
                                        <asp:dropdownlist id="ddlPageSize" runat="server" autopostback="True">
                                        </asp:dropdownlist>
                                    </td>
                                </tr>
                                <tr id="trPageSize" runat="server">
                                    <td class="LabelExtraLarge" align="left" colspan="2" height="39">
                                        <table id="Table2" height="71" cellspacing="1" cellpadding="1" width="812" border="0">
                                            <tr>
                                                <td colspan="6">
                                                    <asp:gridview width="100%" borderwidth="0" boderstyle="none" runat="server" id="gvKontigentarten"
                                                        autogeneratecolumns="false">
                                                        <rowstyle horizontalalign="Center" />
                                                        <columns>
                                                  <asp:TemplateField  HeaderText="col_Temporaer">
                                                  <HeaderTemplate><asp:LinkButton  enabled="false" ID="col_Temporaer" runat="server" >col_Temporaer</asp:LinkButton></HeaderTemplate>
                                                    <ItemTemplate>
                                                   <asp:checkbox id="chkTemporaer"  checked="true"  runat="server" width="77px" >
                                                    </asp:checkbox>
                                                    </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField  HeaderText="col_Endgueltig">
                                                    <HeaderTemplate><asp:LinkButton  enabled="false" ID="col_Endgueltig" runat="server" >col_Endgueltig</asp:LinkButton></HeaderTemplate>
                                                    <ItemTemplate>
                                                   <asp:checkbox id="chkEndgueltig"  checked="True" runat="server" width="77px" >
                                                    </asp:checkbox>
                                                    </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField  HeaderText="col_Retail">
                                                    <HeaderTemplate><asp:LinkButton  enabled="false" ID="col_Retail" runat="server" >col_Retail</asp:LinkButton></HeaderTemplate>
                                                    <ItemTemplate>
                                                   <asp:checkbox id="chkRetail" checked="false" runat="server" width="77px" >
                                                    </asp:checkbox>
                                                    </ItemTemplate>
                                                    </asp:TemplateField>
                                                   
                                                    <asp:TemplateField   HeaderText="col_Delayed">
                                                     <HeaderTemplate><asp:LinkButton  enabled="false" ID="col_Delayed" runat="server" >col_Delayed</asp:LinkButton></HeaderTemplate>
                                                    <ItemTemplate>
                                                   <asp:checkbox id="chkDelayed" checked="false" runat="server" width="77px" >
                                                    </asp:checkbox>
                                                    </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField  HeaderText="col_HEZ">
                                                    <HeaderTemplate><asp:LinkButton  enabled="false" ID="col_HEZ" runat="server" >col_HEZ</asp:LinkButton></HeaderTemplate>
                                                    <ItemTemplate>
                                                   <asp:checkbox id="chkHEZ"  checked="false" runat="server" width="77px" >
                                                    </asp:checkbox>
                                                    </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField  HeaderText="col_KFKL">
                                                    <HeaderTemplate><asp:LinkButton  enabled="false" ID="col_KFKL" runat="server" >col_KFKL</asp:LinkButton></HeaderTemplate>
                                                    <ItemTemplate>
                                                   <asp:checkbox id="chkKFKL"  checked="false" runat="server" width="77px" >
                                                    </asp:checkbox>
                                                    </ItemTemplate>
                                                    </asp:TemplateField>
                                                    </columns>
                                                    </asp:gridview>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <asp:linkbutton id="Linkbutton1" runat="server" visible="True" width="137px" cssclass="StandardButton">Anzeige aktualisieren</asp:linkbutton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="LabelExtraLarge" align="right" height="39">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <img alt="" src="../../../images/excel.gif" style="width: 15px; height: 17px" />
                                        <asp:hyperlink id="lnkExcel" runat="server" target="_blank" cssclass="ExcelButton"
                                            visible="False">Excelformat</asp:hyperlink>&nbsp;
                                        <asp:label id="lblDownloadTip" cssclass="Downloadtip" runat="server" visible="False"
                                            font-size="8pt" font-bold="True">rechte Maustaste => Ziel speichern unter...</asp:label>
                                    </td>
                                </tr>
                                <tr id="trDataGrid1" runat="server">
                                    <td align="middle" colspan="2">
                                        <asp:datagrid id="DataGrid1" runat="server" width="100%" bordercolor="White" pagesize="50"
                                            allowsorting="True" allowpaging="True" autogeneratecolumns="False" bodyheight="350"
                                            bodycss="tableBody" headercss="tableHeader" backcolor="White">
                                            <alternatingitemstyle cssclass="GridTableAlternate"></alternatingitemstyle>
                                            <headerstyle font-bold="True" forecolor="White" cssclass="GridTableHead"></headerstyle>
                                            <columns>
														<asp:TemplateColumn SortExpression="HaendlerNr" HeaderText="H&#228;ndler-Nr.">
															<HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Left"></ItemStyle>
															<ItemTemplate>
																<asp:HyperLink id=HyperLink1 runat="server" NavigateUrl='<%# "Report29_23.aspx?Kunnr=" &amp; DataBinder.Eval(Container, "DataItem.HaendlerNr") %>' Text='<%# DataBinder.Eval(Container, "DataItem.HaendlerNr") %>' Target="_blank">
																</asp:HyperLink>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn   DataField="TmpKontingent" DataFormatString="{0:#####}" HeaderText="Kontingent">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="TmpInanspruchnahme" DataFormatString="{0:#####}" HeaderText="Inanspuch-&lt;br&gt;nahme">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="TmpFreiesKontingent" DataFormatString="{0:#####}" HeaderText="Freies&lt;br&gt;Kontingent">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="TmpFrist" DataFormatString="{0:#####}" HeaderText="Frist">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="TmpSperre"  HeaderText="Sperre">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>														
														<asp:BoundColumn DataField="EndgKontingent" DataFormatString="{0:#####}" HeaderText="Kontingent">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="EndgInanspruchnahme" DataFormatString="{0:#####}" HeaderText="Inanspruch-&lt;br&gt;nahme">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="EndgFreiesKontingent" DataFormatString="{0:#####}" HeaderText="Freies&lt;br&gt;Kontingent">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="EndgFrist" DataFormatString="{0:#####}" HeaderText="Frist">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="EndgSperre"  HeaderText="Sperre">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>															
														<asp:BoundColumn DataField="RetailRichtwert" DataFormatString="{0:#####}" HeaderText="Richtwert">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="RetailAusschoepfung" DataFormatString="{0:#####}" HeaderText="Aussch&#246;pfung">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="RetailFrist" DataFormatString="{0:#####}" HeaderText="Frist">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="RetailSperre"  HeaderText="Sperre">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>															
														<asp:BoundColumn DataField="DelayedRichtwert" DataFormatString="{0:#####}" HeaderText="Richtwert">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="DelayedAusschoepfung" DataFormatString="{0:#####}" HeaderText="Aussch&#246;pfung">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="DelayedFrist" DataFormatString="{0:#####}" HeaderText="Frist">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="DelayedSperre"  HeaderText="Sperre">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>														
														<asp:BoundColumn DataField="HEZRichtwert" DataFormatString="{0:#####}" HeaderText="Richtwert">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="HEZAusschoepfung" DataFormatString="{0:#####}" HeaderText="Aussch&#246;pfung">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="HEZFrist" DataFormatString="{0:#####}" HeaderText="Frist">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="HEZSperre"  HeaderText="Sperre">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>															
														<asp:BoundColumn DataField="KFKLRichtwert" DataFormatString="{0:#####}" HeaderText="Richtwert">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="KFKLAusschoepfung" DataFormatString="{0:#####}" HeaderText="Aussch&#246;pfung">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>	
														<asp:BoundColumn DataField="KFKLFrist" DataFormatString="{0:#####}" HeaderText="Frist">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="KFKLSperre"  HeaderText="Sperre">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>																														
													</columns>
                                            <pagerstyle nextpagetext="N&#228;chste Seite" font-size="12pt" font-bold="True" prevpagetext="Vorherige Seite"
                                                horizontalalign="Left" position="Top" wrap="False" mode="NumericPages"></pagerstyle>
                                        </asp:datagrid>
                                    </td>
                                    <td align="middle">
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
                            <asp:label id="lblError" runat="server" cssclass="TextError" enableviewstate="False">
                            </asp:label>
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
            <td width="1586">

                <script language="Javascript">
						<!-- //
						function FreigebenConfirm(Auftrag,Vertrag,Angefordert,Fahrgest,BriefNr) {
						var Check = window.confirm("Wollen Sie diesen Kfz-Brief wirklich freigeben?\n\tVertrag\t\t" + Vertrag + "\n\tAngefordert am\t" + Angefordert + "\n\tFahrgestellnr.\t" + Fahrgest + "\t\n\tKfz-Briefnr.\t" + BriefNr);
						return (Check);
						}
						//-->
                </script>

            </td>
        </tr>
    </table>
    </form>
</body>
</html>
