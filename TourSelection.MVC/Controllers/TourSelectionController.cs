using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourSelection.MVC.Controllers
{
    public class TourSelectionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create(Models.TourSelection model)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(
                    exchange: "topic-exchange",
                    type: "topic");

                var routingKey = $"tour.{model.TourRequest}";

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));

                channel.BasicPublish(
                    exchange: "topic-exchange",
                    routingKey: routingKey,
                    basicProperties: null,
                    body: body);
            }

            return RedirectToAction("Index");
        }
    }
}
