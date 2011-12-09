
using System;
using MySql.Data.MySqlClient;

namespace ParamTests
{
    using System.IO;

    using DigitalVoterList.Election;
    using NUnit.Framework;

    [TestFixture]
    public partial class TestDataAccessObject
    {
        private IDataAccessObject dao;
        private MySqlConnection conn;
        private string pathToTestfiles = "..\\..\\data\\";

        #region testsetup

        [TestFixtureSetUp]
        public void PrepareTesting()
        {
            /*
             * Generate control user
             * var u = new User(1);
            u.UserSalt = "lkaFDA62lio+3";
            u.HashPassword("12345");*/



            //Connect manually to the database
            conn = new MySqlConnection(
                "SERVER=localhost;" +
                "DATABASE=px3-test;" +
                "UID=root;" +
                "PASSWORD=abcd1234;");

            conn.Open();

            //Clean the database manually
            this.CleanUpAfterEachTest();

            //Login with program
            DAOFactory.ConnectionString = "SERVER=localhost;" +
                                            "DATABASE=px3-test;" +
                                            "UID=root;" +
                                            "PASSWORD=abcd1234;";

            //VoterListApp.CurrentUser = DAOFactory.CurrentUserDAO.LoadUser("jdmo", "12345");
            dao = DAOFactory.CurrentUserDAO;
        }

        [TestFixtureTearDown]
        public void EndTesting()
        {
            //Close manual connection
            conn.Close();
        }

        [SetUp]
        public void PrepareForEachTest()
        {
            MySqlCommand insertData = new MySqlCommand(this.readTextFile("DataInsertion.txt"), conn);
            object o = insertData.ExecuteScalar();
        }

        [TearDown]
        public void CleanUpAfterEachTest()
        {
            MySqlCommand insertData = new MySqlCommand(this.readTextFile("DataDeletion.txt"), conn);
            object o = insertData.ExecuteScalar();
        }

        #endregion

        #region helpermethods
        private string readTextFile(string file)
        {
            var textReader = new StreamReader(pathToTestfiles + file, System.Text.Encoding.UTF8);
            var text = textReader.ReadToEnd();
            textReader.Close();
            text = text.Replace(Environment.NewLine, "");
            return text;
        }

        #endregion

        #region tests

        [Test]
        public void TestLoadPersonById()
        {
            Person p = dao.LoadPerson(1);
            Assert.That(p.Name.Equals("Jens Dahl Møllerhøj"));
        }

        [Test]
        public void TestLoadPersonById()
        {
            Person p = dao.LoadUser(1);
            Assert.That(p.Name.Equals("Jens Dahl Møllerhøj"));
        }


        #endregion

    }
}
