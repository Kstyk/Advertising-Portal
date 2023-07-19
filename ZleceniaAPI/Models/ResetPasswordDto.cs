namespace ZleceniaAPI.Models
{
    public class ResetPasswordDto
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string ResetPasswordToken { get; set; }
    }
}
