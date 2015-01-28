<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Selection.aspx.cs" Inherits="AutohausPortal.Start.Selection"
     MasterPageFile="/AutohausPortal/MasterPage/Selection.Master" %>
     <%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
    <!-- content start -->
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
        <!--teaserstart-->
        <div class="teaserstart">
			<!--teaser-->
            <div id="teaser1" class="teaser" style="display: none;">
				<div class="teaserbild"><img height="110" width="214" alt="Zulassungen" src="../images/teaser_zulassungen.jpg"/></div>
				<div class="teasercontent" id ="teasercontent1" runat="server">
                    <h2> Zulassungen </h2>
                    <ul>
                        <% for (int i = 0; i < 4 && i < MenuChangeSource.Count; i++)
                            { %>
                            <% System.Data.DataRowView row = MenuChangeSource[i]; %>
                            <li>
                                <a href="<%= ResolveClientUrl(row["AppUrl"].ToString()) %>"><%= row["AppFriendlyName"]%></a>
                            </li>
                        <% } %>
                    </ul>   
				</div>
            </div>
			<!--teaser-->
			<!--teaser-->
            <div id="teaser2" class="teaser" style="display: none;">
				<div class="teaserbild"><img height="110" width="214" alt="Autohaus" src="../images/teaser_autohaus.jpg"/></div>
				<div class="teasercontent" id ="teasercontent2" runat="server">
                    <h2> Autohaus </h2>
                    <ul>
                        <% for (int i = 0; i < 4 && i < MenuChangeAHSource.Count; i++)
                            { %>
                            <% System.Data.DataRowView row = MenuChangeAHSource[i]; %>
                            <li>
                                <a href="<%= ResolveClientUrl(row["AppUrl"].ToString()) %>"><%= row["AppFriendlyName"]%></a>
                            </li>
                        <% } %>
                    </ul>   
				</div>
            </div>
			<!--teaser-->
			<!--teaser-->
            <div id="teaser3" class="teaser" style="display: none;">
				<div class="teaserbild"><img height="110" width="214" alt="Aufträge" src="../images/teaser_auftraege.jpg"/></div>
				<div class="teasercontent" id ="teasercontent3" runat="server">
                    <h2> Aufträge </h2>
                    <ul>
                        <% for (int i = 0; i < 4 && i < MenuReportSource.Count; i++)
                            { %>
                            <% System.Data.DataRowView row = MenuReportSource[i]; %>
                            <li>
                                <a href="<%= ResolveClientUrl(row["AppUrl"].ToString()) %>"><%= row["AppFriendlyName"]%></a>
                            </li>
                        <% } %>
                    </ul>
				</div>
            </div>
			<!--teaser-->
			<!--teaser-->
            <div id="teaser4" class="teaser" style="display: none;">
				<div class="teaserbild"><img height="110" width="214" alt="Tools" src="../images/teaser_tools.jpg" /></div>
                <div class="teasercontent" id ="teasercontent4" runat="server">
                    <h2> Tools </h2>
                    <ul>
                        <% for (int i = 0; i < 4 && i < MenuToolsSource.Count; i++)
                            { %>
                            <% System.Data.DataRowView row = MenuToolsSource[i]; %>
                            <li>
                                <a href="<%= ResolveClientUrl(row["AppUrl"].ToString()) %>"><%= row["AppFriendlyName"]%></a>
                            </li>
                        <% } %>
                    </ul>
				</div>
            </div>
			<!--teaser-->
        </div>
		<!--teaserstart-->
		<!--maincontent-->
        <div class="maincontent">
			<!--subnavigation-->
			<script type="text/javascript">
			    // SUB-NAVIGATION AUF STARTSEITE
			    // Hier werden die aktuell offene Navigationsebene, sowie die maximale Anzahl von Navigationsebenen eingestellt
			    var tnon = 1;
			    var maxtn = 3;
			</script>
			<div class="subnavi">
				<!--buttonrand links-->
				<div class="tnbuttonleft_on">
				</div>
				<!--buttonrand links-->
				<!--button-->
				<div id="tn1" class="tnbutton_on">
					<div class="tnbuttoninner">News</div>
					<div class="tnbuttonrightlast"></div>
                    <!--<div class="tnbuttonright"></div>-->
				</div>  
				<!--button-->
				<!--button-->
				<div style="display:none" onclick="changemaincontent(2);" id="tn2" class="tnbutton">
					<div class="tnbuttoninner">Offene Aufträge</div>
					<div class="tnbuttonright"></div>
				</div>
				<!--button-->
				<!--button-->
				<div style="display:none" onclick="changemaincontent(3);" id="tn3" class="tnbutton">
					<div class="tnbuttoninner">Hilfe</div>
					<div class="tnbuttonrightlast"></div>
				</div>
				<!--button-->
			</div>
			<!--subnavigation-->

			<div class="contentbloecke">
			<!-- CONTENTBLOCK NEWS -->
			<div id="contentblock1">
			<!--subnavigation Headline-->
			<div class="subnavi-info">
				<p>Erfahren Sie mehr über Aktionen und Neuigkeiten bei Kroschke</p>
			</div>
			<!--subnavigation Headline-->

			<!--contentlinks-->
			<div class="contentlinks">
                <iframe allowtransparency="true" name="Iframe" src="https://www.kroschke.de/portalnews.html"
                    scrolling="no" height="500" width="622" marginwidth="0" marginheight="0" hspace="0"
                    vspace="0" frameborder="0" style="margin-top: -18px;"></iframe>
			</div>
			<!--contentlinks-->
			<!--contentrechts-->
			<div class="contentrechts">
				<!--teaser-->
                <div class="teaserlarge">
                    <div class="teaserbild">
                        <asp:Image runat="server" ID="imgAnsprechpartner" AlternateText="Ansprechpartner" Height="66px" Width="200px" 
                            style="margin-left: 7px; margin-right: 7px; margin-top: 10px" ImageUrl="../images/bild_ansprechpartner.jpg"/>
                    </div>
                    <div class="teasercontentlarge">
						<h3>
						    Ansprechpartner
                        </h3>
						Unser kompetentes Berater-Team steht Ihnen mit Rat und Tat 
						zur Verfügung.
                        <br>
						<br>
                        <asp:Label runat="server" ID="lblKontaktdaten" Text="Tel +49 (0)4102 804-170<br>Fax +49 (0)4102 804-300<br>E-Mail service[at]kroschke.de"></asp:Label>
                    </div>
                </div>
				<!--teaser-->
			</div>
			<!--contentrechts-->
			</div>
			<!-- CONTENTBLOCK NEWS -->

			<!-- CONTENTBLOCK AUFTRÄGE -->
			<div id="contentblock2" style="display:none"><!--subnavigation Headline-->
			<div class="subnavi-info">
				<p>Erfahren Sie mehr über Aufträge bei Kroschke</p>
                <asp:Label ID="lblErrorAutrag" Style="color: #B54D4D" runat="server" ></asp:Label>
			</div>
			<!--subnavigation Headline-->
                <div style="padding-top: 0px; padding-right: 20px; padding-bottom: 20px; padding-left: 20px;">
                    <telerik:RadGrid ID="RadGrid1" runat="server" Width="95%" AutoGenerateColumns="False"
                        PageSize="10" AllowSorting="True" AllowMultiRowSelection="False" AllowPaging="True"
                        CssClass="GridView" OnNeedDataSource="RadGrid1_NeedDataSource" EnableEmbeddedSkins="false" Culture="de-DE" >
                        <PagerStyle Mode="NumericPages" PagerTextFormat="Seite wechseln: {4} &nbsp;Seite <strong>{0}</strong> von <strong>{1}</strong>, Vorgänge <strong>{2}</strong> bis <strong>{3}</strong> von <strong>{5}</strong>."></PagerStyle>
                        <MasterTableView Width="890px" DataKeyNames="KUNNR" AllowMultiColumnSorting="True"
                            CommandItemDisplay="Top" ExpandCollapseColumn-Display="true" RowIndicatorColumn-Display="false">
                            <ItemStyle CssClass="ItemStyle" />
                            <AlternatingItemStyle CssClass="ItemStyle" />
                            <HeaderStyle CssClass="GridTableHead" />
                            <CommandItemSettings ShowExportToExcelButton="false" ShowAddNewRecordButton="false"
                                ShowRefreshButton="false" />
                            <Columns>
                                <telerik:GridBoundColumn SortExpression="ZULBELN" HeaderText="ID" HeaderStyle-Font-Bold="true"
                                    HeaderButtonType="LinkButton" DataField="ZULBELN" ItemStyle-Height = "37px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="KUNNR" HeaderText="Kundennr." HeaderStyle-Font-Bold="true"
                                    HeaderButtonType="LinkButton" DataField="KUNNR" ItemStyle-Height = "37px">
                                </telerik:GridBoundColumn>
                                <telerik:GridNumericColumn DataField="MAKTX" HeaderText="Dienstleistung" UniqueName="MAKTX"  ItemStyle-Height = "37px">
                                </telerik:GridNumericColumn>
                                <telerik:GridBoundColumn SortExpression="ZZZLDAT" HeaderStyle-Font-Bold="true" HeaderText="Zulassungsdatum"
                                    HeaderButtonType="LinkButton" DataField="ZZZLDAT" UniqueName="ZZZLDAT" DataFormatString="{0:d}" ItemStyle-Height = "37px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="ZZREFNR1" HeaderText="Referenz1" HeaderStyle-Font-Bold="true"
                                    HeaderButtonType="LinkButton" DataField="ZZREFNR1" UniqueName="ZZREFNR1" ItemStyle-Height = "37px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="ZZKENN" HeaderText="Kennzeichen" HeaderStyle-Font-Bold="true"
                                    HeaderButtonType="LinkButton" DataField="ZZKENN" UniqueName="ZZKENN" ItemStyle-Height = "37px">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>

			<!--contentlinks-->
			</div>
			<!-- CONTENTBLOCK AUFTRÄGE -->

			<!-- CONTENTBLOCK HILFE -->
			<div id="contentblock3" style="display:none">
			<!--subnavigation Headline-->
			<div class="subnavi-info">
				<p>Erhalten Sie Hilfe bei Kroschke</p>
			</div>
			<!--subnavigation Headline-->

			<!--contentlinks-->
			<div class="contentlinks">
			<p>Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi. Nam liber tempor cum soluta nobis eleifend option congue nihil imperdiet doming id quod mazim placerat facer possim assum. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi.</p>
			</div>
			<!--contentlinks-->
			<!--contentrechts-->
			<div class="contentrechts">
				<!--teaser--><div class="teaser"><div class="teaserbild">
					<img height="66" width="200" alt="Ansprechpartner" style="margin-left: 7px; margin-right: 7px; margin-top: 10px"
                        src="../images/bild_ansprechpartner.jpg"/></div>
					<div class="teasercontent">
						<h3>Ansprechpartner</h3>
						Unser kompetentes Berater-Team steht Ihnen mit Rat und Tat 
						zur Verfügung.<br>
						<br>
						Tel +49 (0)4102 804-170<br>
						Fax +49 (0)4102 804-300<br>
						E-Mail service[at]kroschke.de
						<div class="link"><a href="#">mehr erfahren</a></div></div></div>
				<!--teaser-->
			</div>
			<!--contentrechts-->
			</div>
			<!-- CONTENTBLOCK HILFE -->
			</div>

		</div>
		<div class="maincontent_bottom">&nbsp;</div>
		<!--maincontent-->
        <asp:Literal ID="Literal1" runat="server"></asp:Literal><asp:Literal ID="litAlert"
            runat="server"></asp:Literal><asp:Label ID="lblError" Style="color: #B54D4D" runat="server" ></asp:Label>

</asp:Content>
