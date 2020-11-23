var generateLuhnDigit = function (inputString) {
    var total = 0;
    var count = 0;
    for (var i = 0; i < inputString.length; i++) {
        var multiple = count % 2 + 1;
        count++;
        var temp = multiple * +inputString[i];
        temp = Math.floor(temp / 10) + (temp % 10);
        total += temp;
    }

    total = (total * 9) % 10;

    return total;
};

var extractFromID = function (idNumber) {
    var checkIDNumber = function (idNumber) {
        var number = idNumber.substring(0, idNumber.length - 1);
        return generateLuhnDigit(number) === +idNumber[idNumber.length - 1];
    };

    var getBirthdate = function (idNumber) {
        var year = idNumber.substring(0, 2);
        var currentYear = new Date().getFullYear() % 100;

        var prefix = "19";
        if (+year < currentYear)
            prefix = "20";

        var month = idNumber.substring(2, 4);
        var day = idNumber.substring(4, 6);
        return new Date(prefix + year + "/" + month + "/" + day);
    };

    var getGender = function (idNumber) {
        return +idNumber.substring(6, 7) < 5 ? "female" : "male";
    };

    var getCitizenship = function (idNumber) {
        return +idNumber.substring(10, 11) === 0 ? "citizen" : "resident";
    };

    var result = {};
    result.valid = checkIDNumber(idNumber);
    result.birthdate = getBirthdate(idNumber);
    result.gender = getGender(idNumber);
    result.citizen = getCitizenship(idNumber);
    return result;
};