var orderViewModel = function (order) {
    var self = this;

    self.Email = ko.observable(order.Email);
    self.FirstName = ko.observable(order.FirstName);
    self.LastName = ko.observable(order.LastName);
    self.FullName = ko.computed(function () {
        return self.FirstName() + " " + self.LastName();
    }, this);

    self.Notes = order.Notes;
    self.editNoteUrl = function (id) {
        return "/ordernotes/edit/" + id;
    }
    self.deleteNote = function (note) {
        alert("going to delete note " + note.Id);
    };
    self.parseDate = function (jsonDate) {
        var re = /-?\d+/;
        var m = re.exec(jsonDate);
        var d = new Date(parseInt(m[0]));
        var curr_date = d.getDate();
        var curr_month = d.getMonth() + 1; //Months are zero based
        var curr_year = d.getFullYear();
        return curr_month + "-" + curr_date + "-" + curr_year + " " + d.getHours() + ":" + d.getMinutes();
    };


    self.saveOrder = function () {

        var data = $("#orderForm").serialize();
        $.post("/orders/edit/" + order.OrderId, data, function (result) {
            if (result.success) {
                notifier.success(result.message);
            } else {
                notifier.alert(result.message);
            }
        });
    };
};
