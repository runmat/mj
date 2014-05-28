<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportTagList_2.aspx.cs" Inherits="AppZulassungsdienst.forms.ReportTagList_2" MasterPageFile="../MasterPage/App.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/helper.js?26082013"></script>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück" onclick="lb_zurueck_Click"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div id="TableQuery" style="margin-bottom: 10px">
                                <table id="tab1" visible="false" runat="server" cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr class="formquery">
                                            <td class="firstLeft active" style="vertical-align:top">
                                                <asp:Label ID="lblError"  runat="server" CssClass="TextError"></asp:Label>
                                                <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                      
                                    </tbody>
                                </table>
                                <table id="Table1" cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="2" style="vertical-align:top">
                                                <asp:Label ID="Label1"  runat="server" CssClass="TextError"></asp:Label>
                                                <asp:Label ID="Label2" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" style="height: 35px">
                                                    <div style="width:25px; height:25px; background-color:#EA7272">
                                                    </div>
                                                </td>
                                                <td class="firstLeft active" style="height: 35px">
                                                    Hier wurden bereits Tageslisten gedruckt!</td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <div style="width:25px; height:25px; background-color:#FFDB6D"></div></td>
                                                <td class="firstLeft active" style="width:100%" >
                                                    Diese Datensätze wurden bereits in einer Tagesliste gedruckt!<br />
                                                    Bei erneutem Druck muss die alte Tagesliste vernichtet werden! Soll keine 
                                                    erneute Ausgabe erfolgen, dann Liste nicht auswählen!
                                                </td>
                                            </tr> 
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="2" style="vertical-align:top">

                                                &nbsp;&nbsp;</td>
                                        </tr>                                                                                    
                                    </tbody>
                                </table>                                
                            </div>
                            <div id="Result" runat="Server" visible="false">
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False"
                                                    CellPadding="0" CellSpacing="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowSorting="true" AllowPaging="True" CssClass="GridView" PageSize="20" 
                                                    onsorting="GridView1_Sorting" DataKeyNames="KREISKZ" onrowcommand="GridView1_RowCommand" onrowdatabound="GridView1_RowDataBound"  
                                                   >
                                                    <HeaderStyle CssClass="GridTableHead" Width="100%" ForeColor="White" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                    <PagerSettings Visible="False" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>

					                                    <asp:TemplateField HeaderText="Details">
                                                             <ItemTemplate>
                                                            <a href="javascript:expandcollapse('div<%# Eval("KREISKZ") %>', 'one');">
                                                            <img id="imgdiv<%# Eval("KREISKZ") %>" alt="" width="9px" border="0" src="/PortalZLD/Images/minus.gif" />
                                                            </img></a>
                                                            </ItemTemplate>
						                                        <HeaderStyle CssClass="TablePadding" Width="60px" />
						                                        <ItemStyle CssClass="TablePadding"  Width="60px" />                                                            
                                                        </asp:TemplateField>
 
                                                        <asp:TemplateField SortExpression="KREISKZ" HeaderText="col_Kreis">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kreis" runat="server" CommandName="Sort" CommandArgument="KREISKZ">col_Kreis</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblKreis" runat="server"  Text='<%# DataBinder.Eval(Container, "DataItem.KREISKZ") %>'></asp:Label>
                                                            </ItemTemplate>
						                                        <HeaderStyle CssClass="TablePadding" Width="80px" />
						                                        <ItemStyle CssClass="TablePadding"  Width="80px" />    
                                                        </asp:TemplateField>  
                                                                                                     
                                                        <asp:TemplateField SortExpression="ZZZLDAT" HeaderText="col_Zulassungsdatum">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Zulassungsdatum"  runat="server" CommandName="Sort" CommandArgument="ZZZLDAT">col_Zulassungsdatum</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblZulassungsdatum" runat="server"  Text='<%# DataBinder.Eval(Container, "DataItem.ZZZLDAT", "{0:d}") %>'></asp:Label>
                                                            </ItemTemplate>
						                                        <HeaderStyle CssClass="TablePadding"  Width="100px" />
						                                        <ItemStyle CssClass="TablePadding"   Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                           <ItemTemplate>
                                                                <asp:Label ID="lblDRUKZ" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DRUKZ") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>                                                        
                                                       <asp:TemplateField>
                                                        <HeaderStyle Width="100%" />
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td colspan="100" id="tdGrid2"  runat="server" >
                                                                        <div id="div<%# Eval("KREISKZ") %>" style="display: block; position: relative;
                                                                            left: 15px; padding-left:50px; overflow: auto; width: 90%">
                                                                            <asp:GridView ID="GridView2" GridLines="None" BorderStyle="none" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                                                                                DataKeyNames="KreisKZ" 
                                                                                onsorting="GridView2_Sorting">
                                                                                <HeaderStyle CssClass="GridTableHead" ForeColor="#2265BE" />
                                                                             
                                                                               
                                                                                <RowStyle CssClass="ItemStyle" />
                                                                                 <PagerSettings Visible="False" />
                                                                                <Columns>
                                                                                    <asp:TemplateField SortExpression="KREISKZ" HeaderText="col_Kreis">
                                                                                        <HeaderTemplate>
                                                                                            <asp:LinkButton ID="col_Kreis" runat="server" CommandName="Sort" CommandArgument="KREISKZ">col_Kreis</asp:LinkButton></HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblKreis" runat="server"  Text='<%# DataBinder.Eval(Container, "DataItem.KREISKZ") %>'></asp:Label>
                                                                                        </ItemTemplate>
						                                                                    <HeaderStyle CssClass="TablePadding" Width="35px" />
						                                                                    <ItemStyle CssClass="TablePadding"  Width="35px" />    
                                                                                    </asp:TemplateField>                                                                                     
                                                                                    <asp:TemplateField SortExpression="ZZZLDAT" HeaderText="col_Zulassungsdatum">
                                                                                        <HeaderTemplate>
                                                                                            <asp:LinkButton ID="col_Zulassungsdatum"  runat="server" CommandName="Sort" CommandArgument="ZZZLDAT">col_Zulassungsdatum</asp:LinkButton></HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblZulassungsdatum" runat="server"  Text='<%# DataBinder.Eval(Container, "DataItem.ZZZLDAT", "{0:d}") %>'></asp:Label>
                                                                                        </ItemTemplate>
						                                                                    <HeaderStyle CssClass="TablePadding"  Width="65px" />
						                                                                    <ItemStyle CssClass="TablePadding"   Width="65px" />
                                                                                    </asp:TemplateField>                                                                                    
                                                                                    <asp:TemplateField SortExpression="NAME1" HeaderText="col_Kundenname">
                                                                                        <HeaderTemplate>
                                                                                            <asp:LinkButton ID="col_Kundenname" runat="server" CommandName="Sort" CommandArgument="NAME1">col_Kundenname</asp:LinkButton></HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblKundenname" runat="server"  Text='<%# DataBinder.Eval(Container, "DataItem.NAME1") %>'></asp:Label>
                                                                                        </ItemTemplate>
						                                                                    <HeaderStyle CssClass="TablePadding"  Width="200px"/>
						                                                                    <ItemStyle CssClass="TablePadding"  Width="200px"  />
                                                                                    </asp:TemplateField>                                                                                     
                                                                                    <asp:TemplateField SortExpression="ZZREFNR1" HeaderText="col_Referenz1">
                                                                                        <HeaderTemplate>
                                                                                            <asp:LinkButton ID="col_Referenz1" runat="server" CommandName="Sort" CommandArgument="ZZREFNR1">col_Referenz1</asp:LinkButton></HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblReferenz1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFNR1") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                            	                        <HeaderStyle Width="50px" />
						                                                                <ItemStyle  Width="50px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField SortExpression="ZZKENN" HeaderText="col_Kennzeichen">
                                                                                        <HeaderTemplate>
                                                                                            <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="ZZKENN">col_Kennzeichen</asp:LinkButton></HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblKennzeichen" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZKENN") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                            	                        <HeaderStyle Width="60px" />
						                                                                <ItemStyle  Width="60px" />
                                                                                    </asp:TemplateField>                                                                                    

                                                                                    <asp:TemplateField SortExpression="MAKTX" HeaderText="col_Matbez">
                                                                                        <HeaderTemplate>
                                                                                            <asp:LinkButton ID="col_Matbez" runat="server" CommandName="Sort" CommandArgument="MAKTX">col_Matbez</asp:LinkButton></HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblMatbez" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.MAKTX") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                            	                        <HeaderStyle Width="100px" />
						                                                                <ItemStyle  Width="100px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField Visible="false">
                                                                                       <ItemTemplate>
                                                                                            <asp:Label ID="lblPrintKZ" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLAG") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                   </Columns>                                                                                   
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </td>
                                                                </tr>
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
                      <asp:LinkButton ID="cmdCreate" runat="server" CssClass="TablebuttonLarge" 
                            Height="22px" Width="120px" onclick="cmdCreate_Click" 
                           >» PDF erzeugen </asp:LinkButton>
                            
                </div>
            </div>
        </div>
    </div>
    </div>
</asp:Content>
