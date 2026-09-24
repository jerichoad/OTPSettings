namespace OTPSettings.Models
{
    public class OTPSettingViewModel
    {
        public bool WithdrawalEmail { get; set; }
        public bool WithdrawalWhatsapp { get; set; }

        public bool ForgotPasswordEmail { get; set; }
        public bool ForgotPasswordWhatsapp { get; set; }

        public bool ResetPasswordEmail { get; set; }
        public bool ResetPasswordWhatsapp { get; set; }
    }
}
