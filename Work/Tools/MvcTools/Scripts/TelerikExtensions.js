
//
// <Filtered Data>  (depends on "/Web/TelerikExtensions.cs", #region "Filtered Data")
//                  (depends on "/Web/TelerikExtensionsUI.cs", #region "Filtered Data")
//

var _filteredData_force_Grid_OnColumnReorder = false;

function FilteredData_Grid_OnDataBound(grid, persistColumns) {
    grid = (grid || $(this));
    FilteredData_Grid_PrepareAllCommandHrefs(grid, _filteredData_force_Grid_OnColumnReorder ? _filteredData_force_Grid_OnColumnReorder : persistColumns);
    _filteredData_force_Grid_OnColumnReorder = false;
}

function FilteredData_Grid_OnColumnReorder(grid) {
    grid = (grid || $(this));
    FilteredData_Grid_PrepareAllCommandHrefs(grid);

    _filteredData_force_Grid_OnColumnReorder = true;  
    grid.data('tGrid').ajaxRequest();
}

function FilteredData_Grid_OnColumnShowHide(grid) {
    FilteredData_Grid_OnDataBound(grid, true);
}

function FilteredData_Grid_PrepareAllCommandHrefs(grid, persistColumns) {
    var dataGrid = grid.data('tGrid');

    var jsonColumnsString = GetJsonColumns(dataGrid);

    $.each($('#' + grid.attr('id') + ' a[id$="_FilterCommand"]'), function () {
        FilteredData_Grid_PrepareCommandHref(dataGrid, $(this).attr('id'));
    });

    var persistInDb = false;
    if (typeof(persistColumns) != 'undefined') {
        persistInDb = true;
    }
    PersistColumns(jsonColumnsString, persistInDb);
}

function PersistColumns(jsonColumnsString, persistInDb) {
    
    var url = "LogonContextPersistColumns";
    if (document.URL.toLowerCase().indexOf("autohausportalmvc") > 0)
        url = document.URL + "/" + url;
    
    try {
        $.ajax(
            {
                type: "POST",
                url: url,
                data: { jsonColumns: jsonColumnsString, persistInDb: persistInDb },
                loadingShow: false,
                success: function(result) {
                    //alert(result.message);
                }
            });
    } catch(e) {
            //alert(e);
    }
}

function GetJsonColumns(grid) {
    // Update the 'columns' parameter 
    var columnsFiltered = $.grep(grid.columns, function (column, j) {
        return (column.title != "" && column.hidden !== true);
    });
    // Template-Definitionen hier nicht relevant bzw. sogar problematisch wg. Sonderzeichen
    $.each(columnsFiltered, function () {
        $(this).attr('template', "");
    });

    var jsonColumnsString = (JSON.stringify(columnsFiltered) || '~');

    return jsonColumnsString;
}

function FilteredData_Grid_PrepareCommandHref(grid, hrefId) {

    // Get the export link as jQuery object
    var $exportLink = $('#' + hrefId);

    // Get its 'href' attribute - the URL where it would navigate to
    var href = $exportLink.attr('href');

    // Update the 'page' parameter with the grid's current page
    href = href.replace(/page=([^&]*)/, 'page=' + grid.currentPage);

    // Update the 'orderBy' parameter with the grids' current sort state
    href = href.replace(/orderBy=([^&]*)/, 'orderBy=' + (grid.orderBy || '~'));

    // Update the 'filter' parameter with the grids' current filtering state
    href = href.replace(/filterBy=([^&]*)/, 'filterBy=' + (grid.filterBy || '~'));

    // Update the 'href' attribute
    $exportLink.attr('href', href);

    try 
    {
        TrySetOuterExportLink(grid, $exportLink, href, 'excel');
        TrySetOuterExportLink(grid, $exportLink, href, 'pdf');
    }
    catch(e) {}
}



//
// </Filtered Data>
//
