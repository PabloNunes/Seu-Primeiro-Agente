# Paso 4: Agregando Herramienta de Código al Agente

¡Ahora vamos a agregar una funcionalidad poderosa a nuestro agente: la capacidad de ejecutar código Python y generar visualizaciones! Implementaremos la herramienta **Code Interpreter** que permitirá al agente crear gráficos e infografías basados en los datos de AgentCon.

## ¿Qué es Code Interpreter?

Code Interpreter es una herramienta que permite al agente ejecutar código Python en un entorno aislado (sandboxed). Esto significa que el agente puede generar gráficos, procesar datos y crear visualizaciones de forma independiente, sin necesidad de un entorno externo.

Por ejemplo, el agente puede:
- Procesar datos
- Ejecutar cálculos complejos
- Generar gráficos y visualizaciones
- Crear infografías personalizadas

## ¿Cómo Funciona?

Cuando el agente recibe una pregunta que requiere un análisis más profundo, la creación + ejecución de código o la generación de gráficos, llama a la herramienta Code Interpreter si está disponible. El agente puede entonces generar un script Python, ejecutarlo y, si es necesario, devolver una imagen o gráfico como respuesta.

Ten en cuenta que las herramientas pueden colaborar entre sí. Por ejemplo, el agente puede usar la herramienta de búsqueda para encontrar datos relevantes y luego usar el Code Interpreter para generar un gráfico con esos datos.

Veamos un ejemplo de cómo el agente detecta la necesidad y activa varias herramientas:

El agente es consultado sobre la programación del evento y se le pide un infográfico, entonces usa la herramienta de búsqueda para encontrar los datos relevantes y luego usa el Code Interpreter para generar el gráfico.

> **Nota**: El Code Interpreter es una herramienta avanzada y tiene más costos asociados, además de los tokens consumidos, ¡así que úsalo con sabiduría!

## Paso a Paso para Implementar el Code Interpreter

### **Paso 1: Agregar Referencias Necesarias**
Asegúrate de tener las siguientes referencias en tu proyecto:

```csharp
using System.Diagnostics;
```

### **Paso 2: Configurar el Agente con Code Interpreter**

Agreguemos la herramienta Code Interpreter a nuestro agente. Esto permitirá que el agente ejecute código Python y genere visualizaciones.

```csharp
PersistentAgent agent = client.Administration.CreateAgent(
    model: modelDeploymentName,
    name: "AgentCon 2025 - São Paulo",
    instructions: "You are a helpful agent for the AgentCon 2025 conference in São Paulo. Your task is to assist users with questions related to the conference, such as schedules, speakers, and events.",
    tools: new List<ToolDefinition> { new FileSearchToolDefinition(), new CodeInterpreterToolDefinition() },
    toolResources: new ToolResources() { FileSearch = fileSearchToolResource });
```

Observa que ahora estamos agregando `CodeInterpreterToolDefinition` junto con `FileSearchToolDefinition`. Esto permite que el agente busque información y ejecute código Python.

### **Paso 3: Actualizando nuestro procesamiento de información**

Ahora, actualicemos el código para que el agente pueda generar gráficos e infografías basados en los datos de AgentCon.

Actualicemos nuestro código para incluir la generación de gráficos e infografías. Ahora, podemos generar visualizaciones automáticamente. Ten en cuenta que usamos `System.Diagnostics` para abrir los gráficos generados, guardando los archivos y abriéndolos.

Este código debe ser agregado después de procesar la respuesta que es solo texto, en el caso de `MessageTextContent`:

```csharp
// ...código existente del tutorial...
```

> **Nota**: si usas Codespaces, el comando `Process.Start` puede no funcionar como se espera, ya que intenta abrir el archivo en el entorno local. En ese caso, puedes descargar el archivo y abrirlo manualmente.
```csharp
// ...código existente del tutorial...
```

> **Importante**: Si tienes problemas con el código, revisa el código completo en el archivo Passo_4.cs del repositorio. Haz clic [aquí](../passos/Passo_4.cs) para ver el código completo.

> **⚠️ Importante**: Cuando tenemos un agente en producción, tenemos varios tipos que podemos retornar, como texto, imágenes, videos, etc. Consulta la implementación correcta con Switch en el programa Passo_5.md [aquí](../passos/Passo_5.md).

## Ejecutando el Agente con Code Interpreter

Ejecuta el comando:

```bash
dotnet run
```

## Probando la Generación de Código e Infografías

Ahora prueba las siguientes preguntas que demuestran las nuevas capacidades:

### **Pregunta 1: Infografía de Participantes**
```
Hola, soy reportero y me gustaría saber más sobre AgentCon, quisiera tener una infografía en formato de barras de los participantes de las presentaciones y de los workshops.
```

El agente debe:
1. Analizar los datos de AgentCon.txt
2. Extraer información sobre los participantes
3. Generar código Python para crear un gráfico de barras
4. Guardar y abrir automáticamente la imagen generada

### **Pregunta 2: Infografía con Colores por Categoría**
```
Muchas gracias, ¿podrías poner etiquetas de color diferentes entre pausas, workshops y presentaciones, sabiendo que la sala de workshop solo tiene workshop y el auditorio solo tiene presentaciones? Sabiendo esto, ¿puedes rehacerlo?
```

El agente debe:
1. Categorizar eventos por tipo (workshop, presentación, pausa)
2. Usar colores diferentes para cada categoría
3. Generar un gráfico más detallado e informativo

### **Otras preguntas para probar:**
- "Crea un gráfico de pastel mostrando la distribución de duración de las sesiones"
- "Haz una línea de tiempo visual de la programación del evento"
- "Genera un gráfico comparando la capacidad de los diferentes lugares"

## ¿Cómo Funciona el Code Interpreter?

Un hecho muy relevante es que, al ser el Code Interpreter una herramienta que ejecuta código y está aislada, el agente puede generar y ejecutar código Python de forma segura.

El Code Interpreter soporta varios formatos de archivo para análisis y procesamiento:

| Extensión | Tipo MIME |
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
| .xml | application/xml o text/xml |
| .zip | application/zip |

Ahora el agente puede:
- Ejecutar código para análisis
- Generar gráficos usando varios tipos de visualizaciones
- Guardar y mostrar imágenes automáticamente

## Resumen de Mejoras

| Aspecto | **Solo RAG** | **RAG + Code Interpreter** |
|---------|-------------|---------------------------|
| **Herramientas** | Solo FileSearch | FileSearch + CodeInterpreter |
| **Capacidades** | Búsqueda y texto | Búsqueda + código + visualizaciones |
| **Salidas** | Solo texto | Texto + imágenes |

## Próximos Pasos

En este workshop, aprendiste a agregar herramientas de código a tu agente, permitiéndole ejecutar scripts Python y generar visualizaciones automáticamente. Para mejorar aún más, puedes explorar la integración con herramientas propias y funciones de agentes. Consulta cómo hacerlo en nuestros [samples .NET](https://github.com/azure-ai-foundry/foundry-samples/tree/main/samples/microsoft/csharp/getting-started-agents).

**Checkpoint**: Asegúrate de que tu agente pueda generar y mostrar gráficos automáticamente antes de continuar al siguiente paso.
