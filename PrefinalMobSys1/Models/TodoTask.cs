using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrefinalMobSys1.Models
{
    public class TodoTask
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DueDate { get; set; }
    }
}
