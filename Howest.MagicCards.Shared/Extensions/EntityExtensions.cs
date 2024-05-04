namespace Shared.Extensions;

public static class EntityExtensions
{
    public static IQueryable<T> ToPagedList<T>(this IQueryable<T> entities, int pageNr, int pageSize)
    {
        return entities
                    .Skip((pageNr - 1) * pageSize)
                    .Take(pageSize);
    }


}
