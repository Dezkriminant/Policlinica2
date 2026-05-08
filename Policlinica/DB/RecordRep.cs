using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Policlinica.DB;

public class RecordRep:BaseRep
{
    public RecordRep(IOptions<DatabaseConnection> dataBaseConnection) : base(dataBaseConnection)
    {
        OpenConnection();
    }

    public List<Record> GetRecord()
    {
        List<Record> recordsList = new();

        string sql = @"select r.id,r.client_name, r.client_surname,r.doctor_id,user_id,r.total_amount,r.record_date,d.title,u.name 
                       from records r 
                       join doctors d on r.doctor_id = d.id 
                       join users u  on r.user_id  = u.id ";
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                using (var reader = mc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        recordsList.Add(new Record()
                        {
                            Id = reader.GetInt32("id"),
                            ClientName = reader.GetString("client_name"),
                            ClientSurname = reader.GetString("client_surname"),
                            DoctorId = reader.GetInt32("doctor_id"),
                            UserId = reader.GetInt32("user_id"),
                            TotalAmount = reader.GetInt32("total_amount"),
                            RecordDate = reader.GetDateTime("record_date"),
                            Name = reader.GetString("name"),
                            Title =  reader.GetString("title")
                        });
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return recordsList;
    }
}