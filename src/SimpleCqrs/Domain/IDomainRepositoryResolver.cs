namespace SimpleCqrs.Domain
{
    public interface IDomainRepositoryResolver
    {
        IDomainRepository GetRepository();
        void Release(IDomainRepository repository);
    }
}