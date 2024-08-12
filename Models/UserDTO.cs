namespace ajaxdemonew.Models
{
    public class UserDTO
    {
        public string? userName {  get; set; }
        public string? userEmail { get; set; }
        public int? userAge { get; set; }
        public int? userpassword { get; set; }
        public IFormFile? userPhoto {  get; set; }
    }
}
