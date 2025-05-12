using EMS.Core.Interfaces;
using EMS.Core.Models;
using Npgsql;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace EMS.Core.Data
{
    internal class EnergyReadingRepository : IEnergyReadingRepository
    {
        // Function to create connection to the PostgresSQL database
        private static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(@"Server=localhost;Port=5432;Database=EMS;User Id=postgres;Password=12345678;"); // CHANGED THIS TO THE LOCAL DATABASE CREATED
        }

        // Save the data from the passed in EnergyReading class to the database
        public async Task SaveReadingAsync(EnergyReading reading)
        {
            using (NpgsqlConnection con = GetConnection())
            {
                await con.OpenAsync();
                using (var command = con.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO public.EnergyReadings (Start_Timestamp, End_Timestamp, I_L1, I_L2, I_L3, V_L1, V_L2, V_L3, V_L12, V_L23, V_L31, P_L1, P_L2, P_L3, VARQ1_L1, VARQ1_L2, VARQ1_L3, VA_L1, VA_L2, VA_L3, COS1_L1, COS1_L2, COS1_L3)
                                            VALUES (@Start_Timestamp, @End_Timestamp, @I_L1, @I_L2, @I_L3, @V_L1, @V_L2, @V_L3, @V_L12, @V_L23, @V_L31, @P_L1, @P_L2, @P_L3, @VARQ1_L1, @VARQ1_L2, @VARQ1_L3, @VA_L1, @VA_L2, @VA_L3, @COS1_L1, @COS1_L2, @COS1_L3)";

                    command.Parameters.AddWithValue("@Start_Timestamp", reading.Start_Timestamp);
                    command.Parameters.AddWithValue("@End_Timestamp", reading.End_Timestamp);

                    command.Parameters.AddWithValue("@I_L1", reading.I_L1);
                    command.Parameters.AddWithValue("@I_L2", reading.I_L2);
                    command.Parameters.AddWithValue("@I_L3", reading.I_L3);

                    command.Parameters.AddWithValue("@V_L1", reading.V_L1);
                    command.Parameters.AddWithValue("@V_L2", reading.V_L2);
                    command.Parameters.AddWithValue("@V_L3", reading.V_L3);

                    command.Parameters.AddWithValue("@V_L12", reading.V_L12);
                    command.Parameters.AddWithValue("@V_L23", reading.V_L23);
                    command.Parameters.AddWithValue("@V_L31", reading.V_L31);

                    command.Parameters.AddWithValue("@P_L1", reading.P_L1);
                    command.Parameters.AddWithValue("@P_L2", reading.P_L2);
                    command.Parameters.AddWithValue("@P_L3", reading.P_L3);

                    command.Parameters.AddWithValue("@VARQ1_L1", reading.VARQ1_L1);
                    command.Parameters.AddWithValue("@VARQ1_L2", reading.VARQ1_L2);
                    command.Parameters.AddWithValue("@VARQ1_L3", reading.VARQ1_L3);

                    command.Parameters.AddWithValue("@VA_L1", reading.VA_L1);
                    command.Parameters.AddWithValue("@VA_L2", reading.VA_L2);
                    command.Parameters.AddWithValue("@VA_L3", reading.VA_L3);

                    command.Parameters.AddWithValue("@COS1_L1", reading.COS1_L1);
                    command.Parameters.AddWithValue("@COS1_L2", reading.COS1_L2);
                    command.Parameters.AddWithValue("@COS1_L3", reading.COS1_L3);


                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Return an EnergyReading class with aggregated data from the database
        public async Task<ObservableCollection<EnergyReading>> GetReadingsAsync(DateTime? startDate, DateTime? endDate, string frequency)
        {
            if (startDate == null)
            {
                MessageBox.Show("No start date entered");
            }

            if (endDate == null)
            {
                MessageBox.Show("No end date entered");
            }

            var readings = new ObservableCollection<EnergyReading>();

            string query;

            if (frequency == "Weekly")
            {
                query = @"SELECT date_trunc('week', start_timestamp) AS period_start,
                                 date_trunc('week', start_timestamp) + INTERVAL '7 days' AS period_end, 
                         AVG(I_L1) AS I_L1,
                         AVG(I_L2) AS I_L2,
                         AVG(I_L3) AS I_L3,
                         AVG(V_L1) AS V_L1,
                         AVG(V_L2) AS V_L2,
                         AVG(V_L3) AS V_L3,
                         AVG(V_L12) AS V_L12,
                         AVG(V_L23) AS V_L23,
                         AVG(V_L31) AS V_L31,
                         AVG(P_L1) AS P_L1,
                         AVG(P_L2) AS P_L2,
                         AVG(P_L3) AS P_L3,
                         AVG(VARQ1_L1) AS VARQ1_L1,
                         AVG(VARQ1_L2) AS VARQ1_L2,
                         AVG(VARQ1_L3) AS VARQ1_L3,
                         AVG(VA_L1) AS VA_L1,
                         AVG(VA_L2) AS VA_L2,
                         AVG(VA_L3) AS VA_L3,
                         AVG(COS1_L1) AS COS1_L1,
                         AVG(COS1_L2) AS COS1_L2,
                         AVG(COS1_L3) AS COS1_L3
                  FROM EnergyReadings
                  WHERE start_timestamp BETWEEN @startDate AND @endDate
                  GROUP BY period_start
                  ORDER BY period_start;";
            }
            else if (frequency == "Monthly")
            {
                query = @"SELECT date_trunc('month', start_timestamp) AS period_start,
                                 date_trunc('month', start_timestamp) + INTERVAL '1 month' AS period_end,
                         AVG(I_L1) AS I_L1,
                         AVG(I_L2) AS I_L2,
                         AVG(I_L3) AS I_L3,
                         AVG(V_L1) AS V_L1,
                         AVG(V_L2) AS V_L2,
                         AVG(V_L3) AS V_L3,
                         AVG(V_L12) AS V_L12,
                         AVG(V_L23) AS V_L23,
                         AVG(V_L31) AS V_L31,
                         AVG(P_L1) AS P_L1,
                         AVG(P_L2) AS P_L2,
                         AVG(P_L3) AS P_L3,
                         AVG(VARQ1_L1) AS VARQ1_L1,
                         AVG(VARQ1_L2) AS VARQ1_L2,
                         AVG(VARQ1_L3) AS VARQ1_L3,
                         AVG(VA_L1) AS VA_L1,
                         AVG(VA_L2) AS VA_L2,
                         AVG(VA_L3) AS VA_L3,
                         AVG(COS1_L1) AS COS1_L1,
                         AVG(COS1_L2) AS COS1_L2,
                         AVG(COS1_L3) AS COS1_L3
                  FROM EnergyReadings
                  WHERE start_timestamp BETWEEN @startDate AND @endDate
                  GROUP BY period_start
                  ORDER BY period_start;";
            }
            else if (frequency == "Yearly")
            {
                query = @"SELECT date_trunc('year', start_timestamp) AS period_start,
                                 date_trunc('year', start_timestamp) + INTERVAL '1 year' AS period_end,
                         AVG(I_L1) AS I_L1,
                         AVG(I_L2) AS I_L2,
                         AVG(I_L3) AS I_L3,
                         AVG(V_L1) AS V_L1,
                         AVG(V_L2) AS V_L2,
                         AVG(V_L3) AS V_L3,
                         AVG(V_L12) AS V_L12,
                         AVG(V_L23) AS V_L23,
                         AVG(V_L31) AS V_L31,
                         AVG(P_L1) AS P_L1,
                         AVG(P_L2) AS P_L2,
                         AVG(P_L3) AS P_L3,
                         AVG(VARQ1_L1) AS VARQ1_L1,
                         AVG(VARQ1_L2) AS VARQ1_L2,
                         AVG(VARQ1_L3) AS VARQ1_L3,
                         AVG(VA_L1) AS VA_L1,
                         AVG(VA_L2) AS VA_L2,
                         AVG(VA_L3) AS VA_L3,
                         AVG(COS1_L1) AS COS1_L1,
                         AVG(COS1_L2) AS COS1_L2,
                         AVG(COS1_L3) AS COS1_L3
                  FROM EnergyReadings
                  WHERE start_timestamp BETWEEN @startDate AND @endDate
                  GROUP BY period_start
                  ORDER BY period_start;";
            }
            else
            {
                MessageBox.Show($"Could not find {frequency}");
                query = null;
            }

            if (!string.IsNullOrEmpty(query))
            {
                using (NpgsqlConnection con = GetConnection())
                {
                    await con.OpenAsync();
                    using (var command = con.CreateCommand())
                    {
                        command.CommandText = query;

                        command.Parameters.AddWithValue("@startDate", startDate);
                        command.Parameters.AddWithValue("@endDate", endDate);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                readings.Add(new EnergyReading
                                {
                                    //ID = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Start_Timestamp = reader.GetDateTime(reader.GetOrdinal("period_start")),
                                    End_Timestamp = reader.GetDateTime(reader.GetOrdinal("period_end")),

                                    I_L1 = Math.Round(reader.GetDecimal(reader.GetOrdinal("I_L1")), 2),
                                    I_L2 = Math.Round(reader.GetDecimal(reader.GetOrdinal("I_L2")), 2),
                                    I_L3 = Math.Round(reader.GetDecimal(reader.GetOrdinal("I_L3")), 2),

                                    V_L1 = Math.Round(reader.GetDecimal(reader.GetOrdinal("V_L1")), 2),
                                    V_L2 = Math.Round(reader.GetDecimal(reader.GetOrdinal("V_L2")), 2),
                                    V_L3 = Math.Round(reader.GetDecimal(reader.GetOrdinal("V_L3")), 2),

                                    V_L12 = Math.Round(reader.GetDecimal(reader.GetOrdinal("V_L12")), 2),
                                    V_L23 = Math.Round(reader.GetDecimal(reader.GetOrdinal("V_L23")), 2),
                                    V_L31 = Math.Round(reader.GetDecimal(reader.GetOrdinal("V_L31")), 2),

                                    P_L1 = Math.Round(reader.GetDecimal(reader.GetOrdinal("P_L1")), 2),
                                    P_L2 = Math.Round(reader.GetDecimal(reader.GetOrdinal("P_L2")), 2),
                                    P_L3 = Math.Round(reader.GetDecimal(reader.GetOrdinal("P_L3")), 2),

                                    VARQ1_L1 = Math.Round(reader.GetDecimal(reader.GetOrdinal("VARQ1_L1")), 2),
                                    VARQ1_L2 = Math.Round(reader.GetDecimal(reader.GetOrdinal("VARQ1_L2")), 2),
                                    VARQ1_L3 = Math.Round(reader.GetDecimal(reader.GetOrdinal("VARQ1_L3")), 2),

                                    VA_L1 = Math.Round(reader.GetDecimal(reader.GetOrdinal("VA_L1")), 2),
                                    VA_L2 = Math.Round(reader.GetDecimal(reader.GetOrdinal("VA_L2")), 2),
                                    VA_L3 = Math.Round(reader.GetDecimal(reader.GetOrdinal("VA_L3")), 2),

                                    COS1_L1 = Math.Round(reader.GetDecimal(reader.GetOrdinal("COS1_L1")), 2),
                                    COS1_L2 = Math.Round(reader.GetDecimal(reader.GetOrdinal("COS1_L2")), 2),
                                    COS1_L3 = Math.Round(reader.GetDecimal(reader.GetOrdinal("COS1_L3")), 2),

                                });
                            }
                        }
                    }
                }

            }

            return readings;

        }
    }
}
