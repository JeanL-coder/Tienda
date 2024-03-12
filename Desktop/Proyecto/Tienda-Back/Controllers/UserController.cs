using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.RegularExpressions;
using Tienda.Helpers;
using Tienda.Models;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;

namespace Tienda.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UserController : ControllerBase
  {

    private readonly AppDbContext _authContext;
    public UserController(AppDbContext appDbContext)
    {
      _authContext = appDbContext;
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] Usuario userObj)
    {
      if (userObj == null)
        return BadRequest();

      var user = await _authContext.Usuarios.FirstOrDefaultAsync(x => x.Correo == userObj.Correo);
      if (user == null)
        return NotFound(new { Message = "Usuario no encontrado" });

      if (!PasswordHash.VerifyPassword(userObj.Password, user.Password))
      {
        return BadRequest(new { Message = "Contraseña Incorrecta" });
      }

      user.Token = CreateJwt(user);
      return Ok(new
      {
        Token = user.Token,
        Message = "Acceso Exitoso"
      });

    }
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] Usuario userObj)
    {
      if (userObj == null)
        return BadRequest();

      if (!IsValidEmail(userObj.Correo))
        return BadRequest(new { Message = "Formato de correo electrónico no válido." });

      if (await CheckCorreoExistAsync(userObj.Correo))
        return BadRequest(new { Message = "Correo ya existe" });

      var pass = CheckPasswordStrength(userObj.Password);
      if (!string.IsNullOrEmpty(pass))
        return BadRequest(new { Message = pass.ToString() });

      if (userObj.Password != userObj.Passwordv)
        return BadRequest(new { Message = "La contraseña y la confirmación de contraseña no coinciden." });
      userObj.Passwordv = PasswordHash.HashPassword(userObj.Passwordv);
      userObj.Password = PasswordHash.HashPassword(userObj.Password);
      userObj.Role = "User";
      userObj.Token = "";
      await _authContext.Usuarios.AddAsync(userObj);
      await _authContext.SaveChangesAsync();
      return Ok(new
      {
        Message = "Usuario creado"
      });
    }

    private async Task<bool> CheckCorreoExistAsync(string correo)
    {
      return await _authContext.Usuarios.AnyAsync(x => x.Correo == correo);
    }
    private string CheckPasswordStrength(string password)
    {
      StringBuilder sb = new StringBuilder();
      if (password.Length < 5)
        sb.Append("Mínimo de caracteres debe ser 5." + Environment.NewLine);
      if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]")
        && Regex.IsMatch(password, "[0-9]")))
        sb.Append("La contraseña debe ser alfanumérica." + Environment.NewLine);
      if (!Regex.IsMatch(password, "[<,>,,,.,;,:,/,?,',{,},[,|,!,@,#,$,%,^,&, *, (, ), _, -, +, = ]"))
        sb.Append("Contraseña debe tener caracteres especiales." + Environment.NewLine);
      return sb.ToString();
    }
    private bool IsValidEmail(string email)
    {
      {
        string emailPattern = @"^[a-zA-Z0-9._%+-]+@(gmail\.com|hotmail\.com|yahoo\.com)$";

        return Regex.IsMatch(email, emailPattern);
      }
    }
    private string CreateJwt(Usuario usuario)
    {
      var jwtTokenHandler = new JwtSecurityTokenHandler();

      // Generar una clave HMAC-SHA256 aleatoria de 256 bits
      var key = new byte[32]; // 256 bits / 8 = 32 bytes
      using (var rng = new RNGCryptoServiceProvider())
      {
        rng.GetBytes(key);
      }

      var identity = new ClaimsIdentity(new Claim[]
      {
        new Claim(ClaimTypes.Role, usuario.Role),
        new Claim(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}")
      });

      var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = identity,
        Expires = DateTime.Now.AddDays(1),
        SigningCredentials = credentials
      };

      var token = jwtTokenHandler.CreateToken(tokenDescriptor);
      return jwtTokenHandler.WriteToken(token);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<Usuario>> GetAllUsuarios()
    {
      return Ok(await _authContext.Usuarios.ToListAsync());
    }
  }
}

