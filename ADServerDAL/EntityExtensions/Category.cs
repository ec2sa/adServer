using System.ComponentModel.DataAnnotations;

namespace ADServerDAL
{
	/// <summary>
	/// Rozszerzenie encji kategorii
	/// </summary>
	[MetadataType(typeof(CategoryMetadata))]
	public partial class Category
	{
		public override string ToString()
		{
			return this.Name;
		}

		#region Overrided methods

		public override bool Equals(object obj)
		{
			Category category = obj as Category;
			if (category != null)
			{
				if (category.ID != 0 && ID != 0)
				{
					return category.ID == ID;
				}
				return category.Name == Name;
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