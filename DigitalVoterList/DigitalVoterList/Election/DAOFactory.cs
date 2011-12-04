namespace DigitalVoterList.Election
{
    using System.Collections.Generic;

    /// <summary>
    /// A factory responsible of creating Data Access Objects.
    /// </summary>
    public static class DAOFactory
    {
        private static Dictionary<User, IDataAccessObject> daos = new Dictionary<User, IDataAccessObject>();

        public static IDataAccessObject getDAO(User u)
        {
            if (!daos.ContainsKey(u))
            {
                IDataAccessObject dao = new PermissionProxy(u, new DAOMySql());
                daos[u] = dao;
            }
            return daos[u];
        }

        private static IDataAccessObject _globalDAO = getDAO(new User());

        public static IDataAccessObject GlobalDAO 
        { 
            get
            {
                return _globalDAO;
            }
                set
            {
                if (value != null) _globalDAO = value;
            } 
        }
    }
}
