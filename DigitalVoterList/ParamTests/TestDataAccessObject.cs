
using System;
using MySql.Data.MySqlClient;

namespace ParamTests
{

    using DigitalVoterList.Election;
    using NUnit.Framework;


    [TestFixture]
    public partial class TestDataAccessObject
    {
        private IDataAccessObject dao;
        private MySqlConnection conn;
        private MySqlConnection conn2;

        [TestFixtureSetUp]
        public void PrepareClass()
        {
            //VoterListApp.CurrentUser = DAOFactory.CurrentUserDAO.LoadUser("mier", "12345");
            dao = DAOFactory.CurrentUserDAO;
            conn = new MySqlConnection("SERVER=localhost;" +
                "DATABASE=px3;" +
                "UID=root;" +
                "PASSWORD=abcd1234;");
            conn2 = new MySqlConnection("SERVER=localhost;" +
                "DATABASE=px3;" +
                "UID=root;" +
                "PASSWORD=abcd1234;");
            conn.Open();
            conn2.Open();
        }

        [TestFixtureTearDown]
        public void EndTesting()
        {
            conn.Close();
        }

        [SetUp]
        public void PrepareForTest()
        {

        }

        [TearDown]
        public void CleanUpAfterTest()
        {

        }

        [Test]
        public void TestLoadPersonById()
        {
            MySqlCommand insertPerson = new MySqlCommand("INSERT INTO person (name) VALUES ('Hans Peter'); SELECT LAST_INSERT_ID();", conn);
            object o = insertPerson.ExecuteScalar();
            int id = Convert.ToInt32(o);
            Person p = dao.LoadPerson(id);
            Assert.That(p.Name.Equals("Hans Peter"));
            MySqlCommand deletePerson = new MySqlCommand("DELETE FROM person WHERE id = " + id, conn);
            deletePerson.ExecuteNonQuery();
        }

        [Test]
        public void ReproduceCommitProblem()
        {
            MySqlTransaction trans = conn.BeginTransaction();
            MySqlCommand loadPersons = new MySqlCommand("SELECT * FROM person", conn, trans);
            MySqlDataReader rdr = loadPersons.ExecuteReader();
            rdr.Read();
            int i = rdr.GetInt32("id");
            rdr.Close();
            loadPersons = new MySqlCommand("SELECT * FROM person", conn, trans);
            rdr = loadPersons.ExecuteReader();
            rdr.Read();
            i = rdr.GetInt32("id");
            rdr.Close();
            loadPersons = new MySqlCommand("SELECT * FROM person", conn, trans);
            rdr = loadPersons.ExecuteReader();
            rdr.Read();
            i = rdr.GetInt32("id");
            rdr.Close();
            trans.Commit();
        }

        /*[Test]
        public void TestConcurrency()
        {
            new MySqlCommand("START TRANSACTION; SET autocommit=0; SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;", conn).ExecuteNonQuery();
            new MySqlCommand("START TRANSACTION; SET autocommit=0; SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;", conn2).ExecuteNonQuery();
            string value = (string)new MySqlCommand("SELECT name FROM person WHERE name LIKE '%Jonas%'", conn2).ExecuteScalar();
            int i = new MySqlCommand("UPDATE person SET name = 'Jonas Schmidt' WHERE name LIKE '%Jonas%'", conn).ExecuteNonQuery();
            string value2 = (string)new MySqlCommand("SELECT name FROM person WHERE name LIKE '%Jonas%'", conn2).ExecuteScalar();
            new MySqlCommand("COMMIT;", conn).ExecuteNonQuery();
            int j = new MySqlCommand("UPDATE person SET name = 'Jonas Jensen2' WHERE name LIKE '%Jonas%'", conn2).ExecuteNonQuery();
            new MySqlCommand("COMMIT;", conn2).ExecuteNonQuery();
        }*/

        /*private HashSet<int> _personIds;

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
        }*/
    }
}
