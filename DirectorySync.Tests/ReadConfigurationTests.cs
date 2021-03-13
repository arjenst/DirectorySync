using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DirectorySync.Tests
{
    [TestClass]
    public class ReadConfigurationTests
    {
        private readonly Utilities _utilities = new Utilities();

        [TestMethod]
        public void ConfigurationFile_Not_Contains_directorySync_Section_Should_Return_Empty_List()
        {
            //Arrange
            
            //Act
            var result = _utilities.ReadConfiguration();

            //Assert
            result.Should().BeOfType<List<ConfigurationObject>>();
            result.Count.Should().Be(0);
        }
    }
}
