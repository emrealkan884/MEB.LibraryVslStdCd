using Domain.Enums;

namespace Domain.Entities;

public class KatalogKaydiYazar
{
    public int Id { get; set; }
    public int KatalogKaydiId { get; set; }
    public int YazarId { get; set; }
    public YazarRolu Rol { get; set; } = YazarRolu.Yazar;
    public int Sira { get; set; } = 1;

    public KatalogKaydi? KatalogKaydi { get; set; }
    public Yazar? Yazar { get; set; }
}
