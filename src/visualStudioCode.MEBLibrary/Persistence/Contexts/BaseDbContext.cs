using System.Reflection;
using Domain.Entities;
using Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence.Contexts;

public class BaseDbContext : DbContext
{
    protected IConfiguration Configuration { get; set; }
    public DbSet<EmailAuthenticator> EmailAuthenticators { get; set; }
    public DbSet<OperationClaim> OperationClaims { get; set; }
    public DbSet<OtpAuthenticator> OtpAuthenticators { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
    public DbSet<Yazar> Yazarlar { get; set; }
    public DbSet<DeweySiniflama> DeweySiniflamalar { get; set; }
    public DbSet<Etkinlik> Etkinlikler { get; set; }
    public DbSet<KatalogKaydi> KatalogKayitlari { get; set; }
    public DbSet<KatalogKaydiYazar> KatalogKaydiYazarlar { get; set; }
    public DbSet<KatalogKonu> KatalogKonulari { get; set; }
    public DbSet<Kutuphane> Kutuphaneler { get; set; }
    public DbSet<KutuphaneBolumu> KutuphaneBolumleri { get; set; }
    public DbSet<Materyal> Materyalsler { get; set; }
    public DbSet<MateryalEtiket> MateryalEtiketler { get; set; }
    public DbSet<MateryalFormatDetay> MateryalFormatDetaylar { get; set; }
    public DbSet<Nusha> Nushas { get; set; }
    public DbSet<OduncIslemi> OduncIslemleri { get; set; }
    public DbSet<OtoriteKaydi> OtoriteKayitlari { get; set; }
    public DbSet<Raf> Raflar { get; set; }
    public DbSet<Rezervasyon> Rezervasyonlar { get; set; }
    public DbSet<YeniKatalogTalebi> YeniKatalogTalepleri { get; set; }

    public BaseDbContext(DbContextOptions dbContextOptions, IConfiguration configuration)
        : base(dbContextOptions)
    {
        Configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
