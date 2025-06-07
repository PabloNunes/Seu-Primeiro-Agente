// using Azure;
// using Azure.AI.Agents.Persistent;
// using Azure.Identity;
// using Microsoft.Extensions.Configuration;

// IConfigurationRoot configuration = new ConfigurationBuilder()
//     .SetBasePath(AppContext.BaseDirectory)
//     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//     .Build();

// var projectEndpoint = configuration["ProjectEndpoint"];
// var modelDeploymentName = configuration["ModelDeploymentName"];

// //Create a PersistentAgentsClient and PersistentAgent.
// PersistentAgentsClient client = new(projectEndpoint, new DefaultAzureCredential());

// //Create a new agent for AgentCon 2025 - São Paulo.
// PersistentAgent agent = client.Administration.CreateAgent(
//     model: modelDeploymentName,
//     name: "AgentCon 2025 - São Paulo ",
//     instructions: "You are a helpful agent for the AgentCon 2025 conference in São Paulo. Your task is to assist users with questions related to the conference, such as schedules, speakers, and events.",
//     tools: null
// );

// //Create a thread to establish a session between Agent and a User.
// PersistentAgentThread thread = client.Threads.CreateThread();

// //Ask a question of the Agent.
// client.Messages.CreateMessage(
//     thread.Id,
//     MessageRole.User,
//     "Olá, qual é a programação do AgentCon 2025 em São Paulo?");

// //Have Agent beging processing user's question with some additional instructions associated with the ThreadRun.
// ThreadRun run = client.Runs.CreateRun(
//     thread.Id,
//     agent.Id,
//     additionalInstructions: "Nossa programação inclui palestras, painéis e workshops sobre IA e tecnologia. Inclusive, teremos uma sessão especial sobre como usar agentes em dotnet, com Pablo Lopes com uma duraçã de 75 minutos.");

// //Poll for completion.
// do
// {
//     Thread.Sleep(TimeSpan.FromMilliseconds(500));
//     run = client.Runs.GetRun(thread.Id, run.Id);
// }
// while (run.Status == RunStatus.Queued
//     || run.Status == RunStatus.InProgress
//     || run.Status == RunStatus.RequiresAction);

// //Get the messages in the PersistentAgentThread. Includes Agent (Assistant Role) and User (User Role) messages.
// Pageable<PersistentThreadMessage> messages = client.Messages.GetMessages(
//     threadId: thread.Id,
//     order: ListSortOrder.Ascending);

// //Display each message and open the image generated using CodeInterpreterToolDefinition.
// foreach (PersistentThreadMessage threadMessage in messages)
// {
//     foreach (MessageContent content in threadMessage.ContentItems)
//     {
//         if (content is MessageTextContent textContent)
//         {
//             Console.WriteLine($"[{threadMessage.Role}] {textContent.Text}");
//         }
//         else
//         {
//             Console.WriteLine($"[{threadMessage.Role}] {content.GetType().Name} content received.");
//         }
//     }
// }

// //Clean up test resources.
// client.Threads.DeleteThread(threadId: thread.Id);
// client.Administration.DeleteAgent(agentId: agent.Id);