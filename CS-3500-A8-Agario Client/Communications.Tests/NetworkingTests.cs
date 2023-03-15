using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Communications.Tests
{
    /// <summary> 
    /// Author:    Tyler DeBruin and Rayyan Hamid
    /// Partner:   None
    /// Date:      3/28/2022
    /// Course:    CS 3500, University of Utah, School of Computing 
    /// Copyright: CS 3500 and Tyler DeBruin and Rayyan Hamid - This work may not be copied for use in Academic Coursework. 
    /// 
    /// I, Tyler DeBruin and Rayyan Hamid, certify that I wrote this code from scratch and did not copy it in part or whole from  
    /// another source.  All references used in the completion of the assignment are cited in my README file. 
    /// 
    /// File Contents 
    ///
    /// Class that contains all of the tests. Each Subclass is named after a method, and contains that method's test.
    /// </summary>
    [TestClass]
    public class NetworkingTests
    {
        /// <summary>
        /// Random - Generating a Random Port.
        /// </summary>
        private readonly Random _random;

        /// <summary>
        /// Constructor to Generate the Random Class.
        /// </summary>
        public NetworkingTests()
        {
            _random = new Random();
        }

        /// <summary>
        /// Tests the Connect Methods.
        /// </summary>
        [TestClass]
        public class Connect : NetworkingTests
        {
            /// <summary>
            /// Tests when a Host isnt available.
            /// </summary>
            [TestMethod]
            public void GIVEN_host_not_available_WHEN_Connect_THEN_response_correct()
            {
                var connected = false;

                var listeningPort = _random.Next(10000, 50000);

                var systemUnderTest = new Networking(NullLogger.Instance, (x) =>
                {
                    connected = true;
                },
                x =>{}, (x,y) =>{},'\n');

                try
                {
                    systemUnderTest.Connect("", listeningPort);

                    Assert.Fail("Should throw exception.");
                }
                catch (SocketException)
                {
                }

                Assert.IsFalse(connected);
            }

            /// <summary>
            /// Tests when a Host is available.
            /// </summary>
            [TestMethod] 
            public void GIVEN_host_listening_WHEN_Connect_THEN_response_correct()
            {
                var connected = false;
                int listeningPort = _random.Next(10000, 50000);

                var systemUnderTest = new Networking(NullLogger.Instance, (x) =>
                    {
                        connected = true;
                    },
                    x => { }, (x, y) => { }, '\n');

                var serverThread = new Thread(() =>
                {
                    systemUnderTest.WaitForClients(listeningPort, true);
                });

                serverThread.Start();

                systemUnderTest.Connect("", listeningPort);

                Assert.IsTrue(connected);

                systemUnderTest.StopWaitingForClients();
                systemUnderTest.Disconnect();
            }

            /// <summary>
            /// Calling Disconnect dopesn't dispose the TCP Client, just the connection. Allows you to reconnect without a new instance.
            /// </summary>
            [TestMethod]
            public void GIVEN_disconnected_previously_WHEN_Connect_THEN_response_correct()
            {
                var connected = false;

                int listeningPort = _random.Next(10000, 50000);

                var systemUnderTest = new Networking(NullLogger.Instance, (x) =>
                    {
                        connected = true;
                    },
                    x => { connected = false; }, (x, y) => { }, '\n');

                try
                {
                    systemUnderTest.Disconnect();

                    Assert.Fail();
                }
                catch (Exception)
                {
                }

                var serverThread = new Thread(() =>
                {
                    systemUnderTest.WaitForClients(listeningPort, true);
                });

                serverThread.Start();

                systemUnderTest.Connect("", listeningPort);

                Assert.IsTrue(connected);

                systemUnderTest.StopWaitingForClients();
                systemUnderTest.Disconnect();
            }
        }

        /// <summary>
        /// Disconnect Tests
        /// </summary>
        [TestClass]
        public class Disconnect : NetworkingTests
        {
            /// <summary>
            /// Disconnect doesn't blow up, but still calls (onDisconnect) delegate.
            /// </summary>
            [TestMethod]
            public void GIVEN_not_connected_WHEN_Disconnect_THEN_response_correct()
            {
                var disconnectCalled = false;

                var systemUnderTest = new Networking(NullLogger.Instance, (x) => {},
                    x => { disconnectCalled = true; }, (x, y) => { }, '\n');

                systemUnderTest.Disconnect();

                Assert.IsTrue(disconnectCalled);
            }

            /// <summary>
            /// Disconnect Happy Path works.
            /// </summary>
            [TestMethod]
            public void GIVEN_connected_WHEN_Disconnect_THEN_response_correct()
            {
                var connected = false;
                int listeningPort = _random.Next(10000, 50000);

                var systemUnderTest = new Networking(NullLogger.Instance, (x) =>
                    {
                        connected = true;
                    },
                    x => { connected = false; }, (x, y) => { }, '\n');

                var serverThread = new Thread(() =>
                {
                    systemUnderTest.WaitForClients(listeningPort, true);
                });

                serverThread.Start();

                while (!serverThread.IsAlive)
                {
                    Task.Delay(1000);
                }

                systemUnderTest.Connect("", listeningPort);

                Assert.IsTrue(connected);

                systemUnderTest.Disconnect();

                Assert.IsFalse(connected);

                systemUnderTest.StopWaitingForClients();
                serverThread.Join();
            }
        }

        /// <summary>
        /// WaitForClients method works correctly.
        /// </summary>
        [TestClass]
        public class WaitForClients : NetworkingTests
        {
            /// <summary>
            /// Tests the WaitForClients method.
            /// </summary>
            [TestMethod]
            public void WHEN_WaitForClients_THEN_multiple_clients_can_connect()
            {
                var listeningPort = _random.Next(10000, 50000);

                var serverNetworking = new Networking(NullLogger.Instance, (x) => { }, x => { }, (x, y) => { }, '\n');

                var serverThread = new Thread(() =>
                {
                    serverNetworking.WaitForClients(listeningPort, true);
                });

                serverThread.Start();

                while (!serverThread.IsAlive)
                {
                    Task.Delay(1000);
                }

                var connected = new Dictionary<string, List<string>>();

                var clientOne = new Networking(NullLogger.Instance, 
                    (x) => { connected.Add("1", new List<string>());},
                    x => { connected.Remove("1"); }, 
                    (x, y) => { connected["1"].Add(y);}, '\n');

                var clientTwo = new Networking(NullLogger.Instance,
                    (x) => { connected.Add("2", new List<string>()); },
                    x => { connected.Remove("2"); },
                    (x, y) => { connected["2"].Add(y); }, '\n');

                clientOne.Connect("", listeningPort);
                clientTwo.Connect("", listeningPort);

                Assert.AreEqual(2, connected.Count);

                clientOne.Disconnect();
                clientTwo.Disconnect();
                Assert.AreEqual(0, connected.Count);

                serverNetworking.StopWaitingForClients();
                serverNetworking.Disconnect();
            }
        }

        /// <summary>
        /// Send Message tests.
        /// </summary>
        [TestClass]
        public class Send : NetworkingTests
        {
            /// <summary>
            /// Tests sending, with multiple clients.
            /// </summary>
            [TestMethod]
            public void WHEN_WaitForClients_THEN_multiple_clients_can_connect()
            {
                var listeningPort = _random.Next(10000, 50000);

                var messagesRecieved = new List<string>();

                var serverNetworking = new Networking(NullLogger.Instance, (x) => { }, x => { }, (x, y) =>
                {
                    messagesRecieved.Add(y);
                }, '\n');

                var serverThread = new Thread(() =>
                {
                    serverNetworking.WaitForClients(listeningPort, true);
                });

                serverThread.Start();

                while (!serverThread.IsAlive)
                {
                    Thread.Sleep(1000);
                }

                var connected = new Dictionary<string, List<string>>();

                var clientOne = new Networking(NullLogger.Instance,
                    (x) => { connected.Add("1", new List<string>()); },
                    x => { connected.Remove("1"); },
                    (x, y) => { connected["1"].Add(y); }, '\n');

                var clientTwo = new Networking(NullLogger.Instance,
                    (x) => { connected.Add("2", new List<string>()); },
                    x => { connected.Remove("2"); },
                    (x, y) => { connected["2"].Add(y); }, '\n');

                clientOne.Connect("", listeningPort);
                clientTwo.Connect("", listeningPort);
                Assert.AreEqual(2, connected.Count);

                var clientOneBackground = new Thread(() =>
                {
                    clientOne.ClientAwaitMessagesAsync();
                });

                var clientTwoBackground = new Thread(() =>
                {
                    clientOne.ClientAwaitMessagesAsync();
                });

                clientOneBackground.Start();
                clientTwoBackground.Start();

                while (!clientOneBackground.IsAlive && !clientTwoBackground.IsAlive)
                {
                    Thread.Sleep(1000);
                }

                clientOne.Send("This is a message, I hope it makes it to the clients.");


                serverNetworking.StopWaitingForClients();
                clientOne.Disconnect();
                clientTwo.Disconnect();
                serverNetworking.Disconnect();
            }
        }
    }
}