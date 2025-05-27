using Microsoft.AspNetCore.Components;
using PrefinalMobSys1.Data;
using PrefinalMobSys1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrefinalMobSys1.Components.Pages
{
    public partial class RecipeView : ComponentBase
    {
        [Inject]
        public AppShellContext AppShell { get; set; }

        [Inject]
        public NavigationManager Nav { get; set; }

        [Inject]
        public DatabaseContext DB { get; set; }

        [Parameter]
        [SupplyParameterFromQuery]
        public int? recipeid { get; set; }

        public RecipesViewModel Model { get; set; }

        protected override async void OnInitialized()
        {
            Model = new RecipesViewModel();
            Model.Ingredients = await GetIngredients();
            Model.CookingSteps = await GetSteps();

            if (Model.SelectedRecipe != null)
            {
                //Model.LoadedPhoto = $"/ProductPhotos/{Model.SelectedRecipe.ID}.jpg";
                Model.SelectedRecipe = new Recipe();
                Model.Ingredient = new RecipeIngredient();
                //loader
                var allingredients = await DB.RecipeIngredients();
                var allsteps = await DB.CookingSteps();
                Model.Ingredients = (from row in allingredients where row.RecipeID == Model.SelectedRecipe.ID select row).ToList();
                Model.CookingSteps = (from row in allsteps where row.StepId == Model.SelectedRecipe.ID select row).ToList();

                if (recipeid != null)
                {
                    await LoadRecipe(recipeid.Value);
                }
            }
            else
            {
                Model.LoadedPhoto = $"/imgs/recommended/2.jpg";
            }

            await InvokeAsync(StateHasChanged);//refresh rendered page
        }
        public async Task<List<Models.RecipeIngredient>> GetIngredients()
        {
            return await DB.RecipeIngredients();
        }
        public async Task<List<Models.CookingStep>> GetSteps()
        {
            return await DB.CookingSteps();
        }
        public void AddServings()
        {
            Model.Servings++;

        }
        public void MinusServings()
        {
            if (Model.Servings > 0)
            {
                Model.Servings--;
            }
            //removes to cart if zero quantity
            if (Model.Servings == 0)
            {
                //UI to remove to cart
            }
        }
        public async Task ConvertValues()
        {
            foreach (var ingredient in Model.Ingredients)
            {
                switch (ingredient.Unit)
                {
                    case "kg":
                        ingredient.Amount = Model.IsUSA ? ingredient.Amount * 2.20462 : ingredient.Amount / 2.20462;
                        ingredient.Unit = Model.IsUSA ? "lb" : "kg";
                        break;
                    case "g":
                        ingredient.Amount = Model.IsUSA ? ingredient.Amount * 0.035274 : ingredient.Amount / 0.035274;
                        ingredient.Unit = Model.IsUSA ? "oz" : "g";
                        break;
                    case "l":
                        ingredient.Amount = Model.IsUSA ? ingredient.Amount * 0.264172 : ingredient.Amount / 0.264172;
                        ingredient.Unit = Model.IsUSA ? "qt" : "l";
                        break;
                    case "ml":
                        ingredient.Amount = Model.IsUSA ? ingredient.Amount * 0.033814 : ingredient.Amount / 0.033814;
                        ingredient.Unit = Model.IsUSA ? "fl oz" : "ml";
                        break;
                }
            }
            Model.IsUSA = !Model.IsUSA; //toggle the unit system
            await InvokeAsync(StateHasChanged);
        }
        public async Task LoadRecipe(int RecipeID)
        {
            var allrecipes = await DB.Recipes();
            var allingredients = await DB.RecipeIngredients();
            var allsteps = await DB.CookingSteps();
            Model.SelectedRecipe = (from row in allrecipes where row.ID == RecipeID select row).FirstOrDefault();
            Model.Ingredients = (from row in allingredients where row.RecipeID == RecipeID select row).ToList();
            Model.CookingSteps = (from row in allsteps where row.StepId == RecipeID select row).ToList();
            Model.LoadedPhoto = $"/RecipePhotos/{RecipeID}.jpg";
            if (Model.SelectedRecipe == null)
            {
                Model.SelectedRecipe = new Recipe();
            }

            await InvokeAsync(StateHasChanged);//refresh rendered page
        }
    }
}