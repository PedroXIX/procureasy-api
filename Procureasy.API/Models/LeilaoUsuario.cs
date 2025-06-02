namespace Procureasy.API.Models
{
    public class LeilaoUsuario
    {
        public int LeilaoId { get; set; }
        public Leilao Leilao { get; set; } = null!;
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
    }
}
