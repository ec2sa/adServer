using ADServerDAL.Abstract;

namespace ADServerDAL.Concrete
{
    public class EFRepositorySet
    {
        public static RepositorySet CreateRepositorySet(System.Data.Entity.DbContext ctx)
        {
            var result = new RepositorySet
            {
	            CampaignRepository = new EFCampaignRepository(),
	            CategoryRepository = new EFCategoryRepository(),
	            MultimediaObjectRepository = new EFMultimediaObjectRepository(),
	            StatisticsRepository = new EFStatisticRepository(),
	            PriorityRepository = new EFPriorityRepository(),
	            TypeRepository = new EFTypeRepository(),
	            UserRepository = new EFUsersRepository(),
				DeviceRepository = new EFDeviceRepository()
            };

            result.CampaignRepository.SetContext(ctx);
            result.CategoryRepository.SetContext(ctx);
            result.MultimediaObjectRepository.SetContext(ctx);            
            result.StatisticsRepository.SetContext(ctx);
            result.PriorityRepository.SetContext(ctx);
            result.TypeRepository.SetContext(ctx);
	        result.DeviceRepository.SetContext(ctx);

            return result;
        }
    }
}
