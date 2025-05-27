using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrefinalMobSys1.Models
{
	public class TodoItem
	{
		[PrimaryKey]
		[NotNull]
		[AutoIncrement]
		public int TodoID { get; set; }
		[NotNull]
		public string Title { get; set; }
		[NotNull]
		public string Description { get; set; }
		public bool IsCompleted { get; set; }
		public DateTime? DueDate { get; set; } = DateTime.Today;

		[NotNull]
		public bool IsDeleted { get; set; }
		public string CreatedBy { get; set; }
		public string ModifiedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime ModifiedDate { get; set; }
	}
}
