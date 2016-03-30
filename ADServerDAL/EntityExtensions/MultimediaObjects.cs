using System.ComponentModel.DataAnnotations;

namespace ADServerDAL
{
	/// <summary>
	/// Roszerzenie encji obiektu multimedialnego
	/// </summary>
	[MetadataType(typeof(MultimediaObjectMetadata))]
	public partial class MultimediaObject
	{
		public override string ToString()
		{
			return this.Name;
		}

		#region Overrided methods

		public override bool Equals(object obj)
		{
			MultimediaObject multiObj = obj as MultimediaObject;
			if (multiObj != null)
			{
				if (multiObj.ID != 0 && ID != 0)
				{
					return multiObj.ID == ID;
				}
				return multiObj.Name == Name;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#endregion Overrided methods
	}
}