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

		//public Models.TodoItem TodoModel = new Models.TodoItem();

		/// <summary>
		/// This will be called on load or start of a page
		/// </summary>
		protected override async void OnInitialized()
        {
            Model = new HomeViewModel();

            //check logged-in user
            var loggedUser = AppShell.GetSessionUser();
            if (loggedUser != null)
            {
                AppShell.CurrentUser = loggedUser;
                AppShell.IsUserLoggedIn = true;
            }

			//Model.TodoList = await DB.GetTodoItems();
			await InvokeAsync(StateHasChanged);
        }

        public async void SearchTerm(ChangeEventArgs e)
        {

            string searchTerm = e.Value.ToString().ToLower();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                Nav.NavigateTo("/catalog?lookingfor=" + searchTerm);
            }
            await InvokeAsync(StateHasChanged);//refresh rendered page
        }

		//public async void SaveTodo()
		//{
		//	TodoModel.IsDeleted = false;
		//	TodoModel.CreatedBy = "SYSTEM";
		//	TodoModel.ModifiedBy = "SYSTEM";
		//	TodoModel.CreatedDate = DateTime.Now;
		//	TodoModel.ModifiedDate = DateTime.Now;

		//	await DB.SaveTodoItem(TodoModel);

		//	// Refresh the list and reset the form
		//	Model.TodoList = await DB.GetTodoItems();
		//	TodoModel = new Models.TodoItem();

		//	await InvokeAsync(StateHasChanged);
		//}

		//public async void LoadTodo(int todoID)
		//{
		//	Model.SelectedTodo = (from row in Model.TodoList where row.TodoID == todoID select row).FirstOrDefault();
		//	ShowTodoForm();
		//	Model.IsNew = false;
		//	await InvokeAsync(StateHasChanged);//refresh rendered page
		//}

		//public async void ShowTodoForm()
		//{
		//	Model.ShowForm = true;
		//	await Task.Delay(100);
		//	//ClassControl = "animate__animated animate__slideInUp";
		//	await InvokeAsync(StateHasChanged);
		//}

		//public async void CloseTodoForm()
		//{
		//	//ClassControl = "animate__animated animate__slideOutDown";
		//	await Task.Delay(100);
		//	Model.ShowForm = false;
		//	await InvokeAsync(StateHasChanged);
		//}

		//public async void SelectTodo()
		//{
		//	Model.SelectMode = true;
		//	await InvokeAsync(StateHasChanged);
		//}

		//public async void CancelSelectTodo()
		//{
		//	Model.SelectMode = false;
		//	await InvokeAsync(StateHasChanged);
		//}
	}
}
