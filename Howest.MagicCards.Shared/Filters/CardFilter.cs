using Howest.MagicCards.DAL.Models;

namespace Howest.MagicCards.Shared.Filters
{
    
    public class CardFilter: PaginationFilter
    {
        public string SetCode { get; set; }  
        public Artist Artist { get; set; } 
        public string RarityCode { get; set; }
        public string Type { get; set; }
        public string Name { get; set; } 
        public string Text { get; set; }
    }

    public class CardFilterWithSorting : PaginationFilter
    {
        public string SetCode { get; set; }
        public Artist Artist { get; set; }
        public string RarityCode { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public bool SortAsc { get; set; } = true;
    }
}
