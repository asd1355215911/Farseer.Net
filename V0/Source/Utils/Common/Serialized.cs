using System;
using System.IO;
using System.Web;
using System.Windows.Forms;
using System.Xml.Serialization;
using FS.Utils.Web;

namespace FS.Utils.Common
{
    /// <summary>
    ///     SerializationHelper ��ժҪ˵����
    /// </summary>
    public abstract class Serialized<T>
    {
        /// <summary>
        ///     �����л�
        /// </summary>
        /// <param name="filePath">�ļ�·��</param>
        /// <returns></returns>
        public static T Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return default(T);
            }

            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var serializer = new XmlSerializer(typeof (T));
                return (T) serializer.Deserialize(fs);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }


        /// <summary>
        ///     ���л�
        /// </summary>
        /// <param name="t">����</param>
        /// <param name="filePath">�ļ�·��</param>
        public static bool Save(T t, string filePath)
        {
            var succeed = false;
            FileStream fs = null;
            try
            {
                Directory.CreateDirectory(filePath.Substring(0, filePath.LastIndexOf("\\")));
                fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                var serializer = new XmlSerializer(t.GetType());
                serializer.Serialize(fs, t);
                succeed = true;
            }
            catch (Exception ex)
            {
                var context = HttpContext.Current;
                if (context != null)
                {
                    new Terminator().Throw(ex.Message);
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return succeed;
        }
    }
}