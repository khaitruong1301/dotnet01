using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class JwtAuthService
{
    private readonly string _key;
    private readonly string _issuer;
    private readonly string _audience;
    public JwtAuthService(IConfiguration Configuration)
    {
        _key = Configuration["jwt:private-key"];
        _issuer = Configuration["jwt:Issuer"];
        _audience =  Configuration["jwt:Audience"];

    }
    public string GenerateToken(UserLoginVM userLogin)
    {
        // Khóa bí mật để ký token
        var key = Encoding.ASCII.GetBytes(_key);
        // Tạo danh sách các claims cho token
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userLogin.UserName),               // Claim mặc định cho username
            new Claim(ClaimTypes.Role, userLogin.Role),                   // Claim mặc định cho Role
            new Claim(JwtRegisteredClaimNames.Sub, userLogin.UserName),   // Subject của token
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique ID của token
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()) // Thời gian tạo token
        };
        // Tạo khóa bí mật để ký token
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature
        );
        // Thiết lập thông tin cho token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(1), // Token hết hạn sau 1 giờ
            SigningCredentials = credentials,
            Issuer = _issuer,                 // Thêm Issuer vào token
            Audience = _audience              // Thêm Audience vào token
        };
        // Tạo token bằng JwtSecurityTokenHandler
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        // Trả về chuỗi token đã mã hóa
        return tokenHandler.WriteToken(token);
    }

}

