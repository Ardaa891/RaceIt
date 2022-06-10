# if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SkyGenerator))]
public class Editor_SkyGenerator : Editor
{
    bool Res_1024,Res_2048,Res_4096,Res_8192;
    // Styles
    bool styleInit;
    GUIStyle greenTextStyle;
    GUIStyle resolutionButtonStyle;
    GUIStyle refreshButtonStyle;
    GUIStyle layerNameStyle;
    static GUIStyle ConsoleFieldStyle;
    List<bool> LayerMenus;
    static bool init=false;
    public static string consoleText;
    SkyGenerator skyGenerator;
    Color defaultColor;

    private void OnEnable()
    {
        skyGenerator=(SkyGenerator) target;
        SkyGenerator.textureSaved.AddListener(ConsoleTextForSave);
        styleInit=false;
        if(init==false){
            init=true;
            Res_1024=false;
            Res_2048=true;
            Res_4096=false;
            Res_8192=false;
            defaultColor=new Color(1,1,1,0);
            consoleText="Sky Generator v:1.0";
            LayerMenus=new List<bool>();
            
            SkyGenerator.resolutionFactor=2;
            SkyGenerator.skyGradient=new Gradient();
            SkyGenerator.layerNumbers=0;
            SkyGenerator.mixFactors=new List<float>();
            SkyGenerator.textureRotations=new List<int>();
            SkyGenerator.yPositions=new List<int>();
            SkyGenerator.layerTextures=new List<Texture2D>();
            SkyGenerator.layerColors=new List<Color>();
        }
    }
    private void OnDisable()
    {
        SkyGenerator.textureSaved.RemoveListener(ConsoleTextForSave);
    }

