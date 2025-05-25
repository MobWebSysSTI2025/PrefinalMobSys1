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

		public Models.TodoItem TodoModel = new Models.TodoItem();

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

			Model.TodoItems = await DB.TodoList();
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

		public async Task<List<Models.TodoItem>> GetTodoList()
		{
			return await DB.TodoList();
		}

		public async void SaveTodo()
		{
			if (string.IsNullOrWhiteSpace(Model.SelectedTodo.Title))
			{
				Model.Status = "danger";
				Model.StatusMessage = "Title cannot be blank or only spaces!";
			}
			else if (
				Model.TodoItems.Select(r => r.Title).ToList().Contains(Model.SelectedTodo.Title)
				&&
				Model.IsNew)
			{
				Model.Status = "danger";
				Model.StatusMessage = "Title already exists!";
			}
			else
			{
				await DB.SaveTodoItem(Model.SelectedTodo);
				CloseTodoForm();
				Model.Status = "success";
				Model.StatusMessage = "Todo item has been saved successfully!";
				Model.TodoItems = await GetTodoList();
			}
			await InvokeAsync(StateHasChanged);
		}

		public async void AddNewTodo()
		{
			TodoModel.IsDeleted = false;
			TodoModel.CreatedBy = "SYSTEM";
			TodoModel.ModifiedBy = "SYSTEM";
			TodoModel.CreatedDate = DateTime.Now;
			TodoModel.ModifiedDate = DateTime.Now;

			await DB.SaveTodoItem(TodoModel);

			// Refresh the list after adding
			Model.TodoItems = await GetTodoList();

			// Reset the input model
			TodoModel = new Models.TodoItem();

			Model.Status = "success";
			Model.StatusMessage = "Todo item has been saved successfully!";

			await InvokeAsync(StateHasChanged);
		}


		public async void LoadTodo(int todoid)
		{
			Model.SelectedTodo = (from row in Model.TodoItems where row.TodoID == todoid select row).FirstOrDefault();
			ShowTodoForm();
			Model.IsNew = false;
			await InvokeAsync(StateHasChanged);//refresh rendered page
		}

		public async void DeleteTodo(int todoid)
		{
			var selectedTodo = (from row in Model.TodoItems where row.TodoID == todoid select row).FirstOrDefault();
			if (selectedTodo != null)
			{
				await DB.DeleteTodoItem(selectedTodo);
				Model.Status = "success";
				Model.StatusMessage = "Todo item has been deleted successfully!";
				Model.TodoItems = await GetTodoList();
				await InvokeAsync(StateHasChanged);
			}
		}

		public void AddTodo()
		{
			Model.StatusMessage = ""; //clear alert
			Model.SelectedTodo = new Models.TodoItem();
			Model.IsNew = true;
			ShowTodoForm();
		}

		public async void ShowTodoForm()
		{
			Model.ShowForm = true;
			await Task.Delay(100);
			//ClassControl = "animate__animated animate__slideInUp";
			await InvokeAsync(StateHasChanged);
		}

		public async void CloseTodoForm()
		{
			//ClassControl = "animate__animated animate__slideOutDown";
			await Task.Delay(100);
			Model.ShowForm = false;
			await InvokeAsync(StateHasChanged);
		}

		public async void SelectUsers()
		{
			Model.SelectMode = true;
			await InvokeAsync(StateHasChanged);
		}

		public async void CancelSelectUsers()
		{
			Model.SelectMode = false;
			await InvokeAsync(StateHasChanged);
		}
	}
}
