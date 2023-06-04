using Castle.Core.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Store.Application.Services.CaptchaValidatorService
{
    public class CaptchaValidatorService : ICaptchaValidatorService
    {
        private readonly string _secretKey;
        private readonly double _acceptableScore;
        private readonly HttpClient _httpClient;

        public CaptchaValidatorService(HttpClient client, HttpClient httpClient)
        {
            _secretKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ReCaptcha")["SecretKey"];
            string score = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ReCaptcha")["AcceptableScore"];
            _acceptableScore = double.Parse(score, System.Globalization.CultureInfo.InvariantCulture);
            _httpClient = httpClient;
        }


        public async Task<bool> ValidateAsync(string token)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("secret", _secretKey),
                new KeyValuePair<string, string>("response", token)
            });
            var res = await _httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
            if (res.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new HttpRequestException(res.ReasonPhrase);
            }
            var jsonResult = await res.Content.ReadAsStringAsync();
            dynamic response = JObject.Parse(jsonResult);
            if (response.success == "true")
            {
                return Convert.ToDouble(response.score) >= _acceptableScore;
            }
            return false;
        }
    }
}
