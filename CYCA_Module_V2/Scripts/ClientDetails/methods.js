$(document).ready(function () {

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

    //// Initialize jQuery validation
    //$("#personDetailsForm").validate({
    //    rules: {
    //        Identification_Number: {
    //            idNumberValidation: true
    //        }
    //    }
    //});
    
    // Initialize jQuery validation
    $("#personDetailsForm").validate();

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

    // Copy ID Number to PIVA tab if set correct
    CheckAndCopyId();

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
            // Transfer ID Number to PIVA functionality because we need it there too
            $("#txtIdNr").val($(this).val());

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

function CheckAndCopyId() {
    var idType = $("#DropdownIdentificationType").val();

    if ((idType == '1') && ($("#Identification_Number").val() !== '')) {
        $("#txtIdNr").val($("#Identification_Number").val());
    }
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

// Validation check to stop the wizard tabs from switching if there's a validation error
function validateForm() {
    var $valid = $("#personDetailsForm").valid();
     
    if (!$valid) {
        return false;
    }

    return true;
}