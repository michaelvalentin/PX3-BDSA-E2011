
using System;
using System.Collections.Generic;
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
            Assert.That(p2.Name.Equals("Mathilde Roed Birk"));
        }

        [Test]
        public void TestLoadCitizenByCpr()
        {
            Person p = this._dao.LoadCitizen("2405901253");
            Assert.That(p.Name.Equals("Jens Dahl Møllerhøj"));

            Person p2 = this._dao.LoadCitizen("5097508703");
            Assert.That(p2.Name.Equals("Mathilde Roed Birk"));
        }

        [Test]
        public void TestLoadUserById()
        {
            var u = this._dao.LoadUser(1);
            Assert.That(u.Name.Equals("Jens Dahl Møllerhøj"));

            var u2 = this._dao.LoadUser(3);
            Assert.That(u2.Name.Equals("Mathilde Roed Birk"));
        }

        [Test]
        public void TestLoadUserByUsername()
        {
            Person p = this._dao.LoadUser("jdmo");
            Assert.That(p.Name.Equals("Jens Dahl Møllerhøj"));

            Person p2 = this._dao.LoadUser("elec");
            Assert.That(p2.Name.Equals("Mathilde Roed Birk"));
        }

        [Test]
        public void TestValidateUser()
        {
            Assert.That(this._dao.ValidateUser("jdmo", VoterListApp.CurrentUser.HashPassword("12345")));
            Assert.That(!this._dao.ValidateUser("jdmo2", VoterListApp.CurrentUser.HashPassword("12345")));
            Assert.That(!this._dao.ValidateUser("jdmo", VoterListApp.CurrentUser.HashPassword("1235")));
        }

        [Test]
        public void TestPreparedStatements()
        {
            var p = new MySqlCommand("INSERT INTO person (name, address) VALUES (@name, @address)");
            p.Connection = _conn;
            p.Prepare();
            p.Parameters.AddWithValue("@name", "Ronni Holm");
            p.Parameters.AddWithValue("@address", null);
            p.ExecuteScalar();
        }

        [Test]
        public void TestGetPermissions()
        {
            var permissions = this._dao.GetPermissions(VoterListApp.CurrentUser);
            Assert.That(permissions.Count == 22);

            var permissions2 = this._dao.GetPermissions(User.GetUser("slave", "asdf"));
            Assert.That(permissions2.Count == 0);

            var permissions3 = this._dao.GetPermissions(User.GetUser("elec", "hemmelighed"));
            Assert.That(permissions3.Count == 3);
        }

        [Test]
        public void TestSaveCitizen()
        {
            //Replace jens with morten
            var c = new Citizen(1, "1201561234");
            c.Name = "Morten Hyllekilde";
            this._dao.Save(c);

            MySqlCommand selectData = new MySqlCommand("SELECT COUNT(*) FROM person WHERE name='Morten Hyllekilde'", this._conn);
            var i = selectData.ExecuteScalar();
            Assert.That(i.ToString() == "1");

            //Replace make new citizen
            var d = new Citizen(0, "1507814321");
            d.Name = "Secret Ninja";
            this._dao.Save(d);

            MySqlCommand selectData2 = new MySqlCommand("SELECT COUNT(*) FROM person", this._conn);
            var i2 = selectData2.ExecuteScalar();
            Assert.That(i2.ToString() == "5");
        }

        [Test]
        public void TestGetWorkplaces()
        {
            var workplaces = this._dao.GetWorkplaces(VoterListApp.CurrentUser);
            Assert.That(workplaces.Count == 1);

            var workplaces2 = this._dao.GetWorkplaces(User.GetUser("slave", "asdf"));
            Assert.That(workplaces2.Count == 2);
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

        [Test]
        public void TestUpdatePeople()
        {
            var dt = new DataTransformer();
            dt.TransformData();
        }

        //List<VoterCard> Find(VoterCard voterCard);

        [Test]
        void TestSavePerson()
        {
            /*this._dao.Save(new Person() { Name = "Helle Thorsen" });
            MySqlCommand selectData = new MySqlCommand("SELECT COUNT(*) FROM person WHERE name='Helle Thorsen'", this._conn);
            object o = selectData.ExecuteScalar();
            Assert.That(((int)o) == 1);*/
        }

        [Test]
        public void TestFindCitizen()
        {
            List<Citizen> result = _dao.FindCitizens(new Dictionary<CitizenSearchParam, object>
                                                         {
                                                             {CitizenSearchParam.Name,"math"}
                                                         }, SearchMatching.Exact);
            /*Assert.That(result.Count == 1, "search with \"math\" did not find mathilde!");
            result = _dao.FindCitizens(new Dictionary<CitizenSearchParam, object>
                                                         {
                                                             {CitizenSearchParam.Name,"math"}
                                                         }, SearchMatching.Exact);
            Assert.That(result.Count == 0, "Result where returned for exact search on \"math\"");*/
            /*List<Citizen> result = _dao.FindCitizens(new Dictionary<CitizenSearchParam, object>
                                                         {
                                                             {CitizenSearchParam.Cpr,"2405901253"}
                                                         }, SearchMatching.Exact);
            Assert.That(result.Count == 1, "Jens Dahl Møllerhøj could not be found via CPR");
            Assert.That(result[0].Name.Equals("Jens Dahl Møllerhøj"), "Person with CPR 2405901253 was not Jens Dahl Møllerhøj");*/
        }

        //void Save(User user);

        [Test]
        public void TestSaveNewVoterCard()
        {
            Citizen c = _dao.LoadCitizen(1);
            VoterCard v = new VoterCard();
            v.Citizen = c;
            v.IdKey = "AXP956R3";
            v.Valid = true;
            _dao.Save(v);
            MySqlCommand select = new MySqlCommand("SELECT * FROM voter_card WHERE id_key=@idKey", _conn);
            select.Prepare();
            select.Parameters.AddWithValue("@idKey", v.IdKey);
            MySqlDataReader rdr = select.ExecuteReader();
            Assert.That(rdr.Read(), "Voter-card should exist in database");
            Assert.That(rdr.GetInt32("person_id") == 1, "Person id should be correct in DB");
            Assert.That(rdr.GetInt32("valid") == 1, "Valid status should be correct in DB");
            rdr.Close();
        }

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
