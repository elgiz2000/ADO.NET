using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Npgsql;

namespace Ado.NetSql
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=localhost;Port=5432;Database=TestADO;User Id=postgres;Password=root;";
            var DATA = new Dictionary<string, int>(){
                            {"'BMW'",20000 },
                            {"'Porsche'", 15000},
                            {"'Audi'",10000 },
                            {"'Kia'",5000}
};
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = con;      
                    Console.WriteLine("Press 0-4 for action:");
                    Console.WriteLine("0 - Delete");
                    Console.WriteLine("1 - Display all data");
                    Console.WriteLine("2 - Update");
                    Console.WriteLine("3 - Create");
                    Console.WriteLine("4 - Add some random data");
                    while (true)
                    {
                        char a = Console.ReadKey().KeyChar;
                        Console.WriteLine();
                        switch (a)
                        {
                            case '0':
                                try
                                {
                                Console.Write("Enter car name that you want to delete: ");
                                string str = Console.ReadLine();
                                    cmd.CommandText = String.Format("Select * from cars where name = '{0}'", str);
                                    NpgsqlDataReader reader = cmd.ExecuteReader();
                                    bool isExists = reader.Read();
                                    reader.Close();
                                    if (isExists)
                                    {
                                        cmd.CommandText = String.Format("DELETE FROM cars WHERE name = '{0}'", str);
                                        cmd.ExecuteNonQuery();
                                        Console.WriteLine("Succesfully deleted");
                                    }
                                    else
                                    {
                                        Console.WriteLine("No matching data for this search.Try again pls");
                                    }
                                  
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }
                                break;

                            case '1':
                                try
                                {
                                cmd.CommandText = "Select * from cars";
                                NpgsqlDataReader reader = cmd.ExecuteReader();
                                Console.WriteLine($"{reader.GetName(0),-4} {reader.GetName(1),-10} {reader.GetName(2),10}");
                                while (reader.Read())
                                {
                                    Console.WriteLine($"{reader.GetInt32(0),-4} {reader.GetString(1),-10} {reader.GetInt32(2),10}");
                                }
                                reader.Close();
                                }
                                catch (Exception e)
                                {

                                    Console.WriteLine(e);
                                }
                                break;

                            case '2':
                                try
                                {
                                Console.Write("Old name: ");
                                string oldCarName=Console.ReadLine();
                                cmd.CommandText=String.Format("Select * from cars where name = '{0}'",oldCarName);
                                NpgsqlDataReader reader=cmd.ExecuteReader();
                                    bool isExists= reader.Read();
                                    reader.Close();
                                    if (isExists)
                                    {
                                        Console.Write("New name: ");
                                        string newCarName = Console.ReadLine();
                                        cmd.CommandText = String.Format("UPDATE cars SET name = '{0}' WHERE name = '{1}'", newCarName, oldCarName);
                                        cmd.ExecuteNonQuery();
                                        Console.WriteLine("Succesfully changed. Press 1 to see all result");
                                    }
                                    else
                                    {
                                        Console.WriteLine("No matching data for this old name.Try again pls");
                                    }

                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }
                                break;

                            case '3':
                                try
                                {
                                Console.Write("Enter the name of the car that you want to create: ");
                                string carName=Console.ReadLine();
                                Console.Write("Enter the price of the car: ");
                                int price = int.Parse(Console.ReadLine());
                                string txt = String.Format("INSERT INTO cars(name, price) VALUES('{0}','{1}')", carName,price);
                                cmd.CommandText = txt; 
                                cmd.ExecuteNonQuery();
                                    Console.WriteLine("Successfully added.");
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }
                                break;

                            case '4':
                                try
                                {
                                    foreach (var item in DATA)
                                {
                                    cmd.CommandText = String.Format("INSERT INTO cars(name, price) VALUES({0},{1})", item.Key, item.Value); 
                                    cmd.ExecuteNonQuery();
                                }
                                Console.WriteLine("Successfully added 4 rows to the db");
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }
                                break;

                            default:
                                Console.WriteLine("Enter the value range of 0-4 please!");
                                break;
                        }
                        
                    }
                }
            }
        }
    }
}
