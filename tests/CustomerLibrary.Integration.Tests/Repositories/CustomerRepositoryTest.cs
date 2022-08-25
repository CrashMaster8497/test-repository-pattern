using CustomerLibrary.BusinessEntities;
using CustomerLibrary.Repositories;

namespace CustomerLibrary.Integration.Tests.Repositories
{
    [Collection("CustomerLibraryTests")]
    public class CustomerRepositoryTest
    {
        [Fact]
        public void ShouldBeAbleToCreateCustomerRepository()
        {
            CustomerRepositoryFixture.DeleteAllCustomers();

            var customerRepository = CustomerRepositoryFixture.GetCustomerRepository();

            Assert.NotNull(customerRepository);
        }

        [Fact]
        public void ShouldBeAbleToCreateAndReadCustomer()
        {
            CustomerRepositoryFixture.DeleteAllCustomers();

            var customer = GenerateDefaultCustomer();
            int? customerId = CustomerRepositoryFixture.CreateCustomer(customer);

            Assert.NotNull(customerId);

            var createdCustomer = CustomerRepositoryFixture.ReadCustomer((int)customerId);

            Assert.NotNull(createdCustomer);
            Assert.Equal(customer.FirstName, createdCustomer.FirstName);
            Assert.Equal(customer.LastName, createdCustomer.LastName);
            Assert.Equal(customer.PhoneNumber, createdCustomer.PhoneNumber);
            Assert.Equal(customer.Email, createdCustomer.Email);
            Assert.Equal(customer.TotalPurchasesAmount, createdCustomer.TotalPurchasesAmount);
        }

        [Fact]
        public void ShouldNotReadWithWrongId()
        {
            CustomerRepositoryFixture.DeleteAllCustomers();

            var customer = GenerateDefaultCustomer();
            int? customerId = CustomerRepositoryFixture.CreateCustomer(customer);

            var readCustomer = CustomerRepositoryFixture.ReadCustomer((int)customerId + 1);

            Assert.Null(readCustomer);
        }

        [Fact]
        public void ShouldBeAbleToUpdateCustomer()
        {
            CustomerRepositoryFixture.DeleteAllCustomers();

            var customer = GenerateDefaultCustomer();
            int? customerId = CustomerRepositoryFixture.CreateCustomer(customer);
            var createdCustomer = CustomerRepositoryFixture.ReadCustomer((int)customerId);

            createdCustomer.PhoneNumber = "+12112111111";
            bool isUpdated = CustomerRepositoryFixture.UpdateCustomer(createdCustomer);
            var updatedCustomer = CustomerRepositoryFixture.ReadCustomer((int)customerId);

            Assert.True(isUpdated);
            Assert.Equal(customer.FirstName, updatedCustomer.FirstName);
            Assert.Equal(customer.LastName, updatedCustomer.LastName);
            Assert.Equal("+12112111111", updatedCustomer.PhoneNumber);
            Assert.Equal(customer.Email, updatedCustomer.Email);
            Assert.Equal(customer.TotalPurchasesAmount, updatedCustomer.TotalPurchasesAmount);
        }

        [Fact]
        public void ShouldNotUpdateWithWrongId()
        {
            CustomerRepositoryFixture.DeleteAllCustomers();

            var customer = GenerateDefaultCustomer();
            int? customerId = CustomerRepositoryFixture.CreateCustomer(customer);
            var createdCustomer = CustomerRepositoryFixture.ReadCustomer((int)customerId);

            createdCustomer.CustomerId++;
            createdCustomer.PhoneNumber = "+12112111111";
            bool isUpdated = CustomerRepositoryFixture.UpdateCustomer(createdCustomer);
            var updatedCustomer = CustomerRepositoryFixture.ReadCustomer((int)customerId);

            Assert.False(isUpdated);
            Assert.Equal(customer.FirstName, updatedCustomer.FirstName);
            Assert.Equal(customer.LastName, updatedCustomer.LastName);
            Assert.Equal(customer.PhoneNumber, updatedCustomer.PhoneNumber);
            Assert.Equal(customer.Email, updatedCustomer.Email);
            Assert.Equal(customer.TotalPurchasesAmount, updatedCustomer.TotalPurchasesAmount);
        }

        [Fact]
        public void ShouldBeAbleToDeleteCustomer()
        {
            CustomerRepositoryFixture.DeleteAllCustomers();

            var customer = GenerateDefaultCustomer();
            int? customerId = CustomerRepositoryFixture.CreateCustomer(customer);

            bool isDeleted = CustomerRepositoryFixture.DeleteCustomer((int)customerId);

            Assert.True(isDeleted);

            var readCustomer = CustomerRepositoryFixture.ReadCustomer((int)customerId);

            Assert.Null(readCustomer);
        }

        [Fact]
        public void ShouldNotDeleteWithWrongId()
        {
            CustomerRepositoryFixture.DeleteAllCustomers();

            var customer = GenerateDefaultCustomer();
            int? customerId = CustomerRepositoryFixture.CreateCustomer(customer);

            bool isDeleted = CustomerRepositoryFixture.DeleteCustomer((int)customerId + 1);

            Assert.False(isDeleted);

            var readCustomer = CustomerRepositoryFixture.ReadCustomer((int)customerId);

            Assert.NotNull(readCustomer);
        }

        private static Customer GenerateDefaultCustomer()
        {
            return new Customer
            {
                FirstName = "first",
                LastName = "last",
                PhoneNumber = "+12002000000",
                Email = "a@b.c",
                TotalPurchasesAmount = 0
            };
        }
    }

    public class CustomerRepositoryFixture
    {
        public static CustomerRepository GetCustomerRepository()
        {
            return new CustomerRepository();
        }

        public static void DeleteAllCustomers()
        {
            var customerRepository = new CustomerRepository();
            customerRepository.DeleteAll();
        }

        public static int? CreateCustomer(Customer customer)
        {
            var customerRepository = new CustomerRepository();
            return customerRepository.Create(customer);
        }

        public static Customer ReadCustomer(int customerId)
        {
            var customerRepository = new CustomerRepository();
            return customerRepository.Read(customerId);
        }

        public static bool UpdateCustomer(Customer customer)
        {
            var customerRepository = new CustomerRepository();
            return customerRepository.Update(customer);
        }

        public static bool DeleteCustomer(int customerId)
        {
            var customerRepository = new CustomerRepository();
            return customerRepository.Delete(customerId);
        }
    }
}
