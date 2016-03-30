// Flaga określająca, czy lista kategorii została już załadowana
var categoriesLoaded = false;

// Lista wszystkich kategorii
var availableCategories = [];

// Lista powiązanych z kampanią kategorii
var connectedCategories = [];

// Sortowanie 
var ascendingCats = -1;

// Pobranie wszystkich kategorii z bazy danych z wyszczególnieniem kategorii przypisanych do bieżącej kampanii
function LoadCategories(url, id) {
	var dataJSON = { Id: id };
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
			availableCategories = r.AvailableCategories;
			connectedCategories = r.ConnectedCategories;
			RefreshAvailableCategoriesUI();
			RefreshConnectedCategoriesUI();
			categoriesLoaded = true;
		},
		error: function (r) {
			ADSrvError(r);
		}
	});
}

// Wygenerowanie tabel html dla list dostępnych kategorii
function RefreshAvailableCategoriesUI() {
	$('#availableCategoriesTable').find("tr:gt(0)").remove();

	var nameFilter = $('#Filter_AvailableCategories_Name').val();

	for (var i = 0; i < availableCategories.length; i++) {
		var id = availableCategories[i].Id;
		var name = availableCategories[i].Name;
		var code = availableCategories[i].Code;

		if (nameFilter == null ||
            nameFilter.length == 0 ||
            name.toLowerCase().indexOf(nameFilter.toLowerCase()) != -1 ||
            code.toLowerCase().indexOf(nameFilter.toLowerCase()) != -1) {
			var row = '<tr><td class="text-center" width="10%"><input name="category.' + id + '" type="checkbox" class="selectionCB" /></td><td class="text-right">' + id + '</td><td class="text-left">' + name + '</td><td class="text-center">' + code + '</td></tr>';
			$('#availableCategoriesTable').append(row);
		}
	}
}

// Wygenerowanie tabel html dla list powiązanych kategorii
function RefreshConnectedCategoriesUI() {
	$('#connectedCategoriesTable').find("tr:gt(0)").remove();

	var nameFilter = $('#Filter_ConnectedCategories_Name').val();

	for (var i = 0; i < connectedCategories.length; i++) {
		var id = connectedCategories[i].Id;
		var name = connectedCategories[i].Name;
		var code = connectedCategories[i].Code;

		if (nameFilter == null ||
           nameFilter.length == 0 ||
           name.toLowerCase().indexOf(nameFilter.toLowerCase()) != -1 ||
           code.toLowerCase().indexOf(nameFilter.toLowerCase()) != -1) {
			var row = '<tr><td class="text-center" width="10%"> <input name="category.' + id + '" type="checkbox" class="selectionCB" /></td><td class="text-right">' + id + '</td><td class="text-left">' + name + '</td><td class="text-center">' + code + '</td></tr>';
			$('#connectedCategoriesTable').append(row);
		}
	}
}

// Utworzenie powiazania kategorii z kampanią
function AddCategoryConnection() {

	$("#dialog-availableCategories").dialog({
		resizable: true,
		draggable: true,
		height: 600,
		width: 800,
		modal: true,
		buttons: {
			OK:
                {
                	click: function () {
                		$(this).dialog("close");

                		var n = $("#availableCategoriesTable input:checked");
                		if (n.length > 0) {
                			n.each(function (index, element) {
                				var categoryId = parseInt(element.name.replace("category.", ""));

                				for (var i = 0; i < availableCategories.length; i++) {
                					if (availableCategories[i].Id == categoryId) {
                						var item = availableCategories[i];
                						availableCategories.splice(i, 1);
                						connectedCategories.push(item);
                						--i;
                						break;
                					}
                				}
                			});

                			RefreshAvailableCategoriesUI();
                			RefreshConnectedCategoriesUI();
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

// Usunięcie powiązania kategorii z kampanią
function RemoveCategoryConnection() {

	var n = $("#connectedCategoriesTable input:checked");
	if (n.length > 0) {
		n.each(function (index, element) {
			var categoryId = parseInt(element.name.replace("category.", ""));

			for (var i = 0; i < connectedCategories.length; i++) {
				if (connectedCategories[i].Id == categoryId) {

					var item = connectedCategories[i];
					connectedCategories.splice(i, 1);
					availableCategories.push(item);
					--i;
					break;
				}
			}
		});

		RefreshAvailableCategoriesUI();
		RefreshConnectedCategoriesUI();
	}
}

// Sortowanie tabel HTML kategorii wg zadanego pola
function SortCategories(fieldName, connected) {
	if (connected) {
		connectedCategories.sort(function (a, b) {
			return ((a[fieldName] < b[fieldName]) ? -1 : (a[fieldName] > b[fieldName]) ? 1 : 0) * ascendingCats;
		});

		RefreshConnectedCategoriesUI();
	}
	else {

		availableCategories.sort(function (a, b) {
			return ((a[fieldName] < b[fieldName]) ? -1 : (a[fieldName] > b[fieldName]) ? 1 : 0) * ascendingCats;
		});

		RefreshAvailableCategoriesUI();
	}
	if (ascendingCats == -1)
		ascendingCats = 1;
	else {
		ascendingCats = -1;
	}
}

///Wyczyszczenie filtrów dla listy dostępnych kategorii
function ClearAvailableCategoriesFilters() {
	$('#Filter_AvailableCategories_Name').val('');
	RefreshAvailableCategoriesUI();
}

///Wyczyszczenie filtrów dla listy przypisanych kategorii
function ClearConnectedCategoriesFilters() {
	$('#Filter_ConnectedCategories_Name').val('');
	RefreshConnectedCategoriesUI();
}