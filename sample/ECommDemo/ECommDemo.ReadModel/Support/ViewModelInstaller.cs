using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace ECommDemo.ViewModel.Support
{
    public class ViewModelInstaller : IWindsorInstaller
    {
        private bool _installWriters = false;
        private bool _installReaders = false;

        public ViewModelInstaller AddReaders()
        {
            _installReaders = true;
            return this;
        }

        public ViewModelInstaller AddWriters()
        {
            _installWriters = true;
            return this;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes
                    .FromThisAssembly()
                    .Where(type => type.Name.EndsWith("Service"))
                    .WithServiceSelect((type, types) =>
                        type.GetInterfaces().Where(x =>
                                _installReaders && typeof(IReader).IsAssignableFrom(x)
                            ||  _installWriters && typeof(IWriter).IsAssignableFrom(x)
                        )
                     )
                    .Configure(registration => registration.
                        DependsOn(Dependency.OnComponent("database", "viewmodel-db"))
                    )
                    .LifestyleTransient()
            );
        }
    }
}
