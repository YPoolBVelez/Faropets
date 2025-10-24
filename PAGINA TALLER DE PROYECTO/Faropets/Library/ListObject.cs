using Faropets.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faropets.Library
{
    public class ListOject
    {
        public LibraryUsersRoles _usersRole;

        public IdentityError _identityError;
        public ApplicationDbContext _context;
        public IWebHostEnvironment _environment;

        public SignInManager<IdentityUser> _singInManager;
        public UserManager<IdentityUser> _userManager;
        public RoleManager<IdentityRole> _roleManager;

    }
}
