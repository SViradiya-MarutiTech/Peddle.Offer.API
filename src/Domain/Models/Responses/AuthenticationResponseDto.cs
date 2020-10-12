using Newtonsoft.Json;
using System.Collections.Generic;

namespace Domain.Models.Responses
{
    public class AuthenticationResponseDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public bool IsVerified { get; set; }
        public string JWToken { get; set; }
        
    }
}
