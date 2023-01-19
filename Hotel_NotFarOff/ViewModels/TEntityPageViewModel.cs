using Hotel_NotFarOff.Enums;
using Hotel_NotFarOff.Models.Entities;

namespace Hotel_NotFarOff.ViewModels
{
    public class TEntityPageViewModel<T> where T : BaseEntity
    {
        public PageTypeEnum PageType { get; private set; }
        public T Entity { get; private set; }

        public TEntityPageViewModel(PageTypeEnum pageType, T entity)
        {
            PageType = pageType;
            Entity = entity;
        }
    }
}
