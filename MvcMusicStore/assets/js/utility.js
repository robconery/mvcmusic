MusicStore.Utility = function () {

    var _parseDate = function (jsonDate) {
        var re = /-?\d+/;
        var m = re.exec(jsonDate);
        var d = new Date(parseInt(m[0]));
        var curr_date = d.getDate();
        var curr_month = d.getMonth() + 1; //Months are zero based
        var curr_year = d.getFullYear();
        return curr_month + "-" + curr_date + "-" + curr_year + " " + d.getHours() + ":" + d.getMinutes();
    };

    return {
        parseDate : _parseDate
    }

}();
