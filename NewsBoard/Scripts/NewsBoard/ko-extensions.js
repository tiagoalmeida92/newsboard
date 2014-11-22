﻿ko.bindingHandlers.date = {
    init: function(element, valueAccessor, allBindingsAccessor, viewModel) {
        var jsonDate = valueAccessor();
        var date = new Date(jsonDate);
        var res = date.toLocaleDateString() + ' - ' + date.toLocaleTimeString().substring(0, 5);
        element.innerHTML = res;
    },
    update: function(element, valueAccessor, allBindingsAccessor, viewModel) {
    }
};
//http://stackoverflow.com/questions/16317622/knockout-limit-number-of-characters-in-an-observable-field
ko.bindingHandlers.trimLengthText = {};
ko.bindingHandlers.desc = {
    init: function(element, valueAccessor, allBindingsAccessor, viewModel) {
        var trimmedText = ko.computed(function() {
            var untrimmedText = ko.utils.unwrapObservable(valueAccessor());
            var defaultMaxLength = 200;
            var minLength = 5;
            var maxLength = ko.utils.unwrapObservable(allBindingsAccessor().trimTextLength) || defaultMaxLength;
            if (maxLength < minLength) maxLength = minLength;
            var text = untrimmedText.length > maxLength ? untrimmedText.substring(0, maxLength - 1) + '...' : untrimmedText;
            return text;
        });
        ko.applyBindingsToNode(element, {
            text: trimmedText
        }, viewModel);

        return {
            controlsDescendantBindings: true
        };
    }
};