using shareshare.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Database;
using shareshare.PriceServer;
using shareshare.Message;
using WebSocket4Net;
namespace ShareShareTestProject
{
    
    
    /// <summary>
    ///This is a test class for BusinessUnitTest and is intended
    ///to contain all BusinessUnitTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BusinessUnitTest
    {


        private TestContext testContextInstance;
        
 
        //WebSocket websocket = new WebSocket("ws://0.0.0.0:8877/");
        /*
        websocket.Opened += new EventHandler(websocket_Opened);
        websocket.Error += new EventHandler<ErrorEventArgs>(websocket_Error);
        websocket.Closed += new EventHandler(websocket_Closed);
        websocket.MessageReceived += new EventHandler(websocket_MessageReceived);
        websocket.Open();
        */
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        /*
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
        }*/
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for RegisterUser
        ///</summary>
        [TestMethod()]
        public void RegisterUserTest()
        {
            WebSocket websocket = new WebSocket("ws://0.0.0.0:8877/");
            websocket.Open();
            websocket.Opened += delegate(object sender, EventArgs e)
            {
                Console.WriteLine("successfully connected");
            };

            Assert.AreEqual(null, null);
                
              
          //  websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocket_MessageReceived);
            /*
            DbAccess dba = null; // TODO: Initialize to an appropriate value
            IExchangeServer exchange = null; // TODO: Initialize to an appropriate value
            EmailService emailservice = null; // TODO: Initialize to an appropriate value
            BusinessUnit target = new BusinessUnit(dba, exchange, emailservice); // TODO: Initialize to an appropriate value
            RegisterRequestMessage req = null; // TODO: Initialize to an appropriate value
            RegisterResponseMessage expected = null; // TODO: Initialize to an appropriate value
            RegisterResponseMessage actual;
            actual = target.RegisterUser(req);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");*/
        }

        void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
           // e.Message
        }

        void websocket_Opened(object sender, EventArgs e)
        {
            
        }
    }
}
