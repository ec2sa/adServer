// Flaga określająca, czy lista obiektów została już załadowana
var objectsLoaded = false;

// Sortowanie 
var ascendingObj = -1;

// Lista wszystkich obiektów multimedialnych
var availableObjects = [];

// Lista powiązanych z kampanią obiektów
var connectedObjects = [];

// Pobranie wszystkich obiektów multimedialnych z bazy danych z wyszczególnieniem obiektów przypisanych do bieżącej kampanii
function LoadObjects(url, id) {
	var dataJSON = { CampaignId: id };

	// Wywołanie metody API kontrolera
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
			availableObjects = r.AvailableObjects;
			connectedObjects = r.ConnectedObjects;
			RefreshAvailableObjectsUI();
			RefreshConnectedObjectsUI();
			objectsLoaded = true;
		},
		error: function (r) {
			ADSrvError(r);
		}
	});
}

// Wygenerowanie tabel html dla listy dostępnych obiektów multimedialnych
function RefreshAvailableObjectsUI() {
	$('#availableObjectsTable').find("tr:gt(0)").remove();

	var nameFilter = $('#Filter_AvailableObjects_Name').val();

	for (var i = 0; i < availableObjects.length; i++) {
		var id = availableObjects[i].Id;
		var name = availableObjects[i].Name;
		var mime = availableObjects[i].Mime;
		var type = availableObjects[i].TypeName;
		var width = availableObjects[i].Width;
		var height = availableObjects[i].Height;
		var typeDescription = type + ' (' + width + 'x' + height + ')';

		if (nameFilter == null ||
            nameFilter.length == 0 ||
            name.toLowerCase().indexOf(nameFilter.toLowerCase()) != -1 ||
            mime.toLowerCase().indexOf(nameFilter.toLowerCase()) != -1 ||
            typeDescription.toLowerCase().indexOf(nameFilter.toLowerCase()) != -1) {
			var row = '<tr><td class="text-center" width="10%"><input name="object.' + id + '" type="checkbox" class="selectionCB" /></td><td class="text-right">' + id + '</td><td class="text-left">' + name + '</td><td class="text-center">' + typeDescription + '</td><td class="text-center">' + mime + '</td></tr>';
			$('#availableObjectsTable').append(row);
		}
	}
}

// Wygenerowanie tabel html dla listy powiązanych obiektów multimedialnych
function RefreshConnectedObjectsUI() {
	$('#connectedObjectsTable').find("tr:gt(0)").remove();

	var nameFilter = $('#Filter_ConnectedObjects_Name').val();

	for (var i = 0; i < connectedObjects.length; i++) {
		var id = connectedObjects[i].Id;
		var name = connectedObjects[i].Name;
		var mime = connectedObjects[i].Mime;
		var type = connectedObjects[i].TypeName;
		var width = connectedObjects[i].Width;
		var height = connectedObjects[i].Height;
		var typeDescription = type + ' (' + width + 'x' + height + ')';

		if (nameFilter == null ||
            nameFilter.length == 0 ||
            name.toLowerCase().indexOf(nameFilter.toLowerCase()) != -1 ||
            mime.toLowerCase().indexOf(nameFilter.toLowerCase()) != -1 ||
            typeDescription.toLowerCase().indexOf(nameFilter.toLowerCase()) != -1) {
			var row = '<tr><td class="text-center" width="10%"> <input name="object.' + id + '" type="checkbox" class="selectionCB" /></td><td class="text-right">' + id + '</td><td class="text-left">' + name + '</td><td class="text-center">' + typeDescription + '</td><td class="text-center">' + mime + '</td></tr>';
			$('#connectedObjectsTable').append(row);
		}
	}
}

// Usunięcie powiązania obiektu z kampanią
function RemoveObjectConnection() {

	var n = $("#connectedObjectsTable input:checked");
	if (n.length > 0) {
		n.each(function (index, element) {
			var objectId = parseInt(element.name.replace("object.", ""));

			for (var i = 0; i < connectedObjects.length; i++) {
				if (connectedObjects[i].Id == objectId) {

					var item = connectedObjects[i];
					connectedObjects.splice(i, 1);
					availableObjects.push(item);
					--i;

					break;
				}
			}
		});

		RefreshAvailableObjectsUI();
		RefreshConnectedObjectsUI();
	}
}

// Utworzenie powiazania obiektu z kampanią
function AddObjectConnection() {

	$("#dialog-availableObjects").dialog({
		resizable: true,
		draggable: true,
		height: 600,
		width: 800,
		modal: true,
		buttons: {
			OK: {
				click: function () {
					$(this).dialog("close");

					var n = $("#availableObjectsTable input:checked");
					if (n.length > 0) {
						n.each(function (index, element) {
							var objectId = parseInt(element.name.replace("object.", ""));

							for (var i = 0; i < availableObjects.length; i++) {
								if (availableObjects[i].Id == objectId) {

									var item = availableObjects[i];
									availableObjects.splice(i, 1);
									connectedObjects.push(item);
									--i;

									break;
								}
							}
						});

						RefreshAvailableObjectsUI();
						RefreshConnectedObjectsUI();
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
                	buttonText: 'Anuluj'
                }

		}
	});

}

// Sortowanie tabel HTML obiektów wg zadanego pola
function SortObjects(fieldName, connected) {
	if (connected) {
		connectedObjects.sort(function (a, b) {
			return ((a[fieldName] < b[fieldName]) ? -1 : (a[fieldName] > b[fieldName]) ? 1 : 0) * ascendingObj;
		});

		RefreshConnectedObjectsUI();
	}
	else {

		availableObjects.sort(function (a, b) {
			return ((a[fieldName] < b[fieldName]) ? -1 : (a[fieldName] > b[fieldName]) ? 1 : 0) * ascendingObj;
		});

		RefreshAvailableObjectsUI();
	}
	if (ascendingObj == -1)
		ascendingObj = 1;
	else {
		ascendingObj = -1;
	}
}

///Wyczyszczenie filtrów dla listy dostępnych obiektów
function ClearAvailableObjectsFilters() {
	$('#Filter_AvailableObjects_Name').val('');
	RefreshAvailableObjectsUI();
}

///Wyczyszczenie filtrów dla listy przypisanych obiektów
function ClearConnectedObjectsFilters() {
	$('#Filter_ConnectedObjects_Name').val('');
	RefreshConnectedObjectsUI();
}



