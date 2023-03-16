using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  sheepwalk
{
    public class CustomTags : MonoBehaviour
    {
        [SerializeField]
        private List<string> tags = new List<string>();
     
        public bool HasTag(string tag)
        {
            return tags.Contains(tag);
        }
     
        public IEnumerable<string> GetTags()
        {
            return tags;
        }
     
        public void Rename(int index, string tagName)
        {
            tags[index] = tagName;
        }
     
        public string GetAtIndex(int index)
        {
            return tags[index];
        }

        public bool Remove(string tagName)
        {
            return tags.Remove(tagName);
        }
        
        public void Add(string tagName)
        {
            if (tagName != null && !HasTag(tagName)) 
            {
                tags.Add(tagName);
            }
        }
     
        public int Count
        {
            get { return tags.Count; }
        }
    }   
}
