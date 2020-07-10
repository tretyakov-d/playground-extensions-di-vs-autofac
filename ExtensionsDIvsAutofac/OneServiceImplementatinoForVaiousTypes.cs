using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ExtensionsDIvsAutofac
{
    public interface IService { }

    public class Service : IService { }

    public class OneServiceImplementatinoForVaiousTypes
    {
        [Fact]
        public void Autofac_ReturnsSameInstance()
        {
            var contaienrBuilder = new ContainerBuilder();
            contaienrBuilder.RegisterType<Service>().AsSelf().AsImplementedInterfaces().SingleInstance();

            var container = contaienrBuilder.Build();

            var byInterface = container.Resolve<IService>();
            var byExactType = container.Resolve<Service>();

            Assert.Same(byInterface, byExactType);
        }

        [Fact]
        public void DI_ReturnsDifferentInstances()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<Service>()
                .AddSingleton<IService, Service>()
                .BuildServiceProvider();

            var byInterface = serviceProvider.GetRequiredService<IService>();
            var byExactType = serviceProvider.GetRequiredService<Service>();

            Assert.NotSame(byInterface, byExactType);
        }

        [Fact]
        public void DI_ReturnsSameInstace_LikeAutofac()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<Service>()
                .AddSingleton<IService>(sp => sp.GetRequiredService<Service>())
                .BuildServiceProvider();

            var byInterface = serviceProvider.GetRequiredService<IService>();
            var byExactType = serviceProvider.GetRequiredService<Service>();

            Assert.Same(byInterface, byExactType);
        }
    }
}
