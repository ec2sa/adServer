using System.ComponentModel.DataAnnotations;

namespace ADServerDAL
{
	/// <summary>
	/// Rozszerzenie encji kampanii
	/// </summary>
	[MetadataType(typeof(CampaignMetadata))]
	public partial class Campaign
	{
		public override string ToString()
		{
			return this.Name;
		}

		#region Overrided methods

		public override bool Equals(object obj)
		{
			Campaign cmp = obj as Campaign;
			if (cmp != null)
			{
				if (cmp.ID != 0 && ID != 0)
				{
					return cmp.ID == ID;
				}
				return cmp.Name == Name;
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