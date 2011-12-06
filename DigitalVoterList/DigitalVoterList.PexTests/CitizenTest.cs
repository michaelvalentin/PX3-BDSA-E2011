// <copyright file="CitizenTest.cs">Copyright ©  2011</copyright>

using System;
using DigitalVoterList.Election;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using DigitalVoterList.Utilities;

namespace DigitalVoterList.Election
{
    [TestClass]
    [PexClass(typeof(Citizen))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class CitizenTest
    {
        [PexMethod]
        public void VoterCardsSet([PexAssumeUnderTest]Citizen target, HashSet<VoterCard> value)
        {
            target.VoterCards = value;
            // TODO: add assertions to method CitizenTest.VoterCardsSet(Citizen, HashSet`1<VoterCard>)
        }
        [PexMethod]
        public void SecurityQuestionsSet([PexAssumeUnderTest]Citizen target, HashSet<Quiz> value)
        {
            target.SecurityQuestions = value;
            // TODO: add assertions to method CitizenTest.SecurityQuestionsSet(Citizen, HashSet`1<Quiz>)
        }
        [PexMethod]
        public HashSet<VoterCard> VoterCardsGet([PexAssumeUnderTest]Citizen target)
        {
            HashSet<VoterCard> result = target.VoterCards;
            return result;
            // TODO: add assertions to method CitizenTest.VoterCardsGet(Citizen)
        }
        [PexMethod]
        public HashSet<Quiz> SecurityQuestionsGet([PexAssumeUnderTest]Citizen target)
        {
            HashSet<Quiz> result = target.SecurityQuestions;
            return result;
            // TODO: add assertions to method CitizenTest.SecurityQuestionsGet(Citizen)
        }
        [PexMethod]
        public void SetHasVoted([PexAssumeUnderTest]Citizen target)
        {
            target.SetHasVoted();
            // TODO: add assertions to method CitizenTest.SetHasVoted(Citizen)
        }
        [PexMethod]
        public Citizen Constructor(int id, string cpr)
        {
            Citizen target = new Citizen(id, cpr);
            return target;
            // TODO: add assertions to method CitizenTest.Constructor(Int32, String)
        }
    }
}
