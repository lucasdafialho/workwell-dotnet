namespace WorkWell.Domain.Entities;

public class MeditationSession : BaseEntity
{
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public string Tipo { get; set; } = string.Empty; // "Mindfulness", "Respiração", etc.
    public int DuracaoMinutos { get; set; }
    public DateTime DataSessao { get; set; } = DateTime.UtcNow;
    public bool Concluida { get; set; }
    public string? Feedback { get; set; }
    public int? NivelRelaxamentoAntes { get; set; }
    public int? NivelRelaxamentoDepois { get; set; }
}
