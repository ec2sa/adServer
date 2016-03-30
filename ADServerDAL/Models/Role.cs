using System;
using System.Collections.Generic;
using ADServerDAL.Models.Base;

namespace ADServerDAL.Models
{
    public class Role : Entity
	{
		#region Description
		public string RoleType { get; set; }
        public short Commission { get; set; }
		#endregion
		
		#region Collections
		public virtual ICollection<User> Users { get; set; }
		#endregion
	}
}
