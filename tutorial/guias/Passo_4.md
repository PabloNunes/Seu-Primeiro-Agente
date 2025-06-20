# Passo 4: Adicionando Ferramenta de Código ao Agente

Agora vamos adicionar uma funcionalidade poderosa ao nosso agente: a capacidade de executar código Python e gerar visualizações! Vamos implementar a ferramenta **Code Interpreter** que permitirá ao agente criar gráficos e infográficos baseados nos dados da AgentCon.

## O que é Code Interpreter?

Code Interpreter é uma ferramenta que permite ao agente executar código Python em um ambiente isolado (sandboxed). Isso significa que o agente pode gerar gráficos, processar dados e criar visualizações de forma independente, sem precisar de um ambiente externo.

Por exemplo, o agente pode:
- Processar dados
- Executar cálculos complexos
- Gerar gráficos e visualizações
- Criar infográficos personalizados

## Como Funciona?

Quando o agente recebe uma pergunta que requer uma análise mais profunda, a criação e execução de código ou a geração de gráficos, ele faz a chamada para a ferramenta Code Interpreter, se disponível. O agente pode então gerar um script Python, executá-lo e, se necessário, retornar uma imagem ou gráfico como resposta.

Observe que as ferramentas podem colaborar entre si. Por exemplo, o agente pode usar a ferramenta de busca para encontrar dados relevantes e, em seguida, usar o Code Interpreter para gerar um gráfico com esses dados.

Vamos ver um exemplo de como o agente identifica a necessidade e ativa várias ferramentas:

O agente é questionado sobre a programação do evento e um infográfico é solicitado. Então ele usa a ferramenta de busca para encontrar os dados relevantes e, em seguida, usa o Code Interpreter para gerar o gráfico.

> **Nota**: O Code Interpreter é uma ferramenta avançada e tem mais custos associados, além dos tokens consumidos, então use com sabedoria!

## Passo a Passo para Implementar o Code Interpreter

### **Passo 1: Adicionar Referências Necessárias**
Certifique-se de que você tem as seguintes referências no seu projeto:

```csharp
using System.Diagnostics;
```

### **Passo 2: Configurar o Agente com Code Interpreter**

Vamos adicionar a ferramenta Code Interpreter ao nosso agente. Isso permitirá que o agente execute código Python e gere visualizações.

```csharp
PersistentAgent agent = client.Administration.CreateAgent(
    model: modelDeploymentName,
    name: "AgentCon 2025 - São Paulo ",
    instructions: "You are a helpful agent for the AgentCon 2025 conference in São Paulo. Your task is to assist users with questions related to the conference, such as schedules, speakers, and events.",
    tools: new List<ToolDefinition> { new FileSearchToolDefinition(), new CodeInterpreterToolDefinition() },
    toolResources: new ToolResources() { FileSearch = fileSearchToolResource });

```

Observe que agora estamos adicionando a `CodeInterpreterToolDefinition` junto com a `FileSearchToolDefinition`. Isso permite que o agente busque informações e execute código Python.


### **Passo 3: Atualizando Nosso Processamento de Informação**

Agora, vamos atualizar o código para que o agente possa gerar gráficos e infográficos com base nos dados da AgentCon.

Vamos atualizar nosso código para incluir a geração de gráficos e infográficos. Agora, podemos gerar visualizações automaticamente. Observe que estamos usando o `System.Diagnostics` para abrir os gráficos gerados, salvando os arquivos e abrindo-os.

Esse código deve ser adicionado após o processamento da resposta que contém apenas texto, no caso o `MessageTextContent`:

```csharp

// Processamento de imagens geradas
if (content is MessageImageFileContent imageFileContent)
{
    Console.WriteLine($"[Assistente]: ID da imagem = {imageFileContent.FileId}");
    BinaryData imageContent = client.Files.GetFileContent(imageFileContent.FileId);
    string tempFilePath = Path.Combine(AppContext.BaseDirectory, $"{Guid.NewGuid()}.png");
    File.WriteAllBytes(tempFilePath, imageContent.ToArray());
    client.Files.DeleteFile(imageFileContent.FileId);

    ProcessStartInfo psi = new()
    {
        FileName = tempFilePath,
        UseShellExecute = true
    };
    Process.Start(psi);
}
```

