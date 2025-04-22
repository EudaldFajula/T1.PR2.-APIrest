using T2.PR2._APIRESTRazorPages.Models;

namespace T2.PR2._APIRESTRazorPages.Tools
{
    public class GameTools
    {
        private readonly HttpClient _httpClient;

        public GameTools(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("AuthorizedClient");
        }

        public string ApiErrorMessage { get; set; } = string.Empty;

        public async Task<List<Game>> GetGamesAsync()
        {
            var response = await _httpClient.GetAsync("/api/Game");
            if (response.IsSuccessStatusCode)
            {
                var games = await response.Content.ReadFromJsonAsync<List<Game>>();
                return games?
                    .OrderByDescending(g => g.VotingUsers.Count)
                    .ToList() ?? new List<Game>();
            }
            else
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API Error ({response.StatusCode}): {body}");
            }
        }

        public async Task<Game> GetGameByIdAsync(int gameId)
        {
            var response = await _httpClient.GetAsync($"/api/Game/{gameId}");
            if (response.IsSuccessStatusCode)
            {
                var game = await response.Content.ReadFromJsonAsync<Game>();
                return game;
            }
            else
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API Error ({response.StatusCode}): {body}");
            }
        }

        public async Task<bool> VoteAsync(int gameId, string username)
        {
            var url = $"/api/Game/vote?gameId={gameId}&userName={username}";
            var response = await _httpClient.PostAsync(url, null);
            return response.IsSuccessStatusCode;
        }

    }
}
