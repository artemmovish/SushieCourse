using Newtonsoft.Json;
using SushieUser.Models;
using System.Text;
using System.Text.Json;


public class ApiClient
{
    private readonly HttpClient _httpClient;
    private string? _token;
    public bool Auth = false;
    public ApiClient(string baseUrl)
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
    }

    #region API продуктов и категорий

    #region Категории
    public async Task<Category[]> GetCategories()
    {
        try
        {
            using var response = await _httpClient.GetAsync("api/categories");
            var jsonString = await response.Content.ReadAsStringAsync();

            var res = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<Category[]>>(jsonString,
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })?.Data;

            return res;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task CreatCategories(Category item)
    {
        string url = "/api/categories/create";

        var Request = new
        {
            name = item.Name,
        };

        var json = System.Text.Json.JsonSerializer.Serialize(Request);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync(url, content);

        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteCategories(Category item)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync($"http://localhost:8000/api/categories/destroy/{item.Id}");

        response.EnsureSuccessStatusCode();
    }
    #endregion

    #region Суши
    public async Task<SushieItem[]> GetProduct()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("api/products");

        // Проверка успешности запроса
        response.EnsureSuccessStatusCode();

        // Чтение строки JSON из ответа
        string json = await response.Content.ReadAsStringAsync();

        // Десериализация JSON
        var jsonObject = JsonConvert.DeserializeObject<_ApiResponse>(json);
        var items = jsonObject.Data.ToObject<List<SushieItem>>();

        return items.ToArray();
    }

    public async Task CreatProducts(SushieItem item)
    {
        string url = "/api/products/create";

        var req = new
        {
            Name = item.name,
            Price = item.price,
            Quantity = item.quantity,
            Description = item.description,
            Category_id = item.category_id
        };

        var json = System.Text.Json.JsonSerializer.Serialize(req);
        json = json.ToLower();
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync(url, content);

        response.EnsureSuccessStatusCode();
    }

    public async Task ChangeProducts(SushieItem item)
    {
        var req = new
        {
            Name = item.name,
            Price = item.price,
            Quantity = item.quantity,
            Description = item.description,
            Category_id = item.category_id
        };

        var json = System.Text.Json.JsonSerializer.Serialize(req);
        json = json.ToLower();
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PatchAsync($"api/products/update/{item.id}", content);

        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteProducts(int id)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync($"api/products/destroy/{id}");

        response.EnsureSuccessStatusCode();
    }
    #endregion

    #endregion

    #region API аккаунта
    public void SetToken(string token)
    {
        _token = token;
        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }

    public async Task Login()
    {
        var _authRequest = new
        {
            Login = "admin",
            Password = "admin",
            Telephone = "admin"
        };

        var request = System.Text.Json.JsonSerializer.Serialize(_authRequest);

        request = request.ToLower();

        var content = new StringContent(request, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/login", content);

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();

        using JsonDocument doc = JsonDocument.Parse(responseContent);
        JsonElement root = doc.RootElement;

        // Извлечение токена
        var token = root
            .GetProperty("token")
            .GetString();

        SetToken(token);
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

    #region API админа
    
    #endregion
}


public class _ApiResponse
{
    public dynamic Data { get; set; }
}

public class ApiResponse<T>
{
    public T Data { get; set; }
}
