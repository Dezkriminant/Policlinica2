using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Policlinica.DB;

public class DoctorRepository:BaseRep
{
    public DoctorRepository(IOptions<DatabaseConnection> dataBaseConnection) : base(dataBaseConnection)
    {
        OpenConnection();
    }

    public List<Doctor> GetDoctorsByTest()
    {
        List<Doctor> result = new List<Doctor>();
        string sql = "select * from doctors";
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            using (var dr = mc.ExecuteReader())
            {
                while (dr.Read())
                {
                    result.Add(new Doctor
                    {
                        Id = dr.GetInt32("id"),
                        Title = dr.GetString("title"),
                        Description = dr.GetString("description"),
                       
                    });
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return result;
    }

    public void Dispose()
    {
        base.Dispose();
        CloseConnection();
    }
}
    
    
