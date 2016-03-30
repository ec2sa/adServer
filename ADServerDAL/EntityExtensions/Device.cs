using System.ComponentModel.DataAnnotations;
using ADServerDAL.MetadataEntities;

namespace ADServerDAL
{
	[MetadataType(typeof(DeviceMetadata))]
	public partial class Device
	{
		public override string ToString()
		{
			return Name;
		}
	}
}