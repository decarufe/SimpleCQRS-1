using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using Castle.Core;
using Castle.Windsor;
using ECommDemo.Domain;
using SimpleCqrs.Eventing;

namespace ECommDemo.Web.Support
{
    public class RuntimeBootstrapper 
    {
        private readonly ECommDemoRuntime _runtime;

        public RuntimeBootstrapper(IWindsorContainer container, IEnumerable<Assembly> assembliesToScan)
        {
            Debug.Write("Bootstrapper");
            _runtime = new ECommDemoRuntime(container
                , container.Resolve<IEventStore>()
                , container.Resolve<ISnapshotStore>()
                , assembliesToScan
            );
        }

        public void Start()
        {
            _runtime.Start();
        }

        public void Stop()
        {
           _runtime.Shutdown();
        }
    }
}