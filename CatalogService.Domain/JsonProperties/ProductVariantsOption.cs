namespace CatalogService.Domain.JsonProperties;

public sealed record ProductVariantsOption(
    List<VariantAttributeItem> Variants) 
{
    public bool Equals(ProductVariantsOption? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return Variants.SequenceEqual(other.Variants);
    }
    public override int GetHashCode()
    {
        return Variants.Aggregate(0, (hash, item) 
            => HashCode.Combine(hash, item.Key, item.Value));
    }
}