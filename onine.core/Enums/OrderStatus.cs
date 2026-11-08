    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace onine.core.Enums
    {
        public enum OrderStatus
        {
            Pending,
            PaymentReceived,
            PaymentFailed,
            Shipped,
            Delivered,
            Cancelled
        }
    }
