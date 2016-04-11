// Flaga określająca, czy lista obiektów została już załadowana
var devicesLoaded = false;

// Sortowanie 
var ascendingDevs = -1;

var connectedDevices = [];
// Nośniki do usunięcia
var deletedDevices = [];

var availableDevices = [];

// Ładowanie nośników
function LoadDevices(url, id) {
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

			///Pobranie nośników i odświeżenie interfejsu
			connectedDevices = r.ConnectedDevs;
			RefreshConnectedDevicesUI();
			devicesLoaded = true;
		},
		error: function (r) {
			ADSrvError(r);
		}
	});
}

// Usunięcie powiązania
function RemoveDevice() {
	var n = $("#Devices input:checked");
	if (n.length > 0) {
		n.each(function (index, element) {
			var re = document.getElementById('Devices');
			var child = document.getElementById(element.name);
			re.removeChild(child);
			deletedDevices.push({
				Id: child.id.replace('Device.', '')
			});
		});
	}
}

// Wygenerowanie tabel html dla list powiązanych nośników
function RefreshConnectedDevicesUI() {
	var d = document.getElementById('Devices');
	d.innerHTML = '';
	var nameFilter = $('#Filter_ConnectedDevices_Name').val();

	for (var i = 0; i < connectedDevices.length; i++) {
		var id = connectedDevices[i].Id;
		var name = connectedDevices[i].Name;
		var description = connectedDevices[i].Description;

		if (nameFilter == null ||
           nameFilter.length == 0 ||
           name.toLowerCase().indexOf(nameFilter.toLowerCase()) != -1 ||
           description.toLowerCase().indexOf(nameFilter.toLowerCase()) != -1) {
		    var host = window.location.host;
		    var img = '<script>load();</script><h3 id="loader_img"><span class="glyphicon glyphicon-refresh glyphicon-refresh-animate"></span></h3><img id="imgPreview" src="http://' + host + '/Ad?id=' + id + '" alt="Ad" />';
		    var row = '<tr id="device.' + id + '"><td class="text-center" width="10%"> <input name="device.' + id + '" type="checkbox" class="selectionCB" /></td><td class="text-right">' + id + '</td><td class="text-left"><i data-toggle="tooltip" data-placement="bottom" title=\''+img+'\' data-html="true">' + name + '</i></td><td class="text-center">' + description + '</td></tr>';
			$('#Devices').append(row);
		}
	}
	$(function () {
	    $('[data-toggle="tooltip"]').tooltip({
	        'container': 'body'
	    })
	})
}

///Wyczyszczenie filtrów dla listy przypisanych nośników
function ClearConnectedDevicesFilters() {
	$('#Filter_ConnectedDevices_Name').val('');
	RefreshConnectedDevicesUI();
}

// Sortowanie tabel HTML kategorii wg zadanego pola
function SortDevices(fieldName) {
	connectedDevices.sort(function(a, b) {
		return ((a[fieldName] < b[fieldName]) ? -1 : (a[fieldName] > b[fieldName]) ? 1 : 0) * ascendingDevs;
	});

	RefreshConnectedDevicesUI();

	if (ascendingDevs == -1)
		ascendingDevs = 1;
	else {
		ascendingDevs = -1;

	}
}