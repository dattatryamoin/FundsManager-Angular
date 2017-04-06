using System.Configuration;

namespace BL
{
    internal class Config
    {

        public static string Delimiter
        {
            get
            {

                string delimiter = ConfigurationManager.AppSettings["Delimiter"];
                if (!string.IsNullOrEmpty(delimiter))
                    return delimiter;

                return ";";
            }
        }

        public static string FilePath
        {
            get
            {

                string filePath = ConfigurationManager.AppSettings["FilePath"];
                if (!string.IsNullOrEmpty(filePath))
                    return filePath;

                return "http://portal.amfiindia.com/spages/NAV0.txt";
            }
        }

    }
}
