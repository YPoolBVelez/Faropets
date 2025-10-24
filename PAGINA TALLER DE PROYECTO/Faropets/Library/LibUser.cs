using Faropets.Areas.Users.Models;
using Faropets.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faropets.Library
{
    public class LibUser : ListOject
    {

        public LibUser(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _singInManager = signInManager;
            _roleManager = roleManager;
            _usersRole = new LibraryUsersRoles();

        }
        public async Task<List<RegistroModelInput>> getTUsuariosAsync(string valor, int id)
        {
            List<TUsers> listUsers;
            List<SelectListItem> _listRoles;
            List<RegistroModelInput> userList = new List<RegistroModelInput>();
            if (valor == null && id.Equals(0))
            {
                listUsers = _context.TUsers.ToList();

            }else
            {
                if (id.Equals(0))
                {
                    listUsers = _context.TUsers.Where(u => u.Nombre.StartsWith(valor) || u.Apellido.StartsWith(valor) 
                    || u.Email.StartsWith(valor)).ToList();
                }
                else
                {
                    listUsers = _context.TUsers.Where(u => u.ID.Equals(id)).ToList();
                }
            }
            if (!listUsers.Count.Equals(0))
            {
                foreach (var item in listUsers)
                {
                    _listRoles = await _usersRole.getRole(_userManager, _roleManager, item.IdUser);
                    var user = _context.Users.Where(u => u.Id.Equals(item.IdUser)).ToList().Last();
                    userList.Add(new RegistroModelInput { 
                    
                        Id = item.ID,
                        ID = item.IdUser,
                        Nombre = item.Nombre,
                        Apellido = item.Apellido,
                        Email = item.Email,
                        Role = _listRoles[0].Text,
                        Image = item.Image,
                        IdentityUser = user

                    });
                    _listRoles.Clear();
                }
            }
            return userList;
        }
        //internal async Task<SignInResult> UserLoginAsync(LoginModelInput model)
        //{
        //    var result = await _singInManager.PasswordSignInAsync(model.Email, model.Password,false, lockoutOnFailure:false);
        //    if (result.Succeeded)
        //    {

        //    }
        //    return result;
        //}
    }
}
