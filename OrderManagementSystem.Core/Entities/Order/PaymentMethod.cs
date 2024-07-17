using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Entities.Order
{
    public enum PaymentMethod
    {
        [EnumMember(Value = "Credit Card")]
        CreditCard,
        [EnumMember(Value = "PayPal")]
        PayPal
    }
}
