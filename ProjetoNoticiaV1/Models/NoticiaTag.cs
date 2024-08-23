using ProjetoNoticiaV1.Models;
using System;
using System.Collections.Generic;

namespace ProjetoNoticiaV1.Models;

public partial class NoticiaTag
{
    public int Id { get; set; }

    public int NoticiaId { get; set; }

    public int TagId { get; set; }

    public virtual Noticia Noticia { get; set; } = null!;

    public virtual Tag Tag { get; set; } = null!;
}
