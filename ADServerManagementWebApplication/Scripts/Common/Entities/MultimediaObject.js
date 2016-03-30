/// <reference path="~/Scripts/jquery-2.1.0.js" />

///Definicja obiektu multimedialnego
function MultimediaObject(Id, Name, TypeId, TypeName, MimeType, Width, Height, FileName) {
    var self = this;
    self.Id = Id;
    self.Name = Name;
    self.TypeId = TypeId;
    self.TypeName = TypeName;
    self.MimeType = MimeType;
    self.Width = Width;
    self.Height = Height;
    self.FileName = FileName;
    self.FileContent = null;
    self.Campaigns = [];
}