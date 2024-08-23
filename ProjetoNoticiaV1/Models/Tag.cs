using System;
using System.Collections.Generic;

namespace ProjetoNoticiaV1.Models;

public partial class Tag
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<NoticiaTag> NoticiaTags { get; set; } = new List<NoticiaTag>();
}
