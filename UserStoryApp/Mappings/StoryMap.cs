using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using UserStoryApp.Models;

namespace UserStoryApp.Mappings
{
    public class StoryMap : ClassMap<Story>
    {
        public StoryMap()
        {
            Id(x => x.Id)
                .GeneratedBy.Identity();
            Map(x => x.Narrative);
            Map(x => x.Priority);
            Map(x => x.Estimate);

            References(x => x.Parent)
                .Column("ParentId")
                .Not.LazyLoad();

            HasMany(x => x.Children)
                .KeyColumn("ParentId")
                .ForeignKeyConstraintName("fk_Story_ParentStory")
                .Not.LazyLoad()
                .Inverse();
        }
    }
}
