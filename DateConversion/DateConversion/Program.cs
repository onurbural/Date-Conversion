using DateConversion;
using System;
using DateConversion.Models;

class Program
{
    static void Main()
    {
        using (var context = new DateRecordContext())
        {
            var service = new DateRecordService(context);
            service.AddDate();
            Console.WriteLine("Veriler Eklendi");
            service.UpdateDateDifferences();
        }

        Console.WriteLine("Veritabanı güncellendi.");
    }
}
