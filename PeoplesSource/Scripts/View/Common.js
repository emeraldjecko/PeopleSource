var POPUPTOP = 200;

function DownloadFile(url) {

    var iframe = document.getElementById("hiddenDownloader");

    if (iframe === null) {
        iframe = document.createElement("iframe");
        iframe.id = "hiddenDownloader";
        iframe.style.display = "none";
        document.body.appendChild(iframe);
    }

    iframe.src = url;
}

function PostToDialog(containerId, postUrl, postData) {
    PostToDialog(containerId, postUrl, postData, 'GET');
}

function PostToDialog(containerId, postUrl, postData, postType) {

    var dialogContainer = $(containerId);

    dialogContainer.html(GetLoadingHtml());

    $.ajax({
        url: postUrl,
        type: postType,
        dataType: 'html',
        data: postData,
        cache: false,
        success: function (data) {
            dialogContainer.html(data);
        }
    });
    //dialogContainer.dialog({
    //    position: ['top', POPUPTOP]
    //});
    //dialogContainer.dialog('option', 'top', '0');
    dialogContainer.dialog('open');
    return false;
}

function CloseDialog(containerId) {

    var container = $(containerId);

    container.children().remove();

    container.dialog('close');
}

function ClearDialog(containerId) {

    var container = $(containerId);

    container.children().remove();
}

var currentPost;
var queueList = new Array();
function PostToContainerSyncQueue(queueName, containerId, postUrl, postData, postType) {
    var container = $(containerId);

    container.html(GetLoadingHtml());

    $.ajaxq(queueName, {
        url: postUrl,
        type: postType,
        dataType: 'html',
        data: postData,
        cache: false,
        success: function (data) {
            container.html(data);
        }
    });
}

function PostToContainer(containerId, postUrl, postData, callbackFunction) {
    PostToContainer(containerId, postUrl, postData, 'GET', callbackFunction);
}

function PostToContainer(containerId, postUrl, postData, postType, callbackFunction) {
    var container = $(containerId);
    container.html(GetLoadingHtml());
    $.ajax({
        url: postUrl,
        type: postType,
        dataType: 'html',
        data: postData,
        cache: false,
        success: function (data) {
            container.html(data);
            if ((callbackFunction != undefined) && (typeof callbackFunction == 'function'))
                callbackFunction();
        }
    });
}

function PostToContainerWithoutLoader(containerId, postUrl, postData) {
    PostToContainerWithoutLoader(containerId, postUrl, postData, 'GET');
}

function PostToContainerWithoutLoader(containerId, postUrl, postData, postType) {

    var container = $(containerId);

    $.ajax({
        url: postUrl,
        type: postType,
        dataType: 'html',
        data: postData,
        cache: false,
        success: function (data) {
            container.html(data);
        }
    });
}

function GetLoadingHtml() {
    return '<div class="loading"><img src="/content/images/wait.gif" alt="loading"></div>';
}

jQuery.subscribe = function (eventName, fn) {
    $('body').on(eventName, function (event, param1, param2, param3, param4, param5, param6) {
        fn(event, param1, param2, param3, param4, param5, param6);
    });

    return jQuery;
}

jQuery.unsubscribe = function (eventName) {
    $('body').die(eventName);
    return jQuery;
}

jQuery.publish = function (eventName, params) {
    $('body').trigger(eventName, params);
    return jQuery;
}

function SelectMenu(id) {
    $(id).addClass('active');
}

function ChangeDialogTitle(dialog, title) {
    var dialogControl = $(dialog);
    dialogControl.dialog('option', 'title', title);
}

function NavigateTo(link) {
    window.location.href = link;
}

function AddMinutes(minutes) {
    var currentTime;
    currentTime = new Date();
    currentTime.setTime(currentTime.getTime() + (1000 * 60 * minutes));
    return currentTime;
}

function InitStickyTabs($tabs, currentUrl) {
    var cookieName;
    cookieName = 'StickyTab';
    $tabs.tabs({
        select: function (e, ui) {
            $.cookies.set(cookieName, ui.index, {
                expiresAt: AddMinutes(5),
                path: currentUrl
            });
        }
    });
    stickyTab = $.cookies.get(cookieName);
    if (!isNaN(stickyTab)) {
        $tabs.tabs('select', stickyTab);
    }
}

function InitUI() {
    $('button.back').button({
        icons: {
            primary: "ui-icon-circle-arrow-w"
        },
        text: false
    });

    $('button.delete').button({
        icons: {
            primary: "ui-icon-closethick"
        },
        text: false
    });

    $('button.pencil').button({
        icons: {
            primary: "ui-icon-pencil"
        },
        text: false
    });

    $('button.plus').button({
        icons: {
            primary: "ui-icon-plus"
        },
        text: false
    });

    $('button.copy').button({
        icons: {
            primary: "ui-icon-copy"
        },
        text: false
    });


    $('input:submit, input:button').button();
    $('button.edit').button();
    //$('input[type=submit]').removeAttr('disabled');

    $('form').each(function () {
        $(this).submit(function () {
            if ($(this).validate().form()) {
                $('input[type=submit]', this).attr('disabled', 'disabled');
            }
        });
    });
}

