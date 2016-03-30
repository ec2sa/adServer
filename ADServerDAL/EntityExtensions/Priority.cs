using System.ComponentModel.DataAnnotations;

namespace ADServerDAL
{
	/// <summary>
	/// Rozszerzenie encji priorytetu
	/// </summary>
	[MetadataType(typeof(PriorityMetadata))]
	public partial class Priority
	{
		public override string ToString()
		{
			return this.Name;
		}

		#region Overrided methods

		public override bool Equals(object obj)
		{
			Priority priority = obj as Priority;
			if (priority != null)
			{
				if (priority.ID != 0 && ID != 0)
				{
					return priority.ID == ID;
				}
				return priority.Code == Code;
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