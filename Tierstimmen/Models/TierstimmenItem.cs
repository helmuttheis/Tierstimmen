using SQLite;
using System.Collections.Generic;

namespace Tierstimmen
{
	public class TierstimmenItem
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public string Name { get; set; }
        public string Url { get; set; }
        public bool Selected { get; set; }
        public byte[] Ton { get; set; }
		public byte[] Bild { get; set; }
        public string Beschreibung { get; set; }
        public string Gruppe { get; set; }

    }
    public class TierstimmenItemList
    {
        public List<TierstimmenItem> Data { get; set; }
    }
}

