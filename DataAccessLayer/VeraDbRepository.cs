using AutoMapper;
using Dapper;
using Domain;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class VeraDbRepository : IVeraDbRepository
    {
        private const string ConnectionStringName = "Vera";

        private readonly IConfiguration _config;
        private readonly VeraDbContext _db;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString(ConnectionStringName));
            }
        }

        public VeraDbRepository(IConfiguration config, VeraDbContext db, IMapper mapper, ILogger<VeraDbRepository> logger)
        {
            _config = config;
            _db = db;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<User> GetUser(string lastName, string firstName, string patronymic, string phone)
        {
            var users = await _db.Users.Where(x => x.LastName.ToLower().Equals(lastName.ToLower())
                && x.FirstName.ToLower().Equals(firstName.ToLower())
                && x.Patronymic.ToLower().Equals(patronymic.ToLower())).ToListAsync();

            if (users.Count == 0)
            {
                throw new UserNotFoundException();
            }

            foreach (var user in users)
            {
                if (user.Phone.Equals(phone))
                {
                    return new User(user.Id, user.LastName, user.FirstName, user.Patronymic, user.BirthDate, user.Role, user.Phone);
                }                 
            }
            throw new PhoneNotFoundException();
        }

        public async Task<Guid> CreateUserSmsSession(Guid userId, string smsCode)
        {            
            using (var connection = Connection)
            {
                int delayMinutes = 60;
                var tries = await connection.QuerySingleAsync<(int count, DateTime lasTimeWhen)>(
                    @"SELECT
	                            COUNT(*) AS cnt,
	                            MIN(dss.CreatedAtUtc) AS lastTime
                            FROM UserSmsSessions dss
                            WHERE 1=1
	                            AND dss.UserId = @userId
	                            AND dss.CreatedAtUtc >= DATEADD(MINUTE,-@delayMinutes,@time)", new { userId, time = DateTime.Now, delayMinutes});

                if (tries.count >= 3)
                {
                    int relatedTime = (int)(delayMinutes - (DateTime.Now - tries.lasTimeWhen).TotalMinutes);
                    if (relatedTime == 0)
                    {
                        int delaySeconds = 3600;
                        relatedTime = (int)(delaySeconds - (DateTime.Now - tries.lasTimeWhen).TotalSeconds);
                        throw new SmsDelayException($"Попробуйте через: {relatedTime} сек.");
                    }
                    throw new SmsDelayException($"Попробуйте через: {relatedTime} минут(ы)");
                }
            }
            
            var userSmsSession = new Entities.UserSmsSession
            {
                UserId = userId,
                CreatedAtUtc = DateTime.Now,
                ExpiresAtUtc = DateTime.Now.AddMinutes(Constants.SmsSessionDurationMinutes),
                SmsCode = smsCode
            };

            _db.UserSmsSessions.Add(userSmsSession);
            await _db.SaveChangesAsync();

            return userSmsSession.Id;
        }

        public async Task<ValidateSmsCodeResult> ValidateSmsCode(Guid sessionId, string smsCode)
        {
            var userSmsSessions = await _db.UserSmsSessions
                .OrderByDescending(x => x.ExpiresAtUtc)
                .FirstOrDefaultAsync(x => x.Id == sessionId && x.SmsCode.Equals(smsCode));

            if (userSmsSessions == null)
            {
                return new ValidateSmsCodeResult(false);
            }

            if (userSmsSessions.ExpiresAtUtc < DateTime.Now)
            {
                throw new SmsCodeOutdatedException();
            }

            return new ValidateSmsCodeResult(true, userSmsSessions.Id, userSmsSessions.UserId);
        }
    }
}