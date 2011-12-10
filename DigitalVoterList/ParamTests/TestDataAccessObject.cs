
using System;
using MySql.Data.MySqlClient;

namespace ParamTests
{
    using System.IO;

    using DigitalVoterList;
    using DigitalVoterList.Election;
    using NUnit.Framework;

    [TestFixture]
    public partial class TestDataAccessObject
    {
        private IDataAccessObject _dao;
        private MySqlConnection _conn;
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
            this._conn = new MySqlConnection(
                "SERVER=localhost;" +
                "DATABASE=px3-test;" +
                "UID=root;" +
                "PASSWORD=abcd1234;");

            this._conn.Open();

            //Clean the database manually
            this.CleanUpAfterEachTest();

            //Write to database
            this.PrepareForEachTest();

            //Login
            DAOFactory.ConnectionString = "SERVER=localhost;" +
                                            "DATABASE=px3-test;" +
                                            "UID=root;" +
                                            "PASSWORD=abcd1234;";

            VoterListApp.CurrentUser = User.GetUser("jdmo", "12345");
            _dao = DAOFactory.getDAO(VoterListApp.CurrentUser);

            //Clean the database manually
            this.CleanUpAfterEachTest();
        }

        [TestFixtureTearDown]
        public void EndTesting()
        {
            //Close manual connection
            this._conn.Close();
        }

        [SetUp]
        public void PrepareForEachTest()
        {
            MySqlCommand insertData = new MySqlCommand(this.readTextFile("DataInsertion.txt"), this._conn);
            object o = insertData.ExecuteScalar();
        }

        [TearDown]
        public void CleanUpAfterEachTest()
        {
            MySqlCommand insertData = new MySqlCommand(this.readTextFile("DataDeletion.txt"), this._conn);
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
        public void TestLoadCitizenById()
        {
            Person p = this._dao.LoadCitizen(1);
            Assert.That(p.Name.Equals("Jens Dahl Møllerhøj"));

            Person p2 = this._dao.LoadCitizen(3);
            Assert.That(p2.Name.Equals("Mathilde Roed Birk")); // This doesn't work because the loadpersons function loads votingvenue wrong.
        }

        [Test]
        public void TestLoadCitizenByCpr()
        {
            Person p = this._dao.LoadCitizen("2405901253");
            Assert.That(p.Name.Equals("Jens Dahl Møllerhøj"));
        }

        [Test]
        public void TestLoadUserById()
        {
            Person p = this._dao.LoadUser(1);
            Assert.That(p.Name.Equals("Jens Dahl Møllerhøj"));
        }

        [Test]
        public void TestLoadUserByUsername()
        {
            Person p = this._dao.LoadUser("jdmo");
            Assert.That(p.Name.Equals("Jens Dahl Møllerhøj"));
        }

        [Test]
        public void TestValidateUser()
        {
            Assert.That(this._dao.ValidateUser("jdmo", VoterListApp.CurrentUser.HashPassword("12345")));
            Assert.That(!this._dao.ValidateUser("jdmo2", VoterListApp.CurrentUser.HashPassword("12345")));
            Assert.That(!this._dao.ValidateUser("jdmo", VoterListApp.CurrentUser.HashPassword("1235")));
        }

        [Test]
        public void TestGetPermissions()
        {
            var permissions = this._dao.GetPermissions(VoterListApp.CurrentUser);

            Assert.That(permissions.Count == 22);
        }

        [Test]
        public void TestGetWorkplaces()
        {
            var workplaces = this._dao.GetWorkplaces(VoterListApp.CurrentUser);

            Assert.That(workplaces.Count == 1);
        }

        [Test]
        public void TestLoadVoterCardById()
        {
            VoterCard votercard = this._dao.LoadVoterCard(5);

            Assert.That(votercard.IdKey.Equals("1HN8O9M9"));
        }

        [Test]
        public void TestLoadVoterCardByIdKey()
        {
            VoterCard votercard = this._dao.LoadVoterCard("5HU9KQY4");

            Assert.That(votercard.Id == 3);
        }

        /*[Test]
        public void TestFindPersonByCpr()
        {
            var person = this._dao.Find(new Person() { Cpr = "2405901253" });

            Assert.That(person[0].Name.Equals("Jens Dahl Møllerhøj"));
        }

        [Test]
        public void TestFindUserByName()
        {
            var user = this._dao.Find(new User() { Name = "Jens Dahl Møllerhøj" });
        }

        //List<VoterCard> Find(VoterCard voterCard);

        //List<Citizen> FindElegibleVoters();

        [Test]
        void TestSavePerson()
        {
            this._dao.Save(new Person() { Name = "Helle Thorsen" });
            MySqlCommand selectData = new MySqlCommand("SELECT COUNT(*) FROM person WHERE name='Helle Thorsen'", this._conn);
            object o = selectData.ExecuteScalar();
            Assert.That(((int)o) == 1);
        }

         * */
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
