using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class OyunKontrol : MonoBehaviour
{
    
    //---------------------
    GameObject secilenButon;
    GameObject butonunKendisi;
    //---------------------
    public Sprite defaultSprite;
    public AudioSource[] sesler;
    public GameObject[] Butonlar;
    public TextMeshProUGUI Sayac;
    public GameObject[] OyunSonuPaneller;
    
    //Sayaç-----------
    public float ToplamZaman = 3;
    float dakika;
    float saniye;
    bool Zamanlayici;
    // Genel Ayarlar
    public int hedefBasari;
    int ilkSecimDegeri;
    int anlikBasari;
    //----------------
    public GameObject Grid;
    public GameObject havuz;
    bool olusturmaDurumu;
    int olusturmaSayisi;
    int ToplamElemanSayisi;
    
    

    void Start()
    {
        ilkSecimDegeri = 0;
        Zamanlayici = true;
        olusturmaDurumu = true;
        olusturmaSayisi= 0;
        ToplamElemanSayisi = havuz.transform.childCount;

        StartCoroutine(Olustur());

    }

    
    public void ObjeVer(GameObject objem)
    {
        butonunKendisi = objem;

        butonunKendisi.GetComponent<Image>().sprite = butonunKendisi.GetComponentInChildren<SpriteRenderer>().sprite;
        butonunKendisi.GetComponent<Image>().raycastTarget = false;

        sesler[0].Play();
    
    }
    
    private void Update()
    {
        if (Zamanlayici && ToplamZaman>1)
        {
            ToplamZaman -= Time.deltaTime;
            dakika = Mathf.FloorToInt(ToplamZaman / 60); // 1 2 dakika = 120 saniye
            saniye = Mathf.FloorToInt(ToplamZaman % 60);// 2
                                                        //Sayac.text = Mathf.FloorToInt(ToplamZaman).ToString(); // saniye şeklinde akma
            Sayac.text = string.Format("{0:00} : {1:00}", dakika, saniye);
        }
        else
        {
            Zamanlayici=false;
            Debug.Log("Süre bitti");
            GameOver();
        }
    }
    void ButonlarinDurumu(bool durum)
    {
        foreach(var item in Butonlar)
        {
            if (item!=null)
            {
                item.GetComponent<Image>().raycastTarget = durum;
            }
            
        }
    }
    
    public void ButonTikladi(int deger)
    {
        

        Kontrol(deger);

        
    }
    public void AnaMenu()
    {
        SceneManager.LoadScene("AnaMenu");
    }
    public void TekrarOyna()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
     void GameOver()
    {
        OyunSonuPaneller[0].SetActive(true);
    }
    public void OyunuDurdur()
    {
        OyunSonuPaneller[2].SetActive(true);
        Time.timeScale = 0;
    }
    public void OyunaDevamEt()
    {
        OyunSonuPaneller[2].SetActive(false);
        Time.timeScale = 1;
    }
    void Win()
    {
        OyunSonuPaneller[1].SetActive(true);
    }
    void Kontrol(int gelendeger)
    {
        if (ilkSecimDegeri == 0)
        {
            ilkSecimDegeri = gelendeger;
            secilenButon = butonunKendisi;
        }

        else
        {
            StartCoroutine(kontrolEtBakalim(gelendeger));
        }
    }
    IEnumerator Olustur()
    {
        yield return new WaitForSeconds(.1f);

        while(olusturmaDurumu)
        {
            int rastgelesayi = Random.Range(0, havuz.transform.childCount - 1);

            if(havuz.transform.GetChild(rastgelesayi).gameObject != null)
                
            {
                havuz.transform.GetChild(rastgelesayi).transform.SetParent(Grid.transform);
                olusturmaSayisi++;

                if(olusturmaSayisi==36)
                {
                    olusturmaDurumu = false;
                    Destroy(havuz.gameObject);
                }
            }

            
        }

    }
    
    IEnumerator kontrolEtBakalim(int gelendeger)
    {
        ButonlarinDurumu(false);

        yield return new  WaitForSeconds(1);

        if (ilkSecimDegeri == gelendeger)
        {
            anlikBasari++;
            secilenButon.GetComponent<Image>().enabled = false;
            butonunKendisi.GetComponent<Image>().enabled = false;

            secilenButon.GetComponent<Button>().enabled = false;
            butonunKendisi.GetComponent<Button>().enabled = false;

            /* Destroy(secilenButon);
             Destroy(butonunKendisi.gameObject); */
            ilkSecimDegeri = 0;
            secilenButon = null;
            ButonlarinDurumu(true);
            if(hedefBasari==anlikBasari)
            {
                Win();
            }

        }
        else
        {
            sesler[1].Play();
            secilenButon.GetComponent<Image>().sprite = defaultSprite;
            butonunKendisi.GetComponent<Image>().sprite = defaultSprite;
            ilkSecimDegeri = 0;
            secilenButon = null;
            ButonlarinDurumu(true);
        }
    }
}  bu oyun kontrol dosyyası kodu  using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AnaMenuKontrol : MonoBehaviour
{
    public GameObject CikisPanel;
    public void OyundanCik()
    {
        CikisPanel.SetActive(true);
    }
    private void Start()
    {
        if(Time.time==0)
            Time.timeScale = 1;

    }
    public void Cevap(string cevap)
    {
        if (cevap == "evet")
        {
            Application.Quit();
        }
        else
        {
            CikisPanel.SetActive(false);
        }
        }
    public void OyunaBasla()
    {
        SceneManager.LoadScene(1);
       // SceneManager.LoadScene(PlayerPrefs.GetInt("KaldigiBolum"));//kaldığı yerden devam et

    }
}  bu anamenü dosyası kodu bu da ses dosyası kodu using UnityEngine;

public class SesKontrol : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private static GameObject instance;
    //çoklu level yaparsam oyuncu en son kaldığı leveli playerpref ile sisteme kayıt ederek
    //oyunkaldığı yerden devam edebilir.

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = gameObject;
        else
            Destroy(gameObject);
    }
}