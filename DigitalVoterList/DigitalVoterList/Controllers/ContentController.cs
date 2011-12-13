/*
 * Authors:
 * Team: PX3
 * Date: 12-12-2011
 */

using System.Collections.Generic;
using System.Windows.Controls;
using DigitalVoterList.Election;

namespace DigitalVoterList.Controllers
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// General behavior for system controls
    /// </summary>
    public partial class ContentController
    {
        protected HashSet<SystemAction> _neededPermissions;
        public UserControl View { get; set; }

        public ContentController()
            : base()
        {
            _neededPermissions = new HashSet<SystemAction>();
        }

        public bool HasPermissionToUse(User u)
        {
            Contract.Requires(u != null);
            return u.Permissions.IsSupersetOf(_neededPermissions);
        }
    }
}
