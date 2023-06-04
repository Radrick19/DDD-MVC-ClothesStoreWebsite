using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.CaptchaValidatorService
{
    public interface ICaptchaValidatorService
    {
        Task<bool> ValidateAsync(string token);
    }
}
