// <copyright file="DAOFactoryTest.cs">Copyright ©  2011</copyright>

using System;
using DigitalVoterList.Election;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DigitalVoterList.Election
{
    [TestClass]
    [PexClass(typeof(DAOFactory))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class DAOFactoryTest
    {
        [PexMethod]
        public IDataAccessObject getDAO(User u)
        {
            IDataAccessObject result = DAOFactory.getDAO(u);
            return result;
            // TODO: add assertions to method DAOFactoryTest.getDAO(User)
        }
        [PexMethod]
        public IDataAccessObject CurrentUserDAOGet()
        {
            IDataAccessObject result = DAOFactory.CurrentUserDAO;
            return result;
            // TODO: add assertions to method DAOFactoryTest.CurrentUserDAOGet()
        }
    }
}
