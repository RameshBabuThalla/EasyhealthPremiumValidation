using System;
using System.Collections.Generic;
namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class LookupService
    {
        private Dictionary<int, string> lookupTable;

        public LookupService()
        {
            // Initialize with sample data
            lookupTable = new Dictionary<int, string>
        {
            { 1, "Value1" },
            { 2, "Value2" },
            { 3, "Value3" }
        };
        }

        public string VLookup(int key)
        {
            if (lookupTable.TryGetValue(key, out var value))
            {
                return value;
            }
            else
            {
                return "Not Found"; // Similar to #N/A in Excel
            }
        }
    }

    //// Usage
    //LookupService service = new LookupService();
    //string result = service.VLookup(2); // Example key
}

