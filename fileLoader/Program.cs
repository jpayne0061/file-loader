using CsvHelper;
using System;
using System.Data.SqlClient;
using System.IO;

namespace fileLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Init();
        }

        public static void Init()
        {

            string connectionString = @"Data Source =.\SQLEXPRESS; Initial Catalog = LouisvilleData; Integrated Security = True;";
            int counter = 0;

                string query = "SET ANSI_WARNINGS  OFF Insert Into dbo.SalaryData_89 " +
        "VALUES(@SalaryDataID, @CalendarYear, @EmployeeName, @Department, @JobTitle, @AnnualRate, @RegularRate, @OvertimeRate, @IncentiveAllowance,  @Other, @YearToDate )";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                TextReader reader = new StreamReader(@"C:\Users\Administrator\Documents\data_sets\SalaryData_89.csv");
                var csvReader = new CsvReader(reader);

                csvReader.Configuration.BadDataFound = context =>
                {
                    System.IO.File.AppendAllText(@"C:\Users\Administrator\Source\Repos\fileLoader\fileLoader\errors.txt", context.RawRow.ToString() + Environment.NewLine);
                };

                var records = csvReader.GetRecords<Record>();

                connection.Open();
                foreach (Record record in records)
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        if (counter == 0)
                        {
                            counter++;
                            continue;
                        }
                        counter++;
                        // add parameters and their values
                        cmd.Parameters.AddWithValue("@AnnualRate", record.AnnualRate);
                        cmd.Parameters.AddWithValue("@CalendarYear", record.CalendarYear);
                        cmd.Parameters.AddWithValue("@Department", record.Department);
                        cmd.Parameters.AddWithValue("@EmployeeName", record.EmployeeName);
                        cmd.Parameters.AddWithValue("@IncentiveAllowance", record.IncentiveAllowance);
                        cmd.Parameters.AddWithValue("@JobTitle", record.JobTitle);
                        cmd.Parameters.AddWithValue("@Other", record.Other);
                        cmd.Parameters.AddWithValue("@OvertimeRate", record.OvertimeRate);
                        cmd.Parameters.AddWithValue("@RegularRate", record.RegularRate);
                        cmd.Parameters.AddWithValue("@SalaryDataID", record.SalaryDataID);
                        cmd.Parameters.AddWithValue("@YearToDate", record.YearToDate);
                        // open connection, execute command and close connection
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            try
                            {
                                System.IO.File.AppendAllText(@"C:\Users\Administrator\Source\Repos\fileLoader\fileLoader\errors.txt", counter.ToString() + ": " + e.Message + Environment.NewLine);
                            }
                            catch (Exception ex)
                            {

                            }

                        }

                    }
                }
                   
            }

            System.IO.File.AppendAllText(@"/errors.txt", DateTime.Now.ToString() + " *** END PROCESS " + Environment.NewLine);

        }
    }
}
