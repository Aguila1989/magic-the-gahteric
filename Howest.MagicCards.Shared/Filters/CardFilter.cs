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
        private string _type;
        public string SetCode { get; set; }  
        public Artist Artist { get; set; } 
        public string RarityCode { get; set; }
        public string Type
        {
            get => _type;
            set => _type = value?.Split('-').FirstOrDefault()?.Trim();
        }
        public string Name { get; set; } 
        public string Text { get; set; } 
    }
}
