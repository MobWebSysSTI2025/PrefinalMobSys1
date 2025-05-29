	using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
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
		public partial class Home:ComponentBase
		{
			[Inject]
			public AppShellContext AppShell { get; set; }

			[Inject]
			public NavigationManager Nav { get; set; }

			[Inject]
			public DatabaseContext DB { get; set; }

			[Inject]
			public IJSRuntime JS { get; set; }


			public HomeViewModel Model { get; set; }

			public enum TodoViewMode { List, Grid }
			public TodoViewMode CurrentView { get; set; } = TodoViewMode.List;
			public IEnumerable<TodoItem> VisibleTodoItems => TodoItems.Where(t => !t.IsCompleted && !t.IsDeleted);

			public List<TodoItem> TodoItems { get; set; } = new();

			public TodoItem NewTodoModel = new();

			public TodoItem TodoModel = new();

			public string ClassControl = "";

			/// <summary>
			/// This will be called on load or start of a page
			/// </summary>
			protected override async Task OnInitializedAsync()
			{
				Model = new HomeViewModel();

				//check logged-in user
				var loggedUser = AppShell.GetSessionUser();
				if (loggedUser != null)
				{
					AppShell.CurrentUser = loggedUser;
					AppShell.IsUserLoggedIn = true;
				}

				await DB.Init(); //initialize database
				TodoItems = await DB.GetTodoList();
				await InvokeAsync(StateHasChanged);
			}

			public async void SearchTerm(ChangeEventArgs e)
			{

				string searchTerm = e.Value.ToString().ToLower();
				if (!string.IsNullOrWhiteSpace(searchTerm))
				{
					Nav.NavigateTo("/" + searchTerm);
				}
				await InvokeAsync(StateHasChanged);//refresh rendered page
			}

			//set view mode for todo items
			public void SetViewMode(TodoViewMode mode)
			{
				CurrentView = mode;
				InvokeAsync(StateHasChanged);
			}

			public async Task AddTodo()
			{
				try
				{
					NewTodoModel.CreatedBy = "SYSTEM";
					NewTodoModel.ModifiedBy = "SYSTEM";
					NewTodoModel.CreatedDate = DateTime.Now;
					NewTodoModel.ModifiedDate = DateTime.Now;
					NewTodoModel.IsDeleted = false;

					await DB.SaveTodo(NewTodoModel);
					NewTodoModel = new TodoItem();

					TodoItems = await DB.GetTodoList();
					await JS.InvokeVoidAsync("hideModal", "addTodoModal");

					await InvokeAsync(StateHasChanged);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error in AddTodo: {ex.Message}");
				}
			}

			public async Task UpdateTodo()
			{
				try
				{
					TodoModel.ModifiedDate = DateTime.Now;
					await DB.SaveTodo(TodoModel);

					TodoItems = await DB.GetTodoList();
					TodoModel = new TodoItem();

					await JS.InvokeVoidAsync("hideModal", "editTodoModal");

					await InvokeAsync(StateHasChanged);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error in UpdateTodo: {ex.Message}");
				}
			}


			public async Task DeleteTodo(TodoItem item)
			{
				try
				{
					item.IsDeleted = true;
					await DB.DeleteTodo(item);

					TodoItems = await DB.GetTodoList();

					//string modalId = $"confirmDeleteTodoModal-{item.TodoID}";
					await JS.InvokeVoidAsync("hideModal", $"confirmDeleteTodoModal-{item.TodoID}");

					await InvokeAsync(StateHasChanged);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error in DeleteTodo: {ex.Message}");
				}
			}


			public void SetEditTodo(TodoItem item)
			{
				TodoModel = new TodoItem
				{
					TodoID = item.TodoID,
					Title = item.Title,
					Description = item.Description,	
					DueDate = item.DueDate,
					IsCompleted = item.IsCompleted,
					CreatedBy = item.CreatedBy,
					CreatedDate = item.CreatedDate,
					ModifiedDate = item.ModifiedDate,
					IsDeleted = item.IsDeleted
				};
			}


			public async Task ToggleCompletion(TodoItem item)
			{
				item.IsCompleted = !item.IsCompleted;
				item.ModifiedDate = DateTime.Now;

				await DB.SaveTodo(item);
				TodoItems = await DB.GetTodoList();

				await InvokeAsync(StateHasChanged);
			}
		}
	}
