// <copyright file="DAOPermissionProxyTest.cs">Copyright ©  2011</copyright>

using System;
using DigitalVoterList.Election;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DigitalVoterList.Election
{
    [TestClass]
    [PexClass(typeof(DAOPermissionProxy))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class DAOPermissionProxyTest
    {
        [PexMethod]
        public HashSet<VotingVenue> Workplaces([PexAssumeUnderTest]DAOPermissionProxy target, User u)
        {
            HashSet<VotingVenue> result = target.Workplaces(u);
            return result;
            // TODO: add assertions to method DAOPermissionProxyTest.Workplaces(DAOPermissionProxy, User)
        }
        [PexMethod]
        public bool ValidateUser(
            [PexAssumeUnderTest]DAOPermissionProxy target,
            string username,
            string passwordHash
        )
        {
            bool result = target.ValidateUser(username, passwordHash);
            return result;
            // TODO: add assertions to method DAOPermissionProxyTest.ValidateUser(DAOPermissionProxy, String, String)
        }
        [PexMethod]
        public void UpdatePeople([PexAssumeUnderTest]DAOPermissionProxy target, Func<Person, RawPerson, Person> update)
        {
            target.UpdatePeople(update);
            // TODO: add assertions to method DAOPermissionProxyTest.UpdatePeople(DAOPermissionProxy, Func`3<Person,RawPerson,Person>)
        }
        [PexMethod]
        public void SetHasVoted01([PexAssumeUnderTest]DAOPermissionProxy target, Citizen citizen)
        {
            target.SetHasVoted(citizen);
            // TODO: add assertions to method DAOPermissionProxyTest.SetHasVoted01(DAOPermissionProxy, Citizen)
        }
        [PexMethod]
        public void SetHasVoted(
            [PexAssumeUnderTest]DAOPermissionProxy target,
            Citizen citizen,
            int cprKey
        )
        {
            target.SetHasVoted(citizen, cprKey);
            // TODO: add assertions to method DAOPermissionProxyTest.SetHasVoted(DAOPermissionProxy, Citizen, Int32)
        }
        [PexMethod]
        public void Save02([PexAssumeUnderTest]DAOPermissionProxy target, VoterCard voterCard)
        {
            target.Save(voterCard);
            // TODO: add assertions to method DAOPermissionProxyTest.Save02(DAOPermissionProxy, VoterCard)
        }
        [PexMethod]
        public void Save01([PexAssumeUnderTest]DAOPermissionProxy target, User user)
        {
            target.Save(user);
            // TODO: add assertions to method DAOPermissionProxyTest.Save01(DAOPermissionProxy, User)
        }
        [PexMethod]
        public void Save([PexAssumeUnderTest]DAOPermissionProxy target, Person person)
        {
            target.Save(person);
            // TODO: add assertions to method DAOPermissionProxyTest.Save(DAOPermissionProxy, Person)
        }
        [PexMethod]
        public void RestoreUser([PexAssumeUnderTest]DAOPermissionProxy target, User user)
        {
            target.RestoreUser(user);
            // TODO: add assertions to method DAOPermissionProxyTest.RestoreUser(DAOPermissionProxy, User)
        }
        [PexMethod]
        public void MarkVoterCardInvalid([PexAssumeUnderTest]DAOPermissionProxy target, VoterCard voterCard)
        {
            target.MarkVoterCardInvalid(voterCard);
            // TODO: add assertions to method DAOPermissionProxyTest.MarkVoterCardInvalid(DAOPermissionProxy, VoterCard)
        }
        [PexMethod]
        public void MarkUserInvalid([PexAssumeUnderTest]DAOPermissionProxy target, User user)
        {
            target.MarkUserInvalid(user);
            // TODO: add assertions to method DAOPermissionProxyTest.MarkUserInvalid(DAOPermissionProxy, User)
        }
        [PexMethod]
        public void MarkPeopleNotInRawDataUneligibleToVote([PexAssumeUnderTest]DAOPermissionProxy target)
        {
            target.MarkPeopleNotInRawDataUneligibleToVote();
            // TODO: add assertions to method DAOPermissionProxyTest.MarkPeopleNotInRawDataUneligibleToVote(DAOPermissionProxy)
        }
        [PexMethod]
        public VoterCard LoadVoterCard01([PexAssumeUnderTest]DAOPermissionProxy target, string idKey)
        {
            VoterCard result = target.LoadVoterCard(idKey);
            return result;
            // TODO: add assertions to method DAOPermissionProxyTest.LoadVoterCard01(DAOPermissionProxy, String)
        }
        [PexMethod]
        public VoterCard LoadVoterCard([PexAssumeUnderTest]DAOPermissionProxy target, int id)
        {
            VoterCard result = target.LoadVoterCard(id);
            return result;
            // TODO: add assertions to method DAOPermissionProxyTest.LoadVoterCard(DAOPermissionProxy, Int32)
        }
        [PexMethod]
        public User LoadUser02([PexAssumeUnderTest]DAOPermissionProxy target, int id)
        {
            User result = target.LoadUser(id);
            return result;
            // TODO: add assertions to method DAOPermissionProxyTest.LoadUser02(DAOPermissionProxy, Int32)
        }
        [PexMethod]
        public User LoadUser01(
            [PexAssumeUnderTest]DAOPermissionProxy target,
            string username,
            string password
        )
        {
            User result = target.LoadUser(username, password);
            return result;
            // TODO: add assertions to method DAOPermissionProxyTest.LoadUser01(DAOPermissionProxy, String, String)
        }
        [PexMethod]
        public User LoadUser([PexAssumeUnderTest]DAOPermissionProxy target, string username)
        {
            User result = target.LoadUser(username);
            return result;
            // TODO: add assertions to method DAOPermissionProxyTest.LoadUser(DAOPermissionProxy, String)
        }
        [PexMethod]
        public IEnumerable<RawPerson> LoadRawPeople([PexAssumeUnderTest]DAOPermissionProxy target)
        {
            IEnumerable<RawPerson> result = target.LoadRawPeople();
            return result;
            // TODO: add assertions to method DAOPermissionProxyTest.LoadRawPeople(DAOPermissionProxy)
        }
        [PexMethod]
        public Person LoadPerson([PexAssumeUnderTest]DAOPermissionProxy target, int id)
        {
            Person result = target.LoadPerson(id);
            return result;
            // TODO: add assertions to method DAOPermissionProxyTest.LoadPerson(DAOPermissionProxy, Int32)
        }
        [PexMethod]
        public HashSet<VotingVenue> GetWorkplaces([PexAssumeUnderTest]DAOPermissionProxy target, User u)
        {
            HashSet<VotingVenue> result = target.GetWorkplaces(u);
            return result;
            // TODO: add assertions to method DAOPermissionProxyTest.GetWorkplaces(DAOPermissionProxy, User)
        }
        [PexMethod]
        public HashSet<SystemAction> GetPermissions([PexAssumeUnderTest]DAOPermissionProxy target, User u)
        {
            HashSet<SystemAction> result = target.GetPermissions(u);
            return result;
            // TODO: add assertions to method DAOPermissionProxyTest.GetPermissions(DAOPermissionProxy, User)
        }
        [PexMethod]
        public VotingVenue FindVotingVenue([PexAssumeUnderTest]DAOPermissionProxy target, Citizen citizen)
        {
            VotingVenue result = target.FindVotingVenue(citizen);
            return result;
            // TODO: add assertions to method DAOPermissionProxyTest.FindVotingVenue(DAOPermissionProxy, Citizen)
        }
        [PexMethod]
        public List<Citizen> FindElegibleVoters([PexAssumeUnderTest]DAOPermissionProxy target)
        {
            List<Citizen> result = target.FindElegibleVoters();
            return result;
            // TODO: add assertions to method DAOPermissionProxyTest.FindElegibleVoters(DAOPermissionProxy)
        }
        [PexMethod]
        public List<VoterCard> Find02([PexAssumeUnderTest]DAOPermissionProxy target, VoterCard voterCard)
        {
            List<VoterCard> result = target.Find(voterCard);
            return result;
            // TODO: add assertions to method DAOPermissionProxyTest.Find02(DAOPermissionProxy, VoterCard)
        }
        [PexMethod]
        public List<User> Find01([PexAssumeUnderTest]DAOPermissionProxy target, User user)
        {
            List<User> result = target.Find(user);
            return result;
            // TODO: add assertions to method DAOPermissionProxyTest.Find01(DAOPermissionProxy, User)
        }
        [PexMethod]
        public List<Person> Find([PexAssumeUnderTest]DAOPermissionProxy target, Person person)
        {
            List<Person> result = target.Find(person);
            return result;
            // TODO: add assertions to method DAOPermissionProxyTest.Find(DAOPermissionProxy, Person)
        }
        [PexMethod]
        public DAOPermissionProxy Constructor(User u, IDataAccessObject dao)
        {
            DAOPermissionProxy target = new DAOPermissionProxy(u, dao);
            return target;
            // TODO: add assertions to method DAOPermissionProxyTest.Constructor(User, IDataAccessObject)
        }
        [PexMethod]
        public void ChangePassword01(
            [PexAssumeUnderTest]DAOPermissionProxy target,
            User user,
            string newPasswordHash
        )
        {
            target.ChangePassword(user, newPasswordHash);
            // TODO: add assertions to method DAOPermissionProxyTest.ChangePassword01(DAOPermissionProxy, User, String)
        }
        [PexMethod]
        public void ChangePassword(
            [PexAssumeUnderTest]DAOPermissionProxy target,
            User user,
            string newPasswordHash,
            string oldPasswordHash
        )
        {
            target.ChangePassword(user, newPasswordHash, oldPasswordHash);
            // TODO: add assertions to method DAOPermissionProxyTest.ChangePassword(DAOPermissionProxy, User, String, String)
        }
    }
}
