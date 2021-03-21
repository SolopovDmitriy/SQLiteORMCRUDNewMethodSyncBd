using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteORM
{
    class Program
    {


       




        static void Main(string[] args)
        {
            try
            {
                /*SQLiteConnector.CreateDatabaseSource("test.db");
                SQLiteConnector.Connection.Open();
                string createStudentTableQuery = "CREATE TABLE IF NOT EXISTS students (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, fio VARCHAR(128) NOT NULL, age INTEGER )";
                    SQLiteCommand sQLiteCommand = new SQLiteCommand(createStudentTableQuery, SQLiteConnector.Connection);
                sQLiteCommand.ExecuteNonQuery();
                SQLiteConnector.Connection.Close();*/

                string pathTofile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database", "test.db");
                SQLiteDBEngine dBEngine = new SQLiteDBEngine(pathTofile, SQLIteMode.EXISTS);


                //Console.WriteLine(dBEngine["student"].Name);

                SQLiteTable Students = dBEngine["student"];

                foreach (SQLiteColumn col in Students.HeadRowInfo)
                {
                    Console.WriteLine(col);
                }
                Console.WriteLine("Данные таблицы");
                foreach (var col in Students.BodyRows)
                {
                    Console.WriteLine($"ID: {col.Key}");
                    foreach (var item in col.Value)
                    {
                        Console.Write(item + " ");
                    }
                    Console.WriteLine();
                }








                /*long lastIndex = Students.BodyRows.Count + 1;

                List<string> newRow = new List<string>();
                    newRow.Add("Вася");
                    newRow.Add(15.ToString());
                Students.BodyRows.Add(lastIndex, newRow);*/


                Students.AddOneRow(new List<string> { "Маклауд", 45.ToString() });
                List<KeyValuePair<string, string>> searchPattern = new List<KeyValuePair<string, string>>();
                searchPattern.Add(new KeyValuePair<string, string>("age", "35"));
                searchPattern.Add(new KeyValuePair<string, string>("fio", "Руслан"));
                searchPattern.Add(new KeyValuePair<string, string>("Id", 3.ToString()));


                //-------------------------------------------------------------------------------------------------------------------------start version 2

                // version 2  PK is not first column 
                //Dictionary<string, string> searchPattern = new Dictionary<string, string>();
                //searchPattern.Add("age", "35");
                //searchPattern.Add("fio", "Руслан");
                //searchPattern.Add("id", "3");



                //-------------------------------------------------------------------------------------------------------------------------stop version 2





                #region test List<KeyValuePair<string, string>>
                //Console.WriteLine(searchPattern[0].Value);
                //Console.WriteLine(searchPattern[1].Value);

                //for (int i = searchPattern.Count - 1; i >= 0; i--)
                //{
                //    Console.WriteLine(searchPattern[i].Value);
                //}

                //for (int i=0; i < searchPattern.Count; i++)
                //{
                //    Console.WriteLine(searchPattern[i].Value);
                //}
                //foreach (KeyValuePair<string, string> item in searchPattern)
                //{
                //    Console.WriteLine(item.Key);
                //}

                //foreach (KeyValuePair<string, string> item in searchPattern)
                //{
                //    if (!Students.HeadRowInfo[item.Key].IsPrimaryKey)
                //        Console.WriteLine(item.Value);
                //}



                //KeyValuePair<string, string> x = new KeyValuePair<string, string>("age", "35");
                //Console.WriteLine("hello: " +  x.Key);

                #endregion


                KeyValuePair<long, List<string>> findedStudent = Students.GetOneRow(searchPattern);
                if (findedStudent.Value != null)
                {
                    Console.WriteLine("findedStudent: " + String.Join(" -- ", findedStudent.Value));
                }
                else
                {
                    Console.WriteLine("not found ");
                }

                Students.Print();
                Students.UpdateOneRow(4, new List<string> { "Peter", "33"});
                Students.Print();


                // async  version1
                // Students.Create(new List<string> { "Peter", "33"}) ;   // sql query  = "insert into student('fio',age) values ('Peter',33)" - 
                //сохраняем запрос для синхронизации в будущем

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
                foreach (SQLiteColumn col in Students.HeadRowInfo)
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

                foreach (var col in Students.BodyRows)
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

                //Console.Write(" {0,6} ", "Ошибка");
                //Console.Write(" {0,20} ", "Ошибка"); 
                //Console.Write(" {0,6} ", "Ошибка");


                /*foreach (var item in dBEngine.Tables)
                {
                    Console.WriteLine(item);
                }
                dBEngine.getTableFields("student");*/

                //string[] str = { };
                /*SQLiteColumn sQLiteColumn = new SQLiteColumn(str);
                //sQLiteColumn.GetDateType("VARCHAR(128)");
                Console.WriteLine(sQLiteColumn.DataType);
                */

                /* SQLiteRow sQLiteRow = new SQLiteRow(3);
                 foreach (var item in sQLiteRow)
                 {
                     Console.WriteLine(item);
                 }*/
                //Console.WriteLine(sQLiteRow[0]);
                //Console.WriteLine(sQLiteRow["id"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
            //              |
            //              |    
            //PRAGMA table_info('student');
            //SELECT * FROM student 

        }
    }
}
