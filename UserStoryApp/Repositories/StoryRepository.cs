using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Linq;
using UserStoryApp.Repositories.Interfaces;
using UserStoryApp.Models;

namespace UserStoryApp.Repositories
{
    public class StoryRepository : IStoryRepository
    {

        public Story GetAggregateById(int storyId)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var sql = "from Story s" +
                          " left join fetch s.Parent p" +
                          " left join fetch s.Children c" +
                          " where s.Id = :id";
                var story = session.CreateQuery(sql)
                    .SetInt32("id", storyId)
                    .UniqueResult<Story>();
                return story;
            }
        }

        public Story GetRootAggregate()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var sql = "from Story s" +
                          " left join fetch s.Children c" +
                          " where s.Parent is null";
                var story = session.CreateQuery(sql)
                    .UniqueResult<Story>();
                return story;

                //var query = from story in session.Query<Story>()
                //            where story.Parent.Id == null
                //            select story;
                //return query.Single();

            }
        }

        public ICollection<Story> GetAllLeafNodes()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var sql = "select s from Story s" +
                          " left join s.Parent p" +
                          " where s.Children.size = 0";
                var leafs = session.CreateQuery(sql)
                    .List<Story>();
                return leafs;
            }
        }

        public ICollection<Story> GetLeafNodes(int storyId)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var sql = "select Id, Narrative, ParentId, Priority, Estimate from Story s" +
                          " where s.ParentId = :id";
                var list = session.CreateSQLQuery(sql)
                    .AddEntity(typeof(Story))
                    .SetInt32("id", storyId)
                    .List<Story>();
                return list;
            }
        }

        public ICollection<Story> GetAllDescendantsOfStory(int storyId)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var sql = "with Hierachy(Id, Narrative, ParentId, Priority, Estimate, Level)" +
                          " as" +
                          " (" +
                          "   select Id, Narrative, ParentId, Priority, Estimate, 0 as Level" +
                          "   from Story s" +
                          "   where s.Id = :id" +
                          "  union all" +
                          "   select s.Id, s.Narrative, s.ParentId, s.Priority, s.Estimate, eh.Level + 1" +
                          "   from Story s" +
                          "   inner join Hierachy eh" +
                          "      on s.ParentId = eh.Id" +
                          " )" +
                          " select Id, Narrative, ParentId, Priority, Estimate" +
                          " from Hierachy" +
                          " where Level > 0";
                var list = session.CreateSQLQuery(sql)
                    .AddEntity(typeof(Story))
                    .SetInt32("id", storyId)
                    .List<Story>();
                return list;
            }
        }

        public ICollection<Story> GetAllAncestorsOfStory(int storyId)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var sql = "with Hierachy(Id, Narrative, ParentId, Priority, Estimate, Level)" +
                          " as" +
                          " (" +
                          "   select Id, Narrative, ParentId, Priority, Estimate, 0 as Level" +
                          "   from Story s" +
                          "   where s.Id = :id" +
                          "  union all" +
                          "   select s.Id, s.Narrative, s.ParentId, s.Priority, s.Estimate, eh.Level + 1" +
                          "   from Story s" +
                          "   inner join Hierachy eh" +
                          "      on s.Id = eh.ParentId" +
                          " )" +
                          " select Id, Narrative, ParentId, Priority, Estimate" +
                          " from Hierachy" +
                          " where Level > 0";
                var list = session.CreateSQLQuery(sql)
                    .AddEntity(typeof(Story))
                    .SetInt32("id", storyId)
                    .List<Story>();
                return list;
            }
        }

        public void Add(Story Story)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(Story);
                transaction.Commit();
            }
        }


        public Story GetById(int storyId)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var query = from story in session.Query<Story>()
                            where story.Id == storyId
                            select story;
                return query.Single();
            }
        }

        public ICollection<Story> GetByParentId(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var query = from story in session.Query<Story>()
                            where story.Parent.Id == id
                            select story;
                return query.ToList();
            }
        }

        public void Update(Story story)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(story);
                transaction.Commit();
            }
        }

        public void Delete(Story story)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(story);
                transaction.Commit();
            }
        }
    }
}