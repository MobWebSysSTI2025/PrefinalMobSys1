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
    public partial class Home:ComponentBase
    {
		[Inject]
        public AppShellContext AppShell { get; set; }

        [Inject]
        public NavigationManager Nav { get; set; }

        [Inject]
        public DatabaseContext DB { get; set; }

        public HomeViewModel Model { get; set; }

		public enum TodoViewMode { List, Grid }
		public TodoViewMode CurrentView { get; set; } = TodoViewMode.List;

		public List<TodoItem> TodoItems { get; set; } = new();

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
				TodoModel.CreatedBy = "SYSTEM";
				TodoModel.CreatedDate = DateTime.Now;
				TodoModel.ModifiedDate = DateTime.Now;
				TodoModel.IsDeleted = false;

				await DB.SaveTodo(TodoModel);
				TodoModel = new TodoItem();
				TodoItems = await DB.GetTodoList();

				await InvokeAsync(StateHasChanged);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error in AddTodo: {ex.Message}");
			}
		}


		public async Task UpdateTodo()
		{
			TodoModel.ModifiedDate = DateTime.Now;
			await DB.SaveTodo(TodoModel);

			TodoItems = await DB.GetTodoList();
			await InvokeAsync(StateHasChanged);
		}

		public async Task DeleteTodo(TodoItem item)
		{
			item.IsDeleted = true;
			await DB.DeleteTodo(item);

			TodoItems = await DB.GetTodoList();
			await InvokeAsync(StateHasChanged);
		}

		public void SetEditTodo(TodoItem item)
		{
			TodoModel = item;
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
