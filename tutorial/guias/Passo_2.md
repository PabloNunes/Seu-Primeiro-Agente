# Passo 2: Criando seu Primeiro Agente Inteligente

Está na hora de criar nosso primeiro agente inteligente! 

Neste passo, vamos configurar o ambiente, criar um agente e fazer com que ele responda a perguntas sobre a AgentCon 2025.

## Vamos compreender como funciona o Azure AI Foundry

### Criando o Agente

No nosso código, vamos utilizar o `PersistentAgentsClient` para interagir com o Azure AI Foundry. Este cliente nos permitirá criar agentes que podem manter o estado entre as interações.

Primeiramente, vamos entender o commando:

```csharp
PersistentAgent agent = client.Administration.CreateAgent(
    model: modelDeploymentName,
    name: "AgentCon 2025 - São Paulo ",
    instructions: "You are a helpful agent for the AgentCon 2025 conference in São Paulo. Your task is to assist users with questions related to the conference, such as schedules, speakers, and events.",
    tools: null
);
```

Aqui, estamos criando um agente chamado "AgentCon 2025 - São Paulo" que tem como instrução principal ajudar os usuários com perguntas relacionadas à conferência. 

Você pode ver que ele está sem ferramentas (tools) por enquanto, mas vamos adicionar isso mais tarde.

Note, também, que vamos pegar do nosso arquivo `appsettings.json` o endpoint do projeto e o nome do modelo que vamos utilizar.

Por ultimo, as instruções de mensagem de sistema são definidas para guiar o agente em suas respostas. Neste caso, estamos instruindo o agente a fornecer informações sobre a conferência AgentCon 2025.

### Criando o Thread

Para criar um thread, vamos utilizar o método `CreateThread` do cliente `PersistentAgentsClient`. Isso nos permitirá estabelecer uma sessão entre o agente e um usuário.

```csharp
PersistentAgentThread thread = client.Threads.CreateThread();
```

O `PersistentAgentThread` é essencial para manter o contexto da conversa entre o usuário e o agente. Ele permite que o agente responda de forma coerente às perguntas do usuário, mantendo o histórico de mensagens.

### Enviando Mensagens

Agora que temos nosso agente e thread criados, vamos enviar uma mensagem para o agente. Usaremos o método `CreateMessage` para isso:

```csharp
client.Messages.CreateMessage(
    thread.Id,
    MessageRole.User,
    "Olá, qual é a programação do AgentCon 2025 em São Paulo?");
```
Neste exemplo, estamos enviando uma pergunta ao agente sobre a programação da AgentCon 2025. O `MessageRole.User` indica que esta mensagem é do usuário, na qual vamos buscar a completude da resposta do agente.

### Processando a Resposta do Agente

Para processar a resposta do agente, vamos utilizar o método `CreateRun` do cliente `PersistentAgentsClient`. Isso iniciará o processamento da mensagem pelo agente:

```csharp
ThreadRun run = client.Runs.CreateRun(
    thread.Id,
    agent.Id,
    additionalInstructions: "Nossa programação inclui palestras, painéis e workshops sobre IA e tecnologia. Inclusive, teremos uma sessão especial sobre como usar agentes em dotnet, com Pablo Lopes com uma duraçã de 75 minutos.");
```

Aqui, estamos iniciando um run para o thread criado, passando o ID do agente e algumas instruções adicionais. Essas instruções ajudam o agente a entender melhor o contexto da pergunta e fornecer uma resposta mais precisa.

Depois, vamos utilizar um loop para verificar o status do run até que ele seja concluído:

```csharp
do
{
    Thread.Sleep(TimeSpan.FromMilliseconds(500));
    run = client.Runs.GetRun(thread.Id, run.Id);
}
while (run.Status == RunStatus.Queued
    || run.Status == RunStatus.InProgress
    || run.Status == RunStatus.RequiresAction);
```

Este loop irá aguardar até que o run seja concluído, verificando periodicamente o status do run. Quando o run estiver concluído, podemos obter as mensagens do thread e exibi-las no console.

```csharp
do
{
    Thread.Sleep(TimeSpan.FromMilliseconds(500));
    run = client.Runs.GetRun(thread.Id, run.Id);
}
while (run.Status == RunStatus.Queued
    || run.Status == RunStatus.InProgress
    || run.Status == RunStatus.RequiresAction);
```

Este loop irá aguardar até que o run seja concluído, verificando periodicamente o status do run. Quando o run estiver concluído, podemos obter as mensagens do thread e exibi-las no console, confirmando que o agente vai receber o tipo de mensagem textual.


```csharp
Pageable<PersistentThreadMessage> messages = client.Messages.GetMessages(
    threadId: thread.Id,
    order: ListSortOrder.Ascending);
```

## Configurando o Arquivo de Configuração

Agora que temos nosso ambiente configurado, vamos criar nosso primeiro agente inteligente! Primeiro, precisamos configurar as credenciais e endpoints necessários para conectar com o Azure AI Foundry. 

