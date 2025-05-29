namespace Procureasy.API.Dtos.Produto
{
    public class ProdutoUpdateDto
    {
        public string Nome { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
    }
}
