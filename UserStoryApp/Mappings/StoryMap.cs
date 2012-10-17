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

            HasOne(x => x.Parent)
                .ForeignKey("ParentId");

            HasMany(x => x.Children)
                .KeyColumn("ParentId")
                .ForeignKeyConstraintName("fk_Story_ParentStory");
                
            //<many-to-one name="Parent" class="Equipment" column="ParentId" />

            //<set name="Children" cascade="all-delete-orphan" >
            //   <key column="ParentId" foreign-key="fk_Equipment_ParentEquipment"/>
            //   <one-to-many class="Equipment"/>
            // </set>

        }
    }
}
