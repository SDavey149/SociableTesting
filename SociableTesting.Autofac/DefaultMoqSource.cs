using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;
using Autofac.Core.Activators.Delegate;
using Autofac.Core.Lifetime;
using Autofac.Core.Registration;
using Autofac.Extras.Moq;

namespace AutofacClassicalTesting
{
/*
* Credits to leviwilson: https://github.com/autofac/Autofac.Extras.Moq/issues/4
*/
    internal class DefaultMoqSource : IRegistrationSource
    {
        private readonly AutoMock autoMock = AutoMock.GetLoose();
    
        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<ServiceRegistration>> registrationAccessor)
        {
            var typedService = service as TypedService;
            if (typedService == null || !CanMockService(typedService) || registrationAccessor(service).Any())
            {
                return Enumerable.Empty<IComponentRegistration>();
            }

            var registration = new ComponentRegistration(
                Guid.NewGuid(), DelegateMockActivatorFor(typedService), new CurrentScopeLifetime(),
                InstanceSharing.Shared, InstanceOwnership.OwnedByLifetimeScope,
                new[] { service }, new Dictionary<string, object>());

            return new IComponentRegistration[] { registration };
        }

        public bool IsAdapterForIndividualComponents => false;

        private DelegateActivator DelegateMockActivatorFor(IServiceWithType typedService)
        {
            return new DelegateActivator(typedService.ServiceType, (c, p) => autoMock.Container.Resolve(typedService.ServiceType));
        }

        private static bool CanMockService(IServiceWithType typedService)
        {
            return !typeof(IStartable).IsAssignableFrom(typedService.ServiceType) && !IsConcrete(typedService.ServiceType);
        }

        private static bool IsConcrete(Type serviceType)
        {
            return serviceType.IsClass && !serviceType.IsAbstract && !serviceType.IsInterface;
        }
    }
}