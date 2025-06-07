# Passo 3: Adicionando Dados e RAG ao Agente

Agora vamos tornar nosso agente muito mais inteligente! Vamos adicionar **dados específicos** sobre a AgentCon 2025 e implementar **RAG (Retrieval-Augmented Generation)** para que nosso agente possa responder perguntas detalhadas sobre o evento.

## O que é RAG?

RAG é uma técnica que combina a geração de texto com a recuperação de informações. Isso significa que o agente pode buscar dados específicos em um conjunto de documentos e usá-los para responder perguntas de forma mais precisa.

RAG permite que o agente acesse informações atualizadas e relevantes, melhorando significativamente a qualidade das respostas.

## Preparando os Dados

Primeiro, certifique-se de que o arquivo `AgentCon.txt` está no diretório do seu projeto. Este arquivo contém toda a programação da AgentCon 2025.

No arquivo, temos informações como horários, palestrantes e sessões do evento. O agente usará esses dados para responder perguntas específicas. Por exemplo, ele informa sobre horários de palestras, nomes de palestrantes e detalhes das sessões.

Isso vai dar conhecimento ao agente sobre o evento, permitindo que ele responda perguntas como:
- "Que vai rolar às 2h da tarde?"
- "Qual evento que pode me ajudar com escala de agentes?"
- "Quem é o palestrante da keynote?"


> **Dica**: O arquivo `AgentCon.txt` já está disponível na pasta `src` do projeto e contém informações detalhadas sobre horários, palestrantes e sessões.

## Código Completo com RAG

Substitua todo o conteúdo do arquivo `Program.cs` pelo seguinte código:

```csharp
(... entre a criação do cliente)
// Upload a file to the Agent's file system.
// The file should contain information about the conference, such as schedules, speakers, and events.
PersistentAgentFileInfo uploadedAgentFile = client.Files.UploadFile(
    filePath: "AgentCon.txt",
    purpose: PersistentAgentFilePurpose.Agents
);

// Setup dictionary with list of File IDs for the vector store
Dictionary<string, string> fileIds = new()
{
    { uploadedAgentFile.Id, uploadedAgentFile.Filename }
};

// Create a vector store with the file and wait for it to be processed.

// If you do not specify a vector store, CreateMessage will create a vector
// store with a default expiration policy of seven days after they were last active
PersistentAgentsVectorStore vectorStore = client.VectorStores.CreateVectorStore(
    fileIds: new List<string> { uploadedAgentFile.Id },
    name: "my_vector_store");

// Create tool definition for File Search
FileSearchToolResource fileSearchToolResource = new FileSearchToolResource();
fileSearchToolResource.VectorStoreIds.Add(vectorStore.Id);

//Create a new agent for AgentCon 2025 - São Paulo.
PersistentAgent agent = client.Administration.CreateAgent(
    model: modelDeploymentName,
    name: "AgentCon 2025 - São Paulo ",
    instructions: "You are a helpful agent for the AgentCon 2025 conference in São Paulo. Your task is to assist users with questions related to the conference, such as schedules, speakers, and events.",
    tools: new List<ToolDefinition> { new FileSearchToolDefinition() },
    toolResources: new ToolResources() { FileSearch = fileSearchToolResource });

(... da criação do persistent thread)
```

Também, adcione o seguinte código para criar limpar nosso vector store e arquivos após o teste:

```csharp
(.. na parte final do código, após a execução do agente)

//Clean up test resources.
client.VectorStores.DeleteVectorStore(vectorStore.Id);
client.Files.DeleteFile(uploadedAgentFile.Id);
client.Threads.DeleteThread(threadId: thread.Id);
client.Administration.DeleteAgent(agentId: agent.Id);
``` 

> **Importante**: Se tiver problemas com o código, verifique no arquivo Passo_3.cs do repositório o código completo. Clique [aqui](../passos/Passo_3.cs) para ver o código completo.

## Executando o Agente com nossa busca vetorial

Execute o comando:

```bash
dotnet run
```

> **⚠️ Importante**: Certifique-se de que o arquivo `AgentCon.txt` está no mesmo diretório do seu `Program.cs`.

