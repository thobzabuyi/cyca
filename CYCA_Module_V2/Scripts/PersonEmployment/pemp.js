var $dialog = $('#educationDialog');

console.log('My script');

$(function () {employmentItemsGrid
    var personId = $('#Person_Id').val();

    // Initialize Modal Popup
    // attach modal-container bootstrap attributes to links with .modal-link class.
    // when a link is clicked with these attributes, bootstrap will display the href content in a modal dialog.
    $('body').on('click', '.modal-link', function (e) {
        // Relation button contains some extra functionality so we need to handle it seperately
        if ($(this).attr("id") == 'AddRelationButton') {
            // We need to grab the selectedRelationTypeId from the dropdown and fix the button's path because it's not available at time of submitting.
            var selectedRelationTypeId = $('#Relation_Type_Id').val();

            var page = $(this).attr('href');

            // Validate that we've selected a Relation Type to add before showing the popup
            if ((selectedRelationTypeId == '') && (page.indexOf('selectedRelationTypeId=-1') !== -1)) {
                alert('Please select a Relation Type to add');
                e.preventDefault();
            } else {
                var newPage = page;

                // .. but only if it's not set by the Edit function. Create function would return a -1
                if (page.indexOf('selectedRelationTypeId=-1') !== -1) {
                    newPage = page.replace('selectedRelationTypeId=-1', 'selectedRelationTypeId=' + selectedRelationTypeId);
                    $(this).attr('href', newPage);
                }

                e.preventDefault();
                $(this).attr('data-target', '#modal-container');
                $(this).attr('data-toggle', 'modal');
            }
        } else {
            e.preventDefault();
            $(this).attr('data-target', '#modal-container');
            $(this).attr('data-toggle', 'modal');
        }
    });

    // Attach listener to .modal-close-btn's so that when the button is pressed the modal dialog disappears
    $('body').on('click', '.modal-close-btn', function () {
        $('#modal-container').modal('hide');
    });

    //clear modal cache, so that new content can be loaded
    $('#modal-container').on('hidden.bs.modal', function () {
        $(this).removeData('bs.modal');
    });

    $('#CancelModal').on('click', function () {
        return false;
    });


    // Add custom rule for validating ID Number
    jQuery.validator.addMethod("idNumberValidation", function (value, element) {
        var idType = $("#DropdownIdentificationType").val();

        // Only do validation for RSA ID Numbers, if not then just return true to allow validation to pass
        if (idType == '1') {
            var idDetails = extractFromID(value);
            return (idDetails.valid && value.length == 13) || value === '';
        } else
            return true;
    }, "The ID Number does not appear to be valid");

    // Initialize jQuery validation
    $("#clientDetailsForm").validate();

    $("#Identification_Number").rules("add", {
        idNumberValidation: true
    });

    // Initialize Bootstrap Wizard
    $('#rootwizard').bootstrapWizard({
        'tabClass': 'nav nav-pills',
        'onNext': function (tab, navigation, index) {
            return validateForm();
        },
        'onTabClick': function (tab, navigation, index) {
            return validateForm();
        }
    });

    // Initialize items marked as datepickers
    $(".jqueryui-marker-datepicker").datepicker({
        dateFormat: "dd M yy",
        changeYear: true,
        onSelect: function (d, i) {
            if (d !== i.lastVal) {
                $(this).change();
            }
        }
    });

    // Helper function to calculate age from date of birth field
    $('.jqueryui-marker-datepicker').change(function () {
        var dob = new Date($(this).val());
        if ((this.id == 'Date_of_Birth') && (!isNaN(dob))) {
            var today = new Date();
            $(this).datepicker("setDate", new Date(dob));
            var age = today.getFullYear() - dob.getFullYear();
            var m = today.getMonth() - dob.getMonth();
            if (m < 0 || (m === 0 && today.getDate() < dob.getDate())) {
                age--;
            }

            $("#TextboxAge").val(age);
        }
    });

    // Temporary Workaround: Reset date values so that they display properly in the DatePicker fields
    $('.jqueryui-marker-datepicker').each(function (i, obj) {
        if ($(this).val() != '') {
            var dateValue = new Date($(this).val());
            $(this).datepicker("setDate", new Date(dateValue));
        }
    });

    // Set ID Number field label according to Identification Type Selected
    $("#DropdownIdentificationType").change(function () {
        var idType = $(this).val();

        if (idType == '1') {
            $("#LabelIdentificationNumber").html("ID Number");
        } else if (idType == '2') {
            $("#LabelIdentificationNumber").html("Passport Number");
        }
    });

    // Change Event for ID Number in order to do validation
    $("#Identification_Number").change(function () {
        var idType = $("#DropdownIdentificationType").val();

        // Only do calculations for RSA ID Numbers                
        if (idType == '1') {
            var idDetails = extractFromID($(this).val());

            if (idDetails.valid) {
                // A valid id number is detected, assign the gender
                if (idDetails.gender == 'male') {
                    $("#DropdownGender").val('1'); // Don't like that the values is hardcoded, but not sure how to handle it otherwise
                } else {
                    $("#DropdownGender").val('2'); // Don't like that the values is hardcoded, but not sure how to handle it otherwise
                }

                // Date of Birth
                var dob = idDetails.birthdate;

                $("#Date_of_Birth").datepicker("setDate", dob);

                var today = new Date();
                var birthDate = new Date(dob);
                var age = today.getFullYear() - birthDate.getFullYear();
                var m = today.getMonth() - birthDate.getMonth();
                if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
                    age--;
                }

                $("#TextboxAge").val(age);
                $("#IsEstimatedAge").prop('checked', false);
            }
        }
    });

    // Populate Education Items
    LoadServicesItems(personId);
    LoadReferalsItems(personId);
    LoadEducationItems(personId);
    LoadEmploymentItems(personId);
    LoadMedicalItems(personId);
    LoadRelations(personId, "-1");

    //// Add Popup Event Receiver for Education
    //$('body').on("click", "a.popupEducation", function (e) {
    //    e.preventDefault();
    //    var page = $(this).attr('href');
    //    OpenPopup(page);

    //    $dialog.dialog('option', 'title', 'Education Details');
    //});
    //// Add Popup Event Receiver for Employment
    //$('body').on("click", "a.popupEmployment", function (e) {
    //    e.preventDefault();
    //    var page = $(this).attr('href');
    //    OpenPopup(page);

    //    $dialog.dialog('option', 'title', 'Employment Details');
    //});
    //// Add Popup Event Receiver for Medical Details
    //$('body').on("click", "a.popupMedicalDetail", function (e) {
    //    e.preventDefault();
    //    var page = $(this).attr('href');
    //    OpenPopup(page);

    //    $dialog.dialog('option', 'title', 'Medical Details');
    //});
    //// Add Popup Event Receiver for Relation Details
    //$('body').on("click", "a.popupRelation", function (e) {
    //    // We need to grab the selectedRelationTypeId from the dropdown and fix the button's path because it's not available at time of submitting.
    //    var selectedRelationTypeId = $('#Relation_Type_Id').val();

    //    var page = $(this).attr('href');

    //    // Validate that we've selected a Relation Type to add before showing the popup
    //    if ((selectedRelationTypeId == '') && (page.indexOf('selectedRelationTypeId=-1') !== -1)) {
    //        alert('Please select a Relation Type to add');
    //        e.preventDefault();
    //    } else {
    //        var newPage = page;

    //        // .. but only if it's not set by the Edit function. Create function would return a -1
    //        if (page.indexOf('selectedRelationTypeId=-1') !== -1) {
    //            newPage = page.replace('selectedRelationTypeId=-1', 'selectedRelationTypeId=' + selectedRelationTypeId);
    //        }

    //        e.preventDefault();
    //        OpenPopup(newPage);

    //        $dialog.dialog('option', 'title', 'Relation Details');
    //        $dialog.dialog('option', 'height', 700);
    //    }
    //});
    // Add Filter Event Receiver for Relation Details
    $("body").on("click", "a.filterRelation", function (e) {
        e.preventDefault();
        var selectedRelationTypeId = $('#Relation_Type_Id').val();

        if (selectedRelationTypeId == '') {
            selectedRelationTypeId = "-1";
        }

        LoadRelations(personId, selectedRelationTypeId);
    });

    $("body").on('submit', '#saveCreateVEPServices', function (e) {
        e.preventDefault();
        CreateVEPServicesDetails();
    });

    $("body").on('submit', '#saveCreateVEPReferals', function (e) {
        e.preventDefault();
        CreateVEPReferalsDetails();
    });

    // Create Education Details
    $("body").on('submit', '#saveFormCreate', function (e) {
        e.preventDefault();
        CreateEducationDetails();
    });

    // Edit Education Details
    $("body").on('submit', '#saveFormEdit', function (e) {
        e.preventDefault();
        EditEducationDetails();
    });

    // Create Employment Details
    $("body").on('submit', '#employmentFormCreate', function (e) {
        e.preventDefault();
        CreateEmploymentDetails();
    });

    // Edit Employment Details
    $("body").on('submit', '#employmentFormEdit', function (e) {
        e.preventDefault();
        EditEmploymentDetails();
    });

    // Create Medical Details
    $("body").on('submit', '#medicalDetailFormCreate', function (e) {
        e.preventDefault();
        CreateMedicalDetails();
    });

    // Edit Medical Details
    $("body").on('submit', '#medicalDetailFormEdit', function (e) {
        e.preventDefault();
        EditMedicalDetails();
    });

    // Create Relation Details
    $("body").on('submit', "#relationFormCreate", function (e) {
        e.preventDefault();
        CreateRelationDetails();
    });

    // Edit Relation Details
    $("body").on('submit', '#relationFormEdit', function (e) {
        e.preventDefault();
        EditRelationDetails();
    });

    // Handle Cascaded dropdown for Problem Category / Sub-Category
    $("#DropdownProblemCategory").change(function () {
        var selectedItem = $(this).val();
        $.ajax({
            url: $.url('/ProblemCategory/GetSubCategoriesForCategory'),
            data: { "problemCategoryId": selectedItem },
            type: "GET",
            success: function (response, status, xhr) {
                var dropdownControllers = $('#DropdownProblemSubCategory');
                dropdownControllers.html('');
                $.each(response, function (id, option) {
                    dropdownControllers.append($('<option></option>').val(option.id).html(option.name));
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('Error populating problem sub-category dropdown');
            }
        });
    });

    // On Assessment, check if the case manager supervisor field has been filled, if so, show the case manager input fields
    if (($("#DropdownCaseManagerSupervisor").val() != null) && ($("#DropdownCaseManagerSupervisor").val() != '')) {
        $("#CaseManagerSelection").show();
    }

    // Physical Address items
    $("#DropdownPhysicalAddressProvince").change(function () {
        var dropdownPhysicalMunicipality = $('#DropdownPhysicalAddressMunicipality');
        dropdownPhysicalMunicipality.html('');
        dropdownPhysicalMunicipality.append($('<option></option>').val("").html("- Please Select -"));

        var dropdownCPhysicalLocalMunicipality = $('#DropdownPhysicalAddressLocalMunicipality');
        dropdownCPhysicalLocalMunicipality.html('');
        dropdownCPhysicalLocalMunicipality.append($('<option></option>').val("").html("- Please Select -"));

        var dropdownPhysicalTown = $('#DropdownPhysicalAddressTown');
        dropdownPhysicalTown.html('');
        dropdownPhysicalTown.append($('<option></option>').val("").html("- Please Select -"));

        var selectedItem = $(this).val();
        $.ajax({
            url: $.url('/Intake/FilterFromProvinceAjax'),
            data: { "provinceId": selectedItem },
            type: "GET",
            success: function (response, status, xhr) {
                $.each(response, function (id, option) {
                    dropdownPhysicalMunicipality.append($('<option></option>').val(option.id).html(option.name));
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('Error populating municipality dropdown');
            }
        });

        PopulatePhysicalTownList();
    });

    $("#DropdownPhysicalAddressMunicipality").change(function () {
        var dropdownCPhysicalLocalMunicipality = $('#DropdownPhysicalAddressLocalMunicipality');
        dropdownCPhysicalLocalMunicipality.html('');
        dropdownCPhysicalLocalMunicipality.append($('<option></option>').val("").html("- Please Select -"));

        var dropdownPhysicalTown = $('#DropdownPhysicalAddressTown');
        dropdownPhysicalTown.html('');
        dropdownPhysicalTown.append($('<option></option>').val("").html("- Please Select -"));

        var selectedItem = $(this).val();
        $.ajax({
            url: $.url('/Intake/FilterFromMunicipalityAjax'),
            data: { "municipalityId": selectedItem },
            type: "GET",
            success: function (response, status, xhr) {
                $.each(response, function (id, option) {
                    dropdownCPhysicalLocalMunicipality.append($('<option></option>').val(option.id).html(option.name));
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('Error populating local municipality dropdown');
            }
        });

        PopulateTownList();
    });

    $("#DropdownPhysicalAddressLocalMunicipality").change(function () {
        PopulatePhysicalTownList();
    });

    $("#DropdownPhysicalAddressTown").change(function () {
        var selectedItem = $(this).val();
        $.ajax({
            url: $.url('/Intake/FilterFromTown'),
            data: { "townId": selectedItem },
            type: "GET",
            success: function (response, status, xhr) {
                var arr = response.split(',');

                PopulateAndSelectPhysicalProvinceList(arr[0]);
                PopulateAndSelectPhysicalMunicipalityList(arr[0], arr[1]);
                PopulateAndSelectPhysicalLocalMunicipalityList(arr[1], arr[2]);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('Error flitering town dropdown');
            }
        });
    });


    // Postal Address items
    $("#DropdownPostalAddressProvince").change(function () {
        var dropdownPostalMunicipality = $('#DropdownPostalAddressMunicipality');
        dropdownPostalMunicipality.html('');
        dropdownPostalMunicipality.append($('<option></option>').val("").html("- Please Select -"));

        var dropdownCPostalLocalMunicipality = $('#DropdownPostalAddressLocalMunicipality');
        dropdownCPostalLocalMunicipality.html('');
        dropdownCPostalLocalMunicipality.append($('<option></option>').val("").html("- Please Select -"));

        var dropdownPostalTown = $('#DropdownPostalAddressTown');
        dropdownPostalTown.html('');
        dropdownPostalTown.append($('<option></option>').val("").html("- Please Select -"));

        var selectedItem = $(this).val();
        $.ajax({
            url: $.url('/Intake/FilterFromProvinceAjax'),
            data: { "provinceId": selectedItem },
            type: "GET",
            success: function (response, status, xhr) {
                $.each(response, function (id, option) {
                    dropdownPostalMunicipality.append($('<option></option>').val(option.id).html(option.name));
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('Error populating municipality dropdown');
            }
        });

        PopulateTownList();
    });

    $("#DropdownPostalAddressMunicipality").change(function () {
        var dropdownCPostalLocalMunicipality = $('#DropdownPostalAddressLocalMunicipality');
        dropdownCPostalLocalMunicipality.html('');
        dropdownCPostalLocalMunicipality.append($('<option></option>').val("").html("- Please Select -"));

        var dropdownPostalTown = $('#DropdownPostalAddressTown');
        dropdownPostalTown.html('');
        dropdownPostalTown.append($('<option></option>').val("").html("- Please Select -"));

        var selectedItem = $(this).val();
        $.ajax({
            url: $.url('/Intake/FilterFromMunicipalityAjax'),
            data: { "municipalityId": selectedItem },
            type: "GET",
            success: function (response, status, xhr) {
                $.each(response, function (id, option) {
                    dropdownCPostalLocalMunicipality.append($('<option></option>').val(option.id).html(option.name));
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('Error populating local municipality dropdown');
            }
        });

        PopulateTownList();
    });

    $("#DropdownPostalAddressLocalMunicipality").change(function () {
        PopulatePostalTownList();
    });

    $("#DropdownPostalAddressTown").change(function () {
        var selectedItem = $(this).val();
        $.ajax({
            url: $.url('/Intake/FilterFromTown'),
            data: { "townId": selectedItem },
            type: "GET",
            success: function (response, status, xhr) {
                var arr = response.split(',');

                PopulateAndSelectPostalProvinceList(arr[0]);
                PopulateAndSelectPostalMunicipalityList(arr[0], arr[1]);
                PopulateAndSelectPostalLocalMunicipalityList(arr[1], arr[2]);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('Error flitering town dropdown');
            }
        });
    });

});

// Validation check to stop the wizard tabs from switching if there's a validation error
function validateForm() {
    var $valid = $("#clientDetailsForm").valid();

    if (!$valid) {
        return false;
    }

    return true;
}

// Load Services Items
//function LoadServicesItems(person_Id) {
//    $.ajax({
//        url: $.url('/VEPServices/GetServicesItemsByAjax'),
//        data: { "id": person_Id },
//        datatype: "json",
//        type: "post",
//        contenttype: 'application/json; charset=utf-8',
//        async: true,
//        success: function (data) {
//            console.log('Test number 2');
//            $("#servicesItemsGrid").html(data);
//        },
//        error: function (xhr) {
//            alert('error Services Items');
//        }
//    });
//}

// Load Services Items
//function LoadReferalsItems(person_Id) {
//    console.log('referals 1');
//    $.ajax({
//        url: $.url('/VEPReferrals/GetReferalsItemsByAjax'),
//        data: { "id": person_Id },
//        datatype: "json",
//        type: "post",
//        contenttype: 'application/json; charset=utf-8',
//        async: true,
//        success: function (data) {
//            console.log('Test referal List');
//            console.log('referals 2');
//            $("#referalsDetailsGrid").html(data);
//        },
//        error: function (xhr) {
//            alert('error Referals Items 1 to do');
//        }
//    });
//}

// Load Education Items
function LoadEducationItems(person_Id) {
    $.ajax({
        url: $.url('/Education/GetEducationItemsByAjax'),
        data: { "id": person_Id },
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        async: true,
        success: function (data) {
            $("#educationItemsGrid").html(data);
        },
        error: function (xhr) {
            alert('error Education Items');
        }
    });
}

// Load Employment Items
function LoadEmploymentItems(person_Id) {


    $.ajax({
        url: $.url('/Employment/GetEmploymentItemsByAjax'),
        data: { "id": person_Id },
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        async: true,
        success: function (data) {
            $("#employmentItemsGrid").html(data);
        },
        error: function (xhr) {
            alert('error Employment Items');
        }
    });
}

// Load Medical Items
function LoadMedicalItems(person_Id) {
    $.ajax({
        url: $.url('/MedicalDetail/GetMedicalItemsByAjax'),
        data: { "id": person_Id },
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        async: true,
        success: function (data) {
            $("#medicalDetailsGrid").html(data);
        }
        //,
        //error: function (xhr) {
        //    alert('error Medical Items');
        //}
    });
}

// Load Relations
function LoadRelations(person_Id, relation_Type_Id) {
    $.ajax({
        url: $.url('/Relation/GetRelationItemsByAjax'),
        data: { "id": person_Id, "relationTypeId": relation_Type_Id },
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        async: true,
        success: function (data) {
            $("#relationsGrid").html(data);
        },
        error: function (xhr) {
            alert('error Relation Items');
        }
    });
}

//open popup
function OpenPopup(page) {
    var $pageContent = $('<div/>');
    $pageContent.load(page);
    $dialog = $('<div class="popupWindow" style="overflow:hidden"></div>')
        .html($pageContent)
        .dialog({
            draggable: false,
            autoOpen: false,
            resizable: false,
            modal: true,
            width: 1024,
            height: 340,
            buttons: [
                {
                    text: "Cancel",
                    click: function () {
                        $(this).dialog("close");
                    }
                }
            ],
            close: function () {
                $dialog.dialog('destroy').remove();
            }
        });

    $dialog.dialog('open');
}

// Create New VEP Referals Item
function CreateVEPReferalsDetails() {
    console.log('referal')
    var caseId = $('#CaseId').val();

    var VEPServices = {
        caseId: $('#CaseId').val(),
        FromDepartment: $('#FromDepartment').val(),
        ToDepartment: $('#ToDepartment').val(),
        Notes: $('#Notes').val().trim()
    };

    //Add validation token
    VEPServices.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();

    //Save Education Details
    $.ajax({
        url: $.url('VEPReferrals/Create'),
        type: 'POST',
        data: VEPServices,
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                $('#modal-container').modal('hide');
                $('#CaseId').val('');
                $('#FromDepartment').val('');
                $('#ToDepartment').val('');
                $('#Notes').val('');

                $('#rootwizard a[href="#tab9"]').trigger('click');
            }

            LoadReferalsItems(caseId);
        },
        error: function () {
            $('#msg').html('<div class="failed">Error! Please try again.</div>');
        }
    });
}


function CreateVEPServicesDetails() {
    console.log('services')
    var caseId = $('#CaseId').val();

    var VEPServices = {
        caseId: $('#CaseId').val(),
        serviceTypeId: $('#ServiceTypeId').val(),
        serviceNotes: $('#ServiceNotes').val().trim()
    };

    //Add validation token
    VEPServices.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();

    //Save Education Details
    $.ajax({
        url: $.url('VEPServices/Create'),
        type: 'POST',
        data: VEPServices,
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                $('#modal-container').modal('hide');
                $('#CaseId').val('');
                $('#ServiceTypeId').val('');
                $('#ServiceNotes').val('');
                $('#rootwizard a[href="#tab8"]').trigger('click');
            }

            LoadServicesItems(caseId);
        },
        error: function () {
            $('#msg').html('<div class="failed">Error! Please try again.</div>');
        }
    });
}


// Create New Education Item
function CreateEducationDetails() {

    var personId = $('#Person_Id').val();

    var personEducation = {
        Person_Id: $('#Person_Id').val(),
        School_Id: $('#School_Id').val(),
        Grade_Completed_Id: $('#Grade_Completed_Id').val(),
        Year_Completed: $('#Year_Completed').val().trim(),
        Date_Last_Attended: $('#Date_Last_Attended').val().trim(),
        Additional_Information: $('#Additional_Information').val().trim()
    };

    //Add validation token
    personEducation.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();

    //Save Education Details
    $.ajax({
        url: $.url('Education/Create'),
        type: 'POST',
        data: personEducation,
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                $('#modal-container').modal('hide');
                $('#School_Id').val('');
                $('#Grade_Completed_Id').val('');
                $('#Year_Completed').val('');
                $('#Date_Last_Attended').val('');
                $('#Additional_Information').val('');
            }

            LoadEducationItems(personId);
        },
        error: function () {
            $('#msg').html('<div class="failed">Error! Please try again.</div>');
        }
    });
}

// Edit Education Item
function EditEducationDetails() {

    var personId = $('#Person_Id').val();

    var personEducation = {
        Person_Education_Id: $('#Person_Education_Id').val(),
        Person_Id: $('#Person_Id').val(),
        School_Id: $('#School_Id').val(),
        Grade_Completed_Id: $('#Grade_Completed_Id').val(),
        Year_Completed: $('#Year_Completed').val().trim(),
        Date_Last_Attended: $('#Date_Last_Attended').val().trim(),
        Additional_Information: $('#Additional_Information').val().trim()
    };

    //Add validation token
    personEducation.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();

    //Save Education Details
    $.ajax({
        url: $.url('/Education/Edit'),
        type: 'POST',
        data: personEducation,
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                $('#modal-container').modal('hide');
                $('#School_Id').val('');
                $('#Grade_Completed_Id').val('');
                $('#Year_Completed').val('');
                $('#Date_Last_Attended').val('');
                $('#Additional_Information').val('');
            }

            LoadEducationItems(personId);
        },
        error: function () {
            $('#msg').html('<div class="failed">Error! Please try again.</div>');
        }
    });
}

// Create New Employment Item
function CreateEmploymentDetails() {
    var personId = $('#Person_Id').val();

    var personEmployment = {
        Person_Id: $('#Person_Id').val(),
        NameOfEmployer: $('#NameOfEmployer').val(),
        Nature_Of_Employment_Id: $('#NatureOfEmployment_Id').val(),
        Occupation: $('#Occupation').val().trim(),
        Income_Range_Id: $('#Income_Range_Id').val()
    };

    // Add Validation Token
    personEmployment.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();

    // Save Education Details
    $.ajax({
        url: $.url('/Employment/Create'),
        type: 'POST',
        data: personEmployment,
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                $('#modal-container').modal('hide');
                $('#NameOfEmployer').val('');
                $('#NatureOfEmployment_Id').val('');
                $('#Occupation').val('');
                $('#Income_Range_Id').val('');
                $(this).dialog('hide');
            }
            $('#modal-container').modal('hide');
            //location.reload(true);

            LoadEmploymentItems(personId);
        },
        error: function () {
            $('#msg').html('<div class="failed">Error! Please try again.</div>');
        }
    });
}

