using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using PrefinalMobSys1.Data;
using PrefinalMobSys1.Models;

namespace PrefinalMobSys1.Components.Pages
{
    public partial class UserRecipe : ComponentBase
    {
        [Inject]
        public AppShellContext AppShell { get; set; }

        [Inject]
        public NavigationManager Nav { get; set; }

        [Inject]
        public DatabaseContext DB { get; set; }

        public RecipesViewModel Model { get; set; }

        protected override async void OnInitialized()
        {
            Model = new RecipesViewModel();
            Model.Recipes = await GetRecipes();
            await InvokeAsync(StateHasChanged);//refresh rendered page
            //return Task.Delay(0);
        }

        public async Task<List<Recipe>> GetRecipes()
        {
            return await DB.Recipes();
        }

        public async void LoadRecipe(int RecipeID)
        {
            Nav.NavigateTo("/recipeview?recipeid=" + RecipeID);
        }

        public void ShowRecipeForm()
        {
            //Model.ShowForm = true;
            Nav.NavigateTo("/recipeview");
        }

        public string GetIconFromCategory(string cat)
        {
            string resp = "";
            switch (cat)
            {
                case "Hamburgers":
                    resp = "fa-hamburger";
                    break;
                case "Pizza":
                    resp = "fa-pizza-slice";
                    break;
                case "Hotdogs":
                    resp = "fa-hotdog";
                    break;
                case "Cookies":
                    resp = "fa-cookie-bite";
                    break;
                case "IceCream":
                    resp = "fa-ice-cream";
                    break;
            }
            return resp;
        }

        public async void SearchTerm(ChangeEventArgs e)
        {
            var items = await GetRecipes();
            string searchTerm = e.Value.ToString().ToLower();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {

                var searchResults = (from row in items
                                     where row.Name.ToLower().Contains(searchTerm)
                                     || row.Description.ToLower().Contains(searchTerm)
                                     select row).ToList();
                Model.Recipes = searchResults;
            }
            else
            {
                Model.Recipes = items;
            }

            await InvokeAsync(StateHasChanged);//refresh rendered page
        }

        public async void ShowList()
        {
            Model.IsGrid = false;
            await InvokeAsync(StateHasChanged);//refresh rendered page
        }
        public async void ShowGrid()
        {
            Model.IsGrid = true;
            await InvokeAsync(StateHasChanged);//refresh rendered page
        }
    }
}
