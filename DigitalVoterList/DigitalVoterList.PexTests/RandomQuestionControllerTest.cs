// <copyright file="RandomQuestionControllerTest.cs">Copyright ©  2011</copyright>

using System;
using DigitalVoterList.Controllers;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DigitalVoterList.Views;
using DigitalVoterList.Election;

namespace DigitalVoterList.Controllers
{
    [TestClass]
    [PexClass(typeof(RandomQuestionController))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class RandomQuestionControllerTest
    {
        [PexMethod]
        public void RequestQuestion(
            [PexAssumeUnderTest]RandomQuestionController target,
            object caller,
            EventArgs e
        )
        {
            target.RequestQuestion(caller, e);
            // TODO: add assertions to method RandomQuestionControllerTest.RequestQuestion(RandomQuestionController, Object, EventArgs)
        }
        [PexMethod]
        public RandomQuestionController Constructor(SecurityQuesitonView view, Citizen voter)
        {
            RandomQuestionController target = new RandomQuestionController(view, voter);
            return target;
            // TODO: add assertions to method RandomQuestionControllerTest.Constructor(SecurityQuesitonView, Citizen)
        }
    }
}
