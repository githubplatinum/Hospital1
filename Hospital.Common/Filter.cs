using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Common
{
    public class Filter
    {
        public DateTime? Dob { get; set; }
        public string SearchQuery { get; set; }
        public Guid? SpecializationId { get; set; }
    }
}
