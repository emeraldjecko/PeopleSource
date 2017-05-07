var _data = [];
$("#divLoading").show();

$(function () {
    $('#side-menu').find('li').removeClass('active');
    $('#side-menu').find('#MessageOverview').addClass('active');
    Minimize();
    var url = 'MessageOverview/GetMessageOverview';
    GetJqGrid(url);
    DisplayAllTag();
    $("#CustomizeMenu").find('.MakeDisable').click(function () {
        return false;
    });

    $("#CustomizeFilterMenu").find('.MakeDisable').click(function () {
        return false;
    });
});

var DisplayAllTag = function () {
    $('#TagList').find('.AvailableTags').remove();
    $('#TagList').find('#Loading').removeClass('hidden');
    $.ajax({
        type: "GET",
        url: 'MessageOverview/GetAllTags',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            if (data.success == true) {
                if (data.AllTags.length == 0) {
                    $('#TagList').find('#Loading').addClass('hidden');
                    $('#TagList').find('#NoTag').removeClass('hidden');
                }
                else {
                    $('#TagList').find('#Loading').addClass('hidden');
                    $.each(data.AllTags, function (e, element) {
                        $('#TagList').append('<li  class="AvailableTags"  id="Tags_' + element.TagID + '"><span  id="ATags_' + element.TagID + '"><span class="SpanTag" id="SpanTag_' + element.TagID + '"><i title="Delete Tag" class="fa fa-trash-o i_icons pull-right" onclick="DeleteTag(' + element.TagID + ');return false;"></i><i title="Edit Tag" onclick="EditTag(' + element.TagID + ');return false;" class="fa fa-pencil-square-o i_icons pull-right"></i><span><i class="label label-primary" id="TotalTags_' + element.TagID + '" style="color:#FFF;font-style:normal;"  onclick="SelectedGridData(' + element.TagID + ');return false;">' + element.Total + '</i><span  class="TagName" onclick="SelectedGridData(' + element.TagID + ');return false;">' + element.TagName + '</span></span><div  class="TagID hidden">' + element.TagID + '</div></span></span></li>');
                    });
                }
            }
        },
    });
};

var SaveEditedTags = function (ID) {
    var TagName = $('#divseller').find('#EditedTagName').val();
    if ($('#SpanTag_' + ID).text().toLowerCase().trim() != $('#divseller').find('#EditedTagName').val().toLowerCase().trim()) {
        if (TagName.length > 0) {

            $.ajax({

                type: "POST",
                url: 'MessageOverview/EditTag',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ tagname: TagName, ID: ID }),
                dataType: "json",
                success: function (data) {
                    if (data.success == true) {
                        if (data.message == 'Exists') {
                            $("#divLoading").hide();
                            //FireJGrowl("Proxy is valid", "success");
                            toastr["error"]("Tag already exists", "")
                            toastr.options = {
                                "closeButton": true,
                                "debug": false,
                                "progressBar": true,
                                "positionClass": "toast-top-right",
                                "onclick": null,
                                "showDuration": "400",
                                "hideDuration": "1000",
                                "timeOut": "7000",
                                "extendedTimeOut": "1000",
                                "showEasing": "swing",
                                "hideEasing": "linear",
                                "showMethod": "fadeIn",
                                "hideMethod": "fadeOut"
                            }

                        } else {
                            $("#divLoading").hide();
                            //FireJGrowl("Proxy is valid", "success");
                            toastr["success"]("Tag has been Updated successfully", "")
                            toastr.options = {
                                "closeButton": true,
                                "debug": false,
                                "progressBar": true,
                                "positionClass": "toast-top-right",
                                "onclick": null,
                                "showDuration": "400",
                                "hideDuration": "1000",
                                "timeOut": "7000",
                                "extendedTimeOut": "1000",
                                "showEasing": "swing",
                                "hideEasing": "linear",
                                "showMethod": "fadeIn",
                                "hideMethod": "fadeOut"
                            }

                            var count = 0;
                            var _data = $("#jQGridGroup").jqGrid('getGridParam', 'data');
                            $(_data).each(function () {
                                var self = this;
                                $(self.TagDetail).each(function (i) {
                                    if (this.TagsID == ID) {
                                        self.TagDetail[i].TagName = data.TagName;
                                        count = count + 1;
                                    }
                                });
                            });
                            $("#jQGridGroup").trigger('reloadGrid');
                            $('#TagList').find('#Tags_' + ID).html();
                            $('#TagList').find('#Tags_' + ID).html('<span  id="ATags_' + ID + '" ><span class="SpanTag" id="SpanTag_' + ID + '"><i title="Delete Tag" class="fa fa-trash-o  i_icons pull-right" onclick="DeleteTag(' + ID + ');return false;"></i><i title="Edit Tag" class="fa fa-pencil-square-o i_icons pull-right" onclick="EditTag(' + ID + ');return false;"></i><span><i style="color:#FFF;font-style:normal;" id="TotalTags_' + ID + '" class="label label-primary">' + count + '</i><span  class="TagName">' + data.TagName + '<span></span><div class="TagID hidden">' + ID + '</div></span></span>');
                            $('#TagName').val('');

                        }
                    } else {
                        $("#divLoading").hide();
                        //FireJGrowl("Proxy is invalid", "error");
                        toastr["error"]("Tag is invalid", "error")
                        toastr.options = {
                            "closeButton": true,
                            "debug": false,
                            "progressBar": true,
                            "positionClass": "toast-top-right",
                            "onclick": null,
                            "showDuration": "400",
                            "hideDuration": "1000",
                            "timeOut": "7000",
                            "extendedTimeOut": "1000",
                            "showEasing": "swing",
                            "hideEasing": "linear",
                            "showMethod": "fadeIn",
                            "hideMethod": "fadeOut"
                        }
                    }
                },
            });

        } else {
            $("#divLoading").hide();
            //FireJGrowl("Proxy is invalid", "error");
            toastr["error"]("Please Enter The Tag Name First", "error")
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "onclick": null,
                "showDuration": "400",
                "hideDuration": "1000",
                "timeOut": "7000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        }
    } else {
        CancelEditedTag(ID);
    }
};

