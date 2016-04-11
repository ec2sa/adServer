/// <reference path="~/Scripts/jquery-2.1.0.js" />

///Model obiektów
var objectModel = null;
var Crop = null;

function CroppImg(url, successUrl) {
 
    var x = parseInt($('#x').val());
    var y = parseInt($('#y').val());
    var h = parseInt($('#h').val());
    var w = parseInt($('#w').val());
    var ID = parseInt($('#MultimediaObject_Id').val());

    Crop = new Crop(x,y,h,w,ID);

    $.ajax({
        type: 'POST',
        url: url,
        data: JSON.stringify(Crop),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (r) {

            if (!r.Accepted) {

                ///Ręczna obsługa błędów
                HidePleaseWaitPanel();
                ShowValidationMessages(r, 'Crop');
                ADSrvError(r);
                return;
            }
            window.location = successUrl;
        },
        error: function (r) {
            ADSrvError(r);
        },
        compleded: function (r) {
            HidePleaseWaitPanel();
            window.location = successUrl;
        }
    });
};
///Zapisanie obiektu do bazy danych
function SaveObject(url,url2, successUrl) {

    ///Walidacja po stronie klienta
    if (!ClientSiteModelValidation()) {
        return;
    }

    ///Wypełnienie pól obiektu
    objectModel.Id = parseInt($('#MultimediaObject_Id').val());
    objectModel.Name = $('#MultimediaObject_Name').val();
    objectModel.TypeId = parseInt($('#MultimediaObject_TypeId').val());
    objectModel.MimeType = $('#MultimediaObject_MimeType').val();
    objectModel.FileName = $('#MultimediaObject_FileName').val();
    objectModel.FileContent = $('#ImagePreview').attr('src');
    objectModel.UserId = $('#MultimediaObject_UserId').val();
    objectModel.Url = $('#MultimediaObject_Url').val();
    objectModel.Campaigns = [];
    if (connectedCampaigns.length > 0) {
        for (var i = 0; i < connectedCampaigns.length; i++) {
            objectModel.Campaigns.push(connectedCampaigns[i]);
        }
    }

    ShowPleaseWaitPanel('Zapisywanie obiektu');
    ClearValidationMessages();

    ///Wysłanie żądania na serwer
    $.ajax({
        type: 'POST',
        url: url,
        data: JSON.stringify(objectModel),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (r) {

            if (!r.Accepted) {

                ///Ręczna obsługa błędów
                HidePleaseWaitPanel();
                ShowValidationMessages(r, 'MultimediaObject');
                ADSrvError(r);
                return;
            }
            CroppImg(url2, successUrl);
            //window.location = successUrl;
        },
        error: function (r) {
            ADSrvError(r);
        },
        compleded: function (r) {
            CroppImg(url2, successUrl);
            HidePleaseWaitPanel();
        }
    });
}