> **Nota**: se você estiver utilizando o Codespaces, o comando `Process.Start` pode não funcionar como esperado, pois ele tenta abrir o arquivo no ambiente local. Nesse caso, você pode baixar o arquivo e abri-lo manualmente.
```csharp
if (content is MessageImageFileContent imageFileContent)
{
    Console.WriteLine($"[Assistente]: ID da imagem = {imageFileContent.FileId}");
    BinaryData imageContent = client.Files.GetFileContent(imageFileContent.FileId);
    string tempFilePath = Path.Combine(AppContext.BaseDirectory, $"{Guid.NewGuid()}.png");
    File.WriteAllBytes(tempFilePath, imageContent.ToArray());
    client.Files.DeleteFile(imageFileContent.FileId);
    Console.WriteLine($"[Assistente]: A imagem foi salva em {tempFilePath}. Abra manualmente para visualizar.");
}
``` 

> **Importante**: Se tiver problemas com o código, verifique no arquivo Passo_4.cs do repositório o código completo. Clique [aqui](../passos/Passo_4.cs) para ver o código completo.

> **⚠️ Importante**: Quando temos um agente em produção, temos vários tipos que podemos retornar, como texto, imagens, vídeos, etc. Veja a implementação correta com Switch no programa Passo_5.md [aqui](../passos/Passo_5.md).

## Executando o Agente com Code Interpreter

Execute o comando:

```bash
dotnet run
```

## Testando a Geração de Código e Infográficos

Agora teste as seguintes perguntas que demonstram as novas capacidades:

### **Pergunta 1: Infográfico de Participantes**
```
Olá, sou um reporter e gostaria de saber mais sobre a AgentCon, gostaria de ter um infográfico no formato de barras dos participantes das apresentações e dos workshops.
```

O agente deve:
1. Analisar os dados da AgentCon.txt
2. Extrair informações sobre participantes
3. Gerar código Python para criar um gráfico de barras
4. Salvar e abrir automaticamente a imagem gerada

### **Pergunta 2: Infográfico com Cores por Categoria**
```
Muito obrigado, poderia botar tags de cor diferentes entre pausas, workshops e apresentações, sabendo que o local de sala de workshop só tem workshop no auditório só tem palestra, sabendo isso pode refazer.
```

O agente deve:
1. Categorizar eventos por tipo (workshop, palestra, pausa)
2. Usar cores diferentes para cada categoria
3. Gerar um gráfico mais detalhado e informativo

### **Outras perguntas para testar:**
- "Crie um gráfico de pizza mostrando a distribuição de duração das sessões"
- "Faça um timeline visual da programação do evento"
- "Gere um gráfico comparando a capacidade dos diferentes locais"

## Como Funciona o Code Interpreter

Um fato muito relevante é que o Code Interpreter é uma ferramenta que executa código em um ambiente isolado (sandboxed), permitindo que o agente gere e execute código Python de forma segura. 

O Code Interpreter suporta diversos formatos de arquivo para análise e processamento:

| Extensão | Tipo MIME |
|----------|-----------|
| .c | text/x-c |
| .cpp | text/x-c++ |
| .csv | application/csv |
| .docx | application/vnd.openxmlformats-officedocument.wordprocessingml.document |
| .html | text/html |
| .java | text/x-java |
| .json | application/json |
| .md | text/markdown |
| .pdf | application/pdf |
| .php | text/x-php |
| .pptx | application/vnd.openxmlformats-officedocument.presentationml.presentation |
| .py | text/x-python |
| .py | text/x-script.python |
| .rb | text/x-ruby |
| .tex | text/x-tex |
| .txt | text/plain |
| .css | text/css |
| .jpeg | image/jpeg |
| .jpg | image/jpeg |
| .js | text/javascript |
| .gif | image/gif |
| .png | image/png |
| .tar | application/x-tar |
| .ts | application/typescript |
| .xlsx | application/vnd.openxmlformats-officedocument.spreadsheetml.sheet |
| .xml | application/xml or text/xml |
| .zip | application/zip |

O agente agora pode:
- Executar códigos para análise
- Gerar gráficos usando vários tipos de visualizações.
- Salvar e exibir imagens automaticamente

## Resumo das Melhorias

| Aspecto | **RAG Only** | **RAG + Code Interpreter** |
|---------|-------------|--------------|
| **Ferramentas** | Apenas FileSearch | FileSearch + CodeInterpreter |
| **Capacidades** | Busca e texto | Busca + código + visualizações |
| **Outputs** | Apenas texto | Texto + imagens |

## Próximos Passos

Por esse workshop, você aprendeu a adicionar ferramentas de código ao seu agente, permitindo que ele execute scripts Python e gere visualizações automaticamente. Para melhorar ainda mais, você pode explorar a integração com Ferramentas próprias e funções de agentes, verifique como fazer isso nas samples nas nossas [samples .NET](https://github.com/azure-ai-foundry/foundry-samples/tree/main/samples/microsoft/csharp/getting-started-agents).

**Checkpoint**: Certifique-se de que seu agente consegue gerar e exibir gráficos automaticamente antes de prosseguir para o próximo passo.