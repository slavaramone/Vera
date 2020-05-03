using System.ComponentModel;

namespace Domain.Enums
{
    public enum ErrorCode
    {
        [Description("Все хорошо")]
        Ok = 0,

        [Description("Пользователь не найден")]
        UserNotFound = 1,

        [Description("Ошибка при отправке смс")]
        SmsSendingFailed = 2,

        [Description("Неверный смс код")]
        InvalidSmsCode = 3,

        [Description("Неверный токен")]
        InvalidToken = 4,

        [Description("СМС код устарел")]
        SmsCodeOutdated = 5,
      
        [Description("Кол-во попыток получения смс ограничено")]
        SmsHasDelay = 6,

        [Description("Телефон не найден")]
        PhoneNotFound = 7,

        [Description("Неизвестная ошибка")]
        Unknown = 255
	}
}
