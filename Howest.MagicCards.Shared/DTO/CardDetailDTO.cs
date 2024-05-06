namespace Howest.MagicCards.Shared.DTO
{
    public record CardDetailDTO
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Type { get; init; }
        public string RarityCode { get; init; }
        public string ImageUrl { get; init; }
        public string Text { get; init; }
        public string Flavor {  get; init; }
        public int Power {  get; init; }
        public int Toughness { get; init; }
    }
}
