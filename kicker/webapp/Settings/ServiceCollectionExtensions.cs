using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Webapp.Settings
{
    //    Usage:

    //services.ConfigureWritable<MyOptions>(Configuration.GetSection("MySection"));
    //Then:

    //private readonly IWritableOptions<MyOptions> _options;

    //    public MyClass(IWritableOptions<MyOptions> options)
    //    {
    //        _options = options;
    //    }
    //    To save the changes to the file:

    //_options.Update(opt => {
    //    opt.Field1 = "value1";
    //    opt.Field2 = "value2";
    //});
    //And you can pass a custom json file as optional parameter(it will use appsettings.json by default):
    //services.ConfigureWritable<MyOptions>(Configuration.GetSection("MySection"), "appsettings.custom.json");
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureWritable<T>(
            this IServiceCollection services,
            IConfigurationSection section,
            string file = "appsettings.json") where T : class, new()
        {
            services.Configure<T>(section);
            services.AddTransient<IWritableOptions<T>>(provider =>
            {
                var environment = provider.GetService<IHostingEnvironment>();
                var options = provider.GetService<IOptionsMonitor<T>>();
                return new WritableOptions<T>(environment, options, section.Key, file);
            });
        }
    }
}
