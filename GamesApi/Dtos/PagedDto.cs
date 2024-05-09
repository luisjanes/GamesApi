using GamesApi.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GamesApi.Dtos
{
    public class PagedDto
    {
        public PagedDto(int atualPage,int totalEntries, int remainingPages, List<Game> data, List<string> errors)
        {
            AtualPage = atualPage;
            TotalEntries = totalEntries;
            RemainingPages = remainingPages;
            Data = data;
            Errors = errors;
        }
        public int AtualPage { get; set; }
        public int TotalEntries { get; set; }
        public int RemainingPages { get; set; }
        public List<Game> Data { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
