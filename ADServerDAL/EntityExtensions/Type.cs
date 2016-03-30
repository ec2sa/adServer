using System.ComponentModel.DataAnnotations;

namespace ADServerDAL
{
	/// <summary>
	/// Rozszerzenie encji typu obiektu multimedialnego
	/// </summary>
	[MetadataType(typeof(TypeMetadata))]
	public partial class Type
	{
		#region Properties

		/// <summary>
		/// Opis z nazwą
		/// </summary>
		public string DescriptorWithName
		{
			get
			{
				return string.Format("{0} ({1})", this.Name, DescriptorWithoutName);
			}
		}

		/// <summary>
		/// Opis bez nazwy
		/// </summary>
		public string DescriptorWithoutName
		{
			get
			{
				return string.Format("{0}x{1}", this.Width, this.Height);
			}
		}

		#endregion Properties

		#region Overrided methods

		public override bool Equals(object obj)
		{
			Type type = obj as Type;
			if (type != null)
			{
				if (type.ID != 0 && ID != 0)
				{
					return type.ID == ID;
				}
				return type.Name == Name;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return DescriptorWithName;
		}

		#endregion Overrided methods
	}
}