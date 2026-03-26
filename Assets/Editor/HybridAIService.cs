using UnityEngine;
using UnityEditor;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System; // <-- Adicionado para TimeSpan

public class HybridAIService : EditorWindow
{
    private string prompt = "";
    private string response = "";
    private ComplexityLevel complexity = ComplexityLevel.Auto;
    private Vector2 scrollPosition;
    private string apiKey = "";
    
    [MenuItem("Tools/Hybrid AI Assistant")]
    public static void ShowWindow()
    {
        var window = GetWindow<HybridAIService>("Hybrid AI");
        window.minSize = new Vector2(400, 500);
    }
    
    private void OnEnable()
    {
        // Carregar API key salva
        apiKey = EditorPrefs.GetString("DeepSeek_API_Key", "");
    }
    
    private async void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        GUILayout.Label("Hybrid AI Assistant", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        // Configuração
        EditorGUILayout.LabelField("Configuration", EditorStyles.boldLabel);
        apiKey = EditorGUILayout.PasswordField("DeepSeek API Key:", apiKey);
        if (GUILayout.Button("Save API Key"))
        {
            EditorPrefs.SetString("DeepSeek_API_Key", apiKey);
            Debug.Log("API Key saved!");
        }
        
        EditorGUILayout.Space();
        
        complexity = (ComplexityLevel)EditorGUILayout.EnumPopup("Complexity:", complexity);
        
        EditorGUILayout.Space(10);
        
        // Prompt
        GUILayout.Label("Prompt:", EditorStyles.boldLabel);
        prompt = EditorGUILayout.TextArea(prompt, GUILayout.Height(100));
        
        EditorGUILayout.Space();
        
        // Botões
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate Code", GUILayout.Height(30)))
        {
            _ = GenerateCode(); // Usar _ para ignorar warning
        }
        
        if (GUILayout.Button("Clear", GUILayout.Height(30)))
        {
            prompt = "";
            response = "";
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(10);
        
        // Resposta
        GUILayout.Label("Response:", EditorStyles.boldLabel);
        response = EditorGUILayout.TextArea(response, GUILayout.Height(200));
        
        EditorGUILayout.EndScrollView();
    }
    
    private async Task GenerateCode()
    {
        if (string.IsNullOrEmpty(prompt))
        {
            EditorUtility.DisplayDialog("Error", "Please enter a prompt", "OK");
            return;
        }
        
        var shouldUseCloud = complexity == ComplexityLevel.High || 
                            (complexity == ComplexityLevel.Auto && IsComplexPrompt(prompt));
        
        try
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(60);
            
            if (shouldUseCloud)
            {
                if (string.IsNullOrEmpty(apiKey))
                {
                    EditorUtility.DisplayDialog("Error", "Please enter your DeepSeek API Key", "OK");
                    return;
                }
                
                await CallDeepSeekCloud(client);
            }
            else
            {
                await CallLocalOllama(client);
            }
            
            // Salvar script
            var scriptPath = EditorUtility.SaveFilePanel("Save Script", "Assets", "NewScript.cs", "cs");
            if (!string.IsNullOrEmpty(scriptPath))
            {
                var code = ExtractCodeFromResponse(response);
                await System.IO.File.WriteAllTextAsync(scriptPath, code);
                AssetDatabase.Refresh();
                EditorUtility.DisplayDialog("Success", $"Script saved to {scriptPath}", "OK");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error generating code: {ex.Message}");
            EditorUtility.DisplayDialog("Error", $"Failed to generate code: {ex.Message}", "OK");
        }
    }
    
    private async Task CallDeepSeekCloud(HttpClient client)
    {
        var requestData = new DeepSeekRequest
        {
            model = "deepseek-chat",
            messages = new List<DeepSeekMessage>
            {
                new DeepSeekMessage 
                { 
                    role = "user", 
                    content = $"Generate Unity C# code with proper formatting. Return only the code between ```csharp and ``` tags.\n\nPrompt: {prompt}"
                }
            }
        };
        
        var jsonRequest = JsonUtility.ToJson(requestData);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        
        var result = await client.PostAsync("https://api.deepseek.com/v1/chat/completions", content);
        var jsonResponse = await result.Content.ReadAsStringAsync();
        
        if (result.IsSuccessStatusCode)
        {
            var responseData = JsonUtility.FromJson<DeepSeekResponse>(jsonResponse);
            if (responseData.choices != null && responseData.choices.Count > 0)
            {
                response = responseData.choices[0].message.content;
            }
        }
        else
        {
            response = $"Error: {jsonResponse}";
        }
    }
    
    private async Task CallLocalOllama(HttpClient client)
    {
        var requestData = new OllamaRequest
        {
            model = "qwen2.5-coder:14b-instruct-q4_K_M",
            prompt = $"Generate Unity C# code with proper formatting. Return only the code between ```csharp and ``` tags.\n\n{prompt}",
            stream = false
        };
        
        var jsonRequest = JsonUtility.ToJson(requestData);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        
        try
        {
            var result = await client.PostAsync("http://localhost:11434/api/generate", content);
            var jsonResponse = await result.Content.ReadAsStringAsync();
            var responseData = JsonUtility.FromJson<OllamaResponse>(jsonResponse);
            response = responseData.response;
        }
        catch (System.Exception ex)
        {
            response = $"Error connecting to Ollama: {ex.Message}\n\nMake sure Ollama is running (ollama serve) and model is installed (ollama pull qwen2.5-coder:14b-instruct-q4_K_M)";
        }
    }
    
    private bool IsComplexPrompt(string prompt)
    {
        string[] complexKeywords = { 
            "arquitetura", "design pattern", "optimization", 
            "performance", "multithreading", "async", 
            "database", "network", "ai behavior", "save system",
            "state machine", "shader", "compute shader", "ecs",
            "job system", "burst", "custom editor", "inspector"
        };
        
        var lowerPrompt = prompt.ToLower();
        foreach (var keyword in complexKeywords)
        {
            if (lowerPrompt.Contains(keyword))
                return true;
        }
        
        return false;
    }
    
    private string ExtractCodeFromResponse(string fullResponse)
    {
        // Tentar extrair código entre ```csharp e ```
        var start = fullResponse.IndexOf("```csharp");
        if (start == -1) start = fullResponse.IndexOf("```cs");
        if (start == -1) start = fullResponse.IndexOf("```");
        
        if (start != -1)
        {
            start = fullResponse.IndexOf('\n', start) + 1;
            var end = fullResponse.IndexOf("```", start);
            
            if (end != -1)
            {
                return fullResponse.Substring(start, end - start).Trim();
            }
        }
        
        // Se não encontrar tags, retornar o texto completo
        return fullResponse;
    }
    
    // Data classes para serialização
    [System.Serializable]
    private class DeepSeekRequest
    {
        public string model;
        public List<DeepSeekMessage> messages;
    }
    
    [System.Serializable]
    private class DeepSeekMessage
    {
        public string role;
        public string content;
    }
    
    [System.Serializable]
    private class DeepSeekResponse
    {
        public List<Choice> choices;
        
        [System.Serializable]
        public class Choice
        {
            public DeepSeekMessage message;
        }
    }
    
    [System.Serializable]
    private class OllamaRequest
    {
        public string model;
        public string prompt;
        public bool stream;
    }
    
    [System.Serializable]
    private class OllamaResponse
    {
        public string response;
    }
    
    public enum ComplexityLevel
    {
        Auto,
        Low,
        High
    }
}