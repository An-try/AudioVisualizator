using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundVisualize : MonoBehaviour
{
    public static SoundVisualize instance;

    public AudioSource audioSource;

    public ParticleSystem _particleSystem;
    
    private GameObject VisualizatorBackground;
    private Vector3 defaultVisualizatorBackgroundPosition = Vector3.zero;
    private Vector3 newTargetBackgroundPosition = Vector3.zero;
    private Quaternion defaultVisualizatorBackgroundRotation = new Quaternion(0, 0, 0, 0);
    public Quaternion backgroundNewRotation = new Quaternion(0, 0, 0, 0);
    public float maxBackgroundNewRotation = 1;

    private GameObject VisualizatorAnimeBackground;
    private Vector3 defaultVisualizatorAnimeBackgroundPosition = Vector3.zero;
    private Vector3 newAnimeBackgroundPosition = Vector3.zero;

    public GameObject ButtonsBlocksCanvas;
    public GameObject ButtonBlockPrefab;

    public float rmsValue;
    public float dbValue;
    public float pitchValue;
    private float visualizerRotateIntencity;
    private float rotateIntencity = 0f;
    private int rotationDirection = 1;

    public float positionVisualizingIntencity = 10f;
    private float currentVisualizerScale = 0.1f;
    public float maxVisualizerScale = 1f;
    public float visualModifier = 50f;
    public float smoothSpeed = 10f;
    public float maxVisualScale = 60f;
    public float keepPercentage = 0.5f;
    public float maxPositionVisualizing = 50f;
    public float circleRotationMultiply = 10f;
    public float circleBigRotationMultiply = 100f;
    
    public Vector3 visualCubeDefaultScale = new Vector3(1f, 1f, 1f);

    public float circleBigRotationTime = 3f;
    private float currentCircleBigRotationTime;

    private float circleRotationForce = 0f;
    private float circleBigRotationForce = 0f;

    private Color32 currentParticleColor = new Color32(255, 255, 255, 255);

    public bool visualiserVisible = true;
    public bool visualizeByCircleRotation = true;
    public bool visualizeByCircleBigRotation = true;
    public bool visualizeByPosition = true;
    public bool visualizeByRotation = true;
    public bool visualizeByScale = true;
    public bool visualizeByBackground = true;
    public bool visualizeByParticles = true;
    public bool particlesRainbowColor = true;

    public float circleRadius = 10f;

    public bool Red_Yellow = false;
    public bool Yellow_Green = false;
    public bool Red_Green = false;
    public bool Rainbow = false;

    private int sampleSize = 1024;

    private int averageSize;

    private float sampleRate;

    private float[] samples;
    private float[] spectrum;

    private Transform[] visualList;
    private float[] visualScale;
    private int amountVisual = 64;

    private List<Vector3> visualsDefaultPositions = new List<Vector3>();

    private string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private bool[] visualScalesCanCreateButtonBlock;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        VisualizatorBackground = Visualizator.instance.Background;
        defaultVisualizatorBackgroundPosition = VisualizatorBackground.transform.position;
        defaultVisualizatorBackgroundRotation = VisualizatorBackground.transform.localRotation;

        VisualizatorAnimeBackground = Visualizator.instance.AnimeBackground;
        defaultVisualizatorAnimeBackgroundPosition = VisualizatorAnimeBackground.transform.position;

        audioSource = Visualizator.instance.GetComponent<AudioSource>();
        samples = new float[sampleSize];
        spectrum = new float[sampleSize];
        sampleRate = AudioSettings.outputSampleRate;
        averageSize = (int)(sampleSize * keepPercentage / amountVisual);

        currentCircleBigRotationTime = circleBigRotationTime;

        CreateRainbowMaterials();

        SpawnVisualizeCircle();

        visualScalesCanCreateButtonBlock = new bool[visualScale.Length / 2];
        for (int i = 0; i < visualScalesCanCreateButtonBlock.Length; i++)
        {
            visualScalesCanCreateButtonBlock[i] = true;
        }

        InvokeRepeating("ChangeRotationDirection", 10f, 10f);
        InvokeRepeating("SetCircleBigRotationTime", 0f, 25f);
        InvokeRepeating("ChangeNewTargetBackgroundPosition", 0f, 1f);
        //InvokeRepeating("NullMaxBackgroundNewRotation", 1f, 1f);
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
            return;
        }
        else
        {
            if (!audioSource.isPlaying && audioSource.clip != null)
            {
                audioSource.UnPause();
                if (!audioSource.isPlaying)
                {
                    // TODO: Choose a new random audio clip
                    //audioSource.clip = Visualizator.instance.audioClips[Random.Range(0, Visualizator.instance.audioClips.Count)];
                    audioSource.Play();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        AnalyzeSound();
        VisualizeSound();
        SetVisualizerRotateIntencity();
        RotateVisualizer();
        CircleBigRotationTimeDecrease();
        VisualizeParticles();

        //CreateButtonBlocks();

        if (visualizeByRotation)
        {
            RotateVisualizerObjects();
        }

        if (visualizeByBackground)
        {
            AnimeBackgroundZomming();
            BackgroundSetShakeRotation();
            BackgroundShake();
        }

        if (currentVisualizerScale < maxVisualizerScale)
        {
            transform.localScale = new Vector3(currentVisualizerScale, currentVisualizerScale, currentVisualizerScale);
            currentVisualizerScale += Time.deltaTime * 2;
        }
    }

    private void CreateRainbowMaterials()
    {
        Manager.instance.RainbowMaterials = new Material[amountVisual];

        for (int i = 0; i < amountVisual; i++)
        {
            byte colorR = (byte)Random.Range(0, 256);
            byte colorG = (byte)Random.Range(0, 256);
            byte colorB = (byte)Random.Range(0, 256);
            byte colorA = 255;

            Material material = new Material(Manager.instance.visualiseCubeMaterial);
            material.color = new Color32(colorR, colorG, colorB, colorA);

            Manager.instance.RainbowMaterials[i] = material;
        }
    }

    private void SpawnVisualizeCircle()
    {
        visualScale = new float[amountVisual];
        visualList = new Transform[amountVisual];

        for(int i = 0; i < amountVisual; i++)
        {
            float angle = i * 1f / amountVisual;
            angle = angle * Mathf.PI * 2;

            float x = transform.position.x + Mathf.Cos(angle) * circleRadius;
            float z = transform.position.z + Mathf.Sin(angle) * circleRadius;

            Vector3 position = new Vector3(x, 0f, z);

            GameObject visualGameObject = SpawnVisual(i, position);
            visualGameObject.transform.rotation = Quaternion.LookRotation(Vector3.up, position);

            visualList[i] = visualGameObject.transform;

            visualsDefaultPositions.Add(visualList[i].localPosition);
        }
        transform.localScale *= maxVisualizerScale;

        SetVisualsToDefaultPosition();
        SetVisualsToDefaultScale();
    }

    private GameObject SpawnVisual(int i, Vector3 position)
    {
        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube) as GameObject;

        gameObject.transform.SetParent(transform);
        gameObject.transform.position = position;
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z * 5f);
        gameObject.AddComponent<CubeChangeColor>();
        gameObject.GetComponent<MeshRenderer>().material = Manager.instance.RainbowMaterials[i];
        if (gameObject.GetComponent<BoxCollider>() != null)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }

        return gameObject;
    }

    private void SetVisualizerRotateIntencity()
    {
        visualizerRotateIntencity -= Time.deltaTime * smoothSpeed;
        if (visualizerRotateIntencity < dbValue)
        {
            visualizerRotateIntencity = dbValue;
        }
    }

    private void SetCircleBigRotationTime()
    {
        if (visualizeByCircleBigRotation)
        {
            currentCircleBigRotationTime = circleBigRotationTime;
        }
    }

    private void CircleBigRotationTimeDecrease()
    {
        if(currentCircleBigRotationTime > 0)
        {
            circleBigRotationForce = Time.deltaTime * circleBigRotationMultiply;
            currentCircleBigRotationTime -= Time.deltaTime;
        }
        else
        {
            circleBigRotationForce = 0f;
            currentCircleBigRotationTime = 0f;
        }
    }

    private void RotateVisualizer()
    {
        circleRotationForce = 0f;

        if (visualizeByCircleRotation)
        {
            circleRotationForce = circleRotationMultiply;
        }
        if (circleBigRotationForce == 0)
        {
            if (transform.rotation.x != 0f || transform.rotation.z != 0f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w), Time.deltaTime);
            }
        }
        float defaultAdditionalIntencity = 100f;
        
        rotateIntencity = (defaultAdditionalIntencity + visualizerRotateIntencity +
                                                                                    visualList[1].localScale.y * 5f +
                                                                                    visualList[2].localScale.y * 5f +
                                                                                    visualList[3].localScale.y * 5f +
                                                                                    visualList[4].localScale.y * 5f +
                                                                                    visualList[5].localScale.y * 5f) / 10000f;
        rotateIntencity = Mathf.Clamp(rotateIntencity, 0f, Mathf.Infinity);
        transform.Rotate(circleBigRotationForce, rotateIntencity * circleRotationForce, circleBigRotationForce);
    }

    private void RotateVisualizerObjects()
    {
        float defaultAdditionalIntencity = 100f;

        rotateIntencity = (defaultAdditionalIntencity + visualizerRotateIntencity +
                                                                                    visualList[21].localScale.y * 50f +
                                                                                    visualList[22].localScale.y * 50f +
                                                                                    visualList[23].localScale.y * 50f +
                                                                                    visualList[24].localScale.y * 50f +
                                                                                    visualList[25].localScale.y * 50f) / 1000f;

        foreach (Transform visualObject in visualList)
        {
            visualObject.Rotate(0f, 0f, rotateIntencity * rotationDirection);
        }
    }

    private void VisualizeParticles()
    {
        float particlesSpeedIntencity = visualizerRotateIntencity +
                                                                     visualList[2].localScale.y * 5f +
                                                                     visualList[3].localScale.y * 5f +
                                                                     visualList[4].localScale.y * 5f;


        var particleSystemMain = _particleSystem.main;
        var particleSystemEmission = _particleSystem.emission;
        var particlesColorOverLifetime = _particleSystem.colorOverLifetime;

        if (visualizeByParticles)
        {
            particleSystemEmission.enabled = true;

            particleSystemMain.simulationSpeed = particlesSpeedIntencity / 30f;
            particleSystemMain.simulationSpeed = Mathf.Clamp(particleSystemMain.simulationSpeed, 1f, 100f);

            if (particlesRainbowColor)
            {
                particlesColorOverLifetime.enabled = true;
            }
            else
            {
                particlesColorOverLifetime.enabled = false;
            }
        }
        else
        {
            particleSystemEmission.enabled = false;
        }
    }

    private void ChangeNewTargetBackgroundPosition()
    {
        newTargetBackgroundPosition =
            new Vector3(Random.Range(defaultVisualizatorBackgroundPosition.x - 100f, defaultVisualizatorBackgroundPosition.x + 100f),
                        Random.Range(defaultVisualizatorBackgroundPosition.y - 100f, defaultVisualizatorBackgroundPosition.y + 100f),
                        Random.Range(defaultVisualizatorBackgroundPosition.z - 100f, defaultVisualizatorBackgroundPosition.z + 100f));
    }

    private void NullMaxBackgroundNewRotation()
    {
        maxBackgroundNewRotation = 1;
    }

    private void BackgroundSetShakeRotation()
    {
        float backgroundShakeForce = visualScale[4] + visualScale[5] + visualScale[6] + visualScale[7];
        // Assign new rotation to the background
        backgroundNewRotation = new Quaternion(0, 0, defaultVisualizatorBackgroundRotation.z - backgroundShakeForce, 0f);

        if (-backgroundNewRotation.z > maxBackgroundNewRotation)
        {
            maxBackgroundNewRotation = -backgroundNewRotation.z;
        }
    }

    private void AnimeBackgroundZomming()
    {
        newAnimeBackgroundPosition =
            new Vector3(defaultVisualizatorBackgroundPosition.x,
                        defaultVisualizatorBackgroundPosition.y - backgroundNewRotation.z * 5f,
                        defaultVisualizatorBackgroundPosition.z);
        VisualizatorAnimeBackground.transform.position = Vector3.Lerp(VisualizatorAnimeBackground.transform.position, newAnimeBackgroundPosition, 0.5f);
    }

    private void BackgroundShake()
    {
        // Move background
        VisualizatorBackground.transform.position = Vector3.Lerp(VisualizatorBackground.transform.position, newTargetBackgroundPosition, 0.01f);
        // Shake background
        // If new rotation is enough to shake the background
        if (backgroundNewRotation.z <= -100f)
        {
            // Set position target of background closer to camera
            newTargetBackgroundPosition =
                new Vector3(defaultVisualizatorBackgroundPosition.x,
                            defaultVisualizatorBackgroundPosition.y - backgroundNewRotation.z * 5f,
                            defaultVisualizatorBackgroundPosition.z);
            // Rotate the background to new rotation
            backgroundNewRotation.z *= 0.5f;
            VisualizatorBackground.transform.localRotation = Quaternion.Lerp(defaultVisualizatorBackgroundRotation, backgroundNewRotation, 0.001f);
        }
        else
        {
            // Set visualizer to default position
            VisualizatorBackground.transform.localRotation =
                Quaternion.Lerp(VisualizatorBackground.transform.localRotation, defaultVisualizatorBackgroundRotation, 0.05f);
        }

        SetBackgroundColor();
    }

    private void SetBackgroundColor()
    {
        VisualizatorBackground.GetComponent<Image>().color =
            new Color32(255, 255, 255, (byte)(255 + VisualizatorBackground.transform.localRotation.z * 1000f));
        if (VisualizatorBackground.GetComponent<Image>().color.a <= 0)
        {
            VisualizatorBackground.GetComponent<Image>().color =
            new Color32(255, 255, 255, 0);
        }

        for (int i = 0; i < VisualizatorBackground.transform.childCount; i++)
        {
            VisualizatorBackground.transform.GetChild(i).GetComponent<Image>().color =
                new Color32(255, 255, 255, (byte)(255 + VisualizatorBackground.transform.localRotation.z * 1000f));
        }
    }

    private void ChangeRotationDirection()
    {
        rotationDirection *= -1;
    }

    private void SetVisualsToDefaultPosition()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).transform.localPosition = visualsDefaultPositions[i];
        }
    }

    private void SetVisualsToDefaultScale()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localScale = visualCubeDefaultScale;
        }
    }
    
    private void CreateButtonBlocks()
    {
        for (int i = 0; i < visualScale.Length; i++)
        {
            if (i % 2 == 0 && visualScalesCanCreateButtonBlock[i / 2] && visualScale[i] > maxVisualScale * 0.75)
            {
                GameObject ButtonBlock = Instantiate(ButtonBlockPrefab,
                                                     new Vector3(ButtonsBlocksCanvas.transform.position.x + Random.Range(-100, 101),
                                                                 ButtonsBlocksCanvas.transform.position.y,
                                                                 ButtonsBlocksCanvas.transform.position.z),
                                                     ButtonsBlocksCanvas.transform.rotation,
                                                     ButtonsBlocksCanvas.transform);
                try
                {
                    ButtonBlock.GetComponent<ButtonBlock>().buttonName = alphabet[i / 2];
                }
                catch
                {
                    Destroy(ButtonBlock);
                    return;
                }
                ButtonBlock.GetComponent<ButtonBlock>().SetButtonText();

                visualScalesCanCreateButtonBlock[i / 2] = false;
                IEnumerator coroutine = SetTimeToCanCreateButtonBlock(i / 2, 5f);
                StartCoroutine(coroutine);
            }
        }
    }

    private IEnumerator SetTimeToCanCreateButtonBlock(int i, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        visualScalesCanCreateButtonBlock[i] = true;
    }

    private void AnalyzeSound()
    {
        audioSource.GetOutputData(samples, 0);

        // Получить RMS
        float sum = 0;
        for(int i = 0; i < sampleSize; i++)
        {
            sum = samples[i] * samples[i]; // samples^2
        }
        rmsValue = Mathf.Sqrt(sum / sampleSize);

        // Получить DB
        dbValue = 20 * Mathf.Log10(rmsValue / 0.1f);

        // Получить спектрум звука
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        // Получить высоту
        float maxV = 0;
        var maxN = 0;
        for(int i = 0; i < sampleSize; i++)
        {
            if(spectrum[i] <= maxV || spectrum[i] <= 0)
            {
                continue;
            }
            maxV = spectrum[i];
            maxN = i;
        }
        float freqN = maxN;
        if(maxN > 0 && maxN < sampleSize - 1)
        {
            var dL = spectrum[maxN - 1] / spectrum[maxN];
            var dR = spectrum[maxN + 1] / spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }
        pitchValue = freqN * (sampleRate / 2) / sampleSize;
    }

    private void VisualizeSound()
    {
        int visualIndex = 0;
        int spectrumIndex = 0;
        averageSize = (int)(sampleSize * keepPercentage / amountVisual);

        while (visualIndex < amountVisual)
        {
            float sum = 0;
            for(int i = 0; i < averageSize; i++)
            {
                sum += spectrum[spectrumIndex];
                spectrumIndex++;
            }
            float scaleY = sum / averageSize * visualModifier;
            visualScale[visualIndex] -= Time.deltaTime * smoothSpeed;

            if(visualScale[visualIndex] < scaleY)
            {
                visualScale[visualIndex] = scaleY;
            }

            visualScale[visualIndex] = Mathf.Clamp(visualScale[visualIndex], 0f, maxVisualScale - 2);

            if (visualizeByScale)
            {
                visualList[visualIndex].localScale = visualCubeDefaultScale + Vector3.up * visualScale[visualIndex];
            }
            else
            {
                visualList[visualIndex].localScale = visualCubeDefaultScale;
            }
            if (visualizeByPosition)
            {
                Vector3 direction = visualsDefaultPositions[visualIndex] - transform.position;
                float force = visualScale[visualIndex] * positionVisualizingIntencity * 0.0017f;
                
                if (Vector3.Distance(visualList[visualIndex].transform.localPosition, visualsDefaultPositions[visualIndex]) < maxPositionVisualizing)
                {
                    visualList[visualIndex].transform.localPosition = visualsDefaultPositions[visualIndex] + direction * force;
                }
                else
                {
                    visualList[visualIndex].transform.localPosition -= visualList[visualIndex].transform.localPosition / 10f;
                }
            }
            else
            {
                visualList[visualIndex].transform.localPosition = visualsDefaultPositions[visualIndex];
            }

            visualIndex++;
        }
    }
}