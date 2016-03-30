/// <reference path="~/Scripts/jquery-2.1.0.js" />

// Instancja modelu kampanii do zapisywania w bazie danych
var campaignModel = null;

var ascending = 1;
// Funkcja sprawdzająca, czy wszystkie listy zostały już załadowane
function AreListsLoaded() {
	return categoriesLoaded && objectsLoaded;
}

// Zapisanie kampanii do bazy danych
function SaveCampaign(url, successUrl) {

	// Wywołaj walidację po stronie klienta
	if (!ClientSiteModelValidation()) {
		return;
	}
	GetCampaign();
	ShowPleaseWaitPanel('Zapisywanie kampanii');
	ClearValidationMessages();
	// Wywołanie metody API kontrolera
	$.ajax({
		type: 'POST',
		url: url,
		data: JSON.stringify(campaignModel),
		contentType: 'application/json; charset=utf-8',
		dataType: 'json',
		success: function (r) {
			if (!r.Accepted) {
				///Ręczna obsługa błędów
				HidePleaseWaitPanel();
				ShowValidationMessages(r, 'Campaign');
				ADSrvError(r);
				return;
			}

			//Sukces - przekieruj na wskazaną stronę
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

function GetCampaign() {
	// Pobierz aktualne wartości pól
	campaignModel.Name = $('#Campaign_Name').val();
	campaignModel.Description = $('#Campaign_Description').val();
	campaignModel.StartDate = $('#Campaign_StartDate').val();
	campaignModel.EndDate = $('#Campaign_EndDate').val();
	campaignModel.IsActive = $('#Campaign_IsActive').is(':checked');
	campaignModel.PriorityId = parseInt($('#Campaign_PriorityId').val());
	campaignModel.UserId = $('#Campaign_UserId').val();
	campaignModel.AdPoints = floatValue($('#Campaign_AdPoints').val());
	campaignModel.ViewValue = floatValue($('#Campaign_ViewValue').val());
	campaignModel.ClickValue = floatValue($('#Campaign_ClickValue').val());
	campaignModel.Categories = [];
	if (connectedCategories.length > 0) {
		for (var i = 0; i < connectedCategories.length; i++) {
			campaignModel.Categories.push(connectedCategories[i]);
		}
	}
	campaignModel.Devices = [];
	var dev = $('#Devices').children();
	if (dev.length > 0) {
		for (var i = 0; i < dev.length; i++) {
			var id = dev[i].id.replace(/device./g, '');
			campaignModel.Devices.push({ Id: id });
		}
	}
	campaignModel.MultimediaObjects = [];
	if (connectedObjects.length > 0) {
		for (var i = 0; i < connectedObjects.length; i++) {
			campaignModel.MultimediaObjects.push(connectedObjects[i]);
		}
	}
	campaignModel.DeletedDevices = deletedDevices;
}