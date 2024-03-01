using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApiAutoresConStartup.DTOs;
using WebApiAutoresConStartup.Services;

namespace WebApiAutoresConStartup.Controllers.V1
{

    [ApiController]
    [Route("api/V1/cuentas")]
    public class CuentasController : ControllerBase
    {
        public UserManager<IdentityUser> UserManager { get; }
        public IConfiguration Configuration { get; }
        public SignInManager<IdentityUser> SignInManager { get; }
        public HashService HashService { get; }
        public IDataProtector DataProtection { get; }

        public CuentasController(UserManager<IdentityUser> userManager, 
                                            IConfiguration configuration, 
                                            SignInManager<IdentityUser> signInManager,
                                            IDataProtectionProvider dataProtectionProvider,
                                            HashService hashService)
        {
            UserManager = userManager;
            Configuration = configuration;
            SignInManager = signInManager;
            HashService = hashService;
            DataProtection =  dataProtectionProvider.CreateProtector("Espejito_Espejito");
        }

        #region hash, encripar

        //[HttpGet("hash/{textoPlano}")]
        //public ActionResult RealizarHash(string textoPlane)
        //{
        //    var resultado1 = HashService.Hash(textoPlane);
        //    var resultado2 = HashService.Hash(textoPlane);
        //    return Ok(new
        //    {
        //        textoPlane = textoPlane,
        //        Hash1 = resultado1,
        //        Hash2 = resultado2,

        //    });
        //}



        //[HttpGet("Encriptar")]
        //public ActionResult Encriptar()
        //{
        //    var textoPlano = "Junior Carpenter";
        //    var textoCifrado = DataProtection.Protect(textoPlano);
        //    var textoDecifrado = DataProtection.Unprotect(textoCifrado);


        //    return Ok(new
        //    {
        //        textoPlano = textoPlano,
        //        textoCifrado = textoCifrado,
        //        textoDecifrado = textoPlano
        //    });
        //}

        //[HttpGet("EncriptarPorTiempo")]
        //public ActionResult EncriptarPorTiempo()
        //{
        //    var dataProtectorTime = DataProtection.ToTimeLimitedDataProtector();
        //    var textoPlano = "Junior Carpenter";
        //    var textoCifrado = dataProtectorTime.Protect(textoPlano, lifetime: TimeSpan.FromSeconds(5));
        //    Thread.Sleep(6000);
        //    var textoDecifrado = dataProtectorTime.Unprotect(textoCifrado);


        //    return Ok(new
        //    {
        //        textoPlano = textoPlano,
        //        textoCifrado = textoCifrado,
        //        textoDecifrado = textoPlano
        //    });
        //}

        #endregion


        [HttpPost("registrar", Name = "registrarUsuario")]
        public async Task<ActionResult<RespuestaAutenticacion>> Registrar(CredencialesUsuario credencialesUsuario) 
        {

            var usuario = new IdentityUser { UserName = credencialesUsuario.Email, Email = credencialesUsuario.Email };
            var resultado = await UserManager.CreateAsync(usuario, credencialesUsuario.Pasword);

            if(resultado.Succeeded)
            {
                return await ConstruirToken(credencialesUsuario);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
            
        }

        [HttpPost("login", Name = "loginUsuario")]
        public async Task<ActionResult<RespuestaAutenticacion>> login(CredencialesUsuario credencialesUsuario)
        {
            var resultado = await SignInManager.PasswordSignInAsync(credencialesUsuario.Email, 
                                                                    credencialesUsuario.Pasword, 
                                                                    isPersistent: false, 
                                                                    lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(credencialesUsuario);
            }
            else
            {
                return BadRequest("Login incorrecto");
            }
        }

        [HttpGet("RenovarToken", Name = "renovarToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<RespuestaAutenticacion>> Renovar()
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;

            var credencialesUsuario = new CredencialesUsuario()
            {
                Email = email,
            };

            return await ConstruirToken(credencialesUsuario);
        }

        [HttpPost("HacerAdmin", Name = "hacerAdmin")]
        public async Task<ActionResult> HacerAdmin(EditarAdminDTO editarAdmin)
        {
            var usuario = await UserManager.FindByEmailAsync(editarAdmin.Email);
            await UserManager.AddClaimAsync(usuario, new Claim("EsAdmin", "1"));
            return NoContent();
        }

        [HttpPost("RemoverAdmin", Name = "removerAdmin")]
        public async Task<ActionResult> RemoverAdmin(EditarAdminDTO editarAdmin)
        {
            var usuario = await UserManager.FindByEmailAsync(editarAdmin.Email);
            await UserManager.RemoveClaimAsync(usuario, new Claim("EsAdmin", "1"));
            return NoContent();
        }


        private async Task<RespuestaAutenticacion> ConstruirToken(CredencialesUsuario credencialesUsuario)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", credencialesUsuario.Email),
                new Claim("Lo que yo quiera", "Cuando yo quiera")
            };


            var usuario = await UserManager.FindByEmailAsync(credencialesUsuario.Email);
            var claimsDB = await UserManager.GetClaimsAsync(usuario);

            claims.AddRange(claimsDB);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["LlaveJWT"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddYears(1);

            var security = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: creds);

            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(security),
                Expiration = expiration,
            };
        }
    }
}