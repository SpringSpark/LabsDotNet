using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myListLibrary
{
    public class myList<T> : IList<T>
    {

        private int _count = 0;
        
        class MyNode<T>
        {
            public T Data;
            public MyNode<T> Next;
        }

        private MyNode<T> _root = null;
        
        public T this[int index]
        {
            get
            {
                if (index >= _count || index < 0)
                    throw new ArgumentOutOfRangeException();
                else
                {
                    MyNode<T> cur = _root;
                    for (int i = 0; i < index; i++)
                    {
                        cur = cur.Next;
                    }
                    return cur.Data;
                }
            }

            set
            {
                if (index >= _count || index < 0)
                    throw new ArgumentOutOfRangeException();
                else
                {
                    MyNode<T> cur = _root;
                    for (int i = 0; i < index && i < _count; i++)
                    {
                        cur = cur.Next;
                    }
                    cur.Data = value;
                }
            }
        }

        public int Count
        {
            get
            {
                return _count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(T item)
        {
            MyNode<T> newNode = new MyNode<T>();
            newNode.Data = item;
            if (_count == 0)
            {
                _root = newNode;
            }
            else
            {
                MyNode<T> cur = _root;
                for (int i = 0; i < _count - 1; i++)
                {
                    cur = cur.Next;
                }
                cur.Next = newNode;
            }
            _count++;
        }

        public void Clear()
        {
            _count = 0;
            _root = null;
        }

        public bool Contains(T item)
        {
            bool inList = false;
            MyNode<T> cur = _root;
            for (int i = 0; i < _count; i++)
            {
                if (cur.Data.Equals(item))
                {
                    inList = true;
                    break;
                }
                cur = cur.Next;
            }
            return inList;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            int j = arrayIndex;
            MyNode<T> cur = _root;
            for (int i = 0; i < _count; i++)
            {
                array.SetValue(cur.Data, j);
                cur = cur.Next;
                j++;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            MyNode<T> cur = _root;
            for (int i = 0; i < _count; i++)
            {
                yield return cur.Data;
                cur = cur.Next;
            }
        }

        public int IndexOf(T item)
        {
            int itemIndex = -1;
            MyNode<T> cur = _root;
            for (int i = 0; i < _count; i++)
            {
                if (cur.Data.Equals(item))
                {
                    itemIndex = i;
                    break;
                }
                cur = cur.Next;
            }
            return itemIndex;
        }

        public void Insert(int index, T item)
        {
            if (index > _count || index < 0)
                throw new ArgumentOutOfRangeException();
            else
            {
                MyNode<T> newNode = new MyNode<T>();
                newNode.Data = item;
                MyNode<T> cur = _root, prev = null;
                for(int i = 0; i < index; i++)
                {
                    prev = cur;
                    cur = cur.Next;
                }
                if(cur == _root)
                {
                    newNode.Next = _root;
                    _root = newNode;
                }
                else
                {
                    prev.Next = newNode;
                    newNode.Next = cur;
                }
                _count++;
            }
        }

        public bool Remove(T item)
        {
            bool removed = false;
            MyNode<T> cur = _root, prev = null;          
            for (int i = 0; i < _count; i++)
            {
                if (cur.Data.Equals(item))
                {
                    if (cur == _root)                    
                        _root = _root.Next;                    
                    else                    
                        prev.Next = cur.Next;                    
                    _count--;
                    removed = true;
                    break;
                }
                prev = cur;
                cur = cur.Next;
            }
            return removed;
        }

        public void RemoveAt(int index)
        {
            if (index >= _count || index < 0)
                throw new ArgumentOutOfRangeException();
            else
            {
                MyNode<T> cur = _root, prev = null;
                if (index == 0)
                    _root = _root.Next;
                else
                {
                    for (int i = 0; i < index; i++)
                    {
                        prev = cur;
                        cur = cur.Next;
                    }
                    prev.Next = cur.Next;
                }
                _count--;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
