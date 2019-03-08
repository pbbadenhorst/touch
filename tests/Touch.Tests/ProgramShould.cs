using System;
using System.IO;
using Xunit;
using static Ez.Tools.Touch.Properties.Resources;

namespace Ez.Tools.Touch.Tests
{
    public class ProgramShould : IDisposable
    {
        StringWriter ConsoleWriter { get; }

        public ProgramShould()
        {
            ConsoleWriter = new StringWriter();
            Console.SetOut(ConsoleWriter);
        }

        public void Dispose()
        {
            ConsoleWriter.Dispose();
        }

        [Fact]
        public void PrintFailureAndReturnOneWhenCalledWithNull()
        {
            // Act
            var code = Program.Main(null);

            // Assert
            Assert.Equal(1, code);
            Assert.Contains(UsageInformation, ConsoleWriter.ToString());
        }

        [Fact]
        public void PrintUsageInfomationAndReturnOneWhenCalledWithoutArguments()
        {
            // Act
            var code = Program.Main(new string[0]);

            // Assert
            Assert.Equal(1, code);
        }

        [Fact]
        public void PrintUsageInfomationAndReturnOneWhenCalledWithTooManyArguments()
        {
            // Act
            var code = Program.Main(new string[0]);

            // Assert
            Assert.Equal(1, code);
            Assert.Contains(UsageInformation, ConsoleWriter.ToString());
        }

        [Fact]
        public void PrintSuccessAndReturnZeroWhenCalledWithAValidFileName()
        {
            // Arrange
            var filePath = "temp.file";
            try
            {
                // Act
                var code = Program.Main(new[] { filePath });

                // Assert
                Assert.Equal(0, code);
                Assert.True(File.Exists(filePath));
                Assert.Contains(FileCreated, ConsoleWriter.ToString());
            }
            finally
            {
                // Ameliorate
                File.Delete(filePath);
            }
        }

        [Fact]
        public void PrintSuccessAndReturnZeroWhenCalledWithAValidDotFileName()
        {
            // Arrange
            var filePath = ".special";
            try
            {
                // Act
                var code = Program.Main(new[] { filePath });

                // Assert
                Assert.Equal(0, code);
                Assert.True(File.Exists(filePath));
                Assert.Contains(FileCreated, ConsoleWriter.ToString());
            }
            finally
            {
                // Ameliorate
                File.Delete(filePath);
            }
        }

        [Fact]
        public void PrintSuccessAndReturnZeroWhenCalledWithAnExistingFileName()
        {
            // Arrange
            var filePath = "existing.file";
            try
            {
                File.CreateText(filePath).Close();

                // Act
                var code = Program.Main(new[] { filePath });

                // Assert
                Assert.Equal(0, code);
                Assert.True(File.Exists(filePath));
                Assert.Contains(FileAlreadyExists, ConsoleWriter.ToString());
            }
            finally
            {
                // Ameliorate
                File.Delete(filePath);
            }
        }

        [Fact]
        public void PrintFailureAndReturnMinusOneWhenCalledWithAnNonExistingDriveAndFolder()
        {
            // Arrange
            var filePath = @"does\not\exist.tmp";

            // Act
            var code = Program.Main(new[] { filePath });

            // Assert
            Assert.Equal(-1, code);
            Assert.False(File.Exists(filePath));
            Assert.Contains(FailedToCreateFile.Replace("{0}", string.Empty), ConsoleWriter.ToString());
            Assert.Contains("Could not find a part of the path", ConsoleWriter.ToString());
        }

        [Fact]
        public void PrintFailureAndReturnMinusOneWhenCalledWithAnInvalidFileName()
        {
            // Arrange
            var filePath = "s" + new string(Path.GetInvalidFileNameChars()) + ".ext";

            // Act
            var code = Program.Main(new[] { filePath });

            // Assert
            Assert.Equal(-1, code);
            Assert.False(File.Exists(filePath));
            Assert.Contains(FailedToCreateFile.Replace("{0}", string.Empty), ConsoleWriter.ToString());
            Assert.Contains("Illegal characters in path", ConsoleWriter.ToString());
        }
    }
}
