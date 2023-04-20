using Amazon.EventBridge.Model;
using Amazon.EventBridge;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cargueiro.Domain;

namespace Cargueiro.Domain
{
    public class CargueiroMessage
    {
        public string Origem { get; set; }
        public string Destino { get; set; }
        public int Preco { get; set; }
        public DateTime DataPartida { get; set; }

        public static CargueiroMessage FromJson(string json)
        {
            return JsonConvert.DeserializeObject<CargueiroMessage>(json);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public async Task SendCargueiroMessageAsync()
        {
            var client = new AmazonEventBridgeClient();
            var message = new CargueiroMessage { Origem = "Rio de Janeiro", Destino = "São Paulo", Preco = 1000, DataPartida = DateTime.Now };
            var request = new PutEventsRequest
            {
                Entries = new List<PutEventsRequestEntry>
        {
            new PutEventsRequestEntry
            {
                EventBusName = "my-event-bus",
                Source = "my-source",
                DetailType = "CargueiroMessage",
                Detail = message.ToJson()
            }
        }
            };
            await client.PutEventsAsync(request);

            var ruleName = "my-rule-name";
            var targetId = Guid.NewGuid().ToString();
            var target = new Target { Arn = "my-target-arn", Id = targetId };
            await client.PutTargetsAsync(new PutTargetsRequest { Rule = ruleName, Targets = new List<Target> { target } });

            await Task.Delay(TimeSpan.FromSeconds(30)); // Aguarda 30 segundos para receber as mensagens

            await client.RemoveTargetsAsync(new RemoveTargetsRequest { Rule = ruleName, Ids = new List<string> { targetId } });
        }
    }
}






