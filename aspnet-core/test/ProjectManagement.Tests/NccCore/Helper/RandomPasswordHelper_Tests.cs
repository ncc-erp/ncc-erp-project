using NccCore.Helper;
using ProjectManagement.NccCore.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ProjectManagement.Tests.NccCore.Helper
{
    public class UserHelper_Tests
    {
        [Fact]
        public void GetUserName_Test()
        {
            // Act
            var output = UserHelper.GetUserName("tranvana@gmail.com");

            // Assert
            Assert.Equal("tranvana", output);

            // Act
            output = UserHelper.GetUserName("tranvana@ncc.asia");

            // Assert
            Assert.Equal("tranvana", output);

            // Act
            output = UserHelper.GetUserName("tranvana");

            // Assert
            Assert.Equal("tranvana", output);

        }
    }
}
