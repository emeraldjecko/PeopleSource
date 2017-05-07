
//For Messge Popup

$("#divLoading").hide();
//FireJGrowl("Proxy is invalid", "error");
toastr["error"]("Please Enter Both From Date and To Date..", "error")  //Success For Success
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




// Popup Html

<div style="display: none;" class="modal inmodal" id="myModal" tabindex="-1" role="dialog" aria-hidden="false">
    <div class="modal-dialog" style="width: 278px;">
        <div class="modal-content animated bounceInRight">
            <div class="modal-header" style="padding: 4px 5px;">
                <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>

                <h4 class="modal-title">Tags</h4>
                <small class="font-bold">You can select multiple tags to assign.</small>
            </div>
            <div class="modal-body">
                <div class="row">
                    <div class="col-lg-12">
                        <div id="AssignTagList" style="text-align: center;">
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-primary" id="Savebtn" onclick="SaveAssignTag();return false;">Save</button>
                <button type="button" class="btn btn-white" data-dismiss="modal">Close</button>

            </div>
        </div>
    </div>
</div>


 $("#myModal").modal('show');
 $("#myModal").modal('hide');




// DateTime Picker

 $("#FromDatepkr").click(function () {
        $(this).datetimepicker({
            controlType: 'select',
            format: 'd/m/Y',
            timepicker: false
        }).datetimepicker("show")
    });





//From Date to To Date

 jQuery(function () {
        jQuery('#FromDatepkr').datetimepicker({
            format: 'Y/m/d',
            onShow: function (ct) {
                this.setOptions({
                    maxDate: jQuery('#ToDatepkr').val() ? jQuery('#ToDatepkr').val() : false
                })
            },
            timepicker: false
        });
        jQuery('#ToDatepkr').datetimepicker({
            format: 'Y/m/d',
            onShow: function (ct) {
                this.setOptions({
                    minDate: jQuery('#FromDatepkr').val() ? jQuery('#FromDatepkr').val() : false
                })
            },
            timepicker: false
        });
    });





