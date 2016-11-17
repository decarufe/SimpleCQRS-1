using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using SimpleCqrs.Eventing;

namespace ECommDemo.ViewModel.Support
{
    public class DomainEventHandlersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes
                    .FromThisAssembly()
                    .BasedOn(typeof (IHandleDomainEvents<>))
                    .WithServiceSelf()
                );
        }
    }
}
