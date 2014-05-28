<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report30ZLD.aspx.cs" Inherits="AppZulassungsdienst.forms.Report30ZLD" 
    MasterPageFile="../MasterPage/App.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel" />
                        </Triggers>
                        <ContentTemplate>
                            <div id="paginationQuery">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td class="active">
                                                Neue Abfrage starten
                                            </td>
                                            <td align="right">
                                                <div id="queryImage">
                                                    <asp:ImageButton ID="NewSearch" runat="server" 
                                                        ImageUrl="../../../Images/queryArrow.gif" onclick="NewSearch_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <asp:Panel ID="Panel1" DefaultButton="btnEmpty" runat="server">
                                <div id="TableQuery" style="margin-bottom: 10px">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="2" width="100%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblKennzeichen" runat="server">Kennzeichen:</asp:Label>
                                                </td>
                                                <td class="active" style="width: 100%">
                                                    <asp:TextBox ID="txtKennzeichen" runat="server" MaxLength="3" CssClass="TextBoxNormal"></asp:TextBox>
                                                  </td>
                                            </tr>                                            
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblPLZ" runat="server">PLZ Halterort:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtPLZ" runat="server" MaxLength="10" CssClass="TextBoxNormal"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblZulassungspartner" runat="server">Name Zulassungspartner:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtZulassungspartner" MaxLength="40" runat="server" CssClass="TextBoxNormal" />
                                                </td>
                                            </tr>
                                         
                                                   <tr class="formquery">
                                                <td colspan="2">
                                                    <asp:ImageButton
                                                        ID="btnEmpty" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
                                                        Width="1px" onclick="btnEmpty_Click" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>
                                </div>
                            </asp:Panel>
                            <div id="dataQueryFooter">
                                <asp:LinkButton ID="cmdCreate" runat="server" CssClass="Tablebutton" 
                                    Width="78px" onclick="cmdCreate_Click">» Erstellen </asp:LinkButton>
                            </div>
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel" ForeColor="White" runat="server" 
                                            onclick="lnkCreateExcel_Click">Excel herunterladen</asp:LinkButton></span></div>
                                </div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvZuldienst" Width="100%" runat="server" AutoGenerateColumns="False"
                                                    CellPadding="0" CellSpacing="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowSorting="true" AllowPaging="True" CssClass="GridView" PageSize="20" 
                                                    onrowdatabound="gvZuldienst_RowDataBound" onsorting="gvZuldienst_Sorting">
                                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White"  />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                    <PagerSettings Visible="False" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                        <asp:TemplateField SortExpression="Name" HeaderText="col_Name">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Name" runat="server" CommandName="Sort" CommandArgument="Name">col_Name</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>   
                                                        <asp:TemplateField SortExpression="Kreisbezeichnung" HeaderText="col_Kreisbezeichnung">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kreisbezeichnung" runat="server" CommandName="Sort" CommandArgument="Kreisbezeichnung">col_Kreisbezeichnung</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblKreisbezeichnung" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kreisbezeichnung") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>   
                                                        <asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">col_Kennzeichen</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblKennzeichen" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Details">
                                                            <ItemTemplate>
                                                                 <asp:Label ID="lblDetail" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.Details") %>'></asp:Label>
                                                                <asp:ImageButton ID="ibtnDetail" ToolTip="Detailansicht (in neuem Fenster öffnen)" ImageUrl="../../../images/Lupe_16x16.gif"
                                                                 runat="server"     />                                                                 
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div id="dataFooter">
                        &nbsp;</div>
                </div>
            </div>
        </div>
    </div>
        <script type="text/javascript">
        function openinfo(url) {
            fenster = window.open(url, "Detailsicht", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=750,height=465");
            fenster.focus();
        }
        </script>
</asp:Content>
