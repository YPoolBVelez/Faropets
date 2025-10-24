using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Faropets.Areas.Users.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Faropets.Library;
using Microsoft.AspNetCore.Mvc.Rendering;
using Faropets.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace Faropets.Areas.Users.Pages.Account
{
    [Authorize]
    [Area("Users")]
    public class RegisterModel : PageModel
    {
        private SignInManager<IdentityUser> _singInManager;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;
        private LibraryUsersRoles _usersRole;
        private static ModelInput _dataInput;
        private static RegistroModelInput _dataUser1, _dataUser2;
        private Uploadimage _uploadimage;
        private IWebHostEnvironment _environment;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _singInManager = signInManager;
            _roleManager = roleManager;
            _environment = environment;
            _usersRole = new LibraryUsersRoles();
            _uploadimage = new Uploadimage();
        }
        public void OnGet(int id)
        {
            _dataUser2 = null;
            if (id.Equals(0))
            {
                _dataUser2 = null;
                _dataInput = null;
            }

            if (_dataInput != null || _dataUser1 != null || _dataUser2 != null)
            {
                if (_dataInput != null)
                {
                    Input = _dataInput;
                    Input.rolesLista = _usersRole.getRoles(_roleManager);
                    Input.AvatarImage = null;
                    Input.Image = _dataUser2.Image;
                }
                else
                {
                    if (_dataUser1 != null || _dataUser2 != null)
                    {
                        if (_dataUser2 != null)
                            _dataUser1 = _dataUser2;
                        Input = new ModelInput
                        {
                            Nombre = _dataUser1.Nombre,
                            Apellido = _dataUser1.Apellido,
                            Email = _dataUser1.Email,
                            Image = _dataUser1.Image,
                            rolesLista = getRoles(_dataUser1.Role),
                        };
                        if (_dataInput != null)
                        {
                            Input.ErrorMessage = _dataInput.ErrorMessage;
                        }
                    }
                }

            }
            else
            {
                Input = new ModelInput
                {
                    rolesLista = _usersRole.getRoles(_roleManager)
                };
            }
            if (_dataUser2 == null)
            {
                _dataUser2 = _dataUser1;
            }
                _dataUser1 = null;
        }        
        [BindProperty]
        public ModelInput Input {get; set;}
        public class ModelInput : RegistroModelInput
        {
            public IFormFile AvatarImage { get; set; }
            [TempData]
            public string ErrorMessage { get; set; }
            public List<SelectListItem> rolesLista { get; set; }
        }
        public async Task<IActionResult> OnPost(String dataUser)
        {
            if (dataUser == null)
            {
                if (User.IsInRole("Admin"))
                {
                    if (_dataUser2 == null)
                    {
                        if (await SaveAsync())
                        {
                            _dataUser2 = null;
                            _dataUser1 = null;
                            _dataInput = null;
                            return Redirect("/Users/Users?area=Users");
                        }
                        else
                        {
                            return Redirect("/Users/Register");
                        }
                    }
                    else
                    {
                        return Redirect("/Users/Users?area=Users");
                    }
                }
                else
                {
                    if (User.IsInRole("Admin"))
                    {
                        if (await UpdateAsync())
                        {
                            var url = $"/Users/Account/Details?id={_dataUser2.Id}";
                            _dataUser2 = null;
                            _dataUser1 = null;
                            _dataInput = null;
                            return Redirect(url);
                        }
                        else
                        {
                            return Redirect("/Users/Register");
                        }
                    }
                    else
                    {
                        return Redirect("/Users/Users?area=Users");
                    }
                }


            }
            else
            {
                _dataUser1 = JsonConvert.DeserializeObject<RegistroModelInput>(dataUser);
                return Redirect("/Users/Register?id=1");
            }
        }
        [Authorize(Roles = "Admin")]
        private async Task<bool> SaveAsync()
        {
            _dataInput = Input;
            var valor = false;
            if (ModelState.IsValid)
            {
                var userList = _userManager.Users.Where(u => u.Email.Equals(Input.Email)).ToList();
                if (userList.Count.Equals(0))
                {
                    var estrategia = _context.Database.CreateExecutionStrategy();
                    await estrategia.ExecuteAsync(async () => { 
                        using (var transaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                var user = new IdentityUser
                                {
                                    UserName = Input.Email,
                                    Email = Input.Email,                                                                        
                                };
                                var result = await _userManager.CreateAsync(user, Input.Password);
                                if (result.Succeeded)
                                {
                                    await _userManager.AddToRoleAsync(user, Input.Role);
                                    var dataUser = _userManager.Users.Where(u => u.Email.Equals(Input.Email)).ToList().Last();
                                    var ImageByte = await _uploadimage.ByteAvatarImageAsync
                                    (Input.AvatarImage, _environment, "images/images/default.png");
                                    var T_User = new TUsers
                                    {
                                        Nombre = Input.Nombre,
                                        Apellido = Input.Apellido,
                                        Email = Input.Email,
                                        IdUser = dataUser.Id,
                                        Image = ImageByte

                                    };
                                    await _context.AddAsync(T_User);
                                    _context.SaveChanges();
                                    transaction.Commit();
                                    _dataInput = null;
                                    valor = true;
                                }
                                else
                                {
                                    foreach (var item in result.Errors)
                                    {
                                        _dataInput.ErrorMessage = item.Description;
                                    }
                                    valor = false;
                                    transaction.Rollback();
                                }
                               
                            }
                            catch (Exception ex)
                            {
                                _dataInput.ErrorMessage = ex.Message;
                                transaction.Rollback();
                                valor = false;
                                
                            }
                        }
                    });


                }
                else
                {
                    _dataInput.ErrorMessage = $"El {Input.Email} ya esta registrado";
                    valor = false;
                }
            }
            else
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _dataInput.ErrorMessage += error.ErrorMessage;
                    }
                }
                
                valor = false;
            }
            return valor;
        }
        private List<SelectListItem> getRoles(String role)
        {
            List<SelectListItem> rolesLista = new List<SelectListItem>();
            rolesLista.Add(new SelectListItem
                {
                Text = role          
            });
            var roles = _usersRole.getRoles(_roleManager);
            roles.ForEach(item => {
                if (item.Text != role)
                {
                    rolesLista.Add(new SelectListItem
                    {
                        Text = item.Text
                    }); ;
                }
            });
            return rolesLista;
        }
 
        private async Task<bool> UpdateAsync()
        {
            var valor = false;
            byte[] imageByte = null;
            var estrategia = _context.Database.CreateExecutionStrategy();
            await estrategia.ExecuteAsync(async () => 
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var identityUser = _userManager.Users.Where(u => u.Id.Equals(_dataUser2.ID)).ToList().Last();
                        identityUser.UserName = Input.Email;
                        identityUser.Email = Input.Email;
                        _context.Update(identityUser);
                        await _context.SaveChangesAsync();
                        if (Input.AvatarImage == null)
                        {
                            imageByte = _dataUser2.Image;
                        }
                        else
                        {
                            imageByte = await _uploadimage.ByteAvatarImageAsync(Input.AvatarImage, _environment, "");
                        }
                        var t_user = new TUsers
                        {
                            ID = _dataUser2.Id,
                            Nombre = Input.Nombre,
                            Apellido = Input.Apellido,
                            Email = Input.Email,
                            IdUser = _dataUser2.ID,
                            Image = imageByte,
                        };
                        _context.Update(t_user);
                        _context.SaveChanges();
                        if (_dataUser2.Role != Input.Role )
                        {
                            await _userManager.RemoveFromRoleAsync(identityUser, _dataUser2.Role);
                            await _userManager.AddToRoleAsync(identityUser, Input.Role);
                        }
                        transaction.Commit();

                        valor = true;

                    }
                    catch (Exception ex)
                    {

                        _dataInput.ErrorMessage = ex.Message;
                        transaction.Rollback();
                        valor = false;

                    }
                }
            });
            return valor;
        }
    }
}