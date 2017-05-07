
var MessageReply = function () {
    if (CKEDITOR.instances['MessageContent']) {
        CKEDITOR.instances['MessageContent'].destroy();
        CKEDITOR.replace('MessageContent', {
            toolbar: [{ name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Cut', 'Copy', 'Paste', 'PasteText', 'Undo', 'Redo'] }, { name: 'colorstyle', items: ['Font', 'FontSize', 'TextColor', 'BGColor']},{ name: 'list' , items: ['NumberedList', 'BulletedList'] }, { name: 'external', items: ['Smiley'] }]
        }).setData(' ');
    } else {
        CKEDITOR.replace('MessageContent', {
            toolbar: [{ name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Cut', 'Copy', 'Paste', 'PasteText', 'Undo', 'Redo'] }, { name: 'colorstyle', items: ['Font', 'FontSize', 'TextColor', 'BGColor'] }, { name: 'list', items: ['NumberedList', 'BulletedList'] }, { name: 'external', items: ['Smiley'] }]
        }).setData(' ');
    }
    CKEDITOR.config.toolbarLocation = 'bottom';
    $('#ReplyPopup').modal('show');
}

var AddNotes = function (MessageId) {
    $('#Notes_Popup').find('#NoteContent').val('');
    $.ajax({
        type: "POST",
        url: '../MessageOverview/ShowNote',
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
    $("#Tag_Popup").modal('hide');
};

var SaveNotes = function (MsgId) {
    var Note = $('#Notes_Popup').find('#NoteContent').val();
    if (Note.length != 0) {
        $.ajax({
            type: "POST",
            url: '../MessageOverview/SaveNote',
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

var GlobalMessage = function (Type, Message) {
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

var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
};

var EvaluteData = function (MsgId, Type) {
    $.ajax({
        type: "POST",
        url: '../Filter/GetLinkData?MsgId=' + MsgId + '&Type=' + Type,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            if (data.success == true) {
                if (data.Data != null) {
                    $('#Previous').removeAttr('disabled');
                    $('#Next').removeAttr('disabled');
                    var disable = "Nothing";
                    if (data.count == 1) {
                        if (Type == "Next") {
                            disable = "Next";
                        } else {
                            disable = "Prev";
                        }
                    }
                    if (data.Data.Type == true) {
                        window.open('../EbayMessages/GetEbayMessagesById?sellerId=' + data.Data.SellerID + '&messageId=' + data.Data.MessageID + '&Disable=' + disable, '_self'); //GetMessagesById
                    } else {
                        window.open('../Ebay/GetReturnDetails?retId=' + data.Data.MessageID + "&selId=" + data.Data.SellerID + '&Disable=' + disable, '_self');
                    }
                }
            }
        },
    });
}

var Tagging = function (MsgID) {
    $.ajax({
        type: "GET",
        url: '../EbayMessages/GetDetailTag?MessageId='+MsgID+'',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.success == true) {
                 $("#Txt_Tags").tagsinput('removeAll');
                if (data.TagList != null || data.TagList != undefined) {
                    $('#Txt_Tags').tagsinput("add", data.TagList);
                } else {
                    $("#Txt_Tags").tagsinput();
                }
                $('#Tag_Popup').find('#Save_Tag').attr('onclick', 'SaveTags(' + MsgID + ')');
                $('#Tag_Popup').modal('show');
            } else {
                $("#divLoading").hide();
                GlobalMessage('error', 'InValid Selection');
            }
        }
    });
}

var SaveTags = function (MsgID) {
    var TagString = $('#Txt_Tags').val();
    if (TagString == null || TagString == undefined || TagString.length == 0) {
        GlobalMessage('error', 'Please enter the tag first');
    } else {
        $.ajax({
            type: "POST",
            url: '../EbayMessages/SaveDetailTag?MessageId=' + MsgID + '&TagList=' + TagString,
            contentType: "application/json; charset=utf-8",
            //        data: JSON.stringify({ MessageId : MsgID, TagList: TagStrings }),
            dataType: "json",
            success: function (data) {
                if (data.success == true) {
                    var Tags = data.TagList.split(',');
                    $('#DetailTagList').html('');
                    $.each(Tags, function (e, element) {
                        $('#DetailTagList').append('<a class="btn btn-xs btn-primary ATags" style="margin-left:5px;margin-top:10px;"><i style="margin-right:5px;" class="fa fa-tag"></i>' + element + '</a>');
                    });
                    $('#Tag_Popup').modal('hide');
                    GlobalMessage('success', 'Tags has been assign successfully..');
                } else {
                    $("#divLoading").hide();
                    GlobalMessage('error', 'InValid Selection');
                }
            }
        });
    }
}


var DeleteMessage = function (MsgID)
{
    $("#divLoading").show();
    $("#divLoading").hide();
    BootstrapDialog.show({

        message: 'Are you sure to delete this Message?',
        title: 'Delete Confirmation',
        closeByBackdrop: false,
        closeByKeyboard: false,
        draggable: true,
        buttons: [{
            label: 'Yes',
            cssClass: 'btn-primary',
            action: function (e) {
                $.post('DeleteEbayMessage?MessageId=' + MsgID,
                    function (data) {
                        if (data.success == true) {
                            var disable = "Nothing";
                            if (data.NextPagedDetail.Type == true) {
                                window.open('../EbayMessages/GetEbayMessagesById?sellerId=' + data.NextPagedDetail.Sellerid + '&messageId=' + data.NextPagedDetail.MasterMessageid + '&Disable=' + data.Button, '_self'); //GetMessagesById
                            } else {
                                window.open('../Ebay/GetReturnDetails?retId=' + data.NextPagedDetail.MasterMessageid + "&selId=" + data.NextPagedDetail.Sellerid + '&Disable=' + data.Button, '_self');
                            }
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
}

var AssignLabel = function (MsgID) {
    $.ajax({
        type: "GET",
        url: 'GetTagsDetails?MsgID=' + MsgID,
        contentType: "application/json; charset=utf-8",
        //        data: JSON.stringify({ MessageId : MsgID, TagList: TagStrings }),
        dataType: "json",
        success: function (data) {
            if (data.success == true) {
                $('#AssignTag_PopUp').find('#AssignTagList').empty();
                if (data.TagList != null) {
                    $.each(data.TagList, function (e, element) {
                        if (element.Existing == "True") {
                            $('#AssignTag_PopUp').find('#AssignTagList').append('<div style="margin-bottom:5px;" class="col-lg-12 chcks"><label class="checkbox-inline i-checks pull-left"><div class="icheckbox_square-green checked" id ="checked_' + element.TagID + '" onclick="checked(' + element.TagID + ');return false;" style="position: relative;"><span class="Tag_class hidden">' + element.TagID + '</span><input type="checkbox" value="option1" style="position: absolute; opacity: 0;"><ins class="iCheck-helper"></ins></div><span class="AssTags" style="font-weight: bold;font-size:14px;padding:5px;">' + element.TagName + '</span></label></div>');
                        } else {
                                $('#AssignTag_PopUp').find('#AssignTagList').append('<div style="margin-bottom:5px;" class="col-lg-12 chcks"><label class="checkbox-inline i-checks pull-left"><div class="icheckbox_square-green" id ="checked_' + element.TagID + '" onclick="checked(' + element.TagID + ');return false;" style="position: relative;"><span class="Tag_class hidden">' + element.TagID + '</span><input type="checkbox" value="option1" style="position: absolute; opacity: 0;"><ins class="iCheck-helper"></ins></div><span class="AssTags" style="font-weight: bold;font-size:14px;padding:5px;">' + element.TagName + '</span></label></div>');
                        }
                    });
                    $('#AssignTag_PopUp').find('#Savebtn').attr('onclick', 'SaveAssignTag(' + MsgID + ')').removeClass('hidden');
                } else {
                    $('#AssignTag_PopUp').find('#Savebtn').addClass('hidden');
                    $('#AssignTag_PopUp').find('#AssignTagList').html('<span style="color:red;font-size:16px;margin-top:20px;margin-bottom:20px;">No Tag Found....</span>');
                }
                $('#AssignTag_PopUp').modal('show');
            } else {
                $("#divLoading").hide();
                GlobalMessage('error', 'InValid Selection');
            }
        }
    });
  
}

var SaveAssignTag = function (MsgID) {
    var CheckedValues = [];
    $.each($('#AssignTag_PopUp').find('#AssignTagList').find('.chcks'), function (e, element) {
        if ($(element).find('div').hasClass('checked')) {
            CheckedValues.push($(element).find('.Tag_class').text());
        }
    });
    if (CheckedValues.length == 0) {
        $("#divLoading").hide();
        GlobalMessage('error', 'Please Select Atleast 1 Tag');
    } else {
        $.ajax({
            type: "POST",
            url: 'AssignSystemTags',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ MsgID : MsgID, CheckedTagList: CheckedValues.toString() }),
            dataType: "json",
            success: function (data) {
                $("#AssignTag_PopUp").modal('hide');
                if (data.success == true) {
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

var checked = function (value) {
    if ($('#AssignTag_PopUp').find('#AssignTagList').find('#checked_' + value + '').hasClass('checked')) {
        $('#AssignTag_PopUp').find('#AssignTagList').find('#checked_' + value + '').removeClass('checked');
    } else {
        $('#AssignTag_PopUp').find('#AssignTagList').find('#checked_' + value + '').addClass('checked');
    }
};
