namespace DigitalVoterList.Election
{
    using System;

    /// <summary>
    /// Exception for indicating a permission violation. That is when a user tries to access a method, which he has no permission to use.
    /// </summary>
    public class PermissionException : Exception
    {
        private readonly Action _action;
        private readonly User _user;

        public PermissionException(string msg, Action action, User user)
            : base(msg)
        {
            _action = action;
            _user = user;
        }

        public PermissionException(Action action, User user)
            : this("You don't have permission to perform this action.", action, user)
        {
        }

        public Action Action { get; private set; }

        public User User { get; private set; }
    }
}
