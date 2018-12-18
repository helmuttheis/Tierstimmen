using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace Tierstimmen
{
	public class TierstimmenItemDatabase
	{
		SQLiteAsyncConnection database;
        public string szGruppe = "voegel";
        public string szFilename;
		public TierstimmenItemDatabase(string dbPath)
		{
            szFilename = dbPath;
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
        public async Task Close()
        {
            await database.CloseAsync();
            SQLiteAsyncConnection.ResetPool();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        public async Task Reopen()
        {
            database = new SQLiteAsyncConnection(this.szFilename);
            await database.CreateTableAsync<TierstimmenItem>();
            await database.CreateTableAsync<TierstimmenGroup>();
        }
        public Task<List<TierstimmenItem>> GetItemsAsync()
		{
            return database.QueryAsync<TierstimmenItem>("SELECT * FROM [TierstimmenItem] where Gruppe ='" + szGruppe + "' order by Name Limit 25");
        }
        public Task<List<TierstimmenItem>> GetItemsAsync(string filter)
        {
            string szPattern = filter + "%";
            if( filter.Length > 1)
            {
                szPattern = "%" + szPattern;
            }
            return database.QueryAsync<TierstimmenItem>("SELECT * FROM [TierstimmenItem] where Name like '" + szPattern + "' and Gruppe ='" + szGruppe + "' order by Name  Limit 25");
        }
        public Task<List<TierstimmenGroup>> GetGroupsAsync()
        {
            return database.Table<TierstimmenGroup>().ToListAsync();
        }

        public Task<List<GroupCnt>> GetCountByGroupAsync()
		{
			return database.QueryAsync<GroupCnt>("SELECT Gruppe as szGroup, Count(*) as iCount FROM [TierstimmenItem] group by Gruppe ");
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
    public class GroupCnt
    {
        public string szGroup { get; set; }
        public int iCount { get; set; }
    }
}

