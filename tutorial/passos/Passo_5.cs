// using Azure;
// using Azure.AI.Agents.Persistent;
// using Azure.Identity;
// using Microsoft.Extensions.Configuration;
// using System.Diagnostics;


// IConfigurationRoot configuration = new ConfigurationBuilder()
//     .SetBasePath(AppContext.BaseDirectory)
//     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//     .Build();

// var projectEndpoint = configuration["ProjectEndpoint"];
// var modelDeploymentName = configuration["ModelDeploymentName"];

// //Create a PersistentAgentsClient and PersistentAgent.
// PersistentAgentsClient client = new(projectEndpoint, new DefaultAzureCredential());

// // Upload a file to the Agent's file system.
// // The file should contain information about the conference, such as schedules, speakers, and events.
// PersistentAgentFileInfo uploadedAgentFile = client.Files.UploadFile(
//     filePath: "AgentCon.txt",
//     purpose: PersistentAgentFilePurpose.Agents
// );

// // Setup dictionary with list of File IDs for the vector store
// Dictionary<string, string> fileIds = new()
// {
//     { uploadedAgentFile.Id, uploadedAgentFile.Filename }
// };

// // Create a vector store with the file and wait for it to be processed.

// // If you do not specify a vector store, CreateMessage will create a vector
// // store with a default expiration policy of seven days after they were last active
// PersistentAgentsVectorStore vectorStore = client.VectorStores.CreateVectorStore(
//     fileIds: new List<string> { uploadedAgentFile.Id },
//     name: "my_vector_store");

// // Create tool definition for File Search
// FileSearchToolResource fileSearchToolResource = new FileSearchToolResource();
// fileSearchToolResource.VectorStoreIds.Add(vectorStore.Id);

// //Create a new agent for AgentCon 2025 - São Paulo.
// PersistentAgent agent = client.Administration.CreateAgent(
//     model: modelDeploymentName,
//     name: "AgentCon 2025 - São Paulo ",
//     instructions: "You are a helpful agent for the AgentCon 2025 conference in São Paulo. Your task is to assist users with questions related to the conference, such as schedules, speakers, and events.",
//     tools: new List<ToolDefinition> { new FileSearchToolDefinition(), new CodeInterpreterToolDefinition() },
//     toolResources: new ToolResources() { FileSearch = fileSearchToolResource });

// //Create a thread to establish a session between Agent and a User.
// PersistentAgentThread thread = client.Threads.CreateThread();

// // Get a boolean to determine if the user wants to continue the conversation.
// bool continueConversation = true;

// Console.WriteLine("Bem-vindo ao AgentCon 2025 - São Paulo!");
// Console.WriteLine("Você pode fazer perguntas sobre a programação do evento, palestrantes e muito mais.");
// Console.WriteLine("Digite sua pergunta ou pressione Enter para sair.");

// do
// {
//     // Make the user can continue to talk with the chatbot and send messages to the agent.
//     Console.Write("Você: ");
//     string userInput = Console.ReadLine();

//     if (string.IsNullOrWhiteSpace(userInput))
//     {
//         continueConversation = false;
//         break;
//     }

//     // Create a new message in the PersistentAgentThread with the user's input.
//     client.Messages.CreateMessage(
//         thread.Id,
//         MessageRole.User,
//         userInput);


//     // Create a new run to process the user's message with the agent.
//     ThreadRun run = client.Runs.CreateRun(
//         thread.Id,
//         agent.Id,
//         additionalInstructions: "Nossa programação inclui palestras, painéis e workshops sobre IA e tecnologia. Inclusive, teremos uma sessão especial sobre como usar agentes em dotnet, com Pablo Lopes com uma duraçã de 75 minutos.");


//     // Wait for the agent to respond.
//     do
//     {
//         Thread.Sleep(TimeSpan.FromMilliseconds(500));
//         run = client.Runs.GetRun(thread.Id, run.Id);
//     } while (run.Status == RunStatus.Queued
//         || run.Status == RunStatus.InProgress
//         || run.Status == RunStatus.RequiresAction);


//     //Get the messages in the PersistentAgentThread. Includes Agent (Assistant Role) and User (User Role) messages.
//     Pageable<PersistentThreadMessage> messages = client.Messages.GetMessages(
//         threadId: thread.Id,
//         order: ListSortOrder.Ascending,
//         limit: 1);

//     // Display only the latest assistant response
//     var latestAssistantMessage = messages.LastOrDefault(m => m.Role == "assistant");

//     if (latestAssistantMessage != null)
//     {
//         foreach (MessageContent content in latestAssistantMessage.ContentItems)
//         {
//             switch (content)
//             {
//                 case MessageTextContent textContent:
//                     Console.WriteLine($"[Assistente] {textContent.Text}");
//                     break;
//                 case MessageImageFileContent imageFileContent:
//                     Console.WriteLine($"[Assistente]: ID da imagem = {imageFileContent.FileId}");
//                     BinaryData imageContent = client.Files.GetFileContent(imageFileContent.FileId);
//                     string tempFilePath = Path.Combine(AppContext.BaseDirectory, $"{Guid.NewGuid()}.png");
//                     File.WriteAllBytes(tempFilePath, imageContent.ToArray());
//                     client.Files.DeleteFile(imageFileContent.FileId);

//                     ProcessStartInfo psi = new()
//                     {
//                         FileName = tempFilePath,
//                         UseShellExecute = true
//                     };
//                     Process.Start(psi);
//                     break;
//                 default:
//                     Console.WriteLine($"[{latestAssistantMessage.Role}] {content.GetType().Name} content received.");
//                     break;
//             }
//         }
//     }

// } while (continueConversation);

// //Clean up test resources.
// client.VectorStores.DeleteVectorStore(vectorStore.Id);
// client.Files.DeleteFile(uploadedAgentFile.Id);
// client.Threads.DeleteThread(threadId: thread.Id);
// client.Administration.DeleteAgent(agentId: agent.Id);