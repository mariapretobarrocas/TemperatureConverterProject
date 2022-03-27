using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using LumenWorks.Framework.IO.Csv;
using System.Text.Json;
using System.Linq;

namespace TemperatureConverter
{
    public class Program
    {
        static void Main(string[] args)
        {
            string csv_file_path = @"C:\Users\maria.preto\Desktop\TemperatureConverterFile.csv";

            GetDataTabletFromCSVFile(csv_file_path);

            Console.WriteLine("Done!");
        }

        private static DataTable GetDataTabletFromCSVFile(string csv_file_path)
        {
            var csvTable = new DataTable();

            using (var csvReader = new CsvReader(new StreamReader(System.IO.File.OpenRead(csv_file_path)), true))
            {
                //the entire data of the.csv file is loaded to the data table variable
                csvTable.Load(csvReader);
            }

            //access the columns
            DataColumn column1 = csvTable.Columns[1];

            List<TemperatureConverterParam> searchParameters = new List<TemperatureConverterParam>();


            for (int i = 0; i < csvTable.Rows.Count; i++)
            {
                if (!csvTable.Rows[i].IsNull(column1))
                {
                    var sensorID = csvTable.Rows[i][0].ToString();
                    var readingValue = double.Parse(csvTable.Rows[i][1].ToString());
                    var dateTime = double.Parse(csvTable.Rows[i][2].ToString());
                    var format = csvTable.Rows[i][3].ToString();

                    switch (format)
                    {
                        case "F":
                            readingValue = Math.Round(((readingValue - 32) * 5 / 9), 2);
                            break;
                        case "K":
                            readingValue = Math.Round(readingValue - 273, 2);
                            break;
                        default:
                            break;
                    }

                    var singleReading = new Reading()
                    {
                        ReadingValue = readingValue,
                        ReadingTime = UnixTimeStampToDateTime(dateTime)
                    };

                    searchParameters.Add(new TemperatureConverterParam
                    {
                        SensorID = sensorID,
                        Readings = new List<Reading> { singleReading }
                    });
                }               
            }

            JsonOutputFunctionAsync(searchParameters);

            return csvTable;
        }


        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimeStamp);

            return dt;
        }

        private static void JsonOutputFunctionAsync(List<TemperatureConverterParam> parameters)
        {
            List<TemperatureConverterParam> _data = new List<TemperatureConverterParam>();

            foreach (var param in parameters)
            {
                _data.Add(new TemperatureConverterParam()
                {
                    SensorID = param.SensorID,
                    Readings = param.Readings
                });
            }

            var wat = _data.GroupBy(i => i.SensorID).Select(i => new { id = i.Key, readings = i.Select(x => x.Readings ).ToList() });

            string json = JsonSerializer.Serialize(wat);
            File.WriteAllText(@"C:\Users\maria.preto\Desktop\readings.json", json);
        }
    }
}
