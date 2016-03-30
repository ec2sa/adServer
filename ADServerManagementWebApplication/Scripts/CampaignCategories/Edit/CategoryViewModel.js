/// Model kategorii
var campaignCategoryModel = null;

///Zapis kategorii do bazy danych
function SaveCategory(url, successUrl) {

    ///Walidacja po stronie klienta
    if (!ClientSiteModelValidation()) {
        return;
    }

    ///Wypełnienie pól obiektu
    campaignCategoryModel.Id = $('#Category_Id').val();
    campaignCategoryModel.Name = $('#Category_Name').val();
    campaignCategoryModel.Code = $('#Category_Code').val();

    campaignCategoryModel.Campaigns = [];
    if (connectedCampaigns.length > 0) {
        for (var i = 0; i < connectedCampaigns.length; i++) {
            campaignCategoryModel.Campaigns.push(connectedCampaigns[i]);
        }
    }

    ShowPleaseWaitPanel('Zapisywanie kategorii');
    ClearValidationMessages();

    ///Wysłanie żądania na serwer
    $.ajax({
        type: 'POST',
        url: url,
        data: JSON.stringify(campaignCategoryModel),
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
