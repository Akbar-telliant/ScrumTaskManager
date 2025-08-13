using System.ComponentModel.DataAnnotations;

namespace ScrumMaster.Models
{
    public class Users
    {

        /// <summary>
        /// User roles for the application.
        /// </summary>
        public enum TUserRole
        {
            Admin,
            User
        }

        /// <summary>
        /// User Name.
        /// </summary>
        [Required]
        [StringLength(250)]
        public string UserName { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Role of the user.
        /// </summary>
        [Required]
        public TUserRole Role { get; set; } = TUserRole.User;

        public Users()
        {
          
        }

        public Users(string name, string email, TUserRole role)
        {
            UserName = name;
            Email = email;
            Role = role;
        }

    }
}
