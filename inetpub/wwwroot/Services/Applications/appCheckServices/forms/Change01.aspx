<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Change01.aspx.cs" Inherits="appCheckServices.forms.Change01"
    MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" onclick="lbBack_Click"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div id="Div1" runat="server" style="background-color: #dfdfdf; height:22px;padding-left:15px;padding-top:5px">
                        <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" 
                            EnableViewState="False"></asp:Label>
                        <asp:Label ID="lblNoData" runat="server"></asp:Label>&nbsp
                    </div>
                    <div id="TableQuery" >
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0"  >
                            <tr>
                                <td colspan="3" >

                               

                                </td>
                            </tr>
                            
                            <tr class="formquery">
                                <td class="firstLeft active" nowrap="nowrap" colspan="3">
                                    &nbsp
                                </td>
                            </tr>
                            
                            <tr id="trUpload" runat="server" class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    Dateiauswahl: 
                                </td>
                                <td class="active" style="width: 88%" colspan="2">
                                    <input id="upFile" type="file" size="49" name="File1" runat="server" />&nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="3" style="width: 100%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td colspan="3" align="right" class="rightPadding" style="width: 100%">
                                    <div>
                                        <asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" Width="78px"
                                            Height="16px" CausesValidation="False" Font-Underline="False" 
                                            onclick="cmdSearch_Click">» Hochladen</asp:LinkButton>
                                        &nbsp;
                                    </div>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="3" style="width: 100%">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <script language="JavaScript" type="text/javascript">										
				                                <!--
                            function openinfo(url) {
                                fenster = window.open(url, "Zulassungsdatum", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=650,height=250");
                                fenster.focus();
                            }
				                                -->
                        </script>

                        <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                            
                        </div>
                    </div>
                                                        <div id="dataQueryFooter" >

&nbsp;
                                    </div>
                           <div id="Result" runat="Server" visible="false">
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0"  bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" headerCSS="tableHeader"
                                                    bodyCSS="tableBody" CssClass="GridView" AllowSorting="True" AutoGenerateColumns="False"
                                                    OnSelectedIndexChanged="DataGrid1_SelectedIndexChanged" OnItemCommand="DataGrid1_ItemCommand" GridLines="None">
                                                    <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                    <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                                    <ItemStyle CssClass="ItemStyle"  />
                                                    <Columns>
                                                        <asp:TemplateColumn SortExpression="Filename" HeaderText="">
                                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="Linkbutton1" runat="server" CommandArgument="Filename" CommandName="Sort">Dokument</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="lbtPDF" runat="server" CommandName="open" Visible="False"
                                                                    ImageUrl="../../../images/iconPDF.gif" ToolTip="PDF"></asp:ImageButton>
                                                                <asp:ImageButton ID="lbtExcel" runat="server" CommandName="open" Visible="False"
                                                                    ImageUrl="../../../Images/iconXLS.gif" ToolTip="Excel"></asp:ImageButton>
                                                                <asp:LinkButton ID="Linkbutton2" runat="server" Width="229px" CommandName="open"
                                                                    Text='<%# DataBinder.Eval(Container, "DataItem.Filename") %>' ></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn Visible="False" DataField="Serverpfad" SortExpression="Serverpfad">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn Visible="False" DataField="Pattern" SortExpression="Pattern"></asp:BoundColumn>
                                                    </Columns>
                                                    <PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True"
                                                        PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Wrap="False"></PagerStyle>
                                                </asp:DataGrid>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="dataFooter">
                                    &nbsp;</div>
                            </div><asp:Literal ID="Literal1" runat="server"></asp:Literal>
                </div>
            </div>
        </div>
    </div>
</asp:Content>