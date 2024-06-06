using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Threading.Tasks;

namespace EcoTrack
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<SustainableAction>().Wait();
        }

        public Task<List<SustainableAction>> GetActionsAsync()
        {
            return _database.Table<SustainableAction>().ToListAsync();
        }

        public Task<SustainableAction> GetActionAsync(int id)
        {
            return _database.Table<SustainableAction>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveActionAsync(SustainableAction action)
        {
            if (action.ID != 0)
            {
                return _database.UpdateAsync(action);
            }
            else
            {
                return _database.InsertAsync(action);
            }
        }

        public Task<int> DeleteActionAsync(SustainableAction action)
        {
            return _database.DeleteAsync(action);
        }
    }
}
