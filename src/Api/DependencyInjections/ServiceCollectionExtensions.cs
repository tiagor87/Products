using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Products.Application.Adapters;
using Products.Application.Contracts;
using Products.Application.Services;
using Products.Domain.Models;
using Products.Domain.Repositories;
using Products.Infrastructure.Providers;
using Products.Infrastructure.Providers.MongoDB;
using Products.Infrastructure.Repositories;

namespace Products.Api
{
    public static class ServiceCollectionExtensions {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services, IConfiguration configuration) {
            services
                .AddScoped<IMongoClient>(provider => new MongoClient(configuration.GetConnectionString("mongodb")))
                .AddScoped<IMongoDatabase>(provider => provider.GetService<IMongoClient>().GetDatabase(configuration.GetValue<string>("mongo:database")))
                .AddScoped<IAdapter<ProductContract, Product>, ProductAdapter>()
                .AddScoped<IProductAppService, ProductAppService>()
                .AddScoped<IProductRepository, ProductRepository>()
                .AddScoped<IProvider<Product>, ProductProvider>();
            return services;
        }
    }
}