    public override void OnInspectorGUI()
    {   
        GenerateGUIStyle();
        
        // Console Field
        if(consoleText==null){consoleText="Sky Generator v:1.0";}
        GUILayout.TextArea(consoleText,ConsoleFieldStyle);
        GUILayout.Space(7);

        // Texture Resolution------------------------
        GUILayout.Label("Sky Texture Resolution");

        GUILayout.BeginHorizontal();
        if(GUILayout.Toggle(Res_1024, "1024x512", resolutionButtonStyle)){
            EnableAllResolution();
            Res_1024=true;
            SkyGenerator.resolutionFactor=8;
            skyGenerator.SetResolution(1024,512);
        }
        if(GUILayout.Toggle(Res_2048, "2048x1024", resolutionButtonStyle)){
            EnableAllResolution();
            Res_2048=true;
            SkyGenerator.resolutionFactor=4;
            skyGenerator.SetResolution(2048,1024);

        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if(GUILayout.Toggle(Res_4096, "4096x2048", resolutionButtonStyle)){
            EnableAllResolution();
            Res_4096=true;
            SkyGenerator.resolutionFactor=2;
            //SkyGenerator.RecalculateYPositions();
            skyGenerator.SetResolution(4096,2048);
        }
        if(GUILayout.Toggle(Res_8192, "8192x4096", resolutionButtonStyle)){
            EnableAllResolution();
            Res_8192=true;
            SkyGenerator.resolutionFactor=1;
            //SkyGenerator.RecalculateYPositions();
            skyGenerator.SetResolution(8192,4096);
        }
        GUILayout.EndHorizontal();
        //-------------------------------------------

        // Generate Sky Texture ------------------
        EditorGUILayout.Space(10);
        if(GUILayout.Button("Generate Sky Texture",greenTextStyle)){
            TextureControl();
        }
        // ---------------------------------------

        EditorGUILayout.Space(10);
        
        // Layer Add and Delete 
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Add Layer")){
            SkyGenerator.layerNumbers++;
        }
        if(GUILayout.Button("Remove Layer")){
            if(SkyGenerator.layerNumbers>0){
                SkyGenerator.layerNumbers--;
            }
        }
        GUILayout.EndHorizontal();
        // -----------------------------

        //GUILayout.
        // Sky Gradient --------------------
        EditorGUILayout.Space();
        GUILayout.Label("Background Gradient Colors");
        SkyGenerator.skyGradient=(Gradient)EditorGUILayout.GradientField(SkyGenerator.skyGradient);
        // ---------------------------------

        Control();

        // Generate Layer Menus -----------
        for(int i=0;i<SkyGenerator.layerNumbers;i++){
            // Layer isimleri
            string LayerName="LAYER "+(i+1).ToString();
            if(SkyGenerator.layerTextures[i]!=null){
                LayerName="LAYER "+(i+1).ToString()+ " ("+SkyGenerator.layerTextures[i].name+")";
            }

            EditorGUILayout.Space(8);
            LayerMenus[i]=EditorGUILayout.Foldout(LayerMenus[i],LayerName,true,layerNameStyle);
            
            GUILayout.BeginVertical();
            if(LayerMenus[i]==true){
                EditorGUILayout.Space(15);

                GUILayout.BeginHorizontal();
                Texture_and_ColorField(i);
                EditorGUILayout.Space();
                GUILayout.BeginVertical();
                GUILayout.Label("");
                if(GUILayout.Button("Refresh",refreshButtonStyle)){
                    TextureControl();
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();

                EditorGUILayout.Space(8);

                GUILayout.Label("Mix Factor");
                GUILayout.BeginHorizontal();
                SkyGenerator.mixFactors[i]=EditorGUILayout.Slider(SkyGenerator.mixFactors[i],0f,1f);
                if(GUILayout.Button("-")){
                   if(SkyGenerator.mixFactors[i]>0.01f){
                        SkyGenerator.mixFactors[i]-=0.01f;
                    }
                }
                if(GUILayout.Button("+")){
                    if(SkyGenerator.mixFactors[i]<0.99f){
                        SkyGenerator.mixFactors[i]+=0.01f;
                    }
                }
                GUILayout.EndHorizontal();
                EditorGUILayout.Space(1);

                GUILayout.Label("Texture Rotation");
                GUILayout.BeginHorizontal();
                SkyGenerator.textureRotations[i]=EditorGUILayout.IntSlider(SkyGenerator.textureRotations[i],0,360);
                if(GUILayout.Button("-")){
                   if(SkyGenerator.textureRotations[i]>0){
                        SkyGenerator.textureRotations[i]-=1;
                    }
                }
                if(GUILayout.Button("+")){
                    if(SkyGenerator.textureRotations[i]<360){
                        SkyGenerator.textureRotations[i]+=1;
                    }
                }
                GUILayout.EndHorizontal();

                EditorGUILayout.Space(1);

                GUILayout.Label("Y Position (-2048=0 , 2048="+skyGenerator.yResolution+")");
                GUILayout.BeginHorizontal();
                SkyGenerator.yPositions[i]=EditorGUILayout.IntSlider(SkyGenerator.yPositions[i],-2048,2048);
                
                if(GUILayout.Button("-")){
                    if(SkyGenerator.yPositions[i]>-2048){
                        SkyGenerator.yPositions[i]-=1;
                    }
                }
                if(GUILayout.Button("+")){
                    if(SkyGenerator.yPositions[i]<2048){
                        SkyGenerator.yPositions[i]+=1;
                    }
                }
                GUILayout.EndHorizontal();


            }
            GUILayout.EndVertical();
        }
        // ---------------------------
        
        //base.OnInspectorGUI();

    }
    void TextureControl(){
        if(SkyGenerator.layerTextures.Count<SkyGenerator.layerNumbers){
            ConsoleFieldStyle.normal.textColor=new Color(1f,0.4f,0.4f,1);
            consoleText="Please set all texture fields!";
        }
        if(SkyGenerator.layerTextures.Count==SkyGenerator.layerNumbers){
            bool nullCheck=true;
            for(int i=0;i<SkyGenerator.layerTextures.Count;i++){
                if(SkyGenerator.layerTextures[i]==null){nullCheck=false;}
            }
            if(nullCheck==true){
                ConsoleFieldStyle.normal.textColor=Color.white;
                consoleText="Please Wait! Sky Texture Generating...";
                skyGenerator.GenerateTexture();
            }
            else{
                ConsoleFieldStyle.normal.textColor=new Color(1f,0.4f,0.4f,1);
                consoleText="Please set all texture fields!";}
        }
    }
    public static void ConsoleTextForSave(){
        ConsoleFieldStyle.normal.textColor=new Color(0.3f,0.8f,0.2f,1);
        string saveText=SkyGenerator.FilePath();
        consoleText=saveText;
    }

    void EnableAllResolution(){
        Res_1024=false;
        Res_2048=false;
        Res_4096=false;
        Res_8192=false;
    }

    void GenerateGUIStyle(){
        if(styleInit==false){
            Color greenColor=new Color(0.3f,1f,0.5f,1);
            styleInit=true;
            // Generate Texture Button
            greenTextStyle=new GUIStyle(GUI.skin.button);
            greenTextStyle.normal.textColor = greenColor;
            greenTextStyle.alignment=TextAnchor.MiddleCenter;
            greenTextStyle.fontStyle = FontStyle.Bold;
            // Refresh Button
            refreshButtonStyle=new GUIStyle(GUI.skin.button);
            refreshButtonStyle.alignment=TextAnchor.MiddleCenter;
            refreshButtonStyle.fontStyle = FontStyle.Bold;
            refreshButtonStyle.fixedWidth=100; 
            refreshButtonStyle.fixedHeight=64;
            // layerNameStyle
            layerNameStyle=new GUIStyle(EditorStyles.foldout);
            layerNameStyle.normal.textColor = new Color(0.5f,0.7f,1,1);
            layerNameStyle.fontSize=12;
            layerNameStyle.fontStyle = FontStyle.Bold;
            layerNameStyle.onNormal.textColor = greenColor;
            // Resolution button styles
            resolutionButtonStyle = new GUIStyle(GUI.skin.button);
            resolutionButtonStyle.onNormal.textColor = greenColor;
            resolutionButtonStyle.fontStyle = FontStyle.Bold;
            // Console Field for Debug.Log
            ConsoleFieldStyle=new GUIStyle(GUI.skin.label);
            ConsoleFieldStyle.alignment=TextAnchor.MiddleLeft;
            ConsoleFieldStyle.wordWrap=true;
        }
    }

    private void Texture_and_ColorField(int indexNumber)
    {   
        GUILayout.BeginVertical();
        GUILayout.Label("Layer Texture");
        SkyGenerator.layerTextures[indexNumber] = (Texture2D)EditorGUILayout.ObjectField(SkyGenerator.layerTextures[indexNumber], typeof(Texture2D), false, GUILayout.Width(128), GUILayout.Height(64));
        GUILayout.EndVertical();

        EditorGUILayout.Space();

        GUILayout.BeginVertical();
        GUILayout.Label("Mix Color");
        SkyGenerator.layerColors[indexNumber] = (Color)EditorGUILayout.ColorField(SkyGenerator.layerColors[indexNumber],GUILayout.Width(128), GUILayout.Height(64));
        GUILayout.EndVertical();
    }

    void Control(){
        int controlNumber=0;
        int layerNumbers=SkyGenerator.layerNumbers;
        for(int control=0;control<30;control++){
            // LAYER MENU CONTROL
            if(LayerMenus==null){LayerMenus=new List<bool>();}
            if(LayerMenus.Count!=layerNumbers){
                if(LayerMenus.Count>layerNumbers){
                    LayerMenus.RemoveRange(layerNumbers,LayerMenus.Count-layerNumbers);
                }
                else{LayerMenus.Add(false);}
            }else{controlNumber=1;}

            // LAYER TEXTURES CONTROL
            if(SkyGenerator.layerTextures==null){SkyGenerator.layerTextures=new List<Texture2D>();}
            if(SkyGenerator.layerTextures.Count!=layerNumbers){
                if(SkyGenerator.layerTextures.Count>layerNumbers){
                    SkyGenerator.layerTextures.RemoveRange(layerNumbers,SkyGenerator.layerTextures.Count-layerNumbers);
                }
                else{SkyGenerator.layerTextures.Add(null);}
            }else{if(controlNumber==1){controlNumber=2;}}

            // LAYER COLORS CONTROL
            if(SkyGenerator.layerColors==null){SkyGenerator.layerColors=new List<Color>();}
            if(SkyGenerator.layerColors.Count!=layerNumbers){
                if(SkyGenerator.layerColors.Count>layerNumbers){
                    SkyGenerator.layerColors.RemoveRange(layerNumbers,SkyGenerator.layerColors.Count-layerNumbers);
                }
                else{SkyGenerator.layerColors.Add(defaultColor);}
            }else{if(controlNumber==2){controlNumber=3;}}

            // MIX FACTOR CONTROL
            if(SkyGenerator.mixFactors.Count!=layerNumbers){
                if(SkyGenerator.mixFactors.Count>layerNumbers){
                    SkyGenerator.mixFactors.RemoveRange(layerNumbers,SkyGenerator.mixFactors.Count-layerNumbers);
                }
                else{SkyGenerator.mixFactors.Add(1);}
            }else{if(controlNumber==3){controlNumber=4;}}

            // TEXTURE ROTATIONS CONTROL
            if(SkyGenerator.textureRotations.Count!=layerNumbers){
                if(SkyGenerator.textureRotations.Count>layerNumbers){
                    SkyGenerator.textureRotations.RemoveRange(layerNumbers,SkyGenerator.textureRotations.Count-layerNumbers);
                }
                else{SkyGenerator.textureRotations.Add(0);}
            }else{if(controlNumber==4){controlNumber=5;}}

            // Y_POSITIONS CONTROL
            if(SkyGenerator.yPositions.Count!=layerNumbers){
                if(SkyGenerator.yPositions.Count>layerNumbers){
                    SkyGenerator.yPositions.RemoveRange(layerNumbers,SkyGenerator.yPositions.Count-layerNumbers);
                }
                else{SkyGenerator.yPositions.Add(0);}
            }else{if(controlNumber==5){controlNumber=6;}}

            if(controlNumber==6){control=100;}
        }
    }
}
#endif