//Delete Popup Message
   
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
                $.post('@Url.Action("DeleteTag", "MessageOverview")', { ID: ID },
                    function (data) {
                        $('#TagList').find('#Tags_' + ID).remove();
                        $("#divLoading").hide();
                        $.each(data.ListTagArray, function (e, element) {
                            $('#jQGridGroup').find('#Div_MessageID_' + element.MessageID + '').find('#LTags_' + element.MessageID + '_' + element.TagsID + '').remove();
                        });
                        //FireJGrowl("Proxy is valid", "success");
                        toastr["success"]("Tag has been deleted successfully", "")
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




//Ckeditor

    <script src="~/Content/Theme/ckeditor/ckeditor.js"></script>
    if (CKEDITOR.instances['MessageContent']) {   //For Reset
        CKEDITOR.instances['MessageContent'].destroy();
    }

    CKEDITOR.replace('MessageContent', {  // Set to null
        toolbar: [{ items: ['Bold', 'Italic', 'Underline'] }]
    }).setData(' ');

    CKEDITOR.instances.MessageContent.getData();  //Get Data of Ckeditor

    CKEDITOR.replace('MessageContent', {  //Set Data When Edited
        toolbar: [{ items: ['Bold', 'Italic', 'Underline'] }]
    }).setData(data.ItemtagData.Message);




//DropDown Default Functionality
    $('#SellerList option:eq(0)').prop('selected', true);  //for First position
    $('#SellerList :selected').val();  //For value
    $('#SellerList :selected').text();  //For Text




//Tag Input
    <link href="~/Content/Theme/css/plugins/TagInput/bootstrap-tagsinput.css" rel="stylesheet" />
    <script src="~/Content/Theme/js/TagInput/bootstrap-tagsinput.min.js"></script>

    $('#Tags').tagsinput('removeAll');  //Remove All TagInput

    var Tags = [];
    $.each($('#TagLists').find('.bootstrap-tagsinput').find('.tag'), function (e, element) {
        Tags.push($(element).find('.TagText').text());

    });  //Save Tag

    var TagList = data.ItemtagData.ItemTags.split(',');
    $.each(TagList, function (e, element) {
        $('#Tags').tagsinput('add', element);
    });   //Dyanmically Add Tag to input Box





//Add Dynamic Row When Save in jqgrid

    jQuery.fn.addRows = function (rows) { // Will not work for treegrid
        var self = this;

        var newData = [];
        var allRows = this.jqGrid('getGridParam', 'data');
     
        $.each(rows, function () {
            newData.push(this);
        });

        $.each(allRows, function () {
            newData.push(this);
        });

        this.jqGrid('clearGridData')
            .jqGrid('setGridParam', { data: newData })
            .trigger('reloadGrid');
    }




//Jqgrid window Size

    $(window).bind('resize', function () {
        var width = $('#jqGrid_container').width();
        $('#jQGridGroup').setGridWidth(width);
    });



//jqgrid Display Functionality

    @*@model PeoplesSource.Ebay.Models.Message
    @using Elmah.ContentSyndication
    @using PeoplesSource.Helpers*@
    @{
        ViewBag.Title = "Messages List";
        Layout = null;
        // var pageInfo = ViewData.Get<PageInfo>();
    }

    <div id="jqGrid_container" class="jqGrid">
        <table id="jQGridGroup"></table>
        <div id="jQGridGroupPager"></div>
    </div>

    <div id="listComment"></div>

    <style>
    .paddingright {
        padding-left: 10px !important;
    } 
    </style>

<script type="text/javascript">


    $("#divLoading").show();
    //var mainGridPrefix = "s_";

    $(window).bind('resize', function () {
        var width = $('#jqGrid_container').width();
        $('#jQGridGroup').setGridWidth(width);
    });


    $("#jQGridGroup").jqGrid({
        url: '@Url.Action("GetItemTagDetails","ItemTag")',
        datatype: "json",
        colNames: ['ItemTagID', 'ItemTagsList', 'Sellerid', 'SellerName', 'Tags', 'Message', 'Actions'],
        hideColumns: ["ItemTagID", "ItemTagsList", "Sellerid"],
        colModel: [
                
           {
               name: 'ItemTagID',
               index: 'ItemTagID',
               hidden: true,
           }, {
               name: 'ItemTagsList',
               index: 'ItemTagsList',
               hidden: true,
           }, {
               name: 'Sellerid',
               index: 'Sellerid',
               hidden: true,
           },
             {
                 name: 'SellerName',
                 index: 'SellerName',
                 width: 10,
                 stype: 'text',
                 formatter: function (cellvalue, options, rowObject) {
                     if (rowObject.SellerName != null) {
                         var x = "<div class='paddingright SellerName'>" + rowObject.SellerName;
                         x = x + "</div>";
                         return x;
                     }
                 },
                 sortable: true,
                 editable: false,
                 searchoptions: { sopt: ['cn', 'eq', 'ne', 'bw'] }
             },{
                 name: 'Tags',
                 index: 'Tags',
                 width: 15,
                 stype: 'text',
                 formatter: function (cellvalue, options, rowObject) {
                     if (rowObject.ItemTagsList != null) {
                         var x = "<div class='paddingright TagList' style='padding:10px;'>";
                         var Taglist = rowObject.ItemTagsList.split(',');
                         $.each(Taglist, function (e, element) {
                             x = x + ('<span style="margin-left:10px;" class="tag label label-info"><i class="fa fa-tag"></i><span style="padding:5px;" class="TagText">' + element + '</span></span>');
                         });
                         x = x + '</div>';
                         return x;
                     }
                 },
                 sortable: true,
                 editable: false,
                 searchoptions: { sopt: ['cn', 'eq', 'ne', 'bw'] }
             },
             {
                 name: 'Message',
                 index: 'Message',
                 width: 10,
                 stype: 'text',
                 formatter: function (cellvalue, options, rowObject) {
                     if (rowObject.SellerName != null) {
                         var x = "<div class='paddingright Message'>" + rowObject.Message;
                         x = x + "</div>";
                         return x;
                     }
                 },
                 sortable: true,
                 editable: false,
                 searchoptions: { sopt: ['cn', 'eq', 'ne', 'bw'] }
             },
            {
                name: 'Actions',
                index: 'Actions',
                width: 5,
                stype: 'text',
                formatter: function (cellvalue, options, rowObject) {
                    var x = "<div style='text-align:center;'>" +
                                      "<a style='cursor: pointer;color:#1ab394;' onclick='EditItemTag(" + rowObject.ItemTagID + "," + rowObject.Sellerid + ");'><i class='fa fa-pencil-square-o fa-lg fa-fw' title='Edit Details'></i></a><a style='cursor: pointer;color:#1ab394;'  onclick='DeleteItemTag(" + rowObject.ItemTagID + ");'><i class='fa fa-trash fa-lg fa-fw' title='Delete Details'></i></a>" +
                                   "</div>";
                            
                    return x;
                },
                sortable: false,
                search: false,
                editable: false,

            }
        ],
        rowNum: 10,
        rowList: [10, 50, 100, 150,200, 1000000],
        mtype: 'GET',   
        pager: '#jQGridGroupPager',
        scroll: false,
        viewrecords: true,
        async:true,
        height: 500,
        autowidth: true,
        shrinkToFit: true,
        ignorecase: true,
        loadComplete: function () {

            $("#jQGridGroupPager").find("select option[value=1000000]").text('All');
            if ($('#jQGridGroup').getGridParam('records') === 0) {
                oldGrid = $('#jQGridGroup tbody').html();
                $('#jQGridGroup tbody').html("<div id='NoRecord' style='padding:6px;background:#D8D8D8'>No records found</div>");
                 
            }

            else
                oldGrid = "";
            InitUI();
        }

    });        


    $(function () {
        $('#side-menu').find('li').removeClass('active');
        $('#side-menu').find('#MessageSchedule').addClass('active');
    });
    </script>


//Call Webmethod of MVC

    $.ajax({
        type: "POST",
        url: '@Url.Action("SaveTags", "ItemTag")',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ TagList: Tags.join(), Sellerid: Sellerid, Message: Message }),
        dataType: "json",
        async: true,
        success: function (data) {




//Get Partial Info Means Grid In MVC
            @{
                ViewBag.Title = "Set Thank You Messages";
                Layout = "~/Views/Shared/_Layout.cshtml";
            }

            <style>
            
            </style>
            
            
            <div class="row" style="margin-top: 10px;" id="divseller">
            <div class="col-lg-12" style="margin-bottom: 25px; width: 100%;">
                <div class="ibox float-e-margins">
                    <div class="ibox-title">
                        <h5>List Of  ThankYou Messages</h5>
                        <div class="ibox-tools">
                            <button class="btn btn-xs btn-primary" type="button" onclick="AssignTag();return false;">
                                Assign Tag
                   
                            </button>
                   
                        </div>
                    </div>
                    <div class="ibox-content" style="margin-bottom: 20px;">
                        <div class="row">
                            <div id="SellerListContainer">
                                @Html.Partial("GetItemTagDetails")
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>





//KeyPress Enter Key Event
   
      $("#txtSearch").keypress(function (e) {
          if (e.which == 13) {
              e.preventDefault();
              Search();
          }

      });