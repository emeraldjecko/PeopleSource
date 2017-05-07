/// <reference path="jquery-1.6.2-vsdoc.js" />
/// <reference path="jquery-ui.js" />
$(document).ready(function () {
    $('*[data-autocomplete-url]')
        .each(function () {
            $(this).autocomplete({
                source: $(this).data("autocomplete-url"),
                select: function (event, ui) {
                    //fill selected customer details on form
                    var callbackfunction = $(this).data("callbackfunction");
                    $.ajax({
                        cache: false,
                        async: false,
                        type: "POST",
                        url: $(this).data("select-url"),
                        data: { "id": ui.item.id },

                        success: function (data) {                            
                            var func = new Function("(" + callbackfunction + ")(" + JSON.stringify(data) + ");");
                            func(); 
                        },
                        error: function(xhr, ajaxOptions, thrownError) {
                            alert('Failed to retrieve states.');
                        }
                    });
                },
            });
        });
});