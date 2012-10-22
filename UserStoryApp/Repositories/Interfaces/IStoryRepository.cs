using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserStoryApp.Models;

namespace UserStoryApp.Repositories.Interfaces
{
    public interface IStoryRepository
    {
        Story GetAggregateById(int storyId);
        Story GetRootAggregate();
        ICollection<Story> GetAllLeafNodes();
        ICollection<Story> GetLeafNodes(int storyId);
        ICollection<Story> GetAllDescendantsOfStory(int storyId);
        ICollection<Story> GetAllAncestorsOfStory(int storyId);
        ICollection<Story> GetByParentId(int id);
        void Add(Story story);
        Story GetById(int storyId);
        void Update(Story story);
        void Delete(Story story);
    }
}