## Testando o Conhecimento Específico

Agora teste as perguntas específicas:

### **Pergunta 1: "Que vai rolar às 2h da tarde?"**
O agente deve responder com informações específicas sobre as sessões das 14h:
- **2:15 pm**: Contribuindo com Open Source com GitHub Copilot (Agent Mode) - Pachi Parra
- **2:30 pm**: Da Coordenação à Execução: Orquestrando Agentes com MCP e A2A - Glaucio Daniel Santos

### **Pergunta 2: "Qual evento que pode me ajudar com escala de agentes?"**
O agente deve identificar:
- **1:45 pm**: Multi-Agent Systems for Marketing at Scale - João Paulo Martins

### **Outras perguntas para testar:**
- "Quem é o palestrante da keynote?"
- "Onde fica o evento?"
- "Qual a duração do workshop do Pablo Lopes?"
- "Quantas pessoas participam da sessão sobre Serverless GenAI?"

## Como Funciona o nossa busca vetorial

Nosso agente agora está equipado com RAG, o que significa que ele pode buscar informações específicas no Vector Store. Aqui está como isso funciona:

### **1. Upload do Arquivo** 
```csharp
PersistentAgentFileInfo uploadedAgentFile = client.Files.UploadFile(
    filePath: "AgentCon.txt",
    purpose: PersistentAgentFilePurpose.Agents
);
```
O arquivo é enviado para o Azure AI Foundry. No qual, ele é processado e criado uma representação vetorial para busca, essa representação pode ser vista como um índice de informações contidas no arquivo, nas quais podemos tentar buscar informações específicas, através de uma busca por comparação de informações, pelo que chamamos de busca semântica (Cosine Similarity Search).

### **2. Vector Store** 
```csharp
PersistentAgentsVectorStore vectorStore = client.VectorStores.CreateVectorStore(
    fileIds: new List<string> { uploadedAgentFile.Id },
    name: "my_vector_store");
```
O conteúdo é processado e indexado para busca semântica. Aqui criamos um Vector Store que armazena os dados do arquivo `AgentCon.txt`. Esse Vector Store permite que o agente busque informações específicas de forma eficiente.
Usando o nosso dicionário de IDs de arquivos, o agente pode acessar rapidamente os dados relevantes.

### **3. Ferramenta de Busca**
```csharp
tools: new List<ToolDefinition> { new FileSearchToolDefinition() }
```
O agente ganha a capacidade de buscar informações nos dados. Aqui, definimos uma ferramenta de busca que permite ao agente acessar o Vector Store e procurar informações relevantes, isso é relevante, pois é a primeira ferramenta que o agente tem acesso, realmente deixando ele ser automato e cumprindo a definição de agente.

### **4. Processamento** 
Quando você faz uma pergunta, o agente:
1. Busca informações relevantes no Vector Store, no nosso AI Foundry, onde ele armazena os dados do evento e consegue buscar ao precisar ser requisitado pelo agente.
2. Usa essas informações para complementar sua resposta, fornecendo contexto e detalhes adicionais, além de citações.
3. Fornece respostas precisas baseadas nos dados reais, ao notar que precisa buscar informações específicas, ele utiliza a ferramenta de busca para encontrar as respostas corretas.

## Resumo das Melhorias

| Aspecto | **Sem Vetor** | **Com Vetor** |
|---------|-------------|--------------|
| **Conhecimento** | Apenas conhecimento geral | Dados específicos do evento |
| **Precisão** | Respostas genéricas | Informações específicas e precisas |
| **Recursos** | Básico | Vector Store + File Search |

## Próximos Passos

Agora que temos um agente com conhecimento específico sobre a AgentCon, vamos adicionar ainda mais funcionalidades! No próximo passo, vamos incluir ferramentas de código para que nosso agente possa executar código e gerar visualizações.

Veja o próximo passo [aqui](Passo_4.md).

---

✅ **Checkpoint**: Certifique-se de que seu agente consegue responder perguntas específicas sobre horários e eventos da AgentCon antes de prosseguir para o próximo passo.