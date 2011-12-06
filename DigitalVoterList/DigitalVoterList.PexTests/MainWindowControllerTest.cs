// <copyright file="MainWindowControllerTest.cs">Copyright ©  2011</copyright>

using System;
using DigitalVoterList.Controllers;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DigitalVoterList.Views;

namespace DigitalVoterList.Controllers
{
    [TestClass]
    [PexClass(typeof(MainWindowController))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class MainWindowControllerTest
    {
        [PexMethod]
        public MainWindowController Constructor(MainWindow view)
        {
            MainWindowController target = new MainWindowController(view);
            return target;
            // TODO: add assertions to method MainWindowControllerTest.Constructor(MainWindow)
        }
    }
}
