/// <reference path="~/Scripts/jquery-2.1.0.js" />

///Walidacja danych formularza po stronie klienta
function ClientSiteModelValidation() {
	var elementsToValidate = $("[data-val='true']");
	var validator = $("Form").validate();
	var isValid = true;
	elementsToValidate.each(function (index, element) {
		isValid = isValid & validator.element(element);
	});

	return isValid;
}

function ClientSiteModelValidationForInput(target) {
	var validator = $("form").validate();
	if (validator.element(target)) {
		$(target).parent().children("span[data-valmsg-for]").removeClass('field-validation-error').hide();
	}
}

// Zaznaczenie / odznaczenie we wskazanej tabeli wszystkich elementów opatrzonych klasą selectionCB
function CheckOrUncheckOptions(id) {
	var nTotal = $("#" + id + " input.selectionCB:checkbox").length;
	var nChecked = $("#" + id + " input.selectionCB:checkbox:checked").length;

	if (nTotal == nChecked) {
		$('#' + id + " input.selectionCB:checkbox").removeAttr('checked');
	}
	else {
		$('#' + id + " input.selectionCB:checkbox").attr('checked', 'checked');
	}
}

//Czyści/resetuje wszystkie inputowe pola formularza
//@param showWaitPanel - okeśla, czy pokazać panel "Proszę czekać"
function ClearFormFields(showWaitPanel) {
	$(':input', "form")
      .not(':button, :submit, :reset, :hidden')
      .val('')
      .removeAttr('checked')
      .removeAttr('selected');

	if (showWaitPanel) {
		ShowPleaseWaitPanel('Proszę czekać', true);
	}
}
