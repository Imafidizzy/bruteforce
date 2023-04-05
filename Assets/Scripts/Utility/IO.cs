using System.IO;

namespace Utility
{
    public static class IO
    {
        public static void WriteString(string path, string data)
        {
            StreamWriter writer = new StreamWriter(path);
            writer.WriteLine(data);
            writer.Close();
        }

        public static string ReadString(string path)
        {
            StreamReader reader = new StreamReader(path);
            string result = reader.ReadToEnd();
            reader.Close();
            return result;
        }
    }
}