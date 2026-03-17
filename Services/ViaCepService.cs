using ConsultorioApi.Models;

namespace ConsultorioApi.Services
{
    public class ViaCepService
    {
        public readonly HttpClient _httpClient;
        public ViaCepService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Models.ViaCepResponse> BuscarEnderecoAsync(string cep)
        {
            var endereco = await _httpClient.GetFromJsonAsync<ViaCepResponse>($"https://viacep.com.br/ws/{cep}/json/");

            return endereco;
        }
    }
}
