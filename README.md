# PusulaSu

PusulaSu, su abonelerinin sayaç okumalarını kaydedebildiği tüketimlerini takip edebildiği ve güncel tarifeye göre tahmini su bedelini görüntüleyebildiği web tabanlı bir takip uygulamasıdır.

Bu projeyi su tüketim sürecini hem kullanıcı hem de yönetici açısından daha anlaşılır ve düzenli hâle getirmek amacıyla geliştirdim.

## Özellikler

### Abone işlemleri

- Abone ve sayaç numarasıyla üyelik doğrulama
- Güvenli giriş ve şifre yenileme
- Abonelik bilgilerinin görüntülenmesi
- Tarih ve endeks bilgisiyle sayaç okuması ekleme
- Geçmiş sayaç okumalarının listelenmesi
- Son okumanın silinmesi ve yeniden girilmesi
- Dönemlik tüketim ve tahmini bedel takibi
- Tüketim dönemini kapatma ve son kapatılan dönemi geri alma
- Tarife değişikliklerine ait bildirimlerin görüntülenmesi

### Yönetici işlemleri

- Abone, kullanıcı, dönem ve tarife istatistiklerinin takibi
- Son abonelerin ve sayaç okumalarının görüntülenmesi
- Tarife ekleme, düzenleme ve silme
- Kademeli su ve atık su fiyatlarının yönetilmesi
- Tarife işlemlerinden sonra otomatik bildirim oluşturulması
- Rol tabanlı yetkilendirme ile yönetici sayfalarının korunması

## Hesaplama Yapısı

Tüketim miktarı, başlangıç ve bitiş sayaç endeksleri arasındaki fark üzerinden hesaplanır. Dönem içinde birden fazla tarife yürürlüğe girdiyse tüketim, tarifelerin geçerli olduğu gün sayılarına göre paylaştırılır. Her bölüm kendi tarife kademeleri üzerinden değerlendirilir.

Tahmini toplam bedel şu kalemlerden oluşur:

- Su bedeli
- Atık su bedeli
- Çevre Temizlik Vergisi (ÇTV)
- Su KDV'si
- Atık su KDV'si

## Kullanılan Teknolojiler

- .NET 10
- ASP.NET Core MVC
- ASP.NET Core Identity
- Entity Framework Core
- SQLite
- Razor Pages ve Razor Views
- Bootstrap
- HTML, CSS ve JavaScript

## Proje Yapısı

```text
PusulaSu/
├── Areas/Identity/       # Kayıt, giriş ve hesap yönetimi
├── Controllers/          # Kullanıcı ve yönetici işlemleri
├── Data/                 # DbContext, başlangıç verileri ve migration'lar
├── Models/               # Uygulama ve ViewModel sınıfları
├── ViewComponents/       # Bildirim bileşeni
├── Views/                # MVC arayüzleri
├── wwwroot/              # CSS, JavaScript ve statik dosyalar
└── Program.cs            # Uygulama başlangıç yapılandırması
```

## Kurulum

### Gereksinimler

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Git

### Projeyi çalıştırma

```bash
git clone https://github.com/ACerenDogan/Pusula-Su.git
cd Pusula-Su
dotnet restore
dotnet run
```

Uygulama varsayılan olarak aşağıdaki adreslerden açılır:

- `https://localhost:7286`
- `http://localhost:5137`

Proje SQLite kullanır. Veritabanı dosyası `app.db`, migration dosyaları ise `Data/Migrations` klasöründe bulunur.

## Demo Bilgileri

Yeni kullanıcı hesabı oluşturmak için sistemde kayıtlı bir abone ve sayaç numarasının birlikte girilmesi gerekir. Bu sebepten var olan örnek kayıt:

| Bilgi | Değer |
|---|---|
| Abone numarası | `123456` |
| Şifre | `Ceren123!` |

Geliştirme ortamındaki varsayılan yönetici hesabı:

| Bilgi | Değer |
|---|---|
| Kullanıcı adı | `Admin` |
| Şifre | `Admin123!` |

> Varsayılan yönetici bilgileri yalnızca geliştirme ve demo amaçlıdır. Gerçek kullanım öncesinde başlangıç hesabı ve şifresi mutlaka değiştirilmelidir.

## Proje Notu

PusulaSu bir staj projesi kapsamında hazırlanmış prototiptir. Şifre yenileme işleminde abonelik bilgileriyle doğrulama yapılmaktadır. Gerçek bir kullanım senaryosunda bu adımın SMS veya e-posta üzerinden tek kullanımlık doğrulama koduyla desteklenmesi gerekir.

## Geliştirici

**Ayşe Ceren Doğan**

