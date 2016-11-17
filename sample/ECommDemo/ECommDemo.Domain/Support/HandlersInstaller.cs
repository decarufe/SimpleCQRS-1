using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using SimpleCqrs.Commanding;
using SimpleCqrs.Eventing;

namespace ECommDemo.Domain.Support
{
    public class HandlersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                AllTypes.FromThisAssembly()
                    .BasedOn(typeof (IHandleCommands<>)),
                AllTypes.FromThisAssembly()
                    .BasedOn(typeof (IHandleDomainEvents<>))
                    .WithServiceSelf()
                );
        }
    }
}