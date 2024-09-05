using System.Net.Http;
using Newtonsoft.Json;
using Domain.DTO;
using Domain.Entities;
using Infrastructure.Repository.Interface;
using AutoMapper;

namespace Infrastructure.Services
{
    public class LiveTodoService : ILiveTodoService
    {
        private readonly HttpClient _httpClient;

        public LiveTodoService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("todos");
        }

        public async Task<IEnumerable<LiveTodoDTO>> GetAllLiveTodosAsync(int page, int pageSize)
        {
            var response = await _httpClient.GetAsync("/todos");
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var externalTodos = JsonConvert.DeserializeObject<List<LiveTodoDTO>>(jsonResponse);
                var totalCount = externalTodos.Count;
                var paginatedTodos = externalTodos.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                return paginatedTodos;
            }

            return new List<LiveTodoDTO>();
        }





    }
}
