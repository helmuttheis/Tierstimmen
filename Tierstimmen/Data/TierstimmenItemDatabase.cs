using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace Tierstimmen
{
	public class TierstimmenItemDatabase
	{
		readonly SQLiteAsyncConnection database;
        public string szGruppe = "voegel";
		public TierstimmenItemDatabase(string dbPath)
		{
			database = new SQLiteAsyncConnection(dbPath);
           
            database.CreateTableAsync<TierstimmenItem>().Wait();
            database.CreateTableAsync<TierstimmenGroup>().Wait();
        }
        public void Reset()
        {
            try
            {
                database.DropTableAsync<TierstimmenItem>().Wait();
                database.DropTableAsync<TierstimmenGroup>().Wait();
            }
            catch (System.Exception)
            {

            }
            database.CreateTableAsync<TierstimmenItem>().Wait();
            database.CreateTableAsync<TierstimmenGroup>().Wait();
        }
        public Task<List<TierstimmenItem>> GetItemsAsync()
		{
            return database.QueryAsync<TierstimmenItem>("SELECT * FROM [TierstimmenItem] where Gruppe ='" + szGruppe + "' order by Name Limit 25");
        }
        public Task<List<TierstimmenItem>> GetItemsAsync(string filter)
        {
            return database.QueryAsync<TierstimmenItem>("SELECT * FROM [TierstimmenItem] where Name like '%" + filter + "%' and Gruppe ='" + szGruppe + "' order by Name  Limit 25");
        }
        public Task<List<TierstimmenGroup>> GetGroupsAsync()
        {
            return database.Table<TierstimmenGroup>().ToListAsync();
        }

        public Task<List<TierstimmenItem>> GetItemsByGroupAsync()
		{
			return database.QueryAsync<TierstimmenItem>("SELECT * FROM [TierstimmenItem] and Gruppe ='" + szGruppe + "' order by Name Limit 25");
		}

		public Task<TierstimmenItem> GetItemAsync(int id)
		{
			return database.Table<TierstimmenItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
		}

		public Task<int> SaveItemAsync(TierstimmenItem item)
		{
			if (item.ID != 0)
			{
				return database.UpdateAsync(item);
			}
			else {
				return database.InsertAsync(item);
			}
		}

		public Task<int> DeleteItemAsync(TierstimmenItem item)
		{
			return database.DeleteAsync(item);
		}
	}
}

