// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// =====================================================
// MESKİ KURULUŞ TARİHİ SAYACI
// Rakamları sırayla çevirerek 04.05.1995 tarihine ulaştırır.
// =====================================================

document.addEventListener("DOMContentLoaded", function () {
    // Ana sayfadaki tarih sayacını buluyoruz.
    const sayac = document.getElementById("meskiTarihSayaci");

    // Sayaç bu sayfada yoksa kodu durduruyoruz.
    if (!sayac) {
        return;
    }

    // Sayacın içindeki bütün rakam kutularını seçiyoruz.
    const rakamKutulari = sayac.querySelectorAll(".counter-digit");

    function sayaciCalistir() {
        // Her çalışmada bütün kutuları yeniden sıfırlıyoruz.
        rakamKutulari.forEach(function (kutu) {
            kutu.textContent = "0";
            kutu.classList.remove("is-flipping");
        });

        rakamKutulari.forEach(function (kutu, kutuSirasi) {
            // HTML içindeki data-target değerini alıyoruz.
            const hedefRakam = Number(kutu.dataset.target);

            // Kutular aynı anda değil, sırayla çalışmaya başlar.
            setTimeout(function () {
                let mevcutRakam = 0;
                let adim = 0;

                // Bir tam tur attıktan sonra hedef rakama ulaşır.
                const toplamAdim = 10 + hedefRakam;

                const cevirmeIslemi = setInterval(function () {
                    mevcutRakam = (mevcutRakam + 1) % 10;
                    kutu.textContent = mevcutRakam;

                    // CSS animasyonunu her rakam değişiminde yeniden çalıştırır.
                    kutu.classList.remove("is-flipping");
                    void kutu.offsetWidth;
                    kutu.classList.add("is-flipping");

                    adim++;

                    if (adim >= toplamAdim) {
                        clearInterval(cevirmeIslemi);
                        kutu.textContent = hedefRakam;
                    }
                }, 150);

            }, kutuSirasi * 240);
        });
    }

    // Sayfa açıldığında ilk animasyonu çalıştırır.
    sayaciCalistir();

    // Animasyonu 12 saniyede bir tekrarlar.
    setInterval(sayaciCalistir, 12000);
});
document.addEventListener("DOMContentLoaded", function () {
    const endeksInput = document.getElementById("endeks");
    const sayacEkrani = document.getElementById("canliSayacEndeksi");

    if (!endeksInput || !sayacEkrani) {
        return;
    }

    const rakamKutulari =
        sayacEkrani.querySelectorAll(".sayac-rakam");

    const baslangicDegeri =
        sayacEkrani.dataset.baslangic || "------";

    endeksInput.addEventListener("input", function () {
        // Ondalık bölümünü ayırır ve yalnızca rakamları alır.
        const tamKisim = endeksInput.value
            .split(/[.,]/)[0]
            .replace(/\D/g, "");

        // Alan boşaltılırsa son kayıtlı değere döner.
        const gosterilecekDeger = tamKisim
            ? tamKisim.padStart(6, "0").slice(-6)
            : baslangicDegeri;

        rakamKutulari.forEach(function (kutu, index) {
            if (kutu.textContent !== gosterilecekDeger[index]) {
                kutu.textContent = gosterilecekDeger[index];

                // Animasyonu yeniden çalıştırır.
                kutu.classList.remove("is-changing");
                void kutu.offsetWidth;
                kutu.classList.add("is-changing");
            }
        });
    });
});