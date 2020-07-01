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
}