var SaveTags = function () {
    var TagName = $('#divseller').find('#TagName').val();
    if (TagName.length > 0) {

        $.ajax({

            type: "POST",
            url: 'MessageOverview/AddNewTag',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ tagname: TagName }),
            dataType: "json",
            success: function (data) {
                $('#divseller').find('#TagName').val('');
                $('#HiddenTagInput').addClass('hidden');
                $('#TagAddBtn').removeClass('hidden');
                $('#NoTag').addClass('hidden');
                if (data.success == true) {
                    if (data.message == 'Exists') {
                        GlobalMessage('error', 'Tag Already Exists');
                    } else if (data.message == 'Deleted') {
                        GlobalMessage('success', 'Tag has been added successfully');
                        $('#TagList').append('<li class="AvailableTags"  id="Tags_' + data.DataID + '"><span href="" id="ATags_' + data.DataID + '"><span class="SpanTag" id="SpanTag_' + data.DataID + '"><i title="Delete Tag" class="fa fa-trash-o i_icons pull-right" onclick="DeleteTag(' + data.DataID + ');return false;"></i><i title="Edit Tag" class="fa fa-pencil-square-o i_icons pull-right" onclick="EditTag(' + data.DataID + ');return false;"></i><span><i style="color:#FFF;font-style:normal;" id="TotalTags_' + data.DataID + '" class="label label-primary" onclick="SelectedGridData(' + data.DataID + ');return false;">0</i><span  class="TagName" onclick="SelectedGridData(' + data.DataID + ');return false;">' + data.DataName + '</span></span><div class="hidden TagID">' + data.DataID + '</div></span></span></li>');
                        $('#TagName').val('');
                    }
                    else {
                        GlobalMessage('success', 'Tag has been added successfully');
                        $('#TagList').append('<li class="AvailableTags"  id="Tags_' + data.Data.Tagid + '"><span href="" id="ATags_' + data.Data.Tagid + '"><span class="SpanTag" id="SpanTag_' + data.Data.Tagid + '"><i title="Delete Tag" class="fa fa-trash-o i_icons pull-right" onclick="DeleteTag(' + data.Data.Tagid + ');return false;"></i><i title="Edit Tag" class="fa fa-pencil-square-o i_icons pull-right" onclick="EditTag(' + data.Data.Tagid + ');return false;"></i><span><i style="color:#FFF;font-style:normal;" id="TotalTags_' + data.Data.Tagid + '" class="label label-primary" onclick="SelectedGridData(' + data.Data.Tagid + ');return false;">0</i><span  class="TagName" onclick="SelectedGridData(' + data.Data.Tagid + ');return false;">' + data.Data.TagName + '</span></span><div class="hidden TagID">' + data.Data.Tagid + '</div></span></span></li>');
                        $('#TagName').val('');
                    }
                }
            },
        });

    } else {
        GlobalMessage('error', 'Please Enter The Tag Name First');
    }
};

var SaveTagEvent = function (e) {
    if (e.keyCode == 13) {
        var TagName = $('#divseller').find('#TagName').val();
        if (TagName.length > 0) {

            $.ajax({

                type: "POST",
                url: 'MessageOverview/AddNewTag',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ tagname: TagName }),
                dataType: "json",
                success: function (data) {
                    $('#divseller').find('#TagName').val('');
                    $('#HiddenTagInput').addClass('hidden');
                    $('#TagAddBtn').removeClass('hidden');
                    $('#NoTag').addClass('hidden');
                    if (data.success == true) {
                        if (data.message == 'Exists') {
                            GlobalMessage('error', 'Tag already exists');
                        } else if (data.message == 'Deleted') {
                            GlobalMessage('success', 'Tag has been added successfully');
                            $('#TagList').append('<li class="AvailableTags"  id="Tags_' + data.DataID + '"><span href="" id="ATags_' + data.DataID + '"><span class="SpanTag" id="SpanTag_' + data.DataID + '"><i title="Delete Tag" class="fa fa-trash-o i_icons pull-right" onclick="DeleteTag(' + data.DataID + ');return false;"></i><i title="Edit Tag" class="fa fa-pencil-square-o i_icons pull-right" onclick="EditTag(' + data.DataID + ');return false;"></i><span><i style="color:#FFF;font-style:normal;" id="TotalTags_' + data.DataID + '" class="label label-primary" onclick="SelectedGridData(' + data.DataID + ');return false;">0</i><span class="TagName" onclick="SelectedGridData(' + data.DataID + ');return false;">' + data.DataName + '</span></span><div class="hidden TagID">' + data.DataID + '</div></span></span></li>');
                            $('#TagName').val('');
                        }
                        else {
                            GlobalMessage('success', 'Tag has been added successfully');
                            $('#TagList').append('<li class="AvailableTags"  id="Tags_' + data.Data.Tagid + '"><span href="" id="ATags_' + data.Data.Tagid + '"><span class="SpanTag" id="SpanTag_' + data.Data.Tagid + '"><i title="Delete Tag" class="fa fa-trash-o i_icons pull-right" onclick="DeleteTag(' + data.Data.Tagid + ');return false;"></i><i title="Edit Tag" class="fa fa-pencil-square-o i_icons pull-right" onclick="EditTag(' + data.Data.Tagid + ');return false;"></i><span><i style="color:#FFF;font-style:normal;" id="TotalTags_' + data.Data.Tagid + '" class="label label-primary" onclick="SelectedGridData(' + data.Data.Tagid + ');return false;">0</i><span  class="TagName" onclick="SelectedGridData(' + data.Data.Tagid + ');return false;">' + data.Data.TagName + '</span></span><div class="hidden TagID">' + data.Data.Tagid + '</div></span></span></li>');
                            $('#TagName').val('');
                        }
                    }
                },
            });

        } else {
            GlobalMessage('error', 'Please Enter The Tag Name First');
        }
    }
};

var OpenTagInput = function () {
    $('#HiddenTagInput').removeClass('hidden');
    $('#TagAddBtn').addClass('hidden');
};

var DeleteTag = function (ID) {

    $("#divLoading").show();
    $("#divLoading").hide();
    BootstrapDialog.show({

        message: 'Are you sure to delete this Tag? It will also affects to dependent table',
        title: 'Delete Confirmation',
        closeByBackdrop: false,
        closeByKeyboard: false,
        draggable: true,
        buttons: [{
            label: 'Yes',
            cssClass: 'btn-primary',
            action: function (e) {
                $.post('MessageOverview/DeleteTag?ID='+ID,
                    function (data) {
                        $('#TagList').find('#Tags_' + ID).remove();
                        $("#divLoading").hide();
                        var data = $("#jQGridGroup").jqGrid('getGridParam', 'data');
                        $(data).each(function () {
                            var self = this;
                            var _tagDetail = [];
                            $(self.TagDetail).each(function (i) {
                                if (!(this.TagsID == ID)) {
                                    //self.TagDetail[i] = []
                                    _tagDetail.push(this);
                                }
                            });
                            self.TagDetail = _tagDetail;
                        });

                        $("#jQGridGroup").trigger('reloadGrid');
                        //FireJGrowl("Proxy is valid", "success");
                        GlobalMessage('success', 'Tag has been deleted successfully');
                        if (!($('#TagList').find('li').hasClass('AvailableTags')))
                            $('#TagList').find('#NoTag').removeClass('hidden');
                        e.close();
                    }
                );
            },

        }, {
            label: 'No',
            cssClass: 'btn-primary',
            action: function (dialogItself) {
                dialogItself.close();
            }
        }]
    });
};

var EditTag = function (ID) {
    $('#TagList').find('.SpanTag').removeClass('hidden');
    $('#TagList').find('#EditedSpanTag').remove();
    $.ajax({
        type: "POST",
        url: 'MessageOverview/GetEditedData',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ ID: ID }),
        dataType: "json",
        success: function (data) {
            if (data.success == true) {
                $('#TagList').find('#Tags_' + data.Tagid).find('#ATags_' + data.Tagid).find('#SpanTag_' + data.Tagid).addClass('hidden');
                $('#TagList').find('#Tags_' + data.Tagid).find('#ATags_' + data.Tagid).append('<span id="EditedSpanTag"><input id="EditedTagName" class="form-control-sm EditedInputBox" placeholder="Tag Name" value=\'' + data.TagName + '\'><i onclick="CancelEditedTag(' + data.Tagid + ');return false;" title="Cancel" class="fa fa-times i_icons pull-right" style="margin-top:5px;"></i><i onclick="SaveEditedTags(' + data.Tagid + ');return false;" style="margin-top:5px;" title="Update" class="fa fa-save i_icons pull-right"></i></span>');
            }
        },
    });
};

