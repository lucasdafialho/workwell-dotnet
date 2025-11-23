namespace WorkWell.Domain.Entities;

public class ChatConversation : BaseEntity
{
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public List<ChatMessage> Mensagens { get; set; } = new();
    public DateTime DataInicio { get; set; } = DateTime.UtcNow;
    public DateTime DataUltimaMensagem { get; set; } = DateTime.UtcNow;
    public string? Topico { get; set; }
}
