using Microsoft.AspNetCore.Components;
using PrefinalMobSys1.Data;
using PrefinalMobSys1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PrefinalMobSys1.Components.Pages
{
	public partial class KanbanBoard
	{
		public List<TodoItem> TodoItems { get; set; } = new();
		public List<TodoItem> CompletedTodoItems { get; set; } = new();

		protected override async Task OnInitializedAsync()
		{
			var allTodoItems = await DB.GetTodoList();
			TodoItems = allTodoItems.Where(todoItem => !todoItem.IsCompleted && !todoItem.IsDeleted).ToList();
			CompletedTodoItems = allTodoItems.Where(completedItem => completedItem.IsCompleted).ToList();
		}

		public async Task ToggleCompletion(TodoItem item)
		{
			item.IsCompleted = !item.IsCompleted;
			await DB.SaveTodo(item);

			if (item.IsCompleted)
			{
				TodoItems.Remove(item);
				CompletedTodoItems.Add(item);
			}
			else
			{
				CompletedTodoItems.Remove(item);
				TodoItems.Add(item);
			}

			await InvokeAsync(StateHasChanged);
		}
	}
}
