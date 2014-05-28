
//
// Knockout extensions by Matthias Jenzen, 2014 
//

//
// Converts all model properties into observable properties (i. e. functions), like WPF properties implementing the "INotifyPropertyChanged" interface
//
$.extend(true, ko,
    {
        applyBindingsAsObservable: function (model, containerID, initFunction) {

            var container = $("#" + containerID);

            for (var property in model) {
                var propertyName = property.toString();
                var propertyControl = container.find("#" + propertyName);
                if (propertyControl != null && typeof (propertyControl.attr('id')) !== 'undefined') {
                    var propertyValue = propertyControl.val().toString();
                    var isCheckbox = propertyValue == 'false' || propertyValue == 'true';
                    var propertyValueSetFunction = 'val()';
                    if (isCheckbox) {
                        propertyValueSetFunction = 'is(":checked")';
                    }
                    eval('model.' + propertyName + ' = ko.observable($("#' + propertyName + '").' + propertyValueSetFunction + ')');
                    eval('model.' + propertyName + 'Property = $("#' + propertyName + '").' + propertyValueSetFunction + '');
                }
            }

            if (initFunction != null)
                initFunction();

            ko.applyBindings(model, container[0]);

        }
    });

//
// UniformJS is not watching internal checkbox changes.
// http://stackoverflow.com/questions/19148647/stying-form-elements-with-uniform-js-within-knockout-templates
// http://jsfiddle.net/nemesv/KvTzG/
//
ko.bindingHandlers.checkedUniform = {
    init: function (element, valueAccessor) {
        ko.bindingHandlers.checked.init(element, valueAccessor);
        $(element).uniform();
    },
    update: function (element, valueAccessor) {
        ko.bindingHandlers.checked.update(element, valueAccessor);
        $.uniform.update($(element));
    }
};

//
// This binding allows you set an arbitrary css class for an element. 
// https://github.com/knockout/knockout/wiki/Bindings---class
//
ko.bindingHandlers.class = {
    update: function (element, valueAccessor) {
        if (element['__ko__previousClassValue__']) {
            $(element).removeClass(element['__ko__previousClassValue__']);
        }
        var value = ko.utils.unwrapObservable(valueAccessor());
        $(element).addClass(value);
        element['__ko__previousClassValue__'] = value;
    }
};