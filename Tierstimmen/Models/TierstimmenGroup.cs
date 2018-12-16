using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tierstimmen
{
    public class TierstimmenGroup
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }

    }
}
