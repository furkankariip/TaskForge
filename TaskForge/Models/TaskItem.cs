using System.ComponentModel.DataAnnotations;

namespace TaskForge.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Görev başlığı zorunludur.")]
        [StringLength(150, ErrorMessage = "Görev başlığı en fazla 150 karakter olabilir.")]
        public string? Title { get; set; }

        [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Bitiş tarihi zorunludur.")]
        public DateTime DueDate { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime CreatedAt { get; set; }

        // Foreign Key
        public int ProjectId { get; set; }

        // Navigation Property
        public Project? Project { get; set; }
    }
}