(function ($) {
    $.extend($.validator, {

        messages: {
            required: "Pole jest wymagane.",
            remote: "Please fix this field.",
            email: "Email jest nieprawidłowy.",
            url: "URL jest nieprawidłowy.",
            date: "Należy podać prawidłową datę.",
            dateISO: "Należy podać właściwą datę (ISO).",
            number: "Należy podać numer.",
            digits: "Należy wpisać tylko cyfry.",
            creditcard: "Please enter a valid credit card number.",
            equalTo: "Please enter the same value again.",
            accept: "Please enter a value with a valid extension.",
            maxlength: $.validator.format("Maksymalna ilość znaków to: {0}."),
            minlength: $.validator.format("Minimalna ilość znaków to: {0}."),
            rangelength: $.validator.format("Please enter a value between {0} and {1} characters long."),
            range: $.validator.format("Please enter a value between {0} and {1}."),
            max: $.validator.format("Please enter a value less than or equal to {0}."),
            min: $.validator.format("Please enter a value greater than or equal to {0}.")
        },
    })
})(jQuery);

