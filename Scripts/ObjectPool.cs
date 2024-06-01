using System.Collections.Generic;
using UnityEngine;

namespace bronya
{
    public class ObjectPool
    {
        //˽�о�̬����
        private static ObjectPool instance;
        private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();
        //��������һ��GameObject����֮Ϊpool
        private GameObject pool;
        //���о�̬����
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
                //Ѱ���Ƿ��Ѿ�����Ӧ�Ķ���ء�
                GameObject child = GameObject.Find(prefab.name + "Pool");
                //�����������һ��child��Ȼλ�յĻ�
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
            //���Ȼ�ȡ���Ѿ���ʱʹ����ϵ���Ʒ��
            string _name = prefab.name.Replace("(Clone)", string.Empty);  //ʵ��������Ʒ�����涼�����(Clone)��������������ʹ��string.Replace ��������ɿ��ַ���
            if (!objectPool.ContainsKey(_name))
            {
                objectPool.Add(_name, new Queue<GameObject>()); //Dictionary contains the function "Add".
            }
            objectPool[_name].Enqueue(prefab);
            prefab.SetActive(false);
        }
        public void DeactivateAllObjects()
        {
            // ����ÿ���������
            foreach (KeyValuePair<string, Queue<GameObject>> pair in objectPool)
            {
                // ���������е�ÿ������
                foreach (GameObject obj in pair.Value)
                {
                    // ����������Ϊ�Ǽ���״̬
                    obj.SetActive(false);
                    // �����������£����ڶ����Ѿ��ڶ����У��������ǲ���Ҫ��ô��
                }
            }
        }

    }

}
