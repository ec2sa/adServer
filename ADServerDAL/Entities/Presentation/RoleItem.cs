using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace ADServerDAL.Entities.Presentation
{
	public class RoleItem : PresentationItem
	{
		/// <summary>
		/// Identyfikator
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// Nazwa roli
		/// </summary>
		public string Name { get; set; }
		
		/// <summary>
		/// Uprawnienia
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// Prowizja za usługę
		/// </summary>
		public short Commission { get; set; }
	}

	public enum RoleType
	{
		Admin,
		User,
		Untyped
	}

}