using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace HomeAuthomationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectMessagesController : ControllerBase
    {
        private static readonly ConcurrentDictionary<string, List<DirectMessage>> _messages = new();

        public class DirectMessage
        {
            public string RouterDeviceId { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
            public string DeviceName { get; set; } = string.Empty;
            public string ParameterName { get; set; } = string.Empty;
            public string Value { get; set; } = string.Empty;
            public int DurationSeconds { get; set; } = 60;
        }

        [HttpPost]
        public IActionResult Post(DirectMessage message)
        {
            var list = _messages.GetOrAdd(message.RouterDeviceId, _ => new List<DirectMessage>());
            list.Add(message);
            return Ok();
        }

        [HttpGet("{routerId}")]
        public ActionResult<IEnumerable<DirectMessage>> Get(string routerId)
        {
            if (_messages.TryRemove(routerId, out var list))
            {
                return list;
            }
            return new List<DirectMessage>();
        }
    }
}
