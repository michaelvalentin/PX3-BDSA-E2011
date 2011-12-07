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
        public static void Main()
        {

        }


        [TestMethod]
        public void TestMethod1()
        {

        }

        //[PexAssumeNotNull]

        [TestMethod]
        [PexMethod]
        public void TestDataTransformation()
        {
            VoterListApp.CurrentUser = DAOFactory.CurrentUserDAO.LoadUser("mier", "12345");

            var u = VoterListApp.CurrentUser;

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
