namespace ProjectManagement.Services.AI.Dto
{
    internal sealed class ChatCompletionRequest
    {
        public string Model { get; set; }
        public int MaxTokens { get; set; }
        public double? Temperature { get; set; }
        public bool EnableThinking { get; set; }
        public ResponseFormat ResponseFormat { get; set; }
        public ChatMessage[] Messages { get; set; }
    }

    internal sealed class ChatCompletionResponse
    {
        public ChatCompletionChoice[] Choices { get; set; }
    }

    internal sealed class ChatCompletionChoice
    {
        public ChatMessage Message { get; set; }
    }

    internal sealed class ChatMessage
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }

    internal sealed class ResponseFormat
    {
        public string Type { get; set; }
    }
}
