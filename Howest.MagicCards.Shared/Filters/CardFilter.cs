using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Filters
{
    
    public class CardFilter: PaginationFilter
    {
        string _Sort;
        public string SetCode { get; set; }  
        public Artist Artist { get; set; } 
        public string RarityCode { get; set; }
        public string Type { get; set; }
        public string Name { get; set; } 
        public string Text { get; set; }
        public bool OrderByNameAsc { get; set; } = true;
    }
}
