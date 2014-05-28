<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Menue.ascx.vb" Inherits="CKG.PortalZLD.Menue" %>

<script type="text/javascript" src="/PortalZLD/JScript/Menue.js"></script>

<asp:Repeater ID="MenuePunkte" runat="server">
    <ItemTemplate>
        <table cellpadding="0" cellspacing="0" border="0" bordercolor="black">
            <tr id="menuePunkteContainer" runat="server">
                <td align="right" id="menueOverp" valign="top" onmouseover="javascript:showMenue('1')"
                    onmouseout="javascript:showMenue('2')">
                    <img alt="<%#DataBinder.Eval(Container, "DataItem.DisplayName")%>" onmouseout="javascript:showMenue('2','<%#DataBinder.Eval(Container, "DataItem.AppType")%>',this)"
                        src="<%#DataBinder.Eval(Container, "DataItem.ButtonPath")%>" onmouseenter="javascript:showMenue('1','<%#DataBinder.Eval(Container, "DataItem.AppType")%>',this)" />
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    <div align="right" id="<%#DataBinder.Eval(Container, "DataItem.AppType")%>" style="display: none;
                        border: 1px solid #CECECE; border-left-width: 0;" onmouseover="javascript:showMenue('1',<%#DataBinder.Eval(Container, "DataItem.AppType")%>)"
                        onmouseout="javascript:showMenue('2',<%#DataBinder.Eval(Container, "DataItem.AppType")%>)">
                        <div style="border: 1px solid #D3D3D3; border-left-width: 0;" align="right">
                            <div style="border: 1px solid #DBDBDB; border-left-width: 0;" align="right">
                                <div style="border: 1px solid #E2E2E2; border-left-width: 0;" align="right">
                                    <div style="border: 1px solid #EAEAEA; border-left-width: 0;" align="right">
                                        <div style="border: 1px solid #F2F2F2; border-left-width: 0;" align="right">
                                            <div style="border: 1px solid #F9F9F9; border-left-width: 0;" align="right">
                                            <div style="border: 2px solid #ffffff; border-left-width: 0;" align="right">
                                                <asp:Repeater ID="appLinks" runat="server">
                                                    <ItemTemplate>
                                                        <table cellpadding="0" cellspacing="0" id="tableLinks" onmouseover="javascript:showMenue('1')"
                                                            runat="server" width="100%">
                                                            <tr>
                                                                <td class="MainmenuItemAlternate" onmouseover="javascript:showMenue('1')" nowrap="nowrap"
                                                                    align="left">
                                                                    <a onmouseover="javascript:showMenue('1')" class="MainmenuLink" target="_self" href='<%# getUrlString(DataBinder.Eval(Container, "DataItem.AppUrl"),DataBinder.Eval(Container, "DataItem.AppID")) %>'>
                                                                        <%#DataBinder.Eval(Container, "DataItem.AppFriendlyName")%>
                                                                    </a>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </ItemTemplate>
</asp:Repeater>
