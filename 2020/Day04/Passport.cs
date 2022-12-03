using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day04
{
    public class Passport
    {
        public Dictionary<string, string> Data = new();

        public Passport(string rawPassport)
        {
            string[] keyValues = rawPassport.Split(' ', '\n');

            foreach (string keyValue in keyValues)
            {
                if (string.IsNullOrWhiteSpace(keyValue))
                    continue;

                string[] keyValueSplit = keyValue.Split(':');
                Data.Add(keyValueSplit[0], keyValueSplit[1]);
            }
        }

        public bool IsValidIgnoringCid()
        {
            string[] requiredKeys = new[]
            {
                "byr",
                "iyr",
                "eyr",
                "hgt",
                "hcl",
                "ecl",
                "pid"
            };

            return requiredKeys.All(k => Data.Keys.Contains(k));
        }

        public bool ValidateAllFields()
        {
            return IsValidIgnoringCid()
                && ValidateBirthYear()
                && ValidateIssueYear()
                && ValidateExpirationYear()
                && ValidateHeight()
                && ValidateHairColour()
                && ValidateEyeColour()
                && ValidatePassportId();
        }

        private bool ValidateBirthYear()
        {
            if (Data.TryGetValue("byr", out string byr))
            {
                if (!int.TryParse(byr, out int byear))
                    return false;

                if (byear < 1920 || byear > 2002)
                    return false;

                return true;
            }

            return false;
        }

        private bool ValidateIssueYear()
        {
            if (Data.TryGetValue("iyr", out string iyr))
            {
                if (!int.TryParse(iyr, out int iyear))
                    return false;

                if (iyear < 2010 || iyear > 2020)
                    return false;

                return true;
            }

            return false;
        }

        private bool ValidateExpirationYear()
        {
            if (Data.TryGetValue("eyr", out string eyr))
            {
                if (!int.TryParse(eyr, out int eyear))
                    return false;

                if (eyear < 2020 || eyear > 2030)
                    return false;

                return true;
            }

            return false;
        }

        private bool ValidateHeight()
        {
            if (Data.TryGetValue("hgt", out string hgt))
            {
                if (hgt[^2..] == "cm")
                {
                    if (int.TryParse(hgt[..^2], out int height))
                    {
                        return height >= 150 & height <= 193;
                    }
                }
                else if (hgt[^2..] == "in")
                {
                    if (int.TryParse(hgt[..^2], out int height))
                    {
                        return height >= 59 & height <= 76;
                    }
                }
            }

            return false;
        }

        private bool ValidateHairColour()
        {
            char[] validColourChars = new[]
            {
                '0','1','2','3','4','5','6','7','8','9','a','b','c','d','e','f'
            };

            if (Data.TryGetValue("hcl", out string hcl))
            {
                if (hcl[0] != '#')
                    return false;

                for (int i = 1; i < hcl.Length; i++)
                {
                    if (!validColourChars.Any(v => v == hcl[i]))
                        return false;
                }

                return true;
            }

            return false;
        }

        private bool ValidateEyeColour()
        {
            string[] validEyeColours = new[]
            {
                "amb", "blu", "brn", "gry", "grn", "hzl", "oth"
            };

            if (Data.TryGetValue("ecl", out string ecl))
            {
                return validEyeColours.Any(v => v == ecl);
            }

            return false;
        }

        private bool ValidatePassportId()
        {
            if (Data.TryGetValue("pid", out string pid))
            {
                if (pid.Length != 9)
                    return false;

                return pid.All(c => char.IsDigit(c));
            }

            return false;
        }
    }
}
