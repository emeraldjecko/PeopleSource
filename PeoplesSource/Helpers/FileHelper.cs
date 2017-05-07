using System;
using System.IO;

namespace PeoplesSource.Helpers
{
    /// <summary>
    /// Class FileHelper.
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="text">The text.</param>
        public static void WriteLine(string fileName, string text)
        {
             // create a writer and open the file
            TextWriter tw = new StreamWriter(fileName, true);

            // write a line of text to the file
            tw.WriteLine("{0} - {1}", DateTime.Now, text);

            // close the stream
            tw.Close();
        }
    }
}