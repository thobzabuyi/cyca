

    $(document).ready(function () {
        var child = @ViewBag.childId;
        $(".datepicker").datepicker({
            dateFormat: "dd M yy",
            minDate: new Date(),
            changeYear: true,
            onClose: function () {
                $(this).valid();
            }
        });
        $("#AddBodilySearch").hide();
        $("#EditBodilySearch").hide();
        $("#ViewBodilySearch").hide();
        $("#AddIllegalItem").hide();
        $("#EditIllegalItem").hide();
        $("#ViewIllegalItem").hide();


    });
    function showNew() {
        $("#BodilySearchRecords").hide();
        $("#IllegalItemsRecords").hide();
        $("#EditBodilySearch").hide();
        $("#ViewBodilySearch").hide();
        $("#AddIllegalItem").hide();
        $("#EditIllegalItem").hide();
        $("#ViewIllegalItem").hide();
        $("#AddBodilySearch").show();

    }
    function showNewIllegalItem() {
        $("#BodilySearchRecords").hide();
        $("#IllegalItemsRecords").hide();
        $("#EditBodilySearch").hide();
        $("#ViewBodilySearch").hide();
        $("#AddBodilySearch").hide();
        $("#EditIllegalItem").hide();
        $("#ViewIllegalItem").hide();
        $("#AddIllegalItem").show();

    }
    $(function () {
        $("#myForm").validate();
        $("#myForm3").validate();
        $("#myForm4").validate();
        
    });
    function ShowHideDiv() {
        //Body Search Add
        
        var Document_Type_Id = document.getElementById("Document_Type_Id");           
        var dvOther = document.getElementById("dvOther");
        dvOther.style.display = Document_Type_Id.value == "4" ? "block" : "none";

        var SearchReasonId = document.getElementById("SearchReasonId");
        var dvOtherReason = document.getElementById("dvOtherReason");
        dvOtherReason.style.display = SearchReasonId.value == "5" ? "block" : "none";



        //Body Search Edit
        var Document_Type_Id1 = document.getElementById("Document_Type_Id1");
        var dvOtherEdit = document.getElementById("dvOtherEdit");
        dvOtherEdit.style.display = Document_Type_Id1.value == "4" ? "block" : "none";

        var Search_Reason_Id = document.getElementById("Search_Reason_Id");
        var dvOtherReasonEdit = document.getElementById("dvOtherReasonEdit");
        dvOtherReasonEdit.style.display = Search_Reason_Id.value == "5" ? "block" : "none";


        //Illegal Item
        var DocType_Id = document.getElementById("DocType_Id");
        var dvOtherIllegal = document.getElementById("dvOtherIllegal");
        dvOtherIllegal.style.display = DocType_Id.value == "4" ? "block" : "none";
        //chkatchtbl();                              
    }

    var formdata = new FormData(); //FormData object
    //Body Search
    $("#fileInput").on("change", function () {
        var fileInput = document.getElementById('fileInput');
        //Iterating through each files selected in fileInput
        for (i = 0; i < fileInput.files.length; i++) {
            var sfilename = fileInput.files[i].name;
            let srandomid = Math.random().toString(36).substring(7);
            formdata.append(sfilename, fileInput.files[i]);
            
            var markup = "<tr id='" + srandomid + "'><td>" + sfilename + "</td><td><a href='#' onclick='DeleteFile(\"" + srandomid + "\",\"" + sfilename +
                         "\")'><span class='red'>Remove file</span></a></td></tr>"; // Binding the file name
            $("#FilesList tbody").append(markup);
        }
        chkatchtbl();
        $('#fileInput').val('');
    });

    //Illegal Items
    $("#fileInputIllegalAdd").on("change", function () {
        var fileInputIllegalAdd = document.getElementById('fileInputIllegalAdd');
        //Iterating through each files selected in fileInput
        for (i = 0; i < fileInputIllegalAdd.files.length; i++) {
            var sfilename = fileInputIllegalAdd.files[i].name;
            let srandomid = Math.random().toString(36).substring(7);
            formdata.append(sfilename, fileInputIllegalAdd.files[i]);

            var markup = "<tr id='" + srandomid + "'><td>" + sfilename + "</td><td><a href='#' onclick='DeleteFile(\"" + srandomid + "\",\"" + sfilename +
                "\")'><span class='red'>Remove file</span></a></td></tr>"; // Binding the file name
            $("#FilesListIllegalAdd tbody").append(markup);
        }
        chkatchtbl();
        $('#fileInputIllegalAdd').val('');
    });

    function DeleteFile(Fileid, FileName) {
        formdata.delete(FileName)
        $("#" + Fileid).remove();
        chkatchtbl();
    }
    function chkatchtbl() {
        //Body Search
        if (document.getElementById("Document_Type_Id").value >=1) {
            
            $("#fileInput").removeAttr("disabled")
            if ($('#FilesList tr').length > 1) {
                $("#fileInput").removeAttr("required")
                $("#FilesList").css("visibility", "visible");
            } else {
                $("#fileInput").attr("required", "required")
                $("#FilesList").css("visibility", "hidden");
            }
        } else {
            $("#fileInput").attr("disabled", "disabled")
            $("#FilesList").css("visibility", "hidden");
        }

        //Body Search Edit
        if (document.getElementById("Document_Type_Id1").value >=1) {
           
            $("#fileInputEdit").removeAttr("disabled")
            if ($('#FilesListEdit tr').length > 1) {
                $("#fileInputEdit").removeAttr("required")
                //$("#FilesListEdit").css("visibility", "visible");
            } else {
                $("#fileInputEdit").attr("required", "required")
            }
        } else {
            $("#fileInputEdit").attr("disabled", "disabled")
            //$("#FilesListEdit").css("visibility", "hidden");
        }

        //Illegal Items
        if (document.getElementById("DocType_Id").value >=1) {            
            $("#fileInputIllegalAdd").removeAttr("disabled")
            if ($('#FilesListIllegalAdd tr').length > 1) {
                $("#fileInputIllegalAdd").removeAttr("required")
                $("#FilesListIllegalAdd").css("visibility", "visible");
            } else {
                $("#fileInputIllegalAdd").attr("required", "required")
            }
        } else {
            $("#fileInputIllegalAdd").attr("disabled", "disabled")
            $("#FilesListIllegalAdd").css("visibility", "hidden");
        }

        //if ($('#FilesList1 tr').length > 1) {
        //    $("#FilesList1").css("visibility", "visible");
        //} else {
        //    $("#FilesList1").css("visibility", "hidden");
        //}
    }
    $("#InsertBodySearch").click(function () {
        var $valid = $("#myForm").valid();
        if (!$valid) {
            return;
        }
        console.log(formdata);
        formdata.append('uploadername', $('#txtuploader').val());
        formdata.append("Bodily_Search_Date", $("#BodySearchDate").val());
        formdata.append("Conducted_By", $("#CondactedBy").val());
        formdata.append("Document_Type_Id", $("#Document_Type_Id").val());
        formdata.append("Witnessed_By", $("#WitnessedBy").val());
        formdata.append("Search_Reason_Id", $("#SearchReasonId").val());
        formdata.append("Person_Id",@ViewBag.childId);
        formdata.append("Description", $("#Description").val());
        $.confirm({
            title: 'Add Bodily Search',
            content: 'Are you sure you want to save changes?',
            buttons: {
                Yes: function () {
                    $.ajax({
                        url: '@Url.Action("NewBodilySearch", "Admit")',
                        type: "POST",
                        contentType: false, // Not to set any content header
                        processData: false, // Not to process data
                        data: formdata,
                        async: false,
                        success: function (result) {
                            //alert( "the result is : " + result);
                            location.reload(true);   
                            @*window.location.href = "/Admit/BodySearchList/" + @ViewBag.childId*@
                        },
                        error: function (error) {
                            console.log(error);
                        }
                    })
                },
                No: function () {

                }
            }
        })
    });


  $("#InsertIllegalItem").click(function () {                    
      var $valid = $("#myForm3").valid();      
        if (!$valid) {
            return;
        }
        console.log(formdata);
        formdata.append('uploadername', $('#txtuploader').val());                  
        formdata.append("Person_Id",@ViewBag.childId);
        formdata.append("Description", $("#Item_Description").val());
        formdata.append("Quantity", $("#Quantity").val());
        formdata.append("Handed_By", $("#Handed_By").val());
        formdata.append("Document_Type_Id", $("#DocType_Id").val()); 
      $.confirm({
          title: 'Add Illegal Item',
          content: 'Are you sure you want to save changes?',
          buttons: {
              Yes: function () {
                  $.ajax({
                      url: '@Url.Action("NewIllegalItem", "Admit")',
                      type: "POST",
                      contentType: false, // Not to set any content header
                      processData: false, // Not to process data
                      data: formdata,
                      async: false,
                      success: function (result) {
                          location.reload(true);  

                      },
                      error: function (error) {
                          console.log(error);
                      }
                  });
              },
              No: function () {

              }
          }
      })
    });
    

    $('#cancelAdd').click(function () {
        document.getElementById("myForm").reset();       
        $("#AddBodilySearch").hide();      
        $("#BodilySearchRecords").show();
        $("#IllegalItemsRecords").show();
    });
    $('#cancelIllegal').click(function () {        
        document.getElementById("myForm3").reset();       
        $("#AddIllegalItem").hide();
        $("#BodilySearchRecords").show();
        $("#IllegalItemsRecords").show();
    });
    function showEdit(BodySearchId) {
        $("#BodilySearchRecords").hide();
        $("#IllegalItemsRecords").hide();
        $("#AddBodilySearch").hide();
        $("#ViewBodilySearch").hide();
        $("#AddIllegalItem").hide();
        $("#EditIllegalItem").hide();
        $("#ViewIllegalItem").hide();
        $("#EditBodilySearch").show();
        var url = "/Admit/GetBodySearchByBodySearchId?BodySearchId=" + BodySearchId;
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                console.log(data);
                var obj = JSON.parse(data);
                $("#BodySearchId1").val(obj.BodySearchId);              
                $("#Conducted_By").val(obj.Conducted_By);
                $("#Document_Type_Id1").val(obj.Document_Type_Id);
                $("#Witnessed_By").val(obj.Witnessed_By);
                $("#Search_Reason_Id").val(obj.Search_Reason_Id);
                $("#Description1").val(obj.Description);   
                
            }, error: function () { alert('something bad happened'); }
        })
    }

    function showEditIllegal(Item_Found_Id) {
        $("#BodilySearchRecords").hide();
        $("#IllegalItemsRecords").hide();
        $("#AddBodilySearch").hide();
        $("#ViewBodilySearch").hide();
        $("#AddIllegalItem").hide();
        $("#EditBodilySearch").hide();
        $("#ViewIllegalItem").hide();
        $("#EditIllegalItem").show();
        var url = "/Admit/GetIllegalItemByIllegalItemId?Item_Found_Id=" + Item_Found_Id;
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                console.log(data);
                var obj = JSON.parse(data);
                $("#Item_Found_Id1").val(obj.Item_Found_Id);
                $("#Item_Description1").val(obj.Description);
                $("#Quantity1").val(obj.Quantity);
                $("#Handed_By1").val(obj.Handed_By);
                $("#DocType_Id1").val(obj.Document_Type_Id);
                

            }, error: function () { alert('something bad happened'); }
        })
    }

    $("#Edit").click(function () {
        if (!$('#myForm1').valid()) {
            return false;
        }  
        $.confirm({
            title: 'Edit Bodily Search',
            content: 'Are you sure you want to save changes?',
            buttons: {
                Yes: function () {
                    $.ajax({
                        type: "Post",
                        url: "/Admit/EditBodySearch",
                        data: {
                            Person_Id: @ViewBag.childId,
                            Admission_Id: $("#Admission_Id1").val(),
                            BodySearchId: $("#BodySearchId1").val(),
                            Conducted_By: $("#Conducted_By").val(),
                            Document_Type_Id: $("#Document_Type_Id1").val(),
                            Witnessed_By: $("#Witnessed_By").val(),
                            Search_Reason_Id: $("#Search_Reason_Id").val(),
                            Description: $("#Description1").val()
                        },
                        success: function (result) {
                            location.reload(true); 
                            //document.getElementById("myForm").reset();
                            //$("#EditBodilySearch").hide();
                            //$("#BodilySearchRecords").show();
                            //$("#IllegalItemsRecords").show();
                        },
                        error: function (xhr, ajaxOptions, error) {
                            alert(xhr.status);
                            alert('Error: ' + xhr.responseText);
                        }
                    })
                },
                No: function () {

                }
            }
        })
    })
    $("#SendEdit").click(function () {
        if (!$('#myForm4').valid()) {
            return false;
        }  
        $.confirm({
            title: 'Edit Illegal Item',
            content: 'Are you sure you want to save changes?',
            buttons: {
                Yes: function () {
                    $.ajax({
                        type: "Post",
                        url: "/Admit/EditIllegalItem",
                        data: {
                            Person_Id: @ViewBag.childId,
                            Admission_Id: $("#Admission_Id1").val(),                             
                            Item_Found_Id: $("#Item_Found_Id1").val(),
                            Quantity: $("#Quantity1").val(),
                            Document_Type_Id: $("#DocType_Id1").val(),
                            Handed_By: $("#Handed_By1").val(),                           
                            Description: $("#Item_Description1").val()
                        },
                        success: function (result) {
                            location.reload(true); 
                            //document.getElementById("myForm4").reset();
                            
                            //$("#EditIllegalItem").hide();
                            //$("#BodilySearchRecords").show();
                            //$("#IllegalItemsRecords").show();
                        },
                        error: function (xhr, ajaxOptions, error) {
                            alert(xhr.status);
                            alert('Error: ' + xhr.responseText);
                        }
                    })
                },
                No: function () {

                }
            }
        })
    })
    $("#cancelEdit").click(function () {
        document.getElementById("myForm1").reset();
        $("#BodilySearchRecords").show();
        $("#IllegalItemsRecords").show();

        $("#EditBodilySearch").hide();
    })
    $("#cancelEditIllegal").click(function () {
        document.getElementById("myForm1").reset();
        $("#BodilySearchRecords").show();
        $("#IllegalItemsRecords").show();

        $("#EditIllegalItem").hide();
    })
    
    function showView(BodySearchId) {
        $("#BodilySearchRecords").hide();
        $("#IllegalItemsRecords").hide();
        $("#AddBodilySearch").hide();
        $("#EditBodilySearch").hide();
        $("#AddIllegalItem").hide();
        $("#EditIllegalItem").hide();
        $("#ViewIllegalItem").hide();
        $("#ViewBodilySearch").show();
        var url = "/Admit/GetBodySearchByBodySearchId?BodySearchId=" + BodySearchId;
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                console.log(data);
                var obj = JSON.parse(data);
                $("#BodySearchId2").val(obj.BodySearchId);
                $("#Conducted_By2").val(obj.Conducted_By);
                $("#Document_Type_Id2").val(obj.Document_Type_Id);
                $("#Witnessed_By2").val(obj.Witnessed_By);
                $("#Search_Reason_Id2").val(obj.Search_Reason_Id);
                $("#Description2").val(obj.Description);

            }, error: function () { alert('something bad happened'); }
        })
    }
    function showViewIllegal(Item_Found_Id) {
        $("#BodilySearchRecords").hide();
        $("#IllegalItemsRecords").hide();
        $("#AddBodilySearch").hide();
        $("#EditBodilySearch").hide();
        $("#AddIllegalItem").hide();
        $("#ViewBodilySearch").hide();
        $("#EditIllegalItem").hide();
        $("#ViewIllegalItem").show();
        var url = "/Admit/GetIllegalItemByIllegalItemId?Item_Found_Id=" + Item_Found_Id;
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                console.log(data);
                var obj = JSON.parse(data);
                $("#Item_Found_Id2").val(obj.Item_Found_Id);
                $("#Item_Description2").val(obj.Description);
                $("#Quantity2").val(obj.Quantity);
                $("#Handed_By2").val(obj.Handed_By);
                $("#DocType_Id2").val(obj.Document_Type_Id);

            }, error: function () { alert('something bad happened'); }
        })
    }
    $("#closeView").click(function () {
        $("#BodilySearchRecords").show();
        $("#IllegalItemsRecords").show();
        $("#ViewBodilySearch").hide();
    })
    $("#CloseIllegal").click(function () {
        $("#BodilySearchRecords").show();
        $("#IllegalItemsRecords").show();
        $("#ViewIllegalItem").hide();
    })
    


    //-----------------------------------------------------------------Illegal Items------------------------------------//

 
   
