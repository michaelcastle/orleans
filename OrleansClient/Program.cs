using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Models;
using OutboundAdapter.Interfaces.Opera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrleansClient
{
    public class Program
    {
        static int Main(string[] _)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                using (var client = await ConnectClient())
                {
                    await DoClientWork(client);
                    Console.ReadKey();
                }

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nException while trying to run client: {e.Message}");
                Console.WriteLine("Make sure the silo the client is trying to connect to is running.");
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
                return 1;
            }
        }

        private static async Task<IClusterClient> ConnectClient()
        {
            IClusterClient client;
            client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OperaPmsAdapter";
                })
                .AddSimpleMessageStreamProvider("SMSProvider",
                            options =>
                            {
                                options.OptimizeForImmutableData = false;
                                options.FireAndForgetDelivery = false;
                                options.PubSubType = Orleans.Streams.StreamPubSubType.ImplicitOnly;
                            })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            //await client.Connect(RetryFilter);
            await client.Connect();

            Console.WriteLine("Client successfully connected to silo host \n");

            return client;
        }

        //private static async Task<bool> RetryFilter(Exception exception)
        //{
        //    if (exception.GetType() != typeof(SiloUnavailableException))
        //    {
        //        Console.WriteLine($"Cluster client failed to connect to cluster with unexpected error.  Exception: {exception}");
        //        return false;
        //    }
        //    attempt++;
        //    Console.WriteLine($"Cluster client attempt {attempt} of {initializeAttemptsBeforeFailing} failed to connect to cluster.  Exception: {exception}");
        //    if (attempt > initializeAttemptsBeforeFailing)
        //    {
        //        return false;
        //    }
        //    await Task.Delay(TimeSpan.FromSeconds(4));
        //    return true;
        //}

        private static async Task DoClientWork(IClusterClient client)
        {
            var currentNumbers = new List<int> { 0, 0, 0 };
            var random = new Random();
            var tasks = new List<Task<OrderItem>>();
            var hotelOrder = new List<List<OrderItem>> { new List<OrderItem>(), new List<OrderItem>(), new List<OrderItem>() };

            for (int i = 0; i < 4; i++)
            {
                var hotelId = random.Next(3);
                var currentNumber = currentNumbers[hotelId] + 1;
                currentNumbers[hotelId] = currentNumber;

                var hotel = client.GetGrain<IHotelPmsGrain>(hotelId);
                if (!await hotel.IsOutboundConnected())
                {
                    await hotel.SaveConsumerConfigurationAsync(new OutboundConfiguration
                    {
                        PmsType = nameof(Constants.Outbound.OperaCloud),
                        Url = "https://ove-osb.microsdc.us:9015"
                    });
                }

                var hotelGrain = client.GetGrain<IOutboundAdapterGrain>(hotelId);
                var response = hotelGrain.UpdateRoomStatus(currentNumber, new UpdateRoomStatus {
                    Request = @"
<UpdateRoomStatusRequest xmlns=""http://webservices.micros.com/htng/2008B/SingleGuestItinerary/Housekeeping/Types"">
    <ResortId>USOWS</ResortId>
    <RoomNumber>0105</RoomNumber>
    <RoomStatus>Dirty</RoomStatus>
</UpdateRoomStatusRequest>
"
                });
                tasks.Add(response);
            }

            while (tasks.Count > 0)
            {
                // Identify the first task that completes.
                var firstFinishedTask = await Task.WhenAny(tasks);

                // ***Remove the selected task from the list so that you don't
                // process it more than once.
                tasks.Remove(firstFinishedTask);

                // Await the completed task.
                var result = await firstFinishedTask;

                hotelOrder[result.HotelId].Add(result);

                Console.WriteLine($"Hotel: {result.HotelId} | Order: {result.Number} | TotalNumber: {result.TotalNumber}", result);
            }

            Console.WriteLine("---------------------------------------------------------");

            foreach (var hotel in hotelOrder)
            {
                if (!hotel.Any())
                {
                    continue;
                }

                var hotelId = hotel.FirstOrDefault();

                var expectedList = hotel.OrderBy(@event => @event.Number);

                var orderNumberList = string.Join(",", expectedList.Select(item => item.Number).ToList());
                var eventNumberList = string.Join(",", hotel.Select(item => item.Number).ToList());

                Console.WriteLine($"Hotel: {hotelId.HotelId} | isOrdered : {orderNumberList == eventNumberList} | {eventNumberList}");
            }
        }
    }
}