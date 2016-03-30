/// <reference path="~/Scripts/jquery-2.1.0.js" />

///Definicja obiektu kategorii
function CampaignCategory(Id, Name, Code) {
    var self = this;
    self.Id = Id;
    self.Name = Name;
    self.Code = Code;
    self.Campaigns = [];
}