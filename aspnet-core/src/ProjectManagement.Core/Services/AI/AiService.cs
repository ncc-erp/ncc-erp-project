using Abp.Dependency;
using Abp.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjectManagement.Services.AI.Dto;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static Google.Apis.Requests.BatchRequest;

namespace ProjectManagement.Services.AI
{
    public sealed class AiService : IAiService, ITransientDependency
    {
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
                MaxTokens = options.MaxTokens,
                EnableThinking = false,
                ResponseFormat = new ResponseFormat { Type = "json_object" },
                Messages = new[]
                {
                    new ChatMessage { Role = "system", Content = instructions },
                    new ChatMessage { Role = "user", Content = inputText }
                }
            };

            var aiResponse = await SendRequestAsync(options, payload, cancellationToken);
            return ParseCompletion(aiResponse);
        }

        private async Task<string> SendRequestAsync(
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

                using (var timeoutSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                {
                    timeoutSource.CancelAfter(TimeSpan.FromSeconds(options.TimeoutSeconds));
                    try
                    {
                        using (var response = await _httpClient.SendAsync(
                            request,
                            HttpCompletionOption.ResponseHeadersRead,
                            timeoutSource.Token))
                        {
                            if (!response.IsSuccessStatusCode)
                            {
                                _logger.LogWarning(
                                    "LLM provider returned HTTP {StatusCode} for model {Model}.",
                                    (int)response.StatusCode,
                                    payload.Model);
                                throw new UserFriendlyException(
                                    "Failed to generate the report.");
                            }

                            using (var stream = await response.Content.ReadAsStreamAsync())
                            using (var buffer = new MemoryStream())
                            {
                                var block = new byte[8192];
                                int read;
                                while ((read = await stream.ReadAsync(block, 0, block.Length, timeoutSource.Token)) > 0)
                                {
                                    buffer.Write(block, 0, read);
                                }
                                return Encoding.UTF8.GetString(buffer.ToArray());
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
        }

        private string ParseCompletion(string aiResponse)
        {
            try
            {
                var completion = JsonSerializer.Deserialize<ChatCompletionResponse>(
                    aiResponse,
                    JsonOptions);
                var content = completion?.Choices?.FirstOrDefault()?.Message?.Content;
                if (string.IsNullOrWhiteSpace(content))
                    throw new JsonException();
                return content.Trim();
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Failed to parse the AI provider response or extract message content.");
                throw new UserFriendlyException(
                    "Failed to get a response from AI. Please try again.");
            }
        }

    }
}
