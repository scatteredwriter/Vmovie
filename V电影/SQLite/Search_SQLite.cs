using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V电影.SQLite
{
    public class Search_SQLite
    {
        private const string db_name = "Search.db";
        private const string table_name = "History";
        private const string create_table = "CREATE TABLE IF NOT EXISTS " + table_name + " (KeyWord TEXT);";
        private const string get_all = "SELECT * FROM " + table_name + " ;";
        private const string query = "SELECT * FROM " + table_name + " WHERE KeyWord = ?;";
        private const string insert = "INSERT INTO " + table_name + " VALUES(?);";
        private const string delete = "DELETE FROM " + table_name + " WHERE KeyWord = ?;";
        private const string delete_all = "DELETE FROM " + table_name;

        private SQLiteConnection connection = new SQLiteConnection(db_name);

        public void Add_New_KeyWord(string keyword)
        {
            using (var statement = connection.Prepare(create_table))
            {
                statement.Step();
            }
            using (var statement = connection.Prepare(query))
            {
                statement.Bind(1, keyword);
                SQLiteResult result = statement.Step();
                if (SQLiteResult.ROW == result)
                {
                    Del_Old_KeyWord(keyword);
                }
            }
            using (var statement = connection.Prepare(insert))
            {
                statement.Bind(1, keyword);
                statement.Step();
            }
        }

        public ObservableCollection<string> Get_All_KeyWord()
        {
            using (var statement = connection.Prepare(create_table))
            {
                statement.Step();
            }
            using (var statement = connection.Prepare(get_all))
            {
                SQLiteResult result = statement.Step();
                ObservableCollection<string> lists = new ObservableCollection<string>();
                if (SQLiteResult.ROW == result)
                {
                    try
                    {
                        for (;;)
                        {
                            if (SQLiteResult.ROW == result)
                            {
                                lists.Add(statement[0].ToString());
                                result = statement.Step();
                            }
                            else if (SQLiteResult.DONE == result)
                            {
                                break;
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
                lists = new ObservableCollection<string>(lists.Reverse());
                return lists;
            }
        }

        public void Del_All_KeyWord()
        {
            using (var statement = connection.Prepare(delete_all))
            {
                statement.Step();
            }
        }

        public void Del_Old_KeyWord(string keyword)
        {
            using (var statement = connection.Prepare(delete))
            {
                statement.Bind(1, keyword);
                statement.Step();
            }
        }
    }
}
