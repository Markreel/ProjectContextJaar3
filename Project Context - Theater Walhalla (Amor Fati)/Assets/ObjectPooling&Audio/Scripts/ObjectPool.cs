using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PoolingAndAudio
{
    [System.Serializable]
    public class Pool
    {
        public string Key;
        public GameObject Prefab;
        public GameObject ParentObject;
        public int Size;
        public bool AllowExpand;
    }

    public class PooledObject
    {
        public GameObject GameObject;
        public IPooledObject IPooledObject;

        public PooledObject(GameObject _obj, IPooledObject _ipo)
        {
            GameObject = _obj;
            IPooledObject = _ipo;
        }
    }

    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private List<Pool> pools;
        private Dictionary<string, List<PooledObject>> poolDictionary;

        private GameManager gameManager;

        private void ExpandPool(Pool _pool)
        {
            foreach (var _pooledObjectQueue in poolDictionary)
            {
                if (_pooledObjectQueue.Key == _pool.Key)
                {
                    //Create GameObject
                    GameObject _obj = Instantiate(_pool.Prefab, _pool.ParentObject.transform);
                    _obj.SetActive(false);

                    //Check for IPooledObject interface and provide it with the corresponding key and the DespawnFunction if it is not equal to null
                    IPooledObject _ipo = _obj.GetComponent<IPooledObject>();
                    if (_ipo != null)
                    {
                        _ipo.Key = _pool.Key;
                        _ipo.SetUpOnDestruction(DespawnFromPool);
                    }

                    //Create a PooledObject from the GameObject and the IPooledObject interface
                    PooledObject _po = new PooledObject(_obj, _ipo);

                    //Store the PooledObject in the queue
                    _pooledObjectQueue.Value.Insert(0, _po);

                    //Add to the float "size" of the pool so it's the same as the actual size
                    _pool.Size++;
                }
            }
        }

        public void OnStart(GameManager _gm)
        {
            //Save the GameManager
            gameManager = _gm;

            //Create a new dictionary of PooledObject queues
            poolDictionary = new Dictionary<string, List<PooledObject>>();

            foreach (var _pool in pools)
            {
                //Create a new queue of PooledObjects
                List<PooledObject> _objectPool = new List<PooledObject>();

                for (int i = 0; i < _pool.Size; i++)
                {
                    //Create GameObject
                    GameObject _obj = Instantiate(_pool.Prefab, _pool.ParentObject.transform);
                    _obj.SetActive(false);

                    //Check for IPooledObject interface and provide it with the corresponding key and the DespawnFunction if it is not equal to null
                    IPooledObject _ipo = _obj.GetComponent<IPooledObject>();
                    if (_ipo != null)
                    {
                        _ipo.Key = _pool.Key;
                        _ipo.SetUpOnDestruction(DespawnFromPool);
                    }

                    //Create a PooledObject from the GameObject and the IPooledObject interface
                    PooledObject _po = new PooledObject(_obj, _ipo);

                    //Store the PooledObject in the queue
                    _objectPool.Add(_po);
                }

                //Add the pool to the dictionary
                poolDictionary.Add(_pool.Key, _objectPool);
            }
        }

        public PooledObject SpawnFromPool(string _key, Vector3 _position, Vector3 _eulerAngles, bool _local = false)
        {
            //Check if a pool exist with the given key
            if (!poolDictionary.ContainsKey(_key))
            {
                Debug.LogWarning($"Pool with key {_key} doesn't exist.");
                return null;
            }

            //Get the next pooled object in line and check if it is disabled
            PooledObject _po = null;
            for (int i = 0; i < poolDictionary[_key].Count; i++)
            {
                _po = poolDictionary[_key][i];

                if (!_po.GameObject.activeInHierarchy) { break; }
                else { _po = null; }
            }

            //Expand the object pool if a disabled pooled object is not found
            if (_po == null)
            {
                foreach (var _pool in pools)
                {
                    if (_pool.Key == _key)
                    {
                        if (!_pool.AllowExpand) { return null; }
                        ExpandPool(_pool);
                        break;
                    }
                }
            }

            //Repeat the previous process
            for (int i = 0; i < poolDictionary[_key].Count; i++)
            {
                _po = poolDictionary[_key][i];
                if (!_po.GameObject.activeInHierarchy) { break; }
                else { _po = null; }
            }

            //Fail Safe
            if (_po == null)
            {
                Debug.LogError("Something went wrong with resizing the pool: " + _key);
                return null;
            }

            //Set the local position and euler angles of the GameObject
            if (_local)
            {
                _po.GameObject.transform.localPosition = _position;
                _po.GameObject.transform.localEulerAngles = _eulerAngles;
            }
            //Set the world space position and euler angles of the GameObject
            else
            {
                _po.GameObject.transform.position = _position;
                _po.GameObject.transform.eulerAngles = _eulerAngles;
            }

            //Activate the GameObject and invoke the OnObjectSpawn function
            _po.GameObject.SetActive(true);
            _po.IPooledObject?.OnObjectSpawn();

            //Check if the pooled object is a managed game object and add the OnUpdate accordingly
            //IManagedGameObject _imgo = _obj.GetComponent<IManagedGameObject>();
            //if (_imgo != null) { gameManager.AddToOnUpdate(_imgo.OnUpdate); }

            ////Put the PooledObject back into the queue
            //poolDictionary[_key].Enqueue(_po);

            return _po;
        }

        public void DespawnFromPool(string _key, GameObject _obj)
        {
            //Check if a pool exist with the given key
            if (!poolDictionary.ContainsKey(_key))
            {
                Debug.LogWarning($"Pool with key {_key} doesn't exist.");
                return;
            }

            //Check for IPooledObject interface
            IPooledObject _ipo = _obj.GetComponent<IPooledObject>();

            //Invoke OnObjectDespawn and disable the GameObject afterwards
            _ipo.OnObjectDespawn();
            _obj.SetActive(false);

            return;
        }
    }
}