namespace Api.Domain.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public string Id { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Surname { get; set; }

        [Required]
        [StringLength(250)]
        public string Email { get; set; }

        [StringLength(500, MinimumLength = 3)]
        public string ImageUrl { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public string RoleId { get; set; }

        public Role Role { get; set; }
    }
}
