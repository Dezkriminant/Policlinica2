using System;

namespace Policlinica.DB;

public class BloodSugarRecord
{
    public int Id { get; set; }
    public int RecordId { get; set; }
    public decimal SugarLevel { get; set; }
    public DateTime MeasurementDate { get; set; }
}
