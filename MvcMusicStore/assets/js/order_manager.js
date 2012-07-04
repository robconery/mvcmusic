var OrderManager = function (order) {
    var self = this;
    self.OrderId = ko.observable(order.OrderId);
    self.Email = ko.observable(order.Email);
    self.FirstName = ko.observable(order.FirstName);
    self.LastName = ko.observable(order.LastName);
    self.Status = ko.observable(order.Status);
    self.FullName = ko.computed(function () {
        return self.FirstName() + " " + self.LastName();
    }, this);

    self.Notes = ko.observableArray(order.Notes);
    self.Transactions = ko.observableArray(order.Transactions);
    self.editNoteUrl = function (id) {
        return "/ordernotes/edit/" + id;
    }
    self.deleteNote = function (note) {
        MusicStore.deleteNote(note, function (result) {
            if (result.success)
                self.Notes.remove(note);
        });
    };
    self.canRefund = ko.computed(function () {
        return self.Status() === "paid";
    });

    self.refundOrder = function () {
        return MusicStore.refundOrder(order, function (result) {
            //load the status
            //transactions
            //notes
            self.Status(result.order.Status);
            self.Transactions(result.order.Transactions);
            self.Notes(result.order.Notes);
        });
    };

    self.addNote = function () {
        var note = $("#newNote").val();
        var newNote = { OrderId: self.OrderId(), Note: note };
        MusicStore.addNote(newNote, function (result) {
            console.log(result);
            if (result.success)
                self.Notes.push(result.newNote);
        });
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

MusicStore = function () {

    var _notify = function (result) {
        if (result.message) {
            if (result.success) {
                notifier.success(result.message);
            } else {
                notifier.error(result.message);
            }
        }
    };

    var _canRefund = function (order) {
        return order.Status === "paid";
    };
    var _refundOrder = function (order, callback) {
        $.post("/orders/refund/", { id: order.OrderId }, function (result) {
            _notify(result);
            callback(result);
        });
    };
    var _deleteNote = function (note, callback) {
        $.post("/ordernotes/delete/", { id: note.Id }, function (result) {
            _notify(result);
            callback(result);
        });
    };
    var _addNote = function (newNote, callback) {
        $.post("/ordernotes/create/", newNote, function (result) {
            _notify(result);
            callback(result);
        });
    };

    return {
        deleteNote: _deleteNote,
        addNote: _addNote,
        canRefund: _canRefund,
        refundOrder : _refundOrder
    }

} ();

var loadOrder = function () {
    orderManager = new OrderManager(window.order);
    ko.applyBindings(orderManager);
 
};

$().ready(function () {
    loadOrder();
});
