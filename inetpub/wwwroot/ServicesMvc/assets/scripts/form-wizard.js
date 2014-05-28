var FormWizard = function () {


    return {
        //main function to initiate the module
        init: function () {
            if (!jQuery().bootstrapWizard) {
                return;
            }

            // default form wizard
            $('#form_wizard_1').bootstrapWizard({
                'nextSelector': '.button-next',
                'previousSelector': '.button-previous',
                onTabClick: function (tab, navigation, index) {
                    //alert('on tab click disabled');
                    return false;
                },
                onNext: function (tab, navigation, index) {
                    var total = navigation.find('li').length;
                    var current = index + 1;
                    // set wizard title
                    $('.step-title', $('#form_wizard_1')).text('Schritt ' + (index + 1) + ' / ' + total);
                    // set done steps
                    jQuery('li', $('#form_wizard_1')).removeClass("done");
                    var liList = navigation.find('li');
                    for (var i = 0; i < index; i++) {
                        jQuery(liList[i]).addClass("done");
                    }

                    FormWizardNavButtonsShowHide(current, total);
                    
                    // <customized MJE>

                    if (_formWizardForceHideSubmitButton)
                        $('#form_wizard_1').find('.button-submit').hide();

                    // </customized MJE>

                    //App.scrollTo($('.page-title'));
                },
                onPrevious: function (tab, navigation, index) {
                    var total = navigation.find('li').length;
                    var current = index + 1;
                    // set wizard title
                    $('.step-title', $('#form_wizard_1')).text('Schritt ' + (index + 1) + ' / ' + total);
                    // set done steps
                    jQuery('li', $('#form_wizard_1')).removeClass("done");
                    var li_list = navigation.find('li');
                    for (var i = 0; i < index; i++) {
                        jQuery(li_list[i]).addClass("done");
                    }

                    FormWizardNavButtonsShowHide(current, total);

                    //App.scrollTo($('.page-title'));
                },
                onTabShow: function (tab, navigation, index) {
                    var total = navigation.find('li').length;
                    var current = index + 1;
                    var $percent = (current / total) * 100;
                    $('#form_wizard_1').find('.bar').css({
                        width: $percent + '%'
                    });
                }
            });

            $('#form_wizard_1').find('.form-actions').hide();
            //$('#form_wizard_1').find('.button-previous').hide();

            //            $('#form_wizard_1 .button-submit').click(function () {
            //                //alert('Finished! Hope you like it :)');
            //            }).hide();
        }

    };

} ();

function FormWizardNavButtonsShowHide(current, total) {

    $('#form_wizard_1').find('.form-actions').show();
    
    if (current == 1) {
        $('#form_wizard_1').find('.form-actions').hide();
        $('#form_wizard_1').find('.button-previous').hide();
    } else
        if (current == total) {
            $('#form_wizard_1').find('.button-previous').hide();
            $('#form_wizard_1').find('.button-next').hide();
            $('#form_wizard_1').find('.button-submit').hide();
            $('#form_wizard_1').find('.button-restart').show();
        } else
            if (current == total - 1) {
                $('#form_wizard_1').find('.button-previous').show();
                $('#form_wizard_1').find('.button-next').hide();
                $('#form_wizard_1').find('.button-submit').show();
                $('#form_wizard_1').find('.button-restart').hide();
            } else {
                $('#form_wizard_1').find('.button-previous').show();
                $('#form_wizard_1').find('.button-next').show();
                $('#form_wizard_1').find('.button-submit').hide();
                $('#form_wizard_1').find('.button-restart').hide();
            }    
}