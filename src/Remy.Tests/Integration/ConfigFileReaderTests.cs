using System;
using NUnit.Framework;
using System.IO;
using Remy.Core;
using Remy.Core.Config;

namespace Remy.Tests.Integration
{
	[TestFixture]
	public class ConfigFileReaderTests
    {
		[Test]
		public void should_read_file_from_disk()
		{
			// Arrange
			string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test-config.yml");
			var fileReader = new ConfigFileReader();

			// Act
			string actualContent = fileReader.Read(new Uri(configPath));

			// Assert
			Assert.That(actualContent, Does.Contain("name: \"Text example\""));
		}

		[Test]
		public void should_read_file_from_url()
		{
			// Arrange
			var fileReader = new ConfigFileReader();

			// Act
			string actualContent = fileReader.Read(new Uri("http://www.example.com"));

			// Assert
			Assert.That(actualContent, Does.Contain("Example Domain"));
		}

        [Test]
        public void GetFullPathForFile_should_return_full_path_for_filename()
        {
            // Arrange
            string expectedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "myfile.txt");

            // Act
            string actualPath = ConfigFileReader.GetFullPathForFile("myfile.txt");

            // Assert
            Assert.That(actualPath, Is.EqualTo(expectedPath));
        }

        [Test]
		public void should_throw_remyexception_for_bad_filepath()
		{
            // Arrange
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "doesnexist");
            var fileReader = new ConfigFileReader();

            // Act + Assert
		    Assert.Throws<RemyException>(() => fileReader.Read(new Uri(configPath)));
		}

        [Test]
        public void should_throw_remyexception_for_invalid_uri()
        {
            // Arrange
            var fileReader = new ConfigFileReader();

            // Act + Assert
            Assert.Throws<RemyException>(() => fileReader.Read(new Uri("http://bleh")));
        }

        [Test]
        public void should_throw_remyexception_for_unsupported_scheme()
        {
            // Arrange
            var fileReader = new ConfigFileReader();

            // Act + Assert
            Assert.Throws<RemyException>(() => fileReader.Read(new Uri("ftp://warezplace")));
        }
    }
}