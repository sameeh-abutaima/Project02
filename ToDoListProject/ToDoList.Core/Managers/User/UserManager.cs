using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ToDoList.Common.Exceptions;
using ToDoList.Common.Helpers;
using ToDoList.ModelsDB.Models;
using ToDoList.ModelViews.ModelViews.User;

namespace ToDoList.Core.Managers.User
{
    public class UserManager : IUserManager
    {

        #region Dependency Injections
        private readonly ToDoListDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public UserManager(ToDoListDbContext context, IMapper mapper,IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }
        #endregion Dependency Injections 

        #region Public Methods
        public List<UserMV> GetUsers()
        {
            var users = _context.Users.ToList();
            return _mapper.Map<List<UserMV>>(users);
        }
        public List<UserMV> GetArchivedUsers()
        {
            _context.IgnoreFilter = true;
            var users = _context.Users.Where(usr=> usr.Archived);
            return _mapper.Map<List<UserMV>>(users);
        }
        public LoginResponseUserMV Login(LoginUserMV loginUserMV)
        {
            var user = _context.Users
                                   .FirstOrDefault(usr => usr.Email.ToLower()
                                                           .Equals(loginUserMV.Email.ToLower()));

            if (user == null || !VerifyHashPassword(loginUserMV.Password, user.Password))
            {
                throw new ServiceValidationException(300, "Invalid user name or password received");
            }

            var res = _mapper.Map<LoginResponseUserMV>(user);
            res.Token = $"Bearer {GenerateJwtToken(user)}";
            return res;
        }

        public SignUpResponseUserMv SignUp(SignUpUserMV signUpUserMV)
        {
            if(_context.Users.Any(usr=>usr.Email.ToLower().Equals(signUpUserMV.Email.ToLower())))
            {
                throw new ServiceValidationException("User Already Exist!! ");

            }
            var hashedPassword = HashPassword(signUpUserMV.Password);
            signUpUserMV.Password=hashedPassword;
            signUpUserMV.ConfirmPassword = hashedPassword;

            var newUser=_mapper.Map<ModelsDb.Models.User>(signUpUserMV);
            var user = _context.Users.Add(newUser).Entity;
            _context.SaveChanges();

            var res=_mapper.Map<SignUpResponseUserMv>(user);
            res.Token = $"Bearer {GenerateJwtToken(user)}";
            return res;
        }

        public UserMV UpdateProfile(UserMV currentUser, UserMV userMV)
        {
            var user = _context.Users
                                    .FirstOrDefault(a => a.Id == currentUser.Id)
                                    ?? throw new ServiceValidationException("User not found");

            var url = "";
            var fullName=$"{currentUser.FirstName}{currentUser.LastName}";
            if (!string.IsNullOrWhiteSpace(userMV.ImageString))
            {
                url = Helper.SaveImage(fullName,userMV.ImageString, "ProfileImages");
            }

            user.FirstName = userMV.FirstName;
            user.LastName = userMV.LastName;

            if (!string.IsNullOrWhiteSpace(url))
            {
                var baseURL = "https://localhost:44324/";
                user.ImageUrl = @$"{baseURL}/api/v1/user/retrieve?filename={url}";
            }

            _context.SaveChanges();
            return _mapper.Map<UserMV>(user);
        }

        public void DeleteUser(UserMV currentUser, int id)
        {
            if (currentUser.Id == id)
            {
                throw new ServiceValidationException("You have no access to delete your self");
            }

            var user = _context.Users
                                    .FirstOrDefault(a => a.Id == id)
                                    ?? throw new ServiceValidationException("User not found");
            user.Archived = true;
            _context.SaveChanges();
        }
        #endregion Public Methods

        #region Private Methods
        private static string HashPassword(string pswrd)
                                        => BCrypt.Net.BCrypt.HashPassword(pswrd);

        private string GenerateJwtToken(ModelsDb.Models.User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials=new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,$"{user.FirstName} {user.LastName}"),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim("Id",user.Id.ToString()),
                new Claim("FirstName",user.FirstName),
                new Claim("DateOfJoining",user.CreatedAt.ToString()),
            };
            var token = new JwtSecurityToken
                (
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires:DateTime.UtcNow.AddHours(1),
                signingCredentials:credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static bool VerifyHashPassword(string pswrd, string hashedPasword)
                                        => BCrypt.Net.BCrypt.Verify(pswrd, hashedPasword);

        

        #endregion Private Methods
    }
}
