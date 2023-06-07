using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Common
{
   public class Sorting<T>
    {
        public string SortingBy { get; set; }
        public bool IsAscending { get; set; }
        public Sorting(string sortBy, bool isAscending)
        {
            SortingBy = sortBy;
            IsAscending = isAscending;
        }

    }
}
