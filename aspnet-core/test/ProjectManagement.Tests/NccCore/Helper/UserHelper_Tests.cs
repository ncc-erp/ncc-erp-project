using NccCore.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ProjectManagement.Tests.NccCore.Helper
{
    public class RandomPasswordHelper_Tests
    {
        [Fact]
        public void CreateRandomPassword_Test()
        {
            for (int i = 1; i< 20; i++)
            {
                // Act
                var output = RandomPasswordHelper.CreateRandomPassword(i);

                // Assert
                Assert.Equal(i, output.Length);
            }
            
        }
    }
}
