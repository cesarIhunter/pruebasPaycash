using CleanArch.Application.Interfaces;
using CleanArch.Core.Entities;
using CleanArch.Sql.Queries;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CleanArch.Infrastructure.Repository
{
    public class PersonRepository : IPersonRepository
    {
        #region ===[ Private Members ]=============================================================

        private readonly IConfiguration configuration;

        #endregion

        #region ===[ Constructor ]=================================================================

        public PersonRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        #endregion

        #region ===[ IContactRepository Methods ]==================================================

        public async Task<IReadOnlyList<Person>> GetAllAsync()
        {
            using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
            {
                var result = await connection.QueryAsync<Person>(PersonQueries.AllContact);

                return result.ToList();
            }
        }

        public async Task<Person> GetByIdAsync(long id)
        {
            using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
            {
                var result = await connection.QuerySingleOrDefaultAsync<Person>(PersonQueries.ContactById, new { ContactId = id });

                return result;
            }
        }

        public async Task<string> AddAsync(Person entity)
        {
            using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
            {
                var result = await connection.ExecuteAsync(PersonQueries.AddContact, entity);

                return result.ToString();
            }
        }

        public async Task<string> UpdateAsync(Person entity)
        {
            using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
            {
                var result = await connection.ExecuteAsync(PersonQueries.UpdateContact, entity);

                return result.ToString();
            }
        }

        public async Task<string> DeleteAsync(long id)
        {
            using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
            {
                var result = await connection.ExecuteAsync(PersonQueries.DeleteContact, new { ContactId = id });

                return result.ToString();
            }
        }

        #endregion
    }
}
