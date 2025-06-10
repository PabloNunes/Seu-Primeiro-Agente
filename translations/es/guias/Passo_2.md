# Paso 2: Creando tu Primer Agente Inteligente

¡Es hora de crear nuestro primer agente inteligente!

En este paso, configuraremos el entorno, crearemos un agente y haremos que responda preguntas sobre AgentCon 2025.

## Comprendiendo cómo funciona Azure AI Foundry

### Creando el Agente

En nuestro código, usaremos el `PersistentAgentsClient` para interactuar con Azure AI Foundry. Este cliente nos permite crear agentes que pueden mantener el estado entre interacciones.

Primero, entendamos el comando:

```csharp
PersistentAgent agent = client.Administration.CreateAgent(
    model: modelDeploymentName,
    name: "AgentCon 2025 - São Paulo",
    instructions: "You are a helpful agent for the AgentCon 2025 conference in São Paulo. Your task is to assist users with questions related to the conference, such as schedules, speakers, and events.",
    tools: null
);
```

Aquí, estamos creando un agente llamado "AgentCon 2025 - São Paulo" cuya instrucción principal es ayudar a los usuarios con preguntas relacionadas con la conferencia.

Puedes ver que aún no tiene herramientas, pero las agregaremos más adelante.

También obtendremos el endpoint del proyecto y el nombre del modelo desde nuestro archivo `appsettings.json`.

Por último, las instrucciones del mensaje de sistema se definen para guiar al agente en sus respuestas. En este caso, instruimos al agente para que proporcione información sobre la conferencia AgentCon 2025.

### Creando el Thread

Para crear un thread, usamos el método `CreateThread` del `PersistentAgentsClient`. Esto nos permite establecer una sesión entre el agente y un usuario.

```csharp
PersistentAgentThread thread = client.Threads.CreateThread();
```

El `PersistentAgentThread` es esencial para mantener el contexto de la conversación entre el usuario y el agente. Permite que el agente responda de manera coherente a las preguntas del usuario, manteniendo el historial de mensajes.

### Enviando Mensajes

Ahora que tenemos nuestro agente y thread creados, enviemos un mensaje al agente. Usamos el método `CreateMessage` para esto:

```csharp
client.Messages.CreateMessage(
    thread.Id,
    MessageRole.User,
    "Hola, ¿cuál es la programación de AgentCon 2025 en São Paulo?");
```

En este ejemplo, enviamos una pregunta al agente sobre la programación de AgentCon 2025. `MessageRole.User` indica que este mensaje es del usuario, y buscaremos la respuesta del agente.

### Procesando la Respuesta del Agente

Para procesar la respuesta del agente, usamos el método `CreateRun` del `PersistentAgentsClient`. Esto inicia el procesamiento del mensaje por parte del agente:

```csharp
ThreadRun run = client.Runs.CreateRun(
    thread.Id,
    agent.Id,
    additionalInstructions: "Nuestra programación incluye charlas, paneles y talleres sobre IA y tecnología. También tendremos una sesión especial sobre cómo usar agentes en dotnet, con Pablo Lopes, con una duración de 75 minutos.");
```

Aquí, iniciamos un run para el thread creado, pasando el ID del agente y algunas instrucciones adicionales. Estas instrucciones ayudan al agente a comprender mejor el contexto de la pregunta y proporcionar una respuesta más precisa.

Luego, usamos un bucle para verificar el estado del run hasta que se complete:

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

Este bucle espera hasta que el run se complete, verificando periódicamente el estado. Cuando el run finaliza, podemos obtener los mensajes del thread y mostrarlos en la consola.

```csharp
Pageable<PersistentThreadMessage> messages = client.Messages.GetMessages(
    threadId: thread.Id,
    order: ListSortOrder.Ascending);
```

## Configurando el Archivo de Configuración

Ahora que tenemos nuestro entorno configurado, ¡vamos a crear nuestro primer agente inteligente! Primero, necesitamos configurar las credenciales y endpoints necesarios para conectar con Azure AI Foundry.

### Creando appsettings.json

Crea un archivo de configuración `appsettings.json` en el directorio de tu proyecto. Este archivo contendrá la información necesaria para conectar con Azure AI Foundry.

En el directorio raíz de tu proyecto `MiPrimerAgente`, crea un archivo llamado `appsettings.json` con el siguiente contenido:

```json
{
    "ProjectEndpoint": "https://<AZURE-AI-ENDPOINT>/api/projects/<AZURE-AI-PROJECT-NAME>",
    "ModelDeploymentName": "gpt-4o-mini"
}
```

