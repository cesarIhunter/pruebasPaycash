using CleanArch.Application.Interfaces;

namespace CleanArch.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IPersonRepository personRepository)
        {
            Persons = personRepository;
        }

        public IPersonRepository Persons { get; set; }
    }
}
