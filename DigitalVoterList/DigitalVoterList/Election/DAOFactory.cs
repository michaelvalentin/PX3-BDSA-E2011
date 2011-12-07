namespace DigitalVoterList.Election
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A factory responsible of creating Data Access Objects.
    /// </summary>
    public static class DAOFactory
    {
        private static Dictionary<User, IDataAccessObject> daos = new Dictionary<User, IDataAccessObject>();

        public static IDataAccessObject getDAO(User u)
        {
            Contract.Ensures(Contract.Result<IDataAccessObject>() != null);

            Contract.Ensures(
                (Contract.Result<IDataAccessObject>() == null && !daos.ContainsKey(u)) ||
                (Contract.Result<IDataAccessObject>() != null && daos.ContainsKey(u))
            );

            if (!daos.ContainsKey(u))
            {
                IDataAccessObject dao = DAOMySql.GetDao(u);
                daos[u] = dao;
            }
            return daos[u];
        }

        public static IDataAccessObject CurrentUserDAO
        {
            get { return getDAO(VoterListApp.CurrentUser); }
        }
    }
}
