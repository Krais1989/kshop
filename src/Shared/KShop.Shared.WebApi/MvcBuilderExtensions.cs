using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KShop.Shared.WebApi
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddEnumNameConverter(this IMvcBuilder services)
        {
            return services.AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        }
    }
}
