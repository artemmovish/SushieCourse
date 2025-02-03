
using Newtonsoft.Json;
using SushieUser.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private string? _token;
    public bool Auth = false;

    public ApiClient(string baseUrl)
    {
        _baseUrl = baseUrl;
        _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
    }

    #region API продуктов и категорий
    public async Task<Category[]> GetCategories()
    {
        using var response = await _httpClient.GetAsync("api/categories");

        var jsonString = await response.Content.ReadAsStringAsync();

        var res = JsonConvert.DeserializeObject<ApiResponse<Category[]>>(jsonString)?.Data;

        return res;
    }

    public async Task<SushieItem[]> GetCategoriesProducts(long id)
    {
        using var response = await _httpClient.GetAsync($"api/categories/{id}");

        var jsonString = await response.Content.ReadAsStringAsync();

        var res = JsonConvert.DeserializeObject<ApiResponse<SushieItem[]>>(jsonString)?.Data;

        return res;
    }

    public async Task<SushieItem[]> GetProduct()
    {
        var response = await _httpClient.GetAsync("api/products");

        // Проверка успешности запроса
        response.EnsureSuccessStatusCode();

        // Чтение строки JSON из ответа
        string json = await response.Content.ReadAsStringAsync();

        // Десериализация JSON
        var jsonObject = JsonConvert.DeserializeObject<_ApiResponse>(json);
        var items = jsonObject.Data.ToObject<List<SushieItem>>();
        
        foreach (SushieItem item in items)
        {
            item.photo = _baseUrl + "/" + item.photo;
        }

        return items.ToArray();
    }

    public async Task<SushieItem[]> GetProduct(long id)
    {
        using var response = await _httpClient.GetAsync($"api/products/{id}");

        var jsonString = await response.Content.ReadAsStringAsync();

        var res = JsonConvert.DeserializeObject<ApiResponse<SushieItem[]>>(jsonString)?.Data;

        return res;
    }
    #endregion

    #region API аккаунта
    public void SetToken(string token)
    {
        _token = token;
        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }

    public async Task Register(RegisterRequest registerRequest)
    {
        var request = JsonConvert.SerializeObject(registerRequest);
        request = request.ToLower();
        var content = new StringContent(request, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/register", content);

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();

        var authRequest = new AuthRequest
        {
            Login = registerRequest.Login,
            Password = registerRequest.Password,
            Telephone = registerRequest.Telephone,
        };

        await Login(authRequest);
    }

    public async Task Login(AuthRequest authRequest)
    {
        var _authRequest = new
        {
            Login = authRequest.Login,
            Password = authRequest.Password,
            Telephone = authRequest.Telephone
        };

        var request = JsonConvert.SerializeObject(_authRequest);

        request = request.ToLower();

        var content = new StringContent(request, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/login", content);

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();

        var token = JsonConvert.DeserializeObject<dynamic>(responseContent).token;

        SetToken(token.ToString());
    }

    public async Task<RegisterRequest> GetProfile()
    {
        using var response = await _httpClient.GetAsync("api/profile");
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();

        var user = JsonConvert.DeserializeObject<dynamic>(jsonString).user;

        var res = JsonConvert.DeserializeObject<RegisterRequest>(user.ToString());

        return res;
    }

    public async Task Logout()
    {
        using var response = await _httpClient.GetAsync($"api/logout");

        _token = null;
    }

    public bool CheckToken()
    {
        if (string.IsNullOrEmpty(_token))
        {
            return false;
        }
        return true;
    }
    #endregion

    #region API корзины
    public async Task AddCart(SushieItem item, int q)
    {
        var id = item.id;
        var request = new
        {
            quantity = q
        };

        var json = JsonConvert.SerializeObject(request);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"api/products/{id}", content);

        response.EnsureSuccessStatusCode();
    }

    public async Task<CartItem[]> GetCart()
    {
        var response = await _httpClient.GetAsync("api/cart");

        var json = await response.Content.ReadAsStringAsync();
        var cartResponse = System.Text.Json.JsonSerializer.Deserialize<CartResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        SushieItem[] sushieItems = await GetProduct();

        List<CartItem> sushieItemsCard = new List<CartItem>();

        foreach (var item in cartResponse.CartItems)
        {
            SushieItem matchingItem = sushieItems.FirstOrDefault(s => s.id == item.ProductId);
            if (matchingItem != null)
            {
                var cart = new CartItem
                {
                    id = matchingItem.id,
                    quantity = item.Quantity,
                    description = matchingItem.description,
                    price = matchingItem.price,
                    name = matchingItem.name,
                };

                sushieItemsCard.Add(cart);
            }
        }

        return sushieItemsCard.ToArray();
    }
    #endregion
}

public class ApiResponse<T>
{
    [JsonProperty("data")]
    public T Data { get; set; }
}

public class _ApiResponse
{
    public dynamic Data { get; set; }
}

public class CartResponse
{
    [JsonPropertyName("cartItems")]
    public List<Cart> CartItems { get; set; }
}