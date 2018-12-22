using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace Tierstimmen
{
	public class TierstimmenItemDatabase
	{
		SQLiteAsyncConnection database { get; set; }
        public string szGruppe { get; set; } = "voegel";
        public string szFilename { get; set; }
        public int iLimitResults { get; set; } = 25;
        public Boolean bUseSelected { get; set; } = false;
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
            string szSQL = "SELECT * FROM [TierstimmenItem] where Gruppe ='" + szGruppe + "'";
               
            if( bUseSelected)
            {
                szSQL += " and Selected = 1";
            }
            szSQL += " order by Name Limit " + iLimitResults.ToString();
            return database.QueryAsync<TierstimmenItem>(szSQL);
        }
        public Task<List<TierstimmenItem>> GetItemsAsync(string filter)
        {
            string szPattern = filter + "%";
            if( filter.Length > 1)
            {
                szPattern = "%" + szPattern;
            }
            string szSQL = "SELECT * FROM [TierstimmenItem] where Name like '" + szPattern + "' and Gruppe ='" + szGruppe + "'";
            if (bUseSelected)
            {
                szSQL += " and Selected = 1";
            }
            szSQL += " order by Name Limit " + iLimitResults.ToString();
            return database.QueryAsync<TierstimmenItem>(szSQL);
        }
        public Task<int> GetCountAsync(string filter)
        {
            string szPattern = filter + "%";
            if (filter.Length > 1)
            {
                szPattern = "%" + szPattern;
            }
            string szSQL = "SELECT Count(*) as iCount FROM [TierstimmenItem] where Name like '" + szPattern + "' and Gruppe ='" + szGruppe + "'";
            if (bUseSelected)
            {
                szSQL += " and Selected = 1";
            }
            
            return database.ExecuteScalarAsync<int>(szSQL);
        }
        public Task<List<TierstimmenGroup>> GetGroupsAsync()
        {
            return database.Table<TierstimmenGroup>().ToListAsync();
        }

        public Task<List<GroupCnt>> GetCountByGroupAsync()
		{
            string szSQL = "SELECT Gruppe as szGroup, Count(*) as iCount FROM [TierstimmenItem]  ";
            if (bUseSelected)
            {
                szSQL += " and Selected = 1";
            }
            szSQL += " group by Gruppe";
            return database.QueryAsync<GroupCnt>(szSQL);
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

