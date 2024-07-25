using CleanArch.Api.Controllers;
using CleanArch.Application.Interfaces;
using CleanArch.Core.Entities;
using CleanArch.Infrastructure.Repository;
using CleanArch.Test.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArch.Test.IntegrationTests
{
    [TestClass]
    public class PersonControllerShould
    {
        #region ===[ Private Members ]=============================================================

        protected readonly IConfigurationRoot _configuration;
        private readonly PersonController _controllerObj;
        private readonly PersonController _moqControllerObj;
        private readonly Mock<IUnitOfWork> _moqRepo;

        #endregion

        #region ===[ Constructor ]=================================================================

        /// <summary>
        /// Person
        /// </summary>
        public PersonControllerShould()
        {
            _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                      .AddJsonFile("appsettings.json")
                                                      .Build();

            var repository = new PersonRepository(_configuration);
            var unitofWork = new UnitOfWork(repository);

            _controllerObj = new PersonController(unitofWork);

            _moqRepo = new Mock<IUnitOfWork>();
            _moqControllerObj = new PersonController(_moqRepo.Object);
        }

        #endregion

        #region ===[ Test Methods ]================================================================

        [TestMethod]
        public async Task AddUpdateDeleteAndGetContact()
        {
            //Add Contact
            await SaveContact();

            //Get All Contact
            var contact = await GetAll();

            //Update Contact
            await UpdateContact(contact);

            var contactId = contact.ContactId ?? 0;
            //Get Contact By Id
            await GetById(contactId);

            //Delete Contact
            await DeleteContact(contactId);
        }

        [TestMethod]
        public async Task GetAll_Throw_Exception()
        {
            //SQL Exception Test.
            _moqRepo.Setup(x => x.Persons.GetAllAsync()).Throws(TestConstants.GetSqlException());

            var result = await _moqControllerObj.GetAll();

            Assert.IsFalse(result.Success);
            Assert.IsNull(result.Result);

            //General Exception Test.
            _moqRepo.Setup(x => x.Persons.GetAllAsync()).Throws(TestConstants.GetGeneralException());

            result = await _moqControllerObj.GetAll();

            Assert.IsFalse(result.Success);
            Assert.IsNull(result.Result);
        }

        [TestMethod]
        public async Task GetById_Throw_Exception()
        {
            //SQL Exception Test.
            _moqRepo.Setup(x => x.Persons.GetByIdAsync(It.IsAny<long>())).Throws(TestConstants.GetSqlException());

            var result = await _moqControllerObj.GetById(1);

            Assert.IsFalse(result.Success);
            Assert.IsNull(result.Result);

            //General Exception Test.
            _moqRepo.Setup(x => x.Persons.GetByIdAsync(It.IsAny<long>())).Throws(TestConstants.GetGeneralException());

            result = await _moqControllerObj.GetById(1);

            Assert.IsFalse(result.Success);
            Assert.IsNull(result.Result);
        }

        [TestMethod]
        public async Task Add_Throw_Exception()
        {
            //SQL Exception Test.
            _moqRepo.Setup(x => x.Persons.AddAsync(It.IsAny<Person>())).Throws(TestConstants.GetSqlException());

            var result = await _moqControllerObj.Add(new Person());

            Assert.IsFalse(result.Success);

            //General Exception Test.
            _moqRepo.Setup(x => x.Persons.AddAsync(It.IsAny<Person>())).Throws(TestConstants.GetGeneralException());

            result = await _moqControllerObj.Add(new Person());

            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public async Task Update_Throw_Exception()
        {
            //SQL Exception Test.
            _moqRepo.Setup(x => x.Persons.UpdateAsync(It.IsAny<Person>())).Throws(TestConstants.GetSqlException());

            var result = await _moqControllerObj.Update(new Person());

            Assert.IsFalse(result.Success);

            //General Exception Test.
            _moqRepo.Setup(x => x.Persons.UpdateAsync(It.IsAny<Person>())).Throws(TestConstants.GetGeneralException());

            result = await _moqControllerObj.Update(new Person());

            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public async Task Delete_Throw_Exception()
        {
            //SQL Exception Test.
            _moqRepo.Setup(x => x.Persons.DeleteAsync(It.IsAny<long>())).Throws(TestConstants.GetSqlException());

            var result = await _moqControllerObj.Delete(1);

            Assert.IsFalse(result.Success);

            //General Exception Test.
            _moqRepo.Setup(x => x.Persons.DeleteAsync(It.IsAny<long>())).Throws(TestConstants.GetGeneralException());

            result = await _moqControllerObj.Delete(1);

            Assert.IsFalse(result.Success);
        }

        #endregion

        #region ===[ Private Methods ]=============================================================

        private async Task SaveContact()
        {
            var contact = new Person
            {
                FirstName = TestConstants.ContactTest.FirstName,
                LastName = TestConstants.ContactTest.LastName,
                Email = TestConstants.ContactTest.Email,
                PhoneNumber = TestConstants.ContactTest.PhoneNumber
            };

            var result = await _controllerObj.Add(contact);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        private async Task UpdateContact(Person contact)
        {
            //Update Email Address.
            contact.Email = TestConstants.ContactTest.NewEmail;
            var result = await _controllerObj.Update(contact);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        private async Task<Person> GetAll()
        {
            var result = await _controllerObj.GetAll();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Result?.Count > 0);

            var contact = result.Result
                            .Where(x => x.FirstName == TestConstants.ContactTest.FirstName
                                    && x.LastName == TestConstants.ContactTest.LastName
                                    && x.Email == TestConstants.ContactTest.Email
                                    && x.PhoneNumber == TestConstants.ContactTest.PhoneNumber).First();

            return contact;
        }

        private async Task GetById(int contactId)
        {
            var result = await _controllerObj.GetById(contactId);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Result);
        }

        private async Task DeleteContact(int contactId)
        {
            var result = await _controllerObj.Delete(contactId);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        #endregion
    }
}
