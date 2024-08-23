using ProjetoNoticiaV1.Interface;

namespace ProjetoNoticiaV1.Extension
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            var implementationsType = typeof(DependencyInjectionExtensions).Assembly.GetTypes()
              .Where(t => typeof(IService).IsAssignableFrom(t) &&
                     t.BaseType != null);

            foreach (var item in implementationsType)
                services.AddScoped(item);

            return services;
        }
    }
}