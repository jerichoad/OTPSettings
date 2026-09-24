using MySqlConnector;
using OTPSettings.Models;
using System.Data;

namespace OTPSettings.Data
{
    public interface IOTPSettingRepository
    {
        Task<OTPSettingViewModel> GetAsync();
        Task UpdateAsync(OTPSettingViewModel model);
    }

    public class OTPSettingRepository : IOTPSettingRepository
    {
        private readonly string _connectionString;

        public OTPSettingRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<OTPSettingViewModel> GetAsync()
        {
            var model = new OTPSettingViewModel();

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand("usp_GetOTPSetting", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var actionName = reader.GetString(reader.GetOrdinal("ActionName"));
                var channelType = reader.GetString(reader.GetOrdinal("ChannelType"));
                var isEnabled = reader.GetBoolean(reader.GetOrdinal("IsEnabled"));

                switch ($"{actionName}|{channelType}")
                {
                    case "Withdrawal|Email": model.WithdrawalEmail = isEnabled; break;
                    case "Withdrawal|Whatsapp": model.WithdrawalWhatsapp = isEnabled; break;
                    case "ForgotPassword|Email": model.ForgotPasswordEmail = isEnabled; break;
                    case "ForgotPassword|Whatsapp": model.ForgotPasswordWhatsapp = isEnabled; break;
                    case "ResetPassword|Email": model.ResetPasswordEmail = isEnabled; break;
                    case "ResetPassword|Whatsapp": model.ResetPasswordWhatsapp = isEnabled; break;
                }
            }

            return model;
        }

        public async Task UpdateAsync(OTPSettingViewModel model)
        {
            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand("usp_UpdateOTPSetting", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("p_WithdrawalEmail", MySqlDbType.Bit).Value = model.WithdrawalEmail;
            command.Parameters.Add("p_WithdrawalWhatsapp", MySqlDbType.Bit).Value = model.WithdrawalWhatsapp;
            command.Parameters.Add("p_ForgotPasswordEmail", MySqlDbType.Bit).Value = model.ForgotPasswordEmail;
            command.Parameters.Add("p_ForgotPasswordWhatsapp", MySqlDbType.Bit).Value = model.ForgotPasswordWhatsapp;
            command.Parameters.Add("p_ResetPasswordEmail", MySqlDbType.Bit).Value = model.ResetPasswordEmail;
            command.Parameters.Add("p_ResetPasswordWhatsapp", MySqlDbType.Bit).Value = model.ResetPasswordWhatsapp;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
