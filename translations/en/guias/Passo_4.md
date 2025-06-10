# Step 4: Adding Code Tool to the Agent

Now let's add a powerful feature to our agent: the ability to execute Python code and generate visualizations! We will implement the **Code Interpreter** tool, which will allow the agent to create charts and infographics based on AgentCon data.

## What is Code Interpreter?

Code Interpreter is a tool that allows the agent to execute Python code in a sandboxed (isolated) environment. This means the agent can independently generate charts, process data, and create visualizations without needing an external environment.

For example, the agent can:
- Process data
- Execute complex calculations
- Generate charts and visualizations
- Create custom infographics

## How Does It Work?

When the agent receives a question that requires deeper analysis, code creation + execution, or chart generation, it calls the Code Interpreter tool if available. The agent can then generate a Python script, execute it, and, if needed, return an image or chart as a response.

Note that tools can collaborate. For example, the agent can use the search tool to find relevant data and then use the Code Interpreter to generate a chart with that data.

Let's see an example of how the agent notices the need and activates multiple tools:

The agent is asked about the event schedule and for an infographic, so it uses the search tool to find relevant data and then uses the Code Interpreter to generate the chart.

> **Note**: The Code Interpreter is an advanced tool and has higher costs, in addition to consumed tokens, so use it wisely!

## Step by Step to Implement the Code Interpreter

### **Step 1: Add Required References**
Make sure you have the following references in your project:

```csharp
using System.Diagnostics;
```

### **Step 2: Configure the Agent with Code Interpreter**

Let's add the Code Interpreter tool to our agent. This will allow the agent to execute Python code and generate visualizations.

```csharp
PersistentAgent agent = client.Administration.CreateAgent(
    model: modelDeploymentName,
    name: "AgentCon 2025 - São Paulo",
    instructions: "You are a helpful agent for the AgentCon 2025 conference in São Paulo. Your task is to assist users with questions related to the conference, such as schedules, speakers, and events.",
    tools: new List<ToolDefinition> { new FileSearchToolDefinition(), new CodeInterpreterToolDefinition() },
    toolResources: new ToolResources() { FileSearch = fileSearchToolResource });
```

Note that we are now adding `CodeInterpreterToolDefinition` along with `FileSearchToolDefinition`. This allows the agent to search for information and execute Python code.

### **Step 3: Updating Our Information Processing**

Now, let's update the code so the agent can generate charts and infographics based on AgentCon data.

Let's update our code to include chart and infographic generation. Now, we can generate visualizations automatically. Note that we use `System.Diagnostics` to open the generated charts, saving the files and opening them.

This code should be added after processing the response that is just text, in the case of `MessageTextContent`:

```csharp
// ...existing code from the tutorial...
```

> **Note**: If you are using Codespaces, the `Process.Start` command may not work as expected, as it tries to open the file in the local environment. In this case, you can download the file and open it manually.
```csharp
// ...existing code from the tutorial...
```

> **Important**: If you have issues with the code, check the complete code in the repository's Passo_4.cs file. Click [here](../passos/Passo_4.cs) to see the full code.

> **⚠️ Important**: When we have a production agent, we have various types that we can return, such as text, images, videos, etc. See the correct implementation with Switch in the Passo_5.md program [here](../passos/Passo_5.md).

## Running the Agent with Code Interpreter

Run the command:

```bash
dotnet run
```

## Testing Code and Infographic Generation

Now test the following questions that demonstrate the new capabilities:

### **Question 1: Participants Infographic**
```
Hello, I'm a reporter and would like to know more about AgentCon. I would like to have a bar infographic of the participants in the presentations and workshops.
```

The agent should:
1. Analyze the data from AgentCon.txt
2. Extract information about participants
3. Generate Python code to create a bar chart
4. Save and automatically open the generated image

### **Question 2: Infographic with Colors by Category**
```
Thank you very much, could you use different color tags between breaks, workshops, and presentations, knowing that the workshop room only has workshops and the auditorium only has presentations? Knowing this, can you redo it?
```

The agent should:
1. Categorize events by type (workshop, presentation, break)
2. Use different colors for each category
3. Generate a more detailed and informative chart

### **Other questions to test:**
- "Create a pie chart showing the distribution of session durations"
- "Make a visual timeline of the event schedule"
- "Generate a chart comparing the capacity of different venues"

## How Code Interpreter Works

A very relevant fact is that because the Code Interpreter is a tool that executes code and is sandboxed, the agent can safely generate and execute Python code.

The Code Interpreter supports various file formats for analysis and processing:

| Extension | MIME Type |
|-----------|-----------|
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

The agent can now:
- Execute code for analysis
- Generate charts using various types of visualizations
- Save and display images automatically

## Summary of Improvements

| Aspect | **RAG Only** | **RAG + Code Interpreter** |
|--------|-------------|---------------------------|
| **Tools** | Only FileSearch | FileSearch + CodeInterpreter |
| **Capabilities** | Search and text | Search + code + visualizations |
| **Outputs** | Only text | Text + images |

## Next Steps

In this workshop, you learned to add code tools to your agent, allowing it to execute Python scripts and generate visualizations automatically. To improve even more, you can explore integration with custom tools and agent functions. Check out how to do this in our [samples .NET](https://github.com/azure-ai-foundry/foundry-samples/tree/main/samples/microsoft/csharp/getting-started-agents).

**Checkpoint**: Make sure your agent can generate and display charts automatically before proceeding to the next step.
