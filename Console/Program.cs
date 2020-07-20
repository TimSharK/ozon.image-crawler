using System;
using System.Threading.Tasks;
using Console.ConfigModels;
using Domain;
using FileSystemImageStore;
using ImageLoader;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PostgresImageStore;

namespace Console
{
    class Program
    {
        private static async Task<int> Main(string[] args)
        {
            var serviceProvider = ConfigureServices();

            var imgLoader = serviceProvider.GetService<IImageLoader>();
            
            await imgLoader.Load();

            System.Console.WriteLine("Done");

            return 0;
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            var url = config.GetValue<string>("Url");

            services.AddTransient<IImageLoader, MyImageLoader>(provider =>
                new MyImageLoader(provider.GetService<IServiceProvider>(), provider.GetService<IServiceScopeFactory>(), url));

            services.Configure<AppSettings>(config);
            services.Configure<FileSystemSaverSettings>(config.GetSection("FileSystemSettings"));
            services.Configure<PostgresSaverSettings>(config.GetSection("PostgresSaverSettings"));

            if (config.GetValue<bool>("UseFileSystem"))
            {
                services.AddSingleton<IImageSaver, FileSystemImageSaver>(provider =>
                    new FileSystemImageSaver(provider.GetService<IOptions<FileSystemSaverSettings>>().Value.Folder));
            }
            else
            {
                var cs = config.GetValue<string>("PostgresSaverSettings:ConnectionString");
                services.AddDbContext<ImagesDbContext>(o => o.UseNpgsql(cs), ServiceLifetime.Transient);
                services.AddTransient<IImageSaver, PostgresImageSaver>();
            }

            return services.BuildServiceProvider();
        }
    }
}
