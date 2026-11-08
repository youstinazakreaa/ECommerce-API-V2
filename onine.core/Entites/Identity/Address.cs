using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace onine.core.Entites.Identity
{
    public class Address
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string country { get; set; } = string.Empty;
        public string zipcode { get; set; } = string.Empty;

        public string AppUserId { get; set; } = string.Empty;

        public AppUser AppUser { get; set; } = null!;
    }
}

