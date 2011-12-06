using System.Collections.Generic;
using System.Windows.Controls;
using DigitalVoterList.Election;

namespace DigitalVoterList.Controllers
{

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
            return u.Permissions.IsSupersetOf(_neededPermissions);
        }
    }
}
