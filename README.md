# 🚗 Unity Autopeli

Pienimuotoinen ajopeli, jossa voit ohjata autoa ja säätää sen teknisiä ominaisuuksia, kuten jousitusta ja ohjautuvuutta. Tulevaisuudessa peliin lisätään useampia ajoneuvoja sekä tuning-ominaisuuksia.

---

## 🔧 Ominaisuudet

- Reaaliaikainen ohjaus: kaasu, jarru, peruutus, käsijarru
- Pehmeä jarrutus ja ohjaus (smooth steering & braking)
- Säädettävä jousitus ja renkaiden kitka
- Dynaaminen moottoriääni nopeuden mukaan
- Visuaalisesti synkronoidut pyörät ja fysiikka

---

## ⚙️ Tekniikka

- **Moottori & jarrutus**: `WheelCollider` + `Rigidbody`
- **Jousitus**: konfiguroitavissa `JointSpring`-arvoilla
- **Äänet**: `AudioSource` säätää pitchiä nopeuden mukaan
- **Ohjaus**: sujuva `Mathf.SmoothDamp` ohjausvaste

---

## 🧪 Testaus

- Käytössä yksi testiauto
- Eri jousitus- ja kitka-arvojen kokeilua
- Tavoitteena realistinen ja/tai viihdyttävä ajokokemus

---

## 🚧 Suunnitteilla

- ✅ Useita valittavia autoja
- 🔧 Auton tuning (moottori, väri, jouset, vanteet)
- 🏁 Pelimuodot (aika-ajot, drift-kisat)
- 💾 Tallennus & lataus auton asetuksille
- 🖥️ UI säätövalikoille

---

## 📁 Kansion rakenne