function LockButtons() {
    $('button.edit').button({
        icons: {
            primary: "ui-icon-locked"
        },
        text: true
    });

    $('input:submit, input:button').attr('disabled', 'disabled');
    $('button.delete, button.edit').attr('disabled', 'disabled');
}

function UnLockButtons() {
    $('button.edit').button({
        icons: {
            primary: ""
        },
        text: true
    });

    $('input:submit, input:button').removeAttr('disabled');
    $('button.delete, button.edit').removeAttr('disabled');
}

function EnableInputButtons() {
    $('input[type=submit]').removeAttr('disabled');
}

function HideValidation() {
    $('#ValidationSummary').hide();
}

function ShowValidation(message, parentContainerSelector) {
    EnableInputButtons();
    var validationSummary = null;
    if (parentContainerSelector != null) {
        validationSummary = parentContainerSelector.find('#ValidationSummary');
    }
    else {
        validationSummary = $('#ValidationSummary');
    }
    validationSummary.html(message);
    validationSummary.show();
}

function HandleSaveResponse(data) {
    if (data.Success) {
        HideValidation();
        EnableInputButtons();
        RefreshMessages();
    }
    else {
        ShowValidation(data.Message);
    }
}

function HandleSaveResponseWithEventKey(data, publishingKey, parentContainerSelector) {
    if (data.Success) {

        HideValidation();
        EnableInputButtons();
        $.publish(publishingKey);
        RefreshMessages();
    }
    else {
        ShowValidation(data.Message, parentContainerSelector);
    }
}

function HandleSaveResponseWithRedirect(data, parentContainerSelector) {   
    if (data.Success) {
        NavigateTo(data.Message);
    }
    else {
        ShowValidation(data.Message, parentContainerSelector);
    }
}

function HandleDelete(data, publishingKey) {
    if (data) {
        $.publish(publishingKey);
    }
    else {
        EnableInputButtons();
    }
    RefreshMessages();
}

function HandleRedirectResponse(data, url) {
    if (data) {
        NavigateTo(url);
    }
    else {
        EnableInputButtons();
        RefreshMessages();
    }
}

function InitMessages(messageContainer, messageUrl) {

    $(messageContainer).children().each(function (index) {

        var messageType = $(this).attr('class');
        var messageText = $(this).html();
        FireJGrowl(messageText, messageType);

        $(this).hide();
    });

}

function FireJGrowl(messageText, messageType) {    
    $.jGrowl(messageText, { theme: messageType, life: 6000, position: 'top-right' });
}

function RefreshMessages() {
    $.ajax({
        url: '/Message/Messages',
        type: 'GET',
        dataType: 'json',
        cache: false,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var message = data[i];
                FireJGrowl(message.Text, message.TypeText);
            }
        }
    });
}

$.fn.checkDirty = function (e, url) {
    var isDirty = false;

    $(this).find(':input').each(function () {
        if ($(this).serialize() != $(this).data('formValues')) {
            isDirty = true;
        }
    });

    if (isDirty) {
        if (confirm('Are you sure you want to cancel ?')) {
            //removeLocalStorageForms(this);
            NavigateTo(url);
        } else {
            e.preventDefault();
        }
    }
    else {
        //removeLocalStorageForms(this);
        NavigateTo(url);
    }
    return isDirty;
}

$.fn.checkIsDirty = function (e, publishingKey) {

    var isDirty = false;
    $(this).find(':input[type=text],input[type=checkbox], input[type=radio],canvas,select,textarea').each(function () {
        if ($(this).serialize() != $(this).data('formValues')) {
            isDirty = true;
        }
    });

    if (isDirty == true) {
        if (confirm('Are you sure you want to cancel ?')) {
            //removeLocalStorageForms(this, sisyphus);
            //window.localStorage.clear();
            $.publish(publishingKey);
        } else {
            e.preventDefault();
        }
    } else {
        //removeLocalStorageForms(this, sisyphus);
        //window.localStorage.clear();
        $.publish(publishingKey);
    }
    return isDirty;
}

$.fn.checkFormDirty = function (e, publishingKey) {
    var isDirty = false;
    $(this).find(':input').each(function () {
        if ($(this).serialize() != $(this).data('formValues')) {
            isDirty = true;
        }
    });

    if (isDirty == true) {
        if (confirm('Are you sure you want to cancel ?')) {
            $.publish(publishingKey);
        } else {
            e.preventDefault();
        }
    }
    else {

        $.publish(publishingKey);
    }
    return isDirty;
}

//function removeLocalStorageForms(form, sisyphus) {
//    // check to see if the form has values saved
//    $('input,textarea,select', form).each(function (i, input) {

//        if (sisyphus.browserStorage.get(window.location.pathname + $(form).attr('id') + $(input).attr('name')) != null) {
//            // removes all stored data for the form
//            sisyphus.browserStorage.remove(window.location.pathname + $(form).attr('id') + $(input).attr('name'));
//        }
//    });
//}


$.fn.fix_radios = function () {
    function focus() {
        if (!this.checked) return;
        if (!this.was_checked) {
            $(this).change();
        }
    }

    function change(e) {
        if (this.was_checked) {
            e.stopImmediatePropagation();
            return;
        }
        $("input[name=" + this.name + "]").each(function () {
            this.was_checked = this.checked;
        });
    }
    return this.focus(focus).change(change);
}


