/// Wszystkie kampanie
var availableCampaigns = [];

/// Powiązane kampanie (z obiektem lub kategorią)
var connectedCampaigns = [];

// Sortowanie 
var ascendingCmps = -1;

/// Czy listy zostały załadowane
var campaignsLoaded = false;

function AreListsLoaded() {
	return campaignsLoaded;
}

// Odświeżenie tabel asocjacyjnych dla kampanii (z formatki edycyjnej kategorii oraz formatki edycyjnej obiektu multimedialnego)
function RefreshAvailableCampaignsUI() {

	$('#availableCampaignsTable').find("tr:gt(0)").remove();

	var nameFilter = $('#Filter_AvailableCampaigns_Name').val();

	for (var i = 0; i < availableCampaigns.length; i++) {
		var id = availableCampaigns[i].Id;
		var name = availableCampaigns[i].Name;
		var isActive = availableCampaigns[i].IsActive;
		var startDate = availableCampaigns[i].StartDate;
		var endDate = availableCampaigns[i].EndDate;
		startDate = moment(startDate).format("YYYY-MM-DD");
		endDate = moment(endDate).format("YYYY-MM-DD");
		checked = isActive ? 'checked="checked"' : '';
		var clickValue = availableCampaigns[i].ClickValue;
		var viewValue = availableCampaigns[i].ViewValue;
		var isBlocked = availableCampaigns[i].IsBlocked;
		if (nameFilter == null || nameFilter.length == 0 || name.toLowerCase().indexOf(nameFilter.toLowerCase()) != -1) {

			var row = '<tr><td class="text-center" width="10%"> <input name="campaign.' + id + '" type="checkbox" class="selectionCB"';
			if (isBlocked)
				row += ' disabled="disabled" title="Kampania zablokowana dla tego nośnika"';
			row += ' /></td><td class="text-right">' + id + '</td><td class="text-left">' + name + '</td><td  class="text-center"><input type="checkbox" ' + checked + ' disabled="disabled" /></td><td  class="text-right">' + startDate + '</td><td  class="text-right">' + endDate + '</td><td  class="text-right">' + viewValue + '</td><td  class="text-right">' + clickValue + '</td></tr>';
			$('#availableCampaignsTable').append(row);
		}
	}
}

function RefreshConnectedCampaignsUI() {

	$('#connectedCampaignsTable').find("tr:gt(0)").remove();

	var nameFilter = $('#Filter_ConnectedCampaigns_Name').val();

	for (var i = 0; i < connectedCampaigns.length; i++) {
		var id = connectedCampaigns[i].Id;
		var name = connectedCampaigns[i].Name;
		var isActive = connectedCampaigns[i].IsActive;
		var startDate = connectedCampaigns[i].StartDate;
		var endDate = connectedCampaigns[i].EndDate;
		startDate = moment(startDate).format("YYYY-MM-DD");
		endDate = moment(endDate).format("YYYY-MM-DD");
		checked = isActive ? 'checked="checked"' : '';
		var clickValue = connectedCampaigns[i].ClickValue;
		var viewValue = connectedCampaigns[i].ViewValue;

		if (nameFilter == null || nameFilter.length == 0 || name.toLowerCase().indexOf(nameFilter.toLowerCase()) != -1) {
		    var row = '<tr><td class="text-center" width="10%"><input type="hidden" name="MultimediaObject.Campaigns[' + i + '].Id" value="' + id + '"><input type="hidden" name="MultimediaObject.Campaigns[' + i + '].Name" value="' + name + '"> <input name="campaign.' + id + '" type="checkbox" class="selectionCB" /></td><td class="text-right">' + id + '</td><td class="text-left">' + name + '</td><td  class="text-center"><input type="checkbox" ' + checked + ' disabled="disabled" /></td><td  class="text-right">' + startDate + '</td><td  class="text-right">' + endDate + '</td><td  class="text-right">' + viewValue + '</td><td  class="text-right">' + clickValue + '</td></tr>';
			$('#connectedCampaignsTable').append(row);
		}
	}
}

function LoadCampaigns(url, parentEntityId) {
	var dataJSON = { Id: parentEntityId };
	$.ajax({
		type: 'POST',
		url: url,
		data: JSON.stringify(dataJSON),
		contentType: 'application/json; charset=utf-8',
		dataType: 'json',
		success: function (r) {
			if (!r.Accepted) {
				///Ręczna obsługa błędów
				ADSrvError(r);
				return;
			}
			///Pobranie obiektów i odświeżenie interfejsu
			availableCampaigns = r.AvailableCampaigns;
			connectedCampaigns = r.ConnectedCampaigns;
			RefreshConnectedCampaignsUI();
			RefreshAvailableCampaignsUI();
			campaignsLoaded = true;
		},
		error: function (r) {
			ADSrvError(r);
		}
	});
}

///Usunięcie powiązań z kampaniami
function RemoveCampaignsConnection() {

	var n = $("#connectedCampaignsTable input:checked");
	if (n.length > 0) {
		n.each(function (index, element) {
			var campaignId = parseInt(element.name.replace("campaign.", ""));

			for (var i = 0; i < connectedCampaigns.length; i++) {
				if (connectedCampaigns[i].Id == campaignId) {
					var item = connectedCampaigns[i];
					connectedCampaigns.splice(i, 1);
					availableCampaigns.push(item);
					--i;
					break;
				}
			}
		});

		RefreshConnectedCampaignsUI();
		RefreshAvailableCampaignsUI();
	}
}

///Dodanie powiązań z kampaniami
function AddCampaignsConnection() {

	$("#dialog-availableCampaigns").dialog({
		resizable: true,
		draggable: true,
		height: 600,
		width: 1111,
		modal: true,
		buttons: {
			OK:
                {
                	click: function () {
                		$(this).dialog("close");

                		///Pobranie zaznaczonych elementów z tabeli
                		var n = $("#availableCampaignsTable input:checked");
                		if (n.length > 0) {
                			n.each(function (index, element) {
                				var campaignId = parseInt(element.name.replace("campaign.", ""));

                				for (var i = 0; i < availableCampaigns.length; i++) {
                					if (availableCampaigns[i].Id == campaignId) {
                						var item = availableCampaigns[i];
                						availableCampaigns.splice(i, 1);
                						connectedCampaigns.push(item);
                						--i;
                						break;
                					}
                				}
                			});

                			///Odświeżenie interfejsu
                			RefreshConnectedCampaignsUI();
                			RefreshAvailableCampaignsUI();
                		}
                	},
                	'class': 'btn btn-success',
                	text: 'OK',
                	buttonText: 'OK',
                	open: function (event, ui) {
                		$(this).parent().children().children('.ui-dialog-titlebar-close').hide();

                	}
                },
			Anuluj:
                {
                	click: function () {
                		$(this).dialog("close");
                	},
                	'class': 'btn btn-danger',
                	text: 'Anuluj',
                	buttonText: 'OK'
                }
		}
	});
}

///Sortowanie listy kampanii po zadanym polu
function SortCampaigns(fieldName, connected) {
	if (connected) {
		connectedCampaigns.sort(function (a, b) {
			return ((a[fieldName] < b[fieldName]) ? -1 : (a[fieldName] > b[fieldName]) ? 1 : 0) * ascendingCmps;
		});

		RefreshConnectedCampaignsUI();
	}
	else {
		availableCampaigns.sort(function (a, b) {
			return ((a[fieldName] < b[fieldName]) ? -1 : (a[fieldName] > b[fieldName]) ? 1 : 0) * ascendingCmps;
		});

		RefreshAvailableCampaignsUI();
	}
	if (ascendingCmps == -1)
		ascendingCmps = 1;
	else 
		ascendingCmps = -1;
}

///Wyczyszczenie filtrów dla listy dostępnych kampanii
function ClearAvailableCampaignsFilters() {
	$('#Filter_AvailableCampaigns_Name').val('');
	RefreshAvailableCampaignsUI();
}

///Wyczyszczenie filtrów dla listy przypisanych kampanii
function ClearConnectedCampaignsFilters() {
	$('#Filter_ConnectedCampaigns_Name').val('');
	RefreshConnectedCampaignsUI();
}