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
        private Index model = new Index();
        private string _message = string.Empty;
        private IEnumerable<CardDTO> _cards = new List<CardDTO>();
        private IEnumerable<RarityDTO> _rarities = new List<RarityDTO>();
        private IEnumerable<TypeDTO> _types = new List<TypeDTO>();
        private IEnumerable<ArtistDTO> _artists = new List<ArtistDTO>();
        private IEnumerable<DeckCardDTO> _deckCards = new List<DeckCardDTO>();
        private CardDetailDTO _card = new CardDetailDTO();
        private int currentHoveredCardId;
        private List<(DeckCardDTO DeckCard, string CardName)> _deckCardsWithNames = new List<(DeckCardDTO DeckCard, string CardName)>();
        private int _pageNumber = 1;
        private int _pageSize = 150;
        private int _totalPages = 1;

        [Parameter]
        public string Rarity { get; set; }
        [Parameter]
        public string Type { get; set; }
        [Parameter]
        public int Artist { get; set; }
        [Parameter]
        public string Name { get; set; }
        [Parameter]
        public string Text { get; set; }
        [Parameter]
        public string SetCode { get; set; }

        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; }
        [Inject]
        public NavigationManager NavManager { get; set; }

        private readonly JsonSerializerOptions _jsonOptions;
        private HttpClient _httpClient;
        private HttpClient _httpMinimalClient;

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
            _artists = await GetResponse<ArtistDTO>("artists");
            _types = await GetResponse<TypeDTO>("types");
            _deckCards = await GetCardsDeck();
            await GetPagedResponse();
            await LoadDeckCardsWithNamesAsync();
        }

        protected async Task GetPagedResponse()
        {
            string apiUrl = $"Cards?PageNumber={_pageNumber}&PageSize={_pageSize}";
            if (!string.IsNullOrEmpty(Name))
            {
                apiUrl += $"&Name={Name}";
            }
            if (Artist != 0)
            {
                apiUrl += $"&Artist={Artist}";
            }
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            string apiResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                PagedResponse<IEnumerable<CardDTO>> cards =
                    JsonSerializer.Deserialize<PagedResponse<IEnumerable<CardDTO>>>(apiResponse, _jsonOptions);
                _cards = cards?.Data;
                _totalPages = cards.TotalPages;
            }
            else
            {
                _cards = new List<CardDTO>();
                _message = $"Error: {response.ReasonPhrase}";
            }
        }

        protected async Task UpdateCardDetail(int cardID)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"cards/{cardID}");

            if (response.IsSuccessStatusCode)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                _card = JsonSerializer.Deserialize<CardDetailDTO>(apiResponse, _jsonOptions);
                currentHoveredCardId = cardID;
                StateHasChanged();
            }
            else
            {
                string errorMessage = $"Error: {response.ReasonPhrase}";
                throw new HttpRequestException(errorMessage);
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
            var deckCard = new DeckCardDTO();
            HttpResponseMessage response = await _httpMinimalClient.PutAsJsonAsync($"deckCards/{cardId}", deckCard);

            if (response.IsSuccessStatusCode)
            {
                _deckCards = await GetCardsDeck();
                await LoadDeckCardsWithNamesAsync();
                StateHasChanged();
            }
            else
            {
                _message = "Error adding card to deck!";
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

        protected async Task DeleteCard(int cardId)
        {
            HttpResponseMessage response = await _httpMinimalClient.DeleteAsync($"deckCards/{cardId}");

            if (response.IsSuccessStatusCode)
            {
                _deckCards = await GetCardsDeck();
                await LoadDeckCardsWithNamesAsync();
                StateHasChanged();
            }
            else
            {
                string errorMessage = $"Error: {response.ReasonPhrase}";
                throw new HttpRequestException(errorMessage);
            }
        }

        private async Task ShowCardDetails(int cardID)
        {
            await UpdateCardDetail(cardID);
        }

        private async Task LoadDeckCardsWithNamesAsync()
        {
            var tasks = _deckCards.Select(async deckCard =>
            {
                var cardName = await GetCardNameAsync(deckCard.DeckCardId);
                return (DeckCard: deckCard, CardName: cardName);
            });

            _deckCardsWithNames = (await Task.WhenAll(tasks)).ToList();
        }

        protected async Task<string> GetCardNameAsync(int cardId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"cards/{cardId}");

            if (response.IsSuccessStatusCode)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                CardDTO result = JsonSerializer.Deserialize<CardDTO>(apiResponse, _jsonOptions);
                return result.Name;
            }
            else
            {
                string errorMessage = $"Error: {response.ReasonPhrase}";
                throw new HttpRequestException(errorMessage);
            }
        }

        protected async Task DecreaseCard(int cardId)
        {
            HttpResponseMessage response = await _httpMinimalClient.PutAsync($"deckCards/{cardId}/decrease", null);

            if (response.IsSuccessStatusCode)
            {
                _deckCards = await GetCardsDeck();
                await LoadDeckCardsWithNamesAsync();
                StateHasChanged();
            }
            else
            {
                _message = "Error decreasing card quantity!";
            }
        }

        public async Task PreviousPage()
        {
            if (_pageNumber > 1)
            {
                _pageNumber--;
                await GetPagedResponse();
            }
        }

        public async Task NextPage()
        {
            if (_pageNumber < _totalPages)
            {
                _pageNumber++;
               await GetPagedResponse();
            }
        }
        
    }
}