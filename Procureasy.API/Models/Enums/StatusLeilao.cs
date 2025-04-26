using System;
using System.ComponentModel.DataAnnotations;

namespace Procureasy.API.Models.Enums
{
    public enum StatusLeilao
    {
        [Display(Name = "ABERTO")]
        Aberto,
        
        [Display(Name = "ENCERRADO")]
        Encerrado,
        
        [Display(Name = "CANCELADO")]
        Cancelado
    }
}