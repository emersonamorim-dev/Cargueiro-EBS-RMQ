## Cargueiro EBS RMQ

Codificação em C#  usando abordagem EBS que interage com o serviço Amazon EventBridge (EBS) e é responsável por enviar e receber eventos para a oferta de cargueiros.
Foi desenvolvido usando Arquitetura DDD para modelagem de regra de negócios e integrado para uso com mensageria com RabbitMQ.

### Requisitos
.NET Framework 4.7.2 ou superior
Amazon SDK para .NET
Conta AWS válida com as credenciais necessárias

### Como usar
Este controlador possui dois métodos públicos:

### AddOferta()
Este método envia uma nova oferta de cargueiro para o Amazon EventBridge.

- Parâmetros:

oferta: um objeto do tipo CargueiroDTO que representa a oferta a ser enviada.
### ListarOfertas()
Este método aguarda por 1 minuto para receber novas ofertas do Amazon EventBridge e chama a ação ofertasRecebidas para cada oferta recebida.


### Cargueiro EBS RMQ DTO
Implementado um objeto de transferência de dados (DTO) escrito em C# que representa uma oferta de cargueiro. Ele também fornece métodos para converter a oferta de e para uma string ou JSON.

- Campos
Origem: uma string que representa a origem da oferta.
Destino: uma string que representa o destino da oferta.
Preco: um inteiro que representa o preço da oferta.
DataPartida: um objeto DateTime que representa a data de partida da oferta.

- Construtor
No construtor CargueiroDTO(string origem, string destino, int preco, DateTime dataPartida) cria uma nova instância de CargueiroDTO com os valores especificados.

- Métodos
ToString(): retorna uma string que representa a oferta no formato Origem|Destino|Preco|DataPartida.
FromString(string message): cria uma nova instância de CargueiroDTO a partir de uma string no formato Origem|Destino|Preco|DataPartida.
ToJson(): retorna uma string JSON que representa a oferta.
FromJson(string json): cria uma nova instância de CargueiroDTO a partir de uma string JSON que representa a oferta.


### Cargueiro Domain
Implementado um objeto de domínio escrito em C# que representa uma mensagem de oferta de cargueiro. Ele também fornece métodos para converter a mensagem de e para uma string JSON, e para enviar a mensagem para um barramento de eventos do Amazon EventBridge.

- Campos
Origem: uma string que representa a origem da oferta.
Destino: uma string que representa o destino da oferta.
Preco: um inteiro que representa o preço da oferta.
DataPartida: um objeto DateTime que representa a data de partida da oferta.
Métodos
ToJson(): retorna uma string JSON que representa a mensagem de oferta de cargueiro.
FromJson(string json): cria uma nova instância de CargueiroMessage a partir de uma string JSON que representa a mensagem de oferta de cargueiro.
SendCargueiroMessageAsync(): envia a mensagem de oferta de cargueiro para um barramento de eventos do Amazon EventBridge. Esta operação também cria uma regra e um alvo para receber as mensagens enviadas e aguarda por 30 segundos para receber as mensagens. Após isso, a regra e o alvo são removidos.


### Cargueiro Application
Implementado um serviço escrito em C# que usa o RabbitMQ para enviar e receber mensagens de oferta de frete. Ele também fornece um objeto OfertaFrete que representa uma oferta de frete, e que fornece métodos para converter a oferta de e para uma string.

- Métodos do serviço
- AddOfertas(OfertaFrete oferta): adiciona uma nova oferta de frete ao RabbitMQ.
- ListarOfertas(Action<OfertaFrete> ofertasRecebidas): registra um consumidor no RabbitMQ para receber as ofertas de frete e chama o método ofertasRecebidas para cada oferta recebida.
Objeto OfertaFrete
Origem: uma string que representa a origem da oferta.
Destino: uma string que representa o destino da oferta.
Preco: um double que representa o preço da oferta.
DataPartida: um objeto DateTime que representa a data de partida da oferta.
Métodos do objeto OfertaFrete
ToString(): retorna uma string que representa a oferta no formato Origem|Destino|Preco|DataPartida.
FromString(string str): cria uma nova instância de OfertaFrete a partir de uma string no formato Origem|Destino|Preco|DataPartida.