### Criando o appsettings.json

Vamos criar um arquivo de configuração `appsettings.json` no diretório do seu projeto. Este arquivo conterá as informações necessárias para conectar com o Azure AI Foundry.

No diretório raiz do seu projeto `MeuPrimeiroAgente`, crie um arquivo chamado `appsettings.json` com o seguinte conteúdo:

```json
{
    "ProjectEndpoint": "https://<AZURE-AI-ENDPOINT>/api/projects/<AZURE-AI-PROJECT-NAME>",
    "ModelDeploymentName": "gpt-4o-mini"
}
```

### Login no Azure AI Foundry

Usamos nossas credenciais do Azure para autenticar e acessar o AI Foundry. Para isso, vamos utilizar a biblioteca `Azure.Identity` que já adicionamos anteriormente. Note que usamos o `DefaultAzureCredential`, que tentará autenticar usando várias abordagens, como variáveis de ambiente, Managed Identity, entre outras.

Para logar no Azure, você precisa ter a CLI do Azure instalada. Se você ainda não tem, pode instalar seguindo as instruções [aqui](https://docs.microsoft.com/cli/azure/install-azure-cli).

Nisso, use sua CLI para logar no Azure:

```bash
az login
```

> **Importante**: Substitua o `ProjectEndpoint` pelo endpoint do seu projeto criado no AI Foundry no Passo 1. Você pode encontrar essas informações na página do seu projeto no AI Foundry.

### Copiando o Código do Agente

Agora, vamos copiar o código do nosso agente para o arquivo `Program.cs`. Substitua todo o conteúdo do arquivo `Program.cs` pelo seguinte código:

```csharp
using Azure;
using Azure.AI.Agents.Persistent;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var projectEndpoint = configuration["ProjectEndpoint"];
var modelDeploymentName = configuration["ModelDeploymentName"];

//Create a PersistentAgentsClient and PersistentAgent.
PersistentAgentsClient client = new(projectEndpoint, new DefaultAzureCredential());

//Create a new agent for AgentCon 2025 - São Paulo.
PersistentAgent agent = client.Administration.CreateAgent(
    model: modelDeploymentName,
    name: "AgentCon 2025 - São Paulo ",
    instructions: "You are a helpful agent for the AgentCon 2025 conference in São Paulo. Your task is to assist users with questions related to the conference, such as schedules, speakers, and events.",
    tools: null
);

//Create a thread to establish a session between Agent and a User.
PersistentAgentThread thread = client.Threads.CreateThread();

//Ask a question of the Agent.
client.Messages.CreateMessage(
    thread.Id,
    MessageRole.User,
    "Olá, qual é a programação do AgentCon 2025 em São Paulo?");

//Have Agent beging processing user's question with some additional instructions associated with the ThreadRun.
ThreadRun run = client.Runs.CreateRun(
    thread.Id,
    agent.Id,
    additionalInstructions: "Nossa programação inclui palestras, painéis e workshops sobre IA e tecnologia. Inclusive, teremos uma sessão especial sobre como usar agentes em dotnet, com Pablo Lopes com uma duraçã de 75 minutos.");

//Poll for completion.
do
{
    Thread.Sleep(TimeSpan.FromMilliseconds(500));
    run = client.Runs.GetRun(thread.Id, run.Id);
}
while (run.Status == RunStatus.Queued
    || run.Status == RunStatus.InProgress
    || run.Status == RunStatus.RequiresAction);

//Get the messages in the PersistentAgentThread. Includes Agent (Assistant Role) and User (User Role) messages.
Pageable<PersistentThreadMessage> messages = client.Messages.GetMessages(
    threadId: thread.Id,
    order: ListSortOrder.Ascending);

//Display each message and open the image generated using CodeInterpreterToolDefinition.
foreach (PersistentThreadMessage threadMessage in messages)
{
    foreach (MessageContent content in threadMessage.ContentItems)
    {
        if (content is MessageTextContent textContent)
        {
            Console.WriteLine($"[{threadMessage.Role}] {textContent.Text}");
        }
        else
        {
            Console.WriteLine($"[{threadMessage.Role}] {content.GetType().Name} content received.");
        }
    }
}

//Clean up test resources.
client.Threads.DeleteThread(threadId: thread.Id);
client.Administration.DeleteAgent(agentId: agent.Id);
```



## Executando o Agente

Você vai receber um erro relacionado ao arquivo `appsettings.json` não sendo encontrado, adicione as seguintes linhas ao seu arquivo `.csproj` dentro da seção `<ItemGroup>`:

```xml
<None Update="appsettings.json">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
</None>
```

Seu arquivo `.csproj` deve ficar similar a este:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Azure.AI.Agents.Persistent" Version="1.0.0" />
    <PackageReference Include="Azure.Identity" Version="1.14.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration" Version="9.0.5" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="9.0.5" />
    <None Update="appsettings.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
  </ItemGroup>

</Project>

```

Agora execute novamente:

```bash
dotnet run
```

> **Importante**: Se tiver problemas com o código, verifique no arquivo Passo_1.cs do repositório o código completo. Clique [aqui](../passos/Passo_1.cs) para ver o código completo.

## Testando o Agente

Parabéns! 🎉 Você criou seu primeiro agente inteligente! O agente está agora rodando e pronto para receber suas perguntas sobre a AgentCon 2025.

Você pode testar fazendo perguntas como:
- "Quais são os palestrantes do evento?"
- "Qual é a programação da AgentCon?"
- "Me fale sobre a sessão de agentes em .NET"

O agente responderá usando o modelo GPT-4o-mini e as instruções que definimos. Neste ponto, nosso agente é relativamente simples - ele responde apenas com base no conhecimento geral do modelo e as instruções básicas que fornecemos.

## Vamos conversar com o Agente

Agora, vamos mudar o código para que possamos conversar com o agente de forma interativa. Vamos adicionar um loop que permitirá que você faça perguntas continuamente até decidir sair.

Vamos mudar nosso código para incluir um loop de interação:

```csharp
... (de baixo do CreateThread)

// Get a boolean to determine if the user wants to continue the conversation.
bool continueConversation = true;

Console.WriteLine("Bem-vindo ao AgentCon 2025 - São Paulo!");
Console.WriteLine("Você pode fazer perguntas sobre a programação do evento, palestrantes e muito mais.");
Console.WriteLine("Digite sua pergunta ou pressione Enter para sair.");

do
{
    // Make the user can continue to talk with the chatbot and send messages to the agent.
    Console.Write("Você: ");
    string userInput = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(userInput))
    {
        continueConversation = false;
        break;
    }

    // Create a new message in the PersistentAgentThread with the user's input.
    client.Messages.CreateMessage(
        thread.Id,
        MessageRole.User,
        userInput);


    // Create a new run to process the user's message with the agent.
    ThreadRun run = client.Runs.CreateRun(
        thread.Id,
        agent.Id,
        additionalInstructions: "Nossa programação inclui palestras, painéis e workshops sobre IA e tecnologia. Inclusive, teremos uma sessão especial sobre como usar agentes em dotnet, com Pablo Lopes com uma duraçã de 75 minutos.");


    // Wait for the agent to respond.
    do
    {
        Thread.Sleep(TimeSpan.FromMilliseconds(500));
        run = client.Runs.GetRun(thread.Id, run.Id);
    } while (run.Status == RunStatus.Queued
        || run.Status == RunStatus.InProgress
        || run.Status == RunStatus.RequiresAction);


    //Get the messages in the PersistentAgentThread. Includes Agent (Assistant Role) and User (User Role) messages.
    Pageable<PersistentThreadMessage> messages = client.Messages.GetMessages(
        threadId: thread.Id,
        order: ListSortOrder.Ascending,
        limit: 1);

    // Display only the latest assistant response
    var latestAssistantMessage = messages.LastOrDefault(m => m.Role == "assistant");

    if (latestAssistantMessage != null)
    {
        foreach (MessageContent content in latestAssistantMessage.ContentItems)
        {
            if (content is MessageTextContent textContent)
            {
                Console.WriteLine($"[Assistente] {textContent.Text}");
            }
        }
    }

} while (continueConversation);

