using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;
    public List<AudioClip> mainMenuMusic;
    public List<AudioClip> gameMusic;
    public AudioSource currentMusic;
    [SerializeField] GameObject audioSourcePrefab;
    void Awake()
    {

        if (instance == null)

            instance = this;

        else if (instance != this)

            Destroy(gameObject);
       

    }

    public void ChangeMusic(Scene scene)
    {
        if (currentMusic == null)
        {
            currentMusic = new AudioSource();
            Instantiate(currentMusic);
        }
        if (scene.buildIndex == 0)
        {
            currentMusic.clip = mainMenuMusic[0];
        }
        else if (scene.buildIndex == 1)
        {
            currentMusic.clip = gameMusic[0];
        }
    }

    public void PlaySound(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return;


        GameObject pooledGO = ObjectPool.instance.GetObject(audioSourcePrefab);
        pooledGO.transform.position = position;

        AudioSource aSource = pooledGO.GetComponent<AudioSource>();
        aSource.clip = clip;
        aSource.volume = volume;
        aSource.Play();

        StartCoroutine(ReturnToPoolAfterPlay(pooledGO, clip.length));
    }

    private System.Collections.IEnumerator ReturnToPoolAfterPlay(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        ObjectPool.instance.ReturnObject(go);
    }
}
