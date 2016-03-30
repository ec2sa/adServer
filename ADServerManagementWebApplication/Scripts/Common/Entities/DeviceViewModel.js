/// <reference path="~/Scripts/jquery-2.1.0.js" />

// Definicja obiektu nośnika
function Device(Id, Description, Name) {
	var self = this;
	self.Id = Id;
	self.Name = Name;
	self.Campaigns = [];
	self.Categories = [];
	self.Description = Description;
}