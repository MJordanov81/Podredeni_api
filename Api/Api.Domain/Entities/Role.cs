namespace Api.Domain.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Role
    {
        public string Id { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 2)]
        public string Name { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
