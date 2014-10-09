
/* Extensions by Matthias Jenzen, 2014 */

function FormDateRangePickerInit() {
    $(".FormDateRangePicker").each(function () {
        //alert($(this).attr("data-init"));
        eval($(this).attr("data-init"));
    });
}

function FormDateRangePickerPrepare(dateRangeProperty, onUseDateRangeChangeFunction, onDateRangeChangeFunction, dateRangeGroupsToExclude) {

    var checkbox = $("input[name='" + dateRangeProperty + ".IsSelected']");
    var dateStartControl = $("input[name='" + dateRangeProperty + ".StartDate']");
    var dateEndControl = $("input[name='" + dateRangeProperty + ".EndDate']");

    var dateFormat = 'dd.MM.yy';
    var dateFormatExact = 'dd.MM.yyyy'; // ' hh:mm:ss';

    if (dateStartControl.val().indexOf("/") != -1) {
        dateFormat = 'M/d/yy';
        dateFormatExact = 'M/d/yyyy'; // ' HH:mm:ss AM';
    }

    var wrapperId = '#wrapper-date-range-' + dateRangeProperty;
    var rangePickerId = '#rangePicker-date-range-' + dateRangeProperty;

    if (checkbox.val() === 'true')
        $(wrapperId).toggleClass('hide', !checkbox.is(':checked'));

    if (!jQuery().daterangepicker) {
        return;
    }

//    alert(dateStartControl.val().split(' ')[0]);
    var formDateRangeStart = Date.parseExact(dateStartControl.val().split(' ')[0], dateFormatExact);
    //alert(formDateRangeStart);
    var formDateRangeEnd = Date.parseExact(dateEndControl.val().split(' ')[0], dateFormatExact);
    //alert(dateEndControl.val().split(' ')[0]);
    $(rangePickerId + ' span').html(formDateRangeStart.toString(dateFormat) + ' - ' + formDateRangeEnd.toString(dateFormat));
    //alert(dateStartControl.val() + "    -     " + formDateRangeStart);


    checkbox.change(function () {
        //alert('change');
        if (onUseDateRangeChangeFunction != null && typeof (onUseDateRangeChangeFunction) !== 'undefined')
            onUseDateRangeChangeFunction($(this), formDateRangeStart, formDateRangeEnd);
        $(wrapperId).toggleClass('hide', !$(this).is(':checked'));
    });

    var ranges = {
        'Heute': ['today', 'today', ''],
        'Gestern': ['yesterday', 'yesterday', 'past'],
        'Letzte 7 Tage': [Date.today().add({ days: -7 }), 'today', 'past'],
        'Letzte 30 Tage': [Date.today().add({ days: -30 }), 'today', 'past'],
        'Dieser Monat': [Date.today().moveToFirstDayOfMonth(), Date.today().moveToLastDayOfMonth(), ''],
        'Nächste 30 Tage': ['today', Date.today().add({ days: 30 }), 'future'],
        'Nächster Monat': [Date.today().add({ months: 1 }).moveToFirstDayOfMonth(), Date.today().add({ months: 1 }).moveToLastDayOfMonth(), 'future'],
        'Letzter Monat': [Date.today().moveToFirstDayOfMonth().add({ months: -1 }), Date.today().moveToFirstDayOfMonth().add({ days: -1 }), 'past'],
        'Letzte 3 Monate': [Date.today().moveToFirstDayOfMonth().add({ months: -3 }), Date.today().moveToFirstDayOfMonth().add({ days: -1 }), 'past'],
        'Dieses Jahr': [Date.parseExact("01.01." + (Date.today().getFullYear()), "dd.MM.yyyy"), Date.parseExact("31.12." + (Date.today().getFullYear()), "dd.MM.yyyy"), ''],
        'Letztes Jahr': [Date.parseExact("01.01." + (Date.today().getFullYear() - 1), "dd.MM.yyyy"), Date.parseExact("31.12." + (Date.today().getFullYear() - 1), "dd.MM.yyyy"), 'past|far-past']
    };

    var rangesToSelect = {};
    if (typeof (dateRangeGroupsToExclude) !== 'undefined') {

        var dateRangeArrayToExclude = dateRangeGroupsToExclude.split(',');
        
        for (var i = 0; i < dateRangeArrayToExclude.length; i++)
            if (dateRangeArrayToExclude[i].substr(0, 1) == "!")
                dateRangeArrayToExclude[i] = dateRangeArrayToExclude[i].substr(1, 99);

        for (var range in ranges) {
            var excludeOption = ranges[range][2];
            //alert(range + ' - ' + excludeOption);
            //rangesToSelect.push(range);
            console.log(ranges[range]);
        }
    }

    $(rangePickerId).daterangepicker({
        ranges: rangesToSelect,
        opens: 'right',
        format: dateFormat,
        separator: ' - ',
        startDate: formDateRangeStart,
        endDate: formDateRangeEnd,
        minDate: '01.01.2012',
        maxDate: '31.12.2029',
        locale: {
            applyLabel: 'Übernehmen',
            fromLabel: 'Von',
            toLabel: 'Bis',
            customRangeLabel: 'Beliebiger Datumsbereich',
            daysOfWeek: ['So', 'Mo', 'Di', 'Mi', 'Do', 'Fr', 'Sa'],
            monthNames: ['Januar', 'Februar', 'März', 'April', 'Mai', 'Juni', 'Juli', 'August', 'September', 'Oktober', 'November', 'Dezember'],
            firstDay: 1
        },
        showWeekNumbers: true,
        buttonClasses: ['green']
    },

    function (start, end) {
        $(rangePickerId + ' span').html(start.toString(dateFormat) + ' - ' + end.toString(dateFormat));
        formDateRangeStart = start.toString(dateFormatExact);
        formDateRangeEnd = end.toString(dateFormatExact);
        //alert(formDateRangeStart);

        dateStartControl.val(formDateRangeStart);
        dateEndControl.val(formDateRangeEnd);

        if (onDateRangeChangeFunction != null && typeof (onDateRangeChangeFunction) !== 'undefined')
            onDateRangeChangeFunction(formDateRangeStart, formDateRangeEnd);
    });
}

