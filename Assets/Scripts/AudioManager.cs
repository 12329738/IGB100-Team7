using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;
    [SerializeField] GameObject audioSourcePrefab;
    void Awake()
    {

        if (instance == null)

            instance = this;

        else if (instance != this)

            Destroy(gameObject);

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
