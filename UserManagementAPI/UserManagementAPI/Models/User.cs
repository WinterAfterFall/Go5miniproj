using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UserManagementAPI.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))] // ทำให้ Swagger แสดงเป็นชื่อ Role แทนตัวเลข
    public enum RoleTypeEnum
    {
        Employee,
        SuperAdmin,
        Admin,
        HRAdmin
    }

    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? MobileNo { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Column(TypeName = "nvarchar(50)")]
        public RoleTypeEnum RoleType { get; set; } = RoleTypeEnum.Employee;

        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}