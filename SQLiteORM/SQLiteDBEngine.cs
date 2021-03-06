using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteORM
{
    class SQLiteDBEngine
    {
        private List<SQLiteTable> _dbTables;

        public SQLiteTable this[string nameTable]
        {
            get
            {
                foreach (var item in _dbTables)
                {
                    if (nameTable.ToLower().Equals(item.Name.ToLower()))
                    {
                        return item;
                    }
                }
                throw new ArgumentException("Table is not Exists");
            }
        }

        private List<string> _tables;
        public SQLiteDBEngine(string dbPath, SQLIteMode mode)
        {
            _tables = new List<string>();
            _dbTables = new List<SQLiteTable>();

            switch (mode)
            {
                case SQLIteMode.EXISTS:
                    {
                        SQLiteConnector.PathToDataBase = dbPath;
                        init();
                        break;
                    }
                case SQLIteMode.NEW:
                    {
                        SQLiteConnector.CreateDatabaseSource(dbPath);
                        //развернуть дб по заранее заложенному алгоритму
                        break;
                    }
            }
        }

        private void init()
        {
            getTableNamesExists();
        }

        private void getTableNamesExists()
        {
            string queryGetTablesName = "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%'";
            SQLiteConnector.Connection.Open();
            SQLiteCommand sQLiteCommand = new SQLiteCommand(queryGetTablesName, SQLiteConnector.Connection);
            SQLiteDataReader qLiteDataReader = sQLiteCommand.ExecuteReader();
            foreach (DbDataRecord rows in qLiteDataReader)
            {
                _tables.Add(rows.GetString(0));
                _dbTables.Add(new SQLiteTable(rows.GetString(0), getTableFields(rows.GetString(0)), getTableData(rows.GetString(0))));
            }
            SQLiteConnector.Connection.Close();
        }



        //-------------------------------------------------------------------------------------------------------------------------start version 1

        // version 1  PK is  first column 
        private SortedList<long, List<string>> getTableData(string table)
        {
            SortedList<long, List<string>> dataList = new SortedList<long, List<string>>();

            string queryGetTablesData = $"SELECT * FROM {table}";
            SQLiteCommand sQLiteCommand = new SQLiteCommand(queryGetTablesData, SQLiteConnector.Connection);
            SQLiteDataReader qLiteDataReader = sQLiteCommand.ExecuteReader();
            foreach (DbDataRecord rows in qLiteDataReader)
            {
                List<string> fields = new List<string>();
                for (int i = 1; i < rows.FieldCount; i++)
                {
                    fields.Add(Convert.ToString(rows.GetValue(i)));
                }
                dataList.Add(Convert.ToInt64(rows.GetValue(0)), fields);
            }
            return dataList;
        }

        //-------------------------------------------------------------------------------------------------------------------------stop version 1

       



        //-------------------------------------------------------------------------------------------------------------------------start version 2

        // version 2  PK is not first column 

        //private SortedList<long, List<string>> getTableData(string table)
        //{
        //    SQLiteRow headRow = getTableFields(table);
        //    SortedList<long, List<string>> dataList = new SortedList<long, List<string>>();
        //    string queryGetTablesData = $"SELECT * FROM {table}";
        //    SQLiteCommand sQLiteCommand = new SQLiteCommand(queryGetTablesData, SQLiteConnector.Connection);
        //    SQLiteDataReader qLiteDataReader = sQLiteCommand.ExecuteReader();
        //    foreach (DbDataRecord rows in qLiteDataReader)
        //    {
        //        List<string> fields = new List<string>();
        //        for (int i = 0; i < rows.FieldCount; i++)
        //        {                    
        //            if (!headRow[i].IsPrimaryKey) { 
        //            fields.Add(Convert.ToString(rows.GetValue(i)));}
        //        }
        //        dataList.Add(Convert.ToInt64(rows.GetValue(0)), fields);
        //    }
        //    return dataList;
        //}

        //-------------------------------------------------------------------------------------------------------------------------stop version 2
        private SQLiteRow getTableFields(string table)
        {
            SQLiteRow oneRow = null;
            if (DoesTableExists(table))
            {
                string queryGetTableFields = $"PRAGMA table_info('{table}')";
                Console.WriteLine(queryGetTableFields);
                /*if(SQLiteConnector.Connection.State == System.Data.ConnectionState.Closed)
                {
                    SQLiteConnector.Connection.Open();
                }*/
                SQLiteCommand sQLiteCommand = new SQLiteCommand(queryGetTableFields, SQLiteConnector.Connection);
                SQLiteDataReader qLiteDataReader = sQLiteCommand.ExecuteReader();
                
                oneRow = new SQLiteRow(qLiteDataReader.FieldCount);
                
                foreach (DbDataRecord rows in qLiteDataReader)
                {
                    string[] str = new string[6];
                    for (int i = 0; i < rows.FieldCount; i++)
                    {
                        str[i] = rows.GetValue(i).ToString();
                    }
                    oneRow.AddColumn(new SQLiteColumn(str));
                }
                /*if (SQLiteConnector.Connection.State == System.Data.ConnectionState.Open)
                {
                    SQLiteConnector.Connection.Close();
                }*/
                //SQLiteConnector.Connection.Close();
            }
            return oneRow;
        }
        public bool DoesTableExists(string tableName)
        {
            if(_tables.Count > 0)
            { 
                foreach (string table in _tables)
                {
                    if (table.ToLower().Equals(tableName.ToLower()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public List<string> Tables
        {
            get
            {
                return _tables;
            }
        }
    }
}
