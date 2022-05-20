using System;
using System.IO;
using System.Reflection;

namespace MeeyPage.Core
{
    public static class CommonFunction
    {
        /// <summary>
        /// Ghi lgo vào file
        /// </summary>
        /// <param name="logMessage"></param>
        public static void LogWrite(string logMessage)
        {
            string m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + "log.txt"))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Ghi lgo vào file
        /// </summary>
        /// <param name="logMessage"></param>
        public static void LogWrite(string projectName, string logMessage) 
        {
            string m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + "log.txt"))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Ghi log vào file
        /// </summary>
        /// <param name="logMessage"></param>
        /// <param name="txtWriter"></param>
        static void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }

    }
}
