using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace TourSelection.BackOffice
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(
                    exchange: "topic-exchange",
                    type: "topic");

                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queueName, "topic-exchange", "tour.*");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, e) =>
                {
                    var message = Encoding.UTF8.GetString(e.Body.ToArray());
                    var tourSelection = JsonConvert.DeserializeObject<Models.TourSelection>(message);
                    Console.WriteLine($"{tourSelection.Name} has {tourSelection.TourRequest} the {tourSelection.Tour} tour.");
                };

                channel.BasicConsume(queueName, true, consumer);
                Console.WriteLine("Back Office consumer started..");
                Console.WriteLine("awaiting all tour actions..");
                Console.ReadLine();
            }
        }
    }
}
