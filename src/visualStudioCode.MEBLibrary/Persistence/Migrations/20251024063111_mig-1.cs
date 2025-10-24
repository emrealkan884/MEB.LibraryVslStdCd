using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ActionName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Payload = table.Column<string>(type: "text", nullable: true),
                    ClientIp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    UserAgent = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    OccurredOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeweySiniflamalar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Kod = table.Column<string>(type: "text", nullable: false),
                    Baslik = table.Column<string>(type: "text", nullable: false),
                    Aciklama = table.Column<string>(type: "text", nullable: true),
                    UstSinifId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeweySiniflamalar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeweySiniflamalar_DeweySiniflamalar_UstSinifId",
                        column: x => x.UstSinifId,
                        principalTable: "DeweySiniflamalar",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Kutuphaneler",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Il = table.Column<string>(type: "text", nullable: false),
                    Ilce = table.Column<string>(type: "text", nullable: false),
                    Kod = table.Column<string>(type: "text", nullable: false),
                    Ad = table.Column<string>(type: "text", nullable: false),
                    Tip = table.Column<int>(type: "integer", nullable: false),
                    Adres = table.Column<string>(type: "text", nullable: false),
                    Telefon = table.Column<string>(type: "text", nullable: true),
                    EPosta = table.Column<string>(type: "text", nullable: true),
                    Aktif = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kutuphaneler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperationClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OtoriteKayitlari",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    YetkiliBaslik = table.Column<string>(type: "text", nullable: false),
                    OtoriteTuru = table.Column<int>(type: "integer", nullable: false),
                    AlternatifBasliklar = table.Column<string>(type: "text", nullable: true),
                    BagliTerimler = table.Column<string>(type: "text", nullable: true),
                    Aciklama = table.Column<string>(type: "text", nullable: true),
                    HariciKayitNo = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtoriteKayitlari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: false),
                    AuthenticatorType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Yazarlar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AdSoyad = table.Column<string>(type: "text", nullable: false),
                    DogumTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Uyruk = table.Column<string>(type: "text", nullable: true),
                    Aciklama = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yazarlar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Etkinlikler",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    KutuphaneId = table.Column<Guid>(type: "uuid", nullable: false),
                    Baslik = table.Column<string>(type: "text", nullable: false),
                    Aciklama = table.Column<string>(type: "text", nullable: true),
                    BaslangicTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Konum = table.Column<string>(type: "text", nullable: true),
                    AfisDosyasi = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etkinlikler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Etkinlikler_Kutuphaneler_KutuphaneId",
                        column: x => x.KutuphaneId,
                        principalTable: "Kutuphaneler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KutuphaneBolumler",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    KutuphaneId = table.Column<Guid>(type: "uuid", nullable: false),
                    Ad = table.Column<string>(type: "text", nullable: false),
                    Aciklama = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KutuphaneBolumler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KutuphaneBolumler_Kutuphaneler_KutuphaneId",
                        column: x => x.KutuphaneId,
                        principalTable: "Kutuphaneler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YeniKatalogTalepleri",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TalepEdenKutuphaneId = table.Column<Guid>(type: "uuid", nullable: false),
                    Baslik = table.Column<string>(type: "text", nullable: false),
                    AltBaslik = table.Column<string>(type: "text", nullable: true),
                    YazarMetni = table.Column<string>(type: "text", nullable: true),
                    Isbn = table.Column<string>(type: "text", nullable: true),
                    Issn = table.Column<string>(type: "text", nullable: true),
                    MateryalTuru = table.Column<string>(type: "text", nullable: true),
                    MateryalAltTuru = table.Column<string>(type: "text", nullable: true),
                    Dil = table.Column<string>(type: "text", nullable: true),
                    Yayinevi = table.Column<string>(type: "text", nullable: true),
                    YayinYeri = table.Column<string>(type: "text", nullable: true),
                    YayinYili = table.Column<int>(type: "integer", nullable: true),
                    Aciklama = table.Column<string>(type: "text", nullable: true),
                    RedGerekcesi = table.Column<string>(type: "text", nullable: true),
                    Durum = table.Column<int>(type: "integer", nullable: false),
                    TalepTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SonGuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    KatalogKaydiId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YeniKatalogTalepleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YeniKatalogTalepleri_Kutuphaneler_TalepEdenKutuphaneId",
                        column: x => x.TalepEdenKutuphaneId,
                        principalTable: "Kutuphaneler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmailAuthenticators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ActivationKey = table.Column<string>(type: "text", nullable: true),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailAuthenticators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailAuthenticators_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OtpAuthenticators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SecretKey = table.Column<byte[]>(type: "bytea", nullable: false),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtpAuthenticators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OtpAuthenticators_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    ExpiresDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByIp = table.Column<string>(type: "text", nullable: false),
                    RevokedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RevokedByIp = table.Column<string>(type: "text", nullable: true),
                    ReplacedByToken = table.Column<string>(type: "text", nullable: true),
                    ReasonRevoked = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserOperationClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationClaimId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOperationClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserOperationClaims_OperationClaims_OperationClaimId",
                        column: x => x.OperationClaimId,
                        principalTable: "OperationClaims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOperationClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Raflar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    KutuphaneBolumuId = table.Column<Guid>(type: "uuid", nullable: false),
                    Kod = table.Column<string>(type: "text", nullable: false),
                    Aciklama = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Raflar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Raflar_KutuphaneBolumler_KutuphaneBolumuId",
                        column: x => x.KutuphaneBolumuId,
                        principalTable: "KutuphaneBolumler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KatalogKayitlari",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    KutuphaneId = table.Column<Guid>(type: "uuid", nullable: false),
                    Baslik = table.Column<string>(type: "text", nullable: false),
                    AltBaslik = table.Column<string>(type: "text", nullable: true),
                    Isbn = table.Column<string>(type: "text", nullable: true),
                    Yayinevi = table.Column<string>(type: "text", nullable: true),
                    YayinYeri = table.Column<string>(type: "text", nullable: true),
                    YayinYili = table.Column<int>(type: "integer", nullable: true),
                    SayfaSayisi = table.Column<int>(type: "integer", nullable: true),
                    Dil = table.Column<string>(type: "text", nullable: true),
                    Dizi = table.Column<string>(type: "text", nullable: true),
                    Baski = table.Column<string>(type: "text", nullable: true),
                    Ozet = table.Column<string>(type: "text", nullable: true),
                    Notlar = table.Column<string>(type: "text", nullable: true),
                    KapakResmiYolu = table.Column<string>(type: "text", nullable: true),
                    MateryalTuru = table.Column<int>(type: "integer", nullable: false),
                    MateryalAltTuru = table.Column<string>(type: "text", nullable: true),
                    DeweySiniflamaId = table.Column<Guid>(type: "uuid", nullable: true),
                    Marc21Verisi = table.Column<string>(type: "text", nullable: true),
                    MarcAlanlari = table.Column<string>(type: "text", nullable: true),
                    RdaUyumlu = table.Column<bool>(type: "boolean", nullable: false),
                    YeniKatalogTalebiId = table.Column<Guid>(type: "uuid", nullable: true),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KatalogKayitlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KatalogKayitlari_DeweySiniflamalar_DeweySiniflamaId",
                        column: x => x.DeweySiniflamaId,
                        principalTable: "DeweySiniflamalar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KatalogKayitlari_Kutuphaneler_KutuphaneId",
                        column: x => x.KutuphaneId,
                        principalTable: "Kutuphaneler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KatalogKayitlari_YeniKatalogTalepleri_YeniKatalogTalebiId",
                        column: x => x.YeniKatalogTalebiId,
                        principalTable: "YeniKatalogTalepleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KatalogKaydiYazarlar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    KatalogKaydiId = table.Column<Guid>(type: "uuid", nullable: false),
                    YazarId = table.Column<Guid>(type: "uuid", nullable: false),
                    Rol = table.Column<int>(type: "integer", nullable: false),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    OtoriteKaydiId = table.Column<Guid>(type: "uuid", nullable: true),
                    YazarId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KatalogKaydiYazarlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KatalogKaydiYazarlar_KatalogKayitlari_KatalogKaydiId",
                        column: x => x.KatalogKaydiId,
                        principalTable: "KatalogKayitlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KatalogKaydiYazarlar_OtoriteKayitlari_OtoriteKaydiId",
                        column: x => x.OtoriteKaydiId,
                        principalTable: "OtoriteKayitlari",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KatalogKaydiYazarlar_Yazarlar_YazarId",
                        column: x => x.YazarId,
                        principalTable: "Yazarlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KatalogKaydiYazarlar_Yazarlar_YazarId1",
                        column: x => x.YazarId1,
                        principalTable: "Yazarlar",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "KatalogKonular",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    KatalogKaydiId = table.Column<Guid>(type: "uuid", nullable: false),
                    KonuBasligi = table.Column<string>(type: "text", nullable: false),
                    OtoriteKaydiId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KatalogKonular", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KatalogKonular_KatalogKayitlari_KatalogKaydiId",
                        column: x => x.KatalogKaydiId,
                        principalTable: "KatalogKayitlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KatalogKonular_OtoriteKayitlari_OtoriteKaydiId",
                        column: x => x.OtoriteKaydiId,
                        principalTable: "OtoriteKayitlari",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MateryalFormatDetaylar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    KatalogKaydiId = table.Column<Guid>(type: "uuid", nullable: false),
                    MateryalTuru = table.Column<int>(type: "integer", nullable: false),
                    FizikselTanimi = table.Column<string>(type: "text", nullable: true),
                    SureBilgisi = table.Column<string>(type: "text", nullable: true),
                    FormatBilgisi = table.Column<string>(type: "text", nullable: true),
                    Dil = table.Column<string>(type: "text", nullable: true),
                    ErişimBilgisi = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MateryalFormatDetaylar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MateryalFormatDetaylar_KatalogKayitlari_KatalogKaydiId",
                        column: x => x.KatalogKaydiId,
                        principalTable: "KatalogKayitlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Materyaller",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    KatalogKaydiId = table.Column<Guid>(type: "uuid", nullable: false),
                    KutuphaneId = table.Column<Guid>(type: "uuid", nullable: false),
                    KutuphaneBolumuId = table.Column<Guid>(type: "uuid", nullable: true),
                    Formati = table.Column<int>(type: "integer", nullable: false),
                    RezervasyonaAcik = table.Column<bool>(type: "boolean", nullable: false),
                    MaksimumOduncSuresiGun = table.Column<int>(type: "integer", nullable: false),
                    Not = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materyaller", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materyaller_KatalogKayitlari_KatalogKaydiId",
                        column: x => x.KatalogKaydiId,
                        principalTable: "KatalogKayitlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Materyaller_KutuphaneBolumler_KutuphaneBolumuId",
                        column: x => x.KutuphaneBolumuId,
                        principalTable: "KutuphaneBolumler",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Materyaller_Kutuphaneler_KutuphaneId",
                        column: x => x.KutuphaneId,
                        principalTable: "Kutuphaneler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MateryalEtiketler",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MateryalId = table.Column<Guid>(type: "uuid", nullable: false),
                    Etiket = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MateryalEtiketler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MateryalEtiketler_Materyaller_MateryalId",
                        column: x => x.MateryalId,
                        principalTable: "Materyaller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Nushalar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MateryalId = table.Column<Guid>(type: "uuid", nullable: false),
                    RafId = table.Column<Guid>(type: "uuid", nullable: true),
                    Barkod = table.Column<string>(type: "text", nullable: false),
                    Durumu = table.Column<int>(type: "integer", nullable: false),
                    EklenmeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nushalar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nushalar_Materyaller_MateryalId",
                        column: x => x.MateryalId,
                        principalTable: "Materyaller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Nushalar_Raflar_RafId",
                        column: x => x.RafId,
                        principalTable: "Raflar",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rezervasyonlar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    KutuphaneId = table.Column<Guid>(type: "uuid", nullable: false),
                    KullaniciId = table.Column<Guid>(type: "uuid", nullable: false),
                    MateryalId = table.Column<Guid>(type: "uuid", nullable: false),
                    TalepTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HazirlanmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TamamlanmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Durumu = table.Column<int>(type: "integer", nullable: false),
                    Not = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezervasyonlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rezervasyonlar_Kutuphaneler_KutuphaneId",
                        column: x => x.KutuphaneId,
                        principalTable: "Kutuphaneler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rezervasyonlar_Materyaller_MateryalId",
                        column: x => x.MateryalId,
                        principalTable: "Materyaller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OduncIslemleri",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    KutuphaneId = table.Column<Guid>(type: "uuid", nullable: false),
                    KullaniciId = table.Column<Guid>(type: "uuid", nullable: false),
                    NushaId = table.Column<Guid>(type: "uuid", nullable: false),
                    AlinmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SonTeslimTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IadeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UzatmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UzatmaSayisi = table.Column<int>(type: "integer", nullable: false),
                    Durumu = table.Column<int>(type: "integer", nullable: false),
                    GecikmeCezaMiktari = table.Column<decimal>(type: "numeric", nullable: true),
                    GecikmeGunSayisi = table.Column<int>(type: "integer", nullable: true),
                    Not = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OduncIslemleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OduncIslemleri_Kutuphaneler_KutuphaneId",
                        column: x => x.KutuphaneId,
                        principalTable: "Kutuphaneler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OduncIslemleri_Nushalar_NushaId",
                        column: x => x.NushaId,
                        principalTable: "Nushalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "OperationClaims",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "Name", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Admin", null },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Auth.Admin", null },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Auth.Read", null },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Auth.Write", null },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Auth.RevokeToken", null },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "OperationClaims.Admin", null },
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "OperationClaims.Read", null },
                    { 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "OperationClaims.Write", null },
                    { 9, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "OperationClaims.Create", null },
                    { 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "OperationClaims.Update", null },
                    { 11, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "OperationClaims.Delete", null },
                    { 12, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "UserOperationClaims.Admin", null },
                    { 13, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "UserOperationClaims.Read", null },
                    { 14, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "UserOperationClaims.Write", null },
                    { 15, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "UserOperationClaims.Create", null },
                    { 16, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "UserOperationClaims.Update", null },
                    { 17, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "UserOperationClaims.Delete", null },
                    { 18, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Users.Admin", null },
                    { 19, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Users.Read", null },
                    { 20, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Users.Write", null },
                    { 21, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Users.Create", null },
                    { 22, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Users.Update", null },
                    { 23, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Users.Delete", null },
                    { 24, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Role.BakanlikYetkilisi", null },
                    { 25, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Role.IlYetkilisi", null },
                    { 26, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Role.IlceYetkilisi", null },
                    { 27, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Role.OkulKutuphaneYoneticisi", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AuthenticatorType", "CreatedDate", "DeletedDate", "Email", "FirstName", "LastName", "PasswordHash", "PasswordSalt", "Status", "UpdatedDate" },
                values: new object[] { new Guid("19b95d4d-46c8-4a85-bf81-a9e129a9baf3"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "narch@kodlama.io", "", "", new byte[] { 9, 164, 219, 150, 59, 254, 98, 227, 252, 207, 138, 98, 50, 88, 29, 16, 37, 228, 53, 245, 165, 228, 146, 227, 88, 62, 89, 139, 26, 207, 240, 85, 170, 90, 89, 42, 99, 146, 244, 27, 126, 204, 142, 60, 238, 188, 69, 123, 25, 0, 192, 220, 235, 195, 9, 33, 247, 175, 167, 106, 168, 155, 10, 209 }, new byte[] { 126, 125, 163, 87, 255, 136, 222, 28, 0, 0, 168, 94, 186, 99, 205, 75, 177, 160, 69, 204, 164, 222, 107, 235, 5, 79, 28, 179, 237, 234, 133, 4, 58, 119, 108, 65, 15, 83, 119, 130, 97, 136, 143, 229, 44, 37, 199, 185, 26, 233, 104, 200, 47, 149, 59, 108, 78, 55, 16, 227, 158, 78, 254, 78, 80, 60, 128, 97, 76, 40, 99, 3, 196, 105, 210, 183, 156, 17, 25, 187, 73, 74, 170, 113, 178, 192, 130, 236, 92, 37, 113, 51, 116, 187, 126, 142, 39, 24, 101, 27, 204, 66, 191, 86, 146, 254, 203, 128, 1, 185, 201, 120, 16, 27, 184, 127, 29, 145, 143, 128, 66, 176, 30, 174, 102, 104, 173, 154 }, true, null });

            migrationBuilder.InsertData(
                table: "UserOperationClaims",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "OperationClaimId", "UpdatedDate", "UserId" },
                values: new object[] { new Guid("c129f15b-e842-470f-a6c6-a4c520d6c731"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, null, new Guid("19b95d4d-46c8-4a85-bf81-a9e129a9baf3") });

            migrationBuilder.CreateIndex(
                name: "IX_DeweySiniflamalar_UstSinifId",
                table: "DeweySiniflamalar",
                column: "UstSinifId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailAuthenticators_UserId",
                table: "EmailAuthenticators",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Etkinlikler_KutuphaneId",
                table: "Etkinlikler",
                column: "KutuphaneId");

            migrationBuilder.CreateIndex(
                name: "IX_KatalogKaydiYazarlar_KatalogKaydiId",
                table: "KatalogKaydiYazarlar",
                column: "KatalogKaydiId");

            migrationBuilder.CreateIndex(
                name: "IX_KatalogKaydiYazarlar_OtoriteKaydiId",
                table: "KatalogKaydiYazarlar",
                column: "OtoriteKaydiId");

            migrationBuilder.CreateIndex(
                name: "IX_KatalogKaydiYazarlar_YazarId",
                table: "KatalogKaydiYazarlar",
                column: "YazarId");

            migrationBuilder.CreateIndex(
                name: "IX_KatalogKaydiYazarlar_YazarId1",
                table: "KatalogKaydiYazarlar",
                column: "YazarId1");

            migrationBuilder.CreateIndex(
                name: "IX_KatalogKayitlari_DeweySiniflamaId",
                table: "KatalogKayitlari",
                column: "DeweySiniflamaId");

            migrationBuilder.CreateIndex(
                name: "IX_KatalogKayitlari_KutuphaneId",
                table: "KatalogKayitlari",
                column: "KutuphaneId");

            migrationBuilder.CreateIndex(
                name: "IX_KatalogKayitlari_YeniKatalogTalebiId",
                table: "KatalogKayitlari",
                column: "YeniKatalogTalebiId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KatalogKonular_KatalogKaydiId",
                table: "KatalogKonular",
                column: "KatalogKaydiId");

            migrationBuilder.CreateIndex(
                name: "IX_KatalogKonular_OtoriteKaydiId",
                table: "KatalogKonular",
                column: "OtoriteKaydiId");

            migrationBuilder.CreateIndex(
                name: "IX_KutuphaneBolumler_KutuphaneId",
                table: "KutuphaneBolumler",
                column: "KutuphaneId");

            migrationBuilder.CreateIndex(
                name: "IX_MateryalEtiketler_MateryalId",
                table: "MateryalEtiketler",
                column: "MateryalId");

            migrationBuilder.CreateIndex(
                name: "IX_MateryalFormatDetaylar_KatalogKaydiId",
                table: "MateryalFormatDetaylar",
                column: "KatalogKaydiId");

            migrationBuilder.CreateIndex(
                name: "IX_Materyaller_KatalogKaydiId",
                table: "Materyaller",
                column: "KatalogKaydiId");

            migrationBuilder.CreateIndex(
                name: "IX_Materyaller_KutuphaneBolumuId",
                table: "Materyaller",
                column: "KutuphaneBolumuId");

            migrationBuilder.CreateIndex(
                name: "IX_Materyaller_KutuphaneId",
                table: "Materyaller",
                column: "KutuphaneId");

            migrationBuilder.CreateIndex(
                name: "IX_Nushalar_MateryalId",
                table: "Nushalar",
                column: "MateryalId");

            migrationBuilder.CreateIndex(
                name: "IX_Nushalar_RafId",
                table: "Nushalar",
                column: "RafId");

            migrationBuilder.CreateIndex(
                name: "IX_OduncIslemleri_KutuphaneId",
                table: "OduncIslemleri",
                column: "KutuphaneId");

            migrationBuilder.CreateIndex(
                name: "IX_OduncIslemleri_NushaId",
                table: "OduncIslemleri",
                column: "NushaId");

            migrationBuilder.CreateIndex(
                name: "IX_OtpAuthenticators_UserId",
                table: "OtpAuthenticators",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Raflar_KutuphaneBolumuId",
                table: "Raflar",
                column: "KutuphaneBolumuId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervasyonlar_KutuphaneId",
                table: "Rezervasyonlar",
                column: "KutuphaneId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervasyonlar_MateryalId",
                table: "Rezervasyonlar",
                column: "MateryalId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOperationClaims_OperationClaimId",
                table: "UserOperationClaims",
                column: "OperationClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOperationClaims_UserId",
                table: "UserOperationClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_YeniKatalogTalepleri_TalepEdenKutuphaneId",
                table: "YeniKatalogTalepleri",
                column: "TalepEdenKutuphaneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "EmailAuthenticators");

            migrationBuilder.DropTable(
                name: "Etkinlikler");

            migrationBuilder.DropTable(
                name: "KatalogKaydiYazarlar");

            migrationBuilder.DropTable(
                name: "KatalogKonular");

            migrationBuilder.DropTable(
                name: "MateryalEtiketler");

            migrationBuilder.DropTable(
                name: "MateryalFormatDetaylar");

            migrationBuilder.DropTable(
                name: "OduncIslemleri");

            migrationBuilder.DropTable(
                name: "OtpAuthenticators");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Rezervasyonlar");

            migrationBuilder.DropTable(
                name: "UserOperationClaims");

            migrationBuilder.DropTable(
                name: "Yazarlar");

            migrationBuilder.DropTable(
                name: "OtoriteKayitlari");

            migrationBuilder.DropTable(
                name: "Nushalar");

            migrationBuilder.DropTable(
                name: "OperationClaims");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Materyaller");

            migrationBuilder.DropTable(
                name: "Raflar");

            migrationBuilder.DropTable(
                name: "KatalogKayitlari");

            migrationBuilder.DropTable(
                name: "KutuphaneBolumler");

            migrationBuilder.DropTable(
                name: "DeweySiniflamalar");

            migrationBuilder.DropTable(
                name: "YeniKatalogTalepleri");

            migrationBuilder.DropTable(
                name: "Kutuphaneler");
        }
    }
}
