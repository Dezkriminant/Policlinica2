using System;
using System.Collections.Generic;

namespace Policlinica.DB;

public class Record
{
    public int Id { get; set; }
    
    public string ClientName { get; set; }
    
    public string ClientSurname { get; set; }
    
    public int DoctorId { get; set; }
    
    public int UserId { get; set; }
    
    public int ServiceId { get; set; }
    
    public decimal TotalAmount { get; set; }
    
    public DateTime RecordDate { get; set; }
    
    public string Title { get; set; }
    public string Name { get; set; }
    public string ServiceName { get; set; }
    
    public List<string> Services { get; set; } = new List<string>();
}
