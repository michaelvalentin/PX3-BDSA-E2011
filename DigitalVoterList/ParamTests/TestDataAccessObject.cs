
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

            Person p2 = this._dao.LoadCitizen("1212534321");
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
        public void TestGetPermissions()
        {
            var permissions = this._dao.GetPermissions(VoterListApp.CurrentUser);
            Assert.That(permissions.Count == 22);

            var permissions2 = this._dao.GetPermissions(User.GetUser("slave", "asdf"));
            Assert.That(permissions2.Count == 0);

            var permissions3 = this._dao.GetPermissions(User.GetUser("elec", "hemmelighed"));
            Assert.That(permissions3.Count == 3);
        }

        /*[Test]
        public void TestSaveCitizen()
        {
            //Replace jens with morten
            var c = new Citizen(1, "1201561234");
            c.Name = "Morten Hyllekilde";
            c.SecurityQuestions.Add(new Quiz("Who are you?", "A test person"));
            this._dao.Save(c);

            MySqlCommand selectData = new MySqlCommand("SELECT COUNT(*) FROM quiz WHERE person_id=1", this._conn);
            var i = selectData.ExecuteScalar();
            Assert.That(i.ToString() == "1");

            selectData = new MySqlCommand("SELECT COUNT(*) FROM quiz WHERE name='Morten Hyllekilde'", this._conn);


            //Replace make new citizen
            var d = new Citizen(0, "1507814321");
            d.Name = "Secret Ninja";
            this._dao.Save(d);

            MySqlCommand selectData2 = new MySqlCommand("SELECT COUNT(*) FROM person", this._conn);
            var i2 = selectData2.ExecuteScalar();
            Assert.That(i2.ToString() == "5");
        }*/

        [Test]
        public void TestGetWorkplaces()
        {
            var workplaces = this._dao.GetWorkplaces(VoterListApp.CurrentUser);
            Assert.That(workplaces.Count == 1);

            var workplaces2 = this._dao.GetWorkplaces(User.GetUser("slave", "asdf"));
            Assert.That(workplaces2.Count == 2);
        }

        [Test]
        public void TestFindVoterCards()
        {
            int validVoterCards = _dao.FindVoterCards(new Dictionary<VoterCardSearchParam, object>()
                                                          {
                                                              {VoterCardSearchParam.Valid,true}
                                                          }, SearchMatching.Exact).Count;
            Assert.That(validVoterCards == 3, "Couldn't find all 3 valid voter cards");
            int withLetterHinKeyAndValid = _dao.FindVoterCards(new Dictionary<VoterCardSearchParam, object>()
                                                           {
                                                               {VoterCardSearchParam.Valid,true},
                                                               {VoterCardSearchParam.IdKey,"H"}
                                                           }, SearchMatching.Similair).Count;
            Assert.That(withLetterHinKeyAndValid == 2, "Couldn't find the 2 valid voter cards with letter H in idKey");
        }

        [Test]
        public void TestChangePassword()
        {
            User u = _dao.LoadUser(2);
            u.ChangePassword("passwordWorking1234");
            Assert.That(User.GetUser(u.Username, "passwordWorking1234").DbId == 2);
        }

        [Test]
        public void TestSetHasVoted()
        {
            //Eligible voter that has not voted -> Success
            Citizen c = _dao.LoadCitizen(1);
            c.SetHasVoted();
            MySqlCommand checkHasVoted = new MySqlCommand("SELECT id FROM person WHERE id=@id AND has_voted=1 AND eligible_to_vote=1", _conn);
            checkHasVoted.Prepare();
            checkHasVoted.Parameters.AddWithValue("@id", 1);
            object result = checkHasVoted.ExecuteScalar();
            Assert.That(result != null, "Has voted was not updated in database!");

            //Non-eligible voter...
            Citizen c2 = _dao.LoadCitizen(2);
            Assert.Throws(typeof(Exception), c2.SetHasVoted, "Uneligible voter can never vote!");
            checkHasVoted.Parameters.Clear();
            checkHasVoted.Parameters.AddWithValue("@id", 2);
            result = checkHasVoted.ExecuteScalar();
            Assert.That(result == null, "Uneligible voter can never vote!");

            //Has allready voted..
            Citizen c4 = _dao.LoadCitizen(4);
            Assert.Throws(typeof(Exception), c4.SetHasVoted, "Voters must never vote twice!");
            checkHasVoted.Parameters.Clear();
            checkHasVoted.Parameters.AddWithValue("@id", 4);
            result = checkHasVoted.ExecuteScalar();
            Assert.That(result != null, "Voter with id 4 should allready have voted");
        }

        [Test]
        public void TestLoadVoterCardById()
        {
            VoterCard votercard = this._dao.LoadVoterCard(5);
            Assert.That(votercard.IdKey.Equals("1HN8O9M9"));
            VoterCard votercard2 = this._dao.LoadVoterCard(1);
            Assert.That(votercard2.IdKey.Equals("HR5F4D7"));
        }

        [Test]
        public void TestLoadVoterCardByIdKey()
        {
            VoterCard votercard = this._dao.LoadVoterCard("5HU9KQY4");
            Assert.That(votercard.Id == 3);
            VoterCard votercard2 = this._dao.LoadVoterCard("HR5F4D7");
            Assert.That(votercard2.Id == 1);
        }

        [Test]
        public void TestDataTransform()
        {
            var dt = new DataTransformer();
            dt.TransformData();

            // 13 persons in row data, 4 persons in in current, one overlap (Jens) = 16 total
            var select = new MySqlCommand("SELECT COUNT(*) FROM person;", this._conn);
            object o = select.ExecuteScalar();
            Assert.That(Convert.ToInt32(o) == 16);

            MySqlCommand selectData = new MySqlCommand("SELECT COUNT(*) FROM person WHERE name='Mik Thomasen'", this._conn);
            var i = selectData.ExecuteScalar();
            Assert.That(i.ToString() == "1", "Mik Thomasen was not insert into data");
        }

        //List<VoterCard> Find(VoterCard voterCard);

        [Test]
        public void TestSavePerson()
        {
            /*this._dao.Save(new Person() { Name = "Helle Thorsen" });
            MySqlCommand selectData = new MySqlCommand("SELECT COUNT(*) FROM person WHERE name='Helle Thorsen'", this._conn);
            object o = selectData.ExecuteScalar();
            Assert.That(((int)o) == 1);*/
        }

        [Test]
        public void TestFindCitizen()
        {
            List<Citizen> result;
            result = _dao.FindCitizens(new Dictionary<CitizenSearchParam, object>
                                                         {
                                                             {CitizenSearchParam.Name,"Math"}
                                                         }, SearchMatching.Similair);
            Assert.That(result.Count == 1, "search with \"math\" did not find mathilde!");
            result = _dao.FindCitizens(new Dictionary<CitizenSearchParam, object>
                                                         {
                                                             {CitizenSearchParam.Name,"Math"}
                                                         }, SearchMatching.Exact);
            Assert.That(result.Count == 0, "Result where returned for exact search on \"math\"");
            result = _dao.FindCitizens(new Dictionary<CitizenSearchParam, object>
                                                         {
                                                             {CitizenSearchParam.Cpr,"2405901253"}
                                                         }, SearchMatching.Exact);
            Assert.That(result.Count == 1, "Jens Dahl Møllerhøj could not be found via CPR");
            Assert.That(result[0].Name.Equals("Jens Dahl Møllerhøj"), "Person with CPR 2405901253 was not Jens Dahl Møllerhøj");
            result = _dao.FindCitizens(new Dictionary<CitizenSearchParam, object>()
                                           {
                                               {CitizenSearchParam.EligibleToVote,true},
                                               {CitizenSearchParam.HasVoted,false}
                                           });
            Assert.That(result.Count == 2);
            result = _dao.FindCitizens(new Dictionary<CitizenSearchParam, object>()
                                           {
                                               {CitizenSearchParam.Address,"nørre"},
                                               {CitizenSearchParam.Name,"jens"}
                                           });
            Assert.That(result.Count == 1);
            Assert.That(result[0].Name.Equals("Jens Dahl Møllerhøj"), "Person was not Jens Dahl Møllerhøj");
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

        [Test]
        public void TestUpdateVoterCards()
        {
            // Valid votercard for citizen uneligible to vote is marked invalid
            var v = _dao.LoadVoterCard(2);
            Assert.That(v.Valid, "This votercard should be valid in the testdata");

            MySqlCommand select = new MySqlCommand("SELECT COUNT(*) FROM voter_card WHERE person_id=4 AND valid=1;", _conn);
            var i = Convert.ToInt32(select.ExecuteScalar());
            Assert.That(i == 0, "This citizen should not have any valid votercards in the test data");

            _dao.UpdateVoterCards(); //UPDATE!

            v = _dao.LoadVoterCard(2);
            Assert.That(!v.Valid, "Valid votercard should be marked invalid for citizen uneligible to vote");

            v = _dao.LoadVoterCard(1);
            Assert.That(v.Valid, "Valid votercard for valid citizen should remain unchanged");

            i = Convert.ToInt32(select.ExecuteScalar());
            Assert.That(i == 1, "Eligible citizen with no valid votercards should had one generated.");
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