var CancelEditedTag = function (ID) {
    $('#TagList').find('#Tags_' + ID).find('#ATags_' + ID).find('#SpanTag_' + ID).removeClass('hidden');
    $('#TagList').find('#EditedSpanTag').remove();
};

var SetUnReadOrRead = function (Type) {
    var SeletedROWs = $('#jQGridGroup').getSelectedRows();
    $(SeletedROWs).each(function () {
        _data.push(this.MessageID);
    });
    var _RowIds = [];
    _RowIds = jQuery("#jQGridGroup").jqGrid('getGridParam', 'selarrrow');
    $.ajax({

        type: "POST",
        url: 'MessageOverview/SetUnReadOrRead',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ MessageList: _data.toString(), Type: Type }),
        dataType: "json",
        success: function (data) {
            if (data.success == true) {
                _data = [];
                $("#divLoading").hide();
                //FireJGrowl("Proxy is valid", "success");
                if (Type == 'Read') {
                    $.each(_RowIds, function (e, element) {
                        $('#jQGridGroup').find('#' + element + '').find('.Pstd').find('td').attr('style', 'padding:5px;border:none;');
                    });
                } else {
                    $.each(_RowIds, function (e, element) {
                        $('#jQGridGroup').find('#' + element + '').find('.Pstd').find('td').attr('style', 'font-weight:bold; padding:5px;border:none;');
                    });
                }
                GlobalMessage('success', 'Tag has been assign As ' + Type + ' Successfully..');
            }
        }
    });
};

var TagList = function () {
    var SeletedROWs = $('#jQGridGroup').getSelectedRows();
    $(SeletedROWs).each(function () {
        _data.push(this.MessageID);
    });
    if (_data.length == 0) {
        $("#divLoading").hide();
        //FireJGrowl("Proxy is invalid", "error");
        GlobalMessage('error', 'Please Select Atleast 1 Message From List');
    } else {
        $("#myModal").modal('show');
        $('#myModal').find('#AssignTagList').empty();
        if ($('#divseller').find('#TagList').find('.AvailableTags').find('.SpanTag').length > 0) {
            $.each($('#divseller').find('#TagList').find('.AvailableTags').find('.SpanTag'), function (e, element) {
                $('#myModal').find('#AssignTagList').append('<div style="margin-bottom:5px;" class="col-lg-12 chcks"><label class="checkbox-inline i-checks pull-left"><div class="icheckbox_square-green" id ="checked_' + $(element).find('div').text() + '" onclick="checked(' + $(element).find('div').text() + ');return false;" style="position: relative;"><span class="Tag_class hidden">' + $(element).find('div').text() + '</span><input type="checkbox" value="option1" style="position: absolute; opacity: 0;"><ins class="iCheck-helper"></ins></div><span class="AssTags" style="font-weight: bold;font-size:14px;padding:5px;">' + $(element).find('.TagName').text() + '</span></label></div>');
                //'<label class=""> <div class="icheckbox_square-green" style="position: relative;"><input type="checkbox" class="i-checks" style="position: absolute; opacity: 0;"><ins class="iCheck-helper" style=""></ins></div><span class="AssTags" style="font-weight: bold;font-size:18px;">' + $(element).text() + '</span></label>');
            });
            $('#myModal').find('#Savebtn').removeClass('hidden');
        } else {
            $('#myModal').find('#Savebtn').addClass('hidden');
            $('#myModal').find('#AssignTagList').html('<span style="color:red;font-size:16px;margin-top:20px;margin-bottom:20px;">No Tag Found....</span>');
        }
    }
};

var checked = function (value) {
    if ($('#myModal').find('#AssignTagList').find('#checked_' + value + '').hasClass('checked')) {
        $('#myModal').find('#AssignTagList').find('#checked_' + value + '').removeClass('checked');
    } else {
        $('#myModal').find('#AssignTagList').find('#checked_' + value + '').addClass('checked');
    }
};

var SaveAssignTag = function () {
    var CheckedValues = [];
    $.each($('#myModal').find('#AssignTagList').find('.chcks'), function (e, element) {
        if ($(element).find('div').hasClass('checked')) {
            CheckedValues.push($(element).find('.Tag_class').text());
        }
    });
    _data = [];
    var SeletedROWs = $('#jQGridGroup').getSelectedRows();
    $(SeletedROWs).each(function () {
        _data.push(this.MessageID);
    });
    if (CheckedValues.length == 0) {
        $("#myModal").modal('show');
        $("#divLoading").hide();
        GlobalMessage('error', 'Please Select Atleast 1 Tag');
    } else {
        $.ajax({

            type: "POST",
            url: 'MessageOverview/AssignTag',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ TagIdList: _data.toString(), CheckedTagList: CheckedValues.toString() }),
            dataType: "json",
            success: function (data) {
                $('#jQGridGroup').jqGrid('resetSelection');
                $("#myModal").modal('hide');
                if (data.success == true) {
                    $.each(data.AllTags, function (e, element) {
                        $('#TagList').find('#Tags_' + element.TagID).find('#TotalTags_' + element.TagID).text(element.Total);
                    });
                    $.each(data.MessageTagArray, function (e, element) {
                        $('#jQGridGroup').find('#Div_MessageID_' + element.MessageID + '').empty();
                    });
                    $.each(data.MessageTagArray, function (e, element) {
                        $('#jQGridGroup').find('#Div_MessageID_' + element.MessageID + '').append("<div id='Div_MessageID_" + element.MessageID + "'><div id='LTags_" + element.MessageID + "_" + element.TagsID + "' style='padding-top:3px;padding-bottom:3px;'><i class='fa fa-tags' style='margin-left:5px;'></i><span style='margin-left:10px;'>" + element.TagName + "</span> <i onclick='DeleteListTag(" + element.TagsID + "," + element.MessageID + ");return false;' class='fa fa-trash-o i_icons pull-right' style='margin-right:5px;' title='Delete Tag'></i></div></div>");
                    });
                    $("#divLoading").hide();
                    GlobalMessage('success', 'Tag has been assign successfully');
                } else {
                    $("#divLoading").hide();
                    GlobalMessage('error', 'Something Wrong');
                }
            }
        });
    }
};

var DeleteListTag = function (TagID, MessageID) {
    $("#divLoading").show();
    $("#divLoading").hide();
    BootstrapDialog.show({

        message: 'Are you sure to delete this Tag? It will also affects to dependent table',
        title: 'Delete Confirmation',
        closeByBackdrop: false,
        closeByKeyboard: false,
        draggable: true,
        buttons: [{
            label: 'Yes',
            cssClass: 'btn-primary',
            action: function (e) {
                $.post('MessageOverview/DeleteExistingTag?ID='+TagID+'&MID='+ MessageID,
                    function (data) {
                        $('#LTags_' + MessageID + '_' + TagID + '').remove();
                        var Count = $('#Tags_' + TagID + '').find('#TotalTags_' + TagID + '').text();
                        $('#Tags_' + TagID + '').find('#TotalTags_' + TagID + '').text(parseInt(Count) - 1);
                        $("#divLoading").hide();
                        GlobalMessage('success', 'Tag has been deleted successfully');
                        e.close();
                    }
                );
            },

        }, {
            label: 'No',
            cssClass: 'btn-primary',
            action: function (dialogItself) {
                dialogItself.close();
            }
        }]
    });
};

