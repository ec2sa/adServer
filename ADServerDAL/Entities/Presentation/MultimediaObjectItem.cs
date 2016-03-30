using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ADServerDAL.Entities.Presentation
{
	/// <summary>
	/// Reprezentacja obiektu multimedialnego
	/// </summary>
	public class MultimediaObjectItem : PresentationItem
	{
		#region - Fields -
		private string fileContent;
		private byte[] content;
		#endregion

		/// <summary>
		/// Identyfikator
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// Nazwa obiektu
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Typ mime obiektu
		/// </summary>
		public string Mime { get; set; }

		/// <summary>
		/// Identyfikator typu obiektu
		/// </summary>
		public int TypeId { get; set; }

		/// <summary>
		/// Nazwa typu obiektu
		/// </summary>
		public string TypeName { get; set; }

		/// <summary>
		/// Szerokość obiektu
		/// </summary>
		public int Width { get; set; }

		/// <summary>
		/// Wysokość obiektu
		/// </summary>
		public int Height { get; set; }

		/// <summary>
		/// Nazwa pliku obiektu
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// Zawartość obiektu w formacie base64
		/// </summary>
		public string FileContent
		{
			get
			{
				if (this.fileContent == null &&
					this.content != null)
				{
					this.fileContent = Convert.ToBase64String(this.content);
				}

				return this.fileContent;
			}
			set
			{
				this.fileContent = value;
			}
		}

		/// <summary>
		/// Zawartość obiektu w formacie byte[]
		/// </summary>
		public byte[] Content
		{
			get
			{
				if (this.content == null &&
					this.fileContent != null && this.fileContent.Length > 0)
				{
					int index = this.fileContent.IndexOf("base64,");
					if (index != -1)
					{
						this.fileContent = this.fileContent.Substring(index + "base64,".Length);
					}

					this.content = Convert.FromBase64String(this.fileContent);
				}

				return this.content;
			}
			set
			{
				this.content = value;
			}
		}

		/// <summary>
		/// Miniatura obiektu
		/// </summary>
		public byte[] Thumbnail { get; set; }

		/// <summary>
		/// Lista powiązanych kampanii
		/// </summary>
		public List<CampaignItem> Campaigns { get; set; }

		/// <summary>
		/// Opis typu obiektu (dla list)
		/// </summary>
		public string TypeDescriptor
		{
			get
			{
				return string.Format("{0} ({1}x{2})", TypeName, Width, Height);
			}
		}

		public int UserID { get; set; }
		public string UserName { get; set; }
		public string URL { get; set; }

	}
}