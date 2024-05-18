﻿using Amazon.SecurityToken.Model;
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
        private IEnumerable<DeckCardDTO> _deckCards = new List<DeckCardDTO>();
        private CardDetailDTO _card = new CardDetailDTO();
        private int currentHoveredCardId;

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
                return new List<T>();
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
            DeckCardDTO deckCard = new DeckCardDTO();
            HttpResponseMessage response = await _httpMinimalClient.PutAsJsonAsync($"deckCards/{cardId}", deckCard);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    _deckCards = await GetCardsDeck();
                    StateHasChanged();
                }
                else
                {
                    string errorMessage = $"Error: {response.ReasonPhrase}";
                    throw new HttpRequestException(errorMessage);
                }
            }
            catch (Exception)
            {
                _message = "Deck is full!!!";
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

        //get the name of one singlw card when giving the id
        protected async Task<string> GetCardName(int cardId)
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

        public class Index
        {
            public string Rarity { get; set; }
            public string Type { get; set; }
        }
    }
}