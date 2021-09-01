using System;
using System.Reflection;
using System.Windows.Input;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using Castle.DynamicProxy;
using ExplorerHub.Applications;
using ExplorerHub.Applications.Favorites;
using ExplorerHub.Domain.Favorites;
using ExplorerHub.EfCore;
using ExplorerHub.Framework;
using ExplorerHub.Framework.Domain;

namespace ExplorerHub.Infrastructure
{
    public static class ContainerRegistrationExtensions
    {
        public static void AddEntityMapper(this ContainerBuilder builder)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Favorite, FavoriteDto>()
                    .ForMember(dto => dto.Url, expression => expression.MapFrom(favorite => favorite.Location));
            });

            builder.RegisterInstance(config.CreateMapper());
        }

        public static void AddExplorerHubDbContext(this ContainerBuilder builder, string connectionStr)
        {
            builder.Register(context => new ExplorerHubDbContext(connectionStr))
                .InjectProperties()
                .AsSelf();
        }

        public static IRegistrationBuilder<TRepositoryImpl, ConcreteReflectionActivatorData, SingleRegistrationStyle> AddRepository<TRepositoryInterface, TRepositoryImpl>(this ContainerBuilder builder)
            where TRepositoryImpl : AbstractRepository, TRepositoryInterface
        {
            return builder.RegisterType<TRepositoryImpl>()
                .As<TRepositoryInterface>()
                .InstancePerOwned<IApplicationService>();
        }
    }
}