MusicStore.KnockoutBindings =  {
    Init: function () {
        ko.bindingHandlers.dateText = {
            init: function (element, value) {
                var val = value();
                $(element).text(MusicStore.Utility.parseDate(val));
            }
        }
    }
}
