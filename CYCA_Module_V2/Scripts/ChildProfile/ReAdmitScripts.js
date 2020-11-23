
     var formdata = new FormData(); //FormData object
     $(document).ready(function () {
   
         $("#AddReAdmit").hide();

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
         
     })
     function showNewReAdmit(Admission_Id) {
         $("#AdmissionList").hide();
         $("#ReAdmissionList").hide();
         $("#AddReAdmit").show();
     }
     $(function () {
         // Initialize items marked as datepickers
         $("#caseenddate").datetimepicker({
             dateFormat: "dd M yy",
             changeMonth: true,
             changeYear: true,
             minDate: new Date(),
             //yearRange: "-60:+0",
             controlType: 'select',
             timeFormat: 'hh:mm TT',
             onClose: function () {
                 $(this).valid();
             }
         });
         $("#myForm").validate();
         //Check if signalR is working
         console.log(signalHub.IsConnected);
         if (signalHub.IsConnected) {
             $('.signalRObject').removeAttr('disabled');
         } else {
             $('.signalRObject').attr('disabled', 'disabled');
         };
     });
     function ShowHideDiv() {

         var Document_Type_Id = document.getElementById("Document_Type_Id");
         var dvOther = document.getElementById("dvOther");
         dvOther.style.display = Document_Type_Id.value == "4" ? "block" : "none";
     }

     function DeleteFile(Fileid, FileName) {
         formdata.delete(FileName)
         $("#" + Fileid).remove();
         chkatchtbl();
     }
     function chkatchtbl() {
         //if ($('#FilesList tr').length > 1) {
         //    $("#FilesList").css("visibility", "visible");
         //} else {
         //    $("#FilesList").css("visibility", "hidden");
         //}
         if (document.getElementById("Document_Type_Id").value >= 1) {
             $("#fileInput").removeAttr("disabled")
             if ($('#FilesList tr').length > 1) {
                 $("#fileInput").removeAttr("required")
                 $("#FilesList").css("visibility", "visible");
             } else {
                 $("#fileInput").attr("required", "required")
             }
         } else {
             $("#fileInput").attr("disabled", "disabled")
             $("#fileInput").removeAttr("required")
             $("#FilesList").css("visibility", "hidden");
         }
     }

 