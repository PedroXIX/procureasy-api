using System;
using System.ComponentModel.DataAnnotations;

namespace Procureasy.API.Models.Enums
{
    public enum TipoUsuario
    {
        [Display(Name = "CONSUMIDOR")]
        Consumidor,
        
        [Display(Name = "FORNECEDOR")]
        Fornecedor,
        
        [Display(Name = "ADMINISTRADOR")]
        Administrador
    }
}