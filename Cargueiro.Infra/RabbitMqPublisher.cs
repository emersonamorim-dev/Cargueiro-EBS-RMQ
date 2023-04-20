using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cargueiro.Infra
{
    public class RabbitMqPublisher
    {
        private readonly IModel _channel;
        private readonly string _queueName;

        public RabbitMqPublisher(IModel channel, string queueName)
        {
            _channel = channel;
            _queueName = queueName;
            _channel.QueueDeclare(queue: _queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public void Publish(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "",
                                 routingKey: _queueName,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
