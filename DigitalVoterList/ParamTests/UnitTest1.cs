﻿using System;
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
        public void TestFindPerson()
        {
            var p = DAOMySql.GetDao(VoterListApp.CurrentUser).Find(new Person(1));
            Contract.Ensures(p[0].Name == "Hans Hansen");
        }

        [TestMethod]
        public void TestLoadPerson()
        {
            var p = DAOMySql.GetDao(VoterListApp.CurrentUser).LoadPerson(1);
            Contract.Ensures(p.Name == "Hans Hansen");

        }

        //[PexAssumeNotNull]

        [TestMethod]
        [PexMethod]
        public void TestDataTransformation()
        {
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
