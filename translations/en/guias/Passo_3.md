# Step 3: Adding Data and RAG to the Agent

Now let's make our agent much smarter! We will add **specific data** about AgentCon 2025 and implement **RAG (Retrieval-Augmented Generation)** so our agent can answer detailed questions about the event.

## What is RAG?

RAG is a technique that combines text generation with information retrieval. This means the agent can search for specific data in a set of documents and use it to answer questions more accurately.

RAG allows the agent to access up-to-date and relevant information, significantly improving the quality of responses.

## Preparing the Data

First, make sure the `AgentCon.txt` file is in your project directory. This file contains the entire schedule for AgentCon 2025.

In the file, we have information such as times, speakers, and event sessions. The agent will use this data to answer specific questions. For example, it can inform about talk times, speaker names, and session details.

This will give the agent knowledge about the event, allowing it to answer questions like:
- "What happens at 2pm?"
- "Which event can help me with agent scaling?"
- "Who is the keynote speaker?"

> **Tip**: The `AgentCon.txt` file is already available in the `src` folder of the project and contains detailed information about times, speakers, and sessions.

## Complete Code with RAG

Replace part of the content in your `Program.cs` file with the following code:

```csharp
// ...existing code from the tutorial...
```

Also, add the following code to clean up our vector store and files after the test:

```csharp
// ...existing code from the tutorial...
```

> **Important**: If you have issues with the code, check the complete code in the repository's Passo_3.cs file. Click [here](../passos/Passo_3.cs) to see the full code.

## Running the Agent with Our Vector Search

Run the command:

```bash
dotnet run
```

> **⚠️ Important**: Make sure the `AgentCon.txt` file is in the same directory as your `Program.cs`.

## Testing Specific Knowledge

Now test the following specific questions:

### **Question 1: "What happens at 2pm?"**
The agent should respond with specific information about the 2pm sessions:
- **2:15 pm**: Contributing to Open Source with GitHub Copilot (Agent Mode) - Pachi Parra
- **2:30 pm**: From Coordination to Execution: Orchestrating Agents with MCP and A2A - Glaucio Daniel Santos

### **Question 2: "Which event can help me with agent scaling?"**
The agent should identify:
- **1:45 pm**: Multi-Agent Systems for Marketing at Scale - João Paulo Martins

### **Other questions to test:**
- "Who is the keynote speaker?"
- "Where is the event held?"
- "What is the duration of Pablo Lopes' workshop?"
- "How many people attend the Serverless GenAI session?"

## How Our Vector Search Works

Our agent is now equipped with RAG, which means it can search for specific information in the Vector Store. Here's how it works:

### **1. File Upload**
```csharp
// ...existing code from the tutorial...
```
The file is uploaded to Azure AI Foundry, processed, and a vector representation is created for search. This representation acts as an index of the information in the file, allowing us to search for specific information using semantic search (Cosine Similarity Search).

### **2. Vector Store**
```csharp
// ...existing code from the tutorial...
```
The content is processed and indexed for semantic search. Here we create a Vector Store that stores the data from `AgentCon.txt`. This Vector Store allows the agent to efficiently search for specific information. Using our file ID dictionary, the agent can quickly access relevant data.

### **3. Search Tool**
```csharp
// ...existing code from the tutorial...
```
The agent gains the ability to search for information in the data. Here, we define a search tool that allows the agent to access the Vector Store and look for relevant information. This is important because it's the first tool the agent has access to, truly making it autonomous and fulfilling the definition of an agent.

### **4. Processing**
When you ask a question, the agent:
1. Searches for relevant information in the Vector Store, in our AI Foundry, where it stores the event data and can search as needed.
2. Uses this information to complement its answer, providing context and additional details, as well as citations.
3. Provides accurate answers based on real data, using the search tool to find the correct answers.

## Summary of Improvements

| Aspect | **Without Vector** | **With Vector** |
|--------|-------------------|-----------------|
| **Knowledge** | Only general knowledge | Specific event data |
| **Accuracy** | Generic answers | Specific and accurate information |
| **Resources** | Basic | Vector Store + File Search |

## Next Steps

Now that we have an agent with specific knowledge about AgentCon, let's add even more features! In the next step, we will include code tools so our agent can execute code and generate visualizations.

See the next step [here](Passo_4.md).

---

✅ **Checkpoint**: Make sure your agent can answer specific questions about AgentCon schedules and events before proceeding to the next step.
