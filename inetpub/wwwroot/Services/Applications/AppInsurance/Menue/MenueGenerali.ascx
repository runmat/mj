<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="MenueGenerali.ascx.vb" Inherits="AppGenerali.MenueGenerali" %>

<body>
    <form action="">
    
    <asp:ImageButton ID="ibtnInfo" ToolTip="Menü" OnClientClick="return false;"  runat="server" ImageUrl="../../../Images/navDisplayArrowRight.gif" />

    <div id="flyout" style="display:none; overflow: hidden; z-index: 2; background-color: #FFFFFF;
        border: solid 1px #D0D0D0; ">
    
    </div>
        <div id="info" style="display: none; width: 275px; z-index: 2; opacity: 0; filter: progid:DXImageTransform.Microsoft.Alpha(opacity=0); font-size: 12px; border: solid 1px #CCCCCC; background-color: #FFFFFF; padding: 5px;">
        <div id="btnCloseParent" style="float: right; opacity: 0; filter: progid:DXImageTransform.Microsoft.Alpha(opacity=0);">
            <asp:LinkButton ID="btnClose" runat="server" OnClientClick="return false;" Text="X"
                ToolTip="Close" Style="background-color: #666666; color: #FFFFFF; text-align: center;
                font-weight: bold; text-decoration: none; border: outset thin #FFFFFF; padding: 2px;" />   
        </div>    
           
        <br />   
           <div  style="float: left; margin-top:5px">
   
            <asp:GridView width="100%" ID="GridChange" runat="server" AutoGenerateColumns="False" CellPadding="0"
                AlternatingRowStyle-BackColor="#DEE1E0" HeaderStyle-CssClass="Tablehead">
                <Columns  >
                    
                    <asp:HyperLinkField  DataNavigateUrlFields="AppUrl" DataTextField="AppFriendlyName" HeaderText="Dateieingabe" ItemStyle-CssClass="MainmenuLink" />
                    
                   
                </Columns>
                <HeaderStyle Height="16px" ForeColor="White" />
                <AlternatingRowStyle BackColor="#DEE1E0"></AlternatingRowStyle>
            </asp:GridView>            
        </div>
        <div  style="float: left; margin-top:5px">
            <asp:GridView width="100%"  ID="Report" runat="server" AutoGenerateColumns="False" CellPadding="0"
                AlternatingRowStyle-BackColor="#DEE1E0" HeaderStyle-CssClass="Tablehead">
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="AppUrl" DataTextField="AppFriendlyName" HeaderText="Report" ItemStyle-CssClass="MainmenuLink" />
                </Columns>
                <HeaderStyle Height="16px" ForeColor="White" />
                <AlternatingRowStyle BackColor="#DEE1E0"></AlternatingRowStyle>
            </asp:GridView>
        </div>
    
        </div>

    <script type="text/javascript" language="javascript">
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
                        <ScriptAction Script="Cover($get('ctl00_SampleContent_ibtnInfo'), $get('info')), false;" />
                        <StyleAction AnimationTarget="flyout" Attribute="display" Value="block"/>
                        
                        <%-- Move the wire frame from the button's bounds to the info panel's bounds --%>
                         <ScriptAction Script="Cover($get('info'), $get('info'), true);" />
                        <StyleAction AnimationTarget="info" Attribute="display" Value="block"/>
                        <FadeIn AnimationTarget="info" Duration=".2"/>
                        <StyleAction AnimationTarget="flyout" Attribute="display" Value="none"/>
                                               
                      
                        <%-- Flash the text/border red and fade in the "close" button --%>
                       
                        <Parallel AnimationTarget="flyout" Duration=".5">
                            <FadeIn AnimationTarget="btnCloseParent" MaximumOpacity=".9" />
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
                        <StyleAction Attribute="width" Value="275px"/>
                        <StyleAction Attribute="height" Value=""/>
                        <StyleAction Attribute="fontSize" Value="12px"/>
                        <OpacityAction AnimationTarget="btnCloseParent" Opacity="0" />
                        
                        <%--  Enable the button so it can be played again --%>
                        <EnableAction AnimationTarget="ibtnInfo" Enabled="true" />
                    </Sequence>
                 
                                    
                </OnClick>
                <OnMouseOver>
                    <Color Duration=".2" PropertyKey="color" StartValue="#FFFFFF" EndValue="#FF0000" />
                </OnMouseOver>
                <OnMouseOut>
                    <Color Duration=".2" PropertyKey="color" StartValue="#FF0000" EndValue="#FFFFFF" />
                </OnMouseOut>
        </Animations>
    </ajaxToolkit:AnimationExtender>
    </form>
</body>
