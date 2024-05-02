using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.DTO
{
    public record CardDTO
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Type { get; init; }
        public string RarityCode { get; init; }
        public string ImageUrl { get; init; }
    }
}
