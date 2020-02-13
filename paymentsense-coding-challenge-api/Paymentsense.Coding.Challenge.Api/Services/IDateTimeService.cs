using System;

namespace Paymentsense.Coding.Challenge.Api.Services
{
    public interface IDateTimeService
    {
        DateTime GetNow();
    }

    public class DateTimeService : IDateTimeService
    {
        public DateTime GetNow() => DateTime.Now;
    }
}