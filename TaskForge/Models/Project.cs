using System.ComponentModel.DataAnnotations;

namespace TaskForge.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Proje adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Proje adı en fazla 100 karakter olabilir.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Başlangıç tarihi zorunludur.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Bitiş tarihi zorunludur.")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Proje durumu zorunludur.")]
        public ProjectStatus Status { get; set; }

        [Required(ErrorMessage = "Proje önceliği zorunludur.")]
        public ProjectPriority ProjectPriority { get; set; }

        public DateTime CreatedAt { get; set; }
        public string? UserId { get; set; }

        public ApplicationUser? User { get; set; }
        public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();
    }
}