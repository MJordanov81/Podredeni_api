namespace Api.Services.Implementations
{
    using Api.Data;
    using Api.Domain.Entities;
    using Api.Models.Account;
    using AutoMapper.QueryableExtensions;
    using Infrastructure;
    using Infrastructure.Constants;
    using Interfaces;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserService : IUserService
    {
        private readonly ApiDbContext db;

        public UserService(ApiDbContext db)
        {
            this.db = db;
        }

        public bool CheckRole(string userEmail, string roleName)
        {
            if (!this.db.Users.Any(u => u.Email == userEmail)) throw new ArgumentException(ErrorMessages.InvalidUserCredentials);

            return roleName == this.db.Users
                .Where(u => u.Email == userEmail)
                .Select(u => u.Role.Name)
                .FirstOrDefault();
        }

        public string GetUserId(string userEmail)
        {
            if (!this.db.Users.Any(u => u.Email == userEmail)) throw new ArgumentException(ErrorMessages.InvalidUserCredentials);

            return this.db.Users
                .Where(u => u.Email == userEmail)
                .Select(u => u.Id)
                .FirstOrDefault();
        }

        public async Task<UserTokenModel> Login(string email, string password)
        {
            User user = this.db.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                throw new ArgumentException(ErrorMessages.InvalidUserCredentials);
            }

            if (!PasswordHasher.VerifyHashedPassword(user.PasswordHash, password))
            {
                throw new ArgumentException(ErrorMessages.InvalidUserCredentials);
            }

            return this.db.Users
                .Where(u => u.Email == email)
                .ProjectTo<UserTokenModel>()
                .FirstOrDefault();
        }

        public async Task<UserTokenModel> Register(string email, string password)
        {
            if (this.db.Users.Any(u => u.Email == email))
            {
                throw new ArgumentException(ErrorMessages.EmailAlreadyRegistered);
            }

            User user = new User { Email = email };

            string hashedPass = PasswordHasher.HashPassword(password);

            user.PasswordHash = hashedPass;
      
            await this.db.Users.AddAsync(user);

            await this.db.SaveChangesAsync();

            return this.db.Users
                .Where(u => u.Email == email)
                .ProjectTo<UserTokenModel>()
                .FirstOrDefault();
        }

        public async Task SeedAdmin(string email, string password, string roleName)
        {
            if (!this.db.Users.Any(u => u.Email == email))
            {
                Role role = new Role
                {
                    Name = roleName
                };

                await this.db.Roles.AddAsync(role);

                await this.db.SaveChangesAsync();

                User admin = new User
                {
                    Email = email,
                    RoleId = role.Id                   
                };

                string hashedPass = PasswordHasher.HashPassword(password);

                admin.PasswordHash = hashedPass;

                await this.db.Users.AddAsync(admin);

                await this.db.SaveChangesAsync();
            }
        }
    }
}
