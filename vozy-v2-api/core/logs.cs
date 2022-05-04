using System.IO;
namespace vozy_v2_api.core
{
    public static class logs
    {
        public static void logError(string err)
        {
            string path = "~/logs-vozy/";
            string date = $"{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}.txt";
            if(Directory.Exists(path))
            {
                StreamWriter sw = new StreamWriter(path+date,true);
                sw.WriteLine($"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second} - {err}");
                sw.Close();
            }
            else
            {
                Directory.CreateDirectory(path);
                logError(err);
            }
        }
    }
}
