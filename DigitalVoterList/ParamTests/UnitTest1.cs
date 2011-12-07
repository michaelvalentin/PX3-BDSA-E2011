using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ParamTests
{
    using DigitalVoterList;
    using DigitalVoterList.Election;

    using Microsoft.Pex.Framework;

    [TestClass]
    public partial class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

        }

        //[PexAssumeNotNull]

        [TestMethod]
        [PexMethod]
        public void TestDataTransformation()
        {
            var u = new User();
            VoterListApp.CurrentUser = u;

            var t = new DataTransformer();
            t.TransformData(new ElectionEvent(DateTime.Today, "Test event"));
        }




        [PexMethod]
        public void ParameterizedTest(string data)
        {
            //Asserts
        }
    }
}
