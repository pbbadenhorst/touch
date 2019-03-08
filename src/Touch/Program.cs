using System;
using System.IO;
using static Ez.Tools.Touch.Properties.Resources;

namespace Ez.Tools.Touch
{
    /// <summary>
    /// Represents the application.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">The supplied command line arguments.</param>
        /// <returns>
        /// The application returns the following status codes.
        /// <para>0 Successfully created the file or it already exists.</para>
        /// <para>-1 Failed to create the file</para>
        /// <para>1 No arguments specified.</para>
        /// </returns>
        internal static int Main(string[] args)
        {
            if (args == null || args.Length != 1)
            {
                Console.WriteLine(UsageInformation);
                return 1;
            }
            try
            {
                var filePath = args[0];

                if (File.Exists(filePath))
                {
                    Console.WriteLine(FileAlreadyExists);
                }
                else
                {
                    CreateEmptyFile(filePath);
                    Console.WriteLine(FileCreated);
                }
                return 0;
            }
            catch (Exception exception)
            {
                Console.WriteLine(FailedToCreateFile, exception.Message);
                return -1;
            }
        }

        private static void CreateEmptyFile(string filePath)
        {
            var stream = new FileStream(filePath, FileMode.CreateNew);
            stream.Dispose();
        }
    }
}
