# Step 1: Setting Up the Environment

## Welcome to AgentCon São Paulo 2025! 🎉

Hello, future agent engineer! Welcome to AgentCon São Paulo 2025, where you will take your first steps to become an expert in intelligent agents. You are about to embark on the journey of building an intelligent agent that can interact with data, access tools, and generate amazing insights on its own!

First, let's define what an **intelligent agent** is: it's a system that can make decisions, learn from the environment, and interact with users or other systems autonomously. In this tutorial, you will learn to create an intelligent agent using .NET and advanced tools.

Yes, there are more complex agents, like multi-agents that use Model Context Protocol (MCP), among others, but we need to start somewhere, right? If you want to learn more about these advanced agents, check out our guide on generative AI in .NET [here](https://aka.ms/genainet).

## Prerequisites
First, let's make sure you have everything ready to start:
- **.NET SDK**: Make sure the .NET SDK is installed on your machine. You can download it [here](https://dotnet.microsoft.com/download).
- **Code Editor**: We recommend using Visual Studio Code or Visual Studio for easier development. Download Visual Studio Code [here](https://code.visualstudio.com/).
    - *Optionally*, you can use GitHub Codespaces, which lets you code directly in the browser. Access Codespaces [here](https://github.com/features/codespaces).
- **Azure Account**: You will need an Azure account to access Foundry and other necessary tools. If you don't have an account, create one [here](https://azure.microsoft.com/free/). Don't worry, we will provide free credits for you to get started!

## What will we build?

In this tutorial, you will learn to create a **complete intelligent agent** with:
- 🛠️ **Access to code tools and infographic generation**
- 📚 **RAG (Retrieval-Augmented Generation)** with exclusive AgentCon data
- 🔄 **Iteration capability** to improve its answers

## Testing .NET

First, let's make sure .NET is working correctly in your environment:

Open the terminal (*Ctrl + `*) in VSCode or your preferred terminal and run the following command:

```bash
dotnet new console -n MyFirstAgent
```

This will create a new console project called "MyFirstAgent". Then, navigate to the project directory and run:

```bash
cd MyFirstAgent
dotnet run
```

You should see the message "Hello, World!" in the terminal.

## Installing the Required Libraries

Now that .NET is working, we need to install some essential libraries for our agent. Open the terminal in your project directory and run the following commands:

```bash
dotnet add package Azure.AI.Agents.Persistent
dotnet add package Azure.Identity
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.Json
```

These libraries allow us to work with persistent agents in Azure, manage authentication, and configure our development environment.

> **Tip**: You may need to install an extra package `Microsoft.Extensions.Configuration.Json` for JSON configuration. Use the command:

```bash
dotnet add package Microsoft.Extensions.Configuration.Json
```

## Azure Credits
To ensure you have access to the necessary resources, let's set up your Azure credits. If you don't have credits yet, follow these steps:

Go to the following link: [https://aka.ms/JoinEduLab](https://aka.ms/JoinEduLab).

You will be asked to log in to your Azure account. Use the account you created earlier or log in with an existing account.

You will be directed to the Azure for Education registration page. You will see a screen like this:

![Azure for Education registration screen](../assets/InvitationCodeScreen.png)

Enter the invitation code provided during the AgentCon São Paulo 2025 workshop. After entering the code, you will be redirected to the education page, where you can see the status of your credits and available resources. If you have any issues, notify the speaker.

> **Important**: You will receive an email to join an Azure tenant. Make sure to accept the invitation to access the resources for that tenant. It requires MFA (Multi-Factor Authentication) to ensure your account's security, so make sure you have access to your phone or another authentication method set up.

> *Very important*: Switch to the Azure for Education tenant if you created an Azure account before joining the workshop. You can do this by clicking your profile picture in the upper right corner of the Azure portal and selecting the correct tenant!

## Setting Up Azure CLI

If you are using GitHub Codespaces, you will already have Azure CLI installed, which is necessary to access Azure resources. You can check if Azure CLI is installed by running `az --version` in the terminal. If not installed, follow the installation instructions [here](https://docs.microsoft.com/cli/azure/install-azure-cli). Or use the command:
```bash
curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash
```

## Creating Resources in Foundry

With your Azure account set up, let's create the necessary resources in AI Foundry for our intelligent agent. AI Foundry is our one-stop shop for all AI resources.

There, you will find tools to create, train, and deploy intelligent agents, as well as access data, pre-trained models, evaluate models, and test before deploying your AI-powered products.

![AI Foundry](../assets/AzureAiFoundry.png)

To access Foundry, go to the Azure portal and search for "AI Foundry". You can also access it directly: [AI Foundry](https://ai.azure.com).

You will see a screen like this:
![AI Foundry screen](../assets/HomepageAIFoundry.png)

Before we start creating our agent, we need to set up the environment in Foundry. Change the environment as follows:
1. Click your name icon in the upper right corner of the Azure portal.
2. Select "Change environment" from the dropdown menu.
3. Choose the "Global Ai" environment from the available environments.

![Change Foundry environment](../assets/ChangeEnvironmentFoundry.png)
Now, we need to create some resources in Foundry, such as a project and set up the necessary credentials to access the data and tools we will use in our agent.

1. Access the Foundry portal, click on "Create new". If you don't see this option, click the "Create new agent" button at the top of the screen.

   ![Foundry home screen](../assets/HomepageAIFoundry.png)

   A screen like this will appear, leave the option "Azure Ai Foundry Resource" selected and click "Next":

   ![Foundry Create New](../assets/CreateProjectFoundry.png)

2. Create a new project, give it a name and a resource group if you wish, in the advanced options. I recommend using the long name Foundry gives you, like "pablonunes-1523". Click "Create".

   ![Creating new project](../assets/CreateProjectConfig.png)

   > **Tip**: The name in Foundry must be globally unique, so you need to choose a name that is not already in use. You can add extra numbers or letters to ensure this.

    See the waiting screen while Foundry creates the project:

    ![Foundry waiting screen](../assets/CreatingProjectFoundry.png)

   > **Important**: Foundry may take a few minutes to create the project. Be patient and wait until the project is ready.

3. Check if the project was created successfully. You will see a screen like this:

    ![Project created screen](../assets/ProjectCreatedFoundry.png)
    
    Now, you have a project in Foundry where you can create and manage your intelligent agents.

    We will use the Foundry endpoint to access the necessary data and tools for our agent. We won't need to use the API key, as we will use Azure authentication to access the resources.

4. Now, we need to create the Natural Language Model that we will use in our agent. Click on "Models and Endpoints" in the left menu. You will see a screen like this:

   ![Creating new model](../assets/CreateModelFoundry.png)

    Then, click on "Deploy Model" and then on "Deploy Base Model".

   ![Models and endpoints screen](../assets/BaseModelFoundry.png)

5. Choose the "gpt-4o-mini" model and click "Next". You will see a screen like this:

    ![Model selection screen](../assets/SelectModelFoundry.png)

    Next, you will see a model configuration screen. Leave the default options and click "Next".

    ![Model configuration screen](../assets/ModelConfigFoundry.png)

6. After creating, you will see a summary screen for the model. Access data, such as the endpoint and model name, will be displayed. Ignore these as we will use the Foundry endpoint we created earlier.

7. Done! Now you have a natural language model configured in Foundry. You can see the model in the list of models and endpoints.

    ![Models and endpoints screen](../assets/ModelEndpointFoundry.png)

## Next Steps

With the environment set up, we are ready to start building our agent! In the next step, we will create the basic structure of our assistant.

See the next step [here](Passo_2.md).

---

✅ **Checkpoint**: Make sure .NET is running, libraries are installed, credits are available, and you have access to Foundry before proceeding.