### Iniciando sesión en Azure AI Foundry

Usamos nuestras credenciales de Azure para autenticar y acceder a AI Foundry. Para esto, usamos la biblioteca `Azure.Identity` que agregamos anteriormente. Nota que usamos `DefaultAzureCredential`, que intentará autenticar usando varios métodos, como variables de entorno, Managed Identity, entre otros.

Para iniciar sesión en Azure, necesitas tener instalada la CLI de Azure. Si no la tienes, sigue las instrucciones [aquí](https://docs.microsoft.com/cli/azure/install-azure-cli).

Luego, usa tu CLI para iniciar sesión en Azure:

```bash
az login --tenant <TENANT_ID>
```

> **Importante**: Reemplaza `ProjectEndpoint` por el endpoint de tu proyecto creado en AI Foundry en el Paso 1. Puedes encontrar esta información en la página de tu proyecto en AI Foundry.

### Copiando el Código del Agente

Ahora, copia el código del agente al archivo `Program.cs`. Sustituye todo el contenido del archivo `Program.cs` por el siguiente código:

```csharp
// ...código existente del tutorial...
```

> **Importante**: Si tienes problemas con el código, revisa el código completo en el archivo Passo_1.cs del repositorio. Haz clic [aquí](../passos/Passo_1.cs) para ver el código completo.

## Ejecutando el Agente

Recibirás un error relacionado con que no se encuentra el archivo `appsettings.json`. Agrega las siguientes líneas a tu archivo `.csproj` dentro de la sección `<ItemGroup>`:

```xml
<None Update="appsettings.json">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
</None>
```

Tu archivo `.csproj` debe verse similar a esto:

```xml
// ...código existente del tutorial...
```

Ahora ejecuta nuevamente:

```bash
dotnet run
```

> **Importante**: Si tienes problemas con el código, revisa el código completo en el archivo Passo_1.cs del repositorio. Haz clic [aquí](../passos/Passo_1.cs) para ver el código completo.

## Probando el Agente

¡Felicidades! 🎉 ¡Has creado tu primer agente inteligente! El agente ahora está en ejecución y listo para responder tus preguntas sobre AgentCon 2025.

Puedes probar haciendo preguntas como:
- "¿Quiénes son los ponentes del evento?"
- "¿Cuál es la programación de AgentCon?"
- "Háblame de la sesión de agentes en .NET"

El agente responderá usando el modelo GPT-4o-mini y las instrucciones que definimos. En este punto, nuestro agente es relativamente simple: responde solo en base al conocimiento general del modelo y las instrucciones básicas que proporcionamos.

## Conversemos con el Agente

Ahora, cambiemos el código para poder conversar con el agente de forma interactiva. Agregaremos un bucle que te permitirá hacer preguntas continuamente hasta que decidas salir.

Actualicemos nuestro código para incluir un bucle de interacción:

```csharp
// ...código existente del tutorial...
```

> **Importante**: Si tienes problemas con el código, revisa el código completo en el archivo Passo_2.cs del repositorio. Haz clic [aquí](../passos/Passo_2.cs) para ver el código completo.

Ahora, cuando ejecutes el agente, te permitirá hacer preguntas continuamente hasta que decidas salir presionando Enter sin escribir nada.

Veamos qué se agregó:

- **Bucle de Interacción**: Agregamos un bucle `do-while` que permite al usuario hacer preguntas continuamente. El bucle continúa hasta que el usuario presiona Enter sin escribir nada.
- **Entrada del Usuario**: El agente ahora solicita al usuario que escriba una pregunta. Si el usuario presiona Enter sin escribir nada, el bucle termina y el programa finaliza.
- **Procesamiento de la Respuesta**: Después de enviar el mensaje del usuario, el agente procesa la respuesta y muestra solo la última respuesta del asistente.
- **Visualización de la Respuesta**: El agente muestra la respuesta del asistente, permitiendo al usuario ver las respuestas a sus preguntas.

Ahora, cuando ejecutes el agente, te permitirá hacer preguntas continuamente hasta que decidas salir presionando Enter sin escribir nada, y mantendrá el contexto de la conversación.

## Próximos Pasos

Ahora que tenemos un agente básico funcionando, ¡hagámoslo más inteligente! En el siguiente paso, agregaremos la información que el agente necesita para comprender mejor las preguntas y proporcionar respuestas más precisas.

Consulta el siguiente paso [aquí](Passo_3.md).

---

✅ **Checkpoint**: Asegúrate de que tu agente esté funcionando correctamente y pueda responder preguntas básicas e interactuar de forma continua antes de continuar al siguiente paso.
