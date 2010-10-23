﻿// Copyright (c) 2007-2010 SlimDX Group
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SlimDX2.Tools.XIDL
{
    [DataContract]
    public class CppElement
    {
        private List<CppElement> _innerElements;

        [DataMember(Order = 0)]
        public string Name { get; set; }

        [DataMember(Order = 1)]
        public string Description { get; set; }

        [DataMember(Order = 1)]
        public string Remarks { get; set; }

        public CppElement Parent { get; set; }

        public object Tag { get; set; }

        public virtual string Path
        {
            get
            {
                if (Parent != null)
                    return Parent.FullName;
                return "";
            }
        }

        public virtual string FullName
        {
            get
            {
                string path = Path;
                string name = Name ?? "";
                return string.IsNullOrEmpty(path) ? name : path + "::" + name;
            }
        }

        /// <summary>
        ///   Add an inner element to this CppElement
        /// </summary>
        /// <param name = "element"></param>
        public void Add(CppElement element)
        {
            if (element.Parent != null)
                element.Parent.Remove(element);
            element.Parent = this;
            InnerElements.Add(element);
        }

        public void Add(IEnumerable<CppElement> elements)
        {
            foreach (var cppElement in elements)
                Add(cppElement);
        }

        /// <summary>
        ///   Remove an inner element to this CppElement
        /// </summary>
        /// <param name = "element"></param>
        public void Remove(CppElement element)
        {
            element.Parent = null;
            InnerElements.Remove(element);
        }


        /// <summary>
        ///   Return all inner elements
        /// </summary>
        public List<CppElement> InnerElements
        {
            get
            {
                if (_innerElements == null)
                    _innerElements = new List<CppElement>();
                return _innerElements;
            }
        }

        public IEnumerable<T> Find<T>(string regex) where T : CppElement
        {
            var cppElements = new List<T>();
            Find(regex, cppElements, null);
            return cppElements.OfType<T>();
        }

        public void Modify<T>(string regex, Modifiers.ProcessModifier modifier) where T : CppElement
        {
            var cppElements = new List<T>();
            Find(regex, cppElements, modifier);
        }

        public IEnumerable<CppElement> FindAll(string regex)
        {
            return Find<CppElement>(regex);
        }

        public void ModifyAll(string regex, Modifiers.ProcessModifier modifier)
        {
            Modify<CppElement>(regex, modifier);
        }

        private bool Find<T>(PathRegex regex, List<T> toAdd, Modifiers.ProcessModifier modifier) where T : CppElement
        {
            bool isToRemove = false;
            string path = FullName;

            if ((this is T) && path != null && regex.Match(path))
            {
                if (toAdd != null)
                    toAdd.Add((T) this);
                if (modifier != null)
                    isToRemove = modifier(regex, this);
            }

            List<CppElement> elementsToRemove = new List<CppElement>();
            foreach (var innerElement in InnerElements)
            {
                if (innerElement.Find(regex, toAdd, modifier))
                    elementsToRemove.Add(innerElement);
            }

            foreach (var innerElementToRemove in elementsToRemove)
                Remove(innerElementToRemove);

            return isToRemove;
        }

        public override string ToString()
        {
            return this.GetType().Name + " [" + Name + "]";
        }
    }
}