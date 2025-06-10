# Paso 3: Agregando Datos y RAG al Agente

¡Ahora vamos a hacer que nuestro agente sea mucho más inteligente! Agregaremos **datos específicos** sobre AgentCon 2025 e implementaremos **RAG (Retrieval-Augmented Generation)** para que nuestro agente pueda responder preguntas detalladas sobre el evento.

## ¿Qué es RAG?

RAG es una técnica que combina la generación de texto con la recuperación de información. Esto significa que el agente puede buscar datos específicos en un conjunto de documentos y usarlos para responder preguntas de forma más precisa.

RAG permite que el agente acceda a información actualizada y relevante, mejorando significativamente la calidad de las respuestas.

## Preparando los Datos

Primero, asegúrate de que el archivo `AgentCon.txt` esté en el directorio de tu proyecto. Este archivo contiene toda la programación de AgentCon 2025.

En el archivo, tenemos información como horarios, ponentes y sesiones del evento. El agente usará estos datos para responder preguntas específicas. Por ejemplo, puede informar sobre horarios de charlas, nombres de ponentes y detalles de las sesiones.

Esto le dará conocimiento al agente sobre el evento, permitiéndole responder preguntas como:
- "¿Qué pasa a las 2pm?"
- "¿Qué evento me puede ayudar con la escalabilidad de agentes?"
- "¿Quién es el ponente principal?"

> **Consejo**: El archivo `AgentCon.txt` ya está disponible en la carpeta `src` del proyecto y contiene información detallada sobre horarios, ponentes y sesiones.

## Código Completo con RAG

Sustituye parte del contenido de tu archivo `Program.cs` por el siguiente código:

```csharp
// ...código existente del tutorial...
```

Además, agrega el siguiente código para limpiar nuestro vector store y archivos después de la prueba:

```csharp
// ...código existente del tutorial...
```

> **Importante**: Si tienes problemas con el código, revisa el código completo en el archivo Passo_3.cs del repositorio. Haz clic [aquí](../passos/Passo_3.cs) para ver el código completo.

## Ejecutando el Agente con nuestra búsqueda vectorial

Ejecuta el comando:

```bash
dotnet run
```

> **⚠️ Importante**: Asegúrate de que el archivo `AgentCon.txt` esté en el mismo directorio que tu `Program.cs`.

## Probando el Conocimiento Específico

Ahora prueba las siguientes preguntas específicas:

### **Pregunta 1: "¿Qué pasa a las 2pm?"**
El agente debe responder con información específica sobre las sesiones de las 14h:
- **2:15 pm**: Contribuyendo con Open Source con GitHub Copilot (Agent Mode) - Pachi Parra
- **2:30 pm**: De la Coordinación a la Ejecución: Orquestando Agentes con MCP y A2A - Glaucio Daniel Santos

### **Pregunta 2: "¿Qué evento me puede ayudar con la escalabilidad de agentes?"**
El agente debe identificar:
- **1:45 pm**: Multi-Agent Systems for Marketing at Scale - João Paulo Martins

### **Otras preguntas para probar:**
- "¿Quién es el ponente principal?"
- "¿Dónde se realiza el evento?"
- "¿Cuál es la duración del workshop de Pablo Lopes?"
- "¿Cuántas personas participan en la sesión sobre Serverless GenAI?"

## ¿Cómo funciona nuestra búsqueda vectorial?

Ahora nuestro agente está equipado con RAG, lo que significa que puede buscar información específica en el Vector Store. Así es como funciona:

### **1. Subida de Archivo**
```csharp
// ...código existente del tutorial...
```
El archivo se sube a Azure AI Foundry, se procesa y se crea una representación vectorial para la búsqueda. Esta representación actúa como un índice de la información contenida en el archivo, permitiendo buscar información específica mediante búsqueda semántica (Cosine Similarity Search).

### **2. Vector Store**
```csharp
// ...código existente del tutorial...
```
El contenido se procesa e indexa para la búsqueda semántica. Aquí creamos un Vector Store que almacena los datos del archivo `AgentCon.txt`. Este Vector Store permite que el agente busque información específica de forma eficiente. Usando nuestro diccionario de IDs de archivos, el agente puede acceder rápidamente a los datos relevantes.

### **3. Herramienta de Búsqueda**
```csharp
// ...código existente del tutorial...
```
El agente gana la capacidad de buscar información en los datos. Aquí, definimos una herramienta de búsqueda que permite al agente acceder al Vector Store y buscar información relevante. Esto es importante porque es la primera herramienta a la que el agente tiene acceso, realmente haciéndolo autónomo y cumpliendo la definición de agente.

### **4. Procesamiento**
Cuando haces una pregunta, el agente:
1. Busca información relevante en el Vector Store, en nuestro AI Foundry, donde almacena los datos del evento y puede buscar cuando sea necesario.
2. Usa esa información para complementar su respuesta, proporcionando contexto y detalles adicionales, además de citas.
3. Proporciona respuestas precisas basadas en datos reales, usando la herramienta de búsqueda para encontrar las respuestas correctas.

## Resumen de Mejoras

| Aspecto | **Sin Vector** | **Con Vector** |
|---------|---------------|---------------|
| **Conocimiento** | Solo conocimiento general | Datos específicos del evento |
| **Precisión** | Respuestas genéricas | Información específica y precisa |
| **Recursos** | Básico | Vector Store + File Search |

## Próximos Pasos

Ahora que tenemos un agente con conocimiento específico sobre AgentCon, ¡agreguemos aún más funcionalidades! En el siguiente paso, incluiremos herramientas de código para que nuestro agente pueda ejecutar código y generar visualizaciones.

Consulta el siguiente paso [aquí](Passo_4.md).

---

✅ **Checkpoint**: Asegúrate de que tu agente pueda responder preguntas específicas sobre horarios y eventos de AgentCon antes de continuar al siguiente paso.
