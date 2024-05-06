using Howest.MagicCards.DAL.Models;

namespace Howest.MagicCards.Shared.Extensions
{
    public static class CardExtensions
    {
        public static IQueryable<Card> ToFilteredList(this IQueryable<Card> cards, string set, string type, string name, string text, Artist artist, string rarity)
        {
            IQueryable<Card> res = cards;

            res = res.Where(c => set == null ? true : c.SetCode == set);
            res = res.Where(c => type == null ? true : c.Type.Substring(0, c.Type.IndexOf('-')).Contains(type));
            res = res.Where(c => name == null ? true : c.Name.Contains(name));
            res = res.Where(c => text == null ? true : c.Text.Contains(text));
            res = res.Where(c => artist == null ? true : c.Artist == artist);
            res = res.Where(c => rarity == null ? true : c.RarityCode == rarity);

            return res;
        }

    }
}