// Edit Employment Item
function EditEmploymentDetails() {
    var personId = $('#Person_Id').val();

    var personEmployment = {
        Person_Employment_Id: $('#Person_Employment_Id').val(),
        Person_Id: $('#Person_Id').val(),
        NameOfEmployer: $('#NameOfEmployer').val(),
        Nature_Of_Employment_Id: $('#NatureOfEmployment_Id').val(),
        Occupation: $('#Occupation').val().trim(),
        Income_Range_Id: $('#Income_Range_Id').val()
    };

    //Add validation token
    personEmployment.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();

    //Save Education Details
    $.ajax({
        url: $.url('/Employment/Edit'),
        type: 'POST',
        data: personEmployment,
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                $('#modal-container').modal('hide');
                $('#NameOfEmployer').val('');
                $('#NatureOfEmployment_Id').val('');
                $('#Occupation').val('');
                $('#Income_Range_Id').val('');
            }
            $('#modal-container').modal('hide');
            LoadEmploymentItems(personId);
        },
        error: function () {
            $('#msg').html('<div class="failed">Error! Please try again.</div>');
        }
    });
}

// Create New Medical Detail Item
function CreateMedicalDetails() {
    var personId = $('#Person_Id').val();

    var intakeMedicalConditionItem = {
        Person_Id: $('#Person_Id').val(),
        Medical_Condition_Type_Id: $('#Medical_Condition_Type_Id').val(),
        Medical_Condition_Id: $('#Medical_Condition_Id').val()
    };

    // Add Validation Token
    intakeMedicalConditionItem.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();

    // Save Education Details
    $.ajax({
        url: $.url('/MedicalDetail/Create'),
        type: 'POST',
        data: intakeMedicalConditionItem,
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                $('#modal-container').modal('hide');
                $('#Medical_Condition_Type_Id').val('');
                $('#Medical_Condition_Id').val('');
            }
            $('#modal-container').modal('hide');
            LoadMedicalItems(personId);
        },
        error: function () {
            $('#msg').html('<div class="failed">Error! Please try again.</div>');
        }
    });
}

