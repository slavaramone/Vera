using Domain.Models;
using System;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IVeraDbRepository
    {
        Task<User> GetUser(string lastName, string firstName, string patronymic, string phone);

        Task<Guid> CreateMeasurement(Measurement measurement);

        Task<Guid> CreateUserSmsSession(Guid userId, string smsCode);

        Task<ValidateSmsCodeResult> ValidateSmsCode(Guid sessionId, string smsCode);
    }
}
