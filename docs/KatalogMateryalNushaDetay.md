# Katalog Kaydı – Materyal – Nüsha Senaryoları

Bu doküman katalog kaydı, materyal ve nüsha varlıklarının nasıl birlikte çalıştığını, merkez–okul iş akışlarını ve raporlama etkilerini detaylı şekilde açıklar.

## KatalogKaydi
- Kaynağın (kitap, dergi, video, dijital içerik vb.) bibliyografik kimlik kartıdır.
- `KutuphaneId`, kaydı hangi kütüphanenin oluşturduğunu gösterir; merkez tarafından oluşturulan kayıtlar merkez kütüphaneye, okulların geçici kayıtları ilgili okula bağlanır.
- `MateryalTuru` ve `MateryalAltTuru` alanları farklı formatları (kitap/roman, dergi/aylık bilim, video/belgesel gibi) ayırt eder.
- Tek kayıt, tüm kütüphaneler tarafından paylaşılabilir; ortak katalog havuzu mantığının temelini oluşturur.

## Materyal
- Bir katalog kaydının belirli bir kütüphanedeki kullanım parametrelerini barındırır.
- Okul kütüphanesi merkezdeki kaydı aldığında kendi `Materyal` kaydını açar; `KutuphaneId` okulun kimliğidir, `KatalogKaydiId` merkez kaydını işaretler.
- Ödünç süresi, rezervasyon durumu, bölüm/raf bağlantıları gibi yerel politikalar bu seviyede yönetilir.

## Nusha
- Materyalin tek tek kopyalarını temsil eder; her kopya benzersiz barkod veya RFID etiketine sahiptir.
- `RafId` ile fiziksel konumu, `Durumu` ile rafta/ödünçte gibi durumları izler.
- Okul kütüphanesinde aynı materyalden beş adet varsa, aynı `MateryalId` altında beş farklı `Nusha` kaydı tutulur.

## Senaryo Akışı
1. **Merkez Kaydı Oluşturur**  
   Merkez kütüphane, “Benim Adım Kırmızı” için `KatalogKaydi` oluşturur (`KutuphaneId` merkez, `MateryalTuru` = kitap).
2. **Okul Kütüphanesi Kaydı Kullanır**  
   Okul kütüphanesi bu kaydı havuzdan seçer ve kendi `Materyal` kaydını açar (örneğin ödünç süresi 15 gün, rezervasyona açık).
3. **Nüsha Girişi**  
   Okulda rafta beş kopya varsa beş `Nusha` kaydı oluşturulur; her birinde farklı barkod yer alır.
4. **Raporlama ve Yetki**  
   Merkez kütüphane, hangi okulun hangi katalog kaydını kullandığını ve stok miktarını raporlayabilir. Kaydı güncelleme yetkileri `KutuphaneId` üzerinden kontrol edilir.

Detaylandırılmış bu yapı, merkez–okul iş birliğini ve farklı formatlarda materyal yönetimini tutarlı biçimde destekler.

### Örnek Hikâye: Yahya Turan Fen Lisesi
1. Yahya Turan Fen Lisesi Kütüphanesi, rafına yeni gelen “Bilim İnsanlarının İzinde” kitabını öğrencilere sunmak ister. Sistemden arama yaptığında uygun `KatalogKaydi` bulamaz.  
2. Okul kütüphanecisi “Yeni Katalog Talebi” formunu açar; başlık, yazar, ISBN, Dewey kodu gibi bilgileri girer ve talebi gönderir. Talep kaydı `YeniKatalogTalebi` tablosuna `TalepEdenKutuphaneId = Yahya Turan Fen Lisesi` ve durum `Beklemede` olarak kaydedilir.
3. Merkez kütüphane talebi inceler. Bilgiler doğru ise talebi onaylar ve resmi `KatalogKaydi` oluşturur. Oluşan kaydın `KutuphaneId` alanı merkez kütüphaneyi, `KaynakTalepId` alanı ise Yahya Turan Fen Lisesi’nin talep kaydını gösterir. Artık kayıt merkez kataloğunun parçasıdır.
4. Onay sonrası okul kütüphanecisi artık kataloğu havuzda görür. `Materyal` kaydı açarak kitabın kendi kütüphanesinde kullanacağı parametreleri (ödünç süresi, bölüm, raf) belirler; `KutuphaneId` burada Yahya Turan Fen Lisesi’dir.
5. Kitaptan beş adet mevcut olduğundan her biri için ayrı `Nusha` kaydı oluşturur. Her `Nusha` kendi barkoduna ve `RafId` bilgisine sahiptir.
6. Merkez raporlama ekranında bu kitabın hangi okullar tarafından kullanıldığını, toplam kopya sayısını ve talebin hangi okuldan geldiğini `KutuphaneId`/`KaynakTalepId` alanları sayesinde görebilir. Gerekirse kayıt üzerinde düzeltme yapma yetkisi merkezde kalır.

## Neden `KatalogKaydi.KutuphaneId` Gerekli?
Aşağıdaki örnek akışlar `KutuphaneId` bilgisinin yokluğunda karşılaşılacak sorunları gösterir:

1. **Kaynağı Bilinmeyen Kayıtlar**  
   - Mart ayında merkez kütüphane 120 yeni kayıt ekler, aynı ay iki okul pilot çalışma kapsamında kendi koleksiyonlarından 20 kayıt girer.  
   - `KutuphaneId` olmadan “Bu 140 kaydı kim ekledi?” sorusuna yanıt yoktur; raporlama, performans ölçümü, sorumluluk takibi yapılamaz.

2. **Yetki Çatışması**  
   - Okul A, merkezden gelen bir kaydı yanlışlıkla günceller (ör. MARC alanını siliyor).  
   - `KutuphaneId` merkez olduğunu gösterdiği için sistem “bu kayıt merkezde, okul sadece talep edebilir” kuralını uygulayabilir. Alan olmasa hangi kaydın kim tarafından yönetildiği bilinmez ve tüm okullar kayıtları serbestçe değiştirebilir.

3. **Talep Sonrası Devralma**  
   - Okul B, “Tarihte Bilim” kitabı için talep gönderir; merkez onaylayıp kaydı oluşturur.  
   - Onaylanan kayıt `KutuphaneId = Merkez`, `KaynakTalepId = Talep123` olur. Böylece kayıt merkez tarafından yönetilirken talebin izini sürmek mümkündür.  
   - Eğer yalnızca `KaynakTalepId` tutulsa, merkez talebi kapattıktan sonra kayıt merkez kataloğunun parçası olduğuna dair bilgi kaybolur.

4. **Toplu Aktarım Senaryosu**  
   - Merkez KOHA’dan 10.000 kayıt içeri aktarır, bu kayıtların hiçbir talep kaydı yoktur.  
   - `KutuphaneId` sayesinde bu kayıtların merkez kataloğuna ait olduğu netleşir; okullar sadece materyal/nüsha ekler.

5. **Denetim ve Geri İzleme**  
   - Bir kayıt üzerinde uygunsuz değişiklik veya silme işlemi tespit edildiğinde, `KutuphaneId` hangi kurumun sorumlu olduğunu ve kimin bilgilendirileceğini belirlemede kullanılır.

Bu nedenle `KutuphaneId` bilgisi, merkez–okul sorumluluk ayrımını, talep sürecinin izlenmesini ve yetki politikalarının uygulanmasını sağlar; `KaynakTalepId` ise yalnızca talep kökenli kayıtları işaretleyerek bu yapıyı tamamlar.

## Otorite Kayıtları Neden Gerekli?
Merkez kütüphanenin “Otoriteler” modülü, katalogdaki kişi, kurum ve konu başlıklarının tutarlı kullanılmasını sağlamak için `OtoriteKaydi` varlığını kullanır. Örneğin “STEM Eğitimi” konu başlığı için tek bir otorite kaydı açılır:
- `YetkiliBaslik` resmi adıdır; `AlternatifBasliklar` alanında “Fen Bilimleri Eğitimi” gibi varyantlar tutulur.
- `BagliTerimler`, ilişkili veya daha geniş/dar kavramları listeler; arama yapılırken öneriler bu alandan gelir.
- `HariciKayitNo`, Library of Congress gibi dış authority sistemleriyle eşleşmeyi mümkün kılar.
- `OtoriteTuru` bu kaydın konu, kişi, kurum vb. olduğunu belirtir.

`KatalogKonu` ve `KatalogKaydiYazar` varlıkları ilgili katalog kayıtlarını bu otorite kaydına bağlar. Böylece Yahya Turan Fen Lisesi “Fen Bilimleri Eğitimi” diye arama yaptığında da “STEM Eğitimi” authority kaydına bağlı tüm kitapları bulur; farklı yazımlar tek başlık altında toplanır. Bu yapı, gereksinimlerde belirtilen “Otoriteler” fonksiyonunun domain tarafındaki karşılığıdır.

## Yeni Katalog Talebi Akışı
- Okul kütüphanesi tarama yaptığında uygun `KatalogKaydi` bulamazsa “Yeni Katalog Talebi” oluşturur.
- Talep kaydı (planlanan `YeniKatalogTalebi` varlığı) şu bilgileri içerecektir: talep eden kütüphane, materyal türü/alt türü, temel bibliyografik alanlar (başlık, yazar, ISBN veya ISSN varsa, yayın bilgileri), talep nedeni ve durum (`Beklemede`, `İnceleniyor`, `Onaylandı`, `Reddedildi`).
- Merkez kütüphane talebi inceleyip kabul ederse resmi `KatalogKaydi` oluşturur ve talep kaydını `Onaylandı` olarak işaretler. İlgili `KatalogKaydiId` talep kaydına bağlanır.
- Okul kütüphanesi talebin onaylandığını gördüğünde standart adımları izleyerek `Materyal` ve `Nusha` kayıtlarını açar.
- Reddedilen talepler gerekçe ile birlikte saklanır; okul yetkilisi gerekli düzeltmeyi yaparak yeniden gönderebilir.

Bu süreç sayesinde her okul, merkezde bulunmayan kaynaklar için kontrollü biçimde katalog talebinde bulunabilir; merkez de ortak kataloğu tutarlı şekilde genişletir.
