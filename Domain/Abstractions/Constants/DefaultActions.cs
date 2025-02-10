using Domain.Entities;

namespace Domain.Abstractions.Constants;
public class DefaultActions
{
    public partial class Purchase
    {
        public const string Id = "0194ecf6-f88f-7d2c-bf43-6af89a607876";
        public const string Name = "Purchase";
        public const string Description = "the action affect when user do a purchase";
    }
    public partial class AddToFavourites
    {
        public const string Id = "0194ecf7-55dd-746a-8b97-daa9ac301b25";
        public const string Name = "AddToFavourites";
        public const string Description = "The action affect when user add product to favourites";
    }
    public partial class AddToWishList
    {
        public const string Id = "0194ecf7-d6d6-79d5-b697-c3c157cbc1ba";
        public const string Name = "AddToWishList";
        public const string Description = "The action affect when user add product to wish list";
    }
    public partial class View
    {
        public const string Id = "0194ecf7-ee70-7744-a9ee-2f4b50a0cdc9";
        public const string Name = "View";
        public const string Description = "The action affect when user add view a product";
    }
}
