using System.Collections.Generic;
using UnityEngine;

namespace bronya
{
    public class ObjectPool
    {
        //私有静态对象
        private static ObjectPool instance;
        private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();
        //单独整理一个GameObject，称之为pool
        private GameObject pool;
        //公有静态属性
        public static ObjectPool Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ObjectPool();
                }
                return instance;
            }
        }

        public GameObject GetObject(GameObject prefab)
        {
            GameObject _object;
            if (!objectPool.ContainsKey(prefab.name) || objectPool[prefab.name].Count == 0)
            {
                _object = GameObject.Instantiate(prefab);
                PushObject(_object);
                if (pool == null)
                {
                    pool = new GameObject("ObjectPool");
                }
                //寻找是否已经有相应的对象池。
                GameObject child = GameObject.Find(prefab.name + "Pool");
                //如果经过了上一步child仍然位空的话
                if (!child)
                {
                    child = new GameObject(prefab.name + "Pool");
                    child.transform.SetParent(pool.transform);
                }
                _object.transform.SetParent(child.transform);
            }
            _object = objectPool[prefab.name].Dequeue();
            _object.SetActive(true);
            return _object;
        }

        public void PushObject(GameObject prefab)
        {
            //首先获取到已经暂时使用完毕的物品名
            string _name = prefab.name.Replace("(Clone)", string.Empty);  //实例化的物品名后面都会加上(Clone)，所以我们首先使用string.Replace 把这个换成空字符串
            if (!objectPool.ContainsKey(_name))
            {
                objectPool.Add(_name, new Queue<GameObject>()); //Dictionary contains the function "Add".
            }
            objectPool[_name].Enqueue(prefab);
            prefab.SetActive(false);
        }
        public void DeactivateAllObjects()
        {
            // 遍历每个对象队列
            foreach (KeyValuePair<string, Queue<GameObject>> pair in objectPool)
            {
                // 遍历队列中的每个对象
                foreach (GameObject obj in pair.Value)
                {
                    // 将对象设置为非激活状态
                    obj.SetActive(false);
                    // 但在这个情况下，由于对象已经在队列中，所以我们不需要这么做
                }
            }
        }

    }

}
