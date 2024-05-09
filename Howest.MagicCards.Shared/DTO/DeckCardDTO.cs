using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.DTO
{
    public record DeckCardDTO
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public decimal Quantity { get; init; }
    }
}
