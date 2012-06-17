using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Castle.Facilities.Startable;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ECommDemo.Commanding.Commands;
using ECommDemo.Domain.CommandHandlers;
using SimpleCqrs;
using SimpleCqrs.Commanding;
using SimpleCqrs.EventStore.MongoDb;
using SimpleCqrs.Eventing;
using SimpleCqrs.Windsor;

namespace ECommDemo.Web.Support
{
    public class SupportInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<StartableFacility>();

            container.Register(
                Component
                    .For<ITypeCatalog>()
                    .UsingFactoryMethod(() => new AssemblyTypeCatalog(new Assembly[]
                                                                          {
                                                                             typeof(NewItemCommand).Assembly,
                                                                             typeof(NewItemCommandHandler).Assembly 
                                                                    })),
                Component
                    .For<IEventStore>()
                    .UsingFactoryMethod(() => new MongoEventStore("server=localhost;database=ecomm")),
                Component
                    .For<ISnapshotStore>()
                    .UsingFactoryMethod(() => new MongoSnapshotStore("server=localhost;database=ecomm"))
            );

            container.Register(
                AllTypes.FromThisAssembly().BasedOn<IController>().LifestylePerWebRequest()
            );
        }
    }
}