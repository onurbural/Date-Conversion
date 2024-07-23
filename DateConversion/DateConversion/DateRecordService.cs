using DateConversion.DTOs;
using DateConversion.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateConversion
{
    public class DateRecordService
    {
        private readonly DateRecordContext _context;

        public DateRecordService(DateRecordContext context)
        {
            _context = context;
        }

        public void UpdateDateDifferences()
        {
            try
            {
                var dateRecords = _context.DateRecords.ToList();

                if (dateRecords != null)
                {
                    foreach (var record in dateRecords)
                    {
                        if (record.OriginalDate.HasValue)
                        {
                            var differences = CalculateDifferences(record.OriginalDate.Value);
                            record.DiffDate = differences;
                        }
                        else
                        {
                            // Handle cases where OriginalDate is null if necessary
                            record.DiffDate = null;
                        }
                    }
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void AddDate()
        {
            Random gen = new Random();
            int range = 5 * 365;
            DateTime randomDate = DateTime.Today
                .AddDays(gen.Next(range * -1, range))
                .AddHours(gen.Next(range))
                .AddMinutes(gen.Next(range))
                .AddSeconds(gen.Next(range))
                .AddMilliseconds(gen.Next(range));

            // Create a new DateRecord object
            DateRecord newRecord = new DateRecord
            {
                OriginalDate = randomDate
            };

            _context.DateRecords.Add(newRecord);

            _context.SaveChanges();
        }

        private string CalculateDifferences(DateTime originalDate)
        {

            var timeZoneOffset = new TimeSpan(3, 0, 0); // UTC+03:00 saat dilimi (TR)

            // Statik farklı türlerde tarihlerin manuel verilmesi
            var staticDateTime = new DateTime(2024, 1, 1, 12, 45, 43);                              // -> 2024-01-01 12:45:43
            var staticDateTimeOffset = new DateTimeOffset(2024, 1, 1, 12, 43, 34, timeZoneOffset);   // -> 2024-01-01 12:43:34 +00:00
            var staticTimeOnly = new TimeOnly(12, 43, 34);  // -> 12:43:34
            var staticDateOnly = new DateOnly(2024, 1, 1);  // -> 2024-01-01

            // DateTime ile v.t deki tarihten tarih çıkarma
            var dateTimeDifference = originalDate - staticDateTime;

            // DateTimeOffset ile v.t deki tarihten tarih çıkarma
            var dateTimeOffsetDifference = new DateTimeOffset(originalDate).DateTime - staticDateTimeOffset.DateTime;

            // TimeSpan hesaplama (DateTime ile TimeSpan arasında fark hesaplama)
            var timeSpanDifference = new TimeSpan(originalDate.Ticks - staticDateTime.Ticks);

            // TimeOnly hesaplama
            //Yalnızca saat, dakika ve saniye değerlerini içerir. Fonksiyon isminden de anlaşılacağı gibi bir date alanı içermiyor.
            //DateTime içindeki saat bilgisinin işlemi yapılıyor.
            var timeOnlyDifference = new TimeOnly(originalDate.Hour, originalDate.Minute, originalDate.Second) - staticTimeOnly;

            // DateOnly hesaplama
            var originalDateOnly = new DateOnly(originalDate.Year, originalDate.Month, originalDate.Day);
            var dateOnlyDifference = originalDateOnly.ToDateTime(TimeOnly.MinValue) - staticDateOnly.ToDateTime(TimeOnly.MinValue);

            // OriginalDate'i DateTimeOffset olarak dönüştürme
            var originalDateTimeOffset = new DateTimeOffset(originalDate);

            // DateTimeOffset türü için fark hesaplama
            var dateTimeOffsetOriginalDifference = originalDateTimeOffset.DateTime - staticDateTimeOffset.DateTime;

            return $"DateTime: {dateTimeDifference}, DateTimeOffset: {dateTimeOffsetDifference}, " +
                   $"TimeSpan: {timeSpanDifference}, TimeOnly: {timeOnlyDifference}, DateOnly: {dateOnlyDifference}, " +
                   $"OriginalDateTimeOffset: {dateTimeOffsetOriginalDifference}";
        }

        //DateOnly ile TimeOnly birbirinden çıkartılamaz. Bunların çıkartımı için 2 sinin de ortak noktalarını içeren DateTime türüne çevirmek gerekmektedir.

    }

}
