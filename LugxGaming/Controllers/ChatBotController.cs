using LugxGaming.BusinessLogic.Interfaces;
using LugxGaming.BusinessLogic.Models.ChatBot;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace LugxGaming.Controllers
{
    public class ChatBotController : Controller
    {
        private readonly IShopInterface _shopService;
        private readonly string _apiKey;

        public ChatBotController(IShopInterface shopService, IOptions<OpenAISettings> openAiSettings)
        {
            this._shopService = shopService;
            this._apiKey = openAiSettings.Value.ApiKey;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetChatBotResponse([FromBody]string message)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var games = await this._shopService.GetAllGames();
                    var allGamesListed = games
                        .Select(g => $"{g.GameName} - ${g.Price} - {g.GameGenre} - Promo Price: {g.PromoPrice}")
                        .ToList();

                    var storeContext = string.Join("\n", allGamesListed);

                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

                    var requestBody = new
                    {
                        model = "gpt-4.1-mini",
                        input = $"""
                            You are the AI assistant for the LugxGaming online store.
                            Only recommend games that exist in the LugxGaming catalog below.
                            If the user asks about a game not listed, say it is not available.

                            LugxGaming Store Catalog:
                            {storeContext}

                            User question:
                            {message}
                            """
                    };

                    var json = JsonConvert.SerializeObject(requestBody);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("https://api.openai.com/v1/responses", content);
                    if (!response.IsSuccessStatusCode)
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        var errorObj = JsonConvert.DeserializeObject<OpenAIErrorResponse>(error);
                        throw new Exception($"{errorObj.Error.Code}: {errorObj.Error.Message}");
                    }

                    var responseJson = await response.Content.ReadAsStringAsync();

                    dynamic result = JsonConvert.DeserializeObject(responseJson);
                    string botReply = result.output[0].content[0].text;

                    return Json(new { Ok = true, response = botReply });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Ok = false, response = ex.Message });
            }
        }
    }
}