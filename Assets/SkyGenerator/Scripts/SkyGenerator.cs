// Sky Generator v1.0 by Hami Cihat SERTCAN
// 05.2021
// ----------------------------------------

# if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkyGenerator : MonoBehaviour
{
    public static UnityEvent textureSaved=new UnityEvent();
    public static Gradient skyGradient;
    public int xResolution=2048;
    public int yResolution=1024;
    public static int resolutionFactor;//çözünürlük farklılıklarında pixel seçimi için kullanılıyor
    public Texture2D skyTexture;

    Color backgroundColor,selectedLayerColor,finalColor;

    int xPixel,yPixel;//katmanlardan konum bilgilerine göre pixel seçilirken kullanılıyorlar
    public static int layerNumbers;//katman sayısı
    public static List<float> mixFactors;//katmanların alttaki katmanla karışım miktarları
    public static List<int> textureRotations;//katmanların rotasyonlar
    public static List<int> yPositions;//katmanların y kaçıklıkları
    List<int> calculatedYPositions,maxYPositions;
    public static List<Texture2D> layerTextures;//katman listesi
    public static List<Color> layerColors;// katmanlar ile karıştırılacak seçilebilir renklerin listesi
    Color mixColor;

    List<Color> layerPixelColorList;// her katman işlendikten sonra bu listeye ilgili pixelin rengi ekleniyor

    public static string textureName="SkyTexture";
    static byte[] bytes ;
    int layerStartPixel;
    // layerların renk karışımlarının hangi y piksel aralığında olacağını bulmak için kullanılıyor
    int minY,maxY;
    Color emptyColor;
    public void GenerateTexture(){
        emptyColor=new Color(0,0,0,0);
        calculatedYPositions=new List<int>();
        maxYPositions=new List<int>();
        if(yPositions!=null){
            for(int i=0;i<yPositions.Count;i++){
                // kullanıcının girdiği değerler -512 ile 512 aralığında değişiyor bu değerler
                // yResolution değerine göre 0 ile yResolution değeri arasında bir değere dönüştürülüyor
                int calculatedValue=Mathf.RoundToInt(yResolution*0.5f+yPositions[i]/resolutionFactor);
                calculatedYPositions.Add(calculatedValue);
                maxYPositions.Add(Mathf.RoundToInt(calculatedValue+yResolution*0.5f));
            }
        }
        minY=Mathf.RoundToInt(yResolution*0.5f);
        maxY=yResolution;
        // katman renk karışımı yapılacak maksimum ve minimum pixel aralığı bulunuyor
        if(yPositions!=null){
            minY=Mathf.Min(calculatedYPositions.ToArray());
            maxY=Mathf.Max(maxYPositions.ToArray());
            if(minY<0){minY=0;}
            if(maxY>yResolution){maxY=yResolution;}
        }
        skyTexture = new Texture2D(xResolution,yResolution);
        // Doku oluşturuluyor
        for(int x=0; x<xResolution;x++){
            for (int y = 0; y < yResolution; y++)
            {
                // katman renkleri için liste oluşturuluyor
                layerPixelColorList=new List<Color>();
                // arkaplan için skyGradient kullanılarak renk seçiliyor.
                backgroundColor=skyGradient.Evaluate((float)y / (float)yResolution);
                // layer sayısı 0 dan büyükse ve
                // katman renk karışımı için ilgili pixele gelinmişse (minY) ve
                // katman renk karışımı için son pixelden önceki bir pixelde işlem yapılıyorsa
                if(layerNumbers>0 && y>minY && y < maxY){
                    // bütün katmanlar için renk seçimi ve renklerin karışımları yapılıyor
                    for(int i=0; i<layerNumbers;i++){
                        selectedLayerColor=SelectPixel(x,y,i);
                        MixWithLayerColor(selectedLayerColor,i);
                    }
                    Color finalColor=FinalMix();
                    skyTexture.SetPixel(x, y,finalColor);
                }
                // başlangıç ve bitiş pixelleri arasında değilsek veya katman sayısı 0 sa gradient renk pixele atanıyor
                else{
                    skyTexture.SetPixel(x, y,backgroundColor);
                }
            }
        }
        // hazırlanan doku kaydediliyor
        SaveTexture(skyTexture);
        skyTexture.Apply(false);
    }

    // katman için belirlenen rotasyon ve y kaçıklığı bilgilerine göre o katmandan renk seçiliyor.
    Color SelectPixel(int x, int y,int layerIndex){
        // layerIndex e göre ilgili katmanın y pozisyonu kontrolü yapılıyor
        // eğer ilgili katman işlem yapıacak y pixel konumunda y poziyon bilgisine göre renk vermeyecekse
        // renk olarak emptyColor seçiliyor
        if(y-calculatedYPositions[layerIndex]>=0){
            yPixel=(y-calculatedYPositions[layerIndex])*resolutionFactor;
            if(yPixel<2048){
                // X pikseli seçiliyor
                xPixel=x+Mathf.RoundToInt(textureRotations[layerIndex]*xResolution/360);
                if(xPixel>xResolution){xPixel=xPixel-xResolution;}
                xPixel=xPixel*resolutionFactor;
                return layerTextures[layerIndex].GetPixel(xPixel,yPixel);
            }  
            else{
                return emptyColor;
            }  
        }
        else{
           return emptyColor; 
        }
    }
    
    // Belirlenen karışım rengi ile daha önce seçilen pixel konumuna denk gelen renk karıştırılıyor.
    // ve layerPixelColorList isimli listeye ekleniyor
    void MixWithLayerColor(Color selected_Layer_Color, int layerIndex){
        float mixRatio=layerColors[layerIndex].a; //karışım karıştırılacak rengin alfasına göre belirleniyor.
        Color mixedColor=selected_Layer_Color*(1-mixRatio)+layerColors[layerIndex]*mixRatio;
        mixedColor.a=selected_Layer_Color.a;
        layerPixelColorList.Add(mixedColor);
    }

    Color FinalMix(){
        mixColor=backgroundColor;
        for(int c=0;c<layerNumbers;c++){
            float layerAlpha=layerPixelColorList[c].a;
            float mixFactor=layerAlpha-(1-mixFactors[c]);
            if(mixFactor<0){mixFactor=0;}
            mixColor=mixColor*(1-mixFactor)+layerPixelColorList[c]*mixFactor;
        }
        return mixColor;
    }
    private void SaveTexture(Texture2D texture)
    {
        var dirPath =Application.dataPath+"/SkyGenerator/GeneratedSky/";
        bytes = texture.EncodeToPNG();
        // Eğer png kayıt yapılacak yer yoksa oluşturuluyor
        if (!System.IO.Directory.Exists(dirPath))
        {
            System.IO.Directory.CreateDirectory(dirPath);
        }
        System.IO.File.WriteAllBytes(dirPath+textureName+".png" , bytes);
        #if UNITY_EDITOR
                UnityEditor.AssetDatabase.Refresh();
        #endif
        textureSaved.Invoke();
    }

    public void SetResolution(int resX,int resY){
        xResolution=resX;
        yResolution=resY;
    }

    public static string FilePath(){
        return "Sky Texture ("+bytes.Length / 1024+" Kb)  saved to:\n" +Application.dataPath+"/SkyGenerator/GeneratedSky/SkyTexture.png";
    }
}
#endif