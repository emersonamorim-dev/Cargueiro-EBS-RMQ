using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cargueiro.Infra.RabbitMq
{
    public class RabbitMqConnection
    {
        private readonly IConnection _connection;

        public RabbitMqConnection(string hostName)
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            _connection = factory.CreateConnection();
        }

        public IModel CreateChannel()
        {
            return _connection.CreateModel();
        }
    }
}
