﻿namespace DigitalVoterList.Election
{
    using System;

    /// <summary>
    /// Exception for indicating a permission violation. That is when a user tries to access a method, which he has no permission to use.
    /// </summary>
    public class PermissionException : Exception
    {
        private readonly SystemAction _systemAction;
        private readonly User _user;

        public PermissionException(string msg, SystemAction systemAction, User user)
            : base(msg)
        {
            _systemAction = systemAction;
            _user = user;
        }

        public PermissionException(SystemAction systemAction, User user)
            : this("You don't have permission to perform this SystemAction.", systemAction, user)
        {
        }

        public SystemAction SystemAction { get; private set; }

        public User User { get; private set; }
    }
}
