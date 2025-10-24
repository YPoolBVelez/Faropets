using Faropets.Areas.Users.Models;
using Faropets.Controllers;
using Faropets.Data;
using Faropets.Library;
using Faropets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faropets.Areas.Users.Controllers
{
    [Area("Users")]
    [Authorize]
    public class UsersController : Controller
    {
        private SignInManager<IdentityUser> _singInManager;
        private LibUser _user;
        private static DataPaginador<RegistroModelInput> models;

        public UsersController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> singInManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context)
        {
            _singInManager = singInManager;
            _user = new LibUser(userManager, singInManager, roleManager, context);

        }

        public IActionResult Users(int id, String filtrar, int registros)
        {
            if (_singInManager.IsSignedIn(User))
            {
                Object[] objects = new Object[3];
                var data = _user.getTUsuariosAsync(filtrar, 0);
                if (0 < data.Result.Count)
                {
                    var url = Request.Scheme + "://" + Request.Host.Value;
                    objects = new LibPaginador<RegistroModelInput>().paginador(data.Result,
                        id, registros, "Users", "Users", "Users", url);
                }
                else
                {
                    objects[0] = "No hay datos que mostrar";
                    objects[1] = "No hay datos que mostrar";
                    objects[2] = new List<RegistroModelInput>();

                }
                models = new DataPaginador<RegistroModelInput>
                {
                    List = (List<RegistroModelInput>)objects[2],
                    pagi_info = (String)objects[0],
                    pagi_navegacion = (String)objects[1],
                    Input = new RegistroModelInput(),
                };
                return View(models);

            }
            else
            {
                return Redirect("/");
            }
        }
        //public async Task<IActionResult> LogOut()
        //{
        //    await _singInManager.SignOutAsync();
        //    return RedirectToAction(nameof(HomeController.Index), "Home");
        //}
    }

} 


