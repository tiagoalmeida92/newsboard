using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NewsBoard.Web.ViewModels
{
    [DataContract]
    public class FacebookPostsCrate
    {
        [DataMember(Name = "data")]
        public List<FacebookPostsViewModel> Data { get; set; }

        [DataMember(Name = "paging")]
        public FacebookPaging Paging { get; set; }
    }

    [DataContract]
    public class FacebookLikesCrate
    {
        [DataMember(Name = "data")]
        public List<FacebookLikesModel> Data { get; set; }

        [DataMember(Name = "paging")]
        public FacebookPaging Paging { get; set; }
    }


    [DataContract]
    public class FacebookPostsViewModel
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }

        [DataMember(Name = "from")]
        public FacebookBaseUser From { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "picture")]
        public string Picture { get; set; }

        [DataMember(Name = "link")]
        public string Link { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "caption")]
        public string Caption { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "icon")]
        public string Icon { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/M/yyyy - HH:mm}")]
        [DataMember(Name = "created_time")]
        public DateTime CreatedTime { get; set; }

        [DataMember(Name = "updated_time")]
        public string UpdatedTime { get; set; }
    }

    [DataContract]
    public class FacebookLikesModel
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "created_time")]
        public string CreatedTime { get; set; }

        [DataMember(Name = "category")]
        public string Category { get; set; }

        [DataMember(Name = "category_list")]
        public string CategoryList { get; set; }
    }

    [DataContract]
    public class FacebookBaseUser
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }


    [DataContract]
    public class FacebookPaging
    {
        [DataMember(Name = "previous")]
        public string Previous { get; set; }

        [DataMember(Name = "next")]
        public string Next { get; set; }
    }
}