using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ServiceProcess;

namespace DirectorySync.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        private static string _serviceName = "DirectorySync";
        private int _timeoutMilliseconds = 1000;
        private ServiceController _service = new ServiceController(_serviceName);

        [Ignore]
        [TestMethod]
        public void Restart_Service()
        {
            int millisec1 = Environment.TickCount;
            TimeSpan timeout = TimeSpan.FromMilliseconds(_timeoutMilliseconds);

            if (_service.Status == ServiceControllerStatus.Running)
            {
                _service.Stop();
                _service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
            }

            // count the rest of the timeout
            int millisec2 = Environment.TickCount;
            timeout = TimeSpan.FromMilliseconds(_timeoutMilliseconds - (millisec2 - millisec1));

            _service.Start();
            _service.WaitForStatus(ServiceControllerStatus.Running, timeout);
        }
    }
}