// Edit Medical Detail
function EditMedicalDetails() {
    var personId = $('#Person_Id').val();

    var intakeMedicalConditionItem = {
        Person_Id: $('#Person_Id').val(),
        Remove_Item_Type_Id: $('#Remove_Item_Type_Id').val(),
        Remove_Item_Id: $('#Remove_Item_Id').val(),
        Medical_Condition_Type_Id: $('#Medical_Condition_Type_Id').val(),
        Medical_Condition_Id: $('#Medical_Condition_Id').val()
    };

    // Add Validation Token
    intakeMedicalConditionItem.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();

    // Save Education Details
    $.ajax({
        url: $.url('/MedicalDetail/Edit'),
        type: 'POST',
        data: intakeMedicalConditionItem,
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                $('#modal-container').modal('hide');
                $('#Remove_Item_Id').val('');
                $('#Medical_Condition_Type_Id').val('');
                $('#Medical_Condition_Id').val('');
            }
            $('#modal-container').modal('hide');
            LoadMedicalItems(personId);
        },
        error: function () {
            $('#msg').html('<div class="failed">Error! Please try again.</div>');
        }
    });
}

// Create New Relation Item
function CreateRelationDetails() {

    var personId = $('#Person_Id').val();
    
    // Build Physical Address Object
    var physicalAddress = {
        Address_Type_Id: $('#Physical_Address_Type_Id').val(),
        Address_Line_1: $('#Popup_Physical_Address_Line_1').val().trim(),
        Address_Line_2: $('#Popup_Physical_Address_Line_1').val().trim(),
        Selected_Province_Id: $('#EProvinceId').val(),
        Selected_Municipality_Id: $('#MagistratesId').val(),
        Selected_Local_Municipality_Id: $('#LocalMunicipality').val(),
        Town_Id: $('#Address_Town_Id').val(),
        Postal_Code: $('#Popup_Physical_Address_Postal_Code').val().trim()
    };

    // Build Postal Address Object
    var postalAddress = {
        Address_Type_Id: $('#Postal_Address_Type_Id').val(),
        Address_Line_1: $('#Popup_Postal_Address_Line_1').val().trim(),
        Address_Line_2: $('#Popup_Postal_Address_Line_2').val().trim(),
        Selected_Province_Id: $('#ProvinceId_1').val(),
        Selected_Municipality_Id: $('#MagistratesId_1').val(),
        Selected_Local_Municipality_Id: $('#PostalAddressLocalMunicipality').val(),
        Town_Id: $('#Postal_Address_Town_Id').val(),
        Postal_Code: $('#Popup_Postal_Address_Postal_Code').val().trim()
    };

    // Build Person Object
    var relatedPerson = {
        Person_Id: $('#Popup_Related_Person_Id').val(),
        First_Name: $('#Popup_First_Name').val().trim(),
        Last_Name: $('#Popup_Last_Name').val().trim(),
        Known_As: $('#Popup_Known_As').val().trim(),
        Citizenship_Id: $('#DropdownCitizenship').val(),
        Identification_Type_Id: $('#DropdownIdentificationType_Popup').val(),
        Identification_Number: $('#Identification_Number_Popup').val().trim(),
        Date_Of_Birth: $('#Date_of_Birth_Popup').val(),
        Age: $('#TextboxAge_Popup').val().trim(),
        Is_Estimated_Age: $('#IsEstimatedAge_Popup').val().trim(),
        Gender_Id: $('#DropdownGender_Popup').val(),
        Nationality_Id: $('#Nationality_Id').val(),
        Language_Id: $('#DropdownLanguage').val(),
        Sexual_Orientation_Id: $('#DropdownSexualOrientation').val(),
        Marital_Status_Id: $('#DropdownMaritalStatus_Popup').val(),
        Population_Group_Id: $('#DropdownPopulationGroup_Popup').val(),
        Religion_Id: $('#DropdownReligion_Popup').val(),
        Disability_Id: $('#DropdownDisability_Popup').val(),
        Phone_Number: $('#Popup_Phone_Number').val().trim(),
        Mobile_Phone_Number: $('#Popup_Mobile_Phone_Number').val().trim(),
        Email_Address: $('#Popup_Email_Address').val().trim(),
        Preferred_Contact_Type_Id: $('#Popup_Preferred_Contact_Type').val(),
    };

    // Combine
    var personDetail = {
        Person: relatedPerson,
        PhysicalAddress: physicalAddress,
        PostalAddress: postalAddress
    };

    // Build Relation Object
    var intakeRelationItem = {
        Person_Id: $('#Person_Id').val(),
        Relation_Type_Id: $('#Relation_Type_Id').val(),
        CreatePerson: personDetail,
        IsDeceased: $('#Is_Deceased').val(),
        DateDeceased: $('#Date_Deceased').val().trim(),
        Person_Relationship_Type_Id: $('#Person_Relationship_Type_Id').val()
    };

    // Add Validation Token
    intakeRelationItem.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();

    // Save Relation Details
    $.ajax({
        url: $.url('/Relation/Create'),
        type: 'POST',
        data: intakeRelationItem,
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                $('#modal-container').modal('hide');
                $('#Relation_Type_Id').val('');
            }

            LoadRelations(personId, "-1");
        },
        error: function () {
            $('#msg').html('<div class="failed">Error! Please try again.</div>');
        }
    });
}

