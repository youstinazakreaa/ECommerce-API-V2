using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onine.core.Entites
{
    public  class DeliveryMethod: BaseEntity
    {
        public string ShortName { get; set; }   
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string DeliveryTime { get; set; }= string.Empty;
    }
}
