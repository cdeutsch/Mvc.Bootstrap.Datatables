$.fn.dataTableExt.templateCache = {
    get: function (selector) {
        if (!this.templates) { this.templates = {}; }

        var template = this.templates[selector];
        if (!template) {
            var template = _.template($(selector).html());
            this.templates[selector] = template;
        }

        return template;
    }
};

//$.fn.dataTableExt.aDataToObject = function (aData) {
//    // convert array to object (keys will be 0 to number of items in the array minus one prefixed with col)
//    var obj = {};
//    _.map(aData, function (el, i) {
//        obj["col" + i] = el;
//    });
//    return obj;
//};


/* Set the defaults for DataTables initialisation */
$.extend(true, $.fn.dataTable.defaults, {
    "bProcessing": true,
    "bServerSide": true,
    "oLanguage": {
        "sZeroRecords": "No records to display",
        "sEmptyTable": "No records to display"
    },
    "sAjaxSource": "/",
    "fnServerData": function (sSource, aoData, fnCallback) {
        // add custom filter data.
        $($('.table-filter form').serializeArray()).each(function (index, item) {
            aoData.push(item);
        });
        $.ajax({
            "dataType": 'json',
            "type": "POST",
            "url": $(this).data('ajaxsource'),
            "data": aoData,
            "success": fnCallback
        });
    },
    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
        //console.log(nRow);
        //console.log(aData);
        //console.log(iDisplayIndex);
        //console.log(this);

        // assumes action column is last column in table
        var $nRow = $(nRow),
                    $table = $(this),
                    $actions = $nRow.find('td').last(),
                    trClass = $nRow.attr('class');

        $actions.attr('nowrap', 'nowrap');

        // do template cells
        $table.find('th[data-template]').each(function (index, cell) {
            var index = $(cell).index(),
                        $tdCell = $nRow.find('td').eq(index);

            var template = $.fn.dataTableExt.templateCache.get('#' + $(cell).data('template'));
            //var obj = $.fn.dataTableExt.aDataToObject(aData);
            var html = template(aData);

            $tdCell.html(html);
        });
        
        return nRow;
    },
    "fnDrawCallback": function (dataTable) {
        // do detail rows
        $(dataTable.nTHead).find('tr[data-detailtemplate]').each(function (index, headerRow) {
            var $headerRow = $(headerRow),
                templateName = $headerRow.data('detailtemplate');

            if (templateName) {
                var template = $.fn.dataTableExt.templateCache.get('#' + templateName);

                // loop over each row and add detail row.
                $.each(dataTable.aoData, function (index, data) {
                    //var obj = $.fn.dataTableExt.aDataToObject(data._aData);
                    var $html = $(template(data._aData)),
                        $tr = $(data.nTr);
                    $html.attr('class', $tr.attr('class'));
                    $tr.after($html);
                });
            }
        });

    }
});


$.fn.dataTableExt.oApi.fnReloadAjax = function (oSettings, sNewSource, fnCallback, bStandingRedraw) {
    if (typeof sNewSource != 'undefined' && sNewSource != null) {
        oSettings.sAjaxSource = sNewSource;
    }

    // Server-side processing should just call fnDraw
    if (oSettings.oFeatures.bServerSide) {
        this.fnDraw();
        return;
    }

    this.oApi._fnProcessingDisplay(oSettings, true);
    var that = this;
    var iStart = oSettings._iDisplayStart;
    var aData = [];

    this.oApi._fnServerParams(oSettings, aData);

    oSettings.fnServerData.call(oSettings.oInstance, oSettings.sAjaxSource, aData, function (json) {
        /* Clear the old information from the table */
        that.oApi._fnClearTable(oSettings);

        /* Got the data - add it to the table */
        var aData = (oSettings.sAjaxDataProp !== "") ?
            that.oApi._fnGetObjectDataFn(oSettings.sAjaxDataProp)(json) : json;

        for (var i = 0; i < aData.length; i++) {
            that.oApi._fnAddData(oSettings, aData[i]);
        }

        oSettings.aiDisplay = oSettings.aiDisplayMaster.slice();

        if (typeof bStandingRedraw != 'undefined' && bStandingRedraw === true) {
            oSettings._iDisplayStart = iStart;
            that.fnDraw(false);
        }
        else {
            that.fnDraw();
        }

        that.oApi._fnProcessingDisplay(oSettings, false);

        /* Callback user function - for event handlers etc */
        if (typeof fnCallback == 'function' && fnCallback != null) {
            fnCallback(oSettings);
        }
    }, oSettings);
};