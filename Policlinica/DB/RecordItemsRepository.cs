using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using MySqlConnector;
using Policlinica.ViewModels;

namespace Policlinica.DB;

public class RecordItemsRepository : BaseRep
{
    
    public RecordItemsRepository(IOptions<DatabaseConnection> dataBaseConnection) : base(dataBaseConnection)
    {
        OpenConnection();
    }

    public List<RecordItem> GetRecordItemsByTest()
    {
        List<RecordItem> result = new List<RecordItem>();
        string sql = "select * from record_items";
        try
        {
          
            using (var mc = new MySqlCommand(sql, connection))
            using (var dr = mc.ExecuteReader())
            {
                while (dr.Read())
                {
                    result.Add(new RecordItem
                    {
                        Id = dr.GetInt32("id"),
                        RecordId = dr.GetInt32("record_id"),
                        ServiceId = dr.GetInt32("service_id"),
                        ServicePrice = dr.GetInt32("service_price"),
                       
                    });
                }
            }
            
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return result;
    }
}