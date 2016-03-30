using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ADServerDAL.Models.Base;

namespace ADServerDAL.Models
{
    public class MultimediaObject : UserBase
    {
	    public MultimediaObject()
	    {
		    Campaigns = new Collection<Campaign>();
			Statistics = new Collection<Statistic>();
	    }

		#region Description

		/// <summary>
		/// Nazwa pliku
		/// </summary>
		[Display(Name = "Nazwa pliku")]
		[StringLength(250, ErrorMessage = "Maksymalna dopuszczalna d³ugoœæ pola {0} wynosi {1}")]
		[ReadOnly(true)]
		public string FileName { get; set; }

		/// <summary>
		/// Typ mime pliku
		/// </summary>
		[Display(Name = "Mime")]
		[StringLength(100, ErrorMessage = "Maksymalna dopuszczalna d³ugoœæ pola {0} wynosi {1}")]
		[ReadOnly(true)]
        public string MimeType { get; set; }

		/// <summary>
		/// Adres odnoœnika obiektu
		/// </summary>
		[Display(Name = "Adres URL")]
		[Url]
		[StringLength(200, ErrorMessage = "Maksymalna dopuszczalna d³ugoœæ pola {0} wynosi {1}")]
		public string Url { get; set; }

		public virtual byte[] Contents { get; set; }
		public virtual byte[] Thumbnail { get; set; }
		#endregion
		
		#region Foreign Key
		//[Required]
		[DataMember(IsRequired = false)]
		[Display(Name = "Typ obiektu")]
		[ForeignKey("Type")]
		public int? TypeId { get; set; }
		#endregion

		#region Foreign Key Values
		public virtual Type Type { get; set; }
		#endregion

		#region Collections
        public virtual ICollection<Campaign> Campaigns { get; set; }
        public virtual ICollection<Statistic> Statistics { get; set; }
		#endregion

		/// <summary>
		/// Zawartoœæ obiektu w formacie base64
		/// </summary>
		[NotMapped]
		public string FileContent
		{
			get
			{
				if (fileContent == null &&
					content != null)
				{
					fileContent = Convert.ToBase64String(content);
				}

				return fileContent;
			}
			set
			{
				fileContent = value;
			}
		}

		/// <summary>
		/// Zawartoœæ obiektu w formacie byte[]
		/// </summary>
		[NotMapped]
		public byte[] Content
		{
			get
			{
				if (content == null &&
					!string.IsNullOrEmpty(fileContent))
				{
					var index = fileContent.IndexOf("base64,");
					if (index != -1)
					{
						fileContent = fileContent.Substring(index + "base64,".Length);
					}

					content = Convert.FromBase64String(fileContent);
				}

				return content;
			}
			set
			{
				content = value;
			}
		}
		[NotMapped]
		private string fileContent;
		[NotMapped]
		private byte[] content;
    }
}
