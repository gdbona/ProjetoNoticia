using ProjetoNoticiaV1.Models;
using System;
using System.Collections.Generic;

namespace ProjetoNoticiaV1.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Noticia> Noticia { get; set; } = new List<Noticia>();
}
