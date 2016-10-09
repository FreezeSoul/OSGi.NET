using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Xml.Serialization;

namespace OsgiDllTool
{
    public class Helper
    {
        #region XmlSerializer序列化
        /// <summary>
        /// 将对象序列化到 XML 文档中和从 XML 文档中反序列化对象。XmlSerializer 使您得以控制如何将对象编码到 XML 中。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SerializeXml(object value)
        {
            var serializer = new XmlSerializer(value.GetType());
            using (var ms = new MemoryStream())
            {
                serializer.Serialize(ms, value);
                ms.Seek(0, SeekOrigin.Begin);
                using (var sr = new StreamReader(ms))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        /// <summary>
        ///  XmlSerializer反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static object DeserializeXml(Type type, string str)
        {
            var serializer = new XmlSerializer(type);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
            using (var ms = new MemoryStream(bytes))
            {
                return serializer.Deserialize(ms);
            }
        }
        #endregion

        public static List<string> CopyDll(List<string> fromPath, List<string> toPath, bool isDebug)
        {
            var list = new List<string>();

            for (int i = 0; i < fromPath.Count; i++)
            {
                try
                {
                    var uriFrom = new Uri(new Uri(AppDomain.CurrentDomain.BaseDirectory), fromPath[i]);
                    var uriTo = new Uri(new Uri(AppDomain.CurrentDomain.BaseDirectory), toPath[i]);
                    if (File.Exists(uriFrom.LocalPath))
                        File.Copy(uriFrom.LocalPath, uriTo.LocalPath + ".dll", true);
                    else
                    {
                        list.Add(uriFrom.LocalPath);
                    }
                    if (isDebug)
                    {
                        var from = uriFrom.LocalPath.Remove(uriFrom.LocalPath.Length - 4, 4) + ".pdb";
                        var to = uriTo.LocalPath + ".pdb";
                        if (File.Exists(from))
                            File.Copy(from, to, true);
                    }
                }
                catch (Exception e)
                {
                    list.Add(e.Message);
                }
            }
            return list;
        }

        public static void CreatFile(string file, string str)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
            var fs = new FileStream(file, FileMode.Create);
            var sw = new StreamWriter(fs, Encoding.UTF8);
            sw.Write(str);
            sw.Close();
            sw.Dispose();
            fs.Close();
            fs.Dispose();
        }

        public static string FromPathGetName(string path)
        {
            var list = path.Split('\\');
            string name = string.Empty;
            if (list.Length > 0)
            {
                name = list[list.Length - 1];
                if (name.ToLower().EndsWith(".dll"))
                {
                    name = name.Remove(name.Length - 4, 4);
                }
            }
            return name;
        }

        /// <summary>
        /// 转换绝对路径为相对路径
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <returns></returns>
        public static string Transform(string absolutePath)
        {
            StringBuilder path = new StringBuilder(2600);
            PathRelativePathTo(path, System.Windows.Forms.Application.StartupPath, System.IO.FileAttributes.Directory, absolutePath, System.IO.FileAttributes.Normal);
            if (path.Length == 0)
                return absolutePath;
            return path.ToString();
        }

        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        public static extern bool PathRelativePathTo([Out] StringBuilder pszPath, string pszFrom, FileAttributes dwAttrFrom, string pszTo, FileAttributes dwAttrTo);
    }
}
