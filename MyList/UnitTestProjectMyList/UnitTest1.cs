using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using myListLibrary;
using System.Collections.Generic;

namespace UnitTestProjectMyList
{
    [TestClass]
    public class UnitTest1
    {
        public string MyListToString(myList<int> ml)
        {
            string s = "";
            foreach (int x in ml)
                s += x + " ";
            return s;
        }

        [TestMethod]
        public void Add_ElementsInList()
        {
            myList<int> ml = new myList<int>();
            for (int i = 0; i < 3; i++)
                ml.Add(i);
            string expected = "0 1 2 ";
            string actual = MyListToString(ml);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Replace_ElementsInList()
        {
            myList<int> ml = new myList<int>();
            for (int i = 0; i < 3; i++)
                ml.Add(i);        
            for (int i = 0; i < 3; i++)
                ml[i] = 2 - i;
            string expected = "2 1 0 ";
            string actual = MyListToString(ml);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Insert_ElementsInList()
        {
            myList<int> ml = new myList<int>();
            for (int i = 0; i < 3; i++)
                ml.Insert(i, i);
            string expected = "0 1 2 ";
            string actual = MyListToString(ml);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveAt_ElementsFromList()
        {
            myList<int> ml = new myList<int>();
            for (int i = 0; i < 5; i++)
                ml.Add(i);
            //0 1 2 3 4
            for (int i = 4; i >= 0; i -= 2)
                ml.RemoveAt(i);
            string expected = "1 3 ";
            string actual = MyListToString(ml);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Remove_ElementsFromList()
        {
            myList<int> ml = new myList<int>();
            for (int i = 0; i < 5; i++)
                ml.Add(4 - i);
            //4 3 2 1 0
            ml.Remove(3);
            ml.Remove(0);
            string expected = "4 2 1 ";
            string actual = MyListToString(ml);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Contains_ElementsInList()
        {
            myList<int> ml = new myList<int>();
            for (int i = 0; i < 3; i++)
                ml.Add(i);
            bool result_1 = ml.Contains(1);
            bool result_2 = ml.Contains(3);
            string expected = "True False";
            string actual = result_1 + " " + result_2;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IndexOf_ElementsInList()
        {
            myList<int> ml = new myList<int>();
            for (int i = 0; i < 3; i++)
                ml.Add(2 - i);
            //2 1 0
            int result_1 = ml.IndexOf(1);
            int result_2 = ml.IndexOf(2);
            string expected = "1 0";
            string actual = result_1 + " " + result_2;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CopyTo_ElementsInList()
        {
            myList<int> ml = new myList<int>();
            for (int i = 0; i < 3; i++)
                ml.Add(i);
            int[] array = new int[5];
            array[0] = 100;
            array[1] = 99;
            ml.CopyTo(array, 2);
            string expected = "100 99 0 1 2 ";
            string actual = "";
            foreach (int x in array)
                actual += x + " ";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Clear_ElementsInList()
        {
            myList<int> ml = new myList<int>();
            for (int i = 0; i < 3; i++)
                ml.Add(i);
            ml.Clear();
            string expected = "";
            string actual = MyListToString(ml);
            Assert.AreEqual(expected, actual);
        }
    }
}
