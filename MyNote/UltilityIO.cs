using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote
{
    class UltilityIO
    {
        public static void FullBackUp()
        {
            string path = System.Environment.CurrentDirectory;
            string parentPath = path.Substring(0, path.LastIndexOf('\\'));//删除文件名
            string back = parentPath + "\\back\\" + DateTime.Today.ToString("yyyyMMdd") + "\\";
            CopyFolder(path, back);

        }

        public static void AutoBackUp()
        {
            string path = System.Environment.CurrentDirectory;
            string parentPath = path.Substring(0, path.LastIndexOf('\\'));//删除文件名
            string back = parentPath + "\\AutoBack\\" + DateTime.Today.ToString("yyyyMMdd") + "\\";
            if(!Directory.Exists(back))
                CopyFolder(path, back);
        }

        private static void CopyFolder(string from, string to)
        {
            if (!Directory.Exists(to))
                Directory.CreateDirectory(to);

            // 子文件夹
            foreach (string sub in Directory.GetDirectories(from))
                CopyFolder(sub + "\\", to + Path.GetFileName(sub) + "\\");

            // 文件
            foreach (string file in Directory.GetFiles(from))
                File.Copy(file, to + Path.GetFileName(file), true);
        }
    }

    public class XmlIO<T>
        where T : new()
    {
        public T ReadFile(T obj, string fileName)
        {
            if (!File.Exists(fileName))
            {
                SaveFile(obj, fileName);
            }

            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(T));
            System.IO.StreamReader sr = new System.IO.StreamReader(
                fileName, new System.Text.UTF8Encoding(false));
            obj = (T)serializer.Deserialize(sr);
            sr.Close();
            return obj;
        }

        public void SaveFile(T obj, string fileName)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(T));
            System.IO.StreamWriter sw = new System.IO.StreamWriter(
                fileName, false, new System.Text.UTF8Encoding(false));
            serializer.Serialize(sw, obj);
            sw.Close();
        }
    }

    public class NpgsqlDBImport
    {
        public static string QuoConv(string str)
        {
            string tmp = str;
            if (tmp == null)
                tmp = "";
            if (tmp.Contains("'"))
                tmp = tmp.Replace("'", "''");
            return tmp;
        }

        public static int GetLastId(string tbl, int defaultId, NpgsqlConnection conn)
        {
            int id = defaultId;
            string cmd_str = string.Format("select Max(id) from " + tbl + "; ");
            NpgsqlCommand cmd = new NpgsqlCommand(cmd_str, conn);
            using (NpgsqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    if (!dr.IsDBNull(0))
                    {
                        id = dr.GetInt32(0);
                    }
                }
            }
            return id;
        }

        public static void NpgsqlDBImportEvent(ModelEvent mEvent)
        {
            string conn_str = "Server=localhost;Port=5432;User ID=xiao;Database=note;Password=xiao;Enlist=true";
            using (NpgsqlConnection conn = new NpgsqlConnection(conn_str))
            {
                //PostgreSQLへ接続
                conn.Open();
                int evid = GetLastId("event", 10000000, conn);
                int chid = GetLastId("event_item", 11000000, conn);

                List<ImportEventItem> orderList = new List<ImportEventItem>();
                foreach (var ev in mEvent.Items)
                {
                    if (ev.Name == "Memo")
                    {
                        string cmd_str2 = string.Format("insert into event (id, name, category, status, sorted) values ('{0}', '{1}', '{2}', '{3}', '{4}');",
                                                ++evid, "Diary", QuoConv(ev.Category), "Regular", "Diary");
                        NpgsqlCommand cmd2 = new NpgsqlCommand(cmd_str2, conn);
                        var rtn2 = cmd2.ExecuteNonQuery();
                        foreach (var child in ev.Items)
                        {
                            cmd_str2 = string.Format("insert into event_item (id, date, memo, time, parent_id) values ('{0}', '{1}', '{2}', '{3}', '{4}');",
                                                    ++chid, child.Date.ToString("yyyy-MM-dd"), "", child.Time, evid);
                            cmd2 = new NpgsqlCommand(cmd_str2, conn);
                            rtn2 = cmd2.ExecuteNonQuery();

                            cmd_str2 = string.Format("insert into diary (date, memo, length) values ('{0}', '{1}', '{2}');",
                                                    child.Date.ToString("yyyy-MM-dd"), QuoConv(child.Memo), child.Memo.Length);
                            cmd2 = new NpgsqlCommand(cmd_str2, conn);
                            rtn2 = cmd2.ExecuteNonQuery();
                        }
                        continue;
                    }

                    string cmd_str = string.Format("insert into event (id, name, category, status, sorted) values ('{0}', '{1}', '{2}', '{3}', '{4}');",
                                            ++evid, QuoConv(ev.Name), QuoConv(ev.Category), QuoConv(ev.Status), "");
                    NpgsqlCommand cmd = new NpgsqlCommand(cmd_str, conn);
                    var rtn1 = cmd.ExecuteNonQuery();
                    foreach (var child in ev.Items)
                    {
                        orderList.Add(new ImportEventItem(child, evid));
                    }
                    Console.WriteLine("Insert success! {0}", ev.Name);
                }

                Comparison<ImportEventItem> comparison = new Comparison<ImportEventItem>
                     ((ImportEventItem x, ImportEventItem y) => {
                         return x.Date.CompareTo(y.Date);
                     });
                orderList.Sort(comparison);//1、Comparison<Person>

                foreach (var child in orderList)
                {
                    string cmd_str = string.Format("insert into event_item (id, date, memo, time, parent_id) values ('{0}', '{1}', '{2}', '{3}', '{4}');",
                                            ++chid, child.Date.ToString("yyyy-MM-dd"), QuoConv(child.Memo), child.Time, child.parentId);
                    NpgsqlCommand cmd2 = new NpgsqlCommand(cmd_str, conn);
                    var rtn2 = cmd2.ExecuteNonQuery();
                }
                Console.WriteLine("Insert Complete!");
                conn.Close();
            }
        }

        public static void NpgsqlDBImportPerson(ModelPerson mPerson)
        {

            string conn_str = "Server=localhost;Port=5432;User ID=xiao;Database=note;Password=xiao;Enlist=true";
            using (NpgsqlConnection conn = new NpgsqlConnection(conn_str))
            {
                //PostgreSQLへ接続
                conn.Open();
                List<ImportEventItem> orderList = new List<ImportEventItem>();

                int evid = GetLastId("event", 10000000, conn);
                int chid = GetLastId("event_item", 11000000, conn);
                int personid = GetLastId("person", 21000000, conn);

                foreach (var item in mPerson.Items)
                {

                    string cmd_str = string.Format("insert into person (id, name, category, status, memo, name2, name3, address, input_date) values " +
                                                                "('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');",
                                            ++personid, QuoConv(item.Name), QuoConv(item.Category), QuoConv(item.Status), QuoConv(item.Memo),
                                            QuoConv(item.Name2), QuoConv(item.Name3), QuoConv(item.Address), QuoConv(item.InputDate.ToString("yyyy-MM-dd")));
                    NpgsqlCommand cmd = new NpgsqlCommand(cmd_str, conn);
                    var rtn1 = cmd.ExecuteNonQuery();

                    cmd_str = string.Format("select Count(*) from event where name = '{0}' or name = '{1}' or name = '{2}' ", QuoConv(item.Name), QuoConv(item.Name2), QuoConv(item.Name3));
                    cmd = new NpgsqlCommand(cmd_str, conn);
                    int rtnCount = 0;
                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (!dr.IsDBNull(0))
                            {
                                rtnCount = dr.GetInt32(0);
                            }
                        }
                    }
                    if (rtnCount > 0)
                        continue;

                    cmd_str = string.Format("insert into event (id, name, category, status, sorted) values ('{0}', '{1}', '{2}', '{3}', '{4}');",
                                            ++evid, QuoConv(item.Name), "Communicate", "Finished", "Person");
                    cmd = new NpgsqlCommand(cmd_str, conn);
                    rtn1 = cmd.ExecuteNonQuery();
                    cmd_str = string.Format("insert into event_item (id, date, memo, time, parent_id) values ('{0}', '{1}', '{2}', '{3}', '{4}');",
                                            ++chid, QuoConv(item.InputDate.ToString("yyyy-MM-dd")), "", 0.1, evid);
                    cmd = new NpgsqlCommand(cmd_str, conn);
                    rtn1 = cmd.ExecuteNonQuery();

                    Console.WriteLine("Insert success! {0}", item.Name);
                }

                Console.WriteLine("Insert Complete!");
                conn.Close();
            }
        }
        public static void NpgsqlDBImportCollection(ModelCollection mCollection)
        {

            string conn_str = "Server=localhost;Port=5432;User ID=xiao;Database=note;Password=xiao;Enlist=true";
            using (NpgsqlConnection conn = new NpgsqlConnection(conn_str))
            {
                //PostgreSQLへ接続
                conn.Open();
                List<ImportEventItem> orderList = new List<ImportEventItem>();

                int evid = GetLastId("event", 10000000, conn);
                int chid = GetLastId("event_item", 11000000, conn);
                int collectionid = GetLastId("collection", 30000000, conn);
                int collectionchildid = GetLastId("collection_item", 31000000, conn);

                foreach (var item in mCollection.Items)
                {
                    string cmd_str = string.Format("insert into collection (id, name, category, status, memo, name2, name3, level, input_date, release_date) values " +
                                                                "('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');",
                                            ++collectionid, QuoConv(item.Name), QuoConv(item.Category), QuoConv(item.Status), QuoConv(item.Memo),
                                            QuoConv(item.Name2), QuoConv(item.Name3), item.Level, QuoConv(item.InputDate.ToString("yyyy-MM-dd")), QuoConv(item.ReleaseDate.ToString("yyyy-MM-dd")));
                    NpgsqlCommand cmd = new NpgsqlCommand(cmd_str, conn);
                    var rtn1 = cmd.ExecuteNonQuery();

                    foreach (var child in item.Character)
                    {
                        cmd_str = string.Format("insert into collection_item (id, name, type, index, memo, name2, name3, parent_id, input_date, update_date) values " +
                                                                    "('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');",
                                                ++collectionchildid, QuoConv(child.Name), "Character", child.Index, QuoConv(child.Memo),
                                                QuoConv(child.Name2), QuoConv(child.Name3), collectionid, QuoConv(child.Date.ToString("yyyy-MM-dd")), QuoConv(child.Date.ToString("yyyy-MM-dd")));
                        cmd = new NpgsqlCommand(cmd_str, conn);
                        rtn1 = cmd.ExecuteNonQuery();
                    }

                    foreach (var child in item.Paragraph)
                    {
                        cmd_str = string.Format("insert into collection_item (id, name, type, index, memo, name2, name3, parent_id, input_date, update_date) values " +
                                                                    "('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');",
                                                ++collectionchildid, QuoConv(child.Name), "Paragraph", child.Index, QuoConv(child.Memo),
                                                "", "", collectionid, QuoConv(child.Date.ToString("yyyy-MM-dd")), QuoConv(child.Date.ToString("yyyy-MM-dd")));
                        cmd = new NpgsqlCommand(cmd_str, conn);
                        rtn1 = cmd.ExecuteNonQuery();
                    }

                    foreach (var child in item.Setting)
                    {
                        cmd_str = string.Format("insert into collection_item (id, name, type, index, memo, name2, name3, parent_id, input_date, update_date) values " +
                                                                    "('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');",
                                                ++collectionchildid, QuoConv(child.Name), "Setting", child.Index, QuoConv(child.Memo),
                                                "", "", collectionid, QuoConv(child.Date.ToString("yyyy-MM-dd")), QuoConv(child.Date.ToString("yyyy-MM-dd")));
                        cmd = new NpgsqlCommand(cmd_str, conn);
                        rtn1 = cmd.ExecuteNonQuery();
                    }

                    foreach (var child in item.Tags)
                    {
                        cmd_str = string.Format("insert into collection_parent_bean_tags (collection_parent_bean_id, tag) values " +
                                                                    "('{0}', '{1}');",
                                                collectionid, QuoConv(child));
                        cmd = new NpgsqlCommand(cmd_str, conn);
                        rtn1 = cmd.ExecuteNonQuery();
                    }

                    cmd_str = string.Format("select Count(*) from event where name = '{0}' or name = '{1}' or name = '{2}' ", QuoConv(item.Name), QuoConv(item.Name2), QuoConv(item.Name3));
                    cmd = new NpgsqlCommand(cmd_str, conn);
                    int rtnCount = 0;
                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (!dr.IsDBNull(0))
                            {
                                rtnCount = dr.GetInt32(0);
                            }
                        }
                    }
                    if (rtnCount > 0)
                        continue;

                    cmd_str = string.Format("insert into event (id, name, category, status, sorted) values ('{0}', '{1}', '{2}', '{3}', '{4}');",
                                            ++evid, QuoConv(item.Name), QuoConv(item.Category), "Finished", "Collection");
                    cmd = new NpgsqlCommand(cmd_str, conn);
                    rtn1 = cmd.ExecuteNonQuery();
                    cmd_str = string.Format("insert into event_item (id, date, memo, time, parent_id) values ('{0}', '{1}', '{2}', '{3}', '{4}');",
                                            ++chid, QuoConv(item.InputDate.ToString("yyyy-MM-dd")), "", 0.1, evid);
                    cmd = new NpgsqlCommand(cmd_str, conn);
                    rtn1 = cmd.ExecuteNonQuery();

                    Console.WriteLine("Insert success! {0}", item.Name);
                }

                Console.WriteLine("Insert Complete!");
                conn.Close();
            }
        }

        public static void NpgsqlDBImportCategory(ModelCategory mCategory)
        {

            string conn_str = "Server=localhost;Port=5432;User ID=xiao;Database=note;Password=xiao;Enlist=true";
            using (NpgsqlConnection conn = new NpgsqlConnection(conn_str))
            {
                //PostgreSQLへ接続
                conn.Open();

                int cid = GetLastId("category", 40000000, conn);
                int chid = GetLastId("category_item", 41000000, conn);

                for(int i= mCategory.Items.Count - 1; i >= 0; i--)
                {
                    var item = mCategory.Items[i];
                    var root = InsertCategoryChild(conn, item.Category, 0, 0, ref chid);

                    string cmd_str = string.Format("insert into category (id, name, start_date, end_date, input_date, item_id) values " +
                                                                "('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                                            ++cid, QuoConv(item.Name), QuoConv(item.Date.ToString("yyyy-MM-dd")), "9999-12-31",
                                            QuoConv(item.Date.ToString("yyyy-MM-dd")), root);
                    NpgsqlCommand cmd = new NpgsqlCommand(cmd_str, conn);
                    var rtn1 = cmd.ExecuteNonQuery();

                    Console.WriteLine("Insert success! {0}", item.Name);
                }

                Console.WriteLine("Insert Complete!");
                conn.Close();
            }

        }

        public static int InsertCategoryChild(NpgsqlConnection conn, Category cat, int root, int parent, ref int chid)
        {
            string cmd_str2 = "";

            if (root == 0)
            {
                ++chid;
                if (root == 0)
                    root = chid;
                cmd_str2 = string.Format("insert into category_item (id, name, root, parent_id) values " +
                                                                            "('{0}', '{1}', '{2}', null);",
                                                        chid, QuoConv(cat.Name), root);
            }
            else
            {
                cmd_str2 = string.Format("insert into category_item (id, name, root, parent_id) values " +
                                                                            "('{0}', '{1}', '{2}', '{3}');",
                                                        ++chid, QuoConv(cat.Name), root, parent);
            }


            NpgsqlCommand cmd2 = new NpgsqlCommand(cmd_str2, conn);
            var rtn2 = cmd2.ExecuteNonQuery();

            parent = chid;            
            foreach (var child in cat.Subs)
            {
                InsertCategoryChild(conn, child, root, parent, ref chid);
            }

            return root;
        }
    }
}
