namespace Hotel_NotFarOff.Models.Entities
{
    public class BaseEntity
    {
        public object GetProperty(string property)
        {
            return GetType().GetProperty(property).GetValue(this);
        }
    }
}
