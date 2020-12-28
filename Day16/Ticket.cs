using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    public class Ticket
    {
        public ReadOnlyCollection<int> Values { get; }

        public Ticket(string rawTicket)
        {
            Values = rawTicket.Split(',').Select(s => int.Parse(s)).ToList().AsReadOnly();
        }
    }
}
