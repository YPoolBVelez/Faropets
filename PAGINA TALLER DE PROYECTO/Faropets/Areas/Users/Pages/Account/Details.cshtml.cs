using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Faropets.Areas.Users.Models;
using Faropets.Data;
using Faropets.Library;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Faropets.Areas.Users.Pages.Account
{
    public class DetailsModel : PageModel
    {
        private SignInManager<IdentityUser> _signInManager;
        private LibUser _user;
        public DetailsModel(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _user = new LibUser(userManager, signInManager, roleManager, context);

        }
        public void OnGet(int id)
        {
            var data = _user.getTUsuariosAsync(null, id);
            if (0 < data.Result.Count )
            {
                Input = new InputModel
                {
                    DataUser = data.Result.ToList().Last(),
                };
            }
        }
        [BindProperty]
        public InputModel Input {get; set; }
        public class InputModel
        {
            public RegistroModelInput DataUser { get; set; }
        }
    }
}
