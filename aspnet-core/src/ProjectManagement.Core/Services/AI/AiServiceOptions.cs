namespace ProjectManagement.Services.AI
{
    public sealed class AiServiceOptions
    {
        public string BaseAddress { get; set; }
        public string Token { get; set; }
        public string Model { get; set; }
        public int TimeoutSeconds { get; set; }
        public int MaxTokens { get; set; }
        public double? Temperature { get; set; }
    }
}
