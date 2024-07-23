using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateConversion.DTOs
{
    public class DateRecordDto
    {
        public DateTime OriginalDate { get; set; }
        public object? DiffDate { get; set; }
    }
}
