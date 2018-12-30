using System.Collections.Generic;

namespace GetShredded.ViewModel.Output.Users
{
    public class ChangeRoleModel
    {
        public ChangeRoleModel()
        {
            this.ApplicationRoles = new HashSet<string>();
        }

        public string Id { get; set; }

        public string Username { get; set; }

        public string Role { get; set; }

        public string UpdatedRole { get; set; }

        public ICollection<string> ApplicationRoles { get; set; }
    }
}
