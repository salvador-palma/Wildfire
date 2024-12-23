using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [SerializeField] Transform BigPapa;
    public static ObjectPooling Instance { get; private set;}
    Dictionary<string, ObjectPool> Pools = new Dictionary<string, ObjectPool>();

    const int POOL_ADDITION_AMOUNT = 10;
    private void Awake() {
        if(Instance == null){
            Instance = this;
        }else{
            Destroy(gameObject);
        }
    }
    public static GameObject Spawn(IPoolable poolable, float[] args = null){
        string name = poolable.getReference();

        GameObject GO_poolable;
        if(Instance.Pools.ContainsKey(name)){
            GO_poolable =  Instance.Pools[name].getObject();
        }else{
            Instance.Pools.Add(name, new ObjectPool(poolable.gameObject));
            GO_poolable =  Instance.Pools[name].getObject();
        }

        if(args!=null){GO_poolable.GetComponent<IPoolable>().Define(args);}
        GO_poolable.GetComponent<IPoolable>().Pool();
        GO_poolable.SetActive(true);

        return GO_poolable;  
    }
    

    public static GameObject IncreasePool(ObjectPool pool){
        GameObject Prefab = pool.Prefab;
        for(int i = 0; i< POOL_ADDITION_AMOUNT; i++){
            if(pool.Parent==null){
                GameObject g = new GameObject(pool.Prefab.name);
                g.transform.parent = Instance.BigPapa;
                pool.Parent = g.transform;
            }
            pool.Instances.Add(Instantiate(Prefab, pool.Parent));
        }
        return pool.Instances.Last();
    }

}

public class ObjectPool{
    public GameObject Prefab;
    public Transform Parent;
    public List<GameObject> Instances = new List<GameObject>();

    public ObjectPool(GameObject Prefab){
        
        this.Prefab = Prefab;
        ObjectPooling.IncreasePool(this);
    }
    public GameObject getObject(){
        foreach(GameObject instance in Instances){
            if(!instance.activeInHierarchy){
                return instance;
            }
        }
        return ObjectPooling.IncreasePool(this);
    }
    
}

public abstract class IPoolable:MonoBehaviour{

    public abstract string getReference();
    public virtual void UnPool(){gameObject.SetActive(false);}
    public virtual void Pool(){}
    public virtual void Define(float[] args){}

    

}
