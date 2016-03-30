using ADServerDAL.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADServerDAL.Abstract
{
    public class RepositorySet
    {
        public ICampaignRepository CampaignRepository { get; set; }
        public ICategoryRepository CategoryRepository { get; set; }
        public IMultimediaObjectRepository MultimediaObjectRepository { get; set; }
        public IStatisticRepository StatisticsRepository { get; set; }
        public IPriorityRepository PriorityRepository { get; set; }
        public ITypeRepository TypeRepository { get; set; }
		public IUsersRepository UserRepository { get; set; }
		public IRoleRepository RoleRepository { get; set; }
		public IDeviceRepository DeviceRepository { get; set; }
    }
}
