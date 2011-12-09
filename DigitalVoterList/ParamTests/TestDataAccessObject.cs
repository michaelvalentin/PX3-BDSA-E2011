
using System;
using MySql.Data.MySqlClient;

namespace ParamTests
{
    using System.Collections.Generic;
    using System.IO;

    using DigitalVoterList;
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

            //Write to database
            this.PrepareForEachTest();

            //Login with program
            DAOFactory.ConnectionString = "SERVER=localhost;" +
                                            "DATABASE=px3-test;" +
                                            "UID=root;" +
                                            "PASSWORD=abcd1234;";

            //VoterListApp.CurrentUser = DAOFactory.CurrentUserDAO.LoadUser("jdmo", VoterListApp.CurrentUser.HashPassword("12345"));
            dao = DAOFactory.CurrentUserDAO;

            //Clean the database manually
            this.CleanUpAfterEachTest();
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
        public void TestLoadUserById()
        {
            Person p = dao.LoadUser(1);
            Assert.That(p.Name.Equals("Jens Dahl Møllerhøj"));
        }

        [Test]
        public void TestLoadUserByUsername()
        {
            Person p = dao.LoadUser("jdmo");
            Assert.That(p.Name.Equals("Jens Dahl Møllerhøj"));
        }

        [Test]
        public void TestValidateUser()
        {
            Assert.That(dao.ValidateUser("jdmo", "someHash")); //todo: insert hash
        }

        [Test]
        public void TestGetPermissions()
        {
            HashSet<SystemAction> permissions = dao.GetPermissions(VoterListApp.CurrentUser);

            Assert.That(permissions.Count == 22);
        }

        [Test]
        public void TestLoadVoterCardById()
        {
            VoterCard votercard = dao.LoadVoterCard(5);

            Assert.That(votercard.IdKey.Equals("1HN8O9M9"));
        }

        [Test]
        public void TestLoadVoterCardByIdKey()
        {
            VoterCard votercard = dao.LoadVoterCard("5HU9KQY4");

            Assert.That(votercard.Id == 3);
        }

        [Test]
        public void TestFindPersonByCpr()
        {
            var person = dao.Find(new Person() { Cpr = "2405901253" });

            Assert.That(person[0].Name.Equals("Jens Dahl Møllerhøj"));
        }

        [Test]
        public void TestFindUserByName()
        {
            var user = dao.Find(new User() { Name = "Jens Dahl Møllerhøj" });
        }

        //List<VoterCard> Find(VoterCard voterCard);

        //List<Citizen> FindElegibleVoters();

        [Test]
        void TestSavePerson()
        {
            dao.Save(new Person() { Name = "Helle Thorsen" });
            MySqlCommand selectData = new MySqlCommand("SELECT COUNT(*) FROM person WHERE name='Helle Thorsen'", conn);
            object o = selectData.ExecuteScalar();
            Assert.That(((int)o) == 1);
        }

        //void Save(User user);

        //void Save(VoterCard voterCard);

        //void SetHasVoted(Citizen citizen, string cprKey);

        //void SetHasVoted(Citizen citizen);


        /*void ChangePassword(User user, string newPasswordHash, string oldPasswordHash);

        void ChangePassword(User user, string newPasswordHash);

        void MarkUserInvalid(User user);

        void RestoreUser(User user);

        void MarkVoterCardInvalid(VoterCard voterCard);

        void UpdatePeople(Func<Person, RawPerson, Person> update);

        VotingVenue FindVotingVenue(Citizen citizen); */

        #endregion

    }
}
