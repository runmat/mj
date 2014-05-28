<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Startmenue.ascx.cs" Inherits="AutohausPortal.Controls.Startmenue" %>
<%@ Import Namespace="CKG.Base.Kernel.Security" %>

<div class="startmenu_content">
    <% if (MenuChangeSource.Count > 0)
        { %>
        <h3> Zulassung </h3>
        <table>
	        <tbody>
                <% foreach (System.Data.DataRowView row in MenuChangeSource)
                    { %>
                    <tr>
                        <td class="MainmenuItemAlternate" align="left" style="white-space:nowrap;">
                            <a href='<%= ResolveClientUrl(row["AppURL"].ToString()) %>' onclick="LogPageVisit('<%= row["AppId"] %>', '<%= ResolveClientUrl(row["AppUrl"].ToString()) %>')"><%= row["AppFriendlyName"] %></a>
                        </td>
                    </tr>
                <% } %>
            </tbody>
        </table>   
        <div class="trenner">&nbsp;</div>                                 
    <% } %>
                        
    <% if (MenuChangeAHSource.Count > 0)
        { %>
        <h3> Autohaus </h3>
        <table>
	        <tbody>
                <% foreach (System.Data.DataRowView row in MenuChangeAHSource)
                    { %>
                    <tr>
                        <td class="MainmenuItemAlternate" align="left" style="white-space:nowrap;">
                            <a href='<%= ResolveClientUrl(row["AppURL"].ToString()) %>' onclick="LogPageVisit('<%= row["AppId"] %>', '<%= ResolveClientUrl(row["AppUrl"].ToString()) %>')"><%= row["AppFriendlyName"] %></a>
                        </td>
                    </tr>
                <% } %>
            </tbody>
        </table>  
        <div class="trenner">&nbsp;</div>                                 
    <% } %>
                        
    <% if (MenuReportSource.Count > 0)
        { %>
        <h3> Aufträge </h3>
        <table>
	        <tbody>
                <% foreach (System.Data.DataRowView row in MenuReportSource)
                    { %>
                    <tr>
                        <td class="MainmenuItemAlternate" align="left" style="white-space:nowrap;">
                            <a href='<%= ResolveClientUrl(row["AppURL"].ToString()) %>' onclick="LogPageVisit('<%= row["AppId"] %>', '<%= ResolveClientUrl(row["AppUrl"].ToString()) %>')"><%= row["AppFriendlyName"] %></a>
                        </td>
                    </tr>
                <% } %>
            </tbody>
        </table>     
        <div class="trenner">&nbsp;</div>                               
    <% } %>
                        
    <% if (MenuToolsSource.Count > 0)
        { %>
        <h3> Tools </h3>
        <table>
	        <tbody>
                <% foreach (System.Data.DataRowView row in MenuToolsSource)
                    { %>
                    <tr>
                        <td class="MainmenuItemAlternate" align="left" style="white-space:nowrap;">
                            <a href='<%= ResolveClientUrl(row["AppURL"].ToString()) %>' onclick="LogPageVisit('<%= row["AppId"] %>', '<%= ResolveClientUrl(row["AppUrl"].ToString()) %>')"><%= row["AppFriendlyName"] %></a>
                        </td>
                    </tr>
                <% } %>
            </tbody>
        </table>    
        <div class="trenner">&nbsp;</div>                                
    <% } %>
                        
    <% if ((User.HighestAdminLevel > AdminLevel.None) && (!User.FirstLevelAdmin))
        { %>
        <h3> Admin </h3>
        <table>
	        <tbody>
                <tr>
                    <td class="MainmenuItemAlternate" align="left" style="white-space:nowrap;">
                        <a href='<%= ResolveClientUrl(@"../Admin/AdministrationMenu.aspx") %>'>Verwaltung</a>
                    </td>
                </tr>
            </tbody>
        </table>       
        <div class="trenner">&nbsp;</div>                             
    <% } %>
</div>