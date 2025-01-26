using static OfficeOpenXml.ExcelErrorValue;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class SumIfService
    {
        public decimal SumIf(List<int> conditions, List<decimal> values, int conditionValue)
        {
            decimal sum = 0;
            for (int i = 0; i < conditions.Count; i++)
            {
                if (conditions[i] > conditionValue)
                {
                    sum += values[i];
                }
            }
            return sum;
        }
    }

    // Usage
    //SumIfService service = new SumIfService();
    //List<int> conditions = new List<int> { 5, 15, 20 };
    //List<decimal> values = new List<decimal> { 100, 200, 300 };
    //decimal result = service.SumIf(conditions, values, 10); // Example condition value
}

