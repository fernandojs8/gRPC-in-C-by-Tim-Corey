using Grpc.Net.Client;
using GrpcServer;
using System;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace GrpcClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //var helloRequest = new HelloRequest() { Name = "fer.js" };
            //var channel = GrpcChannel.ForAddress("https://localhost:5001");
            //var client = new Greeter.GreeterClient(channel);

            //var reply = await client.SayHelloAsync(helloRequest);

            //Console.WriteLine(reply.Message);

            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var customerService = new Customer.CustomerClient(channel);

            var customerRequest = new CustomerLookUpModel() { UserId = 2 };

            var reply = await customerService.GetCustomerInfoAsync(customerRequest);

            Console.WriteLine($"Firstname: {reply.FirstName}, LastName: {reply.LastName}");
            var cancellationTokenSource = new CancellationTokenSource();

            Console.WriteLine("New Customer List");

            using (var call = customerService.GetNewCustomers(new NewCustomerModel()))
            {
                while (await call.ResponseStream.MoveNext(cancellationTokenSource.Token))
                {
                    var currentCustomer = call.ResponseStream.Current;
                    Console.WriteLine($"Name: {currentCustomer.FirstName} {currentCustomer.LastName}, " +
                                          $"Email: {currentCustomer.EmailAddress}, " +
                                          $"Age: {currentCustomer.Age}");
                }
            }

            Console.ReadLine();
        }
    }
}
