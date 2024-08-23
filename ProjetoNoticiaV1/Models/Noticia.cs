using System;
using System.Collections.Generic;

namespace ProjetoNoticiaV1.Models;

public partial class Noticia
{
    public int Id { get; set; }

    public string Texto { get; set; } = null!;

    public int UsuarioId { get; set; }

    public string Titulo { get; set; } = null!;

    public virtual ICollection<NoticiaTag>? NoticiaTags { get; set; } = new List<NoticiaTag>();

    public virtual Usuario Usuario { get; set; } = null!;
}
