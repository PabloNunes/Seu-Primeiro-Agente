# Step 2: Creating Your First Intelligent Agent

It's time to create our first intelligent agent!

In this step, we will set up the environment, create an agent, and make it answer questions about AgentCon 2025.

## Understanding How Azure AI Foundry Works

### Creating the Agent

In our code, we will use the `PersistentAgentsClient` to interact with Azure AI Foundry. This client allows us to create agents that can maintain state between interactions.

First, let's understand the command:

```csharp
PersistentAgent agent = client.Administration.CreateAgent(
    model: modelDeploymentName,
    name: "AgentCon 2025 - São Paulo",
    instructions: "You are a helpful agent for the AgentCon 2025 conference in São Paulo. Your task is to assist users with questions related to the conference, such as schedules, speakers, and events.",
    tools: null
);
```

Here, we are creating an agent called "AgentCon 2025 - São Paulo" whose main instruction is to help users with questions related to the conference.

You can see that it has no tools yet, but we will add them later.

Also, we will get the project endpoint and model name from our `appsettings.json` file.

Finally, the system message instructions are set to guide the agent in its responses. In this case, we instruct the agent to provide information about the AgentCon 2025 conference.

### Creating the Thread

To create a thread, we use the `CreateThread` method of the `PersistentAgentsClient`. This allows us to establish a session between the agent and a user.

```csharp
PersistentAgentThread thread = client.Threads.CreateThread();
```

The `PersistentAgentThread` is essential to maintain the conversation context between the user and the agent. It allows the agent to respond coherently to user questions, keeping the message history.

### Sending Messages

Now that we have our agent and thread created, let's send a message to the agent. We use the `CreateMessage` method for this:

```csharp
client.Messages.CreateMessage(
    thread.Id,
    MessageRole.User,
    "Hello, what is the schedule for AgentCon 2025 in São Paulo?");
```

In this example, we are sending a question to the agent about the AgentCon 2025 schedule. `MessageRole.User` indicates that this message is from the user, and we will look for the agent's response.

### Processing the Agent's Response

To process the agent's response, we use the `CreateRun` method of the `PersistentAgentsClient`. This starts the processing of the message by the agent:

```csharp
ThreadRun run = client.Runs.CreateRun(
    thread.Id,
    agent.Id,
    additionalInstructions: "Our schedule includes talks, panels, and workshops on AI and technology. We will also have a special session on how to use agents in dotnet, with Pablo Lopes, lasting 75 minutes.");
```

Here, we start a run for the created thread, passing the agent ID and some additional instructions. These instructions help the agent better understand the context of the question and provide a more accurate answer.

Then, we use a loop to check the run status until it is completed:

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

This loop waits until the run is completed, periodically checking the run status. When the run is finished, we can get the thread messages and display them in the console.

```csharp
Pageable<PersistentThreadMessage> messages = client.Messages.GetMessages(
    threadId: thread.Id,
    order: ListSortOrder.Ascending);
```

## Setting Up the Configuration File

Now that we have our environment set up, let's create our first intelligent agent! First, we need to configure the credentials and endpoints needed to connect to Azure AI Foundry.

### Creating appsettings.json

Create a configuration file `appsettings.json` in your project directory. This file will contain the information needed to connect to Azure AI Foundry.

In the root directory of your `MyFirstAgent` project, create a file called `appsettings.json` with the following content:

```json
{
    "ProjectEndpoint": "https://<AZURE-AI-ENDPOINT>/api/projects/<AZURE-AI-PROJECT-NAME>",
    "ModelDeploymentName": "gpt-4o-mini"
}
```

### Logging in to Azure AI Foundry

We use our Azure credentials to authenticate and access AI Foundry. For this, we use the `Azure.Identity` library we added earlier. Note that we use `DefaultAzureCredential`, which will try to authenticate using various approaches, such as environment variables, Managed Identity, and others.

To log in to Azure, you need to have the Azure CLI installed. If you don't have it, follow the instructions [here](https://docs.microsoft.com/cli/azure/install-azure-cli).

Then, use your CLI to log in to Azure:

```bash
az login --tenant <TENANT_ID>
```

> **Important**: Replace `ProjectEndpoint` with the endpoint of your project created in AI Foundry in Step 1. You can find this information on your project page in AI Foundry.

### Copying the Agent Code

Now, copy the agent code to the `Program.cs` file. Replace all the content of the `Program.cs` file with the following code:

```csharp
// ...existing code from the tutorial...
```

> **Important**: If you have issues with the code, check the complete code in the repository's Passo_1.cs file. Click [here](../passos/Passo_1.cs) to see the full code.

## Running the Agent

You will get an error related to the `appsettings.json` file not being found. Add the following lines to your `.csproj` file inside the `<ItemGroup>` section:

```xml
<None Update="appsettings.json">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
</None>
```

Your `.csproj` file should look similar to this:

```xml
// ...existing code from the tutorial...
```

Now run again:

```bash
dotnet run
```

> **Important**: If you have issues with the code, check the complete code in the repository's Passo_1.cs file. Click [here](../passos/Passo_1.cs) to see the full code.

## Testing the Agent

Congratulations! 🎉 You have created your first intelligent agent! The agent is now running and ready to answer your questions about AgentCon 2025.

You can test by asking questions like:
- "Who are the event speakers?"
- "What is the AgentCon schedule?"
- "Tell me about the .NET agents session"

The agent will respond using the GPT-4o-mini model and the instructions we defined. At this point, our agent is relatively simple - it only responds based on the model's general knowledge and the basic instructions we provided.

## Let's Chat with the Agent

Now, let's change the code so we can chat with the agent interactively. We'll add a loop that allows you to ask questions continuously until you decide to exit.

Let's update our code to include an interaction loop:

```csharp
// ...existing code from the tutorial...
```

> **Important**: If you have issues with the code, check the complete code in the repository's Passo_2.cs file. Click [here](../passos/Passo_2.cs) to see the full code.

Now, when you run the agent, it will allow you to ask questions continuously until you decide to exit by pressing Enter without typing anything.

Let's understand what was added:

- **Interaction Loop**: We added a `do-while` loop that allows the user to ask questions continuously. The loop continues until the user presses Enter without typing anything.
- **User Input**: The agent now prompts the user to type a question. If the user presses Enter without typing anything, the loop ends and the program terminates.
- **Response Processing**: After sending the user's message, the agent processes the response and displays only the latest assistant response.
- **Response Display**: The agent displays the assistant's response, allowing the user to see the answers to their questions.

Now, when you run the agent, it will allow you to ask questions continuously until you decide to exit by pressing Enter without typing anything, and it will maintain the conversation context.

## Next Steps

Now that we have a basic agent working, let's make it smarter! In the next step, we will add the information the agent needs to better understand questions and provide more accurate answers.

See the next step [here](Passo_3.md).

---

✅ **Checkpoint**: Make sure your agent is running correctly and can answer basic questions and interact continuously before proceeding to the next step.