var DeleteEbayMessage = function (MessageID, RowID) {
    $("#divLoading").show();
    $("#divLoading").hide();
    BootstrapDialog.show({

        message: 'Are you sure to delete this Message? It will also affects to dependent table',
        title: 'Delete Confirmation',
        closeByBackdrop: false,
        closeByKeyboard: false,
        draggable: true,
        buttons: [{
            label: 'Yes',
            cssClass: 'btn-primary',
            action: function (e) {
                $.post('MessageOverview/DeleteExistingEbayMsg?MID='+ MessageID,
                    function (data) {
                        if (data.success == true) {
                            $.each(data.ListTagArray, function (e, element) {
                                var Count = $('#Tags_' + element.TagsID + '').find('#TotalTags_' + element.TagsID + '').text();
                                $('#Tags_' + element.TagsID + '').find('#TotalTags_' + element.TagsID + '').text(parseInt(Count) - 1);
                            });
                            $('#jQGridGroup').jqGrid('delRowData', RowID)
                            $("#divLoading").hide();
                            GlobalMessage('success', 'Message has been deleted successfully');
                            e.close();
                        }
                    }
                );
            },
        }, {
            label: 'No',
            cssClass: 'btn-primary',
            action: function (dialogItself) {
                dialogItself.close();
            }
        }]
    });
};

var DeleteSelected = function () {

    var SeletedROWs = $('#jQGridGroup').getSelectedRows();
    _data = [];
    $(SeletedROWs).each(function () {
        _data.push(this.MessageID);
    });
    var _RowIds = [];
    _RowIds = jQuery("#jQGridGroup").jqGrid('getGridParam', 'selarrrow');
    $("#divLoading").show();
    $("#divLoading").hide();
    BootstrapDialog.show({

        message: 'Are you sure to delete this Message? It will also affects to dependent table',
        title: 'Delete Confirmation',
        closeByBackdrop: false,
        closeByKeyboard: false,
        draggable: true,
        buttons: [{
            label: 'Yes',
            cssClass: 'btn-primary',
            action: function (e) {
                $.post('MessageOverview/DeleteSelectedEbayMsg?MID='+ _data.toString(),
                    function (data) {
                        if (data.success == true) {
                            $.each(data.ListTagArray, function (e, element) {
                                var Count = $('#Tags_' + element.TagsID + '').find('#TotalTags_' + element.TagsID + '').text();
                                $('#Tags_' + element.TagsID + '').find('#TotalTags_' + element.TagsID + '').text(parseInt(Count) - 1);
                            });
                            $(_RowIds).each(function (i, element) {
                                $('#jQGridGroup').jqGrid('delRowData', element)
                            });
                            $("#divLoading").hide();
                            $("#jQGridGroup").trigger('reloadGrid');
                            GlobalMessage('success', 'Selected Messages has been deleted successfully');
                            e.close();
                        }
                    }
                );
            },

        }, {
            label: 'No',
            cssClass: 'btn-primary',
            action: function (dialogItself) {
                dialogItself.close();
            }
        }]
    });
};

var ShowAllData = function () {
    $("#jQGridGroup").GridUnload();
    var url = 'MessageOverview/GetMessageOverview';
    GetJqGrid(url);
};

var SelectedGridData = function (TagID) {
    $("#jQGridGroup").GridUnload();
    var url = 'MessageOverview/GetMessageOverviewById?TagID=' + TagID;
    GetJqGrid(url);
};

var AddNotes = function (MessageId) {
    $('#Notes_Popup').find('#NoteContent').val('');
    $.ajax({
        type: "POST",
        url: 'MessageOverview/ShowNote',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ MsgID: MessageId }),
        dataType: "json",
        success: function (data) {
            if (data.success == true) {
                if (data.Notes != null)
                    $('#Notes_Popup').find('#NoteContent').val(data.Notes);
                $('#Notes_Popup').find('#Save_Notes').attr('onclick', 'SaveNotes(' + MessageId + ');return false;');
                $("#Notes_Popup").modal('show');
            } else {
                $("#divLoading").hide();
                GlobalMessage('error', 'InValid Selection');
            }
        }
    });
};

var ClosePopup = function () {
    $("#Notes_Popup").modal('hide');
};

var SaveNotes = function (MsgId) {
    var Note = $('#Notes_Popup').find('#NoteContent').val();
    if (Note.length != 0) {
        $.ajax({
            type: "POST",
            url: 'MessageOverview/SaveNote',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ MsgID: MsgId, Note: Note }),
            dataType: "json",
            success: function (data) {
                if (data.success == true) {
                    $("#divLoading").hide();
                    $("#Notes_Popup").modal('hide');
                    GlobalMessage('success', 'Note has been added successfully');
                } else {
                    $("#divLoading").hide();
                    GlobalMessage('error', 'InValid Selection');
                }
            }
        });
    } else {
        $("#divLoading").hide();
        GlobalMessage('error', 'Please Write a Note First');
    }
};


var FilteNotePopup = function () {
    $('#FilterBox').find('#FilterNote').val('');
    if ($('#FilterBox').hasClass('hidden'))
        $('#FilterBox').removeClass('hidden');
    else {
        $('#FilterBox').addClass('hidden');
    }
};

var BackToCustomeFilter = function () {
    $('#CustomizeMenu').removeClass('hidden');
    $('#CustomizeFilterMenu').addClass('hidden');
};

var Resetall = function () {
    $('#CustomizeMenu').removeClass('hidden');
    $('#CustomizeFilterMenu').addClass('hidden');
    $('#DateList option:eq(0)').prop('selected', true);
    $('#txtSearch').val('');
    $("#Subject").val('');
    $("#FromDatepkr").val('');
    $("#UptoDate").val('');
    $("#ToDatepkr").val('');
    $('#chkboxattechment').removeClass('checked');
    $('#chkboxnot').removeClass('checked');
    $('#HasWord').val('');
    //  $('#CatagoryList option:eq(0)').prop('selected', true);
    $('#CatagoryList').empty();

};

var Clicked = function () {
    //  Resetall();  
    $('#CatagoryList').empty();
    $('#CatagoryFilterList').empty();
    $('#CatagoryList').append('<option value="0" class="MakeDisable">-- Select Tag --</option>');
    $('#CatagoryFilterList').append('<option value="0" class="MakeDisable">-- Select Tag --</option>');
    $.each($('#TagList').find('.AvailableTags'), function (e, element) {
        $('#CatagoryList').append('<option value="' + $(element).find('.TagID').text() + '" class="MakeDisable">' + $(element).find('.TagName').text() + '</option>');
        $('#CatagoryFilterList').append('<option value="' + $(element).find('.TagID').text() + '" class="MakeDisable">' + $(element).find('.TagName').text() + '</option>');
    });
    $('.dropdown-menu').toggle();
};

$(document).click(function () {

    if ($('#divseller').find('.dropdown-menu').css('display') != 'none') {
        $('.dropdown-menu').toggle();
        //Resetall();
    }
});

