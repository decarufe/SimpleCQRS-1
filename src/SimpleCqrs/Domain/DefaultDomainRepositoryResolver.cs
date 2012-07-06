namespace SimpleCqrs.Domain
{
    public class DefaultDomainRepositoryResolver : IDomainRepositoryResolver
    {
        public DefaultDomainRepositoryResolver()
        {
        }

        public IDomainRepository GetRepository()
        {
            return ServiceLocator.Current.Resolve<IDomainRepository>();
        }

        public void Release(IDomainRepository repository)
        {
            ServiceLocator.Current.Release(repository);
        }
    }
}