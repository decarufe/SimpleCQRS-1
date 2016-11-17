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
using ECommDemo.ViewModel.Inventory;
using MongoDB.Driver;
using SimpleCqrs;
using SimpleCqrs.Commanding;
using SimpleCqrs.EventStore.MongoDb;
using SimpleCqrs.Eventing;
using SimpleCqrs.Windsor;

namespace ECommDemo.Web.Support
{
    public static class AssemblyToScanProvider
    {
        public static IEnumerable<Assembly> List
        {
            get
            {
                return new Assembly[]
                {
                    typeof(NewInventoryItemCommand).Assembly,
                    typeof(NewInventoryItemCommandHandler).Assembly,
                    typeof(InventoryItemDenormalizer).Assembly 
                };
            }
        }
    }

    public class SupportInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            const string singleDb = "server=localhost;database=ecomm";
            container.AddFacility<StartableFacility>();

            container.Register(
                Component
                    .For<ITypeCatalog>()
                    .UsingFactoryMethod(() => new AssemblyTypeCatalog(AssemblyToScanProvider.List)),
                Component
                    .For<IEventStore>()
                    .UsingFactoryMethod(() => new MongoEventStore(singleDb)),

                Component
                    .For<ISnapshotStore>()
                    .UsingFactoryMethod(() => new MongoSnapshotStore(singleDb)),

                Component
                    .For<MongoDatabase>()
                    .Named("viewmodel-db")
                    .UsingFactoryMethod(() => MongoServer.Create(new MongoConnectionStringBuilder(singleDb))
                        .GetDatabase("ecomm"))
                    .LifeStyle.Singleton
            );

            container.Register(
                AllTypes.FromThisAssembly().BasedOn<IController>().LifestylePerWebRequest()
            );
        }
    }
}