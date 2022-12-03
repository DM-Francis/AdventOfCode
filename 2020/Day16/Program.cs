using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day16
{
    class Program
    {
        static void Main(string[] args)
        {
            StringSplitOptions trimAndRemoveEmpty = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;
            string input = File.ReadAllText("input.txt");

            string exampleInput = @"class: 0-1 or 4-19
row: 0-5 or 8-19
seat: 0-13 or 16-19

your ticket:
11,12,13

nearby tickets:
3,9,18
15,1,5
5,14,9
20,1,2";

            //input = exampleInput.Replace("\r\n", "\n");

            string[] inputSections = input.Split("\n\n", trimAndRemoveEmpty);

            string[] fieldDefinitionsRaw = inputSections[0].Split('\n', trimAndRemoveEmpty);
            var fields = fieldDefinitionsRaw.Select(f => new FieldDefinition(f)).ToList();

            var allValidValues = new List<int>();
            foreach(FieldDefinition field in fields)
            {
                allValidValues.AddRange(field.ValidValues);
            }

            string[] allNearbyTicketValuesStr = inputSections[2].Replace("nearby tickets:\n", "").Split('\n', ',');
            int[] allNearbyTicketValues = allNearbyTicketValuesStr.Select(s => int.Parse(s)).ToArray();

            // Get values that do not match any fields
            var invalidValues = new List<int>();
            foreach(int value in allNearbyTicketValues)
            {
                if (!allValidValues.Contains(value))
                    invalidValues.Add(value);
            }
            Console.WriteLine(invalidValues.Sum());

            Ticket myTicket = new Ticket(inputSections[1].Replace("your ticket:\n", ""));
            List<Ticket> nearbyTickets = inputSections[2].Replace("nearby tickets:\n", "").Split('\n').Select(s => new Ticket(s)).ToList();

            // Remove invalid tickets
            var nearbyValidTickets = nearbyTickets.Where(t => !t.Values.Any(v => invalidValues.Contains(v))).ToList();

            var allTickets = new List<Ticket>();
            allTickets.Add(myTicket);
            allTickets.AddRange(nearbyValidTickets);

            // Check all positions to determine what fields they could be
            var orderedPotentialFields = new List<List<FieldDefinition>>();
            for (int i = 0; i < allTickets[0].Values.Count; i++)
            {
                int[] allValues = allTickets.Select(t => t.Values[i]).ToArray();

                var potentialFields = new List<FieldDefinition>();
                foreach(FieldDefinition field in fields)
                {
                    if (allValues.All(v => field.ValidValues.Contains(v)))
                        potentialFields.Add(field);
                }

                orderedPotentialFields.Add(potentialFields);
            }

            // Slowly eliminate potential fields until we are left with 1 field per position
            while(orderedPotentialFields.Any(fl => fl.Count > 1))
            {
                List<(FieldDefinition, int index)> singlePosibilities = orderedPotentialFields
                    .Select((list, index) => (list, index))
                    .Where(fl => fl.list.Count == 1)
                    .Select(fl => (fl.list[0], fl.index))
                    .ToList();

                foreach(var (field, index) in singlePosibilities)
                {
                    for (int i = 0; i < orderedPotentialFields.Count; i++)
                    {
                        if (i != index)
                            orderedPotentialFields[i].Remove(field);
                    }
                }
            }

            var finalOrderedFields = new Dictionary<int, FieldDefinition>(
                orderedPotentialFields.Select((fl, index) => new KeyValuePair<int, FieldDefinition>(index, fl[0])));

            var departureFields = finalOrderedFields.Where(f => f.Value.Name.Contains("departure"));

            var myTicketDepartureValues = departureFields.Select(d => myTicket.Values[d.Key]);

            long multiplication = myTicketDepartureValues.Select(v => (long)v).Aggregate((a, b) => a * b);
            Console.WriteLine(multiplication);
        }
    }
}
