# Restoran Modülü 🍽️

Bu proje, küçük ve orta ölçekli restoran ve kafe işletmelerinde sipariş ve masa takibini kolaylaştırmak amacıyla geliştirilmiş masaüstü bir uygulamadır. Kullanıcı dostu arayüzü sayesinde masaların yönetimi, ürün ekleme/düzenleme ve sipariş alma işlemleri basit ve hızlı bir şekilde yapılabilir.

## 📌 Genel Özellikler

- Masa takibi (boş/dolu durumları ve seçili masa üzerinde işlem)
- Ürün ve kategori yönetimi (ekleme, düzenleme, silme)
- Masa bazlı sipariş alma ve güncelleme
- Sipariş detaylarının görüntülenmesi ve hesap kapatma işlemi
- Görsel olarak sade, kolay kullanımlı bir Windows Forms arayüzü

## 🧰 Kullanılan Teknolojiler

- C# (.NET Framework)
- Windows Forms (WinForms)
- MySQL (veritabanı)
- ADO.NET (veritabanı işlemleri için)

## 🖼️ Uygulama Görselleri

**Katlar**  

![Kat Ekranı](https://github.com/user-attachments/assets/be2a0ca8-7368-4510-a065-cc121aebf713)

**Masa Detayı**

![Masa Detay Ekranı](https://github.com/user-attachments/assets/d5735123-9c4a-46e4-b249-92e269a869b0)

**Ürün Ekleme**  

![Ürün Seçme Ekranı](https://github.com/user-attachments/assets/8cb34813-69e9-4119-a0dd-bff4deaf33ff)

**Ödeme Ekranı**

![Ödeme Ekranı](https://github.com/user-attachments/assets/0531e5b4-9329-49bb-914d-ff784a78cb4f)

**Mutfak Ekranı**

![Mutfak Ekranı](https://github.com/user-attachments/assets/119697ce-dff3-41fa-933f-c9b099802459)

### Admin Ekranları

**Kat Ekranları**

![Admin Kat Ekranı](https://github.com/user-attachments/assets/fd77cc0e-47f4-4ef4-b69a-3ab325bcbb96)

**Rol ve Kullanıcı Ekranları**

![Admin Kullanıcı Ekranı](https://github.com/user-attachments/assets/852dfacc-aaa9-4e52-aeee-d2547c049ee9)

**Masa Ekranı**

![Admin Masa Ekranı](https://github.com/user-attachments/assets/0e87e23f-f640-4521-8348-6c1239485623)

**Menü Ekranları**

![Admin Menü Ekranı](https://github.com/user-attachments/assets/08546cc9-3e1d-450b-adf8-fdc20faafe27)

**Müşteri Ekranı**

![Admin Müşteri Ekranı](https://github.com/user-attachments/assets/d0f8787a-c33b-409f-ab84-a0816da62168)

**Sipariş ve Sipariş Detay Ekranları**

![Admin Sipariş Ekranı](https://github.com/user-attachments/assets/3aa93e71-2efd-44ff-bdf6-9b12c8f0c546)

**Kategori ve Ürünler Ekranı**

![Admin Ürün Ekranı](https://github.com/user-attachments/assets/1359491b-fd1e-45d7-aefc-c6acc13fede0)

## 🔧 Kurulum ve Kullanım

1. Projeyi GitHub üzerinden klonlayın:
```bash
git clone https://github.com/falcon3691/RestoranModulu.git
````
2. Uygulamayı kkullanabilmek için bilgisayarınızda MySQL yüklü olmalıdır. Veri tabanını kendi bilgisayarınızda oluşturmak için repo içerisinde bulunan "RestoranModulu_MySql.sql" dosyasını çalıştırabilirsiniz.
3. Visual Studio ile RestoranModulu.sln dosyasını açın.

4. Projedeki veritabanı bağlantı cümleleri .cs dosyalarında doğrudan tanımlanmıştır. Şu şekilde örnek bir bağlantı mevcuttur:
````csharp
public string baglantiKodu = "Server=localhost;Database=restoranmodulu;Uid=root;Pwd=Malukat3691.;";
````

### Veritabanı ER diyagramı:

![Restoran Modülü Veri Tabanı ER Diyagramı - 2](https://github.com/user-attachments/assets/b963333f-d2eb-4d8a-ade2-f24bb7ab5422)