... (deixe coloque o código de limpeza de recursos aqui)
```

> **Importante**: Se tiver problemas com o código, verifique no arquivo Passo_2.cs do repositório o código completo. Clique [aqui](../passos/Passo_2.cs) para ver o código completo.

Agora, quando você executar o agente, ele permitirá que você faça perguntas continuamente até que você decida sair pressionando Enter sem digitar nada.

Vamos entender o que foi adicionado:

- **Loop de Interação**: Adicionamos um loop `do-while` que permite que o usuário faça perguntas continuamente. O loop continua até que o usuário pressione Enter sem digitar nada.
- **Entrada do Usuário**: O agente agora solicita que o usuário digite uma pergunta. Se o usuário pressionar Enter sem digitar nada, o loop será encerrado e o programa terminará.
- **Processamento da Resposta**: Após enviar a mensagem do usuário, o agente processa a resposta e exibe apenas a última resposta do assistente.
- **Exibição da Resposta**: O agente exibe a resposta do assistente, permitindo que o usuário veja as respostas às suas perguntas.

Agora, quando você executar o agente, ele permitirá que você faça perguntas continuamente até que você decida sair pressionando Enter sem digitar nada, e manterá o contexto da conversa.

## Próximos Passos

Agora que temos um agente básico funcionando, vamos torná-lo mais inteligente! No próximo passo, vamos adicionar as informações que  agente precisa para que nosso agente compreenda melhor as perguntas e forneça respostas mais precisas.

Veja o próximo passo [aqui](Passo_3.md).

---

✅ **Checkpoint**: Certifique-se de que seu agente está rodando corretamente e consegue responder perguntas básicas e interagir de forma contínua antes de prosseguir para o próximo passo.