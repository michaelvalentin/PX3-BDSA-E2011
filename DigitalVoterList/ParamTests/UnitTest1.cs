using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ParamTests
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using DigitalVoterList;
    using DigitalVoterList.Election;

    using Microsoft.Pex.Framework;

    [TestClass]
    public partial class UnitTest1
    {
        private HashSet<int> _personIds;

        [TestInitialize()]
        public void Prepare()
        {
            _personIds = new HashSet<int> { 1, 2, 3, 6 };
            VoterListApp.CurrentUser = DAOFactory.CurrentUserDAO.LoadUser("mier", "12345");
        }


        [TestMethod]
        public void TestMethod1()
        {
            var x = _personIds;
            var p = x.Contains(1);
        }

        [TestMethod]
        public void TestLoadUser()
        {
            var u = DAOMySql.GetDao(VoterListApp.CurrentUser).LoadUser(3);
            Contract.Ensures(u.Name == "Michael");
        }

        [TestMethod]
        public void TestLoadPerson()
        {
            var u = DAOMySql.GetDao(VoterListApp.CurrentUser).LoadPerson(3);
            Contract.Ensures(u.Name == "Frederik Paulsen");
        }


        [TestMethod]
        public void TestFindPerson()
        {
            var p = DAOMySql.GetDao(VoterListApp.CurrentUser).Find(new Person(1));
            Contract.Ensures(p[0].Name == "Hans Hansen");
        }

        [TestMethod]
        public void TestSavePerson()
        {
            var person = new Person();
            person.Name = "Jens Dahl Møllerhøj";
            person.Address = "Nørre Alle 75";

            DAOMySql.GetDao(VoterListApp.CurrentUser).Save(person);
            Contract.Ensures(DAOMySql.GetDao(VoterListApp.CurrentUser).Find(person).Equals(person));
        }

        //[PexAssumeNotNull]

        [TestMethod]
        [PexMethod]
        public void TestDataTransformation()
        {
            var t = new DataTransformer();
            var e = new ElectionEvent(DateTime.Today, "Test event");
            t.TransformData(e);
        }





        [PexMethod]
        public void ParameterizedTest(string data)
        {
            //Asserts
        }
    }
}
