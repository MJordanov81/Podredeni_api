namespace Api.Models.Infrastructure.Filters
{
    using System.Collections;
    using System.ComponentModel.DataAnnotations;

    public class CollectionNotEmptyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            ICollection collection = value as ICollection;

            if (collection == null) return true;

            if (collection.Count < 1) return false;

            return true;
        }
    }
}
