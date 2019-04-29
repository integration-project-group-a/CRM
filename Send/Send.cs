using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System.Text;
using System.Diagnostics;


namespace Send
{
    class Send
    {
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest", Port = 5672 };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "CRM",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
                string message = "create_lead";
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "",
                routingKey: "CRM",
                basicProperties: null,
                body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

    }
}
