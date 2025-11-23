namespace WorkWell.Domain.Entities;

public class ChatMessage : BaseEntity
{
    public int ChatConversationId { get; set; }
    public ChatConversation ChatConversation { get; set; } = null!;
    public string Role { get; set; } = string.Empty; // "user" ou "assistant"
    public string Conteudo { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? Sentimento { get; set; }
}
