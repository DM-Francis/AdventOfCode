using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    public class FieldDefinition
    {
        public string Name { get; }
        public ReadOnlyCollection<int> ValidValues { get; }

        public FieldDefinition(string fieldRaw)
        {
            string[] fieldSections = fieldRaw.Split(": ");
            Name = fieldSections[0];

            string[] ranges = fieldSections[1].Split(" or ");

            var validValues = new List<int>();
            foreach(string range in ranges)
            {
                string[] minMaxStr = range.Split('-');
                int min = int.Parse(minMaxStr[0]);
                int max = int.Parse(minMaxStr[1]);

                validValues.AddRange(Enumerable.Range(min, max - min + 1));
            }

            ValidValues = validValues.AsReadOnly();
        }
    }
}
