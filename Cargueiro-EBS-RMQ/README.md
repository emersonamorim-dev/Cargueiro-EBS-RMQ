## Cargueiro EBS RMQ

Codifica��o em C#  usando abordagem EBS que interage com o servi�o Amazon EventBridge (EBS) e � respons�vel por enviar e receber eventos para a oferta de cargueiros.
Foi desenvolvido usando Arquitetura DDD para modelagem de regra de neg�cios e integrado para uso com mensageria com RabbitMQ.

### Requisitos
.NET Framework 4.7.2 ou superior
Amazon SDK para .NET
Conta AWS v�lida com as credenciais necess�rias

### Como usar
Este controlador possui dois m�todos p�blicos:

### AddOferta()
Este m�todo envia uma nova oferta de cargueiro para o Amazon EventBridge.

- Par�metros:

oferta: um objeto do tipo CargueiroDTO que representa a oferta a ser enviada.
### ListarOfertas()
Este m�todo aguarda por 1 minuto para receber novas ofertas do Amazon EventBridge e chama a a��o ofertasRecebidas para cada oferta recebida.


### Cargueiro EBS RMQ DTO
Implementado um objeto de transfer�ncia de dados (DTO) escrito em C# que representa uma oferta de cargueiro. Ele tamb�m fornece m�todos para converter a oferta de e para uma string ou JSON.

- Campos
Origem: uma string que representa a origem da oferta.
Destino: uma string que representa o destino da oferta.
Preco: um inteiro que representa o pre�o da oferta.
DataPartida: um objeto DateTime que representa a data de partida da oferta.

- Construtor
No construtor CargueiroDTO(string origem, string destino, int preco, DateTime dataPartida) cria uma nova inst�ncia de CargueiroDTO com os valores especificados.

- M�todos
ToString(): retorna uma string que representa a oferta no formato Origem|Destino|Preco|DataPartida.
FromString(string message): cria uma nova inst�ncia de CargueiroDTO a partir de uma string no formato Origem|Destino|Preco|DataPartida.
ToJson(): retorna uma string JSON que representa a oferta.
FromJson(string json): cria uma nova inst�ncia de CargueiroDTO a partir de uma string JSON que representa a oferta.


### Cargueiro Domain
Implementado um objeto de dom�nio escrito em C# que representa uma mensagem de oferta de cargueiro. Ele tamb�m fornece m�todos para converter a mensagem de e para uma string JSON, e para enviar a mensagem para um barramento de eventos do Amazon EventBridge.

- Campos
Origem: uma string que representa a origem da oferta.
Destino: uma string que representa o destino da oferta.
Preco: um inteiro que representa o pre�o da oferta.
DataPartida: um objeto DateTime que representa a data de partida da oferta.
M�todos
ToJson(): retorna uma string JSON que representa a mensagem de oferta de cargueiro.
FromJson(string json): cria uma nova inst�ncia de CargueiroMessage a partir de uma string JSON que representa a mensagem de oferta de cargueiro.
SendCargueiroMessageAsync(): envia a mensagem de oferta de cargueiro para um barramento de eventos do Amazon EventBridge. Esta opera��o tamb�m cria uma regra e um alvo para receber as mensagens enviadas e aguarda por 30 segundos para receber as mensagens. Ap�s isso, a regra e o alvo s�o removidos.


### Cargueiro Application
Implementado um servi�o escrito em C# que usa o RabbitMQ para enviar e receber mensagens de oferta de frete. Ele tamb�m fornece um objeto OfertaFrete que representa uma oferta de frete, e que fornece m�todos para converter a oferta de e para uma string.

- M�todos do servi�o
- AddOfertas(OfertaFrete oferta): adiciona uma nova oferta de frete ao RabbitMQ.
- ListarOfertas(Action<OfertaFrete> ofertasRecebidas): registra um consumidor no RabbitMQ para receber as ofertas de frete e chama o m�todo ofertasRecebidas para cada oferta recebida.
Objeto OfertaFrete
Origem: uma string que representa a origem da oferta.
Destino: uma string que representa o destino da oferta.
Preco: um double que representa o pre�o da oferta.
DataPartida: um objeto DateTime que representa a data de partida da oferta.
M�todos do objeto OfertaFrete
ToString(): retorna uma string que representa a oferta no formato Origem|Destino|Preco|DataPartida.
FromString(string str): cria uma nova inst�ncia de OfertaFrete a partir de uma string no formato Origem|Destino|Preco|DataPartida.