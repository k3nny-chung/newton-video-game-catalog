using AutoMapper;

namespace TestProject
{
    public static class MapperBuilder
    {
        public static IMapper Create()
        {
            return new AutoMapper.MapperConfiguration(options =>
            {
                options.AddProfile(new VideoGamesCatalog.Server.Mapper.AutoMapperProfile());
            }).CreateMapper();
        }
    }
}