var chkclickedCustome = function (ID) {
    if (ID == 'MarkasRead') {
        $('#MarkasUnRead').removeClass('checked');
        if ($('#' + ID + '').hasClass('checked')) {
            $('#' + ID + '').removeClass('checked');
        } else {
            $('#' + ID + '').addClass('checked');
        }
    } else if (ID == 'MarkasUnRead') {
        $('#MarkasRead').removeClass('checked');
        if ($('#' + ID + '').hasClass('checked')) {
            $('#' + ID + '').removeClass('checked');
        } else {
            $('#' + ID + '').addClass('checked');
        }
    }
    else {
        if ($('#' + ID + '').hasClass('checked')) {
            $('#' + ID + '').removeClass('checked');
        } else {
            $('#' + ID + '').addClass('checked');
        }
    }

};

var chkclicked = function (ID) {
    if ($('#' + ID + '').hasClass('checked')) {
        $('#' + ID + '').removeClass('checked');
    } else {
        $('#' + ID + '').addClass('checked');
    }
};

$("#UptoDate").click(function () {
    $(this).datetimepicker({
        controlType: 'select',
        format: 'Y/m/d',
        timepicker: false
    }).datetimepicker("show")
});

var CheckCustomeFilter = function () {
    var HasWord = '';
    var chkboxattchment = false;
    var CatagoryList = '';
    var chkboxnote = false;
    var From = '';
    var To = '';
    var Subject = '';
    var GlobalString = '';
    var FromDate = '';
    var ToDate = '';
    HasWord = $('#HasWord').val();
    CatagoryList = $('#CatagoryList :selected').val();
    FromDate = $('#DateList :selected').val();
    ToDate = $('#UptoDate').val();
    chkboxattchment = $('#chkboxattechment').hasClass('checked');
    chkboxnote = $('#chkboxnot').hasClass('checked');
    Subject = $("#Subject").val();
    From = $("#FromDatepkr").val();
    To = $("#ToDatepkr").val();
    if (HasWord.length == 0 && CatagoryList == 0 && chkboxattchment == false && chkboxnote == false && Subject.length == 0 && From.length == 0 && To.length == 0 && GlobalString.length == 0 && FromDate == 0 && ToDate.length == 0) {
        $("#divLoading").hide();
        GlobalMessage('error', 'Please Select atleast any one or multiple option for search..');
    } else {
        $('#CustomizeMenu').addClass('hidden');
        $('#CustomizeFilterMenu').removeClass('hidden');
    }
};

var CustomeFilterSearch = function () {
    var HasWord = '';
    var chkboxattchment = false;
    var CatagoryList = '';
    var chkboxnote = false;
    var From = '';
    var To = '';
    var Subject = '';
    var GlobalString = '';
    var FromDate = '';
    var ToDate = '';
    var Note = '';
    Note = $('#FilterBox').find('#FilterNote').val();
    HasWord = $('#HasWord').val();
    CatagoryList = $('#CatagoryList :selected').val();
    FromDate = $('#DateList :selected').val();
    ToDate = $('#UptoDate').val();
    chkboxattchment = $('#chkboxattechment').hasClass('checked');
    chkboxnote = $('#chkboxnot').hasClass('checked');
    Subject = $("#Subject").val();
    From = $("#FromDatepkr").val();
    To = $("#ToDatepkr").val();
    if (HasWord.length == 0 && CatagoryList == 0 && chkboxattchment == false && chkboxnote == false && Subject.length == 0 && From.length == 0 && To.length == 0 && GlobalString.length == 0 && FromDate == 0 && ToDate.length == 0 && Note.length == 0) {
        $("#divLoading").hide();
        GlobalMessage('error', 'Please Select atleast any one or multiple option for search..');
    } else {
        FromDate = $('#DateList :selected').text();
        var Type = 'CutomeFilter';
        if (CatagoryList != 0) {
            CatagoryList = $('#CatagoryList :selected').text().trim();
        } else {
            CatagoryList = '';
        }
        var AllRead = $('#MarkasRead').hasClass('checked');
        var AllUnRead = $('#MarkasUnRead').hasClass('checked');
        var AllDelete = $('#ChkDelete').hasClass('checked');
        var TagID = $('#CatagoryFilterList :selected').val()
        if (TagID == 0 && AllRead == false && AllUnRead == false && AllDelete == false) {
            $("#divLoading").hide();
            GlobalMessage('error', 'Please Select atleast any one option for Customize filter..');
        } else {
            $("#jQGridGroup").GridUnload();
            var url = 'MessageOverview/GetCustomeFilterMessageOverview?GlobalString=' + GlobalString + '&HasWord=' + HasWord + '&FilterTagName=' + CatagoryList + '&chkboxattchment=' + chkboxattchment + '&chkboxnot=' + chkboxnote + '&Subject=' + Subject + '&From=' + From + '&To=' + To + '&FromDate=' + FromDate + '&ToDate=' + ToDate + '&AllRead=' + AllRead + '&AllUnRead=' + AllUnRead + '&AllDelete=' + AllDelete + '&TagID=' + TagID + '&Note=' + Note;
            GetJqGrid(url);
        }
    }
};
   
var SearchFilter = function (Type) {
    var IsValid = true;
    var HasWord = '';
    var chkboxattchment = false;
    var CatagoryList = '';
    var chkboxnote = false;
    var From = '';
    var To = '';
    var Subject = '';
    var GlobalString = '';
    var FromDate = '';
    var ToDate = '';
    if (Type == 'FilterSearch') {
        HasWord = $('#HasWord').val();
        CatagoryList = $('#CatagoryList :selected').val();
        FromDate = $('#DateList :selected').text();
        ToDate = $('#UptoDate').val();
        chkboxattchment = $('#chkboxattechment').hasClass('checked');
        chkboxnote = $('#chkboxnot').hasClass('checked');
        Subject = $("#Subject").val();
        From = $("#FromDatepkr").val();
        To = $("#ToDatepkr").val();
    } else {
        GlobalString = $('#txtSearch').val();
    }
    if (IsValid) {

        if (HasWord.length == 0 && CatagoryList == 0 && chkboxattchment == false && chkboxnote == false && Subject.length == 0 && From.length == 0 && To.length == 0 && GlobalString.length == 0 && FromDate.length == 0 && ToDate.length == 0) {
            $("#divLoading").hide();
            GlobalMessage('error', 'Please Select atleast any one or multiple option for search..');
        } else {
            if (CatagoryList != 0) {
                CatagoryList = $('#CatagoryList :selected').text().trim();
            } else {
                CatagoryList = '';
            }
            $("#jQGridGroup").GridUnload();
            var url = 'MessageOverview/GetFilterMessageOverview?Type=' + Type + '&GlobalString=' + GlobalString + '&HasWord=' + HasWord + '&CatagoryList=' + CatagoryList + '&chkboxattchment=' + chkboxattchment + '&chkboxnot=' + chkboxnote + '&Subject=' + Subject + '&From=' + From + '&To=' + To + '&FromDate=' + FromDate + '&ToDate=' + ToDate;
            GetJqGrid(url);
            Clicked();
        }
    } else {
        $("#divLoading").hide();
        GlobalMessage('error', 'Please Enter Both From Date and To Date..');
    }
};

var GlobalSearch = function () {
    var txtSrch = $('#txtSearch').text();
    if (txtSrch.length == 0) {
        $("#divLoading").hide();
        GlobalMessage('error', 'Please Write keyword for Search..');
    } else {
    }
};

var ManageFilter = function () {
    var url = 'Filter/GetFilterList';
    $("#Filter_Popup").modal('show');
    GetFilterDetails(url);
    //$('#FilterjQGridGroup').trigger('reloadGrid');
   // $("#Filter_Popup").modal('show');
};

var DeleteFilter = function (FilterID, RowID) {
    $("#divLoading").show();
    $("#divLoading").hide();
    BootstrapDialog.show({

        message: 'Are you sure to delete this Filter?',
        title: 'Delete Confirmation',
        closeByBackdrop: false,
        closeByKeyboard: false,
        draggable: true,
        buttons: [{
            label: 'Yes',
            cssClass: 'btn-primary',
            action: function (e) {
                $.post('DeleteFilter?FilterID:'+FilterID,
                    function (data) {
                        $('#FilterjQGridGroup').jqGrid('delRowData', RowID);
                        $("#divLoading").hide();
                        GlobalMessage('success', 'Filter has been deleted successfully');
                        e.close();
                    }
                );
            },

        }, {
            label: 'No',
            cssClass: 'btn-primary',
            action: function (dialogItself) {
                dialogItself.close();
            }
        }]
    });
};

var ShowFilterDetails = function (FilterID) {
    $.ajax({
        type: "POST",
        url: 'Filter/GetFilterAction?FilterID=' + FilterID,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            if (data.success == true) {
                if (data.ActionList != null) {
                    $('#Filter_Action').find('#Action_AssignTag').val(data.ActionList[0].TagName);
                    $('#Filter_Action').find('#Action_AssignNote').text(data.ActionList[0].Note);
                    if (data.ActionList[0].UnRead == true) {
                        $('#Action_chkUnRead').addClass('checked');
                    } else {
                        $('#Action_chkUnRead').removeClass('checked');
                    }
                    if (data.ActionList[0].Delete == true) {
                        $('#Action_chkDelete').addClass('checked');
                    } else {
                        $('#Action_chkDelete').removeClass('checked');
                    }
                    if (data.ActionList[0].Read == true) {
                        $('#Action_chkRead').addClass('checked');
                    } else {
                        $('#Action_chkRead').removeClass('checked');
                    }
                    $('#Filter_Action').modal('show');
                }
            }
        },
    });
};

$(window).bind('resize', function () {
    var width = $('#jqGrid_container').width();
    $('#jQGridGroup').setGridWidth(width);
});

var GetJqGrid = function (url) {
    $("#jQGridGroup").jqGrid({
        url: url,
        datatype: "json",
        //colNames: ['Sender', 'Receiver', 'MessageType', 'Subject', 'Read', 'Attachment', 'Notes', 'Received', 'Actions'],
        colNames: ['MessageID', 'ExternalMessageID', 'Type', 'Read', 'TagDetail', 'Sender', 'Receiver', '<i class="fa fa-envelope" title="MessageType"></i>', 'Subject', '<i style="font-size:15px;" class="fa fa-pencil-square" title="Notes"></i>', 'Tag', 'Date', ''],
        hideColumns: ["MessageID", "ExternalMessageID", "Type", "Read", "TagDetail"],
        colModel: [
           {
               name: 'MessageID',
               index: 'MessageID',
               hidden: true,
           },
            {
                name: 'ExternalMessageID',
                index: 'ExternalMessageID',
                hidden: true,
            },
            {
                name: 'Type',
                index: 'Type',
                hidden: true,
            },
             {
                 name: 'Read',
                 index: 'Read',
                 hidden: true,
             }, {
                 name: 'TagDetail',
                 index: 'TagDetail',
                 hidden: true,
             },
            {
                name: 'Sender',
                index: 'Sender',
                width: 15,
                stype: 'text',
                formatter: function (cellvalue, options, rowObject) {
                    if (rowObject.Sender != null) {
                        var x = "<div style='padding-top:12px; padding-bottom:12px' class='paddingright'>" + rowObject.Sender;
                        x = x + "</div>";
                        return x;
                    }
                },
                sortable: true,
                editable: false,
                searchoptions: { sopt: ['cn', 'eq', 'ne', 'bw'] }
            },

            {
                name: 'RecipientUserID',
                index: 'RecipientUserID',
                width: 12,
                stype: 'text',
                formatter: function (cellvalue, options, rowObject) {

                    if (rowObject.RecipientUserID != null && rowObject.RecipientUserID != undefined) {
                        var x = "<div style='padding-top:12px; padding-bottom:12px' class='paddingright'>" + rowObject.RecipientUserID;
                        x = x + "</div>";
                        return x;
                    }
                },
                sortable: true,
                editable: false,
                searchoptions: { sopt: ['cn', 'eq', 'ne', 'bw'] }
            },
            {
                name: 'MessageType',
                index: 'MessageType',
                width: 5,
                stype: 'text',
                style: 'text-align:center',
                formatter: function (cellvalue, options, rowObject) {
                    if (rowObject.MessageType != null && rowObject.MessageType != 'undefined') {
                        var x;
                        if (rowObject.MessageType == 'ResponseToASQQuestion') {
                            x = "<div style='text-align:center;'><i class='fa fa-circle-thin' title='ResponseToASQQuestion' style='color:red;font-size:20px;;'></i></div>";
                        } else if (rowObject.MessageType == 'CustomCode') {
                            x = "<div style='text-align:center;'><i class='fa fa-circle-thin' title='CustomCode' style='color:blue;font-size:20px;;'></i></div>";
                        } else if (rowObject.MessageType == 'AskSellerQuestion') {
                            x = "<div style='text-align:center;'><i class='fa fa-circle-thin' title='AskSellerQuestion' style='color:lawngreen;font-size:20px;;'></i></div>";
                        } else if (rowObject.MessageType == 'ContactTransactionPartner') {
                            x = "<div style='text-align:center;'><i class='fa fa-circle-thin' title='ContactTransactionPartner' style='color:yellow;font-size:20px;;'></i></div>";
                        } else if (rowObject.MessageType == 'ContactEbayMember') {
                            x = "<div style='text-align:center;'><i class='fa fa-circle-thin' title='ContactEbayMember' style='color:gray;font-size:20px;;'></i></div>";
                        } else if (rowObject.MessageType == 'UNKNOWN') {
                            x = "<div style='text-align:center;'><i class='fa fa-circle-thin' title='UNKNOWN' style='color:black;font-size:20px;;'></i></div>";
                        } else if (rowObject.MessageType == 'REPLACEMENT') {
                            x = "<div style='text-align:center;'><i class='fa fa-circle-thin' title='REPLACEMENT' style='color:cyan;font-size:20px;;'></i></div>";
                        } else if (rowObject.MessageType == 'MONEYBACK') {
                            x = "<div style='text-align:center;'><i class='fa fa-circle-thin' title='MONEYBACK' style='color:gold;font-size:20px;;'></i></div>";
                        }
                        else {
                            x = "<div style='text-align:center;'><i class='fa fa-circle-thin' title='None' style='color:thistle;font-size:20px;;'></i></div>";
                        }
                        return x;
                    } else {
                        return "<div style='text-align:center;'><i class='fa fa-circle-thin' title='None' style='color:thistle;font-size:20px;;'></i></div>";
                    }
                },
                sortable: true,
                editable: false,
                searchoptions: { sopt: ['cn', 'eq', 'ne', 'bw'] }
            },
            {
                name: 'Subject',
                index: 'Subject',
                width: 30,
                stype: 'text',
                formatter: function (cellvalue, options, rowObject) {

                    if (rowObject.Subject != null) {
                        var x = null;
                        if (rowObject.Read == true) {
                            x = "<table class='Pstd'><tbody><tr><td style=' padding:5px;border:none;'>" + rowObject.Subject + "</td><td align='top' style='padding:5px;border:none;'><i style='font-size:20px' class='fa fa-paperclip' title='Attachment'></i></td></tr></tbody></table>";
                        } else {
                            x = "<table class='Pstd'><tbody><tr><td style='font-weight:bold; padding:5px;border:none;'>" + rowObject.Subject + "</td><td style='padding:5px;border:none;'align='top'><i style='font-size:20px' class='fa fa-paperclip' title='Attachment'></i></td></tr></tbody></table>";
                        }
                        return x;
                    }
                },
                sortable: true,
                editable: false,
                searchoptions: { sopt: ['cn', 'eq', 'ne', 'bw'] }
            }, {
                name: 'Notes',
                index: 'Notes',
                width: 5,
                stype: 'text',
                formatter: function (cellvalue, options, rowObject) {

                    var x = "<div style='padding-top:12px; padding-bottom:12px;text-align:center;'>" +
                            "<a style='cursor: pointer;color:#1ab394; margin-left:5px;'><i style='font-size:20px;' class='fa fa-pencil-square' title='Notes' onclick='AddNotes(" + rowObject.MessageID + ")'></i></a>" +
                        "</div>"
                    return x;

                },
                sortable: true,
                editable: false,
                searchoptions: { sopt: ['cn', 'eq', 'ne', 'bw'] }
            }, {
                name: 'Tags',
                index: 'Tags',
                width: 15,
                stype: 'text',
                formatter: function (cellvalue, options, rowObject) {
                    var x = null;
                    if (rowObject.TagDetail != null) {
                        x = "<div id='Div_MessageID_" + rowObject.MessageID + "'>";
                        if (rowObject.TagDetail.length > 0) {

                            $.each(rowObject.TagDetail, function (e, element) {
                                x = x + ("<div id='LTags_" + rowObject.MessageID + "_" + element.TagsID + "' style='padding-top:3px;padding-bottom:3px;'><i class='fa fa-tags' style='margin-left:5px;'></i><span style='margin-left:10px;'>" + element.TagName + "</span> <i onclick='DeleteListTag(" + element.TagsID + "," + rowObject.MessageID + ");return false;' class='fa fa-trash-o i_icons pull-right' style='margin-right:5px;' title='Delete Tag'></i></div>");
                            });
                        }
                        return x + "</div>";
                    }
                },
                sortable: true,
                editable: false,
                searchoptions: { sopt: ['cn', 'eq', 'ne', 'bw'] }
            }, {
                classes: 'paddingright',
                name: 'ReceiveDate',
                index: 'ReceiveDate',
                width: 10,
                stype: 'date',
                formatter: 'date',
                sorttype: 'date',
                datefmt: 'm/d/Y',
                formatoptions: { srcformat: 'm/d/Y h:m', newformat: 'd/m/Y   h:i A' },
                sortable: true,
                editable: false,
                search: true,
                searchoptions: { sopt: ['eq', 'ne'] }
            },
            {
                name: 'SendingUserID',
                index: 'SendingUserID',
                width: 6,
                stype: 'text',
                formatter: function (cellvalue, options, rowObject) {

                    if (rowObject.MessageID != null) {
                        var x = "<div style='padding-top:12px; padding-bottom:12px;text-align:center;'>" +
                                      "<a style='cursor: pointer;color:#1ab394;' class='pull-right' onclick='DeleteEbayMessage(" + rowObject.MessageID + "," + options.rowId + ");'><i class='fa fa-trash fa-lg fa-fw' title='Delete Details'></i></a><a style='cursor: pointer;color:#1ab394;' class='ActionBtn' onclick='ShowMessageDetailsNewPages(" + rowObject.SendingUserID + "," + rowObject.MessageID + "," + rowObject.Type + ");'><i class='fa fa-pencil-square-o fa-lg fa-fw' title='View Details'></i></a>" +
                                  "</div>";
                        return x.replace("Id", rowObject.MessageID);
                    }
                },

                sortable: false,
                search: false,
                editable: false,

            }
        ],
        multiselect: true,
        rowNum: 10,
        rowList: [10, 50, 100, 150, 200, 1000000],
        mtype: 'GET',
        pager: '#jQGridGroupPager',
        loadonce: true,
        scroll: false,
        viewrecords: true,
        async: true,
        height: 500,
        autowidth: true,
        shrinkToFit: true,
        ignorecase: true,
        //jsonReader: { repeatitems: false, id: "0" },
        loadComplete: function () {
            $("#jQGridGroupPager").find("select option[value=1000000]").text('All');
            if ($('#jQGridGroup').getGridParam('records') === 0) {
                oldGrid = $('#jQGridGroup tbody').html();
                $('#jQGridGroup tbody').html("<div style='padding:6px;background:#D8D8D8'>No records found</div>");

            }
            else
                oldGrid = "";
            InitUI();
        }

    });
};
   
function ShowMessageDetails(sellerId, messageId) {

    var dialog = new BootstrapDialog({
        title: 'Message Details',
        closeByBackdrop: false,
        closeByKeyboard: false,
        draggable: true,
        message: $('<div id="CustomizeDialog">Please Wait While Loading .............</div>'),
        onshow: function (dialogRef) {
            var Page = $('<div></div>').load('EbayMessages/GetMessagesById?sellerId=' + sellerId + "&messageId=" + messageId, function (responseTxt, statusTxt, xhr) {
                if (statusTxt == 'success') {
                    $('#CustomizeDialog').html(Page);
                }
            })
        },
    });
    dialog.open();

};

function ShowMessageDetailsNewPages(sellerId, messageId, Type) {
    var disable = "Nothing";
    if (Type == true) {
        window.open('EbayMessages/GetEbayMessagesById?sellerId=' + sellerId + '&Disable=' + disable + '&messageId=' + messageId, '_blank'); //GetMessagesById
    } else {
        window.open('Ebay/GetReturnDetails?retId=' + messageId + '&Disable=' + disable + "&selId=" + sellerId, '_blank');
    }
};

jQuery.fn.getSelectedRows = function () {
    var rows = [];
    var self = this;
    var selRows = this.jqGrid('getGridParam', 'selarrrow');
    selRows.forEach(function (item) {
        var rowId = item;
        var rowData = self.jqGrid("getRowData", rowId);
        rows.push(rowData);
    });

    return rows;
};

var GlobalMessage = function(Type , Message){
        $("#divLoading").hide();
        toastr["" + Type + ""](Message, "" + Type + "")
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "progressBar": true,
            "positionClass": "toast-top-right",
            "onclick": null,
            "showDuration": "400",
            "hideDuration": "1000",
            "timeOut": "7000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }
};

