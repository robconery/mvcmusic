MusicStore.Order = function () {

    var _saveOrder = function (order,callback) {
        $.post("/orders/edit", order, callback);
    };
    var _deleteNote = function (note, callback) {
        $.post("/ordernotes/delete", { id: note.Id }, callback);
    };
    var _saveNote = function (note, callback) {
        $.post("/ordernotes/edit", note, callback);
    };
    var _addNote = function (note, orderId, callback) {
        $.post("/ordernotes/create", { Note: note, OrderId: orderId }, callback);
    };
    var _ship = function (id, callback) {
        $.post("/orders/ship/", { id: id }, callback);
    }
    var _refund = function (id, callback) {
        $.post("/orders/refund/", { id: id }, callback);
    };

    return {
        saveOrder : _saveOrder,
        deleteNote: _deleteNote,
        saveNote: _saveNote,
        addNote: _addNote,
        ship: _ship,
        refund : _refund
    }

}();

