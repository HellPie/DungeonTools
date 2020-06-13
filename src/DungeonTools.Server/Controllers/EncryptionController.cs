using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using DungeonTools.Encryption;
using DungeonTools.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DungeonTools.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class EncryptionController : Controller {
        private readonly IEncryptionService _encryptionService;
        private readonly ILogger<EncryptionController> _logger;

        public EncryptionController(ILogger<EncryptionController> logger) {
            _encryptionService = new AesEncryptionService();
            _logger = logger;
        }

        [HttpGet("keys")]
        public ActionResult GetKeys() {
            string logHeader = $"[{DateTime.Now}] {GetGdprFriendlyAddress(Request.HttpContext.Connection.RemoteIpAddress)}:{Request.HttpContext.Connection.RemotePort} ->";
            using(_logger.BeginScope($"{logHeader} EncryptionController::GetKeys")) {
                return StatusCode(StatusCodes.Status451UnavailableForLegalReasons);
            }
        }

        [HttpPost("decrypt")]
        [RequestSizeLimit(256_000)]
        public async ValueTask<ActionResult<RawDataModel>> GetDecrypted([FromBody] RawDataModel rawData) {
            string logHeader = $"[{DateTime.Now}] {GetGdprFriendlyAddress(Request.HttpContext.Connection.RemoteIpAddress)}:{Request.HttpContext.Connection.RemotePort} ->";
            using(_logger.BeginScope($"{logHeader} EncryptionController::GetDecrypted")) {
                if(string.IsNullOrWhiteSpace(rawData.Encrypted)) {
                    _logger.LogInformation($"{logHeader} No decrypted data received.");
                    return StatusCode(StatusCodes.Status422UnprocessableEntity);
                }

                try {
                    _logger.LogInformation($"{logHeader} Received {rawData.Encrypted.Length} bytes of data.");
                    string data = PadBase64String(rawData.Encrypted);
                    return await Decrypt(data);
                } catch(Exception e) {
                    _logger.LogError(e, $"{logHeader} Decryption threw exception.");
#if DEBUG
                    // Keep this constrained to local debug builds to avoid having to setup a privacy policy
                    _logger.LogDebug($"{logHeader} Encrypted data was: {rawData.Encrypted}.");
#endif
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
        }

        [HttpPost("encrypt")]
        [RequestSizeLimit(128_000)]
        public async ValueTask<ActionResult<RawDataModel>> GetEncrypted([FromBody] RawDataModel rawData) {
            string logHeader = $"[{DateTime.Now}] {GetGdprFriendlyAddress(Request.HttpContext.Connection.RemoteIpAddress)}:{Request.HttpContext.Connection.RemotePort} ->";
            using(_logger.BeginScope($"{logHeader} EncryptionController::GetEncrypted")) {
                if(string.IsNullOrWhiteSpace(rawData.Decrypted)) {
                    _logger.LogInformation($"{logHeader} No decrypted data received.");
                    return StatusCode(StatusCodes.Status422UnprocessableEntity);
                }

                try {
                    _logger.LogInformation($"{logHeader} Received {rawData.Decrypted.Length} bytes of data.");
                    string data = PadBase64String(rawData.Decrypted);
                    return await Encrypt(data);
                } catch(Exception e) {
                    _logger.LogError(e, $"{logHeader} Encryption threw exception.");
#if DEBUG
                    // Keep this constrained to local debug builds to avoid having to setup a privacy policy
                    _logger.LogDebug($"{logHeader} Decrypted data was: {rawData.Decrypted}.");
#endif
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
        }

        private async ValueTask<RawDataModel> Decrypt(string base64Data) {
            await using MemoryStream encStream = new MemoryStream(Convert.FromBase64String(base64Data));
            await using Stream decStream = _encryptionService.Decrypt(encStream);
            return new RawDataModel {Decrypted = await GetBase64Data(decStream)};
        }

        private async ValueTask<RawDataModel> Encrypt(string base64Data) {
            await using MemoryStream decStream = new MemoryStream(Convert.FromBase64String(base64Data));
            await using Stream encStream = _encryptionService.Encrypt(decStream);
            return new RawDataModel {Encrypted = await GetBase64Data(encStream)};
        }

        private static async ValueTask<string> GetBase64Data(Stream stream) {
            byte[] data = new byte[stream.Length];
            await stream.ReadAsync(data);

            return Convert.ToBase64String(data);
        }

        private static string PadBase64String(string base64) {
            return base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');
        }

        private static string GetGdprFriendlyAddress(IPAddress address) {
            string ipAddress = address.ToString();
            return ipAddress.Substring(0, ipAddress.Length - 3).PadRight(3, '#');
        }
    }
}