var GetFilterDetails = function (url) {
    $("#FilterjQGridGroup").jqGrid({
        url: url,//'@Url.Action("GetFilterList","Filter")',
        datatype: "json",
        //colNames: ['Sender', 'Receiver', 'MessageType', 'Subject', 'Read', 'Attachment', 'Notes', 'Received', 'Actions'],
        colNames: ['FilterID', 'From', 'To', 'Subject', 'HasWord', 'HasNote', 'HasTagName', 'FromDate', 'ToDate', 'Action'],
        hideColumns: ["FilterID"],
        colModel: [
           {
               name: 'MessageID',
               index: 'MessageID',
               hidden: true,
           },
            {
                name: 'From',
                index: 'From',
                width: 15,
                stype: 'text',
                formatter: function (cellvalue, options, rowObject) {
                    if (rowObject.Filter_From != null || rowObject.Filter_From != undefined) {
                        var x = "<div style='padding-top:12px; padding-bottom:12px' class='paddingright'>" + rowObject.Filter_From;
                        x = x + "</div>";
                        return x;
                    } else {
                        return "";
                    }
                },
                sortable: true,
                editable: false,
                searchoptions: { sopt: ['cn', 'eq', 'ne', 'bw'] }
            },
            {
                name: 'To',
                index: 'To',
                width: 15,
                stype: 'text',
                formatter: function (cellvalue, options, rowObject) {
                    if (rowObject.Filter_To != null || rowObject.Filter_To != undefined) {
                        var x = "<div style='padding-top:12px; padding-bottom:12px' class='paddingright'>" + rowObject.Filter_To;
                        x = x + "</div>";
                        return x;
                    } else {
                        return "";
                    }
                },
                sortable: true,
                editable: false,
                searchoptions: { sopt: ['cn', 'eq', 'ne', 'bw'] }
            },
            {
                name: 'Subject',
                index: 'Subject',
                width: 15,
                stype: 'text',
                formatter: function (cellvalue, options, rowObject) {
                    if (rowObject.Filter_Subject != null || rowObject.Filter_Subject != undefined) {
                        var x = "<div style='padding-top:12px; padding-bottom:12px' class='paddingright'>" + rowObject.Filter_Subject;
                        x = x + "</div>";
                        return x;
                    } else {
                        return "";
                    }
                },
                sortable: true,
                editable: false,
                searchoptions: { sopt: ['cn', 'eq', 'ne', 'bw'] }
            }, {
                name: 'HasWord',
                index: 'HasWord',
                width: 15,
                stype: 'text',
                formatter: function (cellvalue, options, rowObject) {
                    if (rowObject.Filter_HasWord != null || rowObject.Filter_HasWord != undefined) {
                        var x = "<div style='padding-top:12px; padding-bottom:12px' class='paddingright'>" + rowObject.Filter_HasWord;
                        x = x + "</div>";
                        return x;
                    } else {
                        return "";
                    }
                },
                sortable: true,
                editable: false,
                searchoptions: { sopt: ['cn', 'eq', 'ne', 'bw'] }
            },
            {
                name: 'HasNote',
                index: 'HasNote',
                width: 10,
                stype: 'text',
                formatter: function (cellvalue, options, rowObject) {
                    if (rowObject.Filter_HasNote != null) {
                        var x = "<div style='padding-top:12px; padding-bottom:12px' class='paddingright'>" + rowObject.Filter_HasNote;
                        x = x + "</div>";
                        return x;
                    }
                },
                sortable: true,
                editable: false,
                searchoptions: { sopt: ['cn', 'eq', 'ne', 'bw'] }
            },
            {
                name: 'HasTagName',
                index: 'HasTagName',
                width: 15,
                stype: 'text',
                formatter: function (cellvalue, options, rowObject) {
                    if (rowObject.Filter_TagName != null || rowObject.Filter_TagName != undefined) {
                        var x = "<div style='padding-top:12px; padding-bottom:12px' class='paddingright'>" + rowObject.Filter_TagName;
                        x = x + "</div>";
                        return x;
                    } else {
                        return "";
                    }
                },
                sortable: true,
                editable: false,
                searchoptions: { sopt: ['cn', 'eq', 'ne', 'bw'] }
            },
            {
                name: 'FromDate',
                index: 'FromDate',
                width: 15,
                stype: 'text',
                formatter: function (cellvalue, options, rowObject) {
                    if (rowObject.Filter_FromDate != null || rowObject.Filter_FromDate != undefined) {
                        var x = "<div style='padding-top:12px; padding-bottom:12px' class='paddingright'>" + rowObject.Filter_FromDate;
                        x = x + "</div>";
                        return x;
                    } else {
                        return "";
                    }
                },
                sortable: true,
                editable: false,
                searchoptions: { sopt: ['cn', 'eq', 'ne', 'bw'] }
            },
            {
                name: 'ToDate',
                index: 'ToDate',
                width: 15,
                stype: 'text',
                formatter: function (cellvalue, options, rowObject) {
                    if (rowObject.Filter_ToDate != null || rowObject.Filter_ToDate != undefined) {
                        var x = "<div style='padding-top:12px; padding-bottom:12px' class='paddingright'>" + rowObject.Filter_ToDate;
                        x = x + "</div>";
                        return x;
                    } else {
                        return "";
                    }
                },
                sortable: true,
                editable: false,
                searchoptions: { sopt: ['cn', 'eq', 'ne', 'bw'] }
            },
            {
                name: 'Action',
                index: 'Action',
                width: 10,
                stype: 'text',
                formatter: function (cellvalue, options, rowObject) {
                    var x = "<div style='padding-top:12px; padding-bottom:12px;text-align:center;'>" +
                            "<a style='cursor: pointer;color:#1ab394;' class='pull-right' onclick='DeleteFilter(" + rowObject.FilterID + "," + options.rowId + ");'><i class='fa fa-trash fa-lg fa-fw' title='Delete Details'></i></a><a style='cursor: pointer;color:#1ab394;' onclick='ShowFilterDetails(" + rowObject.FilterID + ");'><i class='fa fa-pencil-square-o fa-lg fa-fw' title='View Actions'></i></a>" +
                        "</div>";

                    return x;
                },
                sortable: false,
                search: false,
                editable: false,

            }
        ],
        // multiselect: true,
        rowNum: 10,
        rowList: [10, 50, 100, 150, 200, 1000000],
        mtype: 'GET',
        pager: '#FilterjQGridGroupPager',
        // loadonce: true,
        scroll: false,
        viewrecords: true,
        // async: true,
        height: 350,
        // autowidth: true,
        width: 1125,
        shrinkToFit: true,
        ignorecase: true,
        loadComplete: function () {
            var width = $('#FilterjqGrid_container').width();
            $('#FilterjQGridGroup').setGridWidth(width);
            //    $("#FilterjQGridGroup").jqGrid('setGridWidth', $(FilterjqGrid_container).width(), true);
            $("#FilterjQGridGroupPager").find("select option[value=1000000]").text('All');
            if ($('#FilterjQGridGroup').getGridParam('records') === 0) {
                oldGrid = $('#FilterjQGridGroup tbody').html();
                $('#FilterjQGridGroup tbody').html("<div style='padding:6px;background:#D8D8D8'>No records found</div>");

            }
            else
                oldGrid = "";
            InitUI();
        }

    });
}

