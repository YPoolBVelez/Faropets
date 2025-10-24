using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Faropets.Areas.Clientes.Models;
using Faropets.Data;
using Faropets.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Faropets.Areas.Clientes.Pages.Account
{
    [Authorize]
    [Area("Clientes")]
    public class RegisterModel : PageModel
    {
        private SignInManager<IdentityUser> _singInManager;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;
        private static ModelInput _dataInput;
        private Uploadimage _uploadimage;
        private static RegistroModelInput _dataClient1, _dataClient2;
        private IWebHostEnvironment _environment;
        private LibClientes _cliente;

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
            _uploadimage = new Uploadimage();
            _cliente = new LibClientes(context);
        }

        public void OnGet(int id)
        {
            //_dataClient2 = null;
            if (id.Equals(0))
            {
                _dataClient2 = null;
                _dataInput = null;
            }
            if (_dataInput != null || _dataClient1 != null || _dataClient2 != null)
            {
                if (_dataInput != null)
                {
                    Input = _dataInput;
                    Input.AvatarImage = null;
                    Input.Image = _dataClient2.Image;
                }
                else
                {
                    if (_dataClient1 != null || _dataClient2 != null)
                    {
                        if (_dataClient2 != null)
                        {
                            _dataClient1 = _dataClient2;
                            Input = new ModelInput{
                                Nombre = _dataClient1.Nombre,
                                Apellido = _dataClient1.Apellido,
                                Email = _dataClient1.Email,
                                Image = _dataClient1.Image,
                                Telefono = _dataClient1.Telefono,
                                Direccion = _dataClient1.Direccion,
                                Credito = _dataClient1.Credito
                            };
                            if (_dataInput != null)
                            {
                                Input.ErrorMessage = _dataInput.ErrorMessage;
                            }
                        }
                    }
                }
            }
            else
            {
                Input = new ModelInput
                {

                };
            }
            if (_dataClient2 == null)
            {
                _dataClient2 = _dataClient1;
            }
            _dataClient1 = null;
        }
        [BindProperty]
        public ModelInput Input { get; set; }
        public class ModelInput : RegistroModelInput
        {
            public IFormFile AvatarImage { get; set; }
        }
        public async Task<IActionResult> OnPost(string DataClient)
        {
            if (DataClient == null)
            {
                if (_dataClient2 == null)
                {
                    //if (User.IsInRole("Admin"))
                    //{
                        if (await SaveAsync())
                        {
                        _dataClient2 = null;
                        _dataClient1 = null;
                        _dataInput = null;
                        return Redirect("/Clientes/Clientes?area=Clientes");
                        }
                        else
                        {
                            return Redirect("/Clientes/Register");
                        }
                    //}
                    //else
                    //{
                      //  return Redirect("/Clientes/Clientes?area=Clientes");
                    //}
                }
                else
                {
                    //if (User.IsInRole("Admin"))
                    //{
                    if (await UpdateAsync())
                    {
                        var url = $"/Clientes/Account/Details?=id{_dataClient2.IdCliente}";
                        _dataClient2 = null;
                        _dataClient1 = null;
                        _dataInput = null;
                        return Redirect(url);
                    }
                    else 
                    {
                        return Redirect("/Clientes/Register");
                    }
                    //}
                    //else
                    //{
                    //  return Redirect("/Clientes/Clientes?area=Clientes");
                    //}
                }
            }
            else
            {
                _dataClient1 = JsonConvert.DeserializeObject<RegistroModelInput>(DataClient);
                return Redirect("/Clientes/Register?id=1");
            }
        }
        private async Task<bool> SaveAsync()
        {
            _dataInput = Input;
            var valor = false;
            if (ModelState.IsValid)
            {
                var clientList = _context.TClients.Where(u => u.Email.Equals(Input.Email)).ToList();
                if (clientList.Count.Equals(0))
                {
                    var Estrategia = _context.Database.CreateExecutionStrategy();
                    await Estrategia.ExecuteAsync(async () =>
                    {
                        using (var transaction = _context.Database.BeginTransaction())
                        { 
                            try
                            {
                                var ImageByte = await _uploadimage.ByteAvatarImageAsync
                                (Input.AvatarImage, _environment, "images/images/default.png");
                                var Client = new TClients
                                {
                                    Nombre = Input.Nombre,
                                    Apellido = Input.Apellido,
                                    Email = Input.Email,
                                    Image = Input.Image,
                                    Telefono = Input.Telefono,
                                    Direccion = Input.Direccion,
                                    Fecha = DateTime.Now,
                                    Credito = Input.Credito
                                };
                                await _context.AddAsync(Client);
                                _context.SaveChanges();
                                var Report = new TReports_Clients
                                {
                                    Debt = 0.0m,
                                    Monthly = 0.0m,
                                    Change = 0.0m,
                                    LastPayment = 0.0m,
                                    CurrentDebt = 0.0m,
                                    Ticket = "0000000000",
                                    TClients = Client
                                };
                                await _context.AddAsync(Report);
                                _context.SaveChanges();
                                transaction.Commit();
                                _dataInput = null;
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
                }
                else
                {
                    _dataInput.ErrorMessage = $"{Input.Email} ya esta registrado";
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
        private async Task<bool> UpdateAsync()
        {
            _dataInput = Input;
            var valor = false;
            byte[] imageByte = null;
            var Estrategia = _context.Database.CreateExecutionStrategy();
            await Estrategia.ExecuteAsync(async () =>{
                using (var transaction = _context.Database.BeginTransaction()) 
                {
                    try
                    {
                        var clientData = _cliente.getTClient(Input.Email);
                        if (clientData.Count.Equals(0) || clientData[0].IdCliente.Equals(_dataClient2.IdCliente))
                        {
                            if (Input.AvatarImage == null)
                            {
                                imageByte = _dataClient2.Image;
                            }
                            else
                            {
                                imageByte = await _uploadimage.ByteAvatarImageAsync(Input.AvatarImage, _environment, "");
                            }
                            var Client = new TClients
                            {
                                Nombre = Input.Nombre,
                                Apellido = Input.Apellido,
                                Email = Input.Email,
                                Image = Input.Image,
                                Telefono = Input.Telefono,
                                Direccion = Input.Direccion,
                                Fecha = DateTime.Now,
                                Credito = Input.Credito
                            };
                            _context.Update(Client);
                            _context.SaveChanges();
                            transaction.Commit();

                            valor = true;
                        }
                        else
                        {
                            _dataInput.ErrorMessage = $"{Input.Email} ya esta registrado";
                            valor = false;
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
            return valor;
        }
    }

}
