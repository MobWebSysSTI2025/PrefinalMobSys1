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
            await database.CreateTableAsync<TodoTask>();
        }

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

        public async Task<List<TodoTask>> GetTodoTasks()
        {
            await Init();
            return await database.Table<TodoTask>().ToListAsync();
        }

        public async Task<int> SaveTodoTask(TodoTask task)
        {
            await Init();
            if (task.Id != 0)
                return await database.UpdateAsync(task);
            else
                return await database.InsertAsync(task);
        }

        // Add this method to delete all TodoTasks from the database
        public async Task DeleteAllTodoTasks()
        {
            await Init();
            await database.DeleteAllAsync<TodoTask>();
        }
    }
}
