using Newtonsoft.Json;

namespace ProductAPI.Domain.Models;

public class RefreshToken {
    [JsonIgnore]
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Token { get; set; } 
    public bool IsUsed { get; set; }
    
    public bool IsRevoked { get; set; }
    public DateTime AddedDate { get; set; }
    public DateTime ExpDate { get; set; }
    

}