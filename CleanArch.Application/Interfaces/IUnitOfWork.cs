namespace CleanArch.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IPersonRepository Persons { get; }
    }
}