// Physical Address items
function PopulateAndSelectPhysicalProvinceList(provinceId) {
    $.ajax({
        url: $.url('/Intake/GetListOfProvinces'),
        type: "GET",
        success: function (response, status, xhr) {
            var dropdownPhysicalProvince = $('#DropdownPhysicalAddressProvince');
            dropdownPhysicalProvince.html('');
            dropdownPhysicalProvince.append($('<option></option>').val("").html("- Please Select -"));
            $.each(response, function (id, option) {
                dropdownPhysicalProvince.append($('<option></option>').val(option.id).html(option.name));
            });

            if (provinceId != -1)
                dropdownPhysicalProvince.val(provinceId);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert('Error populating Province dropdown');
        }
    });
}

function PopulateAndSelectPhysicalMunicipalityList(provinceId, municipalityId) {
    $.ajax({
        url: $.url('/Intake/GetListOfMunicipalities'),
        data: { "provinceId": provinceId },
        type: "GET",
        success: function (response, status, xhr) {
            var dropdownPhysicalMunicipality = $('#DropdownPhysicalAddressMunicipality');
            dropdownPhysicalMunicipality.html('');
            dropdownPhysicalMunicipality.append($('<option></option>').val("").html("- Please Select -"));
            $.each(response, function (id, option) {
                dropdownPhysicalMunicipality.append($('<option></option>').val(option.id).html(option.name));
            });

            if (municipalityId != -1)
                dropdownPhysicalMunicipality.val(municipalityId);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert('Error populating Municipality dropdown');
        }
    });
}

function PopulateAndSelectPhysicalLocalMunicipalityList(municipalityId, localMunicipalityId) {
    $.ajax({
        url: $.url('/Intake/GetListOfLocalMunicipalities'),
        data: { "municipalityId": municipalityId },
        type: "GET",
        success: function (response, status, xhr) {
            var dropdownPhysicalLocalMunicipality = $('#DropdownPhysicalAddressLocalMunicipality');
            dropdownPhysicalLocalMunicipality.html('');
            dropdownPhysicalLocalMunicipality.append($('<option></option>').val("").html("- Please Select -"));
            $.each(response, function (id, option) {
                dropdownPhysicalLocalMunicipality.append($('<option></option>').val(option.id).html(option.name));
            });
            if (localMunicipalityId != -1)
                dropdownPhysicalLocalMunicipality.val(localMunicipalityId);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert('Error populating Local Municipality dropdown');
        }
    });
}

function PopulatePhysicalTownList() {
    var provinceId = $("#DropdownPhysicalAddressProvince").val();
    var municipalityId = $("#DropdownPhysicalAddressMunicipality").val();
    var localMunicipalityId = $("#DropdownPhysicalAddressLocalMunicipality").val();
    $.ajax({
        url: $.url('/Intake/FilterTown'),
        data: { "provinceIdString": provinceId, "municipalityIdString": municipalityId, "localMunicipalityIdString": localMunicipalityId },
        type: "GET",
        success: function (response, status, xhr) {
            var dropdownPhysicalTown = $('#DropdownPhysicalAddressTown');
            dropdownPhysicalTown.html('');
            dropdownPhysicalTown.append($('<option></option>').val("").html("- Please Select -"));
            $.each(response, function (id, option) {
                dropdownPhysicalTown.append($('<option></option>').val(option.id).html(option.name));
            });
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert('Error populating town dropdown');
        }
    });
}

// Postal Address items
function PopulateAndSelectPostalProvinceList(provinceId) {
    $.ajax({
        url: $.url('/Intake/GetListOfProvinces'),
        type: "GET",
        success: function (response, status, xhr) {
            var dropdownPostalProvince = $('#DropdownPostalAddressProvince');
            dropdownPostalProvince.html('');
            dropdownPostalProvince.append($('<option></option>').val("").html("- Please Select -"));
            $.each(response, function (id, option) {
                dropdownPostalProvince.append($('<option></option>').val(option.id).html(option.name));
            });
            if (provinceId != -1)
                dropdownPostalProvince.val(provinceId);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert('Error populating Province dropdown');
        }
    });
}

function PopulateAndSelectPostalMunicipalityList(provinceId, municipalityId) {
    $.ajax({
        url: $.url('/Intake/GetListOfMunicipalities'),
        data: { "provinceId": provinceId },
        type: "GET",
        success: function (response, status, xhr) {
            var dropdownPostalMunicipality = $('#DropdownPostalAddressMunicipality');
            dropdownPostalMunicipality.html('');
            dropdownPostalMunicipality.append($('<option></option>').val("").html("- Please Select -"));
            $.each(response, function (id, option) {
                dropdownPostalMunicipality.append($('<option></option>').val(option.id).html(option.name));
            });
            if (municipalityId != -1)
                dropdownPostalMunicipality.val(municipalityId);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert('Error populating Municipality dropdown');
        }
    });
}

function PopulateAndSelectPostalLocalMunicipalityList(municipalityId, localMunicipalityId) {
    $.ajax({
        url: $.url('/Intake/GetListOfLocalMunicipalities'),
        data: { "municipalityId": municipalityId },
        type: "GET",
        success: function (response, status, xhr) {
            var dropdownPostalLocalMunicipality = $('#DropdownPostalAddressLocalMunicipality');
            dropdownPostalLocalMunicipality.html('');
            dropdownPostalLocalMunicipality.append($('<option></option>').val("").html("- Please Select -"));
            $.each(response, function (id, option) {
                dropdownPostalLocalMunicipality.append($('<option></option>').val(option.id).html(option.name));
            });
            if (localMunicipalityId != -1)
                dropdownPostalLocalMunicipality.val(localMunicipalityId);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert('Error populating Local Municipality dropdown');
        }
    });
}

function PopulatePostalTownList() {
    var provinceId = $("#DropdownPostalAddressProvince").val();
    var municipalityId = $("#DropdownPostalAddressMunicipality").val();
    var localMunicipalityId = $("#DropdownPostalAddressLocalMunicipality").val();
    $.ajax({
        url: $.url('/Intake/FilterTown'),
        data: { "provinceIdString": provinceId, "municipalityIdString": municipalityId, "localMunicipalityIdString": localMunicipalityId },
        type: "GET",
        success: function (response, status, xhr) {
            var dropdownPostalTown = $('#DropdownPostalAddressTown');
            dropdownPostalTown.html('');
            dropdownPostalTown.append($('<option></option>').val("").html("- Please Select -"));
            $.each(response, function (id, option) {
                dropdownPostalTown.append($('<option></option>').val(option.id).html(option.name));
            });
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert('Error populating town dropdown');
        }
    });
}

// Edit Relation Item
function EditRelationDetails() {
    var personId = $('#Person_Id').val();

    // Build Physical Address Object
    var physicalAddress = {
        Address_Type_Id: $('#Popup_Physical_Address_Type_Id').val(),
        Address_Line_1: $('#Popup_Physical_Address_Line_1').val().trim(),
        Address_Line_2: $('#Popup_Physical_Address_Line_1').val().trim(),
        Town_Id: $('#Popup_Physical_Address_Town_Id').val(),
        Postal_Code: $('#Popup_Physical_Address_Postal_Code').val().trim()
    };

    // Build Postal Address Object
    var postalAddress = {
        Address_Type_Id: $('#Popup_Postal_Address_Type_Id').val(),
        Address_Line_1: $('#Popup_Postal_Address_Line_1').val().trim(),
        Address_Line_2: $('#Popup_Postal_Address_Line_2').val().trim(),
        Town_Id: $('#Popup_Postal_Address_Town_Id').val(),
        Postal_Code: $('#Popup_Postal_Address_Postal_Code').val().trim()
    };

    // Build Person Object
    var relatedPerson = {
        Person_Id: $('#Popup_Related_Person_Id').val(),
        First_Name: $('#Popup_First_Name').val().trim(),
        Last_Name: $('#Popup_Last_Name').val().trim(),
        Known_As: $('#Popup_Known_As').val().trim(),
        Identification_Type_Id: $('#DropdownIdentificationType_Popup').val(),
        Identification_Number: $('#Identification_Number_Popup').val().trim(),
        Date_Of_Birth: $('#Date_of_Birth_Popup').val(),
        Age: $('#TextboxAge_Popup').val().trim(),
        Is_Estimated_Age: $('#IsEstimatedAge_Popup').val().trim(),
        Gender_Id: $('#DropdownGender_Popup').val(),
        Nationality_Id: $('#Nationality_Id').val(),
        Marital_Status_Id: $('#DropdownMaritalStatus_Popup').val(),
        Population_Group_Id: $('#DropdownPopulationGroup_Popup').val(),
        Religion_Id: $('#DropdownReligion_Popup').val(),
        Disability_Id: $('#DropdownDisability_Popup').val(),
        Phone_Number: $('#Popup_Phone_Number').val().trim(),
        Mobile_Phone_Number: $('#Popup_Mobile_Phone_Number').val().trim(),
        Email_Address: $('#Popup_Email_Address').val().trim(),
        Preferred_Contact_Type_Id: $('#Popup_Preferred_Contact_Type').val(),
    };

    // Combine
    var personDetail = {
        Person: relatedPerson,
        PhysicalAddress: physicalAddress,
        PostalAddress: postalAddress
    };

    // Build Relation Object
    var intakeRelationItem = {
        Item_Id: $('#Relation_Item_Id').val(),
        Person_Id: $('#Person_Id').val(),
        Relation_Type_Id: $('#Edited_Relation_Type_Id').val(),
        CreatePerson: personDetail,
        IsDeceased: $('#Is_Deceased').val(),
        DateDeceased: $('#Date_Deceased').val().trim(),
        Person_Relationship_Type_Id: $('#Person_Relationship_Type_Id').val()
    };

    // Add Validation Token
    intakeRelationItem.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();

    // Save Relation Details
    $.ajax({
        url: $.url('/Relation/Edit'),
        type: 'POST',
        data: intakeRelationItem,
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                $('#modal-container').modal('hide');
                $('#Relation_Type_Id').val('');
            }

            LoadRelations(personId, "-1");
        },
        error: function () {
            $('#msg').html('<div class="failed">Error! Please try again.</div>');
        }
    });
}