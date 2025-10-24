using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Faropets.Library
{
    public class Uploadimage
    {
        public async Task<Byte[]> ByteAvatarImageAsync(IFormFile AvatarImage , IWebHostEnvironment environment, string image)
        {
             
            if (AvatarImage != null)
            {
                using(var memorystream = new MemoryStream())
                {
                    await AvatarImage.CopyToAsync(memorystream);
                    return memorystream.ToArray();
                }
            }
            else
            {
                var archivoOrigen = $"{environment.ContentRootPath}/wwwroot/{image}";
                return File.ReadAllBytes(archivoOrigen);
            }
        }
    }
}
