using CsvHelper;
using CsvHelper.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Stock.Web.Scraper.Service.Utilities
{
    public static class CsvUtilitiy
    {
        public static CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture) { PrepareHeaderForMatch = args => args.Header.ToLower() };

        public static void WriteToCsv(this IEnumerable records, string path)
        {
            using (var writer = new StreamWriter($"{path}.csv"))
            {
                using (var csv = new CsvWriter(writer, csvConfig))
                {
                    csv.WriteRecords(records);
                }
            }
        }

        public static IEnumerable<T> ReadFromCsv<T>(this string path)
        {
            using (var reader = new StreamReader($"{path}.csv"))
            {
                using (var csv = new CsvReader(reader, csvConfig))
                {
                    return csv.GetRecords<T>().ToList();
                }
            }
        }
    }
}
