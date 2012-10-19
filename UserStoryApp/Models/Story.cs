using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace UserStoryApp.Models
{
    public class Story
    {
        public virtual int Id { get; set; }
        [DataType(DataType.MultilineText)]
        public virtual String Narrative { get; set; }
        public virtual int Priority { get; set; }
        public virtual int Estimate { get; set; }
        public virtual Story Parent { get; set; }
        public virtual IList<Story> Children { get; set; }
                
        public Story()
        {
            Children = new List<Story>();
        }

        public virtual void AddChildStory(Story Story)
        {
            Children.Add(Story);
            Story.Parent = this;
        }

        public virtual Boolean HasChildren()
        {
            if (Children.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}