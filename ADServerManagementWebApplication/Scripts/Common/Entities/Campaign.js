/// <reference path="~/Scripts/jquery-2.1.0.js" />

// Definicja obiektu kampanii
function Campaign(Id, Name, Description, StartDate, EndDate, PriorityId, IsActive) {
    var self = this;
    self.Id = Id;
    self.Name = Name;
    self.Description = Description;
    self.StartDate = StartDate;
    self.EndDate = EndDate;
    self.PriorityId = PriorityId;
    self.IsActive = IsActive;
    self.Categories = [];
    self.MultimediaObjects = [];
	self.DeletedDevices = [];
}



