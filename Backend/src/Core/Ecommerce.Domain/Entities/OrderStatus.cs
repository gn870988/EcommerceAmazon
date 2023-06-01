using System.Runtime.Serialization;

namespace Ecommerce.Domain.Entities;

public enum OrderStatus
{
    [EnumMember(Value = "Pending")]
    Pending,

    [EnumMember(Value = "The payment was received")]
    Finished,

    [EnumMember(Value = "The product was shipped")]
    Sent,

    [EnumMember(Value = "The payment had errors")]
    Mistake
}