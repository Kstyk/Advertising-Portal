using System.Net;
using Microsoft.AspNetCore.Mvc;
using ZleceniaAPI.Models;
using Microsoft.EntityFrameworkCore;
using ZleceniaAPI.Entities;
using ZleceniaAPI.Exceptions;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;

namespace ZleceniaAPI.Services
{
    public class PasswordResetService : IPasswordResetService
    {
        private OferiaDbContext _dbContext;
        private IPasswordHasher<User> _passwordHasher;

        public PasswordResetService(OferiaDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = context;
            _passwordHasher = passwordHasher;
        }

        public void ForgotPassword(ForgotPasswordDto dto)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == dto.Email);

            if (user == null)
            {
                throw new BadRequestException("Nieprawidłowy adres e-mail.");
            }

            var resetTokenExpired = _dbContext.ResetPasswords.FirstOrDefault(p => p.UserId == user.Id);
            if(resetTokenExpired != null)
            {
                _dbContext.ResetPasswords.Remove(resetTokenExpired);
            }

            var resetToken = Guid.NewGuid().ToString();

            ResetPassword resetPassword = new ResetPassword();
            resetPassword.UserId = user.Id;
            resetPassword.ResetPasswordToken = resetToken;
            resetPassword.ResetPasswordTokenExpiration = DateTime.Now.AddDays(1);

            _dbContext.ResetPasswords.Add(resetPassword);
            _dbContext.SaveChanges();

            SendResetPasswordEmail(dto.Email, resetToken);
        }

        public void ResetPassword(ResetPasswordDto dto)
        {
            var resetPasswordTokenRecord = _dbContext.ResetPasswords.FirstOrDefault(u => u.ResetPasswordToken == dto.ResetPasswordToken);

            if (resetPasswordTokenRecord == null) {
                throw new BadRequestException("Niepoprawny token resetowania hasła.");
            }

            var user = _dbContext.Users.FirstOrDefault(u => resetPasswordTokenRecord.UserId == u.Id);

            if(user == null)
            {
                throw new BadRequestException("Nie znaleziono użytkownika przypisanego do tego tokenu.");
            }

            if(DateTime.Now > resetPasswordTokenRecord.ResetPasswordTokenExpiration)
            {
                throw new BadRequestException("Token resetowania hasła wygasł.");
            }

            if(dto.Password != dto.ConfirmPassword)
            {
                throw new BadRequestException("Hasła nie są identyczne.");
            }

            var hashedPassword = _passwordHasher.HashPassword(user, dto.Password);
            user.Password = hashedPassword;

            _dbContext.Remove(resetPasswordTokenRecord);
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();

        }

        private void SendResetPasswordEmail(string emailAddress, string resetToken)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("Zlecenia.pl", "zleceniaresethasla@gmail.com"));
            email.To.Add(new MailboxAddress(emailAddress,emailAddress));

            email.Subject = "Resetowanie hasła";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $"Kliknij <a style='color:darkcyan;' href='http://localhost:5173/reset-password?token={resetToken}'>tutaj</a>, aby zresetować hasło."
            };

            var smtpSettings = GetSmtpSettingsFromConfig();

            using (var smtp = new SmtpClient())
            {
                smtp.Connect(smtpSettings.SmtpServer, smtpSettings.SmtpPort, false);
                smtp.Authenticate(smtpSettings.Username, smtpSettings.Password);

                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }

        private SmtpSettings GetSmtpSettingsFromConfig()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("smtpsettings.json")
                .Build();

            var smtpSettings = configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
            return smtpSettings;
        }
    }
}
