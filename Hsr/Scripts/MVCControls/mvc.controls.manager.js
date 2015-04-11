function _deserializeDate(date) {

    if (date.constructor === Date) return date;
    if (typeof (date) == "string") {
        if (date.indexOf("Date(") == 1) {
            while (date.indexOf("/") != -1) date = date.replace("/", "");
            return eval("new " + date);
        }
    }

    return null;
}

$(document).ready(function () {
    MVCControlManager.SiteMaster.setDefaults();

    /* DateTime Sorting support */
    if ($.jgrid.internalParseDate != undefined) return;

    $.jgrid.internalParseDate = $.jgrid.parseDate;
    $.jgrid.parseDate = function (format, date) {

        var newDate = _deserializeDate(date);

        if (newDate != null) return newDate;
        else return $.jgrid.internalParseDate(format, date);
    }


    $.fmatter.util.InternalDateFormat = $.fmatter.util.DateFormat;
    $.fmatter.util.DateFormat = function (format, date, newformat, opts) {
        var newDate = _deserializeDate(date);
        if (newDate != null) {


            var res = newformat.replace("dd", (newDate.getDate() < 10 ? "0" : "") + newDate.getDate()).replace("d", newDate.getDate())
                               .replace("YYYY", newDate.getFullYear()).replace("YY", newDate.getFullYear()).replace("Y", newDate.getFullYear())
                               .replace("mm", (newDate.getMonth() < 9 ? "0" : "") + (newDate.getMonth() + 1)).replace("m", newDate.getMonth() + 1)
                               .replace("hh", (newDate.getHours() < 10 ? "0" : "") + newDate.getHours()).replace("h", newDate.getHours())
                               .replace("MM", (newDate.getMinutes() < 10 ? "0" : "") + newDate.getMinutes()).replace("M", newDate.getMinutes())
                               .replace("ss", (newDate.getSeconds() < 10 ? "0" : "") + newDate.getSeconds()).replace("s", newDate.getSeconds());
            return res;
        }
        else return $.fmatter.util.InternalDateFormat(format, date, newformat, opts);
    }

});

var MVCControlManager = {
    Home: {
        MVCControlManager: {}
    },
    SiteMaster: {
        setDefaults: function () {
            $.jgrid.defaults = $.extend($.jgrid.defaults, {
                datatype: 'json',
                height: 'auto',
                imgpath: '/Content/GridCss/images',
                jsonReader: {
                    root: "Rows",
                    page: "Page",
                    total: "Total",
                    records: "Records",
                    repeatitems: false,
                    userdata: "UserData",
                    id: "Id"
                },
                loadui: "block",
                mtype: 'GET',
                multiboxonly: true,
                rowNum: 10,
                rowList: [10, 20, 50],
                viewrecords: true
            });
        }
    }
};

 // Used for the validation in the built in editor
function handleMvcResponse(response, postdata) {
    return eval('(' + response.responseText + ')');
}

 function rowIsBeingEdited(grid, row) {
    var edited = "0";
    var ind = grid.getInd(row, true);

    if (ind != false) {
        edited = $(ind).attr("editable");
    }

    if (edited === "1") {
        // row is being edited        
        return true;
    } else {
        // row is not being edited
        return false;
    }
}

function getAllEditedRows(grid) {
    return grid.find("tr[editable='1']");
}

function anyRowBeingEdited(grid) {
    return getAllEditedRows(grid).length > 0;
}

function saveAllChangedRows(grid) {
    var changedRows = getAllEditedRows(grid);
    jQuery.each(changedRows, function () { grid.jqGrid('saveRow', this.id); });
    updateButtonState(grid);
}

function cancelAllChangedRows(grid) {
    var changedRows = getAllEditedRows(grid);
    jQuery.each(changedRows, function () { grid.jqGrid('restoreRow', this.id); });
    updateButtonState(grid);
}

function updateButtonState(mygrid) {
    selrow = mygrid.jqGrid('getGridParam', 'selrow');
    var gridName = mygrid[0].id;

    var updateButtons = $('input[gridMethod="update_' + gridName + '"]');
    var saveButtons = $('input[gridMethod="save_' + gridName + '"]');
    var cancelButtons = $('input[gridMethod="cancel_' + gridName + '"]');
    var deleteButtons = $('input[gridMethod="delete_' + gridName + '"]');

    if (updateButtons.length == 0 && saveButtons.length == 0 && cancelButtons.length == 0 && deleteButtons.length == 0) {
        return;
    }

    var rowIsSelected = selrow != undefined;
    updateButtons.attr('disabled', !rowIsSelected);
    deleteButtons.attr('disabled', !rowIsSelected);

    // Per row methods
    var rowBeingEdited = rowIsBeingEdited(mygrid, selrow);
    if (rowBeingEdited) {
        updateButtons.attr('disabled', true);
    }
    saveButtons.filter('[allRows="False"]').attr('disabled', !rowBeingEdited);
    cancelButtons.filter('[allRows="False"]').attr('disabled', !rowBeingEdited);

    var beingEdited = anyRowBeingEdited(mygrid);
    saveButtons.filter('[allRows="True"]').attr('disabled', !beingEdited);
    cancelButtons.filter('[allRows="True"]').attr('disabled', !beingEdited);
}

/* MVCControls jqGrid Custom Pager Implementation */
function grid_ChangePage(gridId, pageNumber) {

    var newPageNumber;
    var currentPageNumber = $("#" + gridId)[0].p.page;

    switch (pageNumber) {
        case -2:
            newPageNumber = currentPageNumber - 1;
            break;
        case -1:
            newPageNumber = currentPageNumber + 1;
            break;
        default:
            newPageNumber = pageNumber;
    }

    if ($("#" + gridId + "Page" + newPageNumber.toString()).length == 0) return;

    $("#" + gridId + "Page" + currentPageNumber.toString())[0].className = "gridPagerNormalButton";
    $("#" + gridId + "Page" + newPageNumber.toString())[0].className = "gridPagerSelectedButton";

    $("#" + gridId)[0].p.page = newPageNumber;
    $("#" + gridId).trigger("reloadGrid");
}

/* Initializes the custom MVCControls grid pager */
function grid_initPager(gridId) {

    var pageCount = $("#" + gridId)[0].p.lastpage;
    var currGrid = $("#" + gridId);
    var currButton = $("#" + gridId + "Page1");

    if (currGrid.attr("isLoaded") == "true") return;

    var currentRow;
    var i = 0;

    for (i = 2; i <= pageCount; i++) {
        currentRow = $("<td width=\"18\" id=\"" + gridId + "Page" + i.toString() + "\" onclick=\"grid_ChangePage('" + gridId + "'," + i.toString() + ");\" align=\"center\" class=\"gridPagerNormalButton\">" + i.toString() + "</td>");
        currentRow.insertAfter(currButton);
        currButton = currentRow;
    }

    currGrid.attr("isLoaded", "true");
}

/* Store dirty grid rows */
var _gridNewRows = new Array();
var _currentInsertState = null;
var _gridSelectCache = new Array();


function _grid_init(gridName) {
    _gridSelectCache[gridName] = new Array();
}

/* Add a new grid row 
Parameters: gridName      - The name of the grid
defaultObject - The default object to display (optional)
*/
function gridAddRow(gridName, defaultObject) {
    if (defaultObject == null || defaultObject == undefined) defaultObject = new Object();
    var grid = $("#" + gridName);
    var total = grid.getGridParam("records");
    grid.addRowData(total + 1, defaultObject);
    grid.editRow(total + 1);
}

/* MVCControls jqGrid bulk save support */
function gridSaveRows(gridName, saveUrl, parameterName, callback) {
    var currGrid = $("#" + gridName);
    var gridRows = getAllEditedRows(currGrid);
    var rowsData = new Array();
    var i;

    if (gridRows == undefined) return;
    var _rowIds = new Array();
    for (i = 0; i < gridRows.length; i++) {
        var id = parseInt(gridRows[i].id);
        jQuery("#" + gridName).jqGrid('saveRow', id, null, "clientArray");
        rowsData.push(currGrid.getRowData(id));
        _rowIds.push(id);
    }

    // Used when the bulk update is cancelled
    _gridNewRows[gridName] = _rowIds;

    var args = $.toJSON(rowsData, null, parameterName);

    _currentInsertState = new Object();
    _currentInsertState.callback = callback;
    _currentInsertState.gridName = gridName;
    _currentInsertState.saveUrl = saveUrl;
    _currentInsertState.parameterName = parameterName;

    $.post(saveUrl, args, _onAfterGridSave, "json");
}

/* Internal wrapper for the post callback */
function _onAfterGridSave(data) {
    var clientState = _currentInsertState;
    _currentInsertState = null;

    /* If client has specified a callback method, run it and check for cancellation */
    if (clientState.callback != null && clientState.callback != undefined) {
        if (!clientState.callback(data)) {
            /* User has cancelled the operation - make rows editable */
            var currGridRows = _gridNewRows[clientState.gridName];
            var currGrid = $("#" + clientState.gridName);

            var i = 0;
            for (i = 0; i < currGridRows.length; i++)
                currGrid.editRow(currGridRows[i]);

            return;
        }
        else {
            /* User accepted the changes */
            /* Clear dirty rows */
            _gridNewRows[clientState.gridName] = new Array();
        }
    }
}

function _grid_onCellSelect(rowId, iCol, cellcontent, e) {
    
}

/* Used by the select formatter of the grid */
function _grid_fillList(source, gridName) {
    if (source.indexOf("(") > -1)
        return eval(source);
    else {

        if ((_gridSelectCache[gridName][source] == undefined) || (_gridSelectCache[gridName][source] == null)) {
            _gridSelectCache[gridName][source] = _grid_renderCombo($.ajax({ url: source, async: false }).responseText);
        }

        return _gridSelectCache[gridName][source];
    }
}

