
var request = { signalReset: true, fingerprintSet: 1, codecName: "9", name: "FPCaptureRq" };

//    var HubProxy = $.connection.FingerprintServerHub;
//    if (HubProxy == null) {
//        signalHub.IsConnected = false;

//    } else {
//        HubProxy.client.ServerResponse = function (data) {
//            console.log(data);
//        };
//        $.connection.hub.disconnected(function () {
//            signalHub.IsConnected = false;
//            setTimeout(function () {
//                $.connection.hub.start();
//            }, 5000); // Restart connection after 5 seconds.
//        });
//        $.connection.hub.url = 'http://127.0.0.1:9090/signalr/hubs';
//        $.connection.hub.start()
//            .done(function () {
//                console.log('Now connected, connection ID=' + $.connection.hub.id);
//                signalHub.IsConnected = true;
//            })
//            .fail(function () { console.log('Could not Connect!'); });
//    }
////http://127.0.0.1:9002/hubs/afis/signalr/hubs


var connection1 = $.hubConnection("http://127.0.0.1:9090/signalr/hubs");

//var connection2 = $.hubConnection("http://10.250.0.145:9002/hubs/afis/signalr/hubs");
//var connection2 = $.hubConnection("http://127.0.0.1:9002/hubs/afis/signalr/hubs");
var FingerPrintServerProxy;
var AfisServerProxy;
var CurrentAFISGuid = '00000000-0000-0000-0000-000000000000';
var fingerprintRecord = {};
signalHub.IsConnected = false;
console.log(signalHub.IsConnected);
if (connection1 == null) {
    signalHub.IsConnected = false;
} else {
    FingerPrintServerProxy = connection1.createHubProxy('FingerprintServerHub');
    //AfisServerProxy = connection2.createHubProxy('AfisHub');
    //Callback Client Methods
    FingerPrintServerProxy.on('ServerResponse', function (data) {
        console.log(data);
        
        
        switch (data.requestType) {
            case "Enroll":
                data.PersonId = currentPersonId;
                data.UuId = CurrentAFISGuid;
                $.ajax({
                    type: 'POST',
                    url: '/Afis/AfisEntry',
                    data: {
                        vm: data
                    },
                    success: function (data) {
                        var errorCode = parseInt(data.ErrorCode);
                        if (errorCode > 0) {
                            CreateError(data.ErrorCode, data.Error, data.ErrorLink);
                            if ($('#btnEnroll').length) {
                                $('#btnEnroll').prop('disabled', false);
                            }
                            if ($('#btnEnrollBiometric').length) {
                                $('#btnEnrollBiometric').prop('disabled', false);
                            }
                        } else {
                            if ($('#btnEnroll').length) {
                                $('#btnEnroll').hide();
                                $('#btnEnroll').prop('disabled', false);
                            }
                            if ($('#btnEnrollBiometric').length) {
                                $('#btnEnrollBiometric').prop('disabled', false);
                                AjaxShowTabs(currentPersonId, currentTab, currentFormType, currentTarget);
                            }
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        CreateError('100', "There was an error enrolling the fingerprints", "");
                        if ($('#btnEnroll').length) {
                            $('#btnEnroll').prop('disabled', false);
                        }
                        if ($('#btnEnrollBiometric').length) {
                            $('#btnEnrollBiometric').prop('disabled', false);
                        }
                    }
                });
                break;
            case "Identify":
                $.ajax({
                    type: 'POST',
                    url: '/Afis/AfisEntry',
                    data: {
                        vm: data
                    },
                    success: function (data) {
                        console.log(data);
                        var errorCode = parseInt(data.ErrorCode);
                        if (errorCode > 0) {
                            CreateError(data.ErrorCode, data.Error, data.ErrorLink);
                        } else {
                            if (data.FingerprintRecords.length > 0) {
                                //Record Found
                                SendGuidToServer(data.FingerprintRecords[0].Uuid);
                            } else {
                                //Nothing Found
                                $('table.NestedMainGrid tbody').hide();
                                $(".noRecords").html("No Biometric Records Found");
                            }
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        CreateError('100', "There was an error identifying the fingerprints", "");
                    }
                });
                break;
        }
    });
    //AfisServerProxy.on('EnrollResponse', function (data) {
    //    if (data.fingerprintRecords.length > 0) {
    //        $.ajax({
    //            type: 'POST',
    //            url:  '/Admit/Enroll',
    //            data: {
    //                Id: data.fingerprintRecords[0].uuid,
    //                PersonId: currentPersonId
    //            },
    //            success: function () {
    //                //Change Form
    //                console.log('successEnrollment');
    //                if ($('#btnEnroll').length) {
    //                    $('#btnEnroll').hide();
    //                } 
                    
    //                if ($('#btnEnrollBiometric').length) {
    //                    console.log(currentPersonId);
    //                    console.log(currentTab);
    //                    console.log(currentFormType);
    //                    console.log(currentId);

    //                    AjaxShowTabs(currentPersonId, currentTab, currentFormType, currentId);
    //                }
    //                //$('#HasBiometric').prop('checked', true);
    //                //If biometric tab loaded we need to refresh it
    //            }
    //        });
    //    }

    //});
    //AfisServerProxy.on('DisenrollResponse', function (data) {
    //    if (data.errorCode === 0) {
    //        $.ajax({
    //            type: 'POST',
    //            url: '/Admit/DisEnroll',
    //            data: {
    //                Id: data.fingerprintRecords[0].uuid
    //            },
    //            success: function () {
    //                AjaxShowTabs(currentPersonId, currentTab, currentFormType, currentId);
    //            }
    //        });
    //    } else {
    //        //Error
    //        $.unblockUI();
    //    }
    //});
    //AfisServerProxy.on('VerifyResponse', function (data) {
    //    console.log(data);
    //});
    //AfisServerProxy.on('IdentifyResponse', function (data) {
    //    if (data.fingerprintRecords.length > 0) {
    //        //Record Found
    //        SendGuidToServer(data.fingerprintRecords[0].uuid);
    //    } else {
    //        //Nothing Found
    //        $(".noRecords").html("No Biometric Records Found");
    //    }
    //});
    connection1.start()
        .done(function ()
        {
            //signalHub.IsConnected = true;
            console.log('Now connected 1:, connection ID=' + connection1.id);
            $('#ClientStatus').html('Client Connected');
            signalHub.IsConnected = true;
            //connection2.start()
            //    .done(function () {
            //        console.log('Now connected 2:, connection ID=' + connection2.id);
            //        signalHub.IsConnected = true;
            //        console.log(signalHub.IsConnected);
            //        $('#ServerStatus').html('Server Connected');
                    

            //    })
            //    .fail(function () {
            //        signalHub.IsConnected = false;
            //        console.log('Could not connect');
            //        $('#ServerStatus').html('Server Disconnected');
            //    });
        })
        .fail(function () {
            signalHub.IsConnected = false;
            $('#ClientStatus').html('Client Disconnected');
            console.log('Could not connect');
        });
    
}
function BuildIdentifyObject(data) {
    var jsonObj = [];
    count = 0;
    $.each(data.fingers, function (index, value) {
        if (count === 0) {
            //Any holder
            jsonObj.push(null);
        }
        if (value.sequence === 0) {
            jsonObj.push(null);
        } else {
            // do your stuff here
            item = {};
            item["Id"] = 0;
            item["RecordId"] = 0;
            switch (value.code) {
                case 1:
                    item["Code"] = "RightThumb";
                    break;
                case 6:
                    item["Code"] = "LeftThumb";
                    break;

            }
            item["EncodedImage"] = value.print;
            item["DecodedImage"] = null;
            item["EncodedTemplate"] = null;
            item["DecodedTemplate"] = null;
            item["EncodedKey"] = null;

            jsonObj.push(item);
        }
        count++;
    });
    var fingerprintSet = {};
    fingerprintSet["Id"] = 0;
    fingerprintSet["Fingerprints"] = jsonObj;
    fingerprintSet["ImageEncoding"] = data.codecName;
    fingerprintSet["Dpi"] = 500;
    fingerprintSet["ImageWidth"] = data.printSize.width;
    fingerprintSet["ImageHeight"] = data.printSize.height;
    // fingerprintRecord = {};
    var fingerprintSetHeader = {};
    fingerprintSetHeader["FingerprintSet"] = fingerprintSet;
    fingerprintSetHeader["Id"] = 0;
    fingerprintSetHeader["Uuid"] = "00000000-0000-0000-0000-000000000000";
    fingerprintSetHeader["DateTime"] = "2020-03-25T23:33:11.4704985+02:00";
    fingerprintSetHeader["Active"] = false;
    fingerprintSetHeader["Person"] = null;


    fingerprintRecord["Action"] = "Identify";
    fingerprintRecord["FingerprintRecord"] = fingerprintSetHeader;
    fingerprintRecord["Name"] = "IdentifyRequest";
    fingerprintRecord["Id"] = "5fe4f984-a63d-4745-a47c-7151b6ccb9f8";
    
}
function BuildDisEnrollObject() {
    console.log(CurrentAFISGuid);
    var jsonObj = [];
    count = 0;
    for (i = 0; i < 10; i++) {
        jsonObj.push(null);
    }
    
    var fingerprintSet = {};
    fingerprintSet["Id"] = 0;
    fingerprintSet["Fingerprints"] = jsonObj;
    fingerprintSet["ImageEncoding"] = null;
    fingerprintSet["Dpi"] = 0;
    fingerprintSet["ImageWidth"] = 0;
    fingerprintSet["ImageHeight"] = 0;
    // fingerprintRecord = {};
    var fingerprintSetHeader = {};
    fingerprintSetHeader["FingerprintSet"] = fingerprintSet;
    fingerprintSetHeader["Id"] = 0;
    fingerprintSetHeader["Uuid"] = CurrentAFISGuid;
    fingerprintSetHeader["DateTime"] = "2020-03-25T23:33:11.4704985+02:00";
    fingerprintSetHeader["Active"] = false;
    fingerprintSetHeader["Person"] = null;


    fingerprintRecord["Action"] = "Disenroll";
    fingerprintRecord["FingerprintRecord"] = fingerprintSetHeader;
    fingerprintRecord["Name"] = "DisenrollRequest";
    fingerprintRecord["Id"] = CurrentAFISGuid;

}
function BuildEnrollObject(data, gender) {
    var jsonObj = [];
    count = 0;
    $.each(data.fingers, function (index, value) {
        if (count === 0) {
            //Any holder
            jsonObj.push(null);
        }
        if (value.sequence === 0) {
            jsonObj.push(null);
        } else {
            // do your stuff here
            item = {};
            item["Id"] = 0;
            item["RecordId"] = 0;
            switch (value.code) {
                case 1:
                    item["Code"] = "RightThumb";
                    break;
                case 6:
                    item["Code"] = "LeftThumb";
                    break;

            }
            item["EncodedImage"] = value.print;
            item["DecodedImage"] = null;
            item["EncodedTemplate"] = null;
            item["DecodedTemplate"] = null;
            item["EncodedKey"] = null;

            jsonObj.push(item);
        }
        count++;
    });
    var today = new Date();
    var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
    var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
    var dateTime = date + ' ' + time;

    var fingerprintSet = {};
    fingerprintSet["Id"] = 0;
    fingerprintSet["Fingerprints"] = jsonObj;
    fingerprintSet["ImageEncoding"] = data.codecName;
    fingerprintSet["Dpi"] = 500;
    fingerprintSet["ImageWidth"] = data.printSize.width;
    fingerprintSet["ImageHeight"] = data.printSize.height;
    // fingerprintRecord = {};
    var fingerprintSetHeader = {};
    fingerprintSetHeader["FingerprintSet"] = fingerprintSet;
    fingerprintSetHeader["Id"] = 0;
    fingerprintSetHeader["Uuid"] = "00000000-0000-0000-0000-000000000000";
    fingerprintSetHeader["DateTime"] = dateTime;
    fingerprintSetHeader["Active"] = false;
    fingerprintSetGender = {};
    fingerprintSetGender["Gender"] = gender;
    fingerprintSetHeader["Person"] = fingerprintSetGender;
    fingerprintRecord["Action"] = "Enroll";
    fingerprintRecord["FingerprintRecord"] = fingerprintSetHeader;
    fingerprintRecord["Name"] = "IdentifyRequest";
    fingerprintRecord["Id"] = "5fe4f984-a63d-4745-a47c-7151b6ccb9f8";
}
function CreateError(errorCode, errorText, errorLink) {
    if (errorCode === 99) {
        $.confirm({
            title: 'Biometrics already enrolled!',
            content: errorText,
            type: 'red',
            typeAnimated: true,
            buttons: {
                tryAgain: {
                    text: 'View Profile',
                    btnClass: 'btn-red',
                    action: function () {
                        window.location.href = errorLink;
                    }
                },
                close: function () {
                }
            }
        });
    } else {
        $.confirm({
            title: 'Encountered an error!',
            content: errorText,
            type: 'red',
            typeAnimated: true,
        });
    }
}






