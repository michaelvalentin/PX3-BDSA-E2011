// <copyright file="QuizTest.cs">Copyright ©  2011</copyright>

using System;
using DigitalVoterList.Utilities;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DigitalVoterList.Utilities
{
    [TestClass]
    [PexClass(typeof(Quiz))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class QuizTest
    {
        [PexMethod]
        public Quiz Constructor(string question, string answer)
        {
            Quiz target = new Quiz(question, answer);
            return target;
            // TODO: add assertions to method QuizTest.Constructor(String, String)
        }
    }
}
