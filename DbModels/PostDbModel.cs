namespace DbModels;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Models;

public class PostDbModel : Post
{

    [NotMapped]
    public override IUser Author { get => AuthorDbModel; set =>  new NotImplementedException(); } 

    [JsonIgnore]
    public int AuthorId { get; set; }
    [ForeignKey("AuthorId")]
    [JsonIgnore]
    public virtual UserDbModel AuthorDbModel { get; set; }

}