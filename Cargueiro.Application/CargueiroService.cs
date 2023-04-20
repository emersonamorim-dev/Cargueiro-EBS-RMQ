using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Cargueiro.Application

{
    public class CargueiroService
    {
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        public CargueiroService()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _queueName = "ofertas_frete";
            _channel.QueueDeclare(queue: _queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public void AddOfertas(OfertaFrete oferta)
        {
            var body = Encoding.UTF8.GetBytes(oferta.ToString());
            _channel.BasicPublish(exchange: "",
                                 routingKey: _queueName,
                                 basicProperties: null,
                                 body: body);
        }

        public void ListarOfertas(Action<OfertaFrete> ofertasRebecidas)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var oferta = OfertaFrete.FromString(message);
                ofertasRebecidas(oferta);
            };
            _channel.BasicConsume(queue: _queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }

    public class OfertaFrete
    {
        public string Origem { get; set; }
        public string Destino { get; set; }
        public double Preco { get; set; }
        public DateTime DataPartida { get; set; }

        public OfertaFrete(string origem, string destino, double preco, DateTime dataPartida)
        {
            Origem = origem;
            Destino = destino;
            Preco = preco;
            DataPartida = dataPartida;
        }

        public override string ToString()
        {
            return $"{Origem}|{Destino}|{Preco}|{DataPartida}";
        }

        public static OfertaFrete FromString(string str)
        {
            var parts = str.Split('|');
            return new OfertaFrete(parts[0], parts[1], double.Parse(parts[2]), DateTime.Parse(parts[3]));
        }
    }
}