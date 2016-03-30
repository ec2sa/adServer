/// Model kategorii
var DeviceModel = null;

// Zapis kategorii do bazy danych
function SaveDevice(url, successUrl) {
	///Walidacja po stronie klienta
	if (!ClientSiteModelValidation()) {
		return;
	}

	///Wypełnienie pól obiektu
	DeviceModel = new Device($('#Id').val(), $('#Description').val(), $('#Name').val());
	DeviceModel.Campaigns = [];
	if (connectedCampaigns.length > 0) {
		for (var i = 0; i < connectedCampaigns.length; i++) {
			DeviceModel.Campaigns.push({
				"Id": connectedCampaigns[i].Id
			});
		}
	}

	DeviceModel.TypeId = $('#TypeId').val();

	DeviceModel.Categories = [];
	if (connectedCategories.length > 0) {
		for (var i = 0; i < connectedCategories.length; i++) {
			DeviceModel.Categories.push({
				"Id": connectedCategories[i].Id
			});
		}
	}
	ShowPleaseWaitPanel('Zapisywanie kategorii');
	ClearValidationMessages();

	///Wysłanie żądania na serwer
	$.ajax({
		type: 'POST',
		url: url,
		data: JSON.stringify(DeviceModel),
		contentType: 'application/json; charset=utf-8',
		dataType: 'json',
		success: function (r) {
			if (!r.Accepted) {
				///Ręczna obsługa błędów
				HidePleaseWaitPanel();
				ShowValidationMessages(r, 'Category');
				ADSrvError(r);
				return;
			}

			///Sukces - przekieruj na wybraną stronę
			window.location = successUrl;
		},
		error: function (r) {
			ADSrvError(r);
		},
		compleded: function (r) {
			HidePleaseWaitPanel();
		}
	});
}
