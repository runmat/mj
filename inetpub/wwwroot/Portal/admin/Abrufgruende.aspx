<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Abrufgruende.aspx.vb"
    Inherits="CKG.Admin.Abrufgruende" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Register Assembly="BusyBoxDotNet" Namespace="BusyBoxDotNet" TagPrefix="busyboxdotnet" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema" />
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body>
    <form id="Form1" method="post" runat="server">

    <script type="text/javascript" language="javascript" id="ScrollPosition">
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



            document.forms["Form1"].xCoordHolder.value = scrollX;
            document.forms["Form1"].yCoordHolder.value = scrollY;

        }

        function sstchur_SmartScroller_Scroll() {


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
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td >
                <uc1:header id="ucHeader" runat="server">
                </uc1:header>
            </td>
        </tr>
           <tr>
            <td width="100%" colspan="3">
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td  width="100%"class="PageNavigation" colspan="4">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;<asp:Label ID="lbl_PageTitle"
                                runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td   class="TaskTitle"  >
                           <asp:LinkButton ID="lb_zurueck" Visible="True" CausesValidation="false" runat="server">zurück</asp:LinkButton>  
                        </td>
                        <td  width="100%" class="TaskTitle" colspan="3" >
                     &nbsp;
                        </td>
                       
                    </tr>
                    <tr>
                        <td valign="top" >
                            &nbsp;
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                      
                        <td colspan="2">
                            <table cellpadding="4" cellspacing="4">
                                <tr>
                                    <td>
                                        Firma:
                                    </td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="ddlFilterCustomer" runat="server" Visible="True" Width="160px"
                                            AutoPostBack="True" Height="20px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                <td>
                                &nbsp;
                                </td>
                                    <td colspan="5">
                                       <asp:Label runat="server" ID="lblError" CssClass="TextError"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td colspan="5">
                                        <table>
                                            <tr>
                                                <td>
                                                    Web Bezeichnung:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtWebBezeichnung" MaxLength="150" 
                                                        Width="250px"></asp:TextBox> &nbsp;
                                                   <asp:RequiredFieldValidator ID="rfvWebBezeichnung"  Display="Dynamic"
                                                        runat="server" ControlToValidate="txtWebBezeichnung"  ErrorMessage="Bitte füllen Sie das Feld Web Bezeichnung"></asp:RequiredFieldValidator>
                                                   
                                                </td>
                                                <td>
                                                    Sap Wert:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtSapWert" MaxLength="3" Width="50px"></asp:TextBox>
                                                  &nbsp;<asp:RequiredFieldValidator ID="rfvSapWert"  Display="Dynamic"
                                                        runat="server" ControlToValidate="txtSapWert"  ErrorMessage="Bitte füllen Sie das Feld SAP Wert"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Zusatzeingabe:
                                                </td>
                                                <td>
                                                    <asp:CheckBox runat="server" ID="chkZusatzeingabe" />
                                                    
                                                </td>
                                                <td>
                                                    Zusatzbemerkung:
                                                </td>
                                                <td>
                                                    <asp:TextBox Rows="3" TextMode="MultiLine" MaxLength="150" runat="server" ID="txtZusatzbemerkung"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Abruftyp:
                                                </td>
                                                <td colspan="3">
                                                    <asp:RadioButtonList runat="server" ID="rblTyp">
                                                        <asp:ListItem Text="temporär" Value="temp"></asp:ListItem>
                                                        <asp:ListItem Text="endgültig" Value="endg"></asp:ListItem>
                                                    </asp:RadioButtonList> <br />
                                                    <asp:RequiredFieldValidator ID="rfvrblTyp" 
                                                        runat="server" ControlToValidate="rblTyp"  ErrorMessage="Bitte wählen Sie einen AbrufTyp"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td >
                                                    &nbsp;
                                                    Einschränkung:</td>
                                                <td colspan="3">
                                                    &nbsp;
                                                    <asp:DropDownList ID="ddlEingeschraenkt" runat="server" Visible="True" Width="50px"
                                                         Height="20px">
                                                        <asp:ListItem>0</asp:ListItem>
                                                        <asp:ListItem>1</asp:ListItem>
                                                        <asp:ListItem>2</asp:ListItem>
                                                        <asp:ListItem>3</asp:ListItem>
                                                        <asp:ListItem></asp:ListItem>
                                                    </asp:DropDownList>
&nbsp;* abhängig von Gruppeneinstellung Autorizierungslevel</td>                                                
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" align="right">
                                                    <asp:LinkButton ID="lbEintragen" Text="Eintragen" runat="server" CssClass="StandardButton"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr style="font-weight: bold">
                                    <td align="center" valign="top">
                                        temporär
                                    </td>
                                    <td colspan="5" style="width:100%">
                                                                            <asp:GridView CellPadding="2" ID="gvTemporaer" CssClass="tableMain" AutoGenerateColumns="false"
                                            AllowPaging="false" BackColor="White" Width="100%" runat="server">
                                            <HeaderStyle HorizontalAlign="Left" ForeColor="White" CssClass="GridTableHead" />
                                            <PagerStyle Font-Size="12pt" Font-Bold="True" HorizontalAlign="Left" Wrap="False">
                                            </PagerStyle>
                                            <PagerSettings Mode="Numeric" Position="Top" />
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Entfernen" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton CausesValidation="false" runat="server" ToolTip="Dieses Fahrzeug aus Übernahmetabelle entfernen"
                                                            ID="imgDelete" Height="14" CommandName="loesch" Width="14" ImageUrl="../Images/loesch.gif"
                                                            CommandArgument='<%# DataBinder.Eval(Container, "DataItem.SapWert") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="mit Zusatzeingabe" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" Enabled="false" ID="chkZusatzeingabe" Checked='<%# Cbool(DataBinder.Eval(Container, "DataItem.MitZusatzText")) %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Web Bezeichnung" DataField="WebBezeichnung" />
                                                <asp:BoundField HeaderText="SAP Wert" DataField="SapWert" />
                                                <asp:BoundField ItemStyle-Width="400px" HeaderText="Zusatzbemerkung" DataField="Zusatzbemerkung" />
                                                <asp:TemplateField HeaderText="eingeschränkt" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" Enabled="false" ID="chkeingeschraenkt" Checked='<%# Cbool(DataBinder.Eval(Container, "DataItem.Eingeschraenkt")) %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                                
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        &nbsp;
                                    </td>
                                </tr>
                                
                                <tr style="font-weight: bold">
                                    <td valign="top" align="center">
                                        endgültig
                                    </td>
                                    <td colspan="5">
                                        <asp:GridView CellPadding="2" ID="gvEndgueltig" CssClass="tableMain" AutoGenerateColumns="false"
                                            AllowPaging="false" BackColor="White" Width="100%" runat="server">
                                            <HeaderStyle HorizontalAlign="Left" ForeColor="White" CssClass="GridTableHead" />
                                            <PagerStyle Font-Size="12pt" Font-Bold="True" HorizontalAlign="Left" Wrap="False">
                                            </PagerStyle>
                                            <PagerSettings Mode="Numeric" Position="Top" />
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Entfernen" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton  CausesValidation="false" runat="server" ToolTip="Dieses Fahrzeug aus Übernahmetabelle entfernen"
                                                            ID="imgDelete" Height="14" CommandName="loesch" Width="14" ImageUrl="../Images/loesch.gif"
                                                           CommandArgument='<%# DataBinder.Eval(Container, "DataItem.SapWert") %>'  />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="mit Zusatzeingabe" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" Enabled="false" ID="chkZusatzeingabe" Checked='<%# Cbool(DataBinder.Eval(Container, "DataItem.MitZusatzText")) %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Web Bezeichnung" DataField="WebBezeichnung" />
                                                <asp:BoundField HeaderText="SAP Wert" DataField="SapWert" />
                                                <asp:BoundField ItemStyle-Width="400px" HeaderText="Zusatzbemerkung" DataField="Zusatzbemerkung" />
                                                <asp:TemplateField HeaderText="eingeschränkt" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" Enabled="false" ID="chkeingeschraenkt" Checked='<%# Cbool(DataBinder.Eval(Container, "DataItem.Eingeschraenkt")) %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                                
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                             
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3" valign="top" align="left">
                <!--#include File="../PageElements/Footer.html" -->
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
