using System.ComponentModel.DataAnnotations;
using test.Application.Dtos.Enums;

namespace test.Application.Dtos
{
    public class EventDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public EventType Type { get; set; }
        public object Data { get; set; }
    }
}