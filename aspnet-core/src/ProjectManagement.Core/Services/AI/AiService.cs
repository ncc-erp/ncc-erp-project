using Abp.Dependency;
using Abp.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjectManagement.Services.AI.Dto;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectManagement.Services.AI
{
    public sealed class AiService : IAiService, ITransientDependency
    {
        private static readonly string[] FallbackModels = { "gemini-4-hydrogen", "gpt-6-luna" };

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            IgnoreNullValues = true
        };

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AiService> _logger;

        public AiService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<AiService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient.Timeout = Timeout.InfiniteTimeSpan;
        }

        public async Task<string> GenerateAsync(
            string instructions,
            string inputText,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var options = _configuration.GetSection("AiService").Get<AiServiceOptions>();
            var payload = new ChatCompletionRequest
            {
                Model = options.Model,
                Temperature = options.Temperature,
                EnableThinking = false,
                ResponseFormat = new ResponseFormat { Type = "json_object" },
                Messages = new[]
                {
                    new ChatMessage { Role = "system", Content = instructions },
                    new ChatMessage { Role = "user", Content = inputText }
                }
            };

            return await SendWithFallbackAsync(options, payload, cancellationToken);
        }

        private async Task<string> SendWithFallbackAsync(
            AiServiceOptions options,
            ChatCompletionRequest payload,
            CancellationToken cancellationToken)
        {
            var models = new[] { payload.Model }.Concat(FallbackModels).Distinct(StringComparer.OrdinalIgnoreCase);
            foreach (var model in models)
            {
                payload.Model = model;
                var (statusCode, body) = await SendRequestAsync(options, payload, cancellationToken);
                var content = ParseCompletion(body);

                for (var retry = 0; retry <= 2; retry++)
                {
                    if (statusCode == HttpStatusCode.ServiceUnavailable || !string.IsNullOrWhiteSpace(content))
                        break;

                    _logger.LogWarning("LLM model {Model} returned empty content on attempt {Attempt}.", model, retry + 1);
                    (statusCode, body) = await SendRequestAsync(options, payload, cancellationToken);
                    content = ParseCompletion(body);
                }

                if (statusCode == HttpStatusCode.ServiceUnavailable)
                    continue;

                if (string.IsNullOrWhiteSpace(content))
                {
                    _logger.LogWarning("LLM model {Model} returned empty content after all retries.", model);
                    throw new UserFriendlyException("Failed to get a response from AI. Please try again.");
                }

                return content;
            }

            throw new UserFriendlyException("The AI service is unavailable. Please try again.");
        }

        private async Task<(HttpStatusCode StatusCode, string Body)> SendRequestAsync(
            AiServiceOptions options,
            ChatCompletionRequest payload,
            CancellationToken cancellationToken)
        {
            var endpoint = $"{options.BaseAddress}/v1/chat/completions";
            using (var request = new HttpRequestMessage(HttpMethod.Post, endpoint))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.Token);
                request.Content = new StringContent(
                    JsonSerializer.Serialize(payload, JsonOptions),
                    Encoding.UTF8,
                    "application/json");

                try
                {
                    _logger.LogInformation("Sending request to LLM model {Model}...", payload.Model);
                    using (var response = await _httpClient.SendAsync(
                        request,
                        HttpCompletionOption.ResponseHeadersRead,
                        cancellationToken))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
                                return (response.StatusCode, null);
                            _logger.LogError("LLM provider returned HTTP {StatusCode} for model {Model}.", (int)response.StatusCode, payload.Model);
                            throw new UserFriendlyException("Failed to generate the report.");
                        }

                        if (response.Content == null || response.StatusCode == HttpStatusCode.NoContent)
                            return (response.StatusCode, null);

                        using (var stream = await response.Content.ReadAsStreamAsync())
                        using (var buffer = new MemoryStream())
                        {
                            var block = new byte[8192];
                            int read;
                            while ((read = await stream.ReadAsync(block, 0, block.Length, cancellationToken)) > 0)
                            {
                                buffer.Write(block, 0, read);
                            }
                            return (response.StatusCode, Encoding.UTF8.GetString(buffer.ToArray()));
                        }
                    }
                }
                catch (OperationCanceledException ex)
                    when (!cancellationToken.IsCancellationRequested)
                {
                    _logger.LogError(ex, "LLM request timeout.");
                    throw new UserFriendlyException("The AI request timed out. Failed to generate the report.");
                }
                catch (Exception ex)
                    when (ex is HttpRequestException || ex is IOException)
                {
                    _logger.LogError(ex, "LLM request failed.");
                    throw new UserFriendlyException("Unable to connect to the LLM. Failed to generate the report.");
                }
            }
        }

        public async Task<string> GenerateTextAsync(string instructions, string inputText, CancellationToken cancellationToken)
        {
            var options = _configuration.GetSection("AiService").Get<AiServiceOptions>();
            var payload = new ChatCompletionRequest
            {
                Model = options.Model,
                Temperature = options.Temperature,
                EnableThinking = false,
                Messages = new[]
                {
                    new ChatMessage { Role = "system", Content = instructions },
                    new ChatMessage { Role = "user", Content = inputText }
                }
            };

            return await SendWithFallbackAsync(options, payload, cancellationToken);
        }

        private string ParseCompletion(string aiResponse)
        {
            if (string.IsNullOrWhiteSpace(aiResponse))
                return null;

            try
            {
                var completion = JsonSerializer.Deserialize<ChatCompletionResponse>(
                    aiResponse,
                    JsonOptions);
                return completion?.Choices?.FirstOrDefault()?.Message?.Content?.Trim();
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Failed to parse the AI provider response or extract message content.");
                throw new UserFriendlyException("Failed to get a response from AI. Please try again.");
            }
        }

    }
}
