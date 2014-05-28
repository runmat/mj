<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ServicesMenue.ascx.vb" Inherits="Admin.ServicesMenue" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


   
    <div style="width: 21px; position: static; margin-top: 15px; ">
    <asp:ImageButton ID="ibtnInfo" CssClass="NaviPic" ToolTip="Menü" OnClientClick="return false;"  runat="server" ImageUrl="../Images/navi_grey2.gif" />
</div>
    <div id="flyout">
    
    </div>
        <div id="info">

                <iframe id="HelpShim" src="javascript:''" scrolling="no" frameborder="0" 
                style="position:absolute; top:200px; height:325px; width:460px; left:0; z-index:-1; "></iframe>
            <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" 
                HorizontalAlign="Center" Height="525px" >
            
                <div id="btnCloseParent" >
                    schliessen <asp:LinkButton ID="btnClose" runat="server" 
                        OnClientClick="return false;" Text="X" ToolTip="schliessen  X"  Style=" color: #666666; text-align: center;font-size: 10pt;  font-weight: bold; text-decoration: none;padding: 2px;" />
                </div>
                <br style="margin-top: 10px" />                

                <div style="margin-top: 5px;"  align="center" >            

                    <asp:GridView Width="395px" ID="GridAdmin" runat="server" AutoGenerateColumns="False"
                        CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0" 
                        BackColor="White">
                        <Columns>
                        
                            <asp:HyperLinkField ItemStyle-HorizontalAlign="Left" DataNavigateUrlFields="AppUrl"
                                DataTextField="AppFriendlyName" HeaderText="Administration" ControlStyle-CssClass="MainmenuLink"
                               ItemStyle-CssClass="MainmenuItemAlternate" ItemStyle-Wrap="False" >
                                
                                <ControlStyle CssClass="MainmenuLink"  />
                                <ItemStyle CssClass="MainmenuItemAlternate"  HorizontalAlign="Left" 
                                    Wrap="False" />
                            </asp:HyperLinkField>
                        </Columns>
                        <HeaderStyle CssClass="menuehead"  />
                        <AlternatingRowStyle BackColor="#DEE1E0"></AlternatingRowStyle>
                    </asp:GridView>
                </div>                
                <div id="ChangeMenue" style="margin-top: 5px;"  align="center" >
                    <asp:GridView Width="395px" ID="GridChange" runat="server" AutoGenerateColumns="False"
                        CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0" 
                        BackColor="White">
                        <Columns>
                        
                            <asp:HyperLinkField ItemStyle-HorizontalAlign="Left" DataNavigateUrlFields="AppUrl"
                                DataTextField="AppFriendlyName" HeaderText="Dateneingabe" ControlStyle-CssClass="MainmenuLink"
                               ItemStyle-CssClass="MainmenuItemAlternate" ItemStyle-Wrap="False" >
                                
                                <ControlStyle CssClass="MainmenuLink"  />
                                <ItemStyle CssClass="MainmenuItemAlternate"  HorizontalAlign="Left" 
                                    Wrap="False" />
                                    
                              
                            </asp:HyperLinkField>
                        </Columns>
                        <HeaderStyle CssClass="menuehead"  />
                        <AlternatingRowStyle BackColor="#DEE1E0"></AlternatingRowStyle>
                    </asp:GridView>
                </div>
                <div id="ReportMenue" style="margin-top: 5px;"  align="center" >
                    <asp:GridView Width="395px" ID="Report" runat="server" AutoGenerateColumns="False"
                        CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0" BackColor="White" >
                        <Columns>
                            <asp:HyperLinkField ItemStyle-HorizontalAlign="Left" DataNavigateUrlFields="AppUrl"
                               HeaderText="Reports"  DataTextField="AppFriendlyName"  ControlStyle-CssClass="MainmenuLink"
                                ItemStyle-CssClass="MainmenuItemAlternate" ItemStyle-Wrap="False" >
                                <ControlStyle CssClass="MainmenuLink" />
                                <ItemStyle CssClass="MainmenuItemAlternate" HorizontalAlign="Left" 
                                    Wrap="False" />
                            </asp:HyperLinkField>
                        </Columns>
                          <HeaderStyle  CssClass="menuehead"  />
                        <AlternatingRowStyle BackColor="#DEE1E0"></AlternatingRowStyle>
                    </asp:GridView>
                </div>
                <div id="HelpMenue" style="margin-top: 5px;"  align="center" >
                    <asp:GridView Width="395px" ID="HelpdeskGrid" runat="server" AutoGenerateColumns="False"
                        CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0" BackColor="White" >
                        <Columns>
                            <asp:HyperLinkField ItemStyle-HorizontalAlign="Left" DataNavigateUrlFields="AppUrl"
                               HeaderText="Helpdesk"  DataTextField="AppFriendlyName"  ControlStyle-CssClass="MainmenuLink"
                                ItemStyle-CssClass="MainmenuItemAlternate" ItemStyle-Wrap="False" >
                                <ControlStyle CssClass="MainmenuLink" />
                                <ItemStyle CssClass="MainmenuItemAlternate" HorizontalAlign="Left" 
                                    Wrap="False" />
                            </asp:HyperLinkField>
                        </Columns>
                          <HeaderStyle  CssClass="menuehead"  />
                        <AlternatingRowStyle BackColor="#DEE1E0"></AlternatingRowStyle>
                    </asp:GridView>
                </div>                
            </asp:Panel>

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

