using CsvHelper;
using CsvHelper.Configuration;
using System.Collections;
using System.Globalization;
using System.IO;

namespace Stock.Web.Scraper.Service.Utilities.Csv
{
    public static class CsvUtilitiy
    {
        public static void WriteToCsv(this IEnumerable records, string path)
        {
            using (var writer = new StreamWriter(path))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(records);
                }
            }
        }

        public static void AppendToCsv(this IEnumerable records, string path)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Don't write the header again.
                HasHeaderRecord = false,
            };
            using (var stream = File.Open(path, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(records);
            }
        }

        public static void ReadFromFile<T>(this string path)
        {
            using (var reader = new StreamReader(""))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<T>();
                }
            }
        }
    }
}
