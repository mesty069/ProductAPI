using System.Runtime.Serialization;

namespace Product.Core.Entities.Order
{
    public enum OrderStatus
    {
        [EnumMember(Value = "P")]
        Pending,
        [EnumMember(Value = "R")]
        PaymentRecived,
        [EnumMember(Value = "F")]
        PaymentFelied

    }
}