using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace DirectorySync.Tests
{
    [TestClass]
    public class IsFileNewTests
    {
        private readonly Utilities _utilities = new Utilities();
        private static readonly string _sourceFile = "Resources\\Source.testfile";
        private static readonly string _destinationFile = "Resources\\Destination.testfile";

        [TestMethod]
        public void Source_LastWriteTime_GreaterThan_Destination_LastWriteTime_Should_Return_True()
        {
            //Arrange
            File.SetLastWriteTime(_sourceFile, DateTime.Now);
            File.SetLastWriteTime(_destinationFile, DateTime.Now.AddSeconds(-1));

            //Act
            var result = _utilities.IsFileNew(_sourceFile, _destinationFile);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void Source_LastWriteTime_LessThan_Destination_LastWriteTime_Should_Return_False()
        {
            //Arrange
            File.SetLastWriteTime(_sourceFile, DateTime.Now.AddSeconds(-1));
            File.SetLastWriteTime(_destinationFile, DateTime.Now);

            //Act
            var result = _utilities.IsFileNew(_sourceFile, _destinationFile);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Source_LastWriteTime_EqualTo_Destination_LastWriteTime_Should_Return_False()
        {
            //Arrange
            var dateTime = DateTime.Now;
            File.SetLastWriteTime(_sourceFile, dateTime);
            File.SetLastWriteTime(_destinationFile, dateTime);

            //Act
            var result = _utilities.IsFileNew(_sourceFile, _destinationFile);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Source_NotExists_Should_Return_False()
        {
            //Arrange
            File.SetLastWriteTime(_destinationFile, DateTime.Now);

            //Act
            var result = _utilities.IsFileNew("NonExistingFile.testfile", _destinationFile);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Destination_NotExists_Should_Return_True()
        {
            //Arrange
            File.SetLastWriteTime(_sourceFile, DateTime.Now);

            //Act
            var result = _utilities.IsFileNew(_sourceFile, "NonExistingFile.testfile");

            //Assert
            result.Should().BeTrue();
        }
    }
}
