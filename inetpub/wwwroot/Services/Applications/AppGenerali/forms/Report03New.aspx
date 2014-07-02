<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report03New.aspx.vb" Inherits="AppGenerali.Report03New" MasterPageFile="../../../MasterPage/Services.Master" %>
<%@ Register assembly="BusyBoxDotNet" namespace="BusyBoxDotNet" tagprefix="busyboxdotnet" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">&nbsp;
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    
                    <ContentTemplate>                        
                    <div id="paginationQuery" style="width: 100%; display: block;">
                        
                            <table cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr>
                                        <td class="active">
                                            Neue Abfrage starten
                                        </td>
                                        <td align="right">
                                            <div id="queryImage">
                                                <input type="button" id="NewSearch" onclick="ShowHide('tab1');" alt="Neue Abfrage"
                                                    style="border: none; background-image: url(../../../Images/queryArrow.gif); width: 16px;
                                                    height: 16px; cursor: hand;" runat="server" />
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div> 
                        <div id="TableQuery" style="margin-bottom: 10px">
                            <table id="tab1" cellpadding="0" cellspacing="0" >
                            <tfoot ><tr><td colspan="4">&nbsp;</td></tr></tfoot>
                                <tr  class="formquery" >
                                    <td >
                                        &nbsp;
                                    </td>
                                    <td nowrap="nowrap" width="100%">
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="formquery" >
                                    <td  nowrap="nowrap" class="firstLeft active" >
                                        Versicherungsjahr<span lang="de">:</span>&nbsp;
                                    </td>
                                    <td nowrap="nowrap" class="firstLeft active">
                                              <asp:DropDownList ID="drpVJahr" runat="server" Width="130px" 
                                        Font-Names="Verdana,sans-serif" CssClass="DropDownNormal">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="reqFieldValVersJahr" runat="server" ControlToValidate="drpVJahr"
                                            ErrorMessage="Auswahl des Versicherungsjahres erforderlich."></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr  class="formquery" >
                                    <td  nowrap="nowrap" class="firstLeft active" >
                                        VD-Bezirk:
                                    </td>
                                    <td  nowrap="nowrap" class="firstLeft active" >
                                        <asp:TextBox ID="txtOrgNr" runat="server" Width="129px" CssClass="InputTextbox"></asp:TextBox>
                                        <asp:Label ID="lblPlatzhaltersuche" runat="server"  Visible="True">*(mit Platzhaltersuche)</asp:Label>
                                    </td>
                                </tr>
                                <tr  class="formquery" >
                                    <td  nowrap="nowrap" class="firstLeft active" >
                                        Kennzeichen:
                                    </td>
                                    <td  nowrap="nowrap" class="firstLeft active" >
                                        
                                        <asp:TextBox ID="txtKennzeichen" runat="server" Width="129px" CssClass="InputTextbox"></asp:TextBox>
                                        <asp:Label ID="lblPlatzhaltersuche2" runat="server"  Visible="True">*(mit Platzhaltersuche)</asp:Label>
                                    </td>
                                </tr>
                                <tr  class="formquery" >
                                    <td class="" nowrap="nowrap">
                                    </td>
                                    <td align="right" class="rightPadding">
                                        <asp:LinkButton ID="btnConfirm" runat="server" CssClass="Tablebutton" 
                                            Height="16px" Width="78px">» Weiter</asp:LinkButton>
                                        
                                    </td>
                                </tr>
                               <tr>
                                    <td >
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                </tr>                                
                            </table>
                        </div>
                        <div id="DivPlaceholder"  runat="server" style="height: 550px;"></div>
                        <div id="Result" runat="Server" visible="false">
                      <div class="ExcelDiv">
                            <div  align="right" >
                                <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                <span class="ExcelSpan" >
                                <asp:LinkButton ID="lnkCreateExcel" runat="server" Visible="False" ForeColor="White">Excel herunterladen</asp:LinkButton>
                                </span>
                            </div>
                        </div>

                        <div id="pagination">
                        <uc2:GridNavigation id="GridNavigation1" runat="server" ></uc2:GridNavigation>
                        </div>
                            <div id="data">
                            <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                <tr>
                                    <td>
                                        <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                            CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0" AllowPaging="True" GridLines="None"
                                            PageSize="20" CssClass="gridview">
                                            <PagerStyle HorizontalAlign="Left" Wrap="true" />
                                            <PagerSettings Visible="false"/>
                                            <Columns>
                                                <asp:BoundField DataField="VD-Bezirk" SortExpression="VD-Bezirk" HeaderText="VD-Bezirk">
                                                    <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                    <ItemStyle CssClass="BoundField"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Name1" SortExpression="Name1" HeaderText="Name 1">
                                                    <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                    <ItemStyle CssClass="BoundField"></ItemStyle>                                                
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Name2" SortExpression="Name2" HeaderText="Name 2">
                                                    <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                    <ItemStyle CssClass="BoundField"></ItemStyle>                                                
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Kennzeichen anzeigen" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-VerticalAlign="Middle">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="btnAlleKennzAnzeigen" CommandName="AlleKennzAnzeigen" runat="server"
                                                            ForeColor="#333333" Text="alle Kennz. anzeigen" CssClass="TablebuttonLarge" EnableTheming="True"
                                                            Height="16px" Width="125px"></asp:LinkButton><br />
                                                        <asp:LinkButton ID="btnKennzAnzeigen" CommandName="KennzAnzeigen" runat="server"
                                                            ForeColor="#333333" Text="Kennz. anzeigen" CssClass="TablebuttonLarge" EnableTheming="True"
                                                            Height="16px" Width="125px"></asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkKennzAnzeigen" runat="server"></asp:CheckBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                    <ItemStyle CssClass="BoundField"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Adresse anzeigen" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-VerticalAlign="Middle">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="btnAlleAdressenAnzeigen" CommandName="AlleAdressenAnzeigen" runat="server"
                                                            ForeColor="#333333" Text="Alle Adr. anzeigen" CssClass="TablebuttonLarge"
                                                            EnableTheming="True" Height="16px" Width="125px"></asp:LinkButton>
                                                        <br />
                                                        <asp:LinkButton ID="btnAdressenAnzeigen" CommandName="AdressenAnzeigen" runat="server"
                                                            ForeColor="#333333" Text="Adressen anzeigen" CssClass="TablebuttonLarge" EnableTheming="True"
                                                            Height="16px" Width="125px"></asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAdresseAnzeigen" runat="server"></asp:CheckBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                    <ItemStyle CssClass="BoundField"></ItemStyle>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="Tablehead" ForeColor="White" />
                                            <AlternatingRowStyle BackColor="#DEE1E0"></AlternatingRowStyle>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="dataFooter">
                                        &nbsp;</div>
                        </div>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                        
                    </div>
                </div>
            </div>
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        </div>
</asp:Content>
