using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADServerDAL
{
    public partial class EC2AdServEntities
    {
        public EC2AdServEntities(System.Data.Common.DbConnection existingConnection, bool disposeWithEntities)
            : base(existingConnection, disposeWithEntities)
        {

        }
    }
}
