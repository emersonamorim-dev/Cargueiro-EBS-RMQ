using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.EventBridge;
using Amazon.EventBridge.Model;
using Newtonsoft.Json;

namespace Cargueiro_EBS_RMQ.Controllers

{
    public class CargueiroController
    {
        private readonly IAmazonEventBridge _eventBridgeClient;
        private readonly string _busName;

        public CargueiroController(IAmazonEventBridge eventBridgeClient, string busName)
        {
            _eventBridgeClient = eventBridgeClient;
            _busName = busName;
        }

        public async Task AddOferta(CargueiroDTO oferta)
        {
            var putEventsRequest = new PutEventsRequest
            {
                Entries = new List<PutEventsRequestEntry>
                {
                    new PutEventsRequestEntry
                    {
                        Source = "CargueiroController",
                        DetailType = "NovaOferta",
                        Detail = JsonConvert.SerializeObject(oferta),
                        EventBusName = _busName
                    }
                }
            };

            await _eventBridgeClient.PutEventsAsync(putEventsRequest);
        }

        public async Task ListarOfertas(Action<CargueiroDTO> ofertasRecebidas)
        {
            var ruleName = $"rule-{Guid.NewGuid()}";

            var putRuleRequest = new PutRuleRequest
            {
                Name = ruleName,
                EventBusName = _busName,
                EventPattern = JsonConvert.SerializeObject(new
                {
                    source = new[] { "CargueiroController" },
                    detailType = new[] { "NovaOferta" }
                })
            };

            await _eventBridgeClient.PutRuleAsync(putRuleRequest);

            var targetId = $"target-{Guid.NewGuid()}";

            var target = new Target
            {
                Arn = "arn:aws:lambda:<region>:<account-id>:function:<function-name>",
                Id = targetId
            };

            var putTargetsRequest = new PutTargetsRequest
            {
                Rule = ruleName,
                EventBusName = _busName,
                Targets = new List<Target> { target }
            };

            await _eventBridgeClient.PutTargetsAsync(putTargetsRequest);

            // Espera por 1 minuto para receber as ofertas
            await Task.Delay(TimeSpan.FromMinutes(1));

            // Remove a regra e os alvos criados
            await _eventBridgeClient.RemoveTargetsAsync(new RemoveTargetsRequest
            {
                Rule = ruleName,
                EventBusName = _busName,
                Ids = new List<string> { targetId }
            });

            await _eventBridgeClient.DeleteRuleAsync(new DeleteRuleRequest
            {
                Name = ruleName,
                EventBusName = _busName
            });
        }
    }
}