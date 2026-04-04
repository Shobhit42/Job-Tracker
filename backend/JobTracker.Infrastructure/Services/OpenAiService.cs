using JobTracker.Application.Features.JobApplications.DTOs;
using JobTracker.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JobTracker.Infrastructure.Services
{
    public class OpenAiService : IAiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        private readonly string _model;
        private readonly ILogger<OpenAiService> _logger;

        public OpenAiService(IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<OpenAiService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;

            var apiKey = configuration["Groq:ApiKey"] ?? throw new InvalidOperationException("Groq API key is not configured.");

            _model = configuration["Groq:Model"] ?? "llama-3.1-8b-instant";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        }
        public async Task<ExtractedJobDetailsDto?> ExtractJobDetailsAsync(string pageUrl)
        {

            string pageContent;
            try
            {
                var pageClient = _httpClientFactory.CreateClient("PageFetcher");
                pageContent = await pageClient.GetStringAsync(pageUrl);
                _logger.LogInformation("Page content preview: {Content}", pageContent[..500]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch URL: {Url}", pageUrl);
                return null;
            }

            var trimmedContent = pageContent.Length > 12000
                ? pageContent[..12000]
                : pageContent;

            var jsonTemplate = """
                {
                  "companyName": "string",
                  "jobTitle": "string",
                  "location": "string or null",
                  "jobDescription": "the responsibilities and requirements section only",
                  "skills": ["skill1", "skill2"]
                }
                """;

            var prompt = $"""
                You are a job description parser. Extract structured data from the job posting text below.

                Return ONLY a valid JSON object with these exact keys:
                {jsonTemplate}

                Keep skill names short and clean (e.g. "C#" not "proficiency in C#").
                If a field cannot be found, use null for strings and [] for arrays.

                Job posting text:
                {trimmedContent}
                """;

            try
            {
                var response = await CallOpenAiAsync(prompt);
                if (response == null) return null;
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<ExtractedJobDetailsDto>(response, options);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to extract job details.");
                return null;
            }
        }

        private async Task<string?> CallOpenAiAsync(string userPrompt)
        {
            var requestBody = new
            {
                model = _model,
                messages = new[]
                {
                    new { role = "user", content = userPrompt }
                },
                response_format = new { type = "json_object" },
                temperature = 0.1
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError("OpenAI API returned {StatusCode}: {Error}",
                    response.StatusCode, error);
                return null;
            }

            var responseJson = await response.Content.ReadAsStringAsync();

            // The OpenAI response wraps our answer inside a nested structure.
            // We dig into choices[0].message.content to get our actual JSON string.
            using var doc = JsonDocument.Parse(responseJson);
            return doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();
        }
    }
}
