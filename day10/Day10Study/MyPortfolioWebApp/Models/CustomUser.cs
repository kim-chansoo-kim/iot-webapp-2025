using Microsoft.AspNetCore.Identity;

namespace MyPortfolioWebApp.Models
{
    // 회원가입시 추가로 받고 싶은 정보를 구성
    public class CustomUser : IdentityUser
    {
        public string? Mobile { get; set; }
        public string? City { get; set; }
        public string? Hobby { get; set; }
    }
}
