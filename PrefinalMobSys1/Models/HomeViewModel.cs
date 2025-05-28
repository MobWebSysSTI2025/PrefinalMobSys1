using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrefinalMobSys1.Models
{
    public class HomeViewModel : BaseViewModel
    {
        public string Search { get; set; } 
		public List<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
		public bool ShowCompleted { get; set; } = false;
		public IEnumerable<TodoItem> VisibleTodoItems => TodoItems.Where(t => !t.IsCompleted && !t.IsDeleted);
	}
}
