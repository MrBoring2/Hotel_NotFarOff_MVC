using Hotel_NotFarOff.Models.Entities;
using System;

namespace Hotel_NotFarOff.Filters.EntityFilters
{
    public abstract class FilterModel<T> where T : BaseEntity
    {
        public abstract Func<T, bool> FilterExpression { get; }
    }
}
