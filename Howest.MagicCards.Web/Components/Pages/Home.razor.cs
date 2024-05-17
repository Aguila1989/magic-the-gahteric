using Amazon.SecurityToken.Model;
using Howest.MagicCards.Shared.DTO;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using WebAPI.Wrappers;

namespace Howest.MagicCards.Web.Components.Pages
{
    public partial class Home
    {
        private string _message = string.Empty;

        private IEnumerable<CardDTO> _cards = new List<CardDTO>();
        private IEnumerable<RarityDTO> _rarities = new List<RarityDTO>();
        private IEnumerable<TypeDTO> _types = new List<TypeDTO>();
        private IEnumerable<DeckCardDTO> _deckCards = new List<DeckCardDTO>();

        private readonly JsonSerializerOptions _jsonOptions;

        private HttpClient _httpClient;
        private HttpClient _httpMinimalClient;

        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; }
        [Inject]
        public NavigationManager NavManager { get; set; }

        public Home()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }


        protected override async Task OnInitializedAsync()
        {
            _httpClient = HttpClientFactory.CreateClient("WebAPI");
            _httpMinimalClient = HttpClientFactory.CreateClient("MinimalAPI");
            _rarities = await GetResponse<RarityDTO>("rarities");
            _cards = await GetPagedResponse<CardDTO>("Cards");
            _types = await GetResponse<TypeDTO>("types");
            _deckCards = await GetCardsDeck();

        }

        protected async Task<IEnumerable<T>> GetPagedResponse<T>(string call)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(call);
            string apiResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                PagedResponse<IEnumerable<T>> result = JsonSerializer.Deserialize<PagedResponse<IEnumerable<T>>>(apiResponse, _jsonOptions);
                return result.Data;
            }
            else
            {
                string errorMessage = $"Error: {response.ReasonPhrase}";
                //throw new HttpRequestException(errorMessage);
                return new List<T>();
            }
        }

        protected async Task<IEnumerable<T>> GetResponse<T>(string call)
        {

            HttpResponseMessage response = await _httpClient.GetAsync(call);

            if (response.IsSuccessStatusCode)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                IEnumerable<T> result = JsonSerializer.Deserialize<IEnumerable<T>>(apiResponse, _jsonOptions);
                return result;
            }
            else
            {
                string errorMessage = $"Error: {response.ReasonPhrase}";
                throw new HttpRequestException(errorMessage);
            }
        }

        protected async Task AddCard(int cardId)
        {
            DeckCardDTO deckCard = new DeckCardDTO();
            HttpResponseMessage response = await _httpMinimalClient.PutAsJsonAsync($"deckCards/{cardId}", deckCard);

            if (response.IsSuccessStatusCode)
            {
                // update ui deck
                _deckCards = await GetCardsDeck();
                StateHasChanged();
            }
            else
            {
                string errorMessage = $"Error: {response.ReasonPhrase}";
                throw new HttpRequestException(errorMessage);
            }
        }
        

        protected async Task<IEnumerable<DeckCardDTO>> GetCardsDeck()
        {
            HttpResponseMessage response = await _httpMinimalClient.GetAsync("deckCards");

            if (response.IsSuccessStatusCode)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                IEnumerable<DeckCardDTO> result = JsonSerializer.Deserialize<IEnumerable<DeckCardDTO>>(apiResponse, _jsonOptions);
                return result;
            }
            else
            {
                string errorMessage = $"Error: {response.ReasonPhrase}";
                throw new HttpRequestException(errorMessage);
            }
        }
    }
}
