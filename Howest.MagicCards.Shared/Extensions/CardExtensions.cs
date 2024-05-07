using Howest.MagicCards.DAL.Models;

namespace Howest.MagicCards.Shared.Extensions
{
    public static class CardExtensions
    {
        public static IQueryable<Card> ToFilteredList(this IQueryable<Card> cards, string set, string type, string name, string text, Artist artist, string rarity)
        {
            IQueryable<Card> res = cards;

            res = res.Where(c => set == null || c.SetCode == set);
            res = res.Where(c => type == null || c.CardTypes.Any(ct => ct.Type.Name.ToLower() == type.ToLower()));
            res = res.Where(c => name == null || c.Name.ToLower().Contains(name.ToLower()));
            res = res.Where(c => text == null || c.Text.ToLower().Contains(text.ToLower()));
            res = res.Where(c => artist == null || c.Artist == artist);
            res = res.Where(c => rarity == null || c.RarityCode == rarity);

            return res;
        }

        public static IQueryable<Card> Sort(this IQueryable<Card> cards, bool orderbyNameAsc)
        {
            return orderbyNameAsc ? cards.OrderBy(c => c.Name) : (IQueryable<Card>)cards.OrderByDescending(c => c.Name);
        }

    }
}