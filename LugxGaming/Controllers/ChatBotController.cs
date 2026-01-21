using LugxGaming.BusinessLogic.Models.ChatBot;
using LugxGaming.Data.Data.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;
using OpenAI;
using OpenAI.Chat;
using System.Text;

namespace LugxGaming.Controllers
{
    public class ChatBotController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetChatBotResponse(string message)
        {
            //try
            //{
            // TODO:
            // Change 'ApiKey' and implement the ChatBot functionality
            //var apiKey = "YOUR_OPENAI_API_KEY";

            //using (var client = new HttpClient())
            //{
            //    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            //    var requestBody = new
            //    {
            //        model = "gpt-4.1",
            //        messages = new[]
            //        {
            //        new { role = Roles.User.ToString(), content = message }
            //    }
            //    };

            //    var json = JsonConvert.SerializeObject(requestBody);
            //    var content = new StringContent(json, Encoding.UTF8, "application/json");

            //    var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
            //    if (!response.IsSuccessStatusCode)
            //    {
            //        var error = await response.Content.ReadAsStringAsync();
            //        var errorObj = JsonConvert.DeserializeObject<OpenAIErrorResponse>(error);
            //        throw new Exception($"{errorObj.Error.Code}: {errorObj.Error.Message}");
            //    }

            //    var responseJson = await response.Content.ReadAsStringAsync();

            //    dynamic result = JsonConvert.DeserializeObject(responseJson);
            //    string botReply = result.choices[0].message.content;

            //    return Json(new { Ok = true, response = botReply });


            // TODO:
            // Change 'ApiKey' and implement the ChatBot functionality
            //var apiKey = "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";

            //var client = new ChatClient(model: "text", apiKey: apiKey);

            //ChatCompletion response = client.CompleteChat(message);

            //var text = response.Content[0].Text;

            //return Json(new { Ok = true, response = text });
            //}
            //catch (Exception ex)
            //{
            //    return Json(new { Ok = false, response = ex.Message });
            //}

            return Json(Ok());
        }
    }
}