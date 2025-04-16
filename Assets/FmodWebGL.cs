using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
#if UNITY_ADDRESSABLES_EXIST
using UnityEngine.AddressableAssets;
#endif

class ScriptUsageLoading : MonoBehaviour
{
#if UNITY_ADDRESSABLES_EXIST
   // List of asset references to load if using Addressables
    public List<AssetReference> AssetReferenceBanks = new List<AssetReference>();
#else
    // List of Banks to load
    [FMODUnity.BankRef]
    public List<string> Banks = new List<string>();
#endif

    // The name of the scene to load and switch to
    public string Scene = null;

    public void Start()
    {
        StartCoroutine(LoadGameAsync());
    }

    void Update()
    {
        // Update the loading indication
    }

    IEnumerator LoadGameAsync()
    {
        // Start an asynchronous operation to load the scene
        AsyncOperation async = SceneManager.LoadSceneAsync(Scene);

        // Don't let the scene start until all Studio Banks have finished loading
        async.allowSceneActivation = false;

#if UNITY_ADDRESSABLES_EXIST
        // Iterate all the asset references and start loading their studio banks
        // in the background, including their audio sample data
        foreach (var bank in AssetReferenceBanks)
        {
            FMODUnity.RuntimeManager.LoadBank(bank, true);
        }
#else
        // Iterate all the Studio Banks and start them loading in the background
        // including the audio sample data
        foreach (var bank in Banks)
        {
            FMODUnity.RuntimeManager.LoadBank(bank, true);
        }
#endif

        // Keep yielding the co-routine until all the bank loading is done
        // (for platforms with asynchronous bank loading)
        while (!FMODUnity.RuntimeManager.HaveAllBanksLoaded)
        {
            yield return null;
        }

        // Keep yielding the co-routine until all the sample data loading is done
        while (FMODUnity.RuntimeManager.AnySampleDataLoading())
        {
            yield return null;
        }

        // Allow the scene to be activated. This means that any OnActivated() or Start()
        // methods will be guaranteed that all FMOD Studio loading will be completed and
        // there will be no delay in starting events
        async.allowSceneActivation = true;

        // Keep yielding the co-routine until scene loading and activation is done.
        while (!async.isDone)
        {
            yield return null;
        }
    }
}