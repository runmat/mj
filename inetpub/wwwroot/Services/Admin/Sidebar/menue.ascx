<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="menue.ascx.vb" Inherits="Admin.menue" %>
<script type="text/javascript" src="menue.js"></script>

<body>
    <form action="">
    <asp:Repeater ID="MenuePunkte" runat="server">
        <ItemTemplate>
            <table cellpadding="0" cellspacing="0" border="0"  style="border-color: #000000">
                <tr id="menuePunkteContainer" runat="server">
                    <td align="left" id="menueOverp" valign="top" onmouseover="javascript:showMenue('1')"
                        onmouseout="javascript:showMenue('2')">
                        <img alt="" onmouseout="javascript:showMenue('2','<%#DataBinder.Eval(Container, "DataItem.AppType")%>')"
                            src="<%#DataBinder.Eval(Container, "DataItem.ButtonPath")%>" onmouseover="javascript:showMenue('1','<%#DataBinder.Eval(Container, "DataItem.AppType")%>')" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                  
                    <td >
                   
                        <div align="right" id="<%#DataBinder.Eval(Container, "DataItem.AppType")%>" style="display: none;
                            border: 1px solid #bbbbbb; border-left-width:0;" onmouseover="javascript:showMenue('1',<%#DataBinder.Eval(Container, "DataItem.AppType")%>)"
                            onmouseout="javascript:showMenue('2',<%#DataBinder.Eval(Container, "DataItem.AppType")%>)">
                            <asp:Repeater ID="appLinks" runat="server">
                                <ItemTemplate>
                                    <table cellpadding="0"  cellspacing="0" id="tableLinks" onmouseover="javascript:showMenue('1')"
                                        runat="server" width="100%">
                                        <tr>
                                       
                                            <td onmouseout="javascript:showMenue('2')" class="MainmenuItemAlternate" onmouseover="javascript:showMenue('1')" nowrap="nowrap"
                                                align="left">
                                                <a  onmouseover="javascript:showMenue('1')" 
                                                    class="MainmenuLink" target="_self" href='<%# getUrlString(DataBinder.Eval(Container, "DataItem.AppUrl"),DataBinder.Eval(Container, "DataItem.AppID")) %>'>
                                                    <%#DataBinder.Eval(Container, "DataItem.AppFriendlyName")%>
                                                </a>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:Repeater>
    </form>
</body>