/* Recieves a List<KeyValuePair<T,K>> and renders a select combobox */
function _grid_renderCombo(args) {
    
    var dataItems;
    eval("dataItems = " + args);
    var stringBuilder = new Array();

    for (var i = 0; i < dataItems.length; i++) 
    {
        stringBuilder.push(dataItems[i].Key + ":" + dataItems[i].Value);
    }

    var ttt = stringBuilder.join(";");
    return ttt;
}



/* jQuery toJSON plugin - patched to support MVC post-like paramters */
(function ($) {
    m = {
        '\b': '\\b',
        '\t': '\\t',
        '\n': '\\n',
        '\f': '\\f',
        '\r': '\\r',
        '"': '\\"',
        '\\': '\\\\'
    },
$.toJSON = function (value, whitelist, prefix) {
    var a,          // The array holding the partial texts.
		i,          // The loop counter.
		k,          // The member key.
		l,          // Length.
		r = /["\\\x00-\x1f\x7f-\x9f]/g,
		v;          // The member value.

    switch (typeof value) {
        case 'string':
            return value;
            //            return r.test(value) ?
            //			'"' + value.replace(r, function(a) {
            //			    var c = m[a];
            //			    if (c) {
            //			        return c;
            //			    }
            //			    c = a.charCodeAt();
            //			    return '\\u00' + Math.floor(c / 16).toString(16) + (c % 16).toString(16);
            //			}) + '"' :
            //			'"' + value + '"';

        case 'number':
            return isFinite(value) ? String(value) : 'null';

        case 'boolean':
        case 'null':
            return String(value);

        case 'object':
            if (!value) {
                return 'null';
            }
            if (typeof value.toJSON === 'function') {
                return $.toJSON(value.toJSON());
            }
            a = [];
            if (typeof value.length === 'number' &&
				!(value.propertyIsEnumerable('length'))) {
                l = value.length;
                for (i = 0; i < l; i += 1) {
                    a.push($.toJSON(value[i], whitelist, prefix + "[" + i + "]") || 'null');
                }
                return a.join("&");
                //return '[' + a.join(',') + ']';
            }
            if (whitelist) {
                l = whitelist.length;
                for (i = 0; i < l; i += 1) {
                    k = whitelist[i];
                    if (typeof k === 'string') {
                        v = $.toJSON(value[k], whitelist);
                        if (v) {
                            //a.push($.toJSON(k) + ':' + v);
                            a.push(prefix + "." + k + "=" + v);
                        }
                    }
                }
            } else {
                for (k in value) {
                    if (typeof k === 'string') {
                        v = $.toJSON(value[k], whitelist);
                        if (v) {
                            //a.push($.toJSON(k) + ':' + v);
                            a.push(prefix + "." + k + "=" + v);
                        }
                    }
                }
            }
            //            return '{' + a.join(',') + '}';
            return a.join("&");
    }
};

})(jQuery);

 

function _progressBarBinder(ctrlName, value) {

    $(ctrlName).progressbar({
			value: value
		});

};

var _accordion_loadingText = "Loading...";

function _accordion_changeItem(event, ui) {

    if (ui.newContent.length != 0) {
        var isCache = $(event.target).attr("mvc_cacheItems");
        if ((ui.newContent[0].innerHTML == _accordion_loadingText) || (isCache == "false")) {
            jQuery(ui.newContent[0]).load(ui.newHeader.attr("mvc-action"));
        }
    }
    else {
        if (ui.oldContent[0].innerHTML == _accordion_loadingText) {
            jQuery(ui.oldContent[0]).load(ui.oldHeader.attr("mvc-action"));
        }
    }
}



/*MVC.Controls.Portlets */
var _initialSpanClass = 'ui-icon-minusthick';

function _portletsInit() {
    $('.portlet-content').toggle();
    _initialSpanClass = 'ui-icon-plusthick';
    $('.portlet-content.toggleOpen').toggle();

    $('.portlet').addClass('ui-widget ui-widget-content ui-helper-clearfix ui-corner-all')
			     .find('.portlet-header')
				    .addClass('ui-widget-header ui-corner-all')
				    .prepend('<span class=\'ui-icon ' + _initialSpanClass + ' \'></span>')
				    .end()
			     .find('.portlet-content');

    $('.portlet-header .ui-icon').click(function () {
        $(this).toggleClass('ui-icon-minusthick').toggleClass('ui-icon-plusthick');
        $(this).parents('.portlet:first').find('.portlet-content').toggle();
    });

    $('.portlet-header.toggleOpen > .ui-icon').attr('class', 'ui-icon ui-icon-minusthick');
}

function _portletOnClick(portletId) 
{
    /* Fill remote portlet if expanded */
    if ($("#" + portletId + "_header > .ui-icon-minusthick").length == 1) _portletLoadContent(portletId);
}

function _portletLoadContent(portletId) {
    var pheader = $("#" + portletId + "_header");
    var mvcAction = pheader.attr("mvc-action");
    var method = pheader.attr("mvc-method");

    //Todo: add caching
    $.ajax({ type: method, url: mvcAction, success: function (html) { $("#" + portletId + "_content").html(html); } });
}