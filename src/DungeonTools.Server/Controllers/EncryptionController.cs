using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using DungeonTools.Save.File;
using DungeonTools.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DungeonTools.Server.Controllers {
    [Route(ServerConstants.ControllerName)]
    [ApiController]
    public class EncryptionController : Controller {
        private readonly ILogger<EncryptionController> _logger;

        public EncryptionController(ILogger<EncryptionController> logger) {
            _logger = logger;
        }

        [HttpGet(ServerConstants.ControllerKeysName)]
        public ActionResult GetKeys() {
            string logHeader = $"[{DateTime.Now}] {GetGdprFriendlyAddress(Request.HttpContext.Connection.RemoteIpAddress)}:{Request.HttpContext.Connection.RemotePort} ->";
            using(_logger.BeginScope($"{logHeader} EncryptionController::GetKeys")) {
                return StatusCode(StatusCodes.Status451UnavailableForLegalReasons);
            }
        }

        [HttpPost(ServerConstants.ControllerDecryptionName)]
        [RequestSizeLimit(5_000_000)]
        public async ValueTask<ActionResult<EncryptionData>> GetDecrypted([FromBody] EncryptionData rawData) {
            string logHeader = $"[{DateTime.Now}] {GetGdprFriendlyAddress(Request.HttpContext.Connection.RemoteIpAddress)}:{Request.HttpContext.Connection.RemotePort} ->";
            using(_logger.BeginScope($"{logHeader} EncryptionController::GetDecrypted")) {
                if(string.IsNullOrWhiteSpace(rawData.Encrypted)) {
                    _logger.LogInformation($"{logHeader} No decrypted data received.");
                    return StatusCode(StatusCodes.Status422UnprocessableEntity);
                }

                try {
                    _logger.LogInformation($"{logHeader} Received {rawData.Encrypted.Length} bytes of data.");
                    return await Decrypt(rawData);
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

        [HttpPost(ServerConstants.ControllerEncryptionName)]
        [RequestSizeLimit(5_000_000)]
        public async ValueTask<ActionResult<EncryptionData>> GetEncrypted([FromBody] EncryptionData rawData) {
            string logHeader = $"[{DateTime.Now}] {GetGdprFriendlyAddress(Request.HttpContext.Connection.RemoteIpAddress)}:{Request.HttpContext.Connection.RemotePort} ->";
            using(_logger.BeginScope($"{logHeader} EncryptionController::GetEncrypted")) {
                if(string.IsNullOrWhiteSpace(rawData.Decrypted)) {
                    _logger.LogInformation($"{logHeader} No decrypted data received.");
                    return StatusCode(StatusCodes.Status422UnprocessableEntity);
                }

                try {
                    _logger.LogInformation($"{logHeader} Received {rawData.Decrypted.Length} bytes of data.");
                    return await Encrypt(rawData);
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

        private async ValueTask<EncryptionData> Decrypt(EncryptionData rawData) {
            await using Stream encStream = rawData.EncryptedStream!;
            await using Stream decStream = await AesEncryptionProvider.DecryptAsync(encStream);
            SaveFileHandler.RemoveTrailingZeroes(decStream);
            return await EncryptionData.From(null, decStream);
        }

        private async ValueTask<EncryptionData> Encrypt(EncryptionData rawData) {
            await using Stream decStream = rawData.DecryptedStream!;
            await using Stream encStream = await AesEncryptionProvider.EncryptAsync(decStream);
            return await EncryptionData.From(encStream, null);
        }

        private static string GetGdprFriendlyAddress(IPAddress address) {
            string ipAddress = address.ToString();
            return ipAddress.Substring(0, ipAddress.Length - 3).PadRight(3, '#');
        }
    }
}
