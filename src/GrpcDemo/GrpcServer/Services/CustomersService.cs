using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrpcServer.Services
{
    public class CustomersService : Customer.CustomerBase
    {
        private readonly ILogger _logger;

        public CustomersService(ILogger<CustomersService> logger)
        {
            _logger = logger;
        }

        public override Task<CustomerModel> GetCustomerInfo(CustomerLookUpModel request, ServerCallContext context)
        {
            CustomerModel customerModel = new CustomerModel();

            if (request.UserId == 1)
            {
                customerModel.FirstName = "Jamie";
                customerModel.LastName = "Smith";
            }
            else if(request.UserId == 2)
            {
                customerModel.FirstName = "Jane";
                customerModel.LastName = "Doe";
            }else
            {
                customerModel.FirstName = "Greg";
                customerModel.LastName = "Thomas";
            }

            return Task.FromResult(customerModel);
        }

        public override async Task GetNewCustomers(NewCustomerModel request, IServerStreamWriter<CustomerModel> responseStream, ServerCallContext context)
        {
            List<CustomerModel> customers = new List<CustomerModel>
            {
                new CustomerModel
                {
                    FirstName = "Fer",
                    LastName = "JS",
                    Age = 35,
                    EmailAddress = "fernando.js8@outlook.com",
                    IsAlive = true,
                },
                new CustomerModel
                {
                    FirstName = "Sue",
                    LastName = "Storm",
                    Age = 28,
                    EmailAddress = "sue@stormy.net",
                    IsAlive = false,
                },
                new CustomerModel
                {
                    FirstName = "Bilbo",
                    LastName = "Baggins",
                    Age = 117,
                    EmailAddress = "bilbo@midleearth.net",
                    IsAlive = false,
                },
            };

            foreach (var customer in customers)
            {
                await responseStream.WriteAsync(customer);
            }
        }
    }
}
