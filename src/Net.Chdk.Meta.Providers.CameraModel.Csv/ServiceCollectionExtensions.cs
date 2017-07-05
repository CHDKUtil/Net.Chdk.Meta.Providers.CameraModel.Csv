using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Providers.CameraModel.Csv
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCsvCameraListProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ICameraListProvider, CsvCameraListProvider>();
        }

        public static IServiceCollection AddCsvCameraTreeProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ICameraTreeProvider, CsvCameraTreeProvider>();
        }
    }
}
