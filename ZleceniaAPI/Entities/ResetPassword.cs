namespace ZleceniaAPI.Entities
{
    public class ResetPassword
    {
        public int Id { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
        public string ResetPasswordToken { get; set; }
        public DateTime ResetPasswordTokenExpiration { get; set; }
    }
}
