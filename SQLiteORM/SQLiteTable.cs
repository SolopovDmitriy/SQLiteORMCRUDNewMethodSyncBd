using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteORM
{
    class SQLiteTable
    {
        private List<string> queriesForAsync;

        private string _name;
        private SQLiteRow _headRow;
        private SortedList<long, List<string>> _bodyRows;
        public SQLiteRow HeadRowInfo
        {
            get
            {
                return _headRow;
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
        }
        public SortedList<long, List<string>> BodyRows
        {
            get
            {
                return _bodyRows;
            }
        }
        public SQLiteTable(string tableName, SQLiteRow headRow, SortedList<long, List<string>> bodyRows)
        {
            queriesForAsync = new List<string>();
            _name = tableName; //валидация?
            _headRow = headRow;
            _bodyRows = bodyRows;
        }

        public void AddOneRow(List<string> row)
        {
            this.BodyRows.Add(_bodyRows.Count + 1, row);
        }
        //public void DeleteOneRow(long Id)
        //{
        //    if (BodyRows.ContainsKey(Id))
        //    {
        //        BodyRows.Remove(Id);
        //    }
        //    else 
        //    {
        //        throw new ArgumentException("Incorrect row Id");
        //    }
        //}
        public KeyValuePair<long, List<string>> GetOneRow(long Id)
        {
            if (BodyRows.ContainsKey(Id))
            {
                return new KeyValuePair<long, List<string>>(Id, BodyRows[Id]);
            }
            else
            {
                throw new ArgumentException("Incorrect row Id");
            }
        }




        //-------------------------------------------------------------------------------------------------------------------------start version 1

        // version 1  PK is  first column 


        public KeyValuePair<long, List<string>> GetOneRow(List<KeyValuePair<string, string>> searchPattern)
        {           
            bool searchMatched;// объявляем булевую переменную
            foreach (KeyValuePair<long, List<string>> oneRow in BodyRows)  // oneRow.Value = List{"Stefani", 24}
            {
                searchMatched = true;
                foreach (KeyValuePair<string, string> pattern in searchPattern) // pattern  = new KeyValuePair<string, string>("age", "22") 
                {
                    int indexCol = HeadRowInfo[pattern.Key].Cid; // pattern.Key = "age";   
                    // HeadRowInfo[pattern.Key] = 2	-age-	INTEGER	-0	-	0;  indexCol  = 2, indexCol ==cid;
                    if (indexCol != 0)//indexCol ==cid;
                    {
                        // Console.WriteLine(oneRow.Value[indexCol - 1] + "  == "+ pattern.Value); // item[indexCol - 1] = item[2-1] = item[1]
                        if (oneRow.Value[indexCol - 1] != pattern.Value) // pattern.Value = 35
                        {
                            searchMatched = false;
                            break;
                        }
                    }
                    else
                    {
                        if (oneRow.Key != Int64.Parse(pattern.Value))
                        {
                            searchMatched = false;
                            break;
                        }
                    }
                }
                if (searchMatched)
                {
                    return oneRow;
                }
            }
            return new KeyValuePair<long, List<string>>();
        }

        //-------------------------------------------------------------------------------------------------------------------------stop version 1



        //-------------------------------------------------------------------------------------------------------------------------start version 2

        // version 2  PK is not first column 
        //public KeyValuePair<long, List<string>> GetOneRow(Dictionary<string, string> searchPattern)
        //{
        //    KeyValuePair<long, List<string>> result;
        //    bool searchMatched;

        //    foreach (KeyValuePair<long, List<string>> oneRow in BodyRows)  // example  oneRow =   = key: id 1,	 value :  Олег,	25
        //                                                                   //oneRow.Key = (long) 1; oneRow.Value = List<string> { "Олег",	"25"}; 
        //                                                                   // BodyRows = List<List<string>

        //    {
        //        searchMatched = true; 
        //        int i = 0;
        //        foreach (SQLiteColumn headRow in HeadRowInfo)   //   headRow  - id;  headRow = fio, i = 0;   headRow = age, i = 1         
        //        {                    
        //            if (!headRow.IsPrimaryKey)
        //            {
        //                // Console.WriteLine(oneRow.Value[i] +" == "+ searchPattern[headRow.Name] );                       
        //                if (oneRow.Value[i] != searchPattern[headRow.Name]) //oneRow.Value[i] - Олег	25; oneRow.Value[0] - Олег; oneRow.Value[1] - 25; 
        //                                                                    // searchPattern[headRow.Name];  headRow.Name =  либо fio, либо age
        //                                                                    //searchPattern[headRow.Name]) либо Руслан либо 3 либо 35


                            

        //                {
        //                    searchMatched = false;
        //                    break;
        //                }
        //                i++;
        //            }
        //            else
        //            {
        //                // Console.WriteLine(oneRow.Key  + " == " + Int64.Parse(searchPattern[headRow.Name]));
        //                if (oneRow.Key != Int64.Parse(searchPattern[headRow.Name]))//oneRow.Key - id  в таблице = 3   // searchPattern[headRow.Name] = 3;  headRow.Name =  id
        //                {
        //                    searchMatched = false;
        //                    break;
        //                }
        //            }                  
        //        }
        //        if (searchMatched)
        //        {
        //            return oneRow;
        //        }
        //    }
        //    return new KeyValuePair<long, List<string>>();
        //}

        //-------------------------------------------------------------------------------------------------------------------------stop version 2

        public void Print()
        {


            //Console.WriteLine("{0, -10} --- {1} --- {2}", "hello", "ok", "errro");


            Console.WriteLine("Данные таблицы");
            Console.Write("+");
            for (int i = 0; i < 2; i++)
            {
                Console.Write("-");
            }
            Console.Write("+");
            for (int i = 0; i < 20; i++)
            {
                Console.Write("-");
            }
            Console.Write("+");
            for (int i = 0; i < 20; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine("+");
            foreach (SQLiteColumn col in HeadRowInfo)
            {
                if (col.Name == "id")
                {
                    Console.Write("|{0,-2}", col.Name);

                }

                else
                {
                    Console.Write("|{0,-20}", col.Name);
                }
            }
            Console.WriteLine("|");
            Console.Write("+");
            for (int i = 0; i < 2; i++)
            {
                Console.Write("-");
            }
            Console.Write("+");
            for (int i = 0; i < 20; i++)
            {
                Console.Write("-");
            }
            Console.Write("+");
            for (int i = 0; i < 20; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine("+");
            // Console.WriteLine("-------------------------------------------");

            foreach (var col in BodyRows)
            {
                Console.Write("|{0,-2}", col.Key);
                foreach (var item in col.Value)
                {
                    Console.Write("|{0,-20}", item);
                }
                Console.WriteLine("|");
            }
            Console.Write("+");
            for (int i = 0; i < 2; i++)
            {
                Console.Write("-");
            }
            Console.Write("+");
            for (int i = 0; i < 20; i++)
            {
                Console.Write("-");
            }
            Console.Write("+");
            for (int i = 0; i < 20; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine("+");
        }


        //UPDATE student SET fio = 'DFcz', age = '24' WHERE id = 16;
        public bool UpdateOneRow(long Id, List<string> newData) // List<string> { "Олег",	"25"}; 
        {
            if (BodyRows.ContainsKey(Id))
            {
                BodyRows[Id] = newData;

                string queryUpdate = $"UPDATE {_name} SET ";//делаем текст запроса
                int i = 0;
                foreach (SQLiteColumn column in _headRow)//column - данные о столбце (cid, Name, IsPrimaryKey, .....), _headRow - все столбцы - columns
                {
                    if (!column.IsPrimaryKey)
                    {
                        queryUpdate += $"{column.Name} = '{newData[i]}',"; // {column.Name} = '{row[i]}',    -     fio = 'DFcz',
                        i++;
                    }

                }
                queryUpdate = queryUpdate.Substring(0, queryUpdate.Length - 1);
                queryUpdate += $" WHERE id = {Id} ";

                queriesForAsync.Add(queryUpdate);

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteOneRow(long Id)  
        {
            if (BodyRows.ContainsKey(Id))
            {
                BodyRows.Remove(Id);

                string queryDelete = $"DELETE FROM {_name} WHERE id = {Id}";
               
                queriesForAsync.Add(queryDelete);

                return true;
            }
            else
            {
                return false;
            }
        }

       

        public void Async()
        {
            SQLiteConnector.Connection.Open();
            foreach (string queryGetTablesData in queriesForAsync)
            {
                SQLiteCommand sQLiteCommand = new SQLiteCommand(queryGetTablesData, SQLiteConnector.Connection);
                int insertedCount = sQLiteCommand.ExecuteNonQuery();

            }
            SQLiteConnector.Connection.Close();
        }

    }
}
