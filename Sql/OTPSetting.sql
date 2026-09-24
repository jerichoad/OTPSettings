CREATE DATABASE IF NOT EXISTS OTPSettingsDb
    CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

USE OTPSettingsDb;

CREATE TABLE IF NOT EXISTS OTPSetting
(
    OTPSettingId   INT UNSIGNED    NOT NULL AUTO_INCREMENT,
    ActionName     VARCHAR(50)     NOT NULL,   -- Withdrawal | ForgotPassword | ResetPassword
    ChannelType    VARCHAR(20)     NOT NULL,   -- Email | Whatsapp
    IsEnabled      TINYINT(1)      NOT NULL DEFAULT 0,
    ModifiedDate   DATETIME        NOT NULL DEFAULT CURRENT_TIMESTAMP
                                    ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (OTPSettingId),
    UNIQUE KEY UQ_OTPSetting_Action_Channel (ActionName, ChannelType)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

INSERT INTO OTPSetting (ActionName, ChannelType, IsEnabled)
VALUES
    ('Withdrawal',     'Email',    0),
    ('Withdrawal',     'Whatsapp', 0),
    ('ForgotPassword', 'Email',    0),
    ('ForgotPassword', 'Whatsapp', 0),
    ('ResetPassword',  'Email',    0),
    ('ResetPassword',  'Whatsapp', 0)
ON DUPLICATE KEY UPDATE ActionName = ActionName;

DROP PROCEDURE IF EXISTS usp_GetOTPSetting;

DELIMITER $$
CREATE PROCEDURE usp_GetOTPSetting()
BEGIN
    SELECT ActionName, ChannelType, IsEnabled
    FROM OTPSetting;
END $$
DELIMITER ;

DROP PROCEDURE IF EXISTS usp_UpdateOTPSetting;

DELIMITER $$
CREATE PROCEDURE usp_UpdateOTPSetting(
    IN p_WithdrawalEmail        TINYINT(1),
    IN p_WithdrawalWhatsapp     TINYINT(1),
    IN p_ForgotPasswordEmail    TINYINT(1),
    IN p_ForgotPasswordWhatsapp TINYINT(1),
    IN p_ResetPasswordEmail     TINYINT(1),
    IN p_ResetPasswordWhatsapp  TINYINT(1)
)
BEGIN
    UPDATE OTPSetting SET IsEnabled = p_WithdrawalEmail        WHERE ActionName = 'Withdrawal'     AND ChannelType = 'Email';
    UPDATE OTPSetting SET IsEnabled = p_WithdrawalWhatsapp     WHERE ActionName = 'Withdrawal'     AND ChannelType = 'Whatsapp';
    UPDATE OTPSetting SET IsEnabled = p_ForgotPasswordEmail    WHERE ActionName = 'ForgotPassword' AND ChannelType = 'Email';
    UPDATE OTPSetting SET IsEnabled = p_ForgotPasswordWhatsapp WHERE ActionName = 'ForgotPassword' AND ChannelType = 'Whatsapp';
    UPDATE OTPSetting SET IsEnabled = p_ResetPasswordEmail     WHERE ActionName = 'ResetPassword'  AND ChannelType = 'Email';
    UPDATE OTPSetting SET IsEnabled = p_ResetPasswordWhatsapp  WHERE ActionName = 'ResetPassword'  AND ChannelType = 'Whatsapp';
END $$
DELIMITER ;
