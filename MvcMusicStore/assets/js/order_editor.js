MusicStore.OrderEditor = {
    Start : function(order){
        MusicStore.KnockoutBindings.Init();
        var viewModel = MusicStore.OrderEditor.ViewModel(order);
        MusicStore.OrderEditor.WireEvents(viewModel);
        ko.applyBindings(viewModel);
    }
};

MusicStore.OrderEditor.ViewModel = function (order) {
    var self = this;
    var model = ko.mapping.fromJS(order);

    model.FullName = ko.computed(function () {
        return model.FirstName() + " " + model.LastName();
    });

    model.shippable = ko.computed(function () {
        return model.Status() === "paid";
    });
    model.refundable = ko.computed(function () {
        return model.Status() === "paid";
    });
    model.deleteNote = function (note) {
        MusicStore.Order.deleteNote(note, function (result) {
            if (result.success) {
                model.Notes.remove(note);
            }
            model.handleResult(result);
        });
    };
    model.addNote = function (noteBody) {
        MusicStore.Order.addNote(noteBody, order.OrderId, function (result) {
            if (result.success) {
                model.Notes(result.notes);
            }
            model.handleResult(result);
        });
    };
    model.saveNote = function (note) {
        MusicStore.Order.saveNote(note, function (result) {
            //update notes list?
            model.Notes(result.notes);
            model.handleResult(result);
        });
    };

    model.saveOrder = function () {
        var data = $("#orderForm").serialize();
        MusicStore.Order.saveOrder(data, function (result) {
            model.handleResult(result);
        });
    };

    model.refundOrder = function (evt) {
        evt.preventDefault();
        MusicStore.Order.refund(order.OrderId, function (result) {
            if(result.success){
                model.handleResult(result);
                model.Status(result.order.Status);
                model.Notes(result.order.Notes);
                model.Transactions(result.order.Transactions);
            }
        });
    };
    model.shipOrder = function (evt) {
        evt.preventDefault();
        MusicStore.Order.ship(order.OrderId, function (result) {
            if (result.success) {
                model.handleResult(result);
                model.Status(result.order.Status);
                model.Notes(result.order.Notes);
            }
        });
    };
    model.handleResult = function (result) {
        if (result.success) {
            $(".well").effect("highlight");
        }else{
            notifier.error(result.message);
        }
    };
    //events
    model.formSubmitted = function (evt) {
        evt.preventDefault();
        model.saveOrder();
    };

    model.noteDeleted = function () {
        if (confirm("Delete this note?")) {
            var note = ko.dataFor(this);
            model.deleteNote(note);
        }
    };
    model.noteSaved = function () {
        var note = ko.dataFor(this);
        model.saveNote(note);
    };
    model.noteSelected = function () {
        $(".modal", this).modal();
    };
    model.noteAdded = function () {
        var note = $("#newNote").val();
        model.addNote(note);
    };
    model.notesUpdated = function (notes) {
        $("#noteBox").effect("highlight");
    }
    return model;
}

MusicStore.OrderEditor.WireEvents = function (viewModel) {
    //wire events
    $("#orderForm").submit(viewModel.formSubmitted);
    $("#noteList").delegate(".noteDeleter", "click", viewModel.noteDeleted);
    $("#noteList").delegate(".noteSaver", "click", viewModel.noteSaved);
    $("#noteList").delegate(".editable", "dblclick", viewModel.noteSelected);
    $("#noteAdder").click(viewModel.noteAdded);
    $("#submenu").delegate("#refundButton", "click", viewModel.refundOrder);
    $("#submenu").delegate("#shipButton", "click", viewModel.shipOrder);

    //flash every time something changes
    viewModel.Notes.subscribe(viewModel.notesUpdated);


};
