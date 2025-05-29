//	 
using PrefinalMobSys1.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrefinalMobSys1.Data
{
    /// <summary>
    /// Centralized Class for handing local SQLite Database things for the App
    /// Loaded in MauiProgram as Singleton (one instance only within the App)
    /// </summary>
    public class DatabaseContext
    {
        SQLiteAsyncConnection database;
        public static DatabaseContext Instance { set; get; }
        public DatabaseContext()
        {
            //init from constructor
            DatabaseContext.Instance = this;
        }

        /// <summary>
        /// Initialize Database Availability
        /// </summary>
        /// <returns></returns>
        public async Task Init()
        {
            if (database is not null)
                return;

            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            //Create tables
            await database.CreateTableAsync<User>();
			await database.CreateTableAsync<TodoItem>();
		}

		// Users Methods
		public async Task<List<User>> Users()
        {
            await Init();
            return await database.Table<User>().ToListAsync();
        }

        public async Task<int> SaveUser(User incoming)
        {
            await Init();
            if (incoming.ID != 0)
                return await database.UpdateAsync(incoming);//update existing
            else
                return await database.InsertAsync(incoming);//insert new
        }

        public async Task<int> DeleteUser(User incoming)
        {
            await Init();
            return await database.DeleteAsync(incoming);
        }

		//Todo Methods
		public async Task<List<TodoItem>> GetTodoList()
		{
			await Init();
			return await database.Table<TodoItem>().ToListAsync();
		}

		public async Task<List<TodoItem>> GetItemsDoneAsync()
		{
			await Init();
			return await database.Table<TodoItem>().Where(t => t.IsCompleted).ToListAsync();
		}

		public async Task<TodoItem> GetTodoItem(int id)
		{
			await Init();
			return await database.Table<TodoItem>().Where(i => i.TodoID == id).FirstOrDefaultAsync();
		}

		public async Task<int> SaveTodo(TodoItem item)
		{
			await Init();
			if (item.TodoID != 0)
				return await database.UpdateAsync(item);
			else
				return await database.InsertAsync(item);
		}

		public async Task<int> DeleteTodo(TodoItem item)
		{
			await Init();
			return await database.DeleteAsync(item);
		}
	}
}
