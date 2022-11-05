using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3.Models
{
    public class BillingCoin
    {
        public long Id { get; set; }

        public List<string>? History { get; set; }
    }
}
