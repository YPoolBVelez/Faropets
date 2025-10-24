using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Faropets.Areas.Clientes.Models;
using Faropets.Data;
using Faropets.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Faropets.Areas.Clientes.Pages.Account
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private LibClientes _clientes;
    public DetailsModel(
          ApplicationDbContext context)
        {
            _clientes = new LibClientes(context);
        }
        public void OnGet(int id)
        {
            var data = _clientes.getTClientsAsync(null, id);
            if (0 < data.Count)
            {
                Input = new InputModel
                {
                    DataClient = data.ToList().Last(),
                };
            }
           
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
           public RegistroModelInput DataClient { get; set; }
        }
    }
}
