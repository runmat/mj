<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="KBSMenue.ascx.vb" Inherits="KBS.KBSMenue" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<div style="width: 29px; position: static; margin-top: 15px;">
    <asp:ImageButton ID="ibtnInfo" ToolTip="Menü" OnClientClick="return false;" runat="server"
        ImageUrl="../Images/navi_grey.gif" />
</div>
<div id="flyout">
</div>
<div id="info" style="width: 450px">
    <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" HorizontalAlign="Center">
        <div id="btnCloseParent">
            schliessen
            <asp:LinkButton ID="btnClose" runat="server" OnClientClick="return false;" Text="X"
                ToolTip="schliessen" Style="color: #666666; text-align: center; font-weight: bold;
                text-decoration: none; border: outset thin #FFFFFF; padding: 2px;" />
        </div>
        <br style="margin-top: 10px" />
        <div style="margin-top: 5px; margin-left: 20px; margin-right: 20px; text-align: left; border-color: black; border-width: 1px; border-style: solid">
            <div style="height: 15px; border-bottom-color: black; border-bottom-width: 1px; border-bottom-style: solid"></div>
            <asp:HiddenField ID="dataGroups" runat="server" />
            <asp:Repeater ID="repeater1" runat="server" OnItemDataBound="repeater1_OnItemDataBound" >
                <ItemTemplate>
                    <table id="tableItem" runat="server" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td class="MainmenuItemAlternate">
                                <div style="width: 95%">
                                    <asp:LinkButton ID="lbApplication" runat="server" Text='<%# Eval("AppFriendlyName") %>'
                                        CommandArgument='<%# Eval("AppUrl") %>' OnClick="lbApplication_OnClick" CssClass="MainmenuLink" Width="100%" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </asp:Panel>
</div>

<script type="text/javascript">

    $(document).ready(function () {
        var dataGroups = $('#<%=dataGroups.ClientID%>').val().split(';');

        for (var i = 0; i < dataGroups.length; i++) {
            if (dataGroups[i] != '') {
                $('table').filter(function (inputs) {
                    return ($(this).data('group') == dataGroups[i]);
                }).wrapAll("<div class='accordion' style='top: 0; left: 0; bottom: 0; right: 0'>");
            }
        }
        var accordions = $('.accordion');
        $(accordions).wrapInner("<div>").prepend('<div class="MainmenuItemAlternate MenuGroup" style="top: 0; left: 0; bottom: 0; right: 0"><b><a class="MainmenuLink" href="#">Handle</a></b></div>');

        for (var i = 0; i < accordions.length; i++) {
            $(accordions[i]).find('.MenuGroup a').text('+ ' + $(accordions[i]).find('table:first').data('group'));
        }

        $('.accordion').accordion({ active: false, collapsible: true, autoHeight: false });

        $('.ui-accordion-content a').css('padding-left', '20px');
    });

    // Move an element directly on top of another element (and optionally
    // make it the same size)
    function Cover(bottom, top, ignoreSize) {
        var location = Sys.UI.DomElement.getLocation(bottom);
        top.style.position = 'absolute';
        top.style.top = location.y + 'px';
        top.style.left = location.x + 'px';
        if (!ignoreSize) {
            top.style.height = bottom.offsetHeight + 'px';
            top.style.width = bottom.offsetWidth + 'px';
        }
    }

</script>

<ajaxToolkit:AnimationExtender ID="OpenAnimation" runat="server" TargetControlID="ibtnInfo">
    <Animations>
                <OnClick>
                    <Sequence>
                        <%-- Disable the button so it can't be clicked again --%>
                        <EnableAction Enabled="false" />
                        
                        <%-- Position the wire frame on top of the button and show it --%>
                        <ScriptAction Script="Cover($get('ctl00_SampleContent_ibtnInfo'), $get('info')), true;" />
                        <StyleAction AnimationTarget="flyout"  Attribute="display" Value="block"/>

                        <%-- Move the wire frame from the button's bounds to the info panel's bounds --%>
                        <ScriptAction Script="Cover($get('info'), $get('info'), true);" />
                        <StyleAction AnimationTarget="info" Attribute="display" Value="block"/>
                        <FadeIn AnimationTarget="info" Duration=".2" MaximumOpacity=".94"/>
                        <StyleAction AnimationTarget="flyout" Attribute="display" Value="none"/>
                        <%-- Flash the text/border red and fade in the "close" button --%>
                              <Parallel AnimationTarget="flyout" Duration=".5">
                            <FadeIn AnimationTarget="btnCloseParent" MaximumOpacity=".94" />
                        </Parallel>
                    </Sequence>
                </OnClick>
    </Animations>
</ajaxToolkit:AnimationExtender>
<ajaxToolkit:AnimationExtender ID="CloseAnimation" runat="server" TargetControlID="btnClose">
    <Animations>
                <OnClick>
                    <Sequence AnimationTarget="info">
                        <%--  Shrink the info panel out of view --%>
                        <StyleAction Attribute="overflow" Value="hidden"/>
                        <Parallel Duration=".3" Fps="15">
                            <Scale ScaleFactor="0.05" Center="true" ScaleFont="true" FontUnit="px" />
                            <FadeOut />
                        </Parallel>
                        
                        <%--  Reset the sample so it can be played again --%>
                        <StyleAction Attribute="display" Value="none"/>
                        <StyleAction Attribute="width" Value="450px"/>
                        <StyleAction Attribute="height" Value="535px"/>
                        <StyleAction Attribute="fontSize" Value="12px"/>
                        <OpacityAction AnimationTarget="btnCloseParent" Opacity="0" />
                        
                        <%--  Enable the button so it can be played again --%>
                        <EnableAction AnimationTarget="ibtnInfo" Enabled="true" />
                    </Sequence>
                 
                                    
                </OnClick>
                <OnMouseOver>
                    <Color Duration=".2" PropertyKey="color" StartValue="#666666" EndValue="#FF0000" />
                </OnMouseOver>
                <OnMouseOut>
                    <Color Duration=".2" PropertyKey="color" StartValue="#FF0000" EndValue="#666666" />
                </OnMouseOut>
    </Animations>
</ajaxToolkit:AnimationExtender